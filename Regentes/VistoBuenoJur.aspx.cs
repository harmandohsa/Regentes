using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class VistoBuenoJur : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            GrdDetalle.ItemDataBound += new GridItemEventHandler(GrdDetalle_ItemDataBound);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);
            Grdhistoria.NeedDataSource += new GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            Grdhistoria.ItemCommand += new GridCommandEventHandler(Grdhistoria_ItemCommand);

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

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2200;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " +  Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                item["Print"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
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
                if (Util.ObtieneRegistro("Select * from tresolucion  where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "CodRecomendacion").ToString() == "1")
                    item["Est"].Text = "Procedente";
                else
                    item["Est"].Text = "No Procedente";
            }
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
            
        }

        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdGood")
            {

                StrSql = "Update tsolicitud set codestatus = 6, FECFIN = '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "' where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"];
                Util.EjecutaIns(StrSql);
                string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                string CodRegion = Util.CodRegion(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"]));
                if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and CodRegion = " + CodRegion + " and codestatus = 1") == true)
                {
                    string CodUsrDirReg = Util.ObtieneRegistro("Select * from tusuario where codtipousuario = 3 and CodRegion = " + CodRegion + " and codestatus = 1", "CodUsuario").ToString();
                    string IdElecDirReg = Util.GeneraIdElec(Convert.ToInt32(CodUsrDirReg));
                    StrSql = "Insert into tvoboleg values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " + CodUsrDirReg + ", '" + IdElecDirReg + "', " +  e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + " )";
                }
                else
                {
                    StrSql = "Insert into tvoboleg values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " +  e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                }

                Util.EjecutaIns(StrSql);
                string CodRecomendacion = Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " +  e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "CodRecomendacion").ToString();
                string CodRecomendacionLegal = Util.ObtieneRegistro("Select * from tresolucion where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "CodRecomendacion").ToString();
                if (CodRecomendacion == "1" && CodRecomendacionLegal == "1")
                {
                    if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "1" || e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "4")
                    {
                        string Vigencia = "";
                        if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "1")
                        {
                            decimal MaxPeriodo = Util.MaxCorr("Select Max(Corr) as MAXIMO from tperiodo where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "");
                            Vigencia = Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "Vigencia").ToString();

                            DateTime Hasta = DateTime.Now.AddYears(Convert.ToInt32(Vigencia));
                            string CodUsuarioReg = Util.ObtieneRegistro("Select * from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "CodId").ToString();
                            string IdUnico = Util.GeneraIdunico(CodUsuarioReg);

                            StrSql = "Insert into tperiodo values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", " + MaxPeriodo + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", Hasta) + "',1, '" + IdUnico + "',0)";
                            Util.EjecutaIns(StrSql);
                            decimal NoRegente = Util.MaxCorr("Select Max(Codigo) as MAXIMO from tregente");
                            StrSql = "Update tregente set codigo = " + NoRegente + " where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                            Util.EjecutaIns(StrSql);
                        }
                        else
                        {

                            decimal MaxPeriodo = Util.MaxCorr("Select Max(Corr) as MAXIMO from tperiodo where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "");
                            Vigencia = Util.ObtieneRegistro("Select * from trenovacion where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "Anis").ToString();
                            DateTime Hasta = DateTime.Now.AddYears(Convert.ToInt32(Vigencia));
                            string CodUsuarioReg = Util.ObtieneRegistro("Select * from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "CodId").ToString();
                            string IdUnico = Util.ObtieneRegistro("Select * from tperiodo where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "Idunico").ToString();
                            StrSql = "Update tperiodo set codestatus = 0 where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                            Util.EjecutaIns(StrSql);
                            StrSql = "Insert into tperiodo values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", " + MaxPeriodo + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", Hasta) + "',1, '" + IdUnico + "',0)";
                            Util.EjecutaIns(StrSql);
                        }

                    }
                    else
                    {
                        decimal MaxRegHist = Util.MaxCorr("Select Max(Corr) as MAXIMO from THISTREGENTE where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "");
                        StrSql = "insert into THISTREGENTE " +
                                  "select CODREGENTE," + MaxRegHist + ",CODREG,CODREGEMPF,APELLIDOS,NOMBRES,CODID,NIT,NACIONALIDAD,CODGENERO,FECNAC,CORREO,DIRECCION,CODDEP,CODMUN,TELEFONO,fax,PROFESION,NOCOL,ESPECIALIZACION,EXPERIENCIA,cual,CODIGO,'" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "',CODREGECUT,NOREGENTE,ANIS,CODCATEGORIA,FECACTUALIZACION,CODESTATUS,VERCV " +
                                  "from TREGENTE where codregente =  " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                        Util.EjecutaIns(StrSql);
                        StrSql = "update TREGENTE set DIRECCION = b.DIRECCION, CODMUN = b.CODMUN, coddep = b.CODDEP, TELEFONO = b.TELEFONO, fax = b.FAX, CORREO = b.CORREO, PROFESION = b.PROFESION, NOCOL = b.NOCOL, " +
                                 "ESPECIALIZACION = b.ESPECIALIZACION, EXPERIENCIA = b.EXPERIENCIA, CUAL = b.CUAL " +
                                 "from TREGENTE a, TACTREGENTE b " +
                                 "where b.CODREGENTE = a.CODREGENTE and a.CODREGENTE = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and b.NUS = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                        Util.EjecutaIns(StrSql);
                    }

                }
                else
                {
                    if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "3")
                    {
                        StrSql = "Update tregente set codcategoria = 1 where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                        Util.EjecutaIns(StrSql);
                    }
                    StrSql = "Update tsolicitud set codestatus = -1, FECFIN = '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "' where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"];
                    Util.EjecutaIns(StrSql);
                }
                string CodRerion = this.Util.CodRegion(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"]));
                string CodUsrJuridico = this.Util.ObtieneRegistro("Select * from tusuario where codregion = " + CodRerion + " and CodtipoUsuario = 4 and codestatus = 1", "CodUsuario").ToString();

                
                string Tramite = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "CodTramite").ToString();
                if (Tramite == "1")
                    Tramite = "para ser inscrito como regente";
                else if (Tramite == "2")
                    Tramite = "de actualización de datos";
                else if (Tramite == "3")
                    Tramite = "de actualización de datos y categoría";
                else if (Tramite == "4")
                    Tramite = "de renovación de vigencia";
                
                string Servidor = System.Configuration.ConfigurationManager.AppSettings["Server"].ToString();
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "1")
                {
                    QrCode.Text = "http://" + Servidor + "/ConsultaRegente.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                    //QrCode.Text = "http://186.151.231.171/Regentes_Conap/ConsultaRegente.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                    string PathArchivo = Server.MapPath(".") + @"\CodigosQR\" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                    Directory.CreateDirectory(PathArchivo);
                    string FileName = PathArchivo + @"\" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ".jpg";

                    System.Drawing.Image image = QrCode.GetImage();
                    image.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                try
                {
                    EnvioCorreoJuridico(Util.ObtieneRegistro("Select * from tusuario where codusuario = " + CodUsrJuridico + "", "Mail").ToString(), Util.NomUsuario(Convert.ToInt32(CodUsrJuridico)), Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"])), Tramite);
                    EnvioCorreoRegente(Util.ObtieneRegistro("Select * from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "Correo").ToString(), Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"])), Tramite);
                }
                catch 
                {

                }
                LblMensaje.Text = "Visto Bueno Exitoso";
                LblMensaje.Visible = true;
                GrdDetalle.Rebind();
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

        private void EnvioCorreoJuridico(string Mail, string Nombre, string Regente, string Tramite)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Visto Bueno de Dictamen Jurídico";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que la gestión de solicitud " + Tramite + " del señor " + Regente + " ya cuenta con visto bueno del dictamen jurídico.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Areas Protegidas –CONAP-.</td></tr><tr><td>5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
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

        private void EnvioCorreoRegente(string Mail, string Regente, string Tramite)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Finalización de Tramite";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Regente + "</td></tr><tr><td>Le informamos que su gestión de solicitud " + Tramite + " ha finalizado, por favor acerquese a la oficina de CONAP para que pueda concluir el trámite.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Regente + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Areas Protegidas –CONAP-.</td></tr><tr><td>5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
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
            //this.StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where a.codestatus = 5 and a.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")  order by a.codestatus desc ";
            this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = 5 and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}