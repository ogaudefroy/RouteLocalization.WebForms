﻿namespace RouteLocalization.WebForms.SampleApp
{
    using System;
    using System.Web.UI;

    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("OfferList");
        }
    }
}