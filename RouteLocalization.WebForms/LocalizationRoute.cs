namespace RouteLocalization.WebForms
{
    using System.Web.Routing;

    /// <summary>
    /// A localized route.
    /// </summary>
    public class LocalizationRoute : Route
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRoute"/> class.
        /// </summary>
        /// <param name="url">The public URL.</param>
        /// <param name="virtualPath">The underlying virtual path.</param>
        public LocalizationRoute(string url, string virtualPath)
            : this(url, virtualPath, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRoute"/> class.
        /// </summary>
        /// <param name="url">The public URL.</param>
        /// <param name="virtualPath">The underlying virtual path.</param>
        /// <param name="defaults">The values to use for any parameters that are missing in the URL.</param>
        public LocalizationRoute(string url, string virtualPath, RouteValueDictionary defaults)
            : this(url, virtualPath, defaults, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRoute"/> class.
        /// </summary>
        /// <param name="url">The public URL.</param>
        /// <param name="virtualPath">The underlying virtual path.</param>
        /// <param name="defaults">The values to use for any parameters that are missing in the URL.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        public LocalizationRoute(string url, string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints)
            : this(url, virtualPath, defaults, constraints, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationRoute"/> class.
        /// </summary>
        /// <param name="url">The public URL.</param>
        /// <param name="virtualPath">The underlying virtual path.</param>
        /// <param name="defaults">The values to use for any parameters that are missing in the URL.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">
        /// Custom values that are passed to the route handler, but which are not used to 
        /// determine whether the route matches a specific URL pattern. 
        /// These values are passed to the route handler, where they can be used for processing the request.
        /// </param>
        public LocalizationRoute(string url, string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens)
            : base(url, defaults, constraints, dataTokens, new PageRouteHandler(virtualPath, true))
        {
        }
    }
}