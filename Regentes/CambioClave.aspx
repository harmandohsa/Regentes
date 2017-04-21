<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CambioClave.aspx.cs" Inherits="Regentes.CambioClave" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <table>
        <tr>
            <td colspan="2">
                Cambio de Clave
            </td>
        </tr>
        <tr>
            <td>
                Clave Anterior:
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtClaveAnt" onkeydown = "return (event.keyCode!=13);" TabIndex="1"  TextMode="Password" Width="199px"></telerik:RadTextBox>
            </td>
        </tr>
         <tr>
            <td>
                Nueva Clave:
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtNuevaClave" onkeydown = "return (event.keyCode!=13);"  TabIndex="2"  TextMode="Password" Width="199px"></telerik:RadTextBox>
            </td>
        </tr>
         <tr>
            <td>
                Confirme Clave:
            </td>
            <td>
                <telerik:RadTextBox runat="server" ID="TxtConfClave" TabIndex="3" onkeydown = "return (event.keyCode!=13);"  TextMode="Password" Width="199px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton runat="server" ID="LnkEnvia" Text="Cambiar de Clave"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblmensaje" Font-Bold="true" Font-Size="Large" Visible="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
