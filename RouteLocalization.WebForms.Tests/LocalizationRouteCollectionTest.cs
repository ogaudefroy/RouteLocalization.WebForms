namespace RouteLocalization.WebForms.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using NUnit.Framework;
    using WebFormsRouteUnitTester;

    [TestFixture]
    public class LocalizationRouteCollectionTest
    {
        private static LocalizationRouteCollection CreateLocalizedRoute()
        {
            return new LocalizationRouteCollection(
                virtualPath: "~/pages/offer/details.aspx",
                defaults: new RouteValueDictionary() { { "id", 0 } },
                constraints: new RouteValueDictionary() { { "id", @"\d+" } },
                dataTokens: new RouteValueDictionary() { { "tenant", "adatum" } });
        }

        [Test]
        public void LocalizationRouteCollection_Constructor_EnsureMappedValues()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(localizedRoute.LocalizedRoutes, Is.Empty);
            Assert.That(localizedRoute.NeutralRoute, Is.Null);

            Assert.That(localizedRoute.Defaults, Is.Not.Null);
            Assert.That(localizedRoute.Defaults.Count, Is.EqualTo(1));
            Assert.That(localizedRoute.Defaults["id"], Is.EqualTo(0));

            Assert.That(localizedRoute.Constraints, Is.Not.Null);
            Assert.That(localizedRoute.Constraints.Count, Is.EqualTo(1));
            Assert.That(localizedRoute.Constraints["id"], Is.EqualTo(@"\d+"));

            Assert.That(localizedRoute.DataTokens, Is.Not.Null);
            Assert.That(localizedRoute.DataTokens.Count, Is.EqualTo(1));
            Assert.That(localizedRoute.DataTokens["tenant"], Is.EqualTo("adatum"));

            Assert.That(localizedRoute.VirtualPath, Is.EqualTo("~/pages/offer/details.aspx"));
            Assert.That(localizedRoute.RouteHandler, Is.InstanceOf<PageRouteHandler>());
            Assert.That(((PageRouteHandler)localizedRoute.RouteHandler).CheckPhysicalUrlAccess, Is.True);
            Assert.That(((PageRouteHandler)localizedRoute.RouteHandler).VirtualPath, Is.EqualTo("~/pages/offer/details.aspx"));
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslations_NullValues_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.AddTranslations(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslation_NullOrEmtpyCulture_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.AddTranslation(null, "/test"), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslation_NullOrEmtpyUrl_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.AddTranslation("en-US", null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslations_AddsLocalizedRoute()
        {
            var localizedRoute = CreateLocalizedRoute();

            var translations = new Dictionary<string, string>()
            {
                { "en-US", "job/job-{title}_{id}" },
                { "fr-FR",  "offre-de-emploi/offre-{title}_{id}"}
            };
            localizedRoute.AddTranslations(translations);

            Assert.That(localizedRoute.LocalizedRoutes.Count, Is.EqualTo(2));
            Assert.That(localizedRoute.HasTranslationForCulture("nl-NL"), Is.False);
            Assert.That(localizedRoute.HasTranslationForCulture("fr-FR"), Is.True);
            Assert.That(localizedRoute.HasTranslationForCulture("en-US"), Is.True);
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslation_AddsLocalizedRoute()
        {
            var localizedRoute = CreateLocalizedRoute();
            localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}");
            localizedRoute.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");

            Assert.That(localizedRoute.LocalizedRoutes.Count, Is.EqualTo(2));
            Assert.That(localizedRoute.HasTranslationForCulture("nl-NL"), Is.False);
            Assert.That(localizedRoute.HasTranslationForCulture("fr-FR"), Is.True);
            Assert.That(localizedRoute.HasTranslationForCulture("en-US"), Is.True);
        }

        [Test]
        public void LocalizationRouteCollection_AddTranslationDuplicatesForCulture_Throws()
        {
            var localizedRoute = CreateLocalizedRoute();
            localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}");
            Assert.That(() => localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}"), Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo("Route already has translation for culture 'en-US'."));
        }

        [Test]
        public void LocalizationRouteCollection_RemoveTranslation_NullOrEmtpyCulture_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.RemoveTranslation(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_RemoveTranslation_NonExistingCulture_ThrowsNothing()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.RemoveTranslation("fr-FR"), Throws.Nothing);
        }

        [Test]
        public void LocalizationRouteCollection_OutboundRoute_CultureInThreadTest()
        {
            var localizedRoute = CreateLocalizedRoute();
            localizedRoute.AddTranslation(LocalizationRouteCollection.NeutralCulture, "neutral/neutral-{title}_{id}");
            localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}");
            localizedRoute.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");

            var routes = new RouteCollection();
            routes.Add("OfferDetails", localizedRoute);

            var tester = new RouteTester(routes);

            using (new CultureScopedContext("en-US"))
            {
                tester.WithRouteInfo("OfferDetails", new { title = "project-manager", id = 12 })
                    .ShouldGenerateUrl("/job/job-project-manager_12");
            }
            using (new CultureScopedContext("fr-FR"))
            {
                tester.WithRouteInfo("OfferDetails", new { title = "chef-de-projet", id = 12 })
                    .ShouldGenerateUrl("/offre-de-emploi/offre-chef-de-projet_12");
            }
            using (new CultureScopedContext("nl-NL"))
            {
                tester.WithRouteInfo("OfferDetails", new { title = "project-manager", id = 12 })
                    .ShouldGenerateUrl("/neutral/neutral-project-manager_12");
            }
        }

        [Test]
        public void LocalizationRouteCollection_OutboundRoute_CultureInRouteValuesTest()
        {
            var localizedRoute = CreateLocalizedRoute();
            localizedRoute.AddTranslation(LocalizationRouteCollection.NeutralCulture, "neutral/neutral-{title}_{id}");
            localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}");
            localizedRoute.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");

            var routes = new RouteCollection();
            routes.Add("OfferDetails", localizedRoute);

            var tester = new RouteTester(routes);

            tester.WithRouteInfo("OfferDetails", new { title = "project-manager", id = 12, culture = "en-US" })
                    .ShouldGenerateUrl("/job/job-project-manager_12");
            tester.WithRouteInfo("OfferDetails", new { title = "chef-de-projet", id = 12, culture = "fr-FR" })
                    .ShouldGenerateUrl("/offre-de-emploi/offre-chef-de-projet_12");
            tester.WithRouteInfo("OfferDetails", new { title = "project-manager", id = 12, culture = "nl-NL" })
                .ShouldGenerateUrl("/neutral/neutral-project-manager_12");
        }

        [Test]
        public void LocalizationRouteCollection_InboundRouteTest()
        {
            var localizedRoute = CreateLocalizedRoute();
            localizedRoute.AddTranslation(LocalizationRouteCollection.NeutralCulture, "neutral/neutral-{title}_{id}");
            localizedRoute.AddTranslation("en-US", "job/job-{title}_{id}");
            localizedRoute.AddTranslation("fr-FR", "offre-de-emploi/offre-{title}_{id}");

            var routes = new RouteCollection();
            routes.Add("OfferDetails", localizedRoute);

            var tester = new RouteTester(routes);

            tester.WithIncomingRequest("job/job-project-manager_12")
                .ShouldMatchPageRoute("~/pages/offer/details.aspx", new { id = 12, title = "project-manager", culture = "en-US" });

            tester.WithIncomingRequest("offre-de-emploi/offre-chef-de-projet_12")
                .ShouldMatchPageRoute("~/pages/offer/details.aspx", new { id = 12, title = "chef-de-projet", culture = "fr-FR" });

            tester.WithIncomingRequest("/neutral/neutral-project-manager_12")
                .ShouldMatchPageRoute("~/pages/offer/details.aspx", new { title = "project-manager", id = 12, culture = LocalizationRouteCollection.NeutralCulture });
        }

        [Test]
        public void LocalizationRouteCollection_HasTranslationWithNullCulture_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.HasTranslationForCulture(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_GetRouteDataWithNullContext_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.GetRouteData(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_GetVirtualPathWithNullContext_ThrowsArgumentNullException()
        {
            var localizedRoute = CreateLocalizedRoute();
            Assert.That(() => localizedRoute.GetVirtualPath(null, new RouteValueDictionary()), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LocalizationRouteCollection_GetVirtualPathWithNullValues_ThrowsArgumentNullException()
        {
            var localizedRoute = new LocalizationRouteCollection("~/pages/offer/list.aspx");
            localizedRoute.AddTranslation("en-US", "job/job-list");
            localizedRoute.AddTranslation("fr-FR", "offre-de-emploi/liste-offre");

            var routes = new RouteCollection();
            routes.Add("OfferList", localizedRoute);

            var tester = new RouteTester(routes);

            using (new CultureScopedContext("en-US"))
            {
                tester.WithRouteInfo("OfferList").ShouldGenerateUrl("/job/job-list");
            }
            using (new CultureScopedContext("fr-FR"))
            {
                tester.WithRouteInfo("OfferList").ShouldGenerateUrl("/offre-de-emploi/liste-offre");
            }
        }
    }
}
