namespace RouteLocalization.WebForms.Tests
{
    using System;
    using System.Globalization;
    using System.Threading;

    public class CultureScopedContext : IDisposable
    {
        private readonly int _previousCulture;
        private readonly int _previousUICulture;

        public CultureScopedContext(string culture)
        {
            _previousCulture = CultureInfo.CurrentCulture.LCID;
            _previousUICulture = CultureInfo.CurrentUICulture.LCID;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(_previousCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_previousUICulture);
        }
    }
}
