<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="plantillasfpp.aspx.cs" Inherits="FPP_front.plantillasfpp" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <%--<script src="Scripts/funciones.js" type="text/javascript"></script>--%>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
    <style>
        .select2 {
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container animated fadeIn" style="width: 90%">
        <div class="row well">
            <div class="col-md-10">
                <h4>Plantillas</h4>
            </div>
            <div class="col-md-2">
                <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; color: white; float: right" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
            </div>
        </div>

        <div class="row well">
            <div class="row">
                <div class="col-md-2 ">
                    <b>
                        <asp:Label ID="Label1" runat="server" Text="Nombre Plantilla"></asp:Label></b>
                </div>
                <div class="col-md-4">
                    <%--<asp:TextBox ID="txtNombrePlantilla" CssClass="form-control" runat="server" onkeyup="mayus(this);"></asp:TextBox>--%>
                    <asp:DropDownList ID="ddlNombrePlantilla" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                        <asp:ListItem Text="FPP1" Value="FPP1"></asp:ListItem>
                        <asp:ListItem Text="FPP2" Value="FPP2"></asp:ListItem>
                        <asp:ListItem Text="FPP3" Value="FPP3"></asp:ListItem>
                        <asp:ListItem Text="FPP4" Value="FPP4"></asp:ListItem>
                        <asp:ListItem Text="FPP5" Value="FPP5"></asp:ListItem>
                        <asp:ListItem Text="FPP6" Value="FPP6"></asp:ListItem>
                        <asp:ListItem Text="FPP7" Value="FPP7"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <b>
                        <asp:Label ID="Label2" runat="server" Text="Descripción Plantilla"></asp:Label></b>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtdescPlantilla" CssClass="form-control" runat="server" TextMode="MultiLine" required="true"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <b>
                        <asp:Label ID="Label5" runat="server" Text="Regla"></asp:Label></b>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlRegla" runat="server" CssClass="form-control estilo_select" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Seleccionar--" Value="0">                          
                        </asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <b>
                        <asp:Label ID="Label4" runat="server" Text="Archivo"></asp:Label></b>
                </div>
                <div class="col-md-4">

                    <asp:FileUpload ID="file" runat="server" CssClass="form-control" />
                </div>



            </div>
            <br />
            <div class="row">

                <div style="display: none">
                    <div class="col-md-1">
                        <b>
                            <asp:Label ID="Label3" runat="server" Text="Activo"></asp:Label></b>
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
                    <label>Nombre/Descripción</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control" onkeyup="mayus(this);"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnBusqueda" Style="background: #19A818; color: white;" runat="server" CssClass="btn btn-sm btn-success" OnClick="btnBusqueda_Click">Buscar</asp:LinkButton>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="dgvPlantillas" runat="server" DataKeyNames="idPlantilla,pathPlatilla" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnRowCommand="dgvPlantillas_RowCommand" OnPageIndexChanging="dgvPlantillas_PageIndexChanging" OnRowDataBound="dgvPlantillas_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="idPlantilla" HeaderText="ID" SortExpression="idPlantilla" Visible="false" />
                        <asp:BoundField DataField="nomrePlantilla" HeaderText="Nombre" SortExpression="nomrePlantilla" />
                        <asp:BoundField DataField="descripcionPlantilla" HeaderText="Descripción" SortExpression="descripcionPlantilla" />
                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" runat="server" Checked='<%#Eval("activoPlantilla")%>' onclick="this.checked=!this.checked;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="pathPlatilla" HeaderText="Path" SortExpression="pathPlatilla" />
                        <asp:BoundField DataField="nombreFacultad" HeaderText="Facultad" SortExpression="nombreFacultad" />
                        <asp:BoundField DataField="nombreCarrera" HeaderText="Carrera" SortExpression="nombreCarrera" />
                        <asp:TemplateField HeaderText="Descargar Plantilla">
                            <ItemTemplate>
                                <asp:LinkButton ID="descargarInforme" CommandName="descargarInforme" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"><i class="fas fa-file-download" style="font-size:25px;color:#0F6FB5"></i></asp:LinkButton>
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
            </div>
        </div>
    </div>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
    <script>
        //select con buscador
        $(function () {
            $("[id*=ddlRegla]").select2();
            //estilo select2

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
                    $("[id*=ddlRegla]").select2();

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
