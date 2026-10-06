using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Business logic layer for Bus operations
    /// </summary>
    public class BusService : IDisposable
    {
        private BusRepository _repository;

        public BusService()
        {
            _repository = new BusRepository();
        }

        /// <summary>
        /// Get all buses with status information
        /// </summary>
        public List<Bus> GetAllBuses()
        {
            try
            {
                return _repository.GetAllBuses();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all buses", ex);
            }
        }

        /// <summary>
        /// Get only operational buses
        /// </summary>
        public List<Bus> GetOperationalBuses()
        {
            try
            {
                return _repository.GetActiveBuses();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving operational buses", ex);
            }
        }

        /// <summary>
        /// Get bus by ID
        /// </summary>
        public Bus GetBusById(int busId)
        {
            try
            {
                Bus bus = _repository.GetBusById(busId);
                if (bus == null)
                {
                    throw new ServiceException("Bus not found");
                }
                return bus;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving bus", ex);
            }
        }

        /// <summary>
        /// Get bus by fleet number
        /// </summary>
        public Bus GetBusByFleetNumber(string fleetNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(fleetNumber))
                {
                    throw new ServiceException("Fleet number cannot be empty");
                }

                Bus bus = _repository.GetBusByFleetNumber(fleetNumber);
                if (bus == null)
                {
                    throw new ServiceException("Bus not found");
                }
                return bus;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving bus by fleet number", ex);
            }
        }

        /// <summary>
        /// Create a new bus with validation
        /// </summary>
        public int CreateBus(Bus bus)
        {
            try
            {
                ValidateBus(bus);
                return _repository.InsertBus(bus);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating bus", ex);
            }
        }

        /// <summary>
        /// Update an existing bus with validation
        /// </summary>
        public bool UpdateBus(Bus bus)
        {
            try
            {
                ValidateBus(bus);
                
                // Check if bus exists
                Bus existingBus = _repository.GetBusById(bus.BusId);
                if (existingBus == null)
                {
                    throw new ServiceException("Bus not found");
                }

                return _repository.UpdateBus(bus);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating bus", ex);
            }
        }

        /// <summary>
        /// Update bus status with business rules
        /// </summary>
        public bool UpdateBusStatus(int busId, string newStatus)
        {
            try
            {
                if (!IsValidBusStatus(newStatus))
                {
                    throw new ServiceException("Invalid bus status");
                }

                Bus bus = _repository.GetBusById(busId);
                if (bus == null)
                {
                    throw new ServiceException("Bus not found");
                }

                // Business rule: Cannot change retired bus to operational directly
                if (bus.Status == BusStatus.Retired && newStatus == BusStatus.Operational)
                {
                    throw new ServiceException("Cannot change retired bus directly to operational status");
                }

                return _repository.UpdateBusStatus(busId, newStatus);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating bus status", ex);
            }
        }

        /// <summary>
        /// Delete a bus
        /// </summary>
        public bool DeleteBus(int busId)
        {
            try
            {
                Bus bus = _repository.GetBusById(busId);
                if (bus == null)
                {
                    throw new ServiceException("Bus not found");
                }

                // Business rule: Cannot delete operational buses
                if (bus.Status == BusStatus.Operational)
                {
                    throw new ServiceException("Cannot delete operational bus. Change status to retired first.");
                }

                return _repository.DeleteBus(busId);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error deleting bus", ex);
            }
        }

        /// <summary>
        /// Get buses that need maintenance attention
        /// </summary>
        public List<Bus> GetBusesNeedingAttention()
        {
            try
            {
                List<Bus> allBuses = _repository.GetAllBuses();
                List<Bus> busesNeedingAttention = new List<Bus>();

                foreach (Bus bus in allBuses)
                {
                    // Business rule: High mileage buses need attention
                    if (bus.MileageCategory == MileageCategory.High)
                    {
                        busesNeedingAttention.Add(bus);
                    }
                    // Business rule: Buses in repair need attention
                    else if (bus.Status == BusStatus.InRepair)
                    {
                        busesNeedingAttention.Add(bus);
                    }
                }

                return busesNeedingAttention;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving buses needing attention", ex);
            }
        }

        /// <summary>
        /// Validate bus data
        /// </summary>
        private void ValidateBus(Bus bus)
        {
            if (bus == null)
            {
                throw new ServiceException("Bus object cannot be null");
            }

            if (string.IsNullOrEmpty(bus.FleetNumber))
            {
                throw new ServiceException("Fleet number is required");
            }

            if (string.IsNullOrEmpty(bus.RegistrationNum))
            {
                throw new ServiceException("Registration number is required");
            }

            if (string.IsNullOrEmpty(bus.Model))
            {
                throw new ServiceException("Model is required");
            }

            if (string.IsNullOrEmpty(bus.Manufacturer))
            {
                throw new ServiceException("Manufacturer is required");
            }

            if (bus.ManufactureYear < 1980 || bus.ManufactureYear > 2100)
            {
                throw new ServiceException("Manufacture year must be between 1980 and 2100");
            }

            if (bus.Capacity <= 0)
            {
                throw new ServiceException("Capacity must be greater than 0");
            }

            if (bus.MileageKm < 0)
            {
                throw new ServiceException("Mileage cannot be negative");
            }

            if (!IsValidBusStatus(bus.Status))
            {
                throw new ServiceException("Invalid bus status");
            }
        }

        /// <summary>
        /// Check if bus status is valid
        /// </summary>
        private bool IsValidBusStatus(string status)
        {
            return status == BusStatus.Operational ||
                   status == BusStatus.InRepair ||
                   status == BusStatus.Retired ||
                   status == BusStatus.Reserve;
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

    /// <summary>
    /// Custom exception for service layer errors
    /// </summary>
    public class ServiceException : Exception
    {
        public string LocalizedMessage { get; private set; }

        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }

        public ServiceException(string message, string localizedMessage) : base(message)
        {
            LocalizedMessage = localizedMessage;
        }

        public ServiceException(string message, string localizedMessage, Exception innerException)
            : base(message, innerException)
        {
            LocalizedMessage = localizedMessage;
        }
    }
}