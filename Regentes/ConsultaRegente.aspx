<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaRegente.aspx.cs" Inherits="Regentes.ConsultaRegente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consejo Nacional de Areas Protegidas - Modulo de Regentes[Consulta de Regentes]</title>    
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/logo.jpg" Width="100px" Height="100px" />
            </td>
            <td>
                 <asp:Label ID="Label4" runat="server" Text="PRESIDENCIA DE LA REPÚBLICA DE GUATEMALA" Font-Size="Larger"></asp:Label>
                 <br />
                 <asp:Label ID="Label5" runat="server" Text="CONSEJO NACIONAL DE ÁREAS PROTEGIDAS" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">    
                <asp:Label ID="Label11" runat="server" Text="CONSTANCIA DE INSCRIPCION REGENTES FORESTALES EN ÁREAS PROTEGIDAS" Font-Size="Larger"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image runat="server" ID="ImgRegente" Width="100px" Height="100px"  />
            </td>
            <td>
                <asp:Label ID="lblRegion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label14" Text="No. de Registro CONAP:" runat="server"></asp:Label>
                <asp:Label ID="LblRegConap" Font-Bold="true" Text="REG CONAP" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" Text="No. de Registro RF INAB:" runat="server"></asp:Label>
                <asp:Label ID="LblRegInab" Font-Bold="true" Text="RF INAB" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" Text="No. de Registro EPMF:" runat="server"></asp:Label>
                <asp:Label ID="LblRegEpmf" Font-Bold="true" Text="" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" Text="No. de Registro ECUT:" runat="server"></asp:Label>
                <asp:Label ID="LblRegEcut" Font-Bold="true" Text="" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label16" runat="server" Text="Nombres:"></asp:Label>
                <asp:Label ID="LblNombres" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Apellidos:"></asp:Label>
                <asp:Label ID="LblApellido" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Código Único de Identificación:"></asp:Label>
                <asp:Label ID="LblDui" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:Label ID="Label8" runat="server" Text="Profesión:"></asp:Label>
                <asp:Label ID="lblProfesion" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" Text="Categoría:"></asp:Label>
                <asp:Label ID="LblCategoria" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label12" runat="server" Text="Especialización:"></asp:Label>
                <asp:Label ID="LblEspe" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label13" runat="server" Text="Fecha de Inscripción:"></asp:Label>
                <asp:Label ID="lblFecIns" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label9" runat="server" Text="Fecha de última renovación o actualización de datos:"></asp:Label>
                <asp:Label ID="LblFecLastUpdate" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label15" runat="server" Text="Fecha de vencimiento::"></asp:Label>
                <asp:Label ID="LblFecVen" Font-Bold="true" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
