<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="FPP_front.Empresas" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <%--<script src="Scripts/funciones.js" type="text/javascript"></script>--%>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
    <style>
        .pager li:last-child a {
            border-top-right-radius: 5px;
            border-bottom-right-radius: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <div class="loading" align="center">
        <div class="container2">
            <div class="loader"></div>
        </div>
    </div>--%>
    <div class="container animated fadeIn" style="width: 90%;">
        <asp:HiddenField ID="hddFechaRegistro" runat="server" />
        <div class="well row">
            <div class="col-md-8">
                <h4 class="">Registro de Empresas</h4>
            </div>
            <div class="col-md-4">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="dgvEmpresas" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlAcciones1" runat="server" Visible="false">
                            <asp:LinkButton ID="btntModificarTutor" CssClass="btn btn-sm" Style="background: #19A818; float: right; color: white" runat="server" OnClick="btntModificarEmpresa_Click"><i class="fa fa-save"></i> Modificar</asp:LinkButton>
                            <asp:LinkButton ID="btnCancelarEdicion" CssClass="btn btn-sm" Style="background: #CD3523; margin-right: 2px; float: right; color: white" runat="server" OnClick="btnCancelarEdicion_Click"><i class="fa fa-brush"></i> Cancelar</asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel ID="pnlAcciones2" runat="server">
                            <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; float: right; color: white" runat="server" OnClick="btnGuardar_Click" OnClientClick="ShowProgress()" ValidationGroup="inicio"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row well">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnCancelarEdicion" />
                </Triggers>
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-1 ">
                            <label>RUC</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtRucEmpresa" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1 ">
                            <label>Nombre Empresa</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtNombreEmpresa" runat="server" TextMode="MultiLine" CssClass="form-control" onkeyup="mayus(this);"></asp:TextBox>
                        </div>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-1">
                            <label>Tipo Institución</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtTipoInsctitucion" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                        </div>
                        <div class="col-md-1 ">
                            <label>Dirección</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtDireccion" runat="server" class="form-control" TextMode="MultiLine" onkeyup="mayus(this);"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">

                        <div class="col-md-1">
                            <label>Teléfono 1</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtTelefono1" runat="server" class="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1 ">
                            <label>E-mail</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <br />
                    <div class="row">
                        <div style="display: none">
                            <div class="col-md-1">
                                <label>Homologada</label>
                            </div>
                            <div class="col-md-4">
                                <label class="switch">
                                    <input type="checkbox" runat="server" id="chkHomologada" checked="checked" />
                                    <span class="slider round"></span>
                                </label>
                            </div>
                        </div>

                    </div>
                    <div class="row">

                        <div style="display: none">
                            <div class="col-md-1">
                                <label>Teléfono 2</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTelefono2" runat="server" class="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-1">
                            <label>Descripción convenio</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtObservacion" runat="server" class="form-control" TextMode="MultiLine" onkeyup="mayus(this);"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <label>Activo</label>
                        </div>
                        <div class="col-md-5">
                            <label class="switch">
                                <input type="checkbox" runat="server" id="chkActivoEmp" checked="checked" />
                                <span class="slider round"></span>
                            </label>
                        </div>



                    </div>
                    <br />
                    <div class="row">
                        <div style="display: none">
                            <div class="col-md-1 col-sm-offset-1">
                                <label>Objetivo Empresa</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtObjetivo" runat="server" class="form-control" TextMode="MultiLine" onkeyup="mayus(this);"></asp:TextBox>
                            </div>
                        </div>
                        <div style="display: none">
                            <div class="col-md-1">
                                <label>Fecha Firma Empresa</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFechaFirma" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="row well">

            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-2">
                        <label>RUC/Nombre Empresa</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchEmpresa" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                    </div>
                    <asp:LinkButton ID="btnBusqueda" Style="background: #19A818; color: white" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
                </div>
                <br />
                <asp:UpdatePanel runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:Panel ID="pnlEmpresa" runat="server">
                                <asp:GridView ID="dgvEmpresas" runat="server" DataKeyNames="idEmpresa,nombreEmpresa" AutoGenerateColumns="False" CssClass="table table-bordered table-striped pager" AllowPaging="True" OnRowCommand="dgvEmpresas_RowCommand" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvEmpresas_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="idEmpresa" HeaderText="idEmpresa" SortExpression="idEmpresa" Visible="false" />
                                        <asp:BoundField DataField="RucEmpresa" HeaderText="Ruc Empresa" SortExpression="RucEmpresa" />
                                        <asp:BoundField DataField="NombreEmpresa" HeaderText="Nombre Empresa" SortExpression="NombreEmpresa" />
                                        <asp:BoundField DataField="TipoEmpresa" HeaderText="Tipo Empresa" SortExpression="TipoEmpresa" />
                                        <asp:BoundField DataField="DireccionEmpresa" HeaderText="Dirección Empresa" SortExpression="DireccionEmpresa" />
                                        <asp:BoundField DataField="Telefono1Empresa" HeaderText="Teléfono 1" SortExpression="Telefono1Empresa" />
                                        <asp:BoundField DataField="Telefono2Empresa" HeaderText="Teléfono 2" SortExpression="Telefono2Empresa" Visible="false" />
                                        <asp:BoundField DataField="EmailEmpresa" HeaderText="Email" SortExpression="EmailEmpresa" />
                                        <asp:BoundField DataField="FechafirmaEmpresa" HeaderText="Fecha Firma" SortExpression="FechafirmaEmpresa" Visible="false" />
                                        <asp:BoundField DataField="ObjetivoEmpresa" HeaderText="Objetivo Empresa" SortExpression="ObjetivoEmpresa" Visible="false" />
                                        <asp:BoundField DataField="ObservacionEmpresa" HeaderText="Descripción Convenio" SortExpression="ObservacionEmpresa" />
                                        <asp:BoundField DataField="FechaRegistroEmpresa" HeaderText="Fecha registro" SortExpression="FechaRegistroEmpresa" Visible="false" />
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActivoEmpresa" runat="server" Checked='<%#Eval("activoEmpresa")%>' onclick="this.checked=!this.checked;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Homolagada" Visible="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkHomologada" runat="server" Checked='<%#Eval("HomologadaEmpresa")%>' onclick="this.checked=!this.checked;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Añadir Tutor" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnAddTutor" runat="server" CommandName="AddTutorEnterprise" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                            <i class="fas fa-user-plus" style="font-size:25px;color:limegreen"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modificar Datos">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnModificarEmpresa" runat="server" CommandName="ModificarEmpresa" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                            <i class="fas fa-edit" style="font-size:25px;color:darkorange"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <PagerStyle BackColor="#E8E8E8" ForeColor="#003399" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="#00000" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                    <SortedDescendingHeaderStyle BackColor="#002876" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>


    </div>
    <asp:HiddenField ID="hiddenIdEmpresa" runat="server" />
    <!--inicio modal tutores-->
    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server" Enabled="True" TargetControlID="Button"
        BackgroundCssClass="modalBackground" PopupControlID="PanelModal">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="Button" runat="server" hidden="hidden" />
    <asp:Panel ID="PanelModal" runat="server" Style="display: none; background: white; height: 80%; width: 80%; border: solid 0.5px #CBCBD3; border-radius: 5px; overflow-x: scroll; box-shadow: 0 0 25px #5D5C5C;" CssClass="animated fadeIn">
        <div class="modal-header">
            <asp:Button ID="btnclose" runat="server" Text="x" data-dismiss="modal" aria-hidden="true" class="close" Style="color: white; background-color: red; border: 1px solid red;" />
        </div>
        <div class="modal-body" style="height: 100%; width: 100%">
            <iframe runat="server" id="ifrm" style="border: none; width: 100%; height: 100%"></iframe>
        </div>
    </asp:Panel>
    <!--Fin modal tutores-->
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            var fecha = new Date();
            var fecha_fin = fecha.format('yyyy-MM-dd');
            $('#<%=txtFechaFirma.ClientID%>').val(fecha_fin);
        });
        function ShowProgress() {

            Page_ClientValidate("inicio");
            if (Page_IsValid) {
                var elems = document.getElementsByClassName("carga");
                for (var k = elems.length - 1; k >= 0; k--) {
                    var parent = elems[k].parentNode;
                    parent.removeChild(elems[k]);
                }
                setTimeout(function () {
                    $('.modal').remove();
                    var modal = $('<div />');
                    modal.addClass("modal carga");
                    $('body').append(modal);
                    var loading = $(".loading");
                    loading.show();
                    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                    loading.css({ top: top, left: left });
                }, 200);
            }
        }
    </script>
</asp:Content>
