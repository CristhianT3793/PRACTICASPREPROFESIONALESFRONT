<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitudEstudiante.aspx.cs" Inherits="FPP_front.SolicitudEstudiante" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
    <script src="Scripts/funciones.js" type="text/javascript"></script>


    <style>
        .loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('../images/loader.gif') 50% 50% no-repeat rgb(249,249,249);
            opacity: .8;
        }

        .select2 {
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="loader"></div>
    <div class="container  animated fadeIn ">

        <div class="row well">
            <h4 style="text-align: center">Solicitud Alumno</h4>
        </div>

        <div class="row well">
            <div class="row">
                <div class="col-md-12">
                    <h4>Datos Alumno</h4>

                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Período</label>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group col-sm-6">
                            <label>Identificación</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtIdentificacionEstudiante" onblur="Buscar_Estudiante()" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                            <asp:Label ID="lblCedula_v" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <label>Nombres</label>
                            <asp:TextBox class="form-control" runat="server" ID="txtNombresEstudiante" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="lblnombres_v" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                        <div class="col-sm-6 form-group">
                            <label>Apellidos</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtApellidosEstudiante" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="lblApellidos_v" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Facultad</label>
                            <asp:TextBox ID="txtFacultad" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group col-sm-6">
                            <label>Carrera</label>
                            <asp:TextBox ID="txtCarrera" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="display: none">
                        <div class="form-group col-sm-6">
                            <label>Cod Carrera</label>
                            <asp:TextBox ID="txtCodCarrera" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group col-sm-6">
                            <label>Cod Facultad</label>
                            <asp:TextBox ID="txtCodFacultad" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                    </div>


                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <label>Fecha de inicio</label>
                            <asp:TextBox class="form-control" runat="server" ID="txtFechaInicio" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-sm-6 form-group">
                            <label>Fecha de Finalización</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtFechaFin" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Número de Horas Pasantía</label>
                            <asp:TextBox class="form-control" runat="server" ID="txtNumeroHoras" onkeypress="return soloNumeros(event);"></asp:TextBox>
                        </div>
                    </div>
                    <!--Areas de estudio-->
                    <h4>Área de Estudio</h4>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label>Campo Amplio</label>
                                    <asp:DropDownList ID="ddlCampoAmplio" runat="server" CssClass="form-control estilo_select" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCampoAmplio_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label>Campo Específico</label>
                                    <asp:DropDownList ID="ddlCampoEspecifico" runat="server" CssClass="form-control estilo_select" AppendDataBoundItems="true">
                                        <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!--fin areas de estudio-->
                    <!--datos profesor-->
                    <h4>Datos Profesor</h4>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Identificación</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtIdentificacionProfesor" onblur="Buscar_Docente()"></asp:TextBox>
                            <asp:Label ID="Label1" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                        <div class="form-group col-sm-6" style="display: none">
                            <label>Carrera</label>
                            <%--<asp:DropDownList ID="ddlCarreraP" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                            <asp:TextBox ID="txtCarreraP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                    </div>
                    <div class="row" style="display: none">
                        <%--<div class="form-group col-sm-6">
                            <label>Facultad</label>--%>
                        <%--<asp:DropDownList ID="ddlFacultadP" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                        <%--<asp:TextBox ID="txtFacultadP" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>--%>
                        <asp:TextBox class="form-control" runat="server" ID="txtEmailP"></asp:TextBox>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <label>Nombres</label>
                            <asp:TextBox class="form-control" runat="server" ID="txtnombreDocente" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="Label4" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                        <div class="col-sm-6 form-group">
                            <label>Apellidos</label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtapellidoDocente" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="Label5" Style="width: 100%; color: red; border: hidden" runat="server" Text=""></asp:Label>

                        </div>
                    </div>
                    <!--fin datosprofesor-->

                    <!--datos empresa-->
                    <h4>Datos Empresa</h4>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="form-group col-sm-6">
                                    <label>Empresa</label>
                                    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control estilo_select" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                                        <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label>Encargado</label>
                                    <asp:DropDownList ID="ddlEncargado" runat="server" CssClass="form-control estilo_select" AutoPostBack="true">
                                        <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <!--fin datos empresa-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--tipo pasantía-->
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h4>Reconocimiento Laboral</h4>
                            <div class="row">

                                <div class="form-group col-sm-6">
                                    <label class="switch">
                                        <asp:CheckBox ID="chkReconocimiento" runat="server" OnCheckedChanged="chkReconocimiento_CheckedChanged" AutoPostBack="true" ClientIDMode="Static"/>
                                        <span class="slider round"></span>
                                    </label>
                                </div>

                            </div>
                            <!--fin tipo pasantía-->

                            <!--datos convenio-->
                            <asp:Panel ID="pnlConvenios" runat="server">
                                <h4>Convenio</h4>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="row">4
                                            <div class="form-group col-sm-6">
                                                <label>Convenio</label>
                                                <asp:DropDownList ID="ddlconvenio" runat="server" CssClass="form-control estilo_select" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlconvenio_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <%--<asp:LinkButton ID="btnVerConvenio" runat="server">Ver Convenio</asp:LinkButton>--%>
                                            </div>
                                            <div class="form-group col-sm-6">
                                                <label>Descripción Convenio</label>
                                                <asp:TextBox ID="txtDescConvenio" runat="server" TextMode="MultiLine" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group col-sm-6">
                                                <label>F Inicio</label>
                                                <asp:TextBox ID="txtFechaInicioConvenio" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group col-sm-6">
                                                <label>F Fin</label>
                                                <asp:TextBox ID="txtFechaFinConvenio" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!--fin datos convenio-->
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!--buscar empresa-->
                    <%--<asp:DropDownList ID="ddlempresa_" runat="server" CssClass="form-control "></asp:DropDownList>--%>
                    <!--fin buscar empresa-->
                    <div class="form-group">
                        <asp:Button ID="btnSave" class="btn btn-success" runat="server" Text="Guardar" OnClick="btnSave_Click" />
                    </div>
                    <asp:HiddenField ID="correcto" runat="server" />
                </div>
            </div>
        </div>
        <!--crerar un update prpgress-->
    </div>

    <br />
    <br />
    <br />
    <br />
    <br />
    <script>
        function Buscar_Docente() {
            var cedula = $('#<%=txtIdentificacionProfesor.ClientID%>').val();
            if (cedula != "") {
                $.ajax({
                    type: "POST",
                    url: 'WSFunciones.asmx/BuscarCoordinador',
                    contentType: "application/json; charset=utf-8",
                    data: "{ cedula: '" + cedula + "' }",
                    dataType: "json",
                    beforeSend: function () {
                        $(".loader").fadeIn("slow");
                    },
                    success: function (response, status) {
                        var resp = response.d;
                        $(".loader").fadeOut("slow");
                        if (resp != "") {
                            $('#<%=txtnombreDocente.ClientID%>').val(resp[0]);
                            $('#<%=txtapellidoDocente.ClientID%>').val(resp[1]);
                            $('#<%=txtCarreraP.ClientID%>').val(resp[2]);
                            $('#<%=txtEmailP.ClientID%>').val(resp[3]);
                        }
                        else {
                            alert("La cédula ingresada no existe como docente.");
                            $('#<%=txtIdentificacionProfesor.ClientID%>').val("");
                            $('#<%=txtnombreDocente.ClientID%>').val("");
                            $('#<%=txtapellidoDocente.ClientID%>').val("");
                            $('#<%=txtCarreraP.ClientID%>').val("");
                            $('#<%=txtEmailP.ClientID%>').val("");

                        }
                    }, statusCode: {
                        404: function (content) { alert('cannot find resource'); },
                        500: function (content) { alert('internal server error'); }
                    },
                    error: function (xhr, status, error) {
                        alert("Error ");
                        $(".loader").fadeOut("slow");
                    }
                });
            }

        };

        function Buscar_Estudiante() {
            var cedula = $('#<%=txtIdentificacionEstudiante.ClientID%>').val();
            //var anio = document.getElementById("hd_anio").value;
            if (cedula != "") {
                $.ajax({
                    type: "POST",
                    url: 'WSFunciones.asmx/BuscaEstudiante',
                    contentType: "application/json; charset=utf-8",
                    //data: "{ cedula: '" + cedula + "',anio: '" + anio + "' }",
                    data: "{ cedula: '" + cedula + "'}",
                    dataType: "json",
                    beforeSend: function () {
                        $(".loader").fadeIn("slow");
                    },
                    success: function (response, status) {
                        var resp = response.d;
                        $(".loader").fadeOut("slow");
                        if (resp != "" && resp[0] != "No Existe") {
                            $('#<%=txtFacultad.ClientID%>').val(resp[1]);
                            $('#<%=txtNombresEstudiante.ClientID%>').val(resp[4]);
                            $('#<%=txtApellidosEstudiante.ClientID%>').val(resp[5]);
                            $('#<%=txtCodCarrera.ClientID%>').val(resp[6]);
                            $('#<%=txtCarrera.ClientID%>').val(resp[7]);
                            $('#<%=txtCodFacultad.ClientID%>').val(resp[8]);
                            //extraer profesores automaticamente
<%--                            if ($('#<%=txtCodCarrera.ClientID%>').val(resp[6]) != null || $('#<%=txtCodCarrera.ClientID%>').val(resp[6]) != "") {
                                alert($('#<%=txtCodCarrera.ClientID%>').val());
                            }--%>
                        }
                        else {
                            alert("La cédula ingresada no existe como alumno.");
                            $('#<%=txtIdentificacionEstudiante.ClientID%>').val("");
                            $('#<%=txtNombresEstudiante.ClientID%>').val("");
                            $('#<%=txtFacultad.ClientID%>').val("");
                            $('#<%=txtCarrera.ClientID%>').val("");
                            $('#<%=txtApellidosEstudiante.ClientID%>').val("");
                            $('#<%=txtCodCarrera.ClientID%>').val("");
                        }
                    }, statusCode: {
                        404: function (content) { alert('cannot find resource'); },
                        500: function (content) { alert('internal server error'); }
                    },
                    error: function (xhr, status, error) {
                        alert("Error ");
                        $(".loader").fadeOut("slow");
                    }
                });
            }

        };

        $(document).ready(function () {
            //textbox solo de lectura

            var fecha = new Date();
            var formato = fecha.format('yyyy-MM-dd');
            $('#<%=txtFechaInicio.ClientID%>').val(formato);
            $('#<%=txtFechaFin.ClientID%>').val(formato);
        });

        function confirm(texto) {
            Swal.fire({
                icon: 'success',
                title: 'OK',
                text: texto,
            })
        }
        function error(texto) {
            Swal.fire({
                icon: 'error',
                title: 'error',
                text: texto,
            })
        }
        //loader
        $(window).on('load', function () {
            $(".loader").fadeOut("slow");
        });
        //select con buscador
        $(function () {
            $("[id*=ddlEmpresa]").select2();
            $("[id*=ddlEncargado]").select2();
            $("[id*=ddlCampoAmplio]").select2();
            $("[id*=ddlCampoEspecifico]").select2();
            $("[id*=ddlconvenio]").select2();
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
                    $("[id*=ddlEmpresa]").select2();
                    $("[id*=ddlEncargado]").select2();
                    $("[id*=ddlCampoAmplio]").select2();
                    $("[id*=ddlCampoEspecifico]").select2();
                    $("[id*=ddlconvenio]").select2();
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

