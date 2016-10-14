# RouteLocalization.WebForms
ASP.Net localized routing for WebForms routes

[![Build status](https://ci.appveyor.com/api/projects/status/tnob0qx1b8wdmwxj?svg=true)](https://ci.appveyor.com/project/ogaudefroy/routelocalization-webforms) [![NuGet version](https://badge.fury.io/nu/RouteLocalization.WebForms.svg)](https://badge.fury.io/nu/RouteLocalization.WebForms)

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
    var routeCulture = this.RouteData.Values["culture"] as string;
    if (routeCulture != null) {
        this.Culture = new CultureInfo(routeCulture);
        this.UICulture = new CultureInfo(routeCulture);
    }
}
```
## Outbound routing
```C#
// Classical way
RouteTable.Routes.GetVirtualPath(null, "OfferDetails", new RouteValueDictionary() {{"title", "chef-de-projet"}, {"id", 12}}).VirtualPath

// With explicit culture
RouteTable.Routes.GetVirtualPath(null, "OfferDetails", new RouteValueDictionary() {{"title", "chef-de-projet"}, {"id", 12}, {"id", "fr-FR"}).VirtualPath

// Through RouteUrlExpressionBuilder
<asp:HyperLink runat="server" NavigateUrl="<%$RouteUrl:title=chef-de-projet,id=12,routename=OfferDetails%>" Text="Details" /> 
```

Inspired by [RouteLocalization](https://github.com/Dresel/RouteLocalization)
