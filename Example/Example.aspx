<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="WebApplication1.Example" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="javascript:window.open('<%=this.FlickrPermissionUrl%>','Flickrizer Example','width=600,height:300'); void(0);">Get Flickr Permission</a>
    </div>
    </form>
</body>
</html>
