using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace BRU.WEBFORMS.ASPNET.APP
{
    public class Global : HttpApplication
    {
        private static readonly object DiagnosticLogLock = new object();

        void Application_Start(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            WriteDiagnostic("APP_START", "Application started. PID=" +
                System.Diagnostics.Process.GetCurrentProcess().Id);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server == null ? null : Server.GetLastError();
            HttpContext context = HttpContext.Current;
            string request = context == null || context.Request == null
                ? "(no HTTP request)"
                : context.Request.HttpMethod + " " + context.Request.RawUrl;
            WriteDiagnostic("APPLICATION_ERROR", request + Environment.NewLine +
                (error == null ? "(Server.GetLastError returned null)" : error.ToString()));
        }

        void Application_End(object sender, EventArgs e)
        {
            string reason = "unknown";
            try { reason = HostingEnvironment.ShutdownReason.ToString(); }
            catch (Exception ex) { reason += "; shutdown reason unavailable: " + ex.Message; }

            WriteDiagnostic("APP_END", "Application_End fired. Reason=" + reason +
                "; PID=" + System.Diagnostics.Process.GetCurrentProcess().Id);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            WriteDiagnostic("DOMAIN_UNHANDLED_EXCEPTION", "IsTerminating=" + e.IsTerminating +
                Environment.NewLine + (ex == null ? Convert.ToString(e.ExceptionObject) : ex.ToString()));
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            WriteDiagnostic("PROCESS_EXIT", "AppDomain ProcessExit event fired.");
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            WriteDiagnostic("DOMAIN_UNLOAD", "AppDomain DomainUnload event fired.");
        }

        private static void TaskScheduler_UnobservedTaskException(object sender,
            System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            WriteDiagnostic("UNOBSERVED_TASK_EXCEPTION", e.Exception.ToString());
        }

        public static void WriteDiagnostic(string category, string details)
        {
            string entry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                " [" + category + "] " + (details ?? string.Empty);
            System.Diagnostics.Trace.WriteLine(entry);
            System.Diagnostics.Debug.WriteLine(entry);

            try
            {
                string root = HttpRuntime.AppDomainAppPath;
                if (string.IsNullOrEmpty(root))
                    root = AppDomain.CurrentDomain.BaseDirectory;
                string directory = Path.Combine(root, "App_Data", "Logs");
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, "diagnostics.log");
                lock (DiagnosticLogLock)
                {
                    File.AppendAllText(path, entry + Environment.NewLine,
                        new UTF8Encoding(false));
                }
            }
            catch (Exception logException)
            {
                // Diagnostics must never cause an application failure.
                System.Diagnostics.Trace.WriteLine("Diagnostic file write failed: " + logException);
            }
        }
    }
}