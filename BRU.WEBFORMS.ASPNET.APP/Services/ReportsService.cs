using System;
using BRU.WEBFORMS.ASPNET.APP.DataAccess;
using BRU.WEBFORMS.ASPNET.APP.Models;

namespace BRU.WEBFORMS.ASPNET.APP.Services
{
    public sealed class ReportsService : IDisposable
    {
        private readonly ReportsRepository _repository = new ReportsRepository();

        public SalesReportData GetSalesReport(DateTime dateFrom, DateTime dateToExclusive, string channel, string status, int pageIndex, int pageSize)
        {
            ValidateRange(dateFrom, dateToExclusive);
            ValidatePage(pageIndex, pageSize);
            return _repository.GetSalesReport(dateFrom, dateToExclusive, channel, status, pageIndex, pageSize);
        }

        public MaintenanceReportData GetMaintenanceReport(DateTime dateFrom, DateTime dateToExclusive, int? busId,
            string roadworthiness, int pageIndex, int pageSize)
        {
            ValidateRange(dateFrom, dateToExclusive);
            ValidatePage(pageIndex, pageSize);
            return _repository.GetMaintenanceReport(dateFrom, dateToExclusive, busId, roadworthiness, pageIndex, pageSize);
        }

        private static void ValidateRange(DateTime dateFrom, DateTime dateToExclusive)
        {
            if (dateFrom.Date >= dateToExclusive.Date)
                throw new ServiceException("End date must be after start date.");
            if (dateToExclusive.Date > dateFrom.Date.AddYears(10))
                throw new ServiceException("Report date range cannot exceed ten years.");
        }

        private static void ValidatePage(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 1 || pageSize > 10000)
                throw new ServiceException("Invalid report page request.");
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
