using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.DataAccess
{
    /// <summary>
    /// Data access layer for Ticket and Sale operations
    /// </summary>
    public class TicketRepository : IDisposable
    {
        private DatabaseHelper _db;

        public TicketRepository()
        {
            _db = new DatabaseHelper();
        }

        /// <summary>
        /// Get all tickets
        /// </summary>
        public List<Ticket> GetAllTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.ticket WHERE is_active = 1 ORDER BY ticket_name"))
            {
                while (reader.Read())
                {
                    tickets.Add(MapTicketFromReader(reader));
                }
            }
            
            return tickets;
        }

        /// <summary>
        /// Get ticket by ID
        /// </summary>
        public Ticket GetTicketById(int ticketId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@ticket_id", ticketId, SqlDbType.Int)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "SELECT * FROM dbo.ticket WHERE ticket_id = @ticket_id", 
                parameters))
            {
                if (reader.Read())
                {
                    return MapTicketFromReader(reader);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get tickets by price range
        /// </summary>
        public List<Ticket> GetTicketsByPriceRange(decimal priceFrom, decimal priceTo)
        {
            List<Ticket> tickets = new List<Ticket>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@price_from", priceFrom, SqlDbType.Decimal),
                DatabaseHelper.CreateParameter("@price_to", priceTo, SqlDbType.Decimal)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_ticket_by_price_range @price_from, @price_to", 
                parameters))
            {
                while (reader.Read())
                {
                    tickets.Add(MapTicketFromReader(reader));
                }
            }
            
            return tickets;
        }

        /// <summary>
        /// Insert a new ticket
        /// </summary>
        public int InsertTicket(Ticket ticket)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@ticket_name", ticket.TicketName, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@ticket_type", ticket.TicketType, SqlDbType.NVarChar, 40),
                DatabaseHelper.CreateParameter("@zone", ticket.Zone ?? (object)DBNull.Value, SqlDbType.NVarChar, 40),
                DatabaseHelper.CreateParameter("@price", ticket.Price, SqlDbType.Decimal),
                DatabaseHelper.CreateParameter("@valid_days", ticket.ValidDays, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@available_count", ticket.AvailableCount, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@expiry_date", ticket.ExpiryDate ?? (object)DBNull.Value, SqlDbType.Date)
            };
            
            string sql = @"INSERT INTO dbo.ticket (ticket_name, ticket_type, zone, price, valid_days, available_count, expiry_date)
                          VALUES (@ticket_name, @ticket_type, @zone, @price, @valid_days, @available_count, @expiry_date);
                          SELECT CAST(SCOPE_IDENTITY() AS int)";
            
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Update an existing ticket
        /// </summary>
        public bool UpdateTicket(Ticket ticket)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@ticket_id", ticket.TicketId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@ticket_name", ticket.TicketName, SqlDbType.NVarChar, 120),
                DatabaseHelper.CreateParameter("@ticket_type", ticket.TicketType, SqlDbType.NVarChar, 40),
                DatabaseHelper.CreateParameter("@zone", ticket.Zone ?? (object)DBNull.Value, SqlDbType.NVarChar, 40),
                DatabaseHelper.CreateParameter("@price", ticket.Price, SqlDbType.Decimal),
                DatabaseHelper.CreateParameter("@valid_days", ticket.ValidDays, SqlDbType.SmallInt),
                DatabaseHelper.CreateParameter("@available_count", ticket.AvailableCount, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@expiry_date", ticket.ExpiryDate ?? (object)DBNull.Value, SqlDbType.Date),
                DatabaseHelper.CreateParameter("@is_active", ticket.IsActive, SqlDbType.Bit)
            };
            
            string sql = @"UPDATE dbo.ticket 
                          SET ticket_name = @ticket_name, 
                              ticket_type = @ticket_type, 
                              zone = @zone, 
                              price = @price, 
                              valid_days = @valid_days, 
                              available_count = @available_count,
                              expiry_date = @expiry_date,
                              is_active = @is_active
                          WHERE ticket_id = @ticket_id";
            
            int rowsAffected = _db.ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete a ticket
        /// </summary>
        public bool DeleteTicket(int ticketId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@ticket_id", ticketId, SqlDbType.Int)
            };
            
            int rowsAffected = _db.ExecuteNonQuery("DELETE FROM dbo.ticket WHERE ticket_id = @ticket_id", parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Get all sales
        /// </summary>
        public List<Sale> GetAllSales()
        {
            List<Sale> sales = new List<Sale>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_sale_details ORDER BY sale_date DESC"))
            {
                while (reader.Read())
                {
                    sales.Add(MapSaleFromReader(reader));
                }
            }
            
            return sales;
        }

        /// <summary>
        /// Get sales by date range
        /// </summary>
        public List<Sale> GetSalesByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            List<Sale> sales = new List<Sale>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@date_from", dateFrom, SqlDbType.DateTime2),
                DatabaseHelper.CreateParameter("@date_to", dateTo, SqlDbType.DateTime2)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_sales_by_date_range @date_from, @date_to", 
                parameters))
            {
                while (reader.Read())
                {
                    sales.Add(MapSaleFromReader(reader));
                }
            }
            
            return sales;
        }

        /// <summary>
        /// Get sales by channel
        /// </summary>
        public List<Sale> GetSalesByChannel(string saleChannel)
        {
            List<Sale> sales = new List<Sale>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@sale_channel", saleChannel, SqlDbType.NVarChar, 30)
            };
            
            using (SqlDataReader reader = _db.ExecuteReader(
                "EXEC dbo.get_sales_by_channel @sale_channel", 
                parameters))
            {
                while (reader.Read())
                {
                    sales.Add(MapSaleFromReader(reader));
                }
            }
            
            return sales;
        }

        /// <summary>
        /// Insert a new sale
        /// </summary>
        public long InsertSale(Sale sale)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@schedule_id", sale.ScheduleId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@ticket_id", sale.TicketId, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@cashier_id", sale.CashierId ?? (object)DBNull.Value, SqlDbType.Int),
                DatabaseHelper.CreateParameter("@sale_price", sale.SalePrice, SqlDbType.Decimal),
                DatabaseHelper.CreateParameter("@sale_channel", sale.SaleChannel, SqlDbType.NVarChar, 30),
                DatabaseHelper.CreateParameter("@ticket_quantity", sale.TicketQuantity, SqlDbType.Int),
                DatabaseHelper.CreateOutputParameter("@sale_id", SqlDbType.BigInt)
            };
            
            string sql = @"EXEC dbo.add_sale @schedule_id, @ticket_id, @cashier_id, @sale_price, @sale_channel, @ticket_quantity, @sale_id OUTPUT";
            
            _db.ExecuteNonQuery(sql, parameters);
            
            SqlParameter outputParam = Array.Find(parameters, p => p.ParameterName == "@sale_id");
            return Convert.ToInt64(outputParam.Value);
        }

        /// <summary>
        /// Get all payments
        /// </summary>
        public List<Payment> GetAllPayments()
        {
            List<Payment> payments = new List<Payment>();
            
            using (SqlDataReader reader = _db.ExecuteReader("SELECT * FROM dbo.v_payment_details ORDER BY payment_date DESC"))
            {
                while (reader.Read())
                {
                    payments.Add(MapPaymentFromReader(reader));
                }
            }
            
            return payments;
        }

        /// <summary>
        /// Insert a new payment
        /// </summary>
        public long InsertPayment(Payment payment)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@sale_id", payment.SaleId, SqlDbType.BigInt),
                DatabaseHelper.CreateParameter("@amount", payment.Amount, SqlDbType.Decimal),
                DatabaseHelper.CreateParameter("@payment_method", payment.PaymentMethod, SqlDbType.NVarChar, 40),
                DatabaseHelper.CreateParameter("@transaction_id", payment.TransactionId ?? (object)DBNull.Value, SqlDbType.VarChar, 80)
            };
            
            string sql = @"EXEC dbo.add_payment @sale_id, @amount, @payment_method, @transaction_id";
            object result = _db.ExecuteScalar(sql, parameters);
            return Convert.ToInt64(result);
        }

        /// <summary>
        /// Get daily sales total
        /// </summary>
        public DataTable GetDailySalesTotal(DateTime saleDate)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                DatabaseHelper.CreateParameter("@sale_date", saleDate, SqlDbType.Date)
            };
            
            return _db.ExecuteDataTable("EXEC dbo.get_daily_sales_total @sale_date", parameters);
        }

        /// <summary>
        /// Map SqlDataReader to Ticket object
        /// </summary>
        private Ticket MapTicketFromReader(SqlDataReader reader)
        {
            Ticket ticket = new Ticket();
            
            ticket.TicketId = reader.GetInt32(reader.GetOrdinal("ticket_id"));
            ticket.TicketName = reader.GetString(reader.GetOrdinal("ticket_name"));
            ticket.TicketType = reader.GetString(reader.GetOrdinal("ticket_type"));
            ticket.Zone = reader.IsDBNull(reader.GetOrdinal("zone")) ? null : reader.GetString(reader.GetOrdinal("zone"));
            ticket.Price = reader.GetDecimal(reader.GetOrdinal("price"));
            ticket.ValidDays = reader.GetInt16(reader.GetOrdinal("valid_days"));
            ticket.AvailableCount = reader.GetInt32(reader.GetOrdinal("available_count"));
            ticket.IssueDate = reader.GetDateTime(reader.GetOrdinal("issue_date"));
            ticket.ExpiryDate = reader.IsDBNull(reader.GetOrdinal("expiry_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("expiry_date"));
            ticket.IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
            
            // Optional field from procedure
            if (reader.FieldCount > 10 && !reader.IsDBNull(reader.GetOrdinal("period_value")))
            {
                ticket.PeriodValue = reader.GetDecimal(reader.GetOrdinal("period_value"));
            }
            
            return ticket;
        }

        /// <summary>
        /// Map SqlDataReader to Sale object
        /// </summary>
        private Sale MapSaleFromReader(SqlDataReader reader)
        {
            Sale sale = new Sale();
            
            sale.SaleId = reader.GetInt64(reader.GetOrdinal("sale_id"));
            sale.SaleDate = reader.GetDateTime(reader.GetOrdinal("sale_date"));
            sale.TicketName = reader.GetString(reader.GetOrdinal("ticket_name"));
            sale.TicketType = reader.GetString(reader.GetOrdinal("ticket_type"));
            sale.TicketQuantity = reader.GetInt32(reader.GetOrdinal("ticket_quantity"));
            sale.SalePrice = reader.GetDecimal(reader.GetOrdinal("sale_price"));
            sale.SaleTotal = reader.GetDecimal(reader.GetOrdinal("sale_total"));
            sale.SaleChannel = reader.GetString(reader.GetOrdinal("sale_channel"));
            sale.SaleStatus = reader.GetString(reader.GetOrdinal("sale_status"));
            sale.PaymentStatus = reader.GetString(reader.GetOrdinal("payment_status"));
            sale.CashierName = reader.IsDBNull(reader.GetOrdinal("cashier_name")) ? null : reader.GetString(reader.GetOrdinal("cashier_name"));
            
            return sale;
        }

        /// <summary>
        /// Map SqlDataReader to Payment object
        /// </summary>
        private Payment MapPaymentFromReader(SqlDataReader reader)
        {
            Payment payment = new Payment();
            
            payment.PaymentId = reader.GetInt64(reader.GetOrdinal("payment_id"));
            payment.SaleId = reader.GetInt64(reader.GetOrdinal("sale_id"));
            payment.PaymentDate = reader.GetDateTime(reader.GetOrdinal("payment_date"));
            payment.Amount = reader.GetDecimal(reader.GetOrdinal("amount"));
            payment.PaymentMethod = reader.GetString(reader.GetOrdinal("payment_method"));
            payment.PaymentStatus = reader.GetString(reader.GetOrdinal("payment_status"));
            payment.TransactionId = reader.IsDBNull(reader.GetOrdinal("transaction_id")) ? null : reader.GetString(reader.GetOrdinal("transaction_id"));
            payment.ControlStatus = reader.GetString(reader.GetOrdinal("control_status"));
            
            return payment;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
    }
}