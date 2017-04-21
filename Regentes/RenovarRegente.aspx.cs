using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;



namespace Regentes
{
    public partial class RenovarRegente : System.Web.UI.Page
    {
        CUtilitarios Util;
        string StrSql;
        OleDbCommand CmTransaccion = new OleDbCommand();
        OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            BtnValida.Click += new EventHandler(BtnValida_Click);
            BtnEnviaSol.Click += new EventHandler(BtnEnviaSol_Click);
            BtnPrintSol.Click += new EventHandler(BtnPrintSol_Click);
        }

        void BtnPrintSol_Click(object sender, EventArgs e)
        {
            AbreVentana("RepSolicitud.aspx?CodRegente=" + CodRegente.Text + "&nus=" + TxtNus.Text + "&tramite=4");
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        void BtnEnviaSol_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (TxtAnis.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar la cantidad de años a renovar";
                LblMensaje.Visible = true;
            }
            else if (Util.ExisteDato("Select * from tsolicitud where Codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + " and codtramite = 4 and codestatus in (-2,-1,1,2,3,4,5)") == true)
            {
                LblMensaje.Text = "Actualmente hay una solicitud de renovación en curso, no puede enviar otra.";
                LblMensaje.Visible = true;
            }
            else if (Convert.ToInt32(TxtAnis.Text) <= 0)
            {
                LblMensaje.Text = "El periodo de años no puede ser menor o igual a 0";
                LblMensaje.Visible = true;
                
            }
            else if (Convert.ToInt32(TxtAnis.Text) > 4)
            {
                LblMensaje.Text = "El periodo de años no puede ser mayor a 4";
                LblMensaje.Visible = true;

            }
            else
            {
                decimal MaxNus = Util.MaxCorr("Select Max(nus) as MAXIMO from tsolicitud");

                StrSql = "Insert into tsolicitud values (" + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + "," + MaxNus + ",4, '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "', 1,NULL,0)";
                Util.EjecutaIns(StrSql);
                StrSql = "Insert into TRENOVACION values (" + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + "," + MaxNus + "," + TxtAnis.Text + ")";
                Util.EjecutaIns(StrSql);
                string Region = Util.ObtieneRegistro("select * from tdictamentec a, tregion b where a.codregion = b.codregion and codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + "", "Region").ToString();
                string CodRegion = Util.ObtieneRegistro("select * from tdictamentec a, tregion b where a.codregion = b.codregion and codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + "", "CodRegion").ToString();
                Region = Region + "(" + Util.ObtieneRegistro("select * from tdictamentec a, tregion b where a.codregion = b.codregion and codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "","CodRegente") + "", "Dep").ToString() + ")";
                string Correo = Util.ObtieneRegistro("Select * from tregente where CodRegente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente"), "Correo").ToString();
                EnvioCorreo(Correo, Util.NomRegente(Convert.ToInt32(Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente"))), Region);
                if (Util.ExisteDato("Select * from tusuario where CodTipoUsuario = 1 and CodEstatus = 1 and Codregion = " + CodRegion + "") == true)
                {
                    string Cuantas = Util.ObtieneRegistro("select count(*) as cnt from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + CodRegion + ") and c.codestatus = 1 and a.CODESTATUS = 1", "cnt").ToString();
                    StrSql = "Select * from tusuario where CodTipoUsuario = 1 and CodEstatus = 1 and Codregion = " + CodRegion + "";
                    cn.Open();
                    CmTransaccion.CommandText = StrSql;
                    CmTransaccion.Connection = cn;
                    CmTransaccion.CommandType = CommandType.Text;
                    OleDbDataReader reader = CmTransaccion.ExecuteReader();
                    while (reader.Read())
                    {
                        EnvioCorreoSecretaria(reader["mail"].ToString(), reader["nombres"] + " " + reader["apellidos"], Convert.ToInt32(Cuantas));
                    }
                    cn.Close();
                }
                LblMensaje.Text = "Solicitud enviada a CONAP, por favor no olvide imprimir su solicitud";
                LblMensaje.Visible = true;
                AbreVentana("RepSolicitud.aspx?CodRegente=" + Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente") + "&nus=" + MaxNus + "&tramite=4");
                Label3.Visible = false;
                TxtAnis.Visible = false;
                BtnEnviaSol.Visible = false;
                CodRegente.Text = Util.ObtieneRegistro("Select * from tperiodo where idunico = " + TxtIdUnico.Text + "", "CodRegente").ToString();
                TxtIdUnico.Text = "";
                TxtNus.Text = MaxNus.ToString();
                //BtnPrintSol.Visible = true;
            }
        }

        private void EnvioCorreoSecretaria(string Mail, string Nombre, int Cuantas)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de inscripción de Regente";
            Cuantas = Cuantas - 1;
            if (Cuantas <= 0)
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de renovación de vigencia de Regente Forestal.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de renovación de vigencia de Regente Forestal. Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
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

        private void EnvioCorreo(string Mail, string Nombre, string Region)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de Renovación de Regencia";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para renovación como regente forestal ha sido enviada exitosamente a CONAP, puede presentarse a la oficina regional: " + Region + ". Para poder dar continuidad al trámite de su solicitud es necesario presentar la siguiente documentación: </td></tr><tr><td>Solicitud escrita en Formulario del Consejo Nacional de Áreas Protegidas</td></tr><tr><td>Copia legalizada del Documento Personal de Identificación</td></tr><tr><td>Constancia original de colegiado activo (para profesionales)</td></tr><tr><td><tr><td>-  Constancia de Cuota de Pago por Registro de Inscripción en BANRURAL y Recibo 63 A2 extendido por CONAP. Cuenta Fondos Privativos CONAP, Banrural No. 3-099-03774-6</td></tr></td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
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

        void BtnValida_Click(object sender, EventArgs e)
        {
            Label3.Visible = false;
            TxtAnis.Visible = false;
            BtnEnviaSol.Visible = false;
            BtnPrintSol.Visible = false;
            LblMensaje.Visible = false;
            if (TxtIdUnico.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el texto único";
                LblMensaje.Visible = true;
                TxtIdUnico.Focus();
                Label3.Visible = false;
                TxtAnis.Visible = false;
                BtnEnviaSol.Visible = false;
            }
            else if (Util.ExisteDato("Select * from tperiodo where idunico = '" + TxtIdUnico.Text + "'") == false)
            {
                LblMensaje.Text = "Este texto único no existe";
                LblMensaje.Visible = true;
                TxtIdUnico.Focus();
                TxtIdUnico.Text = "";
                Label3.Visible = false;
                TxtAnis.Visible = false;
                BtnEnviaSol.Visible = false;
            }
            else
            {
                string Corr = Util.ObtieneRegistro("Select Max(Corr) as Max from tperiodo where idunico = '" + TxtIdUnico.Text + "'", "Max").ToString();
                DateTime FecVen = Convert.ToDateTime(Util.ObtieneRegistro("Select FecVen from tperiodo where corr = " + Corr + " and idunico = '" + TxtIdUnico.Text + "'", "FecVen").ToString());
                if (Convert.ToDateTime(DateTime.Now) > Convert.ToDateTime(FecVen))
                {
                    string CodRegente = Util.ObtieneRegistro("Select CodRegente from tperiodo where idunico = '" + TxtIdUnico.Text + "'", "CodRegente").ToString();
                    if (Util.ObtieneRegistro("Select * from tregente where codregente = " + CodRegente + "", "CodEstatus").ToString() == "0")
                    {
                        LblMensaje.Text = "Su estatus como regente fue suspendido.";
                        LblMensaje.Visible = true;
                        Label3.Visible = false;
                        TxtAnis.Visible = false;
                        BtnEnviaSol.Visible = false;
                    }
                    else
                    {
                        Label3.Visible = true;
                        TxtAnis.Visible = true;
                        BtnEnviaSol.Visible = true;
                    }

                }
                else
                {
                    string CodRegente = Util.ObtieneRegistro("Select CodRegente from tperiodo where idunico = '" + TxtIdUnico.Text + "'", "CodRegente").ToString();
                    if (Util.ObtieneRegistro("Select * from tregente where codregente = " + CodRegente + "", "CodEstatus").ToString() == "0")
                    {
                        LblMensaje.Text = "Su estatus como regente fue suspendido.";
                        LblMensaje.Visible = true;
                    }
                    else
                    {
                        LblMensaje.Text = "Su estatus como regente aun se encuentra activo";
                        LblMensaje.Visible = true;
                        TxtIdUnico.Focus();
                    }
                    Label3.Visible = false;
                    TxtAnis.Visible = false;
                    BtnEnviaSol.Visible = false;
                }
            }
        }

    }
}