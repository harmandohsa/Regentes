<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VistoBuenoJur.aspx.cs" Inherits="Regentes.VistoBuenoJur" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <SCRIPT language="JavaScript">
     function url(CodRegente, nus) {
         hidden = open('RepDictamenJur.aspx?CodRegente=' + CodRegente + '&nus=' + nus + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
     }

     function url2(CodRegente, nus) {
         hidden = open('RepResolucion.aspx?CodRegente=' + CodRegente + '&nus=' + nus + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
     }
     </SCRIPT>
    <center>
<table>
            <tr>
                <td>
                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Visto Bueno Dictamen Legal/Resolución" Font-Size="X-Large"></asp:Label>
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
                                <asp:Label ID="Label2" runat="server" Text="Exportar a Pdf"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadGrid runat="server" ID="GrdDetalle" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="Both" AllowFilteringByColumn="true">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Nombre,CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,FecCreacion,Estatus,codestatus,nus,tipotramite,codtramite">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codtramite" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="codestatus" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="tipotramite" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Tipo de tramite" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="nus" Visible="false"  HeaderText="No. Gestión" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>        
                        <telerik:GridBoundColumn DataField="nus" UniqueName="nusfor" AllowFiltering="false" ShowFilterIcon="false" HeaderText="No. Gestión" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Nombre" HeaderText="Nombre" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="CodReg"  AllowFiltering="false" ShowFilterIcon="false" HeaderText="Código RF" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="CodregEMPF" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Código EPMF" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="CODregECUT" HeaderText="Código EECUT"  AllowFiltering="false" ShowFilterIcon="false"  HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Correo" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Correo" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Telefono" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Teléfono"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Profesion"  AllowFiltering="false" ShowFilterIcon="false" HeaderText="Profesión"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FecRecepcion" UniqueName="FecRecep" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Fecha Recepción Secretaria"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Estatus" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Estatus"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dias" UniqueName="dia" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Días en CONAP"  HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Estado" UniqueName="Est" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Opinion"  HeaderStyle-Width="100px">
                        </telerik:GridBoundColumn>
                         <telerik:GridTemplateColumn HeaderText="Historial Enmiendas"  UniqueName="Histori"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="Imghistory" ImageUrl="~/Imagenes/histori.png" CommandName="CmdHistoria"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn HeaderText="Imprimir Dictamen" UniqueName="Print"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrint" ImageUrl="~/Imagenes/print.png"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                         <telerik:GridTemplateColumn HeaderText="Imprimir Resolucion" UniqueName="PrintRes"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintRes" ImageUrl="~/Imagenes/print.png"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText="Dar Visto Bueno" UniqueName="Good"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgGood" ImageUrl="~/Imagenes/yes.png" CommandName="CmdGood" OnClientClick="javascript:if(!confirm('¿Esta Seguro de dar su Visto Bueno?')){return false;}"/>
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
        <table runat="server" id="TblHist" visible="false">
        <tr>
            <td>
                <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Historial de Enmiendas" Font-Size="Large"></asp:Label>
            </td>
        </tr>
            <tr>
                <td>
                    <telerik:RadGrid runat="server" ID="Grdhistoria" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="codregente,corr,FecRecibe,estatus">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codregente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="corr"  HeaderText="Recepcion" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>     
                        <telerik:GridBoundColumn DataField="FecRecibe" HeaderText="Fecha" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridTemplateColumn HeaderText="Ver" UniqueName="VerTrans"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="Imghistory" ImageUrl="~/Imagenes/view.png" CommandName="CmdHistoria"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText="Impresion Constancia" UniqueName="Print"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrint" ImageUrl="~/Imagenes/print.png" CommandName="CmdPrint"/>
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
    <table>
        <tr>
            <td>
            <telerik:RadBarcode runat="server" ID="QrCode" Visible="false" OutputType="EmbeddedPNG" Type="QRCode" Width="371px" Height="371px">
                <QRCodeSettings />
            </telerik:RadBarcode>
            </td>
        </tr>
    </table>
    <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="GrdDetalle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="Grdhistoria" />
                 <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="TxtCodRegente" />
                <telerik:AjaxUpdatedControl ControlID="TblHist" />
                <telerik:AjaxUpdatedControl ControlID="Grdhistoria" />
                <telerik:AjaxUpdatedControl ControlID="Label3" />
                <telerik:AjaxUpdatedControl ControlID="Label1" />
                <telerik:AjaxUpdatedControl ControlID="QrCode" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="Grdhistoria">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="Grdhistoria" />
                 <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="TxtCodRegente" />
                <telerik:AjaxUpdatedControl ControlID="TblHist" />
                <telerik:AjaxUpdatedControl ControlID="Grdhistoria" />
                <telerik:AjaxUpdatedControl ControlID="Label3" />
                <telerik:AjaxUpdatedControl ControlID="Label1" />
                <telerik:AjaxUpdatedControl ControlID="Grdhistoria" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>--%>
<asp:TextBox runat="server" ID="TxtCodRegente" Text="" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" ID="TxtNus" Text="" Visible="false"></asp:TextBox>
</asp:Content>
