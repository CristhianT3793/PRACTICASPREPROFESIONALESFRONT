<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="planificacionsemestral.aspx.cs" Inherits="FPP_front.planificacionsemestral" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container animated fadeIn" style="width: 90%">
        <div class="row well">
            <div class="col-md-10">
                <h4>Planificación Semestral</h4>
            </div>
            <div class="col-md-2">
                <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; color: white; float: right" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
            </div>

        </div>
        <div class="row well ">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label5" runat="server" Text="Cód Carrera"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlcarrera" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlcarrera_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <%--<div class="col-xs-1"></div>--%>
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Semestre"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlsemestre" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label6" runat="server" Text="Plantilla"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtpathFPP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-xs-2">
                    <asp:LinkButton ID="btnDescargar" runat="server" OnClick="btnDescargar_Click">
                        <i class="fas fa-file-download" style="font-size:25px;color:dodgerblue"></i>
                    </asp:LinkButton>
                </div>
                <div style="display: none">
                    <div class="col-md-1">
                        <asp:Label ID="Label2" runat="server" Text="Observación"></asp:Label>
                    </div>
                    <div class="col-md-4" style="display: block">
                        <asp:TextBox ID="txtObservacion" CssClass="form-control" runat="server" TextMode="multiline"></asp:TextBox>
                    </div>
                </div>

            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label4" runat="server" Text="Archivo"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:FileUpload ID="fileplanificacion" runat="server" CssClass="form-control" />
                </div>
                <div class="col-xs-1"></div>
                <div style="display: none">
                    <div class="col-md-1">
                        <asp:Label ID="Label3" runat="server" Text="Activo"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <label class="switch">
                            <input type="checkbox" runat="server" id="chkActivo" checked="checked" />
                            <span class="slider round"></span>
                        </label>
                    </div>
                </div>

            </div>

        </div>

        <div class="row well">
            <div class="row">
                <div class="col-md-2">
                    <label>Carrera</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnBusqueda" runat="server" CssClass="btn btn-sm btn-success" Style="background: #19A818" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
            </div>
            <br />
            <div class="table-responsive">
                <asp:Panel ID="pnlPlanificacion" runat="server">
                    <asp:GridView ID="dgvFppPlanificacion" runat="server" OnRowCommand="dgvFppPlanificacion_RowCommand" DataKeyNames="IdFppCoordinador,IdPlantilla,PathFppCoordinador" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvFppPlanificacion_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="IdFppCoordinador" HeaderText="ID" SortExpression="IdFppCoordinador" Visible="false" />
                            <asp:BoundField DataField="Idcoordinador" HeaderText="ID COORDINADOR" SortExpression="Idcoordinador" Visible="false" />
                            <asp:BoundField DataField="IdPlantilla" HeaderText="ID PLANTILLA" SortExpression="IdPlantilla" Visible="false" />
                            <asp:BoundField DataField="FechaRegistroFpp" HeaderText="Fecha de Registro" SortExpression="FechaRegistroFpp" />
                            <asp:BoundField DataField="PathFppCoordinador" HeaderText="Archivo" SortExpression="PathFppCoordinador" />
                            <asp:BoundField DataField="NomPlatillaCordinador" HeaderText="Nombre Plantilla" SortExpression="NomPlatillaCordinador" />
                            <asp:BoundField DataField="CarreraFppCoordinador" HeaderText="Cód Carrera" SortExpression="CarreraFppCoordinador" />
                            <asp:BoundField DataField="SemestreFppCoordinador" HeaderText="Semestre" SortExpression="SemestreFppCoordinador" />
                            <asp:TemplateField HeaderText="Descargar Planificación">
                                <ItemTemplate>
                                    <asp:LinkButton ID="descargarInforme" CommandName="descargarInforme" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"><i class="fas fa-file-download" style="font-size:25px;color:#0F6FB5"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Modificar Documento">
                                <ItemTemplate>
                                    <asp:LinkButton ID="modificarDocumento" CommandName="modificarDocumento" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"><i class="fas fa-file-edit" style="font-size:25px;color:#3ab017"></i></asp:LinkButton>
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
            </div>
        </div>
        <!--modal subir Archivo-->
        <ajaxToolkit:ModalPopupExtender ID="mdlArchivo" runat="server" Enabled="True" TargetControlID="btnArchivo"
            BackgroundCssClass="modalBackground" PopupControlID="pnlArchivo">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlArchivo" runat="server" Style="display: none; background: white; width: 40%; height: auto; border-radius: 2px; box-shadow: 0 0 25px #5D5C5C;" CssClass="animated fadeIn">
            <div class="modal-header">
            </div>
            <div class="modal-body">
                <b>Archivo: </b>
                <asp:Label ID="lbldescarchivo" runat="server" Text="Archivo">
                </asp:Label>
                <asp:FileUpload ID="fileFpp" runat="server" CssClass="form-control" ClientIDMode="Static" />
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSubirFpp" runat="server" Text="Subir" OnClick="btnSubirFpp_Click" CssClass="btn" OnClientClick="return ConfirmArchivo(this)" />
                <button class="btn" data-dismiss="cancel" aria-hidden="true">Cancelar</button>
            </div>
        </asp:Panel>
        <asp:Button ID="btnArchivo" runat="server" Height="47px" Text="MOSTRAR POPUP"
            Width="258px" hidden="hidden" />
        <!--fin modal subir Archivo-->
    </div>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script>
        var object_acept = { status: false, ele: null };
        function ConfirmArchivo(ev) {
            if (object_acept.status) { return true; }
            Swal.fire({
                title: 'Advertencia',
                text: "Está seguro de modificar el archivo.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#29CB12',
                cancelButtonColor: '#d33',
                confirmButtonText: 'SI',
                cancelButtonText: 'NO'
            }).then((result) => {
                if (result.isConfirmed) {
                    object_acept.status = true;
                    object_acept.ele = ev;
                    object_acept.ele.click();
                }
            })
            return false;
        }
    </script>
</asp:Content>
