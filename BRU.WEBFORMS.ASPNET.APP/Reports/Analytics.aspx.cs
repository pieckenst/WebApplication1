using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP.Services;
using BRU.WEBFORMS.ASPNET.APP.Models;
using System.Data.SqlClient;

namespace BRU.WEBFORMS.ASPNET.APP.Reports
{
    /// <summary>
    /// Business Analytics page for Autopark Management System.
    /// Provides a comprehensive dashboard with autopark KPIs, sales
    /// breakdowns by channel, route, and employee, with simple CSS
    /// bar chart visualizations and data tables.
    /// </summary>
    public partial class Analytics : System.Web.UI.Page
    {
        #region Control Declarations

        protected global::System.Web.UI.WebControls.Panel pnlError;
        protected global::System.Web.UI.WebControls.Literal litError;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Literal litSuccess;
        protected global::System.Web.UI.WebControls.Button btnRefresh;
        protected global::System.Web.UI.WebControls.Button btnExport;
        protected global::System.Web.UI.WebControls.Literal litActiveBuses;
        protected global::System.Web.UI.WebControls.Literal litActiveEmployees;
        protected global::System.Web.UI.WebControls.Literal litActiveRoutes;
        protected global::System.Web.UI.WebControls.Literal litTodaySchedules;
        protected global::System.Web.UI.WebControls.Literal litTodaySales;
        protected global::System.Web.UI.WebControls.Literal litTodayRevenue;
        protected global::System.Web.UI.WebControls.Literal litNeedAttention;
        protected global::System.Web.UI.WebControls.Literal litAvgSale;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesByChannel;
        protected global::System.Web.UI.WebControls.Literal litChannelChart;
        protected global::System.Web.UI.WebControls.GridView gvSalesByChannel;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesByRoute;
        protected global::System.Web.UI.WebControls.Literal litRouteChart;
        protected global::System.Web.UI.WebControls.GridView gvSalesByRoute;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbSalesByEmployee;
        protected global::System.Web.UI.WebControls.Literal litEmployeeChart;
        protected global::System.Web.UI.WebControls.GridView gvSalesByEmployee;

        #endregion

        #region Private Fields

        private decimal _maxChannelAmount = 1;
        private decimal _maxRouteAmount = 1;
        private decimal _maxEmployeeAmount = 1;

        #endregion

