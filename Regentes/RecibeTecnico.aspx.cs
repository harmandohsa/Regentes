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

namespace Regentes
{
    public partial class RecibeTecnico : System.Web.UI.Page
    {
        private string StrSql; 
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemDataBound += new GridItemEventHandler(this.GrdDetalle_ItemDataBound);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);

            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2000;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdProcesar")
            {
                string CodUsuarioHizoEnminada = Util.ObtieneRegistro("Select * from tenmiendatec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "", "CodUsuario").ToString();
                if (CodUsuarioHizoEnminada == "" || CodUsuarioHizoEnminada == Session["CodUsuario"].ToString())
                {
                    if (Util.ExisteDato("Select * from tenmiendatec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + " and Codestatus = 1") == true)
                    {
                        LblMensaje.Text = "Esta solicitud tiene enmiendas abiertas";
                        LblMensaje.Visible = true;
                    }
                    else
                    {
                        string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "", "CodUsuario").ToString();
                        if (CodUsuarioHizo == "" || CodUsuarioHizo == Session["CodUsuario"].ToString())
                            base.Response.Redirect("ProcesaTecnico.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&nus=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString());
                        else
                        {
                            LblMensaje.Text = "El dictamen de esta solicitud fue realizado por otro usuario";
                            LblMensaje.Visible = true;
                        }
                    }
                }
                else
                {
                    LblMensaje.Text = "Esta solicitud está siendo atendida por otro usuario";
                    LblMensaje.Visible = true;
                }

            }
            else if (e.CommandName == "CmdSend")
            {
                string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "","CodUsuario").ToString();
                if (CodUsuarioHizo == "" || CodUsuarioHizo == Session["CodUsuario"].ToString())
                {
                    string CodUsrDirector = "";
                    string CodRerion = this.Util.CodRegion(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString()));
                    CodUsrDirector = this.Util.ObtieneRegistro("Select * from tusuario where codregion = " + CodRerion + " and CodtipoUsuario = 3 and codestatus = 1", "CodUsuario").ToString();
                    StrSql = "Update tsolicitud set codestatus = 3 where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "";
                    Util.EjecutaIns(StrSql);
                    int Dias = Convert.ToInt32(Math.Round(Util.Dias(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString(), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString()), 0));
                    string Cuantas = Util.ObtieneRegistro("select count(*) as cnt from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + CodRerion + ") and c.codestatus = 3", "cnt").ToString();
                    EnvioCorreo(Util.ObtieneRegistro("Select * from tusuario where codusuario = " + CodUsrDirector + "", "Mail").ToString(), Util.NomUsuario(Convert.ToInt32(CodUsrDirector)), Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString())), Convert.ToInt32(Cuantas), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString(),Dias.ToString());
                    GrdDetalle.Rebind();
                    LblMensaje.Text = "Dictamen Técnico enviado exitosamente";
                    LblMensaje.Visible = true;
                }
                else
                {
                    LblMensaje.Text = "El dictamen de esta solicitud fue realizado por otro usuario, no puede enviarlo.";
                    LblMensaje.Visible = true;
                }
                
            }
        }

        private void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " + Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                if (Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "NoDictamen").ToString() != "")
                {
                    item["PrintDic"].FindControl("ImgPrintDictamen").Visible = true;
                    item["SendDir"].FindControl("ImgSend").Visible = true;
                }
                item["PrintDic"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "');");
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

        private void EnvioCorreo(string Mail, string Nombre, string Regente, int Cuantas, string Tramite, string dias)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de Visto Bueno.";
            Cuantas = Cuantas - 1;
            if (Tramite == "1")
                Tramite = "Inscripción";
            else if (Tramite == "2")
                Tramite = "Actualización de Datos";
            else if (Tramite == "3")
                Tramite = "Actualización de Datos y Categoría";
            else if (Tramite == "4")
                Tramite = "Renovación";
            if (Cuantas <= 0)
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de " + Tramite + " para su visto bueno. Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de  " + Tramite + " para su visto bueno. Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso. Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
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
        }

        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            //this.StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where a.codestatus in (2,3) and a.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")  order by a.codestatus desc ";
            this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus  in (2,3,4,5) and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}