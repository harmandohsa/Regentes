using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class ProcesaSecretaria : System.Web.UI.Page
    {
        private OleDbCommand CmTransaccion = new OleDbCommand(); 
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private string StrSql;
        private CUtilitarios Util;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.BtnProcesa.Click += new EventHandler(this.BtnProcesa_Click);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                TxtCodTramite.Text = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();
                string TipoAct = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "CODTIPOMOD").ToString();
                
                //if (TxtCodTramite.Text == "2")
                //{
                //    ChkConstancia.Visible = false;
                //}
                if (this.Util.ObtieneRegistro("Select * from tregente where codregente = " + base.Request.QueryString["CodRegente"], "CodCategoria").ToString() == "1")
                {
                    ChkConstancia.Visible = false;
                }
                if (this.Util.ObtieneRegistro("Select * from tregente a, tsolicitud b where a.codregente = b.codregente and nus = " + Request.QueryString["nus"] + " and a.codregente = " + base.Request.QueryString["CodRegente"], "CodEstatus").ToString() != "1")
                {
                    this.Bloquea();
                }
                if (this.Util.ExisteDato("Select * from trecibesecre where codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + ""))
                {
                    if (base.Request.QueryString["llamada"] == "1")
                    {
                        this.CargaData(base.Request.QueryString["CodRegente"].ToString(), base.Request.QueryString["Corr"].ToString(),Request.QueryString["nus"].ToString());
                        this.Bloquea();
                    }
                    else
                    {
                        decimal MaxCorr = this.Util.MaxCorr("Select Max(Corr) as Maximo from trecibesecre where codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"].ToString());
                        decimal CorrEs = MaxCorr - 1;
                        this.CargaData(base.Request.QueryString["CodRegente"].ToString(), CorrEs.ToString(),Request.QueryString["nus"].ToString());
                    }
                }
                OcultaRequisitos(Convert.ToInt32(TxtCodTramite.Text), TipoAct);
            }
        }

        void OcultaRequisitos(int TipoSol, string TipoAct)
        {
            if (TipoSol == 1)
            {
                if (Util.ObtieneRegistro("Select * from tregente where Codregente = " + Request.QueryString["CodRegente"] + "", "CodRegEmpf").ToString() == "")
                    ChkElabordor.Visible = false;
            }
            else if (TipoSol == 2)
            {
                if (TipoAct == "1")
                {
                    ChkCopiaLegal.Visible = false;
                    ChkTitulo.Visible = false;
                    ChkConstancia.Visible = false;
                    ChkCurri.Visible = false;
                    ChkPago.Visible = false;
                    ChkNit.Visible = false;
                }
                else if (TipoAct == "2")
                {
                    ChkConstancia.Visible = false;
                    ChkCurri.Visible = false;
                    ChkNit.Visible = false;
                }
            }
            else if (TipoSol == 3)
            {
                if (TipoAct == "1")
                {
                    ChkTitulo.Visible = false;
                }
                ChkNit.Visible = false;
            }
            else if (TipoSol == 4)
            {
                if (this.Util.ObtieneRegistro("Select * from tregente where codregente = " + base.Request.QueryString["CodRegente"], "CodCategoria").ToString() == "1")
                    ChkConstancia.Visible = false;
                ChkTitulo.Visible = false;
                ChkCurri.Visible = false;
                ChkNit.Visible = false;
            }
        }

        private void Bloquea()
        {
            this.BtnProcesa.Visible = false;
            this.ChkSol.Enabled = false;
            this.ChkCopiaLegal.Enabled = false;
            this.ChkTitulo.Enabled = false;
            this.ChkConstancia.Enabled = false;
            this.ChkCurri.Enabled = false;
            this.ChkPago.Enabled = false;
            this.ChkNit.Enabled = false;
            ChkRegente.Enabled = false;
            ChkElabordor.Enabled = false;
        }

        private void EnvioCorreo(string Mail, string Nombre, string Tramite)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de aceptación de expediente";
            if (Tramite == "1")
                Tramite = "Inscripción";
            else if (Tramite == "2")
                Tramite = "Actualización de Datos";
            else if (Tramite == "3")
                Tramite = "Actualización de Datos y Categoría";
            else if (Tramite == "4")
                Tramite = "Renovación";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que su gestión de solicitud de " + Tramite + " como Regente Forestal en Áreas Protegidas ha sido aceptada exitosamente por el CONAP, la misma continuará con el trámite interno y se notificará cuando finalice el trámite y se emita su Certificación.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
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

        bool Recibe()
        {
            string TipoAct = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "CodTipoMod").ToString();
            if (TxtCodTramite.Text == "1")
            {
                if (this.Util.ObtieneRegistro("Select * from tregente where codregente = " + base.Request.QueryString["CodRegente"], "CodCategoria").ToString() == "1")
                {
                    if (Util.ObtieneRegistro("Select * from tregente where Codregente = " + Request.QueryString["CodRegente"] + "", "CodRegEmpf").ToString() == "")
                    {
                        if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkCurri.Checked == true && ChkPago.Checked == true && ChkNit.Checked == true && ChkRegente.Checked == true)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkCurri.Checked == true && ChkPago.Checked == true && ChkNit.Checked == true && ChkRegente.Checked == true && ChkElabordor.Checked  == true)
                            return true;
                        else
                            return false;
                    }
                    
                }
                else
                {
                    if (Util.ObtieneRegistro("Select * from tregente where Codregente = " + Request.QueryString["CodRegente"] + "", "CodRegEmpf").ToString() == "")
                    {
                        if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkConstancia.Checked == true && ChkCurri.Checked == true && ChkPago.Checked == true && ChkNit.Checked == true && ChkRegente.Checked == true)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkConstancia.Checked == true && ChkCurri.Checked == true && ChkPago.Checked == true && ChkNit.Checked == true && ChkRegente.Checked == true && ChkElabordor.Checked == true)
                            return true;
                        else
                            return false;
                    }
                    
                }
            }
            else if (TxtCodTramite.Text == "2")
            {
                if (TipoAct == "1")
                {
                    if (ChkSol.Checked == true)
                        return true;
                    else
                        return false;
                }
                else if (TipoAct == "2")
                {
                    if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkPago.Checked == true)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
               
            }
            else if (TxtCodTramite.Text == "3")
            {
                if (TipoAct == "1")
                {
                    if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkConstancia.Checked == true && ChkPago.Checked == true && ChkCurri.Checked == true)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkTitulo.Checked == true && ChkConstancia.Checked == true && ChkPago.Checked == true && ChkCurri.Checked == true)
                        return true;
                    else
                        return false;
                }
                
            }
            else if (TxtCodTramite.Text == "4")
            {
                if (this.Util.ObtieneRegistro("Select * from tregente where codregente = " + base.Request.QueryString["CodRegente"], "CodCategoria").ToString() == "1")
                {
                    if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkPago.Checked == true)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (ChkSol.Checked == true && ChkCopiaLegal.Checked == true && ChkPago.Checked == true)
                        return true;
                    else
                        return false;
                }
            }
            else
                return true;
                    
            
        }

        private void EnvioCorreoTecnico(string Mail, string Nombre, int Cuantas, string Tramite, string dias)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de aceptación de expedeinte secretaria";
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

        private void BtnProcesa_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            decimal num;
            string str = base.Request.QueryString["CodRegente"].ToString();
            string nus = base.Request.QueryString["nus"].ToString();
            string idElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
            decimal NoTramite = Util.MaxCorr("Select Max(NoTramite) as Maximo from trecibesecre where YEAR(fecrecibe) = year(getdate())");
            if (Recibe() == true)
            {
                num = this.Util.MaxCorr("Select Max(Corr) as Maximo from trecibesecre where codregente = " + str + " and nus = " + nus);

                this.StrSql = string.Concat(new object[] { 
                    "Insert into trecibesecre values (", str, ", ", num, ", ", this.Util.IIf(this.ChkSol.Checked, 1, 0), ", ", this.Util.IIf(this.ChkCopiaLegal.Checked, 1, 0), ", ", this.Util.IIf(this.ChkTitulo.Checked, 1, 0), ", ", this.Util.IIf(this.ChkConstancia.Checked, 1, 0), ", ", this.Util.IIf(this.ChkCurri.Checked, 1, 0), ", ", this.Util.IIf(this.ChkPago.Checked, 1, 0), 
                    ", ", this.Util.IIf(this.ChkNit.Checked, 1, 0), ", '", string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()), "', '", idElec, "', '", Session["CodUsuario"], "',1, " + nus + ", " + Util.IIf(ChkRegente.Checked,1,0) + ", " + Util.IIf(ChkElabordor.Checked,1,0) + ", " + NoTramite + ")"
                 });
                this.Util.EjecutaIns(this.StrSql);
                //string Maxnus = Util.ObtieneRegistro("Select max(nus) as nus from tsolicitud where codregente = " + str + " and codtramite = " + Session["CodTramite"] + "", "nus").ToString();

                this.StrSql = "Update tsolicitud set codestatus = 2 where codregente = " + str + " and nus = " + nus + "";
                this.Util.EjecutaIns(this.StrSql);
                string Correo = Util.ObtieneRegistro("Select * from tregente where codregente = " + str + "", "Correo").ToString();
                string Nombres = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tregente where codregente = " + str + "", "Nombre").ToString();
                EnvioCorreo(Correo, Nombres, TxtCodTramite.Text);
                string CodRegion = Util.ObtieneRegistro("select * from tusuario where codusuario = " + Session["CodUsuario"] + "", "CodRegion").ToString();
                int Dias = Convert.ToInt32(Math.Round(Util.Dias(str, nus), 0));
                if (Util.ExisteDato("Select * from tusuario where CodTipoUsuario = 2 and CodEstatus = 1 and Codregion = " + CodRegion + "") == true)
                {
                    string Cuantas = Util.ObtieneRegistro("select count(*) as cnt from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + CodRegion + ") and c.codestatus = 2", "cnt").ToString();
                    StrSql = "Select * from tusuario where CodTipoUsuario = 2 and CodEstatus = 1 and Codregion = " + CodRegion + "";
                    cn.Open();
                    CmTransaccion.CommandText = StrSql;
                    CmTransaccion.Connection = cn;
                    CmTransaccion.CommandType = CommandType.Text;
                    OleDbDataReader reader = CmTransaccion.ExecuteReader();
                    while (reader.Read())
                    {
                        EnvioCorreoTecnico(reader["mail"].ToString(), reader["nombres"] + " " + reader["apellidos"], Convert.ToInt32(Cuantas), TxtCodTramite.Text,Dias.ToString());
                    }
                    cn.Close();
                }
                base.Response.Redirect("RecibeSecretaria.aspx?llamada=1&CodRegente=" + str + "&Corr=" + num + "&nus=" + nus + "");
            }
            else
 
            {
                num = this.Util.MaxCorr("Select Max(Corr) as Maximo from trecibesecre where codregente = " + str + " and nus = " + nus);
                this.StrSql = string.Concat(new object[] { 
                        "Insert into trecibesecre values (", str, ", ", num, ", ", this.Util.IIf(this.ChkSol.Checked, 1, 0), ", ", this.Util.IIf(this.ChkCopiaLegal.Checked, 1, 0), ", ", this.Util.IIf(this.ChkTitulo.Checked, 1, 0), ", ", this.Util.IIf(this.ChkConstancia.Checked, 1, 0), ", ", this.Util.IIf(this.ChkCurri.Checked, 1, 0), ", ", this.Util.IIf(this.ChkPago.Checked, 1, 0), 
                        ", ", this.Util.IIf(this.ChkNit.Checked, 1, 0), ", '", string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()), "', '", idElec, "', '", Session["CodUsuario"], "',0," + nus + ", " + Util.IIf(ChkRegente.Checked,1,0) + ", " + Util.IIf(ChkElabordor.Checked,1,0) + ", " + NoTramite + ")"
                     });
                this.Util.EjecutaIns(this.StrSql);
                base.Response.Redirect("RecibeSecretaria.aspx?llamada=1&CodRegente=" + str + "&Corr=" + num + "&nus=" + nus + "");
            }
            
        }

        private void CargaData(string CodRegente, string Corr, string nus)
        {
            this.StrSql = "Select * from trecibesecre where codregente = " + CodRegente + " and Corr = " + Corr + " and nus = " + nus;
            this.cn.Open();
            this.CmTransaccion.CommandText = this.StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                if (reader["SOLICITUD"].ToString() == "1")
                {
                    this.ChkSol.Checked = true;
                }
                if (reader["DOCID"].ToString() == "1")
                {
                    this.ChkCopiaLegal.Checked = true;
                }
                if (reader["TITULO"].ToString() == "1")
                {
                    this.ChkTitulo.Checked = true;
                }
                if (reader["COLEGIADO"].ToString() == "1")
                {
                    this.ChkConstancia.Checked = true;
                }
                if (reader["CURRI"].ToString() == "1")
                {
                    this.ChkCurri.Checked = true;
                }
                if (reader["PAGO"].ToString() == "1")
                {
                    this.ChkPago.Checked = true;
                }
                if (reader["NIT"].ToString() == "1")
                {
                    this.ChkNit.Checked = true;
                }
                if (reader["Regente"].ToString() == "1")
                {
                    ChkRegente.Checked = true;
                }
                if (reader["Elaborador"].ToString() == "1")
                {
                    ChkElabordor.Checked = true;
                }
            }
            reader.Close();
            this.cn.Close();
        }

    }
}