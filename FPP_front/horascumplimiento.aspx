<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="horascumplimiento.aspx.cs" Inherits="FPP_front.horascumplimiento" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container animated fadeIn" style="width: 90%">
        <div class="row well">
            <div class="col-md-10">
                <h4>Horas de Cumplimiento</h4>
            </div>
            <div class="col-md-2">
                <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; float: right; color: white" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
            </div>
        </div>

        <div class="row well">
            <div class="row ">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Facultad"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlFacultad" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlFacultad_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="--Seleccione una opción--"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" Text="Área"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="--Seleccione una opción--"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label7" runat="server" Text="Carrera"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCarrera" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="--Seleccione una opción--"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <div class="col-md-2 ">
                        <asp:Label ID="Label4" runat="server" Text="Horas Cumplimiento"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtHorasCumplimiento" runat="server" CssClass="form-control" onkeypress="return soloNumeros(event);"></asp:TextBox>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label5" runat="server" Text="Fecha Inicio"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label6" runat="server" Text="Fecha Fin"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="row" style="display: none">
                <div class="col-md-1 col-sm-offset-1">
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
        <div class="row well">
            <div class="row">
                <div class="col-md-2">
                    <label>Facultad/Carrera</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnBusqueda" Style="background: #19A818; color: white" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="dgvProfundidad" runat="server" DataKeyNames="IdModalidad,IdParametro" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvProfundidad_PageIndexChanging" OnRowUpdating="dgvProfundidad_RowUpdating" OnRowCancelingEdit="dgvProfundidad_RowCancelingEdit" OnRowEditing="dgvProfundidad_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="IdModalidad" HeaderText="ID" SortExpression="IdModalidad" Visible="false" />
                        <asp:BoundField DataField="NombreFacultad" HeaderText="Nombre Facultad" SortExpression="NombreFacultad" ReadOnly="true" />
                        <asp:BoundField DataField="Facultad" HeaderText="Cód Facultad" SortExpression="Facultad" ReadOnly="true" />
                        <asp:BoundField DataField="NombreCarrera" HeaderText="Nombre Carrera" SortExpression="NombreCarrera" ReadOnly="true" />
                        <asp:BoundField DataField="Carrera" HeaderText="Cód Carrera" SortExpression="Carrera" ReadOnly="true" />
                        <asp:BoundField DataField="IdParametro" HeaderText="IDParametro" SortExpression="IdParametro" Visible="false" />
                        <asp:BoundField DataField="FechaInicioParametro" HeaderText="Fecha Inicio Parametro" SortExpression="FechaInicioParametro" Visible="false" />
                        <asp:BoundField DataField="FechaFinParametro" HeaderText="Fecha Fin Parametro" SortExpression="FechaFinParametro" Visible="false" />
                        <asp:BoundField DataField="ActivoParametro" HeaderText="Activo Parametro" SortExpression="ActivoParametro" Visible="false" />
                        <asp:TemplateField HeaderText="Horas de Cumplimiento">
                            <EditItemTemplate>
                                <asp:TextBox ID="EditHorasCumplimiento" runat="server" Text='<%# Bind("MaxHorasParametro") %>' onkeypress="return soloNumeros(event);"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("MaxHorasParametro") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" Checked='<%#Eval("Activo")%>' onclick="this.checked=!this.checked;" ReadOnly="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" ReadOnly="true" />
                        <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" ReadOnly="true" />
                        <asp:CommandField ButtonType="Button" ShowEditButton="True">
                            <ControlStyle BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-xs btn-outline-primary" ForeColor="black" />
                        </asp:CommandField>

                    </Columns>
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" ForeColor="#003399" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            var d = new Date();
            var fecha = d.format("yyyy-MM-dd");
            $("#<%=txtFechaInicio.ClientID%>").val(fecha);
            $("#<%=txtFechaFin.ClientID%>").val(fecha);
        });

    </script>
</asp:Content>
