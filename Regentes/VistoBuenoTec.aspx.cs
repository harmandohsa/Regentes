using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class VistoBuenoTec : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            GrdDetalle.ItemDataBound += new GridItemEventHandler(GrdDetalle_ItemDataBound);
            Grdhistoria.NeedDataSource += new GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            Grdhistoria.ItemCommand += new GridCommandEventHandler(Grdhistoria_ItemCommand);
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
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2150;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }

        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmienda.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepEnmiendaTec.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
        }

        void Grdhistoria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendatec where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, Grdhistoria);
            }
        }

        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " +  Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
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
                if (Util.ObtieneRegistro("Select * from TDICTAMENTEC  where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "CodRecomendacion").ToString() == "1")
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
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepDictamenTec.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "&nus=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"]);
            }
            else if (e.CommandName == "CmdGood")
            {
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "2")
                {
                    if (Util.ObtieneRegistro("Select * from TDICTAMENTEC  where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "CodRecomendacion").ToString() == "1")
                    {
                        decimal MaxRegHist = Util.MaxCorr("Select Max(Corr) as MAXIMO from THISTREGENTE where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "");
                        StrSql = "insert into THISTREGENTE " + 
                                  "select CODREGENTE," + MaxRegHist + ",CODREG,CODREGEMPF,APELLIDOS,NOMBRES,CODID,NIT,NACIONALIDAD,CODGENERO,FECNAC,CORREO,DIRECCION,CODDEP,CODMUN,TELEFONO,fax,PROFESION,NOCOL,ESPECIALIZACION,EXPERIENCIA,cual,CODIGO,'" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "',CODREGECUT,NOREGENTE,ANIS,CODCATEGORIA,FECACTUALIZACION,CODESTATUS,VERCV " +
                                  "from TREGENTE where codregente =  " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"]+ "";
                        Util.EjecutaIns(StrSql);
                        StrSql = "update TREGENTE set DIRECCION = b.DIRECCION, CODMUN = b.CODMUN, coddep = b.CODDEP, TELEFONO = b.TELEFONO, fax = b.FAX, CORREO = b.CORREO, PROFESION = b.PROFESION, NOCOL = b.NOCOL, " +
                                 "ESPECIALIZACION = b.ESPECIALIZACION, EXPERIENCIA = b.EXPERIENCIA, CUAL = b.CUAL " +
                                 "from TREGENTE a, TACTREGENTE b " +
                                 "where b.CODREGENTE = a.CODREGENTE and a.CODREGENTE = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and b.NUS = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                        Util.EjecutaIns(StrSql);
                        StrSql = "Update tsolicitud set codestatus = 6, FECFIN = '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "' where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                        Util.EjecutaIns(StrSql);
                        string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                        StrSql = "Insert into tvobotec values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                        Util.EjecutaIns(StrSql);
                        StrSql = "Update tregente set fecactualizacion = '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "' where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                        Util.EjecutaIns(StrSql);
                        string CorreoRegente = Util.ObtieneRegistro("Select Correo from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "Correo").ToString();
                        EnvioCorreoRegente(CorreoRegente, e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Nombre"].ToString(),"con exito");
                        GrdDetalle.Rebind();
                        TblHist.Visible = false;
                        Grdhistoria.Visible = false;
                        Label3.Visible = false;
                        LblMensaje.Text = "Visto Bueno Exitoso";
                        LblMensaje.Visible = true;
                    }
                    else
                    {
                        StrSql = "Update tsolicitud set codestatus = -1, FECFIN = '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "' where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                        Util.EjecutaIns(StrSql);
                        string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                        StrSql = "Insert into tvobotec values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                        Util.EjecutaIns(StrSql);
                        string CorreoRegente = Util.ObtieneRegistro("Select Correo from tregente where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "", "Correo").ToString();
                        EnvioCorreoRegente(CorreoRegente, e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Nombre"].ToString(), "sin exito");
                        GrdDetalle.Rebind();
                        TblHist.Visible = false;
                        Grdhistoria.Visible = false;
                        Label3.Visible = false;
                        LblMensaje.Text = "Visto Bueno Exitoso";
                        LblMensaje.Visible = true;
                    }
                    
                }

                else
                {
                    StrSql = "Update tsolicitud set codestatus = 4 where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "";
                    Util.EjecutaIns(StrSql);
                    string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                    StrSql = "Insert into tvobotec values (" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "', " + Session["CodUsuario"] + ", '" + IdElec + "', " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + ")";
                    Util.EjecutaIns(StrSql);
                    string CodRerion = this.Util.CodRegion(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"]));
                    int Dias = Convert.ToInt32(Math.Round(Util.Dias(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString(), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString()), 0));
                    if (Util.ExisteDato("Select * from tusuario where CodTipoUsuario = 2 and CodEstatus = 1 and Codregion = " + CodRerion + "") == true)
                    {
                        StrSql = "Select * from tusuario where CodTipoUsuario = 2 and CodEstatus = 1 and Codregion = " + CodRerion + "";
                        cn.Open();
                        CmTransaccion.CommandText = StrSql;
                        CmTransaccion.Connection = cn;
                        CmTransaccion.CommandType = CommandType.Text;
                        OleDbDataReader reader = CmTransaccion.ExecuteReader();
                        while (reader.Read())
                        {
                            string Tramite = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + " and nus = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"] + "", "CodTramite").ToString();
                            if (Tramite == "1")
                                Tramite = "para ser inscrito como regente";
                            else if (Tramite == "2")
                                Tramite = "de actualización de datos";
                            else if (Tramite == "3")
                                Tramite = "de actualización de datos y categoría";
                            else if (Tramite == "4")
                                Tramite = "de renovación de vigencia";
                            EnvioCorreoTecnico(reader["mail"].ToString(), reader["nombres"] + " " + reader["apellidos"], Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"])),Tramite);
                        }
                        cn.Close();
                    }
                    if (Util.ExisteDato("Select * from tusuario where CodTipoUsuario = 4 and CodEstatus = 1 and Codregion = " + CodRerion + "") == true)
                    {
                        string Cuantas = Util.ObtieneRegistro("select count(*) as cnt from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + CodRerion + ") and c.codestatus = 4", "cnt").ToString();
                        StrSql = "Select * from tusuario where CodTipoUsuario = 4 and CodEstatus = 1 and Codregion = " + CodRerion + "";
                        cn.Open();
                        CmTransaccion.CommandText = StrSql;
                        CmTransaccion.Connection = cn;
                        CmTransaccion.CommandType = CommandType.Text;
                        OleDbDataReader reader = CmTransaccion.ExecuteReader();
                        while (reader.Read())
                        {
                            EnvioCorreoJuridico(reader["mail"].ToString(), reader["nombres"] + " " + reader["apellidos"], Util.NomRegente(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"])), Convert.ToInt32(Cuantas), e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString(),Dias.ToString());
                        }
                        cn.Close();
                    }
                    GrdDetalle.Rebind();
                    TblHist.Visible = false;
                    Grdhistoria.Visible = false;
                    Label3.Visible = false;
                    LblMensaje.Text = "Visto Bueno Exitoso";
                    LblMensaje.Visible = true;
                }
                    
                
                

            }
            else if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                Grdhistoria.Rebind();
                TblHist.Visible = true;
                Grdhistoria.Visible = true;
                Label3.Visible = true;
            }
        }

        private void EnvioCorreoJuridico(string Mail, string Nombre, string Regente, int Cuantas, string Tramite, string dias)
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
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de " + Tramite + ". Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de  " + Tramite + ". Esta lleva " + dias + " días en CONAP, desde su fecha de ingreso. Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
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

        //private void EnvioCorreoJuridico(string Mail, string Nombre, string Regente)
        //{
        //    MailMessage message = new MailMessage();
        //    string content = "";
        //    message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
        //    message.To.Add(Mail);
        //    message.Subject = "Notificacion de Visto Bueno de Dictamen Técnico";
        //    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que la gestión de solicitud para ser inscrito como regente del señor " + Regente + " ya cuenta con visto bueno del dictamen técnico, para que pueda proceder con la elaboración del dictamen juridico</td></tr></table>") +
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

        private void EnvioCorreoRegente(string Mail, string Nombre, string tipo)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de actualización de datos";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que su gestión de solicitud de actualización de datos ha finalizado " + tipo + ", por favor acérquese a la oficina de CONAP para que pueda concluir el trámite.</td></tr></table>") +
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

        private void EnvioCorreoTecnico(string Mail, string Nombre, string Regente, string Tramite)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Visto Bueno de Dictamen Técnico";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que la gestión de solicitud " + Tramite + " del señor " + Regente + " ya cuenta con visto bueno del dictamen técnico</td></tr></table>") +
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

       
        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            //this.StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where a.codestatus = 3 and a.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")  order by a.codestatus desc ";
            this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = 3 and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}