using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using BRU.WEBFORMS.ASPNET.APP;
using BRU.WEBFORMS.ASPNET.APP.Models;
using BRU.WEBFORMS.ASPNET.APP.Services;

namespace BRU.WEBFORMS.ASPNET.APP.Operations
{
    public partial class Stops : SecurePage
    {
        private List<Stop> _stops;
        private bool _templateControlsResolved;

        protected override string[] RequiredPermissions { get { return new[] { "route.read" }; } }

        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.WebControls.Label lblSuccess;
        protected global::System.Web.UI.WebControls.Button btnNew;
        protected global::System.Web.UI.WebControls.TextBox txtSearch;
        protected global::System.Web.UI.WebControls.DropDownList ddlStatus;
        protected global::System.Web.UI.WebControls.Button btnApply;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbStopEditor;
        protected global::System.Web.UI.WebControls.Panel pnlEditor;
        protected global::System.Web.UI.WebControls.HiddenField hidStopId;
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.TextBox txtLocation;
        protected global::System.Web.UI.WebControls.TextBox txtLatitude;
        protected global::System.Web.UI.WebControls.TextBox txtLongitude;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnCancel;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbStopsList;
        protected global::System.Web.UI.WebControls.GridView gvStops;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureTemplateControlsResolved();
            bool canManage = CanManageStops;
            btnNew.Visible = canManage;
            cbStopEditor.Visible = canManage && pnlEditor.Visible;
            if (!IsPostBack)
                BindStops();
        }

        private void EnsureTemplateControlsResolved()
        {
            if (_templateControlsResolved) return;
            pnlEditor = cbStopEditor.FindContentControl<Panel>("pnlEditor");
            hidStopId = cbStopEditor.FindContentControl<HiddenField>("hidStopId");
            txtName = cbStopEditor.FindContentControl<TextBox>("txtName");
            txtLocation = cbStopEditor.FindContentControl<TextBox>("txtLocation");
            txtLatitude = cbStopEditor.FindContentControl<TextBox>("txtLatitude");
            txtLongitude = cbStopEditor.FindContentControl<TextBox>("txtLongitude");
            btnSave = cbStopEditor.FindContentControl<Button>("btnSave");
            btnCancel = cbStopEditor.FindContentControl<Button>("btnCancel");
            gvStops = cbStopsList.FindContentControl<GridView>("gvStops");
            if (pnlEditor == null || hidStopId == null || txtName == null || txtLocation == null ||
                txtLatitude == null || txtLongitude == null || btnSave == null || btnCancel == null || gvStops == null)
                throw new InvalidOperationException("Stop controls were not created inside their ContentBox templates.");
            _templateControlsResolved = true;
        }

        private bool CanManageStops
        {
            get { return AuthContext.HasPermission("route.write") || AuthContext.IsInRole("administrator"); }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                RequireStopWriteAccess();
                ClearEditor();
                pnlEditor.Visible = true;
                cbStopEditor.Visible = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            gvStops.PageIndex = 0;
            BindStops();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RequireStopWriteAccess();
                int stopId;
                if (!int.TryParse(hidStopId.Value, out stopId))
                    throw new ServiceException("Invalid stop selection.");
                Stop stop = ReadStop(stopId);
                using (RouteService service = new RouteService())
                {
                    if (stopId == 0) service.CreateStop(stop);
                    else service.UpdateStop(stop);
                }
                ClearEditor();
                pnlEditor.Visible = false;
                cbStopEditor.Visible = false;
                gvStops.PageIndex = 0;
                BindStops();
                ShowSuccess("Stop saved.");
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearEditor();
            pnlEditor.Visible = false;
            cbStopEditor.Visible = false;
        }

        protected void gvStops_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStops.PageIndex = e.NewPageIndex;
            BindStops();
        }

        protected void gvStops_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int stopId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out stopId))
                    throw new ServiceException("Invalid stop selection.");
                Stop stop = GetStops().FirstOrDefault(item => item.StopId == stopId);
                if (stop == null)
                    throw new ServiceException("Stop not found.");

                if (e.CommandName == "EditStop")
                {
                    RequireStopWriteAccess();
                    hidStopId.Value = stop.StopId.ToString(CultureInfo.InvariantCulture);
                    txtName.Text = stop.StopName;
                    txtLocation.Text = stop.Location;
                    txtLatitude.Text = stop.Latitude.HasValue ? stop.Latitude.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    txtLongitude.Text = stop.Longitude.HasValue ? stop.Longitude.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    pnlEditor.Visible = true;
                    cbStopEditor.Visible = true;
                }
                else if (e.CommandName == "ToggleStop")
                {
                    RequireStopWriteAccess();
                    using (RouteService service = new RouteService())
                        service.SetStopActive(stop.StopId, !stop.IsActive);
                    BindStops();
                    ShowSuccess(stop.IsActive ? "Stop deactivated. Route-stop associations were retained." : "Stop activated.");
                }
            }
            catch (ServiceException ex)
            {
                ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        protected void gvStops_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Stop stop = (Stop)e.Row.DataItem;
            LinkButton edit = e.Row.FindControl("btnEdit") as LinkButton;
            LinkButton toggle = e.Row.FindControl("btnToggle") as LinkButton;
            if (edit != null) edit.Visible = CanManageStops;
            if (toggle != null)
            {
                toggle.Visible = CanManageStops;
                toggle.Text = stop.IsActive ? "Deactivate" : "Activate";
            }
        }

        private Stop ReadStop(int stopId)
        {
            return new Stop
            {
                StopId = stopId,
                StopName = txtName.Text,
                Location = txtLocation.Text,
                Latitude = ParseCoordinate(txtLatitude.Text, "Latitude"),
                Longitude = ParseCoordinate(txtLongitude.Text, "Longitude")
            };
        }

        private static decimal? ParseCoordinate(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            decimal coordinate;
            if (!decimal.TryParse(value.Trim(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out coordinate) || decimal.Round(coordinate, 6) != coordinate)
                throw new ServiceException(fieldName + " must be a valid number with no more than six decimal places.");
            return coordinate;
        }

        private void RequireStopWriteAccess()
        {
            if (!CanManageStops)
                throw new UnauthorizedAccessException("You do not have permission to change stops.");
        }

        private List<Stop> GetStops()
        {
            if (_stops == null)
            {
                using (RouteService service = new RouteService())
                    _stops = service.GetStopsForManagement();
            }
            return _stops;
        }

        private void BindStops()
        {
            try
            {
                IEnumerable<Stop> stops = GetStops();
                string search = (txtSearch.Text ?? string.Empty).Trim();
                if (search.Length > 0)
                    stops = stops.Where(item => item.StopName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (item.Location ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                if (ddlStatus.SelectedValue == "1") stops = stops.Where(item => item.IsActive);
                else if (ddlStatus.SelectedValue == "0") stops = stops.Where(item => !item.IsActive);
                gvStops.DataSource = stops.ToList();
                gvStops.DataBind();
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        private void ClearEditor()
        {
            hidStopId.Value = "0";
            txtName.Text = string.Empty;
            txtLocation.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = Server.HtmlEncode(message);
            lblSuccess.Visible = true;
            lblError.Visible = false;
        }
    }
}
