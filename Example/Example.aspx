<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="WebApplication1.Example" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Flickstein.NET</title>
    </head>
    <body>
        <form runat="server">
            <center>
                <h1>Flickstein.NET</h1>
                <h3>Open-Source Flickr API</h3>
                <a href="https://github.com/leonardiwagner/Flickstein.NET">view website</a>
                <div style="margin-top:100px;font-size:28px;">
                    Consumer Key: <asp:TextBox runat="server" ID="txtConsumerKey"></asp:TextBox>
                    Secret Key: <asp:TextBox runat="server" ID="txtSecretKey"></asp:TextBox>
                    Callback URL: <asp:TextBox runat="server" ID="txtCallbackUrl"></asp:TextBox>
                    <br /><br/>
                    <asp:LinkButton runat="server" ID="btnOK">Send Data</asp:LinkButton>
                    <% if (!String.IsNullOrEmpty(FlickrPermissionUrl))
                       { %>
                    <br/><br/>
                    <a href="javascript:window.open('<%= FlickrPermissionUrl %>','Flickstein Example','width=600,height:300'); void(0);">Get Flickr Permission</a>
                    <% } %>
                </div>
            </center>
        </form>
    </body>
</html>