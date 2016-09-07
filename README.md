# RouteLocalization.WebForms
ASP.Net localized routing for WebForms routes

[![Build status](https://ci.appveyor.com/api/projects/status/5j77gyb7fsbh7obx/branch/master?svg=true)](https://ci.appveyor.com/project/ogaudefroy/routelocalization-webforms/branch/master)

## Declare a localized route
Just add a configured LocalizationRouteCollection in your route tables.
This component acts as a wrapper containing all localized routes (neutral included) and maps to a [PageRouteHandler](https://msdn.microsoft.com/en-us/library/system.web.routing.pageroutehandler(v=vs.110).aspx).
```C#
var routes = new LocalizationRouteCollection(
                virtualPath: "~/pages/offer/details.aspx",
                defaults: null,
                constraints: new RouteValueDictionary() { { "id", @"\d+" } }, });
route.AddTranslation(LocalizationRouteCollection.NeutralRoute, "neutral/neutral-{title}_{id}");
route.AddTranslation("en-US", "job/job-{title}_{id}");
route.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");
RouteTable.Routes.Add("OfferDetails", route);
```

## Retrieving route culture

Route culture can be easily retrieved as the culture is added in route values dictionary

```C#
protected override void InitializeCulture() {
    var routeCulture = this.RouteData.Values["culture"];
    if (routeCulture != null) {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(routeCulture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(routeCulture);
    }
}
```


Inspired by [RouteLocalization](https://github.com/Dresel/RouteLocalization)