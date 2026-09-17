using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Business logic layer for Maintenance operations
    /// </summary>
    public class MaintenanceService : IDisposable
    {
        private MaintenanceRepository _repository;

        public MaintenanceService()
        {
            _repository = new MaintenanceRepository();
        }

        /// <summary>
        /// Get all maintenance records
        /// </summary>
        public List<Maintenance> GetAllMaintenance()
        {
            try
            {
                return _repository.GetAllMaintenance();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all maintenance records", ex);
            }
        }

        /// <summary>
        /// Get maintenance by bus ID
        /// </summary>
        public List<Maintenance> GetMaintenanceByBus(int busId)
        {
            try
            {
                return _repository.GetMaintenanceByBus(busId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving maintenance by bus", ex);
            }
        }

        /// <summary>
        /// Get latest maintenance for all buses
        /// </summary>
        public List<Maintenance> GetLatestMaintenance()
        {
            try
            {
                return _repository.GetLatestMaintenance();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving latest maintenance", ex);
            }
        }

        /// <summary>
        /// Create a new maintenance record with validation
        /// </summary>
        public int CreateMaintenance(Maintenance maintenance)
        {
            try
            {
                ValidateMaintenance(maintenance);
                return _repository.InsertMaintenance(maintenance);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating maintenance record", ex);
            }
        }

        /// <summary>
        /// Update an existing maintenance record with validation
        /// </summary>
        public bool UpdateMaintenance(Maintenance maintenance)
        {
            try
            {
                ValidateMaintenance(maintenance);
                
                // Check if maintenance record exists
                List<Maintenance> allMaintenance = _repository.GetAllMaintenance();
                bool exists = allMaintenance.Exists(m => m.MaintenanceId == maintenance.MaintenanceId);
                
                if (!exists)
                {
                    throw new ServiceException("Maintenance record not found");
                }

                return _repository.UpdateMaintenance(maintenance);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating maintenance record", ex);
            }
        }

        /// <summary>
        /// Delete a maintenance record
        /// </summary>
        public bool DeleteMaintenance(int maintenanceId)
        {
            try
            {
                List<Maintenance> allMaintenance = _repository.GetAllMaintenance();
                Maintenance maintenance = allMaintenance.Find(m => m.MaintenanceId == maintenanceId);
                
                if (maintenance == null)
                {
                    throw new ServiceException("Maintenance record not found");
                }

                return _repository.DeleteMaintenance(maintenanceId);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error deleting maintenance record", ex);
            }
        }

        /// <summary>
        /// Get autopark summary for dashboard
        /// </summary>
        public AutoparkSummary GetAutoparkSummary()
        {
            try
            {
                return _repository.GetAutoparkSummary();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving autopark summary", ex);
            }
        }

        /// <summary>
        /// Get sales by channel for analytics
        /// </summary>
        public List<SalesByChannel> GetSalesByChannel()
        {
            try
            {
                return _repository.GetSalesByChannel();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving sales by channel", ex);
            }
        }

        /// <summary>
        /// Get sales by route for analytics
        /// </summary>
        public List<SalesByRoute> GetSalesByRoute()
        {
            try
            {
                return _repository.GetSalesByRoute();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving sales by route", ex);
            }
        }

        /// <summary>
        /// Get sales by employee for analytics
        /// </summary>
        public List<SalesByEmployee> GetSalesByEmployee()
        {
            try
            {
                return _repository.GetSalesByEmployee();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving sales by employee", ex);
            }
        }

        /// <summary>
        /// Check if bus needs maintenance based on mileage
        /// </summary>
        public bool BusNeedsMaintenance(int busId, int currentMileage)
        {
            try
            {
                // Business rule: High mileage buses need maintenance
                if (currentMileage >= 250000)
                {
                    return true;
                }

                // Business rule: Check last maintenance date
                List<Maintenance> maintenanceHistory = _repository.GetMaintenanceByBus(busId);
                if (maintenanceHistory.Count > 0)
                {
                    Maintenance lastMaintenance = maintenanceHistory[0];
                    int daysSinceLastMaintenance = (DateTime.Today - lastMaintenance.MaintenanceDate).Days;
                    
                    // Business rule: Maintenance needed every 90 days
                    if (daysSinceLastMaintenance >= 90)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error checking maintenance needs", ex);
            }
        }

        /// <summary>
        /// Get buses that are overdue for maintenance
        /// </summary>
        public List<int> GetBusesOverdueForMaintenance()
        {
            try
            {
                List<int> overdueBusIds = new List<int>();
                List<Maintenance> latestMaintenance = _repository.GetLatestMaintenance();

                foreach (Maintenance maintenance in latestMaintenance)
                {
                    if (maintenance.NextMaintenanceDate.HasValue && 
                        maintenance.NextMaintenanceDate.Value < DateTime.Today)
                    {
                        overdueBusIds.Add(maintenance.BusId);
                    }
                }

                return overdueBusIds;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving overdue maintenance buses", ex);
            }
        }

        /// <summary>
        /// Validate maintenance data
        /// </summary>
        private void ValidateMaintenance(Maintenance maintenance)
        {
            if (maintenance == null)
            {
                throw new ServiceException("Maintenance object cannot be null");
            }

            if (maintenance.BusId <= 0)
            {
                throw new ServiceException("Valid bus ID is required");
            }

            if (string.IsNullOrEmpty(maintenance.MaintenanceType))
            {
                throw new ServiceException("Maintenance type is required");
            }

            if (maintenance.MaintenanceDate == default(DateTime))
            {
                throw new ServiceException("Maintenance date is required");
            }

            if (maintenance.MaintenanceDate > DateTime.Today)
            {
                throw new ServiceException("Maintenance date cannot be in the future");
            }

            if (maintenance.MaintenanceCost < 0)
            {
                throw new ServiceException("Maintenance cost cannot be negative");
            }

            if (maintenance.MileageKm.HasValue && maintenance.MileageKm.Value < 0)
            {
                throw new ServiceException("Mileage cannot be negative");
            }

            if (maintenance.NextMaintenanceDate.HasValue && 
                maintenance.NextMaintenanceDate.Value < maintenance.MaintenanceDate)
            {
                throw new ServiceException("Next maintenance date cannot be before maintenance date");
            }

            if (!IsValidRoadworthinessStatus(maintenance.Roadworthiness))
            {
                throw new ServiceException("Invalid roadworthiness status");
            }
        }

        /// <summary>
        /// Check if roadworthiness status is valid
        /// </summary>
        private bool IsValidRoadworthinessStatus(string status)
        {
            return status == RoadworthinessStatus.Operational ||
                   status == RoadworthinessStatus.NeedsAttention ||
                   status == RoadworthinessStatus.NotOperational;
        }

        public void Dispose()
        {
            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }
        }
    }
}