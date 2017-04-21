<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcesaSecretaria.aspx.cs" Inherits="Regentes.ProcesaSecretaria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
        <tr>
            <td>
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Recepción Regentes" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
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
                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Requisitos" Font-Size="Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Solicitud de inscripción en formulario generado por el SEAF-CONAP" ID="ChkSol" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Copia legalizada del Documento Personal de Identificación" ID="ChkCopiaLegal" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Copia legalizada del Título que acredite el grado académico y especializaciones" ID="ChkTitulo" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Constancia original de colegiado activo, mínimo con un mes de vigencia (para profesionales)" ID="ChkConstancia" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Curriculum vitae según formato generado por el SEAF-CONAP" ID="ChkCurri" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Constancia de Cuota de Pago por Registro de Inscripción en BANRURAL y Recibo 63 A2 extendido por CONAP" ID="ChkPago" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Copia de Carné de Identificación Tributaria, extendido por la SAT" ID="ChkNit" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Copia legalizada del certificado de inscripción vigente ante el Registro Nacional Forestal del INAB como Regente Forestal" ID="ChkRegente" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" Text="Copia legalizada del certificado de inscripción vigente ante el Registro Nacional Forestal del INAB como Elaborador de Planes de Manejo Forestal" ID="ChkElabordor" />
            </td>
        </tr>
        <tr> 
            <td>
                <telerik:RadButton runat="server" Text="Procesar" ID="BtnProcesa" ></telerik:RadButton>
            </td>
        </tr>
    </table>
    <asp:TextBox runat="server" Visible="false" ID="TxtCodTramite"></asp:TextBox>
</asp:Content>
