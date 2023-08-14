<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cargafpp.aspx.cs" Inherits="FPP_front.cargaFPP" %>

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
    <section class="design-process-section" id="process-tab">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <!-- design process steps-->
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs process-model more-icon-preocess" role="tablist">
                        <li role="presentation" class="active"><a href="#fpp1" aria-controls="fpp1" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP1</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp2" aria-controls="fpp2" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP2</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp3" aria-controls="fpp3" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP3</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp4" aria-controls="fpp4" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP4</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp5" aria-controls="reporting" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP5</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp6" aria-controls="fpp6" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP6</i>
                        </a></li>
                        <li role="presentation"><a href="#fpp7" aria-controls="fpp7" role="tab" data-toggle="tab"><i class="" aria-hidden="true">FPP7</i>
                        </a></li>
                    </ul>
                    <!-- end design process steps-->
                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="fpp1">
                            <div class="design-process-content">
                                <h3 class="semi-bold">Cargar Archivo FPP1</h3>
                                <asp:FileUpload ID="fileUploadFpp1" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp1" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp2">
                            <div class="design-process-content">
                                <h3 class="semi-bold">Cargar Archivo FPP2</h3>
                                <asp:FileUpload ID="fileUploadFpp2" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp2" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp3">
                            <div class="design-process-content">
                                <h3 class="semi-bold">Cargar Archivo FPP3</h3>
                                <asp:FileUpload ID="fileUploadFpp3" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp3" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp4">
                            <div class="design-process-content">
                                <h3 class="semi-bold">Cargar Archivo FPP4</h3>
                                <asp:FileUpload ID="fileUploadFpp4" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp4" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp5">
                            <div class="design-process-content">
                                <h3>Cargar Archivo FPP5</h3>
                                <asp:FileUpload ID="fileUploadFpp5" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp5" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp6">
                            <div class="design-process-content">
                                <h3>Cargar Archivo FPP6</h3>
                                <asp:FileUpload ID="fileUploadFpp6" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp6" runat="server" Text="Subir Documento"  CssClass="btn btn-sm btn-success" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="fpp7">
                            <div class="design-process-content">
                                <h3>Cargar Archivo FPP7</h3>
                                <asp:FileUpload ID="fileUploadFpp7" runat="server" />
                                <br />
                                <asp:Button ID="btnFpp7" runat="server" Text="Subir Documento" CssClass="btn btn-sm btn-success" />
                            </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </section>
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
    </script>
</asp:Content>
