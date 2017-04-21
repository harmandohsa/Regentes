<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ValidaRegente.aspx.cs" Inherits="Regentes.ValidaRegente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="redondo">
    <center>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="FORMULARIO DE SOLICITUD DE ACTUALIZACIÓN DE DATOS DEL REGISTRO DE REGENTES FORESTALES EN ÁREAS PROTEGIDAS" Font-Size="Large"></asp:Label>
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
                <td colspan="4">
                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="En la Constancia de Inscripción como regente hay un texto único, ubiquelo y digitelo en el cuadro de abajo" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4"> 
                    <telerik:RadTextBox runat="server"  Font-Size="Large" ID="TxtIdUnico" onkeydown = "return (event.keyCode!=13);" Width="400px" Height="40px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadButton runat="server" Text="Validar" ID="BtnValida"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </center>
</div>
</asp:Content>
