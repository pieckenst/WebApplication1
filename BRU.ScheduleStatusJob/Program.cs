using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.ScheduleStatusJob
{
    internal static class Program
    {
        private static int Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener(true));
            Trace.AutoFlush = true;

            try
            {
                var connection = ConfigurationManager.ConnectionStrings["AutoparkDBConnection"];
                if (connection == null ||
                    !new SqlConnectionStringBuilder(connection.ConnectionString).IntegratedSecurity)
                    throw new ConfigurationErrorsException(
                        "Configure AutoparkDBConnection with Integrated Security for the authorized task account.");

                // SQL Server authorizes the Windows task identity. No web session
                // or page request is used to grant this background job access.
                using (var service = new ScheduleAutomationService())
                {
                    var result = service.UpdateScheduleStatuses();
                    Console.WriteLine("Schedule reconciliation: success={0}, processed={1}, updated={2}",
                        result.Success, result.TotalProcessed, result.TotalUpdated);
                    if (!result.Success)
                    {
                        Console.Error.WriteLine(result.ErrorMessage);
                        return 1;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Schedule reconciliation failed: " + ex.Message);
                return 1;
            }
            finally
            {
                Trace.Flush();
            }
        }
    }
}
