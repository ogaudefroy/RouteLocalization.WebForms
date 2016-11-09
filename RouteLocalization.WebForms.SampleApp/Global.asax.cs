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

            var routeCollection2 = new LocalizationRouteCollection("~/pages/offer/list.aspx");
            routeCollection2.AddTranslation("fr-FR", "offre-de-emploi/liste-offre");
            routeCollection2.AddTranslation("en-US", "job/list-job");
            RouteTable.Routes.Add("OfferList", routeCollection2);
        }
    }
}