<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="PremioExcelencia.Login.login"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="../Style/EstiloLogin.css" type="text/css" rel="stylesheet" />
    <link href="../Style/MyStyle.css" rel="stylesheet" type="text/css" />
 
</head>
<body>
    <!--header-->
    <nav class="navbar-head">
        <div style="float: left; width: 15%; height: 100%;" class="nomAplicacion">
            <a href="#">
                <asp:Image src="/images/logo-sek-2.png" alt="Dont exist image" runat="server" Style="width: 100%; height: 100%" />
            </a>
        </div>
        <p style="float: right; padding-right: 80px">
            FPP
        </p>

        <br />
        <br />
    </nav>
    <nav class="subnav">
        <div style="height: 50px">
        </div>
    </nav>
    <!--fin header-->

    <form id="form1" runat="server">
        <div id="back">
            <div class="backLeft"></div>
        </div>
        <div id="slideBox">

            <div class="right">
                <div class="content">
                    <h2 style="text-align: right; color: black">Login</h2>
                    <div class="row">
                        <div class="form-group">
                            <label for="Usuario" class="form-label">Usuario</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="lbluser_v" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label for="password" class="form-label">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control pass" TextMode="Password">                          
                            </asp:TextBox>
                            <asp:Label ID="lblpassword_v" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                    </div>
                    <center>
                        <div class="row">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-sm btn-success" Width="50%" OnClick="btnLogin_Click"/>
                        </div>
                    </center>
                    <hr size="2px" color="black" />
                    <center>
                        <div class="row">
                        </div>
                    </center>
                </div>
            </div>

        </div>
    </form>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div class="footer-content" style="padding: 0.5%;">
        <footer>
            <p style="color: white"><span class="glyphicon glyphicon-copyright-mark"></span>2021 - Dirección de Tecnología </p>
        </footer>
    </div>
</body>
</html>
<script>

</script>
