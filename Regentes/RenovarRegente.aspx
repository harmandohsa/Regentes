<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RenovarRegente.aspx.cs" Inherits="Regentes.RenovarRegente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="redondo">
    <center>
        <table>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="FORMULARIO PARA RENOVACIÓN DE VIGENCIA DEL EJERCICIO DE REGENCIA FORESTAL EN ÁREAS PROTEGIDAS" Font-Size="Large"></asp:Label>
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
                    <telerik:RadButton runat="server" Text="Validar" ID="BtnValida" ></telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label3" Visible="false" runat="server" Font-Bold="true" Text="Años a renovar" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadNumericTextBox runat="server" Visible="false" onkeydown = "return (event.keyCode!=13);" ID="TxtAnis" Width="75px">
                        <NumberFormat DecimalDigits="0" />
                    </telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadButton runat="server" Text="Enviar Solicitud" ID="BtnEnviaSol" Visible="false"></telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadButton runat="server" Text="Imprimir Solicitud" ID="BtnPrintSol" Visible="false"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </center>
</div>
<%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  EnableSkinTransparency="true" BackgroundPosition="Top" Skin="">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="cargando" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="BtnEnviaSol">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="BtnEnviaSol" />
                <telerik:AjaxUpdatedControl ControlID="LblMensaje" />
                <telerik:AjaxUpdatedControl ControlID="Label3" />
                <telerik:AjaxUpdatedControl ControlID="TxtAnis" />
                <telerik:AjaxUpdatedControl ControlID="BtnPrintSol" />
                <telerik:AjaxUpdatedControl ControlID="CodRegente" />
                <telerik:AjaxUpdatedControl ControlID="TxtNus" />
                <telerik:AjaxUpdatedControl ControlID="BtnEnviaSol" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>--%>
<asp:TextBox runat="server" Visible="false" ID="CodRegente"></asp:TextBox>
<asp:TextBox runat="server" Visible="false" ID="TxtNus"></asp:TextBox>
</asp:Content>
