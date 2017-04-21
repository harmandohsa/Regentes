<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HisDatPer.aspx.cs" Inherits="Regentes.HisDatPer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<SCRIPT language="JavaScript">
    function url(CodRegente,Corr) {
        hidden = open('VerInfo.aspx?CodRegente=' + CodRegente + '&Corr=' + Corr + '', 'NewWindow', 'top=0,left=0,width=800,height=600,status=yes,resizable=yes,scrollbars=yes');
    }
</SCRIPT>
<table>
    <tr>
        <td colspan="4">
            <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Historial de datos personales de Regentes" Font-Size="X-Large"></asp:Label>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td>
            <telerik:RadGrid runat="server" ID="GrdDetalle" PageSize="500" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None" AllowFilteringByColumn="true">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CODIGO,nombre,CORREO,TELEFONO,PROFESION,DIRECCION,DEPARTAMENTO,MUNICIPIO,region,FECHAVEN,CodRegente" CommandItemDisplay="Top">
                <CommandItemTemplate>
                    <table>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="imgbExport" ImageUrl="~/Imagenes/xls.png" ToolTip="Exportar a Excel" runat="server" AlternateText="Export" />  <%--OnClick="imgbExport_Click"--%>
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ExpXls" ImageUrl="~/Imagenes/pdf.png" runat="server" ToolTip="Exportar a Pdf" AlternateText="Export"  /><%-- OnClick="ExpXls_Click"--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Text="Exportar a Excel"></asp:Label>
                            </td >
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Text="Exportar a Pdf"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
            
                    <br />
            
                </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="CODIGO" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>      
                        <telerik:GridBoundColumn DataField="nombre" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderText="Nombre" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>           
                        <telerik:GridBoundColumn DataField="CORREO"  HeaderText="Correo" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="TELEFONO"  HeaderText="Teléfono" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>     
                        <telerik:GridBoundColumn DataField="PROFESION"  HeaderText="Profesión " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="DIRECCION"  HeaderText="Domicilio " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="DEPARTAMENTO"  HeaderText="Departamento " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="MUNICIPIO"  HeaderText="Municipio " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="region"  HeaderText="Delegación CONAP" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="FECHAVEN"  HeaderText="Fecha vencimiento de la vigencia" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridTemplateColumn HeaderText="Historial" UniqueName="Histori"  AllowFiltering="false" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="Imghistory" ImageUrl="~/Imagenes/histori.png" CommandName="CmdHistoria"/>
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
<table>
    <tr>
        <td>
            <telerik:RadGrid runat="server" ID="Grdhistoria" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="Corr"  HeaderText="Correlativo" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>     
                        <telerik:GridTemplateColumn HeaderText="Ver Datos Personales" UniqueName="Print"  AllowFiltering="false" ShowFilterIcon="false"  >
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
<asp:TextBox runat="server" id="TxtCodRegente" Visible="false"></asp:TextBox>
</asp:Content>