        #region Page Lifecycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadSummary();
                    LoadSalesByChannel();
                    LoadSalesByRoute();
                    LoadSalesByEmployee();
                    LoadAvgSale();
                }
            }
            catch (SqlException sqlEx)
            {
                HandleDatabaseError(sqlEx);
            }
            catch (ServiceException svcEx)
            {
                HandleServiceError(svcEx);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Data Loading

        private void LoadSummary()
        {
            using (MaintenanceService service = new MaintenanceService())
            {
                AutoparkSummary summary = service.GetAutoparkSummary();

                litActiveBuses.Text = summary.ActiveBusCount.ToString();
                litActiveEmployees.Text = summary.ActiveEmployeeCount.ToString();
                litActiveRoutes.Text = summary.ActiveRouteCount.ToString();
                litTodaySchedules.Text = summary.TodayScheduleCount.ToString();
                litTodaySales.Text = summary.TodaySaleCount.ToString();
                litTodayRevenue.Text = summary.TodayAmountTotal.ToString("F2");
                litNeedAttention.Text = summary.BusNeedAttentionCount.ToString();
            }
        }

        private void LoadAvgSale()
        {
            try
            {
                using (TicketService service = new TicketService())
                {
                    List<Sale> sales = service.GetAllSales();
                    if (sales == null || sales.Count == 0)
                    {
                        litAvgSale.Text = "0.00";
                        return;
                    }

                    int completedCount = 0;
                    decimal totalRevenue = 0;
                    foreach (Sale s in sales)
                    {
                        if (s.SaleStatus == SaleStatus.Completed)
                        {
                            completedCount++;
                            totalRevenue += s.SaleTotal;
                        }
                    }

                    litAvgSale.Text = completedCount > 0
                        ? (totalRevenue / completedCount).ToString("F2")
                        : "0.00";
                }
            }
            catch (Exception)
            {
                litAvgSale.Text = "0.00";
            }
        }

        private void LoadSalesByChannel()
        {
            List<SalesByChannel> data;
            using (MaintenanceService service = new MaintenanceService())
            {
                data = service.GetSalesByChannel();
            }

            if (data == null) data = new List<SalesByChannel>();

            // Find max for chart scaling
            _maxChannelAmount = 1;
            foreach (SalesByChannel item in data)
            {
                if (item.AmountTotal > _maxChannelAmount)
                    _maxChannelAmount = item.AmountTotal;
            }

            gvSalesByChannel.DataSource = data;
            gvSalesByChannel.DataBind();

            RenderChannelChart(data);
        }

        private void LoadSalesByRoute()
        {
            List<SalesByRoute> data;
            using (MaintenanceService service = new MaintenanceService())
            {
                data = service.GetSalesByRoute();
            }

            if (data == null) data = new List<SalesByRoute>();

            // Find max for chart scaling
            _maxRouteAmount = 1;
            foreach (SalesByRoute item in data)
            {
                if (item.AmountTotal > _maxRouteAmount)
                    _maxRouteAmount = item.AmountTotal;
            }

            gvSalesByRoute.DataSource = data;
            gvSalesByRoute.DataBind();

            RenderRouteChart(data);
        }

        private void LoadSalesByEmployee()
        {
            List<SalesByEmployee> data;
            using (MaintenanceService service = new MaintenanceService())
            {
                data = service.GetSalesByEmployee();
            }

            if (data == null) data = new List<SalesByEmployee>();

            // Find max for chart scaling
            _maxEmployeeAmount = 1;
            foreach (SalesByEmployee item in data)
            {
                if (item.AmountTotal > _maxEmployeeAmount)
                    _maxEmployeeAmount = item.AmountTotal;
            }

            gvSalesByEmployee.DataSource = data;
            gvSalesByEmployee.DataBind();

            RenderEmployeeChart(data);
        }

        #endregion

        #region Chart Rendering

        private void RenderChannelChart(List<SalesByChannel> data)
        {
            if (data == null || data.Count == 0)
            {
                litChannelChart.Text = "<div class='text-small'>No data available.</div>";
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='chart-bar-container'>");
            foreach (SalesByChannel item in data)
            {
                int pct = _maxChannelAmount > 0
                    ? (int)Math.Round((item.AmountTotal / _maxChannelAmount) * 100)
                    : 0;
                if (pct < 1) pct = 1;
                if (pct > 100) pct = 100;

                sb.Append("<div class='chart-bar-row'>");
                sb.Append("<span class='chart-bar-label'>").Append(Server.HtmlEncode(item.SaleChannel ?? "")).Append("</span>");
                sb.Append("<span class='chart-bar-track'>");
                sb.Append("<span class='chart-bar-fill' style='width:").Append(pct).Append("%'></span>");
                sb.Append("</span>");
                sb.Append("<span class='chart-bar-value'>").Append(item.AmountTotal.ToString("F2")).Append("</span>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            litChannelChart.Text = sb.ToString();
        }

        private void RenderRouteChart(List<SalesByRoute> data)
        {
            if (data == null || data.Count == 0)
            {
                litRouteChart.Text = "<div class='text-small'>No data available.</div>";
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='chart-bar-container'>");
            // Limit to top 10 for readability
            int maxItems = Math.Min(10, data.Count);
            for (int i = 0; i < maxItems; i++)
            {
                SalesByRoute item = data[i];
                int pct = _maxRouteAmount > 0
                    ? (int)Math.Round((item.AmountTotal / _maxRouteAmount) * 100)
                    : 0;
                if (pct < 1) pct = 1;
                if (pct > 100) pct = 100;

                string label = (item.RouteNum ?? "") + " - " + (item.RouteName ?? "");
                if (label.Length > 30) label = label.Substring(0, 27) + "...";

                sb.Append("<div class='chart-bar-row'>");
                sb.Append("<span class='chart-bar-label'>").Append(Server.HtmlEncode(label)).Append("</span>");
                sb.Append("<span class='chart-bar-track'>");
                sb.Append("<span class='chart-bar-fill' style='width:").Append(pct).Append("%'></span>");
                sb.Append("</span>");
                sb.Append("<span class='chart-bar-value'>").Append(item.AmountTotal.ToString("F2")).Append("</span>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            litRouteChart.Text = sb.ToString();
        }

        private void RenderEmployeeChart(List<SalesByEmployee> data)
        {
            if (data == null || data.Count == 0)
            {
                litEmployeeChart.Text = "<div class='text-small'>No data available.</div>";
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='chart-bar-container'>");
            // Limit to top 10 for readability
            int maxItems = Math.Min(10, data.Count);
            for (int i = 0; i < maxItems; i++)
            {
                SalesByEmployee item = data[i];
                int pct = _maxEmployeeAmount > 0
                    ? (int)Math.Round((item.AmountTotal / _maxEmployeeAmount) * 100)
                    : 0;
                if (pct < 1) pct = 1;
                if (pct > 100) pct = 100;

                string label = item.EmployeeName ?? "";
                if (label.Length > 30) label = label.Substring(0, 27) + "...";

                sb.Append("<div class='chart-bar-row'>");
                sb.Append("<span class='chart-bar-label'>").Append(Server.HtmlEncode(label)).Append("</span>");
                sb.Append("<span class='chart-bar-track'>");
                sb.Append("<span class='chart-bar-fill' style='width:").Append(pct).Append("%'></span>");
                sb.Append("</span>");
                sb.Append("<span class='chart-bar-value'>").Append(item.AmountTotal.ToString("F2")).Append("</span>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            litEmployeeChart.Text = sb.ToString();
        }

        #endregion

        #region Grid Events

        protected void gvSalesByChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SalesByChannel item = (SalesByChannel)e.Row.DataItem;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        protected void gvSalesByRoute_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SalesByRoute item = (SalesByRoute)e.Row.DataItem;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        protected void gvSalesByEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SalesByEmployee item = (SalesByEmployee)e.Row.DataItem;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != null && cell.Text == "&nbsp;")
                        cell.Text = "<span class='text-small'>—</span>";
                }
            }
        }

        #endregion

        #region Action Button Events

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSummary();
            LoadSalesByChannel();
            LoadSalesByRoute();
            LoadSalesByEmployee();
            LoadAvgSale();
            ShowSuccess("Analytics data refreshed successfully.");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition",
                    "attachment; filename=autopark_analytics_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");

                StringBuilder sb = new StringBuilder();
                sb.Append("AUTOPARK MANAGEMENT SYSTEM - ANALYTICS REPORT\r\n");
                sb.Append("Generated: ").Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")).Append("\r\n");
                sb.Append(new string('=', 60)).Append("\r\n\r\n");

                sb.Append("OVERVIEW:\r\n");
                sb.Append("  Active Buses:      ").Append(litActiveBuses.Text).Append("\r\n");
                sb.Append("  Active Employees:  ").Append(litActiveEmployees.Text).Append("\r\n");
                sb.Append("  Active Routes:     ").Append(litActiveRoutes.Text).Append("\r\n");
                sb.Append("  Today's Trips:     ").Append(litTodaySchedules.Text).Append("\r\n");
                sb.Append("  Today's Sales:    ").Append(litTodaySales.Text).Append("\r\n");
                sb.Append("  Today's Revenue:  ").Append(litTodayRevenue.Text).Append("\r\n");
                sb.Append("  Need Attention:   ").Append(litNeedAttention.Text).Append("\r\n");
                sb.Append("  Avg Sale:         ").Append(litAvgSale.Text).Append("\r\n\r\n");

                sb.Append("SALES BY CHANNEL:\r\n");
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<SalesByChannel> channels = service.GetSalesByChannel();
                    if (channels != null)
                    {
                        foreach (SalesByChannel c in channels)
                        {
                            sb.Append("  ").Append(c.SaleChannel ?? "").Append(": ");
                            sb.Append(c.SaleCount).Append(" sales, ");
                            sb.Append(c.TicketCount).Append(" tickets, ");
                            sb.Append(c.AmountTotal.ToString("F2")).Append(" revenue\r\n");
                        }
                    }
                }

                sb.Append("\r\nSALES BY ROUTE:\r\n");
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<SalesByRoute> routes = service.GetSalesByRoute();
                    if (routes != null)
                    {
                        foreach (SalesByRoute r in routes)
                        {
                            sb.Append("  ").Append(r.RouteNum ?? "").Append(" - ").Append(r.RouteName ?? "").Append(": ");
                            sb.Append(r.SaleCount).Append(" sales, ");
                            sb.Append(r.TicketCount).Append(" tickets, ");
                            sb.Append(r.AmountTotal.ToString("F2")).Append(" revenue\r\n");
                        }
                    }
                }

                sb.Append("\r\nSALES BY EMPLOYEE:\r\n");
                using (MaintenanceService service = new MaintenanceService())
                {
                    List<SalesByEmployee> employees = service.GetSalesByEmployee();
                    if (employees != null)
                    {
                        foreach (SalesByEmployee emp in employees)
                        {
                            sb.Append("  ").Append(emp.EmployeeName ?? "").Append(" (").Append(emp.JobTitle ?? "").Append("): ");
                            sb.Append(emp.SaleCount).Append(" sales, ");
                            sb.Append(emp.AmountTotal.ToString("F2")).Append(" revenue\r\n");
                        }
                    }
                }

                sb.Append("\r\n").Append(new string('=', 60)).Append("\r\n");
                sb.Append("End of Report\r\n");

                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        #endregion

        #region Error Handling

        private void HandleDatabaseError(SqlException sqlEx)
        {
            string errorMsg;
            switch (sqlEx.Number)
            {
                case 53: errorMsg = "Database server is not available. Please check your connection."; break;
                case 18456: errorMsg = "Database login failed. Please check credentials."; break;
                case 208: errorMsg = "Database table not found. Please ensure the schema is deployed."; break;
                default: errorMsg = "Database error (Code " + sqlEx.Number + "): " + sqlEx.Message; break;
            }
            ShowError(errorMsg);
            System.Diagnostics.Debug.WriteLine("Analytics SQL Error: " + sqlEx.ToString());
        }

        private void HandleServiceError(ServiceException svcEx)
        {
            string msg = svcEx.Message;
            if (svcEx.InnerException != null) msg += " (" + svcEx.InnerException.Message + ")";
            ShowError(msg);
            System.Diagnostics.Debug.WriteLine("Analytics Service Error: " + svcEx.ToString());
        }

        private void HandleGenericError(Exception ex)
        {
            ShowError("Unexpected error: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Analytics Generic Error: " + ex.ToString());
        }

        #endregion

        #region UI Helpers

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            pnlSuccess.Visible = false;
            litError.Text = Server.HtmlEncode(message);
        }

        private void ShowSuccess(string message)
        {
            pnlSuccess.Visible = true;
            pnlError.Visible = false;
            litSuccess.Text = Server.HtmlEncode(message);
        }

        #endregion
    }
}
