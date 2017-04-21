<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActRegente.aspx.cs" Inherits="Regentes.ActRegente" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="redondo">
    <center>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label69" runat="server" Font-Bold="true" Text="FORMULARIO DE SOLICITUD DE ACTUALIZACIÓN DE DATOS DEL REGISTRO" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label36" runat="server" Font-Bold="true" Text="DE REGENTES FORESTALES EN ÁREAS PROTEGIDAS" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="LblMensaje" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="No. de Registro RF"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" Enabled="false" ID="TxtNoRegistroRG" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="Label35" runat="server" Text="No. de Registro EPMF"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" Enabled="false" ID="TxtNoRegistroEMPF" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label50" runat="server" Text="No. de Registro EECUT"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" Enabled="false" ID="TxtNoRegistroECUT" Width="200px"></telerik:RadTextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Apellidos"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" Enabled="false" ID="TxtApellidos" Width="200px"></telerik:RadTextBox>

                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Nombres"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" onkeydown = "return (event.keyCode!=13);" ID="TxtNombres" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Doc. Id" ></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" 
                        ID="TxtDocId" Width="200px" MaxLength="13" Enabled="false">
                        </telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="NIT" ></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox onkeydown = "return (event.keyCode!=13);" Enabled="false" runat="server" ID="TxtNit" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Nacionalidad"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox Enabled="false" onkeydown = "return (event.keyCode!=13);" runat="server" ID="TxtNacionalidad" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="Género"></asp:Label>
                </td>
                <td>
                   <telerik:RadComboBox runat="server"  ID="CboGenero" Enabled="false" Width="200px"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" Text="Fecha Nacimiento"></asp:Label>
                </td>
                <td>
                    <telerik:RadDatePicker onkeydown = "return (event.keyCode!=13);" Enabled="false" ID="TxtFecNac" MinDate="01/01/1930" Width="200px" runat="server"></telerik:RadDatePicker>
                </td>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Correo electrónico"></asp:Label>
                    <asp:Label ID="Label44" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtCorreo" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Dirección"></asp:Label>
                    <asp:Label ID="Label45" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtDireccion" Width="565px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" Text="Departamento"></asp:Label>
                    <asp:Label ID="Label46" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox runat="server"  ID="CboDep" Width="200px" AutoPostBack="true"></telerik:RadComboBox>
                </td>
                <td>
                    <asp:Label ID="Label14" runat="server" Text="Municipio"></asp:Label>
                    <asp:Label ID="Label47" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox runat="server"  ID="CboMun" Width="200px"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" Text="Teléfono (s)"></asp:Label>
                    <asp:Label ID="Label48" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTel" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="Label16" runat="server" Text="Fax (s)"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtFax" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label67" runat="server" Text="Categoría"></asp:Label>
                    <asp:Label ID="Label68" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox runat="server"  ID="CboCategoria" Width="200px"></telerik:RadComboBox>
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server" Text="No. Colegiado"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtColegiado" Width="200px"></telerik:RadTextBox>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label17" runat="server" Text="Profesión"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtProfesion" Width="200px"></telerik:RadTextBox>
                </td>
               <td></td>
               <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label19" runat="server" Text="Especialización"></asp:Label>
                    
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtEspecializacion" Width="565px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label20" runat="server" Text="Experiencia en el campo forestal"></asp:Label>
                    <asp:Label ID="Label49" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="OptioEspe" runat="server">
                        <asp:ListItem Text="Si"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label21" runat="server" Text="Cuales:"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtCuales" Width="565px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <b>Tipo de Actualización:</b>
                    <asp:RadioButtonList runat="server" ID="OptList" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Actualización datos personales" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Actualización Curriculum Vitae especializaciones" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </center>
</div>
<div id="Div1" runat="server" class="redondo">
    <center>
        <table>
            <tr>
                <td colspan="4">
                        <asp:FileUpload runat="server" ID="UploadFoto" />
                        <telerik:RadButton runat="server" Text="Subir Foto" ID="BtnSubeFoto"></telerik:RadButton>
                        <asp:Image runat="server" ID="ImgFoto" Width="150px" Height="150px" />
                </td>
            </tr>
            <tr>
            <td colspan="4">
                    <asp:Label ID="LblFoto" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
        </table>
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
                    <asp:Label ID="LblMensajeCv" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
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
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTitMedio" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisMedio" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtInsMedio" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label277" runat="server" Text="T.U."></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTitTu" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisTu" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
               <telerik:RadTextBox runat="server"  onkeydown = "return (event.keyCode!=13);" ID="TxtInsTu" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label29" runat="server" Text="Profesional"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTituProfesional" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisProfesiona" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);"  ID="TxtInsPro" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label31" runat="server" Text="Posgrado"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTitPosGrado" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisPosGrado" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtinsPos" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label33" runat="server" Text="Otro"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTitOtro" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtAnisOtro" Width="175px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtInsOtro" Width="175px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadButton runat="server" Text="Grabar Estudios"  ID="BtnSaveEstudi"></telerik:RadButton>
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
        <tr>
            <td colspan="4">
                    <asp:Label ID="LblMensajeCursos" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label30" runat="server" Text="Enfoque del Curso"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtEnfoqueCurso" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <asp:Label ID="Label32" runat="server" onkeydown = "return (event.keyCode!=13);" Text="Institución"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtInstitucion" Width="200px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label34" runat="server" Text="Duración"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtDuracion" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadButton runat="server" ID="BtnGrabaCurso" Text="Agregar"></telerik:RadButton>
            </td>
            <td>
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
        <tr>
            <td colspan="4">
                    <asp:Label ID="LblExperiencia" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label52" runat="server" Text="Actividad"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtActividad" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <asp:Label ID="Label53" runat="server" Text="Entidad"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtEntidad" Width="200px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label54" runat="server" Text="Ubicación área de trabajo"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtUbicacion" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <asp:Label ID="Label55" runat="server" Text="Observaciones"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtObservaciones" Width="200px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadButton runat="server" ID="BtnAddExperiencia" Text="Agregar"></telerik:RadButton>
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
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtInstitucionActual" Width="550px"></telerik:RadTextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:Label runat="server" ID="Label56" Text="Puesto que desempeña:"></asp:Label>
            </td>
            <td colspan="3">
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtPuesto" Width="550px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadButton runat="server" ID="BtnSaveTrabajoActual" Text="Grabar Trabajo Actual"></telerik:RadButton>
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
        <tr>
            <td colspan="4">
                    <asp:Label ID="LblExperienciaReg" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label58" runat="server" Text="Nombre de Finca o Unidad de Manejo"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtNomFinca" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <asp:Label ID="Label59" runat="server" Text="Ubicación"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtUbicacionExpReg" Width="200px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label60" runat="server" Text="Tipo de Actividad"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtTipoActividad" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <asp:Label ID="Label61" runat="server" Text="Area Forestal (ha)"></asp:Label>
            </td>
            <td>
                <telerik:RadNumericTextBox onkeydown = "return (event.keyCode!=13);"  runat="server" ID="TxtArea" Width="200px">
                    <NumberFormat DecimalDigits="2" />
                </telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
          <td>
                <asp:Label ID="Label62" runat="server" Text="Período de Regencia"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox runat="server" onkeydown = "return (event.keyCode!=13);" ID="TxtPeriodo" Width="200px"></telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadButton runat="server" ID="BtnAddExpReg" Text="Agregar"></telerik:RadButton>
            </td>
            <td>
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
    <table>
        <tr>
            <td colspan="4">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                    <asp:Label ID="LblGrabaTodo" runat="server" Font-Bold="true" Visible="false" ForeColor="Red"  Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label64" runat="server" Text="Fecha de Presentación de Actulización"></asp:Label>
            </td>
            <td>
                <telerik:RadDatePicker onkeydown = "return (event.keyCode!=13);" Visible="false" ID="TxtFecPresentacion" Width="200px" runat="server"></telerik:RadDatePicker>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <telerik:RadButton runat="server" ID="BtnGrabarTodo" Text="Grabar Solicitud de Actualización e Imprimir" Height="35px"></telerik:RadButton>
                <telerik:RadButton runat="server" ID="BtnImprimeCV" Visible="false" Text="Imprimir Curriculum" Height="35px"></telerik:RadButton>
                <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Salir" ImageUrl="~/Imagenes/logoout.ico" PostBackUrl="~/Logon.aspx" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <hr />
            </td>
        </tr>
    </table>
    </center>
</div>
<asp:TextBox runat="server" ID="TxtNoRegente" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" ID="TxtCodCategoria" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" ID="TxtYaGrabo" Visible="false"></asp:TextBox>
<asp:TextBox runat="server" ID="TxtNus" Visible="false"></asp:TextBox>
</asp:Content>
