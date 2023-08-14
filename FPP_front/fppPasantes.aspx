<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fppPasantes.aspx.cs" Inherits="FPP_front.fppPasantes" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloTabs.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
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
    <div class="loading" align="center">
        <div class="container2">
            <div class="loader"></div>
        </div>
    </div>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
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
        <div class="container-fluid">


            <!--encabezado alumno-->
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
            <!--fin encabezado-->
            <asp:HiddenField ID="hdfidpasante" runat="server" />

            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1">
                <!--primer tab-->
                <ajaxToolkit:TabPanel runat="server" HeaderText="Creación de Fpp" ID="TabPanel1" Visible="false">
                    <ContentTemplate>
                        <asp:LinkButton ID="btnCrearFpp" runat="server" CssClass="btn btn-primary" OnClick="btnCrearFpp_Click">Crear Fpps</asp:LinkButton>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <!--segundo tab-->
                <ajaxToolkit:TabPanel runat="server" HeaderText="Aprobación de Fpp" ID="TabPanel2">
                    <ContentTemplate>
                        <!--Grid fpp-->
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:Panel ID="pnlPasante" runat="server">
                                    <asp:GridView ID="dgvPasante" runat="server" DataKeyNames="IdFppPasante,NomrePlantilla,IdPasante,IdEstadoFpp,IdPlantilla,FppPasantePath,FppPasanteObservacion,FppPasanteActivo" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvPasante_RowCommand" OnRowDataBound="dgvPasante_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="IdFppPasante" HeaderText="IdPasante" SortExpression="IdFppPasante" Visible="false" />
                                            <asp:BoundField DataField="IdPasante" HeaderText="IdCampoEspecifico" SortExpression="IdPasante" Visible="false" />
                                            <asp:BoundField DataField="NomrePlantilla" HeaderText="Tipo Documento" SortExpression="NomrePlantilla" Visible="false" />
                                            <asp:TemplateField HeaderText="Tipo Documento">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="abrirModalHistorialFPP" ID="lblFPP" Text='<%#Eval("NomrePlantilla")%>' CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" Style="color: blue; text-decoration: underline;">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DescripcionEstadoFpp" HeaderText="Estado Actual" SortExpression="DescripcionEstadoFpp" />
                                            <asp:BoundField DataField="IdEstadoFpp" HeaderText="Estado" SortExpression="IdEstadoFpp" Visible="false" />
                                            <asp:BoundField DataField="IdAprobador" HeaderText="Aprobador" SortExpression="IdAprobador" Visible="false" />
                                            <asp:BoundField DataField="IdPlantilla" HeaderText="Plantilla" SortExpression="IdPlantilla" Visible="false" />
                                            <asp:BoundField DataField="FppPasanteEstado" HeaderText="Pasante " SortExpression="FppPasanteEstado" Visible="false" />
                                            <asp:BoundField DataField="FppPasantePath" HeaderText="Archivo" SortExpression="FppPasantePath" />
                                            <asp:BoundField DataField="FppPasanteFechaSubida" HeaderText="Fecha subida" SortExpression="FppPasanteFechaSubida" />
                                            <asp:BoundField DataField="FppPasanteObservacion" HeaderText="Observación" SortExpression="FppPasanteObservacion" />
                                            <asp:BoundField DataField="FppPasanteActivo" HeaderText="Fpp Activo" SortExpression="FppPasanteActivo" Visible="false" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEstadoFpp" runat="server" Text='<%# Eval("Idestadofpp") %>' Visible="false" />
                                                    <asp:DropDownList ID="ddlEstadosFpp" runat="server" OnSelectedIndexChanged="ddlEstadosFpp_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descargar" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="dowloadFile" CommandName="dowloadFile" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                                    <i class="fas fa-file-download" style="font-size:25px;color:#0F6FB5"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ver Documento">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="verFpp" CommandName="verFpp" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                                    <i class="fas fa-eye" style="font-size:23px;color:#0F6FB5"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                        <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                        <PagerStyle HorizontalAlign="Center" />
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
                        <!--Grid fpp-->
                        <!--descargar zip-->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="col-md-3">
                                    <asp:LinkButton ID="btnDescargarZip" CssClass="btn btn-sm btn-info" runat="server" OnClick="btnDescargarZip_Click">
                                Descargar ZIP
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="btnAprobarTodo" CssClass="btn btn-sm btn-success" runat="server" OnClick="btnAprobarTodo_Click" OnClientClick="return Acepta(this);">
                                Aprobar
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-5"></div>
                                <div class="col-md-3">
                                    <div style="width: 10px; height: 10px; background-color: white; border: 1px solid">
                                    </div>
                                    <asp:Label ID="Label11" runat="server" Text="Subido para revisión">
                                    </asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <div style="width: 10px; height: 10px; background-color: #FFA416; border: 1px solid">
                                    </div>
                                    <asp:Label ID="Label5" runat="server" Text="Devuelto">
                                    </asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <div style="width: 10px; height: 10px; background-color: #44D754; border: 1px solid">
                                    </div>
                                    <asp:Label ID="Label9" runat="server" Text="Aprobado">
                                    </asp:Label>
                                </div>
                            </div>

                        </div>

                        <%--<asp:Button ID="btnDescargarZip" runat="server" Text="Descargar ZIP" OnClick="btnDescargarZip_Click"/>--%>
                        <!--fin descargar zip-->
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <!--tercer tab-->
                <ajaxToolkit:TabPanel runat="server" HeaderText="Datos Informativos" ID="TabPanel3">
                    <ContentTemplate>
                        <!--encabezado empresa-->
                        <div class="well">
                            <div class="row">
                                <div class="col-md-1">
                                    <b>
                                        <asp:Label ID="lbl7" runat="server" Text="Empresa:"></asp:Label></b>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblnombreempresa" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">
                                    <b>
                                        <asp:Label ID="Label2" runat="server" Text="Encargado:"></asp:Label></b>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblnombreTutor" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="lbl5" runat="server" Text=" C.I Encargado:"></asp:Label></b>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lbltutorempresa" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <b>
                                        <asp:Label ID="Label3" runat="server" Text="Cargo:"></asp:Label></b>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblcargotutor" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <!--fin encabezado-->
                        <!--encabezado area de estudio-->
                        <br />
                        <div class="well">
                            <div class="row">
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="Label6" runat="server" Text="Campo Ámplio:"></asp:Label></b>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCampoAmplio" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="Label8" runat="server" Text="Campo Específico"></asp:Label></b>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCampoEspecifico" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <!--fin encabezado-->
                        <!--encabezado datos pasantía-->
                        <br />
                        <div class="well">
                            <div class="row">
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="Label4" runat="server" Text="Número de Horas:"></asp:Label></b>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblNumHoras" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="Label7" runat="server" Text="Fecha Inicio"></asp:Label></b>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFechaInicio" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <b>
                                        <asp:Label ID="Label10" runat="server" Text="Fecha Fin"></asp:Label></b>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFechaFin" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <!--fin encabezado-->
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <!--cuarto tab--->
                <ajaxToolkit:TabPanel runat="server" HeaderText="Prácticas del Estudiante" ID="TabPanel4">
                    <ContentTemplate>

                        <asp:Table runat="server" ID="grd_Valores" CssClass="table-responsive table table-bordered" Width="100%"></asp:Table>

                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <!--fin cuarto tab-->
            </ajaxToolkit:TabContainer>
        </div>

        <!--modal rechazo-->
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" Enabled="True" TargetControlID="btnPopUp3"
            BackgroundCssClass="modalBackground" PopupControlID="PanelModal3">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="PanelModal3" runat="server" Style="display: none; background: white; width: 40%; height: auto; border-radius: 2px; box-shadow: 0 0 25px #5D5C5C;" CssClass="animated fadeIn">
            <div class="modal-header">
            </div>
            <div class="modal-body">
                <asp:Label ID="Label1" runat="server" Text="">
                </asp:Label>
                <asp:TextBox ID="txtObservacion" TextMode="MultiLine" runat="server" CssClass="form-control" Height="100">
                </asp:TextBox>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnEnviarRechazo" runat="server" Text="Enviar" OnClick="btnEnviarRechazo_Click" CssClass="btn" OnClientClick="ShowProgress()" ValidationGroup="inicio" />
                <button class="btn" data-dismiss="cancel" aria-hidden="true">Cancelar</button>
            </div>
        </asp:Panel>
        <asp:Button ID="btnPopUp3" runat="server" Height="47px" Text="MOSTRAR POPUP"
            Width="258px" hidden="hidden" />
        <!--fin modal rechazo-->
        <!--modal Historial FPP-->
        <ajaxToolkit:ModalPopupExtender ID="ModalHistorialFPP" runat="server" Enabled="True" TargetControlID="btnPopUpHistorial"
            BackgroundCssClass="modalBackground" PopupControlID="PanelHistorial">
        </ajaxToolkit:ModalPopupExtender>

        <asp:Panel ID="PanelHistorial" runat="server" Style="display: none; background: white; width: 40%; height: auto; border-radius: 2px; box-shadow: 0 0 25px #5D5C5C;" CssClass="animated fadeIn">
            <div class="modal-header">
                <div class="row">
                    <div class="col-md-6">
                        <h4>
                            <asp:Label ID="lblHistorialFpp" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div class="col-md-6">
                        <asp:Button ID="btnclose" runat="server" Text="x" data-dismiss="modal" aria-hidden="true" class="close" Style="color: white; background-color: red; border: 1px solid red;" />

                    </div>

                </div>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="dgvHistorico" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:Panel ID="pnlHistorico" runat="server">
                                <asp:GridView ID="dgvHistorico" runat="server" DataKeyNames="IdHistoricoFpp,IdFppPasante,FechaRegistroHisfpp,ObservacionHisfpp,DescripcionEstadoFpp" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnPageIndexChanging="dgvHistorico_PageIndexChanging" PageSize="10">
                                    <Columns>
                                        <asp:BoundField DataField="IdHistoricoFpp" HeaderText="IdHistoricoFpp" SortExpression="IdHistoricoFpp" Visible="false" />
                                        <asp:BoundField DataField="IdFppPasante" HeaderText="IdFppPasanteIdFppPasante" SortExpression="IdFppPasante" Visible="false" />
                                        <asp:BoundField DataField="FechaRegistroHisfpp" HeaderText="Fecha Registro" SortExpression="FechaRegistroHisfpp" />
                                        <asp:BoundField DataField="ObservacionHisfpp" HeaderText="Observación" SortExpression="ObservacionHisfpp" />
                                        <asp:BoundField DataField="DescripcionEstadoFpp" HeaderText="Estado" SortExpression="DescripcionEstadoFpp" />
                                    </Columns>
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="#00000" />
                                    <PagerStyle HorizontalAlign="Center" />
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
            <div class="modal-footer">
            </div>
        </asp:Panel>

        <asp:Button ID="btnPopUpHistorial" runat="server" Height="47px" Text="MOSTRAR POPUP"
            Width="258px" hidden="hidden" />
        <!--fin modal Historial FPP-->


        <!--Modal ver formulario-->
        <ajaxToolkit:ModalPopupExtender ID="ModalVerFPP" runat="server" Enabled="True" TargetControlID="btnverinforme"
            BackgroundCssClass="modalBackground" PopupControlID="Panelverinforme">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Button ID="btnverinforme" runat="server" Text="MOSTRAR POPUP" hidden="hidden" />
        <asp:Panel ID="Panelverinforme" runat="server" Style="display: none; background: white; top: 120px; height: 90%; width: 80%; border: 0.5px solid #CBCBD3; border-radius: 5px; overflow-x: scroll; box-shadow: 0 0 25px #5D5C5C;">
            <div class="modal-header">
                <h4 style="display: inline-block"><b>
                    <asp:Label ID="lbltipoFpp" runat="server" Text=""></asp:Label>
                </b></h4>
                <asp:Button ID="btncloseinforme" runat="server" Text="x" data-dismiss="modal" aria-hidden="true" class="close" Style="color: white; background-color: red; border: 1px solid red;" />
            </div>
            <div class="modal-body" style="height: 100%; width: 100%;">
                <iframe runat="server" id="ifrminformes" style="border: none; width: 100%; height: 100%;"></iframe>
            </div>
        </asp:Panel>
        <!--fin modal ver formualrio-->


    </form>
</body>
</html>
