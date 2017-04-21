<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="Regentes.Usuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center>
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Creación de Usuarios" Font-Size="X-Large"></asp:Label>
                
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <telerik:RadButton runat="server" ID="BtnNuevo" Text="Nuevo Usuario"></telerik:RadButton>
            </td>
        </tr>
        <tr>
             <td colspan="3">
                <hr />
            </td>
        </tr>
         <tr>
            <td colspan="3">
                 <asp:label runat="server" ID="LblMensaje"  Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:label>
            </td>
        </tr>

        <tr>
            <td>    
                <asp:Label ID="Label2" runat="server" Text="Usuario"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Nombres"></asp:Label>
            </td>
            <td>    
                <asp:Label ID="Label5" runat="server" Text="Apellidos"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtUsuario" Width="250px" TabIndex="1"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtNombres" Width="250px" TabIndex="2"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtApellidos" Width="250px" TabIndex="3"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Correo"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Tipo Usuario"></asp:Label>
            </td>
            <td>    
                <asp:Label ID="LblRegion" runat="server" Text="Región"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtCorreo" Width="250px" TabIndex="3"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadComboBox runat="server" ID="CboTipoUsuario" Width="250px" TabIndex="4"></telerik:RadComboBox>
            </td>
            <td>    
                <telerik:RadComboBox runat="server" ID="CboRegion" Width="250px" TabIndex="5" AutoPostBack="true"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="En Funciones" ID="ChkEnFunciones" />
            </td>
            <td>
            </td>
            <td>
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
            <td colspan="3">
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
                            <asp:Label ID="Label7" runat="server" Text="Exportar a Pdf"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td colspan="3">
                <telerik:RadGrid runat="server" ID="GrdDetalle" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="Both" AllowFilteringByColumn="true">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodUsuario,Usuario,Nombres,Apellidos,TipoUsuario,CodTipoUsuario,nombre">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodUsuario" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Usuario" HeaderText="Usuario" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Nombres" HeaderText="Nombres" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="Apellidos" HeaderText="Apellidos" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="TipoUsuario" HeaderText="Tipo de Usuario"  AllowFiltering="false" ShowFilterIcon="false"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CodTipoUsuario" HeaderText="CodTipoUsuario" Visible="false" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="nombre" HeaderText="Region"  AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Editar"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgEditar" ImageUrl="~/Imagenes/edit.png" CommandName="CmdModificar"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                         <telerik:GridTemplateColumn HeaderText="Pass"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPass" ImageUrl="~/Imagenes/pass.ico" CommandName="CmdClave"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn HeaderText="Activar/Desactivar" UniqueName="EstUsr"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgAct" ToolTip="Activar" ImageUrl="~/Imagenes/up.png" CommandName="Activar"/>
                                <asp:ImageButton runat="server" ID="ImgDown" ToolTip="Desactivar" ImageUrl="~/Imagenes/down.png" CommandName="Desactivar"/>
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
                <telerik:AjaxUpdatedControl ControlID="TxtUsuario" />
                <telerik:AjaxUpdatedControl ControlID="TxtClave" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombres" />
                <telerik:AjaxUpdatedControl ControlID="TxtApellidos" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="TxtUsuario" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombres" />
                <telerik:AjaxUpdatedControl ControlID="TxtApellidos" />
                <telerik:AjaxUpdatedControl ControlID="TxtCorreo" />
                <telerik:AjaxUpdatedControl ControlID="TxtCodUsuario" />
                <telerik:AjaxUpdatedControl ControlID="ChkEnFunciones" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdDetalle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblmensaje" />
                <telerik:AjaxUpdatedControl ControlID="CboTipoUsuario" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="TxtUsuario" />
                <telerik:AjaxUpdatedControl ControlID="TxtClave" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombres" />
                <telerik:AjaxUpdatedControl ControlID="TxtApellidos" />
                <telerik:AjaxUpdatedControl ControlID="CboRegion" />
                <telerik:AjaxUpdatedControl ControlID="CboSubRegion" />
                <telerik:AjaxUpdatedControl ControlID="TxtCorreo" />
                <telerik:AjaxUpdatedControl ControlID="TxtCodUsuario" />
                <telerik:AjaxUpdatedControl ControlID="ChkEnFunciones" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnNuevo">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="lblmensaje" />
                <telerik:AjaxUpdatedControl ControlID="CboTipoUsuario" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="TxtUsuario" />
                <telerik:AjaxUpdatedControl ControlID="TxtClave" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombres" />
                <telerik:AjaxUpdatedControl ControlID="TxtApellidos" />
                <telerik:AjaxUpdatedControl ControlID="CboRegion" />
                <telerik:AjaxUpdatedControl ControlID="CboSubRegion" />
                <telerik:AjaxUpdatedControl ControlID="TxtCorreo" />
                <telerik:AjaxUpdatedControl ControlID="TxtCodUsuario" />
                <telerik:AjaxUpdatedControl ControlID="ChkEnFunciones" />
                <telerik:AjaxUpdatedControl ControlID="TxtNombres" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<asp:TextBox runat="server" id="TxtCodUsuario" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" id="TxtCorreoMod" Visible="false"></asp:TextBox>
</asp:Content>
