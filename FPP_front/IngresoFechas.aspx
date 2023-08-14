<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngresoFechas.aspx.cs" Inherits="FPP_front.IngresoFechas" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*Checkbox*/
        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 20px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 12px;
                width: 12px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }


        /*fin checkbox*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h4 class="well" style="background-color: rgb(8, 83, 148); color: white; text-align: center">Ingreso de Fechas
            <asp:LinkButton ID="btnLimpiar" CssClass="btn btn-sm" Style="background: #DB8217; float: right; margin-left: 2px" runat="server"><i class="fa fa-brush"></i> Limpiar</asp:LinkButton>
            <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="background: #19A818; float: right" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save" ></i> Guardar</asp:LinkButton>
        </h4>
        <div class="col-lg-12 well">
            <div class="row">
                <div class="col-md-1 col-sm-offset-1">
                    <label>Período</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtPeriodo" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Tipo FPP</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlTipoFPP" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-1 col-sm-offset-1">
                    <label>Fecha Inicio</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtFechaInicio" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Fecha Fin</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtFechaFin" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-1">
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-1 col-sm-offset-1">
                    <label>Activo</label>
                </div>
                <div class="col-md-3">
                    <label class="switch">
                        <asp:CheckBox ID="chkActivo" runat="server" />
                        <span class="slider round"></span>
                    </label>
                </div>
            </div>
            <br />
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView ID="dgvFechas" runat="server" DataMember="Idfpp" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True">
                        <Columns>
                            <asp:BoundField DataField="Idfpp" HeaderText="ID_FECHA" SortExpression="idfpp" Visible="false" />
                            <asp:BoundField DataField="Descfpp" HeaderText="Período" SortExpression="descfpp" />
                            <asp:BoundField DataField="Desctipofpp" HeaderText="Tipo Fpp" SortExpression="Desctipofpp" />
                            <asp:BoundField DataField="fechainiciofpp" HeaderText="Fecha Inicio" SortExpression="fechainiciofpp" />
                            <asp:BoundField DataField="fechafinfpp" HeaderText="Fecha Fin" SortExpression="fechafinfpp" />
                            <asp:TemplateField HeaderText="acciones">
                                <ItemTemplate>
                                    <asp:Button CommandName="Delete" runat="server" Text="Eliminar" OnClientClick="return  confirmdelete(this);" />
                                </ItemTemplate>
                                <ControlStyle BackColor="#CC0000" BorderStyle="Outset" CssClass="btn btn-sm" ForeColor="White" />
                            </asp:TemplateField>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <script type="text/javascript">

        $(document).ready(function () {
            var d = new Date();
            var fecha = d.format("yyyy-MM-dd");
            $("#<%=txtFechaInicio.ClientID%>").val(fecha);
            $("#<%=txtFechaFin.ClientID%>").val(fecha);
        });


        function confirm() {
            Swal.fire({
                icon: 'success',
                title: 'OK',
                text: 'El Registro se Guardo Corectamente!',
                footer: '<a href></a>'
            })
        }
        var object = { status: false, ele: null };
        function confirmdelete(ev) {

            if (object.status) { return true; }
            Swal.fire({
                title: 'Está Seguro de Eliminar el Registro?',
                text: "Usted no sera capaz de revertir esta acción!",
                icon: 'Precaucion',
                showCancelButton: true,
                confirmButtonColor: '#29CB12',
                cancelButtonColor: '#d33',
                confirmButtonText: 'SI',
                cancelButtonText: 'NO'
            }).then((result) => {
                if (result.isConfirmed) {
                    object.status = true;
                    object.ele = ev;
                    object.ele.click();
                }
            })
            return false;
        }
    </script>

</asp:Content>
