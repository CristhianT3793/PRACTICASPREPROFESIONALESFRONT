<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pasantias.aspx.cs" Inherits="FPP_front.pasantias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="loading" align="center">
        <div class="container2">
            <div class="loader"></div>
        </div>
    </div>
    <div class="container animated fadeIn" style="width: 90%">
        <asp:HiddenField ID="hddPeriodo" runat="server" />
        <asp:HiddenField ID="hddIdentificacion" runat="server" />
        <asp:HiddenField ID="hddIdPasante" runat="server" />
        <div class="row well">
            <h4>Envio de datos a Registro Académico
            </h4>
        </div>
        <div class="row well">
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
                    <asp:LinkButton ID="btnBusqueda" Style="background: #19A818; color: white" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="row well">
            <div class="table-responsive">
                <asp:Panel ID="pnlPasante" runat="server">
                    <asp:GridView ID="dgvPasante" runat="server" DataKeyNames="IDENTIFICACION_PASANTE,FACULTAD_PASANTE,ENVIADO_REGISTRO,APELLIDO_PASANTE,NOMBRE_PASANTE,CARRERA_PASANTE,COD_CARRERA_PASANTE,COD_FACULTAD_PASANTE" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvPasante_RowCommand" PageSize="10" OnPageIndexChanging="dgvPasante_PageIndexChanging" OnRowDataBound="dgvPasante_RowDataBound">
                        <%--<asp:GridView ID="dgvPasante" runat="server" DataKeyNames="IDENTIFICACION_PASANTE,APELLIDO_PASANTE,NOMBRE_PASANTE,CARRERA_PASANTE,COD_CARRERA_PASANTE" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvPasante_RowCommand" PageSize="10" OnPageIndexChanging="dgvPasante_PageIndexChanging" OnRowDataBound="dgvPasante_RowDataBound">--%>
                        <Columns>
                            <asp:TemplateField HeaderText="Identificación">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="pasantiasalumno" ID="lblCedula" Text='<%#Eval("IDENTIFICACION_PASANTE")%>' CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" Style="color: blue; text-decoration: underline;">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:BoundField DataField="IDENTIFICACION_PASANTE" HeaderText="Identificación" SortExpression="IDENTIFICACION_PASANTE" Visible="false" />
                            <asp:BoundField DataField="APELLIDO_PASANTE" HeaderText="Apellidos" SortExpression="APELLIDO_PASANTE" />
                            <asp:BoundField DataField="NOMBRE_PASANTE" HeaderText="Nombres" SortExpression="NOMBRE_PASANTE" />
                            <asp:BoundField DataField="CARRERA_PASANTE" HeaderText="Carrera" SortExpression="CARRERA_PASANTE" />
                            <asp:BoundField DataField="FACULTAD_PASANTE" HeaderText="Facultad" SortExpression="FACULTAD_PASANTE" />
                            <asp:BoundField DataField="COD_CARRERA_PASANTE" HeaderText="Cód Carrera" SortExpression="COD_CARRERA_PASANTE" Visible="false" />
                            <asp:BoundField DataField="COD_FACULTAD_PASANTE" HeaderText="Cód Facultad" SortExpression="COD_FACULTAD_PASANTE" Visible="false" />
                            <asp:BoundField DataField="ENVIADO_REGISTRO" HeaderText="Enviado Registro" SortExpression="ENVIADO_REGISTRO" Visible="false" />
                            <asp:BoundField DataField="TOTAL_HORAS" HeaderText="Total de Horas" SortExpression="TOTAL_HORAS" />
                            <asp:BoundField DataField="EMPRESAS" HeaderText="Empresas" SortExpression="EMPRESAS" />
                            <asp:TemplateField HeaderText="Ver Pasantías" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="pasantiasalumno" runat="server" CommandName="pasantiasalumno" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                            <i class="fas fa-eye" style="font-size:23px;color:#0F6FB5"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Enviar datos">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Enviar" runat="server" CommandName="Enviar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" OnClientClick="return Acepta(this);" ValidationGroup="inicio">
                                            <i class="fas fa-file-import" style="font-size:23px;color:#0F6FB5"></i>
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
        </div>
        <!--Modal de fpp-->
        <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server" Enabled="True" TargetControlID="btnPopUp"
            BackgroundCssClass="modalBackground" PopupControlID="PanelModal">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Button ID="btnPopUp" runat="server" Text="MOSTRAR POPUP" hidden="hidden" />
        <asp:Panel ID="PanelModal" runat="server" Style="display: none; background: white; height: 80%; width: 80%; border: 0.5px solid #CBCBD3; border-radius: 5px; overflow-x: scroll; box-shadow: 0 0 25px #5D5C5C;">
            <div class="modal-header">
                <h4 style="display: inline-block"><b>Pasantías</b></h4>
                <asp:Button ID="btnclose" runat="server" Text="x" data-dismiss="modal" aria-hidden="true" class="close" Style="color: white; background-color: red; border: 1px solid red;" />
            </div>
            <div class="modal-body" style="height: 100%; width: 100%">
                <%--<iframe runat="server" id="ifrm" style="border: none; width: 100%; height: 100%"></iframe>--%>
                <div class="well">
                    <div class="row">
                        <div class="col-md-1">
                            <b>
                                <asp:Label ID="Label21" runat="server" Text="Nombres alumno:"></asp:Label></b>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblNombresAlumno" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <b>
                                <asp:Label ID="Label22" runat="server" Text="Identificación:"></asp:Label></b>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="lblIdentificacion" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <b>
                                <asp:Label ID="Label23" runat="server" Text="Carrera:"></asp:Label></b>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblcarrera" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <b>
                                <asp:Label ID="Label17" runat="server" Text="Facultad:"></asp:Label></b>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblFacultad" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <asp:Table runat="server" ID="grd_Valores" CssClass="table-responsive table table-bordered" Width="100%"></asp:Table>
                <div class="row" style="display: none">
                    <div class="col-md-4 ">
                        <asp:Button ID="btnGenerarInforme" runat="server" Text="Generar Informe" OnClick="btnGenerarInforme_Click" CssClass="btn btn-sm btn-success" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <!--fin modal fpp-->
    </div>
    <script>
        var object_acept = { status: false, ele: null };
        function Acepta(ev) {
            if (object_acept.status) { return true; }
            Swal.fire({
                title: 'Advertencia',
                text: "Está seguro de Enviar los datos de pasantías a Registro Académico",
                icon: 'info',
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
                    ShowProgress();
                }
            })
            return false;
        }
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
