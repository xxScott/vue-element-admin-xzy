<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestCurrent.aspx.cs" Inherits="TestCurrent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Button ID="btnBindCookies" runat="server" Text="绑定OpenID到Cookie" OnClick="btnBindCookies_Click" />
    <asp:Button ID="btnGetCurrentInfo" runat="server" Text="获取当前登录人信息" OnClick="btnGetCurrentInfo_Click" />
        </div>
    </form>
   
</body>
</html>
