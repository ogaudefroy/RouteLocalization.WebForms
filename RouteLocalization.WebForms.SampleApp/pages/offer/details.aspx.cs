namespace RouteLocalization.WebForms.SampleApp.pages.offer
{
    using System.Web.UI;

    public partial class details : Page
    {
        protected override void InitializeCulture()
        {
            var culture = this.RouteData.Values["culture"] as string;
            if (!string.IsNullOrEmpty(culture))
            {
                this.Culture = culture;
                this.UICulture = culture;
            }
        }
    }
}