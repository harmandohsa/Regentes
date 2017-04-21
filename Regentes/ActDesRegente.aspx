<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActDesRegente.aspx.cs" Inherits="Regentes.ActDesRegente" %>
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
    function url3(CodRegente, nus) {
        hidden = open('RepDictamenTec.aspx?CodRegente=' + CodRegente + '&nus=' + nus + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
    }
    function url4(CodRegente, nus) {
        hidden = open('RepConstancia.aspx?CodRegente=' + CodRegente + '&nus=' + nus + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
    }

    function url7(CodRegente, nus, tramite) {
        hidden = open('RepSolicitud.aspx?CodRegente=' + CodRegente + '&nus=' + nus + '&tramite=' + tramite + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
    }
    function url8(CodRegente) {
        hidden = open('RepCurriculu.aspx?CodRegente=' + CodRegente + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
    }
</SCRIPT>
<center>
    <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Activar/Desactivar Regente Forestal" Font-Size="X-Large"></asp:Label>
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
                <MasterTableView DataKeyNames="CodRegente,Nombre,CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,FecCreacion,Estatus,codestatus,nus,recomendacion,tipotramite,codtramite,codigo,codest,categoria">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codtramite" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="codest" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="tipotramite" Visible="false" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Tipo de tramite" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>      
                        <telerik:GridBoundColumn DataField="codigo" UniqueName="nusfor" AllowFiltering="false" ShowFilterIcon="false" HeaderText="No. de Registro CONAP" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Nombre" HeaderText="Nombre" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="categoria"  AllowFiltering="false" ShowFilterIcon="false" HeaderText="Categoría" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="CodregEMPF" Visible="false" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Código EPMF" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="CODregECUT"  Visible="false" HeaderText="Código EECUT"  AllowFiltering="false" ShowFilterIcon="false"  HeaderStyle-Width="100px">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Correo" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Correo" HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Telefono" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Teléfono"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Profesion" Visible="false"  AllowFiltering="false" ShowFilterIcon="false" HeaderText="Profesión"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="" UniqueName="FecRecep" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Fecha de Inscripción"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="" UniqueName="Del" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Delegación CONAP"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Estatus" Visible="false" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Estatus"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="recomendacion" Visible="false" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Estatus"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Historial Enmiendas" Visible="false" UniqueName="Histori"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="Imghistory" ImageUrl="~/Imagenes/histori.png" CommandName="CmdHistoria"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn HeaderText="Imprimir Dictamen Técnico" Visible="false" UniqueName="Print"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintTec" ImageUrl="~/Imagenes/print.png" CommandName="CmdPrint"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                         <telerik:GridTemplateColumn HeaderText="Imprimir Dictamen Legal" Visible="false" UniqueName="PrintJur"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintJur" Visible="false" ImageUrl="~/Imagenes/print.png" CommandName="CmdPrintDicJur"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                         <telerik:GridTemplateColumn HeaderText="Imprimir Resolución" Visible="false" UniqueName="PrintRes"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintRes" Visible="false" ImageUrl="~/Imagenes/print.png" CommandName="CmdPrintRes"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText="Imprimir Constancia" Visible="false" UniqueName="PrintCons"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintCons" Visible="true" ImageUrl="~/Imagenes/print.png" CommandName="CmdPrintCons"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText="Dar Visto Bueno"  Visible="false" UniqueName="Good"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgGood" ImageUrl="~/Imagenes/yes.png" CommandName="CmdGood" OnClientClick="javascript:if(!confirm('¿Esta Seguro de dar su Visto Bueno?')){return false;}"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText="Imprimir Solicitud" Visible="false" UniqueName="PrintSol"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrint" ImageUrl="~/Imagenes/dictamen.png" CommandName="CmdPrint"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn HeaderText="Imprimir Curriculum" Visible="false" UniqueName="PrintCV"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintCV" ImageUrl="~/Imagenes/open.png" CommandName="CmdPrintCV"/>
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
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
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
</telerik:RadAjaxManager>
<asp:TextBox runat="server" ID="TxtCodRegente" Text="" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" ID="TxtNus" Text="" Visible="false"></asp:TextBox>
</asp:Content>
