<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Resolucion.aspx.cs" Inherits="Regentes.Resolucion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="redondo">
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label123" Font-Bold="true" runat="server" Text="Resolución" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:label runat="server" ID="LblMensaje"  Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:label>
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
                <asp:Label ID="Label1" runat="server" Text="Referencia"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" id="TxtReferencia" Width="200px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="No. Control Interno"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" id="TxtControlInterno" Width="200px"></telerik:RadTextBox>
            </td>
            
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="No. de registro interno de regentes forestales"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" id="TxtRegInterno" Width="200px" MaxLength="100"></telerik:RadTextBox>
            </td>
            
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Considerandos"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" id="TxtConsiderandoUno" TextMode="MultiLine" Width="450px" Height="65px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="Agregar Considerando" class="button medium green"  ID="BtnAddConsiderando" />
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdConsiderando" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Considerando">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Considerando" HeaderText="Considerando"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridTemplateColumn HeaderText="Borrar"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgEditar" ImageUrl="~/Imagenes/trash.png" CommandName="CmdDelete" OnClientClick="javascript:if(!confirm('¿Esta Seguro de Eliminar este registro?')){return false;}" />
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
         <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Por Tanto"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" id="TxtPorTando" TextMode="MultiLine" Width="450px" Height="65px"></asp:TextBox>
            </td>
        </tr> <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Resuelve"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" id="TxtResuelve" TextMode="MultiLine" Width="450px" Height="65px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Folios"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Opinion"></asp:Label>
            </td>
            
        </tr>
        <tr>
            <td>
                <telerik:RadNumericTextBox runat="server" ID="TxtFolio" Value="0" Width="200px">
                    <NumberFormat DecimalDigits="0" />
                </telerik:RadNumericTextBox>
            </td>
            <td>
                <telerik:RadComboBox runat="server"  ID="CboRecomendacion" Width="200px"></telerik:RadComboBox>
            </td>
        </tr>
    </table>
</div>
<div class="redondo">   
    <asp:Button runat="server" Text="Grabar Resolución" class="button medium green"  ID="btnGrabaDictamen" />
</div>
<asp:TextBox runat="server" Visible="false" ID="TxtCodregente"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtNus"></asp:TextBox>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/cargando.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnAddConsiderando">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConsiderando" />
                <telerik:AjaxUpdatedControl ControlID="TxtConsiderandoUno" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdConsiderando" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdConsiderando">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConsiderando" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdConsiderando" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="btnGrabaDictamen">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="btnGrabaDictamen" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
</asp:Content>
