using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Enterprise-grade schedule automation service.
    /// Handles automatic status updates, recurring schedule generation,
    /// conflict detection, and schedule validation with full transaction support.
    /// </summary>
    public class ScheduleAutomationService : IDisposable
    {
        private readonly RouteRepository _routeRepository;
        private readonly BusRepository _busRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly MaintenanceRepository _maintenanceRepository;

        public ScheduleAutomationService()
        {
            _routeRepository = new RouteRepository();
            _busRepository = new BusRepository();
            _employeeRepository = new EmployeeRepository();
            _maintenanceRepository = new MaintenanceRepository();
        }

        public RouteSchedule GetScheduleById(int scheduleId)
        {
            RouteSchedule schedule = _routeRepository.GetScheduleById(scheduleId);
            if (schedule == null)
                throw new ServiceException("Schedule not found.");
            return schedule;
        }

        public int SaveSchedule(RouteSchedule schedule)
        {
            if (schedule == null)
                throw new ServiceException("Schedule details are required.");
            if (schedule.ServiceDate.Date < DateTime.Today)
                throw new ServiceException("Schedules cannot be created or changed for past dates.");
            if (schedule.DepartureTime < TimeSpan.Zero || schedule.ArrivalTime >= TimeSpan.FromDays(1) ||
                schedule.DepartureTime >= schedule.ArrivalTime)
                throw new ServiceException("Arrival time must be later than departure time on the same service day.");

            Route route = _routeRepository.GetRouteById(schedule.RouteId);
            if (route == null || !route.IsActive)
                throw new ServiceException("Select an active route.");

            Bus bus = _busRepository.GetBusById(schedule.BusId);
            if (bus == null || bus.Status != BusStatus.Operational)
                throw new ServiceException("Select an operational bus.");

            Employee driver = GetEligibleDrivers()
                .FirstOrDefault(item => item.EmployeeId == schedule.DriverId && item.Status == EmployeeStatus.Working);
            if (driver == null)
                throw new ServiceException("Select an active driver.");

            List<Maintenance> maintenance = _maintenanceRepository.GetMaintenanceByDateRange(
                schedule.ServiceDate.Date, schedule.ServiceDate.Date.AddDays(1));
            if (maintenance.Any(item => item.BusId == schedule.BusId))
                throw new ServiceException("The selected bus is scheduled for maintenance on this date.");

            RouteSchedule existing = null;
            if (schedule.ScheduleId > 0)
            {
                existing = GetScheduleById(schedule.ScheduleId);
                if (existing.ScheduleStatus != ScheduleStatus.Planned)
                    throw new ServiceException("Only planned schedules can be edited.");
                if (existing.ServiceDate.Date == DateTime.Today && existing.DepartureTime <= DateTime.Now.TimeOfDay)
                    throw new ServiceException("A trip cannot be edited after its departure time.");

                Bus previousBus = _busRepository.GetBusById(existing.BusId);
                int seatsSold = previousBus == null ? 0 : Math.Max(0, previousBus.Capacity - existing.AvailableSeatNum);
                if (seatsSold > bus.Capacity)
                    throw new ServiceException("The replacement bus does not have enough seats for tickets already sold.");
                schedule.AvailableSeatNum = Convert.ToInt16(bus.Capacity - seatsSold);
            }

            List<RouteSchedule> sameDay = _routeRepository.GetScheduleRange(
                schedule.ServiceDate.Date, schedule.ServiceDate.Date.AddDays(1));
            foreach (RouteSchedule other in sameDay)
            {
                if (other.ScheduleId == schedule.ScheduleId || other.ScheduleStatus == ScheduleStatus.Cancelled)
                    continue;
                if ((other.BusId == schedule.BusId || other.DriverId == schedule.DriverId) &&
                    TimesOverlap(schedule.DepartureTime, schedule.ArrivalTime, other.DepartureTime, other.ArrivalTime))
                {
                    string resource = other.BusId == schedule.BusId ? "bus" : "driver";
                    throw new ServiceException("The selected " + resource + " is already assigned to route " +
                        other.RouteNum + " from " + other.DepartureTime.ToString(@"hh\:mm") + " to " +
                        other.ArrivalTime.ToString(@"hh\:mm") + ".");
                }
            }

            schedule.ServiceDate = schedule.ServiceDate.Date;
            if (existing == null)
                schedule.AvailableSeatNum = bus.Capacity;
            schedule.ScheduleStatus = existing == null ? ScheduleStatus.Planned : existing.ScheduleStatus;
            return _routeRepository.SaveSchedule(schedule);
        }

        private List<Employee> GetEligibleDrivers()
        {
            List<Employee> drivers = _employeeRepository.GetEmployeesByJob(1);
            return drivers.Count > 0 ? drivers : _employeeRepository.GetEmployeesByJobTitle("Водитель автобуса");
        }

        #region Status Management

        /// <summary>
        /// Updates stored schedule statuses based on the current server time.
        /// Transitions: Planned → In Progress → Completed.
        /// Runs automatically when the authorized schedule page is loaded and
        /// may also be invoked explicitly by trusted backend code.
        /// </summary>
        public ScheduleStatusUpdateResult UpdateScheduleStatuses()
        {
            DateTime startedAt = DateTime.Now;
            ScheduleStatusUpdateResult result = new ScheduleStatusUpdateResult
            {
                ExecutionTime = startedAt
            };

            try
            {
                using (TransactionScope scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TimeSpan.FromMinutes(5)
                    }))
                {
                    DateTime now = DateTime.Now;
                    DateTime today = now.Date;
                    TimeSpan currentTime = now.TimeOfDay;
                    LogScheduleTrace("STATUS", "START scope=all pending/in-progress schedules through " +
                        today.ToString("yyyy-MM-dd") + "; future schedules are left unchanged; currentTime=" +
                        currentTime.ToString(@"hh\:mm\:ss"));

                    List<RouteSchedule> schedules = _routeRepository.GetSchedulesForStatusReconciliation(today);
                    LogScheduleTrace("STATUS", "QUERY_RESULT candidates=" + schedules.Count +
                        "; past and today's pending/in-progress schedules are included.");

                    if (schedules.Count == 0)
                    {
                        LogScheduleTrace("STATUS", "NO_CANDIDATES no past or today's schedules remain " +
                            "in a pending/in-progress status.");
                    }

                    foreach (RouteSchedule schedule in schedules)
                    {
                        string oldStatus = schedule.ScheduleStatus;
                        string newStatus = DetermineScheduleStatus(schedule, today, currentTime);
                        string reason = GetStatusDecisionReason(schedule, today, currentTime);

                        if (oldStatus != newStatus)
                        {
                            LogScheduleTrace("STATUS", "TRANSITION_ATTEMPT scheduleId=" + schedule.ScheduleId +
                                ", route=" + schedule.RouteNum + ", serviceDate=" +
                                schedule.ServiceDate.ToString("yyyy-MM-dd") + ", interval=" +
                                schedule.DepartureTime.ToString(@"hh\:mm") + "-" +
                                schedule.ArrivalTime.ToString(@"hh\:mm") + ", old=" + oldStatus +
                                ", new=" + newStatus + ", reason=" + reason);

                            if (!UpdateScheduleStatus(schedule, newStatus))
                            {
                                LogScheduleTrace("STATUS", "TRANSITION_SKIPPED scheduleId=" + schedule.ScheduleId +
                                    ", reason=concurrent schedule change or cancellation");
                                continue;
                            }

                            result.StatusTransitions.Add(new StatusTransition
                            {
                                ScheduleId = schedule.ScheduleId,
                                RouteNum = schedule.RouteNum,
                                OldStatus = oldStatus,
                                NewStatus = newStatus,
                                TransitionTime = now
                            });
                            LogScheduleTrace("STATUS", "TRANSITION_APPLIED scheduleId=" + schedule.ScheduleId +
                                ", old=" + oldStatus + ", new=" + newStatus);
                        }
                        else
                        {
                            LogScheduleTrace("STATUS", "NO_CHANGE scheduleId=" + schedule.ScheduleId +
                                ", route=" + schedule.RouteNum + ", serviceDate=" +
                                schedule.ServiceDate.ToString("yyyy-MM-dd") + ", status=" + oldStatus +
                                ", reason=" + reason);
                        }
                    }

                    result.Success = true;
                    result.TotalProcessed = schedules.Count;
                    result.TotalUpdated = result.StatusTransitions.Count;

                    LogScheduleTrace("STATUS", "TRANSACTION_COMPLETE_REQUESTED processed=" + result.TotalProcessed +
                        ", transitions=" + result.TotalUpdated);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;
                Trace.TraceError("[SCHEDULE][STATUS] RequestId:{0} FAILED after {1}ms: {2}",
                    GetScheduleRequestId(), (DateTime.Now - startedAt).TotalMilliseconds, ex);
            }

            LogScheduleTrace("STATUS", "FINISH success=" + result.Success + ", processed=" +
                result.TotalProcessed + ", transitions=" + result.TotalUpdated + ", elapsedMs=" +
                (DateTime.Now - startedAt).TotalMilliseconds.ToString("F0"));
            return result;
        }

        /// <summary>
        /// Determines the correct status for a schedule based on current time.
        /// Business rules:
        /// - Before departure: Planned
        /// - Between departure and arrival: In Progress
        /// - After arrival + 15 min buffer: Completed
        /// - Cancelled schedules remain cancelled
        /// </summary>
        private string DetermineScheduleStatus(RouteSchedule schedule, DateTime today, TimeSpan currentTime)
        {
            // Don't auto-update cancelled schedules
            if (schedule.ScheduleStatus == ScheduleStatus.Cancelled)
                return ScheduleStatus.Cancelled;

            if (schedule.ServiceDate.Date < today)
                return ScheduleStatus.Completed;
            if (schedule.ServiceDate.Date > today)
                return ScheduleStatus.Planned;

            TimeSpan departure = schedule.DepartureTime;
            TimeSpan arrival = schedule.ArrivalTime;
            TimeSpan completionBuffer = TimeSpan.FromMinutes(15);

            if (currentTime < departure)
            {
                return ScheduleStatus.Planned;
            }
            else if (currentTime >= departure && currentTime < arrival.Add(completionBuffer))
            {
                return ScheduleStatus.InProgress;
            }
            else
            {
                return ScheduleStatus.Completed;
            }
        }

        private static string GetStatusDecisionReason(RouteSchedule schedule, DateTime today, TimeSpan currentTime)
        {
            if (schedule.ScheduleStatus == ScheduleStatus.Cancelled)
                return "cancelled schedules are never auto-updated";
            if (schedule.ServiceDate.Date < today)
                return "service date is in the past; close as completed";
            if (schedule.ServiceDate.Date > today)
                return "future service date; keep planned";
            if (currentTime < schedule.DepartureTime)
                return "current time is before departure";
            if (currentTime < schedule.ArrivalTime.Add(TimeSpan.FromMinutes(15)))
                return "trip is underway or within the 15-minute completion buffer";
            return "arrival plus the 15-minute completion buffer has passed";
        }

        private static void LogScheduleTrace(string area, string message)
        {
            Trace.TraceInformation("[SCHEDULE][{0}] {1:yyyy-MM-dd HH:mm:ss.fff} RequestId:{2} {3}",
                area, DateTime.Now, GetScheduleRequestId(), message);
        }

        private static string GetScheduleRequestId()
        {
            HttpContext context = HttpContext.Current;
            return context == null || context.Items["RequestId"] == null
                ? "background"
                : context.Items["RequestId"].ToString();
        }

        /// <summary>
        /// Updates a schedule status only when the row still matches the snapshot
        /// used to calculate the new status.
        /// </summary>
        private bool UpdateScheduleStatus(RouteSchedule schedule, string newStatus)
        {
            return _routeRepository.UpdateScheduleStatus(
                schedule.ScheduleId,
                schedule.ScheduleStatus,
                schedule.ServiceDate,
                schedule.DepartureTime,
                schedule.ArrivalTime,
                newStatus);
        }

        #endregion

        #region Recurring Schedule Generation

        /// <summary>
        /// Generates schedules from a template. Past dates are inserted as completed historical trips.
        /// Implements full conflict detection and validation.
        /// </summary>
            /// <param name="generationDate">Target date to generate schedules for</param>
        /// <param name="sourceTemplateDate">Template date to copy schedules from</param>
        public ScheduleGenerationResult GenerateRecurringSchedules(DateTime generationDate, DateTime sourceTemplateDate)
        {
            ScheduleGenerationResult result = new ScheduleGenerationResult
            {
                TargetDate = generationDate,
                TemplateDate = sourceTemplateDate,
                ExecutionTime = DateTime.Now
            };

            try
            {
                using (TransactionScope scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.Serializable,
                        Timeout = TimeSpan.FromMinutes(10)
                    }))
                {
                    // Check if schedules already exist for target date
                    List<RouteSchedule> existing = _routeRepository.GetScheduleRange(generationDate, generationDate.AddDays(1));
                    if (existing != null && existing.Count > 0)
                    {
                        throw new ServiceException($"Schedules already exist for {generationDate:yyyy-MM-dd}. Delete them first if regeneration is needed.");
                    }

                    // Get template schedules
                    List<RouteSchedule> templates = _routeRepository.GetScheduleRange(sourceTemplateDate, sourceTemplateDate.AddDays(1));
                    if (templates == null || templates.Count == 0)
                    {
                        throw new ServiceException($"No template schedules found for {sourceTemplateDate:yyyy-MM-dd}");
                    }

                    bool isHistoricalBackfill = generationDate.Date < DateTime.Today;

                    // Historical entries may use resources that are now inactive.
                    List<Bus> availableBuses = _busRepository.GetAllBuses()
                        .Where(b => isHistoricalBackfill || b.Status == BusStatus.Operational)
                        .ToList();
                    
                    List<Employee> availableDrivers = GetEligibleDrivers()
                        .Where(e => isHistoricalBackfill || e.Status == EmployeeStatus.Working)
                        .ToList();

                    // Check maintenance conflicts
                    List<Maintenance> maintenanceSchedule = _maintenanceRepository.GetMaintenanceByDateRange(generationDate, generationDate.AddDays(1));
                    HashSet<int> busesUnderMaintenance = new HashSet<int>(
                        maintenanceSchedule
                            .Select(m => m.BusId)
                    );

                    // Generate new schedules with validation
                    foreach (RouteSchedule template in templates)
                    {
                        try
                        {
                            // Past schedules may refer to routes that have since been deactivated.
                            Route route = _routeRepository.GetRouteById(template.RouteId);
                            if (route == null || (!isHistoricalBackfill && !route.IsActive))
                            {
                                result.Skipped.Add(new ScheduleGenerationItem
                                {
                                    RouteNum = template.RouteNum,
                                    Reason = "Route is inactive or deleted"
                                });
                                continue;
                            }

                            List<Bus> busCandidates = availableBuses
                                .Where(candidate => !busesUnderMaintenance.Contains(candidate.BusId))
                                .OrderBy(candidate => candidate.BusId == template.BusId ? 0 : 1)
                                .ThenBy(candidate => candidate.FleetNumber)
                                .ToList();
                            List<Employee> driverCandidates = availableDrivers
                                .OrderBy(candidate => candidate.EmployeeId == template.DriverId ? 0 : 1)
                                .ThenBy(candidate => candidate.EmployeeName)
                                .ToList();
                            Bus selectedBus = null;
                            Employee selectedDriver = null;
                            List<string> lastConflicts = new List<string>();

                            foreach (Bus busCandidate in busCandidates)
                            {
                                foreach (Employee driverCandidate in driverCandidates)
                                {
                                    List<string> conflicts = DetectScheduleConflicts(
                                        existing, generationDate, template.DepartureTime, template.ArrivalTime,
                                        busCandidate.BusId, driverCandidate.EmployeeId, result.Created);
                                    if (conflicts.Count == 0)
                                    {
                                        selectedBus = busCandidate;
                                        selectedDriver = driverCandidate;
                                        break;
                                    }
                                    lastConflicts = conflicts;
                                }
                                if (selectedBus != null)
                                    break;
                            }

                            if (selectedBus == null || selectedDriver == null)
                            {
                                string reason = lastConflicts.Count > 0
                                    ? string.Join("; ", lastConflicts)
                                    : "No operational bus and active driver are available.";
                                result.Skipped.Add(new ScheduleGenerationItem
                                {
                                    RouteNum = template.RouteNum,
                                    Reason = reason
                                });
                                continue;
                            }

                            // Create new schedule
                            string generatedStatus = DetermineGeneratedScheduleStatus(
                                generationDate, template.DepartureTime, template.ArrivalTime);
                            int newScheduleId = InsertSchedule(
                                template.RouteId,
                                selectedBus.BusId,
                                selectedDriver.EmployeeId,
                                generationDate,
                                template.DepartureTime,
                                template.ArrivalTime,
                                selectedBus.Capacity,
                                generatedStatus
                            );

                            result.Created.Add(new ScheduleGenerationItem
                            {
                                ScheduleId = newScheduleId,
                                BusId = selectedBus.BusId,
                                DriverId = selectedDriver.EmployeeId,
                                RouteNum = template.RouteNum,
                                DepartureTime = template.DepartureTime,
                                ArrivalTime = template.ArrivalTime,
                                BusFleetNumber = selectedBus.FleetNumber,
                                DriverName = selectedDriver.EmployeeName
                            });
                            LogScheduleTrace("GENERATION", "TRIP_CREATED scheduleId=" + newScheduleId +
                                ", serviceDate=" + generationDate.ToString("yyyy-MM-dd") +
                                ", route=" + template.RouteNum + ", bus=" + selectedBus.FleetNumber +
                                ", driver=" + selectedDriver.EmployeeName + ", status=" + generatedStatus);
                        }
                        catch (Exception ex)
                        {
                            result.Failed.Add(new ScheduleGenerationItem
                            {
                                RouteNum = template.RouteNum,
                                Reason = $"Error: {ex.Message}"
                            });
                        }
                    }

                    result.Success = true;
                    result.TotalGenerated = result.Created.Count;
                    result.TotalSkipped = result.Skipped.Count;
                    result.TotalFailed = result.Failed.Count;

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;
            }

            return result;
        }

        /// <summary>
        /// Detects scheduling conflicts for bus and driver.
        /// Returns list of conflict descriptions.
        /// </summary>
        private List<string> DetectScheduleConflicts(
            List<RouteSchedule> existingSchedules,
            DateTime serviceDate,
            TimeSpan departureTime,
            TimeSpan arrivalTime,
            int busId,
            int driverId,
            List<ScheduleGenerationItem> pendingSchedules)
        {
            List<string> conflicts = new List<string>();

            // Check bus conflicts
            foreach (RouteSchedule existing in existingSchedules)
            {
                if (existing.BusId == busId && existing.ScheduleStatus != ScheduleStatus.Cancelled)
                {
                    if (TimesOverlap(departureTime, arrivalTime, existing.DepartureTime, existing.ArrivalTime))
                    {
                        conflicts.Add($"Bus conflict with route {existing.RouteNum} ({existing.DepartureTime:hh\\:mm}-{existing.ArrivalTime:hh\\:mm})");
                    }
                }
            }

            // Check driver conflicts
            foreach (RouteSchedule existing in existingSchedules)
            {
                if (existing.DriverId == driverId && existing.ScheduleStatus != ScheduleStatus.Cancelled)
                {
                    if (TimesOverlap(departureTime, arrivalTime, existing.DepartureTime, existing.ArrivalTime))
                    {
                        conflicts.Add($"Driver conflict with route {existing.RouteNum} ({existing.DepartureTime:hh\\:mm}-{existing.ArrivalTime:hh\\:mm})");
                    }
                }
            }

            // Check pending schedules (in current transaction)
            foreach (ScheduleGenerationItem pending in pendingSchedules)
            {
                if ((pending.BusId == busId || pending.DriverId == driverId) &&
                    TimesOverlap(departureTime, arrivalTime, pending.DepartureTime, pending.ArrivalTime))
                {
                    conflicts.Add($"Conflict with pending schedule for route {pending.RouteNum}");
                }
            }

            return conflicts;
        }

        /// <summary>
        /// Checks if two time ranges overlap.
        /// Includes buffer time for turnaround (15 minutes).
        /// </summary>
        private bool TimesOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
        {
            TimeSpan buffer = TimeSpan.FromMinutes(15);
            
            // Add buffer to both ranges
            TimeSpan bufferedEnd1 = end1.Add(buffer);
            TimeSpan bufferedEnd2 = end2.Add(buffer);

            // Check overlap
            return start1 < bufferedEnd2 && start2 < bufferedEnd1;
        }

        /// <summary>
        /// Inserts a new schedule into the database.
        /// </summary>
        private string DetermineGeneratedScheduleStatus(DateTime serviceDate, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            DateTime today = DateTime.Today;
            if (serviceDate.Date < today)
                return ScheduleStatus.Completed;
            if (serviceDate.Date > today)
                return ScheduleStatus.Planned;

            return DetermineScheduleStatus(new RouteSchedule
            {
                ServiceDate = serviceDate.Date,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                ScheduleStatus = ScheduleStatus.Planned
            }, today, DateTime.Now.TimeOfDay);
        }

        private int InsertSchedule(int routeId, int busId, int driverId, DateTime serviceDate, 
            TimeSpan departureTime, TimeSpan arrivalTime, int availableSeats, string status)
        {
            return _routeRepository.InsertSchedule(new RouteSchedule
            {
                RouteId = routeId,
                BusId = busId,
                DriverId = driverId,
                ServiceDate = serviceDate,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                AvailableSeatNum = Convert.ToInt16(availableSeats),
                ScheduleStatus = status
            });
        }

        #endregion

        #region Batch Schedule Generation

        /// <summary>
        /// Generates schedules for multiple days at once.
        /// Useful for bulk schedule creation (e.g., generate next month's schedules).
        /// </summary>
        public BatchScheduleGenerationResult GenerateBatchSchedules(
            DateTime startDate, 
            DateTime endDate, 
            DateTime templateDate,
            List<DayOfWeek> daysOfWeek = null)
        {
            BatchScheduleGenerationResult batchResult = new BatchScheduleGenerationResult
            {
                StartDate = startDate,
                EndDate = endDate,
                TemplateDate = templateDate,
                ExecutionTime = DateTime.Now
            };

            try
            {
                if (endDate.Date < startDate.Date)
                    throw new ServiceException("Generate through must be on or after Generate from.");
                if (endDate.Date.Subtract(startDate.Date).TotalDays >= 90)
                    throw new ServiceException("The selected date range cannot exceed 90 days.");

                // If no specific days specified, generate for all days
                if (daysOfWeek == null || daysOfWeek.Count == 0)
                {
                    daysOfWeek = new List<DayOfWeek> 
                    { 
                        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, 
                        DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday 
                    };
                }

                LogScheduleTrace("GENERATION", "START range=" + startDate.ToString("yyyy-MM-dd") +
                    ".." + endDate.ToString("yyyy-MM-dd") + ", historicalBackfill=" +
                    (startDate.Date < DateTime.Today) + ", template=" +
                    templateDate.ToString("yyyy-MM-dd") + ", weekdays=" +
                    string.Join(",", daysOfWeek));

                DateTime currentDate = startDate.Date;
                while (currentDate <= endDate.Date)
                {
                    if (daysOfWeek.Contains(currentDate.DayOfWeek))
                    {
                        ScheduleGenerationResult dayResult = GenerateRecurringSchedules(currentDate, templateDate);
                        batchResult.DailyResults.Add(dayResult);
                        LogScheduleTrace("GENERATION", "DATE_RESULT date=" + currentDate.ToString("yyyy-MM-dd") +
                            ", success=" + dayResult.Success + ", created=" + dayResult.TotalGenerated +
                            ", skipped=" + dayResult.TotalSkipped + ", failed=" + dayResult.TotalFailed +
                            ", error=" + (dayResult.ErrorMessage ?? "none"));

                        batchResult.TotalDaysProcessed++;
                        if (dayResult.Success)
                        {
                            batchResult.TotalDaysSucceeded++;
                            batchResult.TotalSchedulesCreated += dayResult.TotalGenerated;
                        }
                        else
                        {
                            batchResult.TotalDaysFailed++;
                        }
                    }

                    currentDate = currentDate.AddDays(1);
                }

                batchResult.Success = batchResult.TotalDaysFailed == 0;
                LogScheduleTrace("GENERATION", "FINISH success=" + batchResult.Success +
                    ", processedDays=" + batchResult.TotalDaysProcessed +
                    ", created=" + batchResult.TotalSchedulesCreated +
                    ", failedDays=" + batchResult.TotalDaysFailed);
            }
            catch (Exception ex)
            {
                batchResult.Success = false;
                batchResult.ErrorMessage = ex.Message;
                batchResult.Exception = ex;
                Trace.TraceError("[SCHEDULE][GENERATION] RequestId:{0} FAILED: {1}",
                    GetScheduleRequestId(), ex);
            }

            return batchResult;
        }

        #endregion

        #region Schedule Validation

        /// <summary>
        /// Validates schedule integrity across the entire system.
        /// Checks for orphaned records, invalid statuses, and data inconsistencies.
        /// </summary>
        public ScheduleValidationResult ValidateScheduleIntegrity(DateTime startDate, DateTime endDate)
        {
            DateTime startedAt = DateTime.Now;
            ScheduleValidationResult result = new ScheduleValidationResult
            {
                StartDate = startDate,
                EndDate = endDate,
                ExecutionTime = DateTime.Now
            };

            try
            {
                LogScheduleTrace("VALIDATION", "START range=" + startDate.ToString("yyyy-MM-dd") +
                    ".." + endDate.ToString("yyyy-MM-dd") + " (end exclusive)");
                List<RouteSchedule> schedules = _routeRepository.GetScheduleRange(startDate, endDate);
                LogScheduleTrace("VALIDATION", "QUERY_RESULT schedules=" + schedules.Count);

                foreach (RouteSchedule schedule in schedules)
                {
                    LogScheduleTrace("VALIDATION", "CHECK scheduleId=" + schedule.ScheduleId +
                        ", route=" + schedule.RouteNum + ", date=" +
                        schedule.ServiceDate.ToString("yyyy-MM-dd") + ", time=" +
                        schedule.DepartureTime.ToString(@"hh\:mm") + "-" +
                        schedule.ArrivalTime.ToString(@"hh\:mm") + ", busId=" + schedule.BusId +
                        ", driverId=" + schedule.DriverId + ", status=" + schedule.ScheduleStatus +
                        ", seats=" + schedule.AvailableSeats);

                    // Validate route exists and is active
                    Route route = _routeRepository.GetRouteById(schedule.RouteId);
                    if (route == null)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Critical,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Schedule references non-existent route ID {schedule.RouteId}"
                        });
                    }
                    else if (!route.IsActive && schedule.ScheduleStatus == ScheduleStatus.Planned)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Warning,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Planned schedule references inactive route {route.RouteNum}"
                        });
                    }

                    // Validate bus exists and status
                    Bus bus = _busRepository.GetBusById(schedule.BusId);
                    if (bus == null)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Critical,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Schedule references non-existent bus ID {schedule.BusId}"
                        });
                    }
                    else if (bus.Status != BusStatus.Operational && schedule.ScheduleStatus == ScheduleStatus.Planned)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Warning,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Planned schedule uses bus {bus.FleetNumber} with status '{bus.Status}'"
                        });
                    }

                    // Validate driver exists and status
                    Employee driver = _employeeRepository.GetEmployeeById(schedule.DriverId);
                    if (driver == null)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Critical,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Schedule references non-existent employee ID {schedule.DriverId}"
                        });
                    }
                    else if (driver.Status != EmployeeStatus.Working && schedule.ScheduleStatus == ScheduleStatus.Planned)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Warning,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Planned schedule uses driver with status '{driver.Status}'"
                        });
                    }

                    // Validate time logic
                    if (schedule.DepartureTime >= schedule.ArrivalTime)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            ScheduleId = schedule.ScheduleId,
                            Issue = "Departure time is not before arrival time"
                        });
                    }

                    // Validate seat availability
                    if (schedule.AvailableSeats < 0)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Negative seat availability: {schedule.AvailableSeats}"
                        });
                    }

                    if (bus != null && schedule.AvailableSeats > bus.Capacity)
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            ScheduleId = schedule.ScheduleId,
                            Issue = $"Available seats ({schedule.AvailableSeats}) exceeds bus capacity ({bus.Capacity})"
                        });
                    }
                }

                List<Maintenance> maintenanceRecords = _maintenanceRepository.GetMaintenanceByDateRange(startDate, endDate);
                LogScheduleTrace("VALIDATION", "MAINTENANCE_QUERY records=" + maintenanceRecords.Count);
                foreach (RouteSchedule schedule in schedules)
                {
                    if (schedule.ScheduleStatus == ScheduleStatus.Cancelled)
                        continue;
                    if (maintenanceRecords.Any(item => item.BusId == schedule.BusId &&
                        (item.MaintenanceDate.Date == schedule.ServiceDate.Date ||
                         (item.NextMaintenanceDate.HasValue && item.NextMaintenanceDate.Value.Date == schedule.ServiceDate.Date))))
                    {
                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Warning,
                            ScheduleId = schedule.ScheduleId,
                            Issue = "Bus has maintenance scheduled on the service date"
                        });
                    }
                }

                for (int firstIndex = 0; firstIndex < schedules.Count; firstIndex++)
                {
                    RouteSchedule first = schedules[firstIndex];
                    if (first.ScheduleStatus == ScheduleStatus.Cancelled) continue;
                    for (int secondIndex = firstIndex + 1; secondIndex < schedules.Count; secondIndex++)
                    {
                        RouteSchedule second = schedules[secondIndex];
                        if (second.ScheduleStatus == ScheduleStatus.Cancelled ||
                            first.ServiceDate.Date != second.ServiceDate.Date ||
                            (first.BusId != second.BusId && first.DriverId != second.DriverId) ||
                            !TimesOverlap(first.DepartureTime, first.ArrivalTime, second.DepartureTime, second.ArrivalTime))
                            continue;

                        result.Issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            ScheduleId = first.ScheduleId,
                            Issue = "Resource overlap with schedule " + second.ScheduleId + " (" +
                                (first.BusId == second.BusId ? "bus" : "driver") + ")"
                        });
                    }
                }

                result.TotalSchedulesValidated = schedules.Count;
                result.TotalIssuesFound = result.Issues.Count;
                result.Success = true;
                foreach (ValidationIssue issue in result.Issues)
                {
                    Trace.TraceWarning("[SCHEDULE][VALIDATION] RequestId:{0} ISSUE severity={1}, scheduleId={2}, message={3}",
                        GetScheduleRequestId(), issue.Severity, issue.ScheduleId, issue.Issue);
                }
                LogScheduleTrace("VALIDATION", "FINISH success=true, checked=" +
                    result.TotalSchedulesValidated + ", issues=" + result.TotalIssuesFound +
                    ", elapsedMs=" + (DateTime.Now - startedAt).TotalMilliseconds.ToString("F0"));
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;
                Trace.TraceError("[SCHEDULE][VALIDATION] RequestId:{0} FAILED after {1}ms: {2}",
                    GetScheduleRequestId(), (DateTime.Now - startedAt).TotalMilliseconds, ex);
            }

            return result;
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            _routeRepository?.Dispose();
            _busRepository?.Dispose();
            _employeeRepository?.Dispose();
            _maintenanceRepository?.Dispose();
        }

        #endregion
    }

    #region Result Classes

    /// <summary>
    /// Result of schedule status update operation
    /// </summary>
    public class ScheduleStatusUpdateResult
    {
        public bool Success { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int TotalProcessed { get; set; }
        public int TotalUpdated { get; set; }
        public List<StatusTransition> StatusTransitions { get; set; } = new List<StatusTransition>();
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }

    public class StatusTransition
    {
        public int ScheduleId { get; set; }
        public string RouteNum { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime TransitionTime { get; set; }
    }

    /// <summary>
    /// Result of schedule generation operation
    /// </summary>
    public class ScheduleGenerationResult
    {
        public bool Success { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime TemplateDate { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int TotalGenerated { get; set; }
        public int TotalSkipped { get; set; }
        public int TotalFailed { get; set; }
        public List<ScheduleGenerationItem> Created { get; set; } = new List<ScheduleGenerationItem>();
        public List<ScheduleGenerationItem> Skipped { get; set; } = new List<ScheduleGenerationItem>();
        public List<ScheduleGenerationItem> Failed { get; set; } = new List<ScheduleGenerationItem>();
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }

    public class ScheduleGenerationItem
    {
        public int ScheduleId { get; set; }
        public int BusId { get; set; }
        public int DriverId { get; set; }
        public string RouteNum { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string BusFleetNumber { get; set; }
        public string DriverName { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Result of batch schedule generation
    /// </summary>
    public class BatchScheduleGenerationResult
    {
        public bool Success { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime TemplateDate { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int TotalDaysProcessed { get; set; }
        public int TotalDaysSucceeded { get; set; }
        public int TotalDaysFailed { get; set; }
        public int TotalSchedulesCreated { get; set; }
        public List<ScheduleGenerationResult> DailyResults { get; set; } = new List<ScheduleGenerationResult>();
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// Result of schedule validation
    /// </summary>
    public class ScheduleValidationResult
    {
        public bool Success { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int TotalSchedulesValidated { get; set; }
        public int TotalIssuesFound { get; set; }
        public List<ValidationIssue> Issues { get; set; } = new List<ValidationIssue>();
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }

    public class ValidationIssue
    {
        public ValidationSeverity Severity { get; set; }
        public int ScheduleId { get; set; }
        public string Issue { get; set; }
    }

    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    #endregion
}
