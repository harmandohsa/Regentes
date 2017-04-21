<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerInfo.aspx.cs" Inherits="Regentes.VerInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table border="2">
    <tr>
        <td>
            <asp:Label runat="server" Text="Código Registro Forestal" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblRegForestal"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Código Registro EMPF" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblRegEmpf"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Código Registro EECUT" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblRegEcut"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Nombres" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblNombre"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Apellidos" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblApellidos"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label7" runat="server" Text="Categoría" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblCategoria"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="DPI" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblDpi"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label8" runat="server" Text="NIT" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="lblNit"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label10" runat="server" Text="Nacionalidad" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblNacionalidad"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Genero" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblGenero"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label11" runat="server" Text="Fecha de nacimiento" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblFecNAc"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label13" runat="server" Text="Correo" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblCorreo"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label9" runat="server" Text="Dirección" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblDireccion"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label14" runat="server" Text="Departamento" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblDepartamento"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label16" runat="server" Text="Municipio" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblMunicipio"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label12" runat="server" Text="Teléfono" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblTel"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label17" runat="server" Text="Fax" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblFax"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label19" runat="server" Text="Profesión" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblPrfesion"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label15" runat="server" Text="No. Colegiado" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblNoCol"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label20" runat="server" Text="Especialización" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblEspecializacion"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label22" runat="server" Text="Experiencia" Font-Bold="true"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" Id="LblExp"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label18" runat="server" Text="Cuales" Font-Bold="true"></asp:Label>
        </td>
        <td colspan="5">
            <asp:Label runat="server" Id="LblCuales"></asp:Label>
        </td>
    </tr>
</table>
</asp:Content>
