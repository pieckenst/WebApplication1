using System.Web.UI;
using BRU.WEBFORMS.ASPNET.APP;

namespace BRU.WEBFORMS.ASPNET.APP.en
{
    public partial class FAQ : BasePage
    {
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbGeneral;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbCompatibility;
        protected global::BRU.WEBFORMS.ASPNET.APP.Controls.ContentBox cbUpdates;

        protected override void LoadPageContent()
        {
            cbGeneral.ContentHtml = @"
                <div class='faq-question'>What is this template for?</div>
                <div class='faq-answer'>This template is designed for creating Windows XP/2000 styled websites using ASP.NET Web Forms. It provides an authentic retro computing aesthetic.</div>
                
                <div class='faq-question'>Is this template free to use?</div>
                <div class='faq-answer'>Yes, this template is provided as a starting point for your own projects. Feel free to customize it to your needs.</div>
                
                <div class='faq-question'>What technologies does this template use?</div>
                <div class='faq-answer'>This template uses ASP.NET Web Forms with C# code-behind, master pages, user controls, and CSS for styling.</div>";

            cbCompatibility.ContentHtml = @"
                <div class='faq-question'>What browsers are supported?</div>
                <div class='faq-answer'>The template is designed to work with modern browsers while maintaining the classic Windows XP aesthetic. It uses standard HTML and CSS.</div>
                
                <div class='faq-question'>Does this work on mobile devices?</div>
                <div class='faq-answer'>Yes, the template includes responsive CSS that adapts to smaller screens while maintaining the retro look on desktop.</div>
                
                <div class='faq-question'>Can I customize the colors?</div>
                <div class='faq-answer'>Absolutely! All colors are defined in CSS and can be easily modified to match your preferred color scheme.</div>";

            cbUpdates.ContentHtml = @"
                <div class='faq-question'>How do I add new pages?</div>
                <div class='faq-answer'>Create a new Web Form that uses the Site.Master master page and inherits from BasePage. Add your content to the MainContent placeholder.</div>
                
                <div class='faq-question'>How do I change the logo?</div>
                <div class='faq-answer'>Edit SiteLogoUrl, SiteLogoWidth, SiteLogoHeight, and SiteLogoAlt in Web.config appSettings. No code changes required.</div>
                
                <div class='faq-question'>How do I override page content?</div>
                <div class='faq-answer'>Set ContentHtml on ContentBox controls in .aspx markup, or add Page.{PageKey}.{ControlId}.ContentHtml keys in Web.config.</div>";
        }
    }
}
