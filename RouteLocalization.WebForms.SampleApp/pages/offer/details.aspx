<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="details.aspx.cs" Inherits="RouteLocalization.WebForms.SampleApp.pages.offer.details" %>
<%@ Import Namespace="System.Web.Routing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <h2>Inbound routing</h2>
    <ul>
        <li>Culture is: <%: this.Culture %></li>
        <li>UICulture is: <%: this.UICulture %></li>
        <li>Offer ID: <%: this.RouteData.Values["id"] %></li>
        <li>Offer title: <%: this.RouteData.Values["title"] %></li>
    </ul>
    
    <h2>Outbound routing</h2>
    <ul>
        <li><%: RouteTable.Routes.GetVirtualPath(null, "OfferDetails", new RouteValueDictionary() {{"title", "chef-de-projet"}, {"id", 12}}).VirtualPath %></li>
        <li><asp:Literal runat="server" Text="<%$RouteUrl:title=chef-de-projet,id=12,routename=OfferDetails%>"/></li>
    </ul>
</body>
</html>
