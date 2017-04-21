<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoRegentes.aspx.cs" Inherits="Regentes.ListadoRegentes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<SCRIPT language="JavaScript">
    function url(CodRegente) {
        hidden = open('VerCurriculu.aspx?CodRegente=' + CodRegente + '', 'NewWindow', 'top=0,left=0,width=1300,height=600,status=yes,resizable=yes,scrollbars=yes');
    }
</SCRIPT>
<div id="caja">
    <telerik:RadGrid runat="server" ID="GrdDetalle" PageSize="500" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None" AllowFilteringByColumn="true">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="codregente,CODIGO,nombre,CORREO,TELEFONO,PROFESION,DIRECCION,DEPARTAMENTO,MUNICIPIO,region,FECHAVEN">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codregente" Visible="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="CODIGO" Visible="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains"  HeaderText="No. Registro CONAP " HeaderStyle-Width="75px">
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
                        <telerik:GridBoundColumn DataField="DIRECCION" Visible="false"  HeaderText="Domicilio " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="DEPARTAMENTO"  HeaderText="Departamento " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="MUNICIPIO" Visible="false" HeaderText="Municipio " AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="region" Visible="false"  HeaderText="Delegación regional" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AndCurrentFilterFunction="Contains" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="Estatus" Visible="false" HeaderText="Estatus" UniqueName="Est" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="FECHAVEN" Visible="false"  HeaderText="Fecha vencimiento de la vigencia" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="75px">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Ver Curriculum" UniqueName="PrintCV"  AllowFiltering="false" HeaderStyle-Width="75px" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ImgPrintCV" ImageUrl="~/Imagenes/open.png" CommandName="CmdPrintCV"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>            
                    </Columns>        
                </MasterTableView>
                <FilterMenu EnableTheming="true">
                    <CollapseAnimation Duration="200" Type="OutQuint" />
                </FilterMenu>
            </telerik:RadGrid>
</div>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="GrdDetalle">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" />
                <telerik:AjaxUpdatedControl ControlID="GrdDetalle" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
</asp:Content>
