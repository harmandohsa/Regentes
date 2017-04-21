using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data;
using System.Data.OleDb;

namespace Regentes
{
    public partial class EnmiendaJur : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemDataBound += new GridItemEventHandler(this.GrdDetalle_ItemDataBound);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            Grdhistoria.NeedDataSource += new GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            Grdhistoria.ItemCommand += new GridCommandEventHandler(Grdhistoria_ItemCommand);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);
            Grdhistoria.ItemDataBound += new GridItemEventHandler(Grdhistoria_ItemDataBound);

            if (Session["CodUsuario"] == null)
            {
                Response.Redirect("logon.aspx");
            }
            else if (!IsPostBack)
            {
                Util.EstablecePermisos(Master, Convert.ToInt32(Session["CodUsuario"]));
                Label label = (Label)Master.FindControl("LblUsuario");
                label.Text = Util.NomUsuario(int.Parse(Session["CodUsuario"].ToString()));
            }
        }

        void Grdhistoria_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item.GetDataKeyValue("ENVIO").ToString() == "1")
                    item["Del"].FindControl("ImgDel").Visible = false;
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2150;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmiendaLeg.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepEnmiendaLeg.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
            if (e.CommandName == "CmdSend")
            {
                StrSql = "Update TENMIENDALEG set envio = 1 where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + " and Corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                string Mail = Util.ObtieneRegistro("Select * from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"] + "", "correo").ToString();
                string Nombre = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' +  isnull(apellidos,'') as nombre from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"] + "", "nombre").ToString();
                string Delegacion = Util.ObtieneRegistro("Select nombre from tregion where CodRegion = " + Util.ObtieneRegistro("Select CodRegion from tusuario where CodUsuario = " + Session["CodUsuario"] + "", "CodRegion") + "", "nombre").ToString();
                MailMessage message = new MailMessage();
                string content = "";
                message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
                message.To.Add(Mail);
                message.Subject = "Notificacion de Enmiendas Legales de Inscripción de Regente";
                content = "<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>La" + Delegacion + " del Consejo Nacional de Áreas Protegidas hace constar que la documentación presentada para Inscripción como Regente Forestal en Áreas Protegidas NO cumple con los requisitos establecidos por CONAP, por lo que no ha sido ingresada. Es necesario que se atienda lo siguiente:</td></tr></table><table>";
                StrSql = "Select * from tdetenmiendaleg where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"] + " and Corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"] + " and nus = " + TxtNus.Text + "";
                cn.Open();
                CmTransaccion.CommandText = this.StrSql;
                CmTransaccion.Connection = this.cn;
                CmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
                while (reader.Read())
                {
                    content = content + "<tr><td>" + reader["enmienda"] + "</td></tr>";

                }
                cn.Close();
                content = content + "</table><table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>" + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Areas Protegidas –CONAP-.</td></tr><tr><td>5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
                AlternateView item = null;
                item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
                LinkedResource resource = null;
                resource = new LinkedResource(base.Server.MapPath("Imagenes/principalfinal20122.gif"))
                {
                    ContentId = "imagen001"
                };
                //item.LinkedResources.Add(resource);
                message.AlternateViews.Add(item);
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings["Host"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Puerto"]));

                smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], System.Configuration.ConfigurationManager.AppSettings["Clave"]);
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;

                ServicePointManager.ServerCertificateValidationCallback =

                   delegate(object s

                       , X509Certificate certificate

                       , X509Chain chain

                       , SslPolicyErrors sslPolicyErrors)

                   { return true; };
                smtp.Send(message);
                LblMensaje.Text = "Enmiendas enviadas";
                LblMensaje.Visible = true;
                Grdhistoria.Rebind();
                this.AbreVentana("RepEnmiendaLeg.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
            if (e.CommandName == "CmdDel")
            {
                StrSql = "Delete from TDETENMIENDALEG where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"] + " and corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"] + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                StrSql = "Delete from TENMIENDALEG where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"] + " and corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"] + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                Grdhistoria.Rebind();
            }
            
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        void Grdhistoria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus,ENVIO " +
                         "from TENMIENDALEG where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, Grdhistoria);
            }

        }

        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CmdProcesar")
            {

                if (Util.ExisteDato("Select * from TENMIENDALEG where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and Codestatus = 1 and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "") == true)
                {
                    LblMensaje.Text = "Esta solicitud tiene enmiendas abiertas ";
                    LblMensaje.Visible = true;
                }
                else
                    base.Response.Redirect("SolicitaEnmiendaLeg.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&nus= " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "");
            }
            else if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                Grdhistoria.Rebind();
                TblHist.Visible = true;
                Grdhistoria.Visible = true;
            }
        }

        private void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " + Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                item["FecRecep"].Text = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FECRECIBE,103) as FECRECIBE from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FECRECIBE").ToString();
                if (Util.ObtieneRegistro("Select * from tsolicitud where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "CodEstatus").ToString() == "6")
                {
                    Int32 dias = Convert.ToInt32(Math.Truncate(Util.CalculaDias(Convert.ToDateTime(Util.ObtieneRegistro("Select * from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecRecibe")), Convert.ToDateTime(Util.ObtieneRegistro("Select * from TSOLICITUD where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecFin")))));
                    if (dias == 0)
                        item["dia"].Text = "0";
                    else
                        item["dia"].Text = (dias).ToString();

                }
                else
                {
                    Int32 dias2 = Convert.ToInt32(Math.Truncate(Util.CalculaDias(Convert.ToDateTime(Util.ObtieneRegistro("Select * from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecRecibe")), Util.FechaDB())));
                    if (dias2 == 0)
                        item["dia"].Text = "0";
                    else
                        item["dia"].Text = (dias2).ToString();
                }
            }
        }

        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            //this.StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where a.codestatus = 4 and a.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")  order by a.codestatus desc ";
            this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = 4 and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1  order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }

    }
}