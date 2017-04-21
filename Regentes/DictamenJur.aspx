<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DictamenJur.aspx.cs" Inherits="Regentes.DictamenJur" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="redondo">
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label123" Font-Bold="true" runat="server" Text="Dictamen Legal" Font-Size="X-Large"></asp:Label>
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
                <asp:Label runat="server" Text="Referencia"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" id="TxtReferencia" Width="200px"></telerik:RadTextBox>
            </td>
            
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="No. Control Interno"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" id="TxtControlInterno" Width="200px"></telerik:RadTextBox>
            </td>
            
        </tr>
        <tr>
            
            <td>
                <asp:Button runat="server" Text="Grabar Referencia y Folios" Visible="false" class="button medium green"  ID="BtnGrabaReferencia" />
            </td>
        </tr>
    </table>
    <table>
         <tr>
            <td colspan="3">
                <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Fundamento de Derecho" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:TextBox runat="server" TextMode="MultiLine" Height="75px"  ID="TxtFundamento" Width="450px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Button runat="server" Text="Agregar Fundamento" class="button medium green"  ID="BtnAddConFundamento" />
            </td>
        </tr>
    </table>
     <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdFundamento" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Fundamento">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Fundamento" HeaderText="Fundamento"  HeaderStyle-Width="425px">
                            <HeaderStyle Width="425px"></HeaderStyle>
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
                <hr />
            </td>
        </tr>
    </table>
     <table>
         <tr>
            <td colspan="3">
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Análisis Legal" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:TextBox runat="server" TextMode="MultiLine" Height="75px"   ID="TxtAnalisis" Width="450px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Button runat="server" Text="Agregar Análisis" class="button medium green"  ID="BtnAddAnalisis" />
            </td>
        </tr>
    </table>
     <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdAnalisis" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,AnalisisLeg">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="AnalisisLeg" HeaderText="Analisis"  HeaderStyle-Width="425px">
                            <HeaderStyle Width="425px"></HeaderStyle>
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
                <hr />
            </td>
        </tr>
    </table>
    <table>
         <tr>
            <td colspan="3">
                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Recomendaciones" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:TextBox runat="server" TextMode="MultiLine" Height="75px"   ID="TxtRecomendaciones" Width="450px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Button runat="server" Text="Agregar Recomendación" class="button medium green"  ID="BtnAddRecomendacion" />
            </td>
        </tr>
    </table>
     <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdRecomendacion" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Recomendacion">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Recomendacion" HeaderText="Recomendacion"  HeaderStyle-Width="425px">
                            <HeaderStyle Width="425px"></HeaderStyle>
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
            <td colspan="3>
                 <asp:Label ID="Label14" Font-Bold="true" runat="server" Text="Puesto" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                 <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtPuesto" Text="Asesor Jurídico" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
    </table>
</div>
<div class="redondo">   
    <asp:Button runat="server" Text="Grabar Dictamen" class="button medium green"  ID="btnGrabaDictamen" />
</div>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/cargando.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnAddConFundamento">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdFundamento" />
                <telerik:AjaxUpdatedControl ControlID="TxtFundamento" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdFundamento" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdFundamento">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdFundamento" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdFundamento" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnAddAnalisis">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdAnalisis" />
                <telerik:AjaxUpdatedControl ControlID="TxtAnalisis" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdAnalisis" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdAnalisis">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdAnalisis" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdAnalisis" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="btnGrabaDictamen">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="btnGrabaDictamen" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="btnGrabaDictamen" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<asp:TextBox runat="server" Visible="false" ID="TxtCodregente"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtNus"></asp:TextBox>
</asp:Content>
