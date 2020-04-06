<%@ Page Language="C#" EnableSessionState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Who Am I?</title>
</head>
<body>
    <div>
        Web Name:
        <%= Request.ServerVariables["Server_Name"] %></div>
    <div>
        Server IP:
        <%= Request.ServerVariables["LOCAL_ADDR"]%></div>
    <div>
        Server Time:
        <%= DateTime.Now%></div>
    <div>
        Client IP:
        <%= Request.UserHostAddress%></div>
</body>
</html>