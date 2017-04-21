<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoReg.aspx.cs" Inherits="Regentes.ListadoReg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
    <tr>
        <td colspan="4">
            <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Listado de regentes inscritos" Font-Size="X-Large"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="4">
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
            <asp:Label runat="server" Text="No. de registro CONAP"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Nombres"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Apellidos"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Profesión"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox runat="server" Width="200px" ID="TxtNoReg" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox runat="server" Width="200px" ID="TxtNombres" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox runat="server" Width="200px" ID="TxtApellidos" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox runat="server" Width="200px" ID="TxtProfesion" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Departamento"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Municipio"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label7" runat="server" Text="Delegación"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label8" runat="server" Text="Estatus"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
             <telerik:RadComboBox runat="server"  ID="CboDep" AllowCustomText="true" Width="200px" ZIndex="10000" AutoPostBack="true"></telerik:RadComboBox>
        </td>
        <td>
            <telerik:RadComboBox runat="server"  ID="CboMun" AllowCustomText="true" Width="200px"  ZIndex="10000"></telerik:RadComboBox>
        </td>
        <td>
            <telerik:RadComboBox runat="server"  ID="CboDelegacion" AllowCustomText="true"  Width="200px"  ZIndex="10000"></telerik:RadComboBox>
        </td>
        <td>
            <telerik:RadComboBox runat="server"  ID="CboEstatus" Width="200px">
                <Items>
                    <telerik:RadComboBoxItem Text="Activo" Value="1" />
                    <telerik:RadComboBoxItem Text="Inactivo" Value="2" />
                    <telerik:RadComboBoxItem Text="Suspendido" Value="3" />
                </Items>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
         <td>
            <asp:Label ID="Label10" runat="server" Text="Categoría"></asp:Label>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadComboBox runat="server"  ID="CboCat" AllowCustomText="true" Width="200px"  ZIndex="10000"></telerik:RadComboBox>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadButton runat="server" Text="Generar Listado" ID="BtnGenerar"></telerik:RadButton>
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
                <MasterTableView DataKeyNames="CODIGO,nombre,CORREO,TELEFONO,PROFESION,DIRECCION,DEPARTAMENTO,MUNICIPIO,region,FECHAVEN,categoria" CommandItemDisplay="Top">
                <CommandItemTemplate>
                    <table>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="imgbExport" ImageUrl="~/Imagenes/xls.png" ToolTip="Exportar a Excel" runat="server" AlternateText="Export" OnClick="imgbExport_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ExpXls" ImageUrl="~/Imagenes/pdf.png" runat="server" ToolTip="Exportar a Pdf" AlternateText="Export" OnClick="ExpXls_Click" />
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
                         <telerik:GridBoundColumn DataField="CODIGO" Visible="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="" UniqueName="nusfor" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
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
                          <telerik:GridBoundColumn DataField="categoria"  HeaderText="Categoría " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
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
                        <telerik:GridBoundColumn DataField="Estatus"  HeaderText="Estatus" UniqueName="Est" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="FECHAVEN"  HeaderText="Fecha vencimiento de la vigencia" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>           
                    </Columns>        
                </MasterTableView>
                <FilterMenu EnableTheming="true">
                    <CollapseAnimation Duration="200" Type="OutQuint" />
                </FilterMenu>
            </telerik:RadGrid>
        </td>
    </tr>
</table>
</asp:Content>
