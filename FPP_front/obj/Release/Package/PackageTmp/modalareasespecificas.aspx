<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modalareasespecificas.aspx.cs" Inherits="FPP_front.modalareasespecificas" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Style/EstiloPPP.css" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <!-- jQuery CDN -->
    <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
    <!-- Bootstrap Js CDN -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <!-- jQuery Custom Scroller CDN -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/malihu-custom-scrollbar-plugin/3.1.5/jquery.mCustomScrollbar.concat.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="scrip">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
            </Scripts>
        </asp:ScriptManager>
        <div class="container-fluid well">
            <div class="row">
                <div class="col-md-5">
                    <h3>Campo Específico
                        
                    </h3>
                </div>
                <div class="col-md-5">
                    <asp:Button runat="server" Text="Guardar" ID="btnSaveAreaEspecifica" CssClass="btn btn-sm btn-success" />
                </div>

            </div>
        </div>

        <div class="container-fluid well">
            <div class="row-fluid">
                <div class="span4">
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label class="control-label"><strong>Campo Amplio</strong></label>
                            <div class="controls">
                                <asp:TextBox ID="txtCampoAmplio" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label class="control-label"><strong>Campo Específico</strong></label>
                        <div class="controls">
                            <div class="col-md-8" style="display: block">
                                <asp:TextBox ID="txtCampoEspecifico" CssClass="form-control" runat="server" onkeyup="mayus(this)"></asp:TextBox>
                                <asp:Repeater ID="rptCampoespecifco" runat="server" OnItemCreated="rptCampoespecifco_ItemCreated" OnItemCommand="rptCampoespecifco_ItemCommand">
                                    <HeaderTemplate>
                                        <table class="table">
                                            <thead>
                                                <tr>
                                                    <th>Campo Específico</th>
                                                    <th>Acciones</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtcampoespecifico" Text='<%# Eval("DescripcionCampoEspecifico") %>' CssClass="form-control" MaxLength="149" onkeyup="mayus(this)" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnEliminar" CommandName="Delete" runat="server" CommandArgument="<%# Container.ItemIndex + 1 %>"><i class="fas fa-trash-alt fa-2x" style="color:red"></i></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>       
                                    </FooterTemplate>
                                </asp:Repeater>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
