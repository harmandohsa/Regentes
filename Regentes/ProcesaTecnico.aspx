<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcesaTecnico.aspx.cs" Inherits="Regentes.ProcesaTecnico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style1
    {
        width: 201px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="redondo">
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label123" Font-Bold="true" runat="server" Text="Dictamen Técnico" Font-Size="X-Large"></asp:Label>
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
        <tr>
            <td colspan="3" align="left">
                <asp:Label ID="Label1" runat="server" Text="Referencia"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtReferencia" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Documentación Presentada" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label3" runat="server"  Text="Recibo de Ingresos Varios 63-A2"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Fecha"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Monto (Q)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtRecibo" Width="200px"></asp:TextBox>
            </td>
            <td>
                <telerik:RadDatePicker onkeydown = "return (event.keyCode!=13);" ID="TxtFecRecibo" MinDate="01/01/1930" Width="200px" runat="server"></telerik:RadDatePicker>
            </td>
            <td>
                <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtBoleta" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="Label6" runat="server"  Text="Serie/Número de Constancia de Colegiado Activo"></asp:Label>
            </td>
            <td colspan="2" align="left">
                <asp:Label ID="Label7" runat="server" Text="No. de colegiado activo"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtConstancia" Width="200px"></asp:TextBox>
            </td>
            <td colspan="2" align="left">
                <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtNoConstancia" Width="200px"></asp:TextBox>
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
                <asp:Label ID="Label10" Font-Bold="true" runat="server" Text="Dictamen"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadComboBox runat="server"  ID="CboRecomendacion" Width="200px"></telerik:RadComboBox>
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
                <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Recomendaciones" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Label ID="Label8" runat="server" Text="Recomendaciones"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:TextBox runat="server" TextMode="MultiLine" Height="75px"  ID="TxtConclusion" Width="450px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <asp:Button runat="server" Text="Agregar Recomendación" class="button medium green"  ID="BtnAddConclusion" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdConclusion" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Conclusion">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Conclusion" HeaderText="Recomendaciones"  HeaderStyle-Width="225px">
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
                <hr />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Folios"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label12" Font-Bold="true" runat="server" Text="Categoría"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label13" Font-Bold="true" runat="server" Text="Vigencia"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadNumericTextBox runat="server" Width="200px"  onkeydown = "return (event.keyCode!=13);" ID="TxtFolio">
                    <NumberFormat DecimalDigits="0" />
                </telerik:RadNumericTextBox>
            </td>
            <td>
                <telerik:RadComboBox runat="server"  ID="CboCategoria" Width="200px">
                    <Items>
                        <telerik:RadComboBoxItem Text="Técnico" Value="1" />
                        <telerik:RadComboBoxItem Text="Profesional" Value="2" />
                    </Items>
                </telerik:RadComboBox>
                <%--<telerik:RadTextBox runat="server" id="TxtCategoria" Width="200px" onkeydown = "return (event.keyCode!=13);" ></telerik:RadTextBox>--%>
            </td>
             <td>
                <telerik:RadNumericTextBox runat="server" Enabled="false" Width="200px"  onkeydown = "return (event.keyCode!=13);" ID="TxtVigencia">
                    <NumberFormat DecimalDigits="0" />
                </telerik:RadNumericTextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="3">
                 <asp:Label ID="Label14" Font-Bold="true" runat="server" Text="Puesto" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                 <asp:TextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtPuesto" Text="Asesor Forestal Regional" Width="200px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Button runat="server" Text="Guardar Dictamen" class="button medium green"  ID="BtnGrabar" />
            </td>
        </tr>
         <tr>
            <td>
                <asp:Button runat="server" Text="Imprimir Dictamen" class="button medium green"  ID="BtnPrint" Visible="false"/>
            </td>
        </tr>
    </table>
</div>
<%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/cargando.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnAddConclusion">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" />
                <telerik:AjaxUpdatedControl ControlID="TxtConclusion" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdConclusion">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnGrabar">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdConclusion" />
                <telerik:AjaxUpdatedControl ControlID="BtnPrint " />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="BtnGrabar" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>--%>
<asp:TextBox runat="server" Visible="false" ID="TxtCodregente"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtNus"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtCodTramite"></asp:TextBox>
</asp:Content>
