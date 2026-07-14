using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class Privacy : BasePage
    {
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbInfoCollect;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbInfoUse;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbDataSecurity;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbContact;

        protected override void LoadPageContent()
        {
            cbInfoCollect.ContentHtml = @"We collect minimal information necessary to provide our services:
                <ul>
                    <li><b>Usage Data:</b> Anonymous usage statistics to improve our services</li>
                    <li><b>Contact Information:</b> If you contact us, we may store your email for correspondence</li>
                    <li><b>Cookies:</b> Essential cookies for site functionality</li>
                </ul>";

            cbInfoUse.ContentHtml = @"The information we collect is used to:
                <ul>
                    <li>Provide and maintain our services</li>
                    <li>Improve user experience</li>
                    <li>Respond to inquiries and support requests</li>
                    <li>Send important notifications about our services</li>
                </ul>";

            cbDataSecurity.ContentHtml = @"We implement appropriate security measures to protect your information:
                <ul>
                    <li>SSL/TLS encryption for data transmission</li>
                    <li>Regular security assessments</li>
                    <li>Limited access to personal data</li>
                </ul>
                <br />
                <i>Note: No method of transmission over the Internet is 100% secure. We strive to protect your data but cannot guarantee absolute security.</i>";

            cbContact.ContentHtml = @"If you have questions about this privacy policy, please contact us:
                <br /><br />
                <b>Email:</b> <a href='mailto:privacy@example.com'>privacy@example.com</a>";
        }
    }
}
