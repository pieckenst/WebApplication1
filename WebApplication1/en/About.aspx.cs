using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class About : BasePage
    {
        protected global::System.Web.UI.WebControls.Literal litLastUpdated;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbAbout;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbHowItWorks;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbHistory;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbContact;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbLegal;

        protected override void LoadPageContent()
        {
            LogPageContentLoad();

            cbAbout.ContentHtml = GetConfiguredContent("cbAbout", @"This project demonstrates how to create a Windows XP styled website using ASP.NET Web Forms. 
                The template provides a nostalgic look reminiscent of classic Windows XP/2000 era websites, bringing back the 
                authentic feel of early 2000s web design.");

            cbHowItWorks.ContentHtml = GetConfiguredContent("cbHowItWorks", @"The template uses a master page with frame-based layout simulation using CSS flexbox. 
                The navigation is handled through a configurable user control that allows easy customization of menu items.
                Content is displayed through reusable content box controls that maintain consistent styling.");

            cbHistory.ContentHtml = GetConfiguredContent("cbHistory", @"This template was created to help developers build retro-styled websites that capture 
                the essence of the Windows XP/2000 era. It's perfect for nostalgia projects, retro computing enthusiasts, 
                or anyone looking to create an authentic early 2000s web experience.");

            cbContact.ContentHtml = GetConfiguredContent("cbContact", @"If you would like to contact us for any reason, you can do so through the following channels:
                <br /><br />
                <b>Email:</b> <a href='mailto:contact@example.com'>contact@example.com</a>
                <br />
                <b>Website:</b> <a href='../Default.aspx'>Home Page</a>");

            cbLegal.ContentHtml = GetConfiguredContent("cbLegal", @"Learn more about how your data is used and the legal side of this website by visiting the 
                <a href='Privacy.aspx'>Privacy Policy</a> and <a href='Legal.aspx'>Legal Information</a> pages.");
        }
    }
}
