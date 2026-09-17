using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class Legal : BasePage
    {
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbDisclaimer;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbLicense;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbTrademarks;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbLiability;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbDisclaimer.ContentHtml = GetConfiguredContent("cbDisclaimer", @"<font color='#660000'>
                <b>THIS SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
                INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
                PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
                FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
                OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
                DEALINGS IN THE SOFTWARE.</b>
                </font>");

            cbLicense.ContentHtml = GetConfiguredContent("cbLicense", @"This template is provided for educational and personal use. 
                You are free to:
                <ul>
                    <li>Use this template for your own projects</li>
                    <li>Modify and customize the code to suit your needs</li>
                    <li>Distribute copies to others</li>
                </ul>
                <br />
                Please retain any copyright notices and attributions when using this template.");

            cbTrademarks.ContentHtml = GetConfiguredContent("cbTrademarks", @"Windows, Windows XP, and other Microsoft product names are trademarks or 
                registered trademarks of Microsoft Corporation in the United States and/or other countries.
                <br /><br />
                This template is not affiliated with, endorsed, sponsored, or specifically approved by Microsoft Corporation.
                <br /><br />
                All other trademarks mentioned are the property of their respective owners.");

            cbLiability.ContentHtml = GetConfiguredContent("cbLiability", @"The developers of this template shall not be held liable for any direct, 
                indirect, incidental, special, exemplary, or consequential damages arising from the use of this template.
                <br /><br />
                Users are responsible for ensuring their use of this template complies with applicable laws and regulations.
                <br /><br />
                <i>This template is provided for convenience and educational purposes. Use at your own risk.</i>");
        }
    }
}
