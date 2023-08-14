<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConveniosEmpresas.aspx.cs" Inherits="FPP_front.ConveniosEmpresas" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container animated fadeIn" style="width: 90%">
        <div class="row well">
            <div class="col-md-8">
                <h4>Convenios</h4>
            </div>
            <div class="col-md-4">
                <asp:HiddenField ID="hdfNombreArchivo" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnGuardar" />
                        <asp:PostBackTrigger ControlID="btntModificarConvenio" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlAcciones1" runat="server" Visible="false">
                            <asp:LinkButton ID="btntModificarConvenio" CssClass="btn btn-sm" Style="background: #19A818; float: right; color: white" runat="server" OnClick="btntModificarConvenio_Click"><i class="fa fa-save"></i> Modificar</asp:LinkButton>
                            <asp:LinkButton ID="btnCancelarEdicion" CssClass="btn btn-sm" Style="background: #CD3523; float: right; margin-right: 2px; color: white" runat="server" OnClick="btnCancelarEdicion_Click"><i class="fa fa-brush"></i> Cancelar</asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel ID="pnlAcciones2" runat="server">
                            <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; color: white; float: right" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:HiddenField ID="hddFechaRegistro" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnCancelarEdicion" />
            </Triggers>
            <ContentTemplate>
                <div class="row well">
                    <div class="row">
                        <div class="col-md-2">
                            <label>Empresa</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control estilo_select" AppendDataBoundItems="true">
                                <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-2 ">
                            <label>Nombre Convenio</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtNombreConvenio" runat="server" CssClass="form-control" onkeyup="mayus(this);"></asp:TextBox>
                        </div>

                        <div class="col-md-2">
                            <label>Descripción Convenio</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDescripcionConvenio" runat="server" class="form-control" onkeyup="mayus(this);" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-2">
                            <label>Fecha Inicio</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFechaInicio" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-2 ">
                            <label>Fecha Fin</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFechaFin" TextMode="Date" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-2">
                            <label>Archivo</label>
                        </div>
                        <div class="col-md-4">
                            <asp:FileUpload ID="fileconvenio" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-2 ">
                            <label>Activo Convenio</label>
                        </div>
                        <div class="col-md-4">
                            <label class="switch">
                                <input type="checkbox" runat="server" id="chkActivoConvenio" checked="checked" />
                                <span class="slider round"></span>
                            </label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row well">
            <div class="row">
                <div class="col-md-2">
                    <label>Nombre/Descripción</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnBusqueda" Style="background: #19A818; color: white" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
            </div>
            <br />
            <asp:UpdatePanel runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                </Triggers>
                <ContentTemplate>
                    <div class="table-responsive">
                        <asp:Panel ID="pnlConvenio" runat="server">
                            <asp:GridView ID="dgvConvenios" runat="server" DataKeyNames="IdConvenio,PathConvenio,IdEmpresa" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" OnRowCommand="dgvConvenios_RowCommand" PageSize="10" OnPageIndexChanging="dgvConvenios_PageIndexChanging" OnRowDataBound="dgvConvenios_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="IdConvenio" HeaderText="IdConvenio" SortExpression="IdConvenio" Visible="false" />
                                    <asp:BoundField DataField="EmpresaConvenio" HeaderText="Empresa" SortExpression="EmpresaConvenio" />
                                    <asp:BoundField DataField="NombreConvenio" HeaderText="Nombre Convenio" SortExpression="NombreConvenio" />
                                    <asp:BoundField DataField="DescripcionConvenio" HeaderText="Descripción" SortExpression="DescripcionConvenio" />
                                    <asp:BoundField DataField="FechaInicioConvenio" HeaderText="Fecha Inicio " SortExpression="FechaInicioConvenio" />
                                    <asp:BoundField DataField="FechaFinConvenio" HeaderText="Fecha Fin" SortExpression="FechaFinConvenio" />
                                    <asp:BoundField DataField="PathConvenio" HeaderText="Archivo" SortExpression="PathConvenio" />
                                    <asp:BoundField DataField="IdEmpresa" HeaderText="IdEmpresa" SortExpression="IdEmpresa" Visible="false" />
                                    <asp:TemplateField HeaderText="Activo">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkActivoConvenio" runat="server" Checked='<%#Eval("ActivoConvenio")%>' onclick="this.checked=!this.checked;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descargar">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="descargarInforme" CommandName="descargarInforme" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"><i class="fas fa-file-download" style="font-size:25px;color:#0F6FB5"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modificar Convenio">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnModificarConvenio" runat="server" CommandName="ModificarConvenio" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
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
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            var fecha = new Date();
            var fecha_fin = fecha.format('yyyy-MM-dd');
            $('#<%=txtFechaInicio.ClientID%>').val(fecha_fin);
            $('#<%=txtFechaFin.ClientID%>').val(fecha_fin);
        });
        //select con buscador
        $(function () {
            $("[id*=ddlEmpresa]").select2();
            $(".estilo_select").select2({
                theme: "classic",
                width: 'resolve'

            });

        });
        //renderizado para todas las funciones cuando hace un postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                $(function () {
                    $("[id*=ddlEmpresa]").select2();
                });
                $(function () {
                    $(".estilo_select").select2({
                        theme: "classic",
                        width: 'resolve'
                    });
                });
            });
        }
    </script>
</asp:Content>
