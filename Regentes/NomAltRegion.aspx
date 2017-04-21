<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NomAltRegion.aspx.cs" Inherits="Regentes.NomAltRegion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Nombres alternos para Directores Regionales." Font-Size="X-Large"></asp:Label>
                
            </td>
        </tr>
        <tr>
             <td>
                <hr />
            </td>
        </tr>
         <tr>
            <td>
                 <asp:label runat="server" ID="LblMensaje"  Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:label>
            </td>
        </tr>

        <tr>
            <td>    
                <asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtNombre" Width="250px" TabIndex="1"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton runat="server" ID="LnkGrabar" Text="Grabar" TabIndex="8"></asp:LinkButton>
            </td>
        </tr>
         <tr>
             <td colspan="3">
                <hr />
            </td>
        </tr>
        
    </table>
    <table>
        <tr>
                <td>
                    <table>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgExpExl" runat="server" ImageUrl="~/Imagenes/xls.png" ToolTip="Exportar a Excel" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgExpPdf" runat="server" ImageUrl="~/Imagenes/pdf.png" ToolTip="Exportar a PDF" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Text="Exportar a Excel"></asp:Label>
                            </td >
                            <td align="center">
                                <asp:Label ID="Label3" runat="server" Text="Exportar a Pdf"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        <tr>
            <td colspan="3">
                <telerik:RadGrid runat="server" ID="GrdDetalle" PageSize="30" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="Both">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="alterno,region,codregion">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codregion" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="region" HeaderText="Región"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="alterno" HeaderText="Alterno"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Editar"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgEditar" ImageUrl="~/Imagenes/edit.png" CommandName="CmdModificar"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>        
                </MasterTableView>
                <FilterMenu EnableTheming="true">
                    <CollapseAnimation Duration="200" Type="OutQuint" />
                </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
</center>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="LnkGrabar">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblmensaje" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombre" />
                <telerik:AjaxUpdatedControl ControlID="CodRegion" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdDetalle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblmensaje" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombre" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="CodRegion" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<asp:TextBox runat="server" id="CodRegion" Visible="false"></asp:TextBox>
</asp:Content>
