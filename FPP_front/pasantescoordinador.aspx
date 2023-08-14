<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pasantescoordinador.aspx.cs" Inherits="FPP_front.pasantescoordinador" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container animated fadeIn" style="width: 90%">
        <asp:HiddenField ID="hddPeriodo" runat="server" />
        <asp:HiddenField ID="hddIdentificacion" runat="server" />
        <asp:HiddenField ID="hddIdPasante" runat="server" />
        <div class="row well">
            <h4>Carga de Formularios de Pasantías</h4>
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
                    <asp:GridView ID="dgvPasante" runat="server" DataKeyNames="IdPasante,IdentificacionPasante,NombrePasante,ApellidoPasante,NombreEmpresa,IdentificacionTutorEmpresa,CarreraPasante,FacultadPasante,IdCampoEspecifico,NumeroHorasPasante,FechaInicioPasante,FechaFinPasante,CarpetaPasanteExpediente,CodCarreraPasante,PeriodoPasante,EmailTutor,EstadoAprobado" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvPasante_RowCommand" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvPasante_PageIndexChanging" OnRowDataBound="dgvPasante_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="NombreEmpresa" HeaderText="NombreEmpresa" SortExpression="NombreEmpresa" Visible="false" />
                            <asp:BoundField DataField="IdentificacionTutorEmpresa" HeaderText="IdentificacionTutorEmpresa" SortExpression="IdentificacionTutorEmpresa" Visible="false" />

                            <asp:BoundField DataField="IdPasante" HeaderText="IdPasante" SortExpression="IdPasante" Visible="false" />
                            <asp:BoundField DataField="IdCampoEspecifico" HeaderText="IdCampoEspecifico" SortExpression="IdCampoEspecifico" Visible="false" />
                            <asp:BoundField DataField="IdentificacionPasante" HeaderText="Identificación" SortExpression="IdentificacionPasante" />
                            <asp:BoundField DataField="NombrePasante" HeaderText="Nombres" SortExpression="NombrePasante" />
                            <asp:BoundField DataField="ApellidoPasante" HeaderText="Apellidos" SortExpression="ApellidoPasante" />
                            <asp:BoundField DataField="FechaRegistroPasante" HeaderText="Fecha Registro Pasante" SortExpression="FechaRegistroPasante" />
                            <asp:BoundField DataField="NumeroHorasPasante" HeaderText="Número Horas Pasantía" SortExpression="NumeroHorasPasante" />
                            <asp:BoundField DataField="FechaInicioPasante" HeaderText="Fecha Inicio Pasantía" SortExpression="FechaInicioPasante" />
                            <asp:BoundField DataField="FechaFinPasante" HeaderText="Fecha Fin Pasantía" SortExpression="FechaFinPasante" />

                            <asp:BoundField DataField="CarreraPasante" HeaderText="Carrera" SortExpression="CarreraPasante" />
                            <asp:BoundField DataField="CodCarreraPasante" HeaderText="Cod.Carrera" SortExpression="CodCarreraPasante"/>
                            <asp:BoundField DataField="CodFacultadPasante" HeaderText="Cod Facultad" SortExpression="CodFacultadPasante" Visible="false" />
                            <asp:BoundField DataField="ActivoPasante" HeaderText="ActivoPasante" SortExpression="ActivoPasante" Visible="false" />
                            <asp:BoundField DataField="FacultadPasante" HeaderText="Facultad" SortExpression="FacultadPasante" />
                            <asp:BoundField DataField="PeriodoPasante" HeaderText="Período" SortExpression="PeriodoPasante" />
                            <asp:BoundField DataField="NombreEmpresa" HeaderText="Empresa" SortExpression="NombreEmpresa" />
                            <asp:BoundField DataField="EmailTutor" HeaderText="Email" SortExpression="EmailTutor" Visible="false" />
                            <asp:BoundField DataField="EstadoAprobado" HeaderText="EstadoAprobado" SortExpression="EstadoAprobado" Visible="false" />
                            <asp:BoundField DataField="CarpetaPasanteExpediente" HeaderText="CarpetaExpediente" SortExpression="CarpetaPasanteExpediente" Visible="false" />
                            <asp:TemplateField HeaderText="Ver Detalle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="fppAlumno" runat="server" CommandName="fppAlumno" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                                            <i class="fas fa-eye" style="font-size:23px;color:#0F6FB5"></i>
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
        <!--Modal de tutorias-->
        <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server" Enabled="True" TargetControlID="btnPopUp"
            BackgroundCssClass="modalBackground" PopupControlID="PanelModal">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Button ID="btnPopUp" runat="server" Text="MOSTRAR POPUP" hidden="hidden" />
        <asp:Panel ID="PanelModal" runat="server" Style="display: none; background: white; height: 80%; width: 80%; border: solid 0.5px #CBCBD3; border-radius: 5px; overflow-x: scroll; box-shadow: 0 0 25px #5D5C5C;">
            <div class="modal-header">
                <h4 style="display: inline-block"><b>FPP Alumnos</b></h4>
                <asp:Button ID="btnclose" runat="server" Text="x" data-dismiss="modal" aria-hidden="true" class="close" Style="color: white; background-color: red; border: 1px solid red;" />
            </div>
            <div class="modal-body" style="height: 100%; width: 100%">
                <iframe runat="server" id="ifrm" style="border: none; width: 100%; height: 100%"></iframe>
            </div>
        </asp:Panel>
        <!--fin modal tutorias-->
    </div>
</asp:Content>
