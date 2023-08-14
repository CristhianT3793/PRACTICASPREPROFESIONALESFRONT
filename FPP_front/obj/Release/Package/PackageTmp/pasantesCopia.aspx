<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pasantesCopia.aspx.cs" Inherits="FPP_front.pasantesCopia" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .design-process-section .text-align-center {
            line-height: 25px;
            margin-bottom: 12px;
        }

        .design-process-content {
            border: 1px solid #e9e9e9;
            position: relative;
            padding: 16px 34% 30px 30px;
        }

            .design-process-content img {
                position: absolute;
                top: 0;
                right: 0;
                bottom: 0;
                z-index: 0;
                max-height: 100%;
            }

            .design-process-content h3 {
                margin-bottom: 16px;
            }

            .design-process-content p {
                line-height: 26px;
                margin-bottom: 12px;
            }

        .process-model {
            list-style: none;
            padding: 0;
            position: relative;
            max-width: 850px;
            margin: 20px auto 26px;
            border: none;
            z-index: 0;
        }

            .process-model li::after {
                background: #e5e5e5 none repeat scroll 0 0;
                bottom: 0;
                content: "";
                display: block;
                height: 4px;
                margin: 0 auto;
                position: absolute;
                right: -30px;
                top: 33px;
                width: 100%;
                z-index: -1;
            }

            .process-model li.visited::after {
                background: #57b87b;
            }

            .process-model li:last-child::after {
                width: 0;
            }

            .process-model li {
                display: inline-block;
                width: 10%;
                text-align: center;
                float: none;
            }

                .nav-tabs.process-model > li.active > a, .nav-tabs.process-model > li.active > a:hover, .nav-tabs.process-model > li.active > a:focus, .process-model li a:hover, .process-model li a:focus {
                    border: none;
                    background: transparent;
                }


                .process-model li a {
                    padding: 0;
                    border: none;
                    color: #606060;
                }

                .process-model li.active,
                .process-model li.visited {
                    color: #57b87b;
                }



                .process-model li i {
                    display: block;
                    height: 78px;
                    width: 78px;
                    text-align: center;
                    margin: 0 auto;
                    background: #f5f6f7;
                    border: 2px solid #e5e5e5;
                    line-height: 65px;
                    font-size: 12px;
                    border-radius: 50%;
                }

                .process-model li.active i, .process-model li.visited i {
                    background: #fff;
                    border-color: #57b87b;
                }

                .process-model li p {
                    font-size: 12px;
                    margin-top: 11px;
                }

            .process-model.contact-us-tab li.visited a, .process-model.contact-us-tab li.visited p {
                color: #606060 !important;
                font-weight: normal
            }

            .process-model.contact-us-tab li::after {
                display: none;
            }

            .process-model.contact-us-tab li.visited i {
                border-color: #e5e5e5;
            }



        @media screen and (max-width: 560px) {
            .more-icon-preocess.process-model li span {
                font-size: 12px;
                height: 60px;
                line-height: 46px;
                width: 70px;
            }

            .more-icon-preocess.process-model li::after {
                top: 24px;
            }
        }

        @media screen and (max-width: 380px) {
            .process-model.more-icon-preocess li {
                width: 18%;
            }

            .more-icon-preocess.process-model li span {
                font-size: 12px;
                height: 45px;
                line-height: 32px;
                width: 45px;
            }

            .more-icon-preocess.process-model li p {
                font-size: 5px;
            }

            .more-icon-preocess.process-model li::after {
                top: 18px;
            }

            .process-model.more-icon-preocess {
                text-align: center;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h4 class="well" style="background-color: rgb(8, 83, 148); color: white; text-align: center">Consulta Pasantes
        </h4>
        <div class="col-lg-12 well">
            <div class="row">
                <div class="col-sm-2">
                    <asp:Label ID="Label2" runat="server" Text="Identificación"></asp:Label>
                    <asp:TextBox ID="txtIdentificación" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="table-responsive">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                    <asp:GridView ID="dgvPasante" runat="server" DataKeyNames="IdPasante,IdentificacionPasante" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvPasante_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="IdPasante" HeaderText="IdPasante" SortExpression="IdPasante" Visible="false" />
                            <asp:BoundField DataField="IdCampoEspecifico" HeaderText="IdCampoEspecifico" SortExpression="IdCampoEspecifico" />
                            <asp:BoundField DataField="IdentificacionPasante" HeaderText="IdentificacionPasante" SortExpression="IdentificacionPasante" />
                            <asp:BoundField DataField="NombrePasante" HeaderText="NombrePasante" SortExpression="NombrePasante" />
                            <asp:BoundField DataField="ApellidoPasante" HeaderText="ApellidoPasante" SortExpression="ApellidoPasante" />
                            <asp:BoundField DataField="FechaRegistroPasante" HeaderText="FechaRegistroPasante" SortExpression="FechaRegistroPasante" />
                            <asp:BoundField DataField="NumeroHorasPasante" HeaderText="NumeroHorasPasante" SortExpression="NumeroHorasPasante" />
                            <asp:BoundField DataField="FechaInicioPasante" HeaderText="FechaInicioPasante" SortExpression="FechaInicioPasante" />
                            <asp:BoundField DataField="FechaFinPasante" HeaderText="FechaFinPasante" SortExpression="FechaFinPasante" />
                            <asp:BoundField DataField="CarreraPasante" HeaderText="CarreraPasante" SortExpression="CarreraPasante" />
                            <asp:BoundField DataField="ActivoPasante" HeaderText="ActivoPasante" SortExpression="ActivoPasante" />
                            <asp:BoundField DataField="FacultadPasante" HeaderText="FacultadPasante" SortExpression="FacultadPasante" />
                            <asp:BoundField DataField="PeriodoPasante" HeaderText="PeriodoPasante" SortExpression="PeriodoPasante" />

                            <asp:ButtonField ButtonType="Button" Text="Ver" CommandName="fppAlumno">
                                <ControlStyle BackColor="#085394" BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-sm" ForeColor="White" />
                            </asp:ButtonField>
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
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hddPeriodo" runat="server" />
        <asp:HiddenField ID="hddIdentificacion" runat="server" />
        <asp:HiddenField ID="hddIdPasante" runat="server" />
        <!--modal 2-->

        <%--<Triggers>--%>
        <%--<asp:PostBackTrigger ControlID="upModal"/>--%>
        <%--                <asp:AsyncPostBackTrigger ControlID="upModal"/>
            </Triggers>--%>

        <!-- Bootstrap Modal Dialog -->

        <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="width: 100%">
            <div class="modal-dialog" style="width: 80%">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">FPPS</h4>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                            <div class="col-lg-12" style="padding-left: 0px; padding-right: 0px">
                                <div class="table-responsive">
                                    <%--                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>--%>
                                    <asp:GridView ID="dgvFppsAlumno" runat="server" DataKeyNames="IdfppAlumno,Idestudiante,Desctipofpp" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" OnRowCommand="dgvFppsAlumno_RowCommand" PageSize="10" OnPageIndexChanging="dgvFppsAlumno_PageIndexChanging" OnRowDataBound="dgvFppsAlumno_RowDataBound" EnableViewState="true" OnRowUpdating="dgvFppsAlumno_RowUpdating" OnRowEditing="dgvFppsAlumno_RowEditing">
                                        <Columns>
                                            <asp:BoundField DataField="IdfppAlumno" HeaderText="IdfppAlumno" SortExpression="IdfppAlumno" Visible="false" />
                                            <asp:BoundField DataField="Idfpp" HeaderText="Idfpp" SortExpression="Idfpp" Visible="false" />
                                            <asp:BoundField DataField="Idestudiante" HeaderText="Idestudiante" SortExpression="Idestudiante" Visible="false" />
                                            <asp:BoundField DataField="Desctipofpp" HeaderText="Tipo Fpp" SortExpression="Desctipofpp" />
                                            <asp:BoundField DataField="Descfpp" HeaderText="Período" SortExpression="Descfpp" />
                                            <asp:BoundField DataField="fechainiciofpp" HeaderText="Fecha Inicio Fpp" SortExpression="fechainiciofpp" />
                                            <asp:BoundField DataField="fechafinfpp" HeaderText="Fecha Fin Fpp" SortExpression="fechafinfpp" />
                                            <asp:BoundField DataField="FpparchivourlAlumno" HeaderText="Archivo" SortExpression="FpparchivourlAlumno" />
                                            <asp:BoundField DataField="Idestadofpp" HeaderText="Estado" SortExpression="Idestadofpp" Visible="false" />
                                            <asp:BoundField DataField="DescEstadofpp" HeaderText="Estado " SortExpression="DescEstadofpp" />
                                            <asp:BoundField DataField="Fechasubidaarchivo" HeaderText="Fec Subida Archivo" SortExpression="Fechasubidaarchivo" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEstadoFpp" runat="server" Text='<%# Eval("Idestadofpp") %>' Visible="false" />
                                                    <asp:DropDownList ID="ddlEstadosFpp" runat="server">
                                                        <%--<asp:ListItem Text="-Seleccione una opción-" Value="0"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Button" Text="Descargar Archivo" CommandName="dowloadfppAlumno">
                                                <ControlStyle BackColor="#085394" BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-sm" ForeColor="White" />
                                            </asp:ButtonField>
                                            <asp:ButtonField ButtonType="Button" Text="Enviar" CommandName="Enviar">
                                                <ControlStyle BackColor="#085394" BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-sm" ForeColor="White" />
                                            </asp:ButtonField>
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
                                    <%--                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>
                            <div class="modal-footer" style="padding-left: 0px; padding-right: 0px">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <!--modal rechazo-->

                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" Enabled="True" TargetControlID="btnPopUp3"
                    BackgroundCssClass="modalBackground" PopupControlID="PanelModal3">
                </ajaxToolkit:ModalPopupExtender>
                <!--fin boton abrir modal-->
                <asp:Panel ID="PanelModal3" runat="server" Style="display: none; background: white; width: 20%; height: auto; border: solid 1px black; border-radius: 2px">
                    <div class="modal-header">
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="Label1" runat="server" Text="Motivo de Rechazo">
                        </asp:Label>
                        <asp:TextBox ID="txtpanel" TextMode="MultiLine" runat="server" CssClass="form-control">

                        </asp:TextBox>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnEnviarRechazo" runat="server" Text="Enviar" OnClick="btnEnviarRechazo_Click"/>
                        <button class="btn" data-dismiss="cancel" aria-hidden="true">Cancelar</button>
                    </div>
                </asp:Panel>
                <asp:Button ID="btnPopUp3" runat="server" Height="47px" Text="MOSTRAR POPUP"
                    Width="258px" hidden="hidden" />
                <!--fin modal rechazo-->
    </div>
    <script>
        // script for tab steps
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var href = $(e.target).attr('href');
            var $curr = $(".process-model  a[href='" + href + "']").parent();
            $('.process-model li').removeClass();
            $curr.addClass("active");
            $curr.prevAll().addClass("visited");
        });
        // end  script for tab steps
        //function confirm(texto) {
        //    Swal.fire({
        //        icon: 'success',
        //        title: 'OK',
        //        text: texto,
        //    })
        //}


        function enviaPaso1(accion) {

            if (accion == "Next") {
                var answer = confirm("¿Esta seguro que desea actualizar el estado de esta solicitud?");

                if (answer) {

                } else {
                    alert("La actualizacion para esta convalidacion ha sido cancelada.");
                    location.reload(true);
                }
            }
        }



    </script>
</asp:Content>
