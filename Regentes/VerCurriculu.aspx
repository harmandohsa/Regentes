<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerCurriculu.aspx.cs" Inherits="Regentes.VerCurriculu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="Div1" runat="server" class="redondo">
    <center>
       <table>
        <tr>
            <td colspan="4">
                <asp:Label ID="Label27"  Font-Bold="true" runat="server" Text="Curriculum Vitae Regentes" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
         
         <tr>
            <td colspan="4">
                <asp:Label ID="LblNombre"  Font-Bold="true" runat="server" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                    <hr />
                </td>
        </tr>
          <tr>
            <td>
                <asp:Label ID="Label22" runat="server" Text="Nivel"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label23" runat="server" Text="Título"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label24" runat="server" Text="Año"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label25" runat="server" Text="Institución Educativa"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label26" runat="server" Text="Medio"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtTitMedio" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisMedio" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtInsMedio" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label277" runat="server" Text="T.U."></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false"  onkeydown = "return (event.keyCode!=13);" ID="TxtTitTu" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false"  onkeydown = "return (event.keyCode!=13);" ID="TxtAnisTu" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
               <telerik:RadTextBox runat="server" Enabled="false"   onkeydown = "return (event.keyCode!=13);" ID="TxtInsTu" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label29" runat="server" Text="Profesional"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server"  Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtTituProfesional" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisProfesiona" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);"  ID="TxtInsPro" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label31" runat="server" Text="Posgrado"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false"  onkeydown = "return (event.keyCode!=13);" ID="TxtTitPosGrado" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false"  onkeydown = "return (event.keyCode!=13);" ID="TxtAnisPosGrado" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false"  onkeydown = "return (event.keyCode!=13);" ID="TxtinsPos" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label33" runat="server" Text="Otro"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtTitOtro" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisOtro" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtInsOtro" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
       </table>
       <table>
       <tr>
        <td colspan="4">
            <hr />
        </td>
       </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="Label28" runat="server" Font-Bold="true" Text="Cursos de Especialización en materia forestal y/o administración de Áreas Protegidas" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdCurso" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Enfoque,Institucion,Duracion">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Enfoque" HeaderText="Enfoque del Curso"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                         <telerik:GridBoundColumn DataField="Institucion" HeaderText="Institución"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="Duracion" HeaderText="Duración"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
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
    <table>
        <tr>
        <td colspan="4">
            <hr />
        </td>
       </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="Label51" Font-Bold="true" runat="server" Text="Experiencia laboral relacionada con materia forestal y/o administración de Areas Protegidas" Font-Size="Large"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdExperiencia" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Actividad,Entidad,Ubicacion,Observaciones">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Actividad" HeaderText="Actividad"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                         <telerik:GridBoundColumn DataField="Entidad" HeaderText="Entidad"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="Ubicacion" HeaderText="Ubicación"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="Observaciones" HeaderText="Observaciones"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
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
    <table>
        <tr>
        <td colspan="4">
            <hr />
        </td>
       </tr>
       <tr>
            <td colspan="4">
                <asp:Label ID="Label65" Font-Bold="true" runat="server" Text="Trabajo Actual" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                    <asp:Label ID="LblTrabajo" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Institución para la que trabaja actualmente:"></asp:Label>
            </td>
            <td colspan="3">
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtInstitucionActual" Width="550px"></telerik:RadTextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:Label runat="server" ID="Label56" Text="Puesto que desempeña:"></asp:Label>
            </td>
            <td colspan="3">
                <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtPuesto" Width="550px"></telerik:RadTextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
        <td colspan="4">
            <hr />
        </td>
       </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="Label57" Font-Bold="true" runat="server" Text="Experiencia como Regente Forestal" Font-Size="Large"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadGrid runat="server" ID="GrdExperienciaFores" PageSize="15" 
                AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    AllowPaging="true" GridLines="None">
                <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Siguiente"   
                  PrevPageText="Anterior" Position="Bottom" 
                  PagerTextFormat="Change page: {4} &amp;nbsp;Pagina &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, registros &lt;strong&gt;{2}&lt;/strong&gt; a &lt;strong&gt;{3}&lt;/strong&gt; de &lt;strong&gt;{5}&lt;/strong&gt;." 
                  PageSizeLabelText="Registros"/>
                <MasterTableView DataKeyNames="CodRegente,Corr,Finca,ubicacion,tipoactividad,area,periodo">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CodRegente" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Corr" Visible="false" HeaderText="Código" HeaderStyle-Width="75px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                        <telerik:GridBoundColumn DataField="Finca" HeaderText="Finca"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn>         
                         <telerik:GridBoundColumn DataField="ubicacion" HeaderText="Ubicacion"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="tipoactividad" HeaderText="Tipo de Actividad"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                         <telerik:GridBoundColumn DataField="area" HeaderText="Area"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="periodo" HeaderText="Periodo"  HeaderStyle-Width="225px">
                            <HeaderStyle Width="225px"></HeaderStyle>
                        </telerik:GridBoundColumn> 
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
                <asp:Label ID="Label2" runat="server" Text="Manejo de bosques naturales (latifoliados, coníferas, etc.), establecimiento y manejo de plantaciones forestales, otros estudios afines en materia forestal. "></asp:Label>
            </td>
        </tr>
        <tr>
        <td>
            <hr />
        </td>
       </tr>
    </table>
    </center>
</div>
</asp:Content>
