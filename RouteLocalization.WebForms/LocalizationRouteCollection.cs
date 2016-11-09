namespace RouteLocalization.WebForms
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Route containing all translated routes for a virtual path.
    /// </summary>
    public class LocalizationRouteCollection : Route
    {
        /// <summary>
        /// Gets the neutral culture key.
        /// </summary>
        public const string NeutralCulture = "";

        private readonly IDictionary<string, LocalizationRoute> _localizedRoutesContainer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRouteCollection"/> class, by using the specified virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path to map.</param>
        public LocalizationRouteCollection(string virtualPath)
            : this(virtualPath, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRouteCollection"/> class, by using the specified virtual path and default parameter values.
        /// </summary>
        /// <param name="virtualPath">The virtual path to map.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        public LocalizationRouteCollection(string virtualPath, RouteValueDictionary defaults)
        : this(virtualPath, defaults, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRouteCollection"/> class, by using the specified virtual path, default parameter values and constraints.
        /// </summary>
        /// <param name="virtualPath">The virtual path to map.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        public LocalizationRouteCollection(string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints)
            : this(virtualPath, defaults, constraints, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRouteCollection"/> class, by using the specified virtual path, default parameter values, constraints and custom values.
        /// </summary>
        /// <param name="virtualPath">The virtual path to map.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. These values are passed to the route handler, where they can be used for processing the request.</param>
        public LocalizationRouteCollection(string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens)
            : base(null, defaults, constraints, dataTokens, new PageRouteHandler(virtualPath, true))
        {
            _localizedRoutesContainer = new Dictionary<string, LocalizationRoute>(StringComparer.OrdinalIgnoreCase);
            this.VirtualPath = virtualPath;
        }

        /// <summary>
        /// Gets the Localized Route virtual path.
        /// </summary>
        public string VirtualPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the localized routes.
        /// </summary>
        public ICollection<LocalizationRoute> LocalizedRoutes
        {
            get
            {
                return _localizedRoutesContainer.Values;
            }
        }

        /// <summary>
        /// Gets the neutral route.
        /// </summary>
        public LocalizationRoute NeutralRoute
        {
            get
            {
                return _localizedRoutesContainer.ContainsKey(NeutralCulture) ? _localizedRoutesContainer[NeutralCulture] : null;
            }
        }

        /// <summary>
        /// Adds route translations, key: culture, value: the route value.
        /// </summary>
        /// <param name="values">The route translations.</param>
        public void AddTranslations(IDictionary<string, string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            foreach (var culture in values.Keys)
            {
                this.AddTranslation(culture, values[culture]);
            }
        }

        /// <summary>
        /// Adds a route translation.
        /// </summary>
        /// <param name="culture">The route localization culture.</param>
        /// <param name="url">The localized URL.</param>
        public void AddTranslation(string culture, string url)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (this.HasTranslationForCulture(culture))
            {
                throw new InvalidOperationException(string.Format("Route already has translation for culture '{0}'.", culture));
            }
            var route = new LocalizationRoute(
                url,
                this.VirtualPath,
                new RouteValueDictionary(this.Defaults ?? new RouteValueDictionary()) { { "culture", culture } },
                new RouteValueDictionary(this.Constraints ?? new RouteValueDictionary()),
                new RouteValueDictionary(this.DataTokens ?? new RouteValueDictionary()));
            _localizedRoutesContainer[culture] = route;
        }

        /// <summary>
        /// Returns whether or not the route has a translation for a culture.
        /// </summary>
        /// <param name="culture">The target culture.</param>
        /// <returns>True if route is translated, False otherwise.</returns>
        public bool HasTranslationForCulture(string culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            return _localizedRoutesContainer.ContainsKey(culture);
        }

        /// <summary>
        /// Removes the route translation.
        /// </summary>
        /// <param name="culture">The target culture.</param>
        public void RemoveTranslation(string culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            _localizedRoutesContainer.Remove(culture);
        }

        /// <summary>
        /// Retrieves the first translated route having route data non null.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The route data retrieved.</returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            return _localizedRoutesContainer.Select(x => x.Value.GetRouteData(httpContext)).FirstOrDefault(x => x != null);
        }

        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>An object that contains information about the URL that is associated with the route.</returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            
            string currentCulture = (values?["culture"] as string) ?? Thread.CurrentThread.CurrentUICulture.Name;
            var localizationRoute = this.GetLocalizedOrDefaultRoute(currentCulture);
            if (localizationRoute == null)
            {
                return null;
            }
            var pathData = localizationRoute.GetVirtualPath(requestContext, values != null ? new RouteValueDictionary(CopyAndRemoveFromValueDictionary(values)) : null);
            return pathData;
        }

        /// <summary>
        /// Gets the localized route ou neutral if non existing.
        /// </summary>
        /// <param name="currentCulture">The current culture.</param>
        /// <returns>The localized route.</returns>
        private RouteBase GetLocalizedOrDefaultRoute(string currentCulture)
        {
            LocalizationRoute route;

            _localizedRoutesContainer.TryGetValue(currentCulture, out route);

            if (route == null)
            {
                CultureInfo cultureInfo = new CultureInfo(currentCulture);
                _localizedRoutesContainer.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out route);
            }
            return route ?? NeutralRoute;
        }

        private static IDictionary<string, object> CopyAndRemoveFromValueDictionary(IDictionary<string, object> values)
        {
            if (values == null)
            {
                return null;
            }
            Dictionary<string, object> routeValueDictionary = new Dictionary<string, object>(values, StringComparer.OrdinalIgnoreCase);
            routeValueDictionary.Remove("culture");
            return routeValueDictionary;
        }
    }
}