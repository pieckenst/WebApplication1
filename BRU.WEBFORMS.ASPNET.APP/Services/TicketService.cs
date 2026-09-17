using System;
using System.Collections.Generic;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    /// <summary>
    /// Business logic layer for Ticket and Sale operations
    /// </summary>
    public class TicketService : IDisposable
    {
        private TicketRepository _repository;

        public TicketService()
        {
            _repository = new TicketRepository();
        }

        /// <summary>
        /// Get all tickets
        /// </summary>
        public List<Ticket> GetAllTickets()
        {
            try
            {
                return _repository.GetAllTickets();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all tickets", ex);
            }
        }

        /// <summary>
        /// Get ticket by ID
        /// </summary>
        public Ticket GetTicketById(int ticketId)
        {
            try
            {
                Ticket ticket = _repository.GetTicketById(ticketId);
                if (ticket == null)
                {
                    throw new ServiceException("Ticket not found");
                }
                return ticket;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving ticket", ex);
            }
        }

        /// <summary>
        /// Get tickets by price range
        /// </summary>
        public List<Ticket> GetTicketsByPriceRange(decimal priceFrom, decimal priceTo)
        {
            try
            {
                if (priceFrom < 0 || priceTo < 0)
                {
                    throw new ServiceException("Price values cannot be negative");
                }

                if (priceFrom > priceTo)
                {
                    throw new ServiceException("Price from cannot be greater than price to");
                }

                return _repository.GetTicketsByPriceRange(priceFrom, priceTo);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving tickets by price range", ex);
            }
        }

        /// <summary>
        /// Create a new ticket with validation
        /// </summary>
        public int CreateTicket(Ticket ticket)
        {
            try
            {
                ValidateTicket(ticket);
                return _repository.InsertTicket(ticket);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating ticket", ex);
            }
        }

        /// <summary>
        /// Update an existing ticket with validation
        /// </summary>
        public bool UpdateTicket(Ticket ticket)
        {
            try
            {
                ValidateTicket(ticket);
                
                // Check if ticket exists
                Ticket existingTicket = _repository.GetTicketById(ticket.TicketId);
                if (existingTicket == null)
                {
                    throw new ServiceException("Ticket not found");
                }

                return _repository.UpdateTicket(ticket);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error updating ticket", ex);
            }
        }

        /// <summary>
        /// Delete a ticket
        /// </summary>
        public bool DeleteTicket(int ticketId)
        {
            try
            {
                Ticket ticket = _repository.GetTicketById(ticketId);
                if (ticket == null)
                {
                    throw new ServiceException("Ticket not found");
                }

                // Business rule: Cannot delete active tickets with available stock
                if (ticket.IsActive && ticket.AvailableCount > 0)
                {
                    throw new ServiceException("Cannot delete active ticket with available stock. Set available count to 0 first.");
                }

                return _repository.DeleteTicket(ticketId);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error deleting ticket", ex);
            }
        }

        /// <summary>
        /// Get all sales
        /// </summary>
        public List<Sale> GetAllSales()
        {
            try
            {
                return _repository.GetAllSales();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all sales", ex);
            }
        }

        /// <summary>
        /// Get sales by date range
        /// </summary>
        public List<Sale> GetSalesByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                if (dateFrom > dateTo)
                {
                    throw new ServiceException("Start date cannot be after end date");
                }

                return _repository.GetSalesByDateRange(dateFrom, dateTo);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving sales by date range", ex);
            }
        }

        /// <summary>
        /// Get sales by channel
        /// </summary>
        public List<Sale> GetSalesByChannel(string saleChannel)
        {
            try
            {
                if (!IsValidSaleChannel(saleChannel))
                {
                    throw new ServiceException("Invalid sale channel");
                }

                return _repository.GetSalesByChannel(saleChannel);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving sales by channel", ex);
            }
        }

        /// <summary>
        /// Create a new sale with validation
        /// </summary>
        public long CreateSale(Sale sale)
        {
            try
            {
                ValidateSale(sale);
                return _repository.InsertSale(sale);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating sale", ex);
            }
        }

        /// <summary>
        /// Get all payments
        /// </summary>
        public List<Payment> GetAllPayments()
        {
            try
            {
                return _repository.GetAllPayments();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving all payments", ex);
            }
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        public long CreatePayment(Payment payment)
        {
            try
            {
                ValidatePayment(payment);
                return _repository.InsertPayment(payment);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error creating payment", ex);
            }
        }

        /// <summary>
        /// Get daily sales total
        /// </summary>
        public System.Data.DataTable GetDailySalesTotal(DateTime saleDate)
        {
            try
            {
                return _repository.GetDailySalesTotal(saleDate);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving daily sales total", ex);
            }
        }

        /// <summary>
        /// Validate ticket data
        /// </summary>
        private void ValidateTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ServiceException("Ticket object cannot be null");
            }

            if (string.IsNullOrEmpty(ticket.TicketName))
            {
                throw new ServiceException("Ticket name is required");
            }

            if (string.IsNullOrEmpty(ticket.TicketType))
            {
                throw new ServiceException("Ticket type is required");
            }

            if (ticket.Price < 0)
            {
                throw new ServiceException("Price cannot be negative");
            }

            if (ticket.ValidDays <= 0)
            {
                throw new ServiceException("Valid days must be greater than 0");
            }

            if (ticket.AvailableCount < 0)
            {
                throw new ServiceException("Available count cannot be negative");
            }

            if (ticket.ExpiryDate.HasValue && ticket.ExpiryDate.Value < DateTime.Today)
            {
                throw new ServiceException("Expiry date cannot be in the past");
            }
        }

        /// <summary>
        /// Validate sale data
        /// </summary>
        private void ValidateSale(Sale sale)
        {
            if (sale == null)
            {
                throw new ServiceException("Sale object cannot be null");
            }

            if (sale.TicketId <= 0)
            {
                throw new ServiceException("Valid ticket ID is required");
            }

            if (sale.SalePrice < 0)
            {
                throw new ServiceException("Sale price cannot be negative");
            }

            if (sale.TicketQuantity <= 0)
            {
                throw new ServiceException("Ticket quantity must be greater than 0");
            }

            if (!IsValidSaleChannel(sale.SaleChannel))
            {
                throw new ServiceException("Invalid sale channel");
            }

            if (!IsValidSaleStatus(sale.SaleStatus))
            {
                throw new ServiceException("Invalid sale status");
            }

            if (!IsValidPaymentStatus(sale.PaymentStatus))
            {
                throw new ServiceException("Invalid payment status");
            }
        }

        /// <summary>
        /// Validate payment data
        /// </summary>
        private void ValidatePayment(Payment payment)
        {
            if (payment == null)
            {
                throw new ServiceException("Payment object cannot be null");
            }

            if (payment.SaleId <= 0)
            {
                throw new ServiceException("Valid sale ID is required");
            }

            if (payment.Amount < 0)
            {
                throw new ServiceException("Payment amount cannot be negative");
            }

            if (string.IsNullOrEmpty(payment.PaymentMethod))
            {
                throw new ServiceException("Payment method is required");
            }

            if (!IsValidPaymentStatus(payment.PaymentStatus))
            {
                throw new ServiceException("Invalid payment status");
            }
        }

        /// <summary>
        /// Check if sale channel is valid
        /// </summary>
        private bool IsValidSaleChannel(string channel)
        {
            return channel == SaleChannel.CashDesk ||
                   channel == SaleChannel.Conductor ||
                   channel == SaleChannel.Validator ||
                   channel == SaleChannel.QR ||
                   channel == SaleChannel.Online;
        }

        /// <summary>
        /// Check if sale status is valid
        /// </summary>
        private bool IsValidSaleStatus(string status)
        {
            return status == SaleStatus.Created ||
                   status == SaleStatus.Completed ||
                   status == SaleStatus.Cancelled ||
                   status == SaleStatus.Refunded;
        }

        /// <summary>
        /// Check if payment status is valid
        /// </summary>
        private bool IsValidPaymentStatus(string status)
        {
            return status == PaymentStatus.Pending ||
                   status == PaymentStatus.Paid ||
                   status == PaymentStatus.Error ||
                   status == PaymentStatus.Refunded;
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