<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="areasestudio.aspx.cs" Inherits="FPP_front.areasestudio" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Style/EstiloAnimaciones.css" type="text/css" rel="stylesheet" />
    <link href="Style/EstiloPPP.css" type="text/css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.5/dist/sweetalert2.all.min.js" type="text/javascript"></script>
    <script src="Scripts/alertas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="width: 80%">
        <div class="row well">
            <div class="col-md-10">
                <h4>Áreas de Estudio</h4>
            </div>
        </div>

        <div class="row well">
            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                <!--primer tab-->
                <ajaxToolkit:TabPanel runat="server" HeaderText="Campo Amplio" ID="TabPanel1">
                    <ContentTemplate>
                        <div class="row">
                            <asp:LinkButton ID="btnGuardar" CssClass="btn btn-sm" Style="float: right; color: white; margin-right: 15px; background: #19A818;" runat="server" OnClick="btnGuardar_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-5">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label1" runat="server" Text="Campo Amplio"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtCampoAmplio" CssClass="form-control" runat="server" onkeyup="mayus(this)"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label2" runat="server" Text="Campo Específico"></asp:Label>
                                    </div>
                                    <div class="col-md-8" style="display: block">
                                        <asp:TextBox ID="txtCampoEspecifico" CssClass="form-control" runat="server" onkeyup="mayus(this)"></asp:TextBox>
                                        <asp:Repeater ID="rptCampoespecifco" runat="server" OnItemCreated="rptCampoespecifco_ItemCreated" OnItemCommand="rptCampoespecifco_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="table">
                                                    <thead>
                                                        <tr>
                                                            <th>Campo Específico</th>
                                                            <th>Acciones</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtcampoespecifico" Text='<%# Eval("campo") %>' CssClass="form-control" MaxLength="149" onkeyup="mayus(this)" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="btnEliminar" CommandName="Delete" runat="server" CommandArgument="<%# Container.ItemIndex + 1 %>"><i class="fas fa-trash-alt fa-2x" style="color:red"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                            </table>       
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div>
                                        <asp:LinkButton ID="addcampoespecifico" runat="server" OnClick="addcampoespecifico_Click">
                                                 <i class="fas fa-plus-circle" style="font-size:25px;color:dodgerblue"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <div class="table-responsive">
                                    <asp:GridView ID="dgvAreasEstudio" runat="server" DataKeyNames="IdCampoAmplio,DescripcionCampoAmplio" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvAreasEstudio_PageIndexChanging" OnRowCommand="dgvAreasEstudio_RowCommand" OnRowCancelingEdit="dgvAreasEstudio_RowCancelingEdit" OnRowEditing="dgvAreasEstudio_RowEditing" OnRowUpdating="dgvAreasEstudio_RowUpdating">
                                        <Columns>
                                            <asp:BoundField DataField="IdCampoAmplio" HeaderText="ID" SortExpression="IdCampoAmplio" Visible="false" />
                                            <asp:TemplateField HeaderText="Descripción Campo Amplio">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="EditCampoAmplio" runat="server" Text='<%# Bind("DescripcionCampoAmplio") %>' onkeyup="mayus(this)"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DescripcionCampoAmplio") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ButtonType="Button" ShowEditButton="True">
                                                <ControlStyle BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-xs btn-outline-primary" />
                                            </asp:CommandField>
                                        </Columns>
                                        <HeaderStyle BackColor="#085394" Font-Bold="True" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                        <PagerStyle BackColor="#E8E8E8" ForeColor="#003399" HorizontalAlign="Center" />
                                        <RowStyle BackColor="White" ForeColor="#00000" Font-Size="Smaller"/>
                                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                        <SortedDescendingHeaderStyle BackColor="#002876" />
                                        
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <!--segundo tab-->
                <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Campo Específico">
                    <ContentTemplate>
                        <div class="row">
                            <asp:LinkButton ID="btnUpdateCampoEsp" CssClass="btn btn-sm" Style="margin-right: 15px; color: white; background: #19A818; float: right" runat="server" OnClick="btnUpdateCampoEsp_Click"><i class="fa fa-save"></i> Guardar</asp:LinkButton>
                        </div>
                        <br />
                        <div class="row ">
                            <div class="col-md-5">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label3" runat="server" Text="Campo Amplio"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCampoAmplio" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCampoAmplio" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCampoAmplio_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Seleccionar--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnNewCampoEsp" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="col-md-3">
                                                <asp:Label ID="Label4" runat="server" Text="Campo Específico"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:LinkButton ID="btnNewCampoEsp" runat="server" OnClick="btnNewCampoEsp_Click">
                                                 <i class="fas fa-plus-circle" style="font-size:25px;color:dodgerblue"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-8">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="rptNewCampEsp" runat="server" OnItemCreated="rptNewCampEsp_ItemCreated" OnItemCommand="rptNewCampEsp_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table">
                                                            <thead>
                                                                <tr>
                                                                    <th>Campo Específico</th>
                                                                    <th>Acciones</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtcampoespecificoU" Text='<%# Eval("campo") %>' CssClass="form-control" MaxLength="149" onkeyup="mayus(this)" />
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="btnEliminar" CommandName="Delete" runat="server" CommandArgument="<%# Container.ItemIndex + 1 %>"><i class="fas fa-trash-alt fa-2x" style="color:red" ></i></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                            </table>       
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="table-responsive">
                                            <asp:GridView ID="dgvUpdateCampoEspecifico" runat="server" DataKeyNames="IdCampoEspecifico,IdCampoAmplio,DescripcionCampoEspecifico" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" AllowPaging="True" AllowCustomPaging="true" PageSize="10" OnPageIndexChanging="dgvAreasEstudio_PageIndexChanging" OnRowCommand="dgvAreasEstudio_RowCommand" OnRowCancelingEdit="dgvUpdateCampoEspecifico_RowCancelingEdit" OnRowEditing="dgvUpdateCampoEspecifico_RowEditing" OnRowUpdating="dgvUpdateCampoEspecifico_RowUpdating">
                                                <Columns>
                                                    <asp:BoundField DataField="IdCampoEspecifico" HeaderText="ID" SortExpression="IdCampoAmplio" Visible="false" />
                                                    <asp:BoundField DataField="IdCampoAmplio" HeaderText="ID" SortExpression="IdCampoAmplio" Visible="false" />
                                                    <asp:TemplateField HeaderText="Campo Específico">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="EditCampoEspecifico" runat="server" Text='<%# Bind("DescripcionCampoEspecifico") %>' onkeyup="mayus(this)" TextMode="MultiLine"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("DescripcionCampoEspecifico") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ButtonType="Button" ShowEditButton="True">
                                                        <ControlStyle BorderColor="Black" BorderStyle="Outset" CssClass="btn btn-xs btn-outline-primary" />
                                                    </asp:CommandField>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>



                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    <script src="Scripts/funciones.js" type="text/javascript"></script>
</asp:Content>
