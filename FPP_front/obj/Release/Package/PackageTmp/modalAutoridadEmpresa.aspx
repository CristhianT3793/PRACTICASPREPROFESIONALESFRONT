<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modalAutoridadEmpresa.aspx.cs" Inherits="FPP_front.modalAutoridadEmpresa" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Style/EstiloPPP.css" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" />
    <!-- jQuery CDN -->
    <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
    <!-- Bootstrap Js CDN -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <!-- jQuery Custom Scroller CDN -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/malihu-custom-scrollbar-plugin/3.1.5/jquery.mCustomScrollbar.concat.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
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
                <div class="col-md-8">
                    <h4><b>
                        <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label></b></h4>                   
                    <%--<h4>Autoridades Empresa </h4>--%>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="dgvTutorEmpresa" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:LinkButton ID="btnSaveTutor" CssClass="btn btn-sm" Style="background: #19A818; float: right; margin-left: 2px; color: white" runat="server" OnClick="btnSaveTutor_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
                        <asp:LinkButton ID="btnCancelarEdicion" CssClass="btn btn-sm" Style="background: #CD3523; float: right; margin-left: 2px; color: white" runat="server" OnClick="btnCancelarEdicion_Click" Visible="false"><i class="fa fa-brush"></i> Cancelar</asp:LinkButton>
                        <asp:LinkButton ID="btntModificarTutor" CssClass="btn btn-sm" Style="background: #19A818; float: right; color: white" runat="server" OnClick="btntModificarTutor_Click" Visible="false"><i class="fa fa-save"></i> Modificar</asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnCancelarEdicion" />
            </Triggers>
            <ContentTemplate>
                <div class="container-fluid well">
                    <div class="row-fluid">
                        <div class="span4">
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Identificación</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtIdentificacionTEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Cargo</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCargoTEmpresa" runat="server" CssClass="form-control" onkeyup="mayus(this);"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Nombres</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNombresTEmpresa" runat="server" CssClass="form-control" onkeyup="mayus(this);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Apellidos</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" onkeyup="mayus(this);"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Teléfono</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTelefonoTEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Celular</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCelularTEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Email</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtEmailTEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Dirección</strong></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDireccinTEmpresa" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="mayus(this);"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="display:none">
                                <div class="form-group col-sm-6">
                                    <label class="control-label"><strong>Activo</strong></label>
                                    <div class="controls">
                                        <label class="switch">
                                            <input type="checkbox" runat="server" id="chkActivoTEmp" checked="checked" />
                                            <span class="slider round"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Identificación/nombres"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton Style="background: #19A818; color: white" ID="btnBusqueda" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="table-responsive">
                    <asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSaveTutor" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <h3 style="text-align: center">
                                    <asp:Label Text="No existen datos" runat="server" Visible="false" ID="lblExisteDatos"></asp:Label>
                                </h3>
                                <asp:GridView ID="dgvTutorEmpresa" runat="server" DataKeyNames="idEmpresa,idAutoridadEmpresa" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvTutorEmpresa_PageIndexChanging" OnRowCommand="dgvTutorEmpresa_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="idAutoridadEmpresa" HeaderText="idAutoridadEmpresa" SortExpression="idAutoridadEmpresa" Visible="false" />
                                        <asp:BoundField DataField="idEmpresa" HeaderText="idEmpresa" SortExpression="idEmpresa" Visible="false" />
                                        <asp:BoundField DataField="identificacionAempresa" HeaderText="Identificación" SortExpression="identificacionAempresa" />
                                        <asp:BoundField DataField="nombreAempresa" HeaderText="Nombres" SortExpression="nombreAempresa" />
                                        <asp:BoundField DataField="apellidoAempresa" HeaderText="Apellidos" SortExpression="apellidoAempresa" />
                                        <asp:BoundField DataField="emailAempresa" HeaderText="Email" SortExpression="emailAempresa" />
                                        <asp:BoundField DataField="telefonoAempresa" HeaderText="Teléfono" SortExpression="telefonoAempresa" />
                                        <asp:BoundField DataField="celularAempresa" HeaderText="Celular" SortExpression="celularAempresa" />
                                        <asp:BoundField DataField="direccionAempresa" HeaderText="Dirección" SortExpression="direccionAempresa" />
                                        <asp:BoundField DataField="cargoAempresa" HeaderText="Cargo" SortExpression="cargoAempresa" />
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActivoTEmpresa" runat="server" Checked='<%#Eval("ActivoAempresa")%>' onclick="this.checked=!this.checked;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modificar">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnModificarTutor" runat="server" CommandName="ModificarTutor" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                                    <i class="fas fa-edit" style="font-size:25px;color:darkorange"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <PagerStyle HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="#003399" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                    <SortedDescendingHeaderStyle BackColor="#002876" />
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

