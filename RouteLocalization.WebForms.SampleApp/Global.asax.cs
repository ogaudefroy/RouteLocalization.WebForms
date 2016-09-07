namespace RouteLocalization.WebForms.SampleApp
{
    using System;
    using System.Web.Routing;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var routeCollection = new LocalizationRouteCollection("~/pages/offer/details.aspx");
            routeCollection.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");
            routeCollection.AddTranslation("en-US", "job/job-{title}_{id}");
            RouteTable.Routes.Add("OfferDetails", routeCollection);
        }
    }
}