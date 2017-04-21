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
    public partial class VerJuridico : System.Web.UI.Page
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
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2100;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdProcesar")
            {
                if (Util.ExisteDato("Select * from TENMIENDALEG where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + " and Codestatus = 1") == true)
                {
                    LblMensaje.Text = "Esta solicitud tiene enmiendas abiertas";
                    LblMensaje.Visible = true;
                }
                else
                {
                    if (Util.ExisteDato("Select * from tdictamenleg where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "") != true)
                    {
                        string idElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                        StrSql = "Insert into tdictamenleg (codRegente,idelec,feccre,codusuario,nus) values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + idElec + "','" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                        Util.EjecutaIns(StrSql);
                    }
                    string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "", "CodUsuario").ToString();
                    if (CodUsuarioHizo == "" || CodUsuarioHizo == Session["CodUsuario"].ToString())
                        base.Response.Redirect("DictamenJur.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&nus=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"]);
                    else
                    {
                        LblMensaje.Text = "El dictamen de esta solicitud fue realizado por otro usuario";
                        LblMensaje.Visible = true;
                    }
                    
                }

            }
            else if (e.CommandName == "CmdResolucion")
            {
                if (Util.ExisteDato("Select * from TENMIENDALEG where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " +  e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + " and Codestatus = 1") == true)
                {
                    LblMensaje.Text = "Esta solicitud tiene enmiendas abiertas";
                    LblMensaje.Visible = true;
                }
                else
                {
                    if (Util.ExisteDato("Select * from tresolucion where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "") != true)
                    {
                        string idElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                        StrSql = "Insert into tresolucion (codRegente,idelec,feccre,codusuario,nus) values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + idElec + "','" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                        Util.EjecutaIns(StrSql);
                    }
                    string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "", "CodUsuario").ToString();
                    if (CodUsuarioHizo == "" || CodUsuarioHizo == Session["CodUsuario"].ToString())
                        base.Response.Redirect("Resolucion.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&nus=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"]);
                    else
                    {
                        LblMensaje.Text = "El dictamen de esta solicitud fue realizado por otro usuario, la resolución también debe ser echa por el mismo usuario ";
                        LblMensaje.Visible = true;
                    }
                    
                }
                
            }
            else if (e.CommandName == "CmdSend")
            {
                string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "", "CodUsuario").ToString();
                if (CodUsuarioHizo == "" || CodUsuarioHizo == Session["CodUsuario"].ToString())
                {
                    string CodUsrDirector = "";
                    StrSql = "Update tsolicitud set codestatus = 5 where codRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                    Util.EjecutaIns(StrSql);
                    string CodRerion = this.Util.CodRegion(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"]));
                    string Cuantas = Util.ObtieneRegistro("select count(*) as cnt from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + CodRerion + ") and c.codestatus = 5", "cnt").ToString();
                    int Dias = Convert.ToInt32(Math.Round(Util.Dias(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString(), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString()), 0));
                    if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and CodRegion = " + CodRerion + " and codestatus = 1") == true)
                        CodUsrDirector = this.Util.ObtieneRegistro("Select * from tusuario where codregion = " + CodRerion + " and CodtipoUsuario = 5 and codestatus = 1", "CodUsuario").ToString();
                    else
                        CodUsrDirector = this.Util.ObtieneRegistro("Select * from tusuario where codregion = " + CodRerion + " and CodtipoUsuario = 3 and codestatus = 1", "CodUsuario").ToString();
                    EnvioCorreo(Util.ObtieneRegistro("Select * from tusuario where codusuario = " + CodUsrDirector + "", "Mail").ToString(), Util.NomUsuario(Convert.ToInt32(CodUsrDirector)), Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"])), Convert.ToInt32(Cuantas), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["tipotramite"].ToString(),Dias.ToString());
                    GrdDetalle.Rebind();
                    LblMensaje.Text = "Dictamen Legal y Resolución enviados exitosamente";
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
            GridDataItem item = e.Item as GridDataItem;
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                item["nusfor"].Text = "RRAP - " +  Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                if (Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "NoDictamen").ToString() != "")
                {
                    item["PrintDic"].FindControl("ImgPrintDictamen").Visible = true;
                }
                if (Util.ObtieneRegistro("Select * from tresolucion where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "NoResolucion").ToString() != "")
                {
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = true;
                }
                if ((Util.ObtieneRegistro("Select * from tresolucion where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "NoResolucion").ToString() != "") && (Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + item.GetDataKeyValue("CodRegente") + "", "NoDictamen").ToString() != ""))
                {
                    item["SendDir"].FindControl("ImgSend").Visible = true;
                }
                item["PrintDic"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintRes"].Attributes.Add("onclick", "javascript:url2('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
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
            this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus in (4,5) and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }

        private void EnvioCorreo(string Mail, string Nombre, string Regente, int Cuantas, string Tramite, string dias)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Ingreso de nueva solicitud";
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
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de " + Tramite + " para su visto Bueno. Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de  " + Tramite + " para su visto Bueno. Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso. Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
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
            //new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["Host"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Puerto"])) { Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], System.Configuration.ConfigurationManager.AppSettings["Clave"]) }.Send(message);
        }

        //private void EnvioCorreo(string Mail, string Nombre, string Regente, int Cuantas, string tramite)
        //{
        //    MailMessage message = new MailMessage();
        //    string content = "";
        //    message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
        //    message.To.Add(Mail);
        //    message.Subject = "Notificacion de Emisión de Dictamen Jurídico";
        //    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que la gestión de solicitud para ser inscrito como regente del señor " + Regente + " ya cuenta con dictamen jurídico y resolución para que pueda proceder a dar su Visto Bueno</td></tr></table>") +
        //                "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Areas Protegidas –CONAP-.</td></tr><tr><td>5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
        //    AlternateView item = null;
        //    item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
        //    LinkedResource resource = null;
        //    resource = new LinkedResource(base.Server.MapPath("Imagenes/principalfinal20122.gif"))
        //    {
        //        ContentId = "imagen001"
        //    };
        //    //item.LinkedResources.Add(resource);
        //    message.AlternateViews.Add(item);
        //    message.IsBodyHtml = true;
        //    message.Priority = MailPriority.High;
        //    new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["Host"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Puerto"])) { Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], System.Configuration.ConfigurationManager.AppSettings["Clave"]) }.Send(message);
        //}
    }
}