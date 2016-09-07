<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="details.aspx.cs" Inherits="RouteLocalization.WebForms.SampleApp.pages.offer.details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <ul>
        <li>Culture is: <%: this.Culture %></li>
        <li>UICulture is: <%: this.UICulture %></li>
        <li>Offer ID: <%: this.RouteData.Values["id"] %></li>
        <li>Offer title: <%: this.RouteData.Values["title"] %></li>
    </ul>
</body>
</html>
