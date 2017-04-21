﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepSolicitud.aspx.cs" Inherits="Regentes.RepSolicitud" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <style>
        .classname
        {
             font-weight:bold; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="True" Height="1127px" ReportSourceID="CrystalReportSource1" 
            Width="898px" EnableDatabaseLogonPrompt="False" />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="RptRecepcion.rpt">
            </Report>
        </CR:CrystalReportSource> 
    </div>
    </form>
</body>
</html>
