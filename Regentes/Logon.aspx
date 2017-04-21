<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Regentes.Logon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div>
    <hr />
</div>

<div class="redondo">
    <center> 
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Administración módulo regentes" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="LblMensaje" Font-Bold="true" Visible="false" runat="server" ForeColor="Red" Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Usuario"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="TxtUsuario" TabIndex="1" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Clave"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="TxtClave" TextMode="Password" TabIndex="2" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <telerik:RadButton runat="server" Text="Ingresar" ID="BtnIngresa" TabIndex="3"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </center>
</div>
<div id="caja">
    <asp:ImageButton ImageAlign="Middle" runat="server" Width="150px" Height="150px" ID="NuevoReg" ImageUrl="~/Imagenes/NuevoReg.gif" TabIndex="4" PostBackUrl="~/NuevoRegente.aspx"  />
    <asp:ImageButton ImageAlign="Middle" runat="server" Width="150px" Height="150px" ID="ActReg" ImageUrl="~/Imagenes/ActReg.gif" TabIndex="5" PostBackUrl="~/ValidaRegente.aspx"  />
    <asp:ImageButton ImageAlign="Middle" runat="server" Width="150px" Height="150px" ID="RenReg" ImageUrl="~/Imagenes/RenReg.gif" TabIndex="6" PostBackUrl="~/RenovarRegente.aspx"  />
</div>
<div id="caja">
    <a target="_blank" href="ListadoRegentes.aspx" id="LnkListado">Listado Regentes Activos Nivel Nacional</a>
</div>
</asp:Content>
