<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitaEnmiendaLeg.aspx.cs" Inherits="Regentes.SolicitaEnmiendaLeg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="redondo">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label123" Font-Bold="true" runat="server" Text="Solicitud de Enmiendas" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:label runat="server" ID="LblMensaje"  Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:label>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Enmienda:"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" TextMode="MultiLine" Width="500px" Height="80px" ID="TxtEnmienda"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:Button runat="server" Text="Agregar Enmienda" class="button medium green"  ID="BtnAddEnmienda" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdEnmienda" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames=" CodRegente,Corr,CorrEnmienda,Enmienda">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="CorrEnmienda" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>       
                        <telerik:GridBoundColumn DataField="Enmienda" HeaderText="Enmienda"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridTemplateColumn HeaderText="Borrar"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgEditar" ToolTip="Borrar" ImageUrl="~/Imagenes/trash.png" CommandName="CmdDelete" OnClientClick="javascript:if(!confirm('¿Esta Seguro de Eliminar este registro?')){return false;}" />
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
        <tr>
            <td>
                <asp:Button runat="server" Text="Terminar Enmiendas" class="button medium green"  ID="BtnTerminarEnmienda" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="Cerrar Enmiendas" class="button medium green"  ID="BtnCerrarEnmiendas" Visible="false" />
            </td>
        </tr>
    </table>
</div>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/cargando.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnAddEnmienda">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="TxtEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="TxtMaxEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdEnmienda" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdEnmienda">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdEnmienda" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnTerminarEnmienda">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="BtnAddEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="BtnTerminarEnmienda" />
                <telerik:AjaxUpdatedControl ControlID="GrdEnmienda" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<asp:TextBox runat="server" Visible="false" ID="TxtCodregente"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtMaxEnmienda"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtNus"></asp:TextBox>
</asp:Content>
