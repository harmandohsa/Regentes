using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using Regentes.ServiceReference1;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class ActRegente : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private IConsultaRegistro ConsultaRegistro = new ConsultaRegistroClient();


        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            CboDep.TextChanged += new EventHandler(CboDep_TextChanged);
            BtnSubeFoto.Click += new EventHandler(BtnSubeFoto_Click);
            GrdCurso.NeedDataSource += new GridNeedDataSourceEventHandler(GrdCurso_NeedDataSource);
            GrdExperiencia.NeedDataSource += new GridNeedDataSourceEventHandler(GrdExperiencia_NeedDataSource);
            GrdExperienciaFores.NeedDataSource += new GridNeedDataSourceEventHandler(GrdExperienciaFores_NeedDataSource);
            BtnSaveEstudi.Click += new EventHandler(BtnSaveEstudi_Click);
            BtnGrabaCurso.Click += new EventHandler(BtnGrabaCurso_Click);
            BtnAddExperiencia.Click += new EventHandler(BtnAddExperiencia_Click);
            BtnSaveTrabajoActual.Click += new EventHandler(BtnSaveTrabajoActual_Click);
            BtnAddExpReg.Click += new EventHandler(BtnAddExpReg_Click);
            BtnGrabarTodo.Click += new EventHandler(BtnGrabarTodo_Click);
            BtnImprimeCV.Click += new EventHandler(BtnImprimeCV_Click);
            if (!IsPostBack)
            {
                Util.LlenaCombo("Select * from tgenero", CboGenero, "CodGenero", "Genero");
                Util.LlenaCombo("Select * from tdepartamento", CboDep, "CodDep", "Departamento");
                //Util.LlenaCombo("Select * from tmunicipio where coddep = 1", CboMun, "CodMun", "Municipio");
                Util.LlenaCombo("Select * from tcategoria where codcategoria in (1,2)", CboCategoria, "CodCategoria", "Categoria");
                this.TxtFecNac.MinDate = Convert.ToDateTime("01/01/1930");
                this.TxtFecPresentacion.SelectedDate = new DateTime?(DateTime.Now);
                RadMenu menu = (RadMenu)Master.FindControl("Menu");
                menu.Visible = false;
                Label label = (Label)Master.FindControl("LblUsuario");
                label.Visible = false;
                ImageButton button = (ImageButton)Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
                TxtNoRegente.Text = Util.ObtieneRegistro("Select * from tperiodo where idunico = '" + Request.QueryString["idunico"] + "'", "CodRegente").ToString();
                CargaDatos(Util.ObtieneRegistro("Select * from tperiodo where idunico = '" + Request.QueryString["idunico"] + "'", "CodRegente").ToString());
                CargaImagen(TxtNoRegente.Text);
                CargaEstudios(TxtNoRegente.Text);
                CargaTrabajoActual(TxtNoRegente.Text);
            }
        }

        void BtnImprimeCV_Click(object sender, EventArgs e)
        {
            this.AbreVentana("RepCurriculu.aspx?CodRegente=" + this.TxtNoRegente.Text);
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        bool ValidaOpcion()
        {
            if (OptList.SelectedValue == "" && CboCategoria.SelectedValue == TxtCodCategoria.Text)
            {
                LblMensaje.Text = "Debe Seleccionar un tipo de Actualización";
                LblMensaje.Visible = true;
                return false;
            }
            else
                return true;
        }

        void BtnGrabarTodo_Click(object sender, EventArgs e)
        {
            if (ValidaOpcion() == true)
            {
                string TipoTramite = "";
                if (TxtNoRegistroECUT.Enabled == true)
                {
                    if (!(!(this.TxtNoRegistroECUT.Text != "") || this.ConsultaRegistro.ExisteREG(this.TxtNoRegistroECUT.Text)))
                    {
                        this.LblMensaje.Text = "Este n\x00famero de elaborador de cuidado de la tierra no existe en el INAB";
                        this.LblMensaje.Visible = true;
                        return;
                    }
                }
                Int32 TipoAct = 0;
                OcultaMensajes();
                if (CboCategoria.SelectedValue == TxtCodCategoria.Text)
                {

                    //StrSql = "Update tregente set correo = '" + TxtCorreo.Text + "', direccion = '" + TxtDireccion.Text + "', coddep = " + CboDep.SelectedValue + ", codmun = " + CboMun.SelectedValue + ", Telefono = '" + TxtTel.Text + "', fax = '" + TxtFax.Text + "', nocol = '" + TxtColegiado.Text + "', profesion = '" + TxtProfesion.Text + "', especializacion = '" + TxtEspecializacion.Text + "', experiencia = " + Util.IIf(this.OptioEspe.Items[0].Selected, 1, 0) + ", cual = '" + TxtCuales.Text + "' where Codregente = " + TxtNoRegente.Text + "";
                    TipoTramite = "2";
                    if (OptList.SelectedValue == "1")
                        TipoAct = 1;
                    else 
                        TipoAct = 2;
                    
                }
                else
                {
                    StrSql = "Update tregente set  CodCategoria = " + CboCategoria.SelectedValue + " where Codregente = " + TxtNoRegente.Text + "";
                    //StrSql = "Update tregente set correo = '" + TxtCorreo.Text + "', direccion = '" + TxtDireccion.Text + "', coddep = " + CboDep.SelectedValue + ", codmun = " + CboMun.SelectedValue + ", Telefono = '" + TxtTel.Text + "', fax = '" + TxtFax.Text + "', nocol = '" + TxtColegiado.Text + "', profesion = '" + TxtProfesion.Text + "', especializacion = '" + TxtEspecializacion.Text + "', experiencia = " + Util.IIf(this.OptioEspe.Items[0].Selected, 1, 0) + ", cual = '" + TxtCuales.Text + "', CodCategoria = " + CboCategoria.SelectedValue + " where Codregente = " + TxtNoRegente.Text + "";
                    TipoTramite = "3";
                    if (OptList.SelectedValue == "1")
                        TipoAct = 1;
                    else if (OptList.SelectedValue == "2")
                        TipoAct = 2;
                    else
                        TipoAct = 0;
                    Util.EjecutaIns(StrSql);
                }

                if (TxtYaGrabo.Text == "")
                {
                    decimal MaxNus = Util.MaxCorr("Select Max(nus) as MAXIMO from tsolicitud");

                    StrSql = "Insert into tsolicitud values (" + TxtNoRegente.Text + "," + MaxNus + ", " + TipoTramite + ", '" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "', 1,NULL," + TipoAct + ")";
                    Util.EjecutaIns(StrSql);
                    StrSql = "Insert into tactregente values ('" + TxtDireccion.Text + "', " + CboDep.SelectedValue + ", " + CboMun.SelectedValue + ", '" + TxtTel.Text + "','" + TxtFax.Text + "','" + TxtCorreo.Text + "','" + TxtProfesion.Text + "','" + TxtColegiado.Text + "','" + TxtEspecializacion.Text + "'," + Util.IIf(this.OptioEspe.Items[0].Selected, 1, 0) + ",'" + TxtCuales.Text + "'," + TxtNoRegente.Text + "," + MaxNus + ")";
                    Util.EjecutaIns(StrSql);
                    TxtYaGrabo.Text = "1";
                    TxtNus.Text = MaxNus.ToString();
                    string Region = Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "Region").ToString();
                    string CodRegion = Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "CodRegion").ToString();
                    Region = Region + "(" + Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "Dep").ToString() + ")";
                    EnvioCorreo(TxtCorreo.Text, TxtNombres.Text + " " + TxtApellidos.Text, Region, TipoTramite,TipoAct.ToString());
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
                            EnvioCorreoSecretaria(reader["mail"].ToString(), reader["nombres"] + " " + reader["apellidos"], Convert.ToInt32(Cuantas), TipoTramite);
                        }
                        cn.Close();
                    }
                    LblGrabaTodo.Text = "Datos Enviados con Exito. Se ha enviado información importante a su correo electrónico para que pueda continuar con el trámite respectivo. Mientras no cierre esta ventana podra hacer cambios a la información enviada, al cerrar la ventana ya no podrá modificar la información.";
                }

                BtnImprimeCV.Visible = true;
                this.AbreVentana("RepSolicitud.aspx?CodRegente=" + this.TxtNoRegente.Text + "&nus=" + TxtNus.Text + "&tramite=" + TipoTramite + "");
                BtnGrabarTodo.Text = "Modificar/Imprimir Solicitud de Actualización";
            }
        }

        private void EnvioCorreoSecretaria(string Mail, string Nombre, int Cuantas, string Tramite)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de inscripción de Regente";
            Cuantas = Cuantas - 1;
            if (Tramite == "2")
                Tramite = "Actualización de Datos";
            else if (Tramite == "3")
                Tramite = "Actualización de Datos y Categoría";
            if (Cuantas <= 0)
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de " + Tramite + ".</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de " + Tramite + ". Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
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

        private void EnvioCorreo(string Mail, string Nombre, string Region, string TipoTramite, string TipoAct)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de Actualización de Regente";
            if (TipoTramite == "2")
            {
                if (TipoAct == "1")
                {
                    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para actualización de datos como regente forestal ha sido enviada exitosamente a CONAP, puede presentarse a la oficina regional: " + Region + ". Para poder dar continuidad al trámite de su solicitud es necesario presentar la siguiente documentación: </td></tr><tr><td>-  Solicitud escrita en Formulario del Consejo Nacional de Áreas Protegidas</td></tr></table>") +
                            "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
                }
                else
                {
                    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para actualización de datos como regente forestal ha sido enviada exitosamente a CONAP, puede presentarse a la oficina regional: " + Region + ". Para poder dar continuidad al trámite de su solicitud es necesario presentar la siguiente documentación: </td></tr><tr><td>-  Solicitud escrita en Formulario del Consejo Nacional de Áreas Protegidas</td></tr><tr><td>-  Copia legalizada del Documento Personal de Identificación</td></tr><tr><td>-  Constancias (diplomas) que respalden especialización</td></tr><tr><td>-  Constancia de Cuota de Pago por actualización en Cuenta de Fondos Privativos: 3-099-03774-6 de BANRURAL, y Recibo 63 A2 extendido por CONAP.</td></tr></table>") +
                            "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
                }
                
            }
            else if (TipoTramite == "3")
            {
                if (TipoAct == "2")
                {
                    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para actualización de datos como regente forestal ha sido enviada exitosamente a CONAP, puede presentarse a la oficina regional: " + Region + ". Para poder dar continuidad al trámite de su solicitud es necesario presentar la siguiente documentación: </td></tr><tr><td>-  Solicitud escrita en Formulario del Consejo Nacional de Áreas Protegidas</td></tr><tr><td>Copia legalizada del Documento Personal de Identificación</td></tr><tr><td>-  Copia legalizada del Título que acredite el grado académico y especializaciones</td></tr><tr><td>-  Constancia de Colegiado Activo original</td></tr><tr><td>-  Copia legalizada del Título que acredite el grado  académico</td></tr><tr><td><tr><td>-  Currículum Vitae actualizado según formato del Consejo Nacional de Áreas Protegidas.</td></tr></td></tr><tr><td>-  Constancias (diplomas) que respalden especializacion**</td></tr></td></tr><tr><td>-  Constancia de Cuota de Pago por Registro de actualización en BANRURAL, y Recibo 63 A2 extendido por CONAP.</td></tr></table>") +
                            "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
                }
                else
                {
                    content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para actualización de datos como regente forestal ha sido enviada exitosamente a CONAP, puede presentarse a la oficina regional: " + Region + ". Para poder dar continuidad al trámite de su solicitud es necesario presentar la siguiente documentación: </td></tr><tr><td>-  Solicitud escrita en Formulario del Consejo Nacional de Áreas Protegidas</td></tr><tr><td>Copia legalizada del Documento Personal de Identificación</td></tr><tr><td>-  Copia legalizada del Título que acredite el grado académico y especializaciones</td></tr><tr><td>-  Constancia de Colegiado Activo original</td></tr><tr><td>-  Copia legalizada del Título que acredite el grado  académico</td></tr><tr><td><tr><td>-  Currículum Vitae actualizado según formato del Consejo Nacional de Áreas Protegidas.</td></tr></td></tr><tr><td>-  Constancia de Cuota de Pago por Registro de actualización en BANRURAL, y Recibo 63 A2 extendido por CONAP.</td></tr></table>") +
                            "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
                }
                
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

        void BtnAddExpReg_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.TxtNomFinca.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe Ingresar el nombre de la finca";
            }
            else if (this.TxtUbicacionExpReg.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe Ingresar la ubicaci\x00f3n de la finca";
            }
            else if (this.TxtTipoActividad.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe Ingresar el tipo de actividad";
            }
            else if (this.TxtArea.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe Ingresar el \x00e1rea";
            }
            else if (this.TxtPeriodo.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe Ingresar el per\x00edodo";
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from texperienciareg where codregente = " + this.TxtNoRegente.Text);
                this.StrSql = string.Concat(new object[] { "Insert into texperienciareg values (", this.TxtNoRegente.Text, ", ", num, ", '", this.TxtNomFinca.Text.Trim(), "', '", this.TxtUbicacionExpReg.Text.Trim(), "', '", this.TxtTipoActividad.Text.Trim(), "', ", this.TxtArea.Text.Trim(), ",'", this.TxtPeriodo.Text.Trim(), "')" });
                this.Util.EjecutaIns(this.StrSql);
                this.LblExperienciaReg.Text = "Experiencia Forestal Agregada Exitosamente";
                this.LblExperienciaReg.Visible = true;
                this.GrdExperienciaFores.Rebind();
                LimpiaExperienciaRegente();
            }
        }

        void BtnSaveTrabajoActual_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.Util.ExisteDato("Select * from ttrabajo where codregente = " + this.TxtNoRegente.Text))
            {
                this.StrSql = "Update ttrabajo set institucion = '" + this.TxtInstitucionActual.Text.Trim() + "', puesto =  '" + this.TxtPuesto.Text.Trim() + "' where codregente = " + this.TxtNoRegente.Text;
            }
            else
            {
                this.StrSql = "Insert into ttrabajo values (" + this.TxtNoRegente.Text + ", '" + this.TxtInstitucionActual.Text.Trim() + "', '" + this.TxtPuesto.Text.Trim() + "')";
            }
            this.Util.EjecutaIns(this.StrSql);
            this.LblTrabajo.Text = "Datos de Trabajo Actual Grabados";
            this.LblTrabajo.Visible = true;
        }

        void BtnAddExperiencia_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.TxtActividad.Text == "")
            {
                this.LblExperiencia.Text = "Debe Ingresar la actividad";
                this.LblExperiencia.Visible = true;
            }
            else if (this.TxtEntidad.Text == "")
            {
                this.LblExperiencia.Text = "Debe Ingresar la entidad";
                this.LblExperiencia.Visible = true;
            }
            else if (this.TxtUbicacion.Text == "")
            {
                this.LblExperiencia.Text = "Debe Ingresar la ubicaci\x00f3n";
                this.LblExperiencia.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from texperiencia where codregente = " + this.TxtNoRegente.Text);
                this.StrSql = string.Concat(new object[] { "Insert into texperiencia values (", this.TxtNoRegente.Text, ", ", num, ", '", this.TxtActividad.Text.Trim(), "', '", this.TxtEntidad.Text.Trim(), "', '", this.TxtUbicacion.Text.Trim(), "', '", this.TxtObservaciones.Text.Trim(), "')" });
                this.Util.EjecutaIns(this.StrSql);
                this.LblExperiencia.Text = "Experiencia Agregada Exitosamente";
                this.LblExperiencia.Visible = true;
                this.GrdExperiencia.Rebind();
                LimpiaExperiencia();
            }
        }

        void BtnGrabaCurso_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.TxtEnfoqueCurso.Text == "")
            {
                this.LblMensajeCursos.Text = "Debe ingresar el enfoque";
                this.LblMensajeCursos.Visible = true;
            }
            else if (this.TxtInstitucion.Text == "")
            {
                this.LblMensajeCursos.Text = "Debe ingresar la instituci\x00f3n del curso";
                this.LblMensajeCursos.Visible = true;
            }
            else if (this.TxtDuracion.Text == "")
            {
                this.LblMensajeCursos.Text = "Debe ingresar la duraci\x00f3n";
                this.LblMensajeCursos.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from tcurso where codregente = " + this.TxtNoRegente.Text);
                this.StrSql = string.Concat(new object[] { "Insert into tcurso values (", this.TxtNoRegente.Text, ", ", num, ", '", this.TxtEnfoqueCurso.Text.Trim(), "', '", this.TxtInstitucion.Text.Trim(), "', '", this.TxtDuracion.Text.Trim(), "')" });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensajeCursos.Text = "Curso Agregado Exitosamente";
                this.LblMensajeCursos.Visible = true;
                this.GrdCurso.Rebind();
                LimpiaCurso();

            }
        }

        void BtnSaveEstudi_Click(object sender, EventArgs e)
        {
            OcultaMensajes();
            if (Util.ExisteDato("Select * from testudio where codregente = " + TxtNoRegente.Text))
            {
                this.StrSql = "Update testudio set titmedio = '" + this.TxtTitMedio.Text.Trim() + "', Anismedio =  '" + this.TxtAnisMedio.Text.Trim() + "',insmedio = '" + this.TxtInsMedio.Text.Trim() + "', tittecnico =  '" + this.TxtTitTu.Text.Trim() + "', anistecnico = '" + this.TxtAnisTu.Text + "', instecnico = '" + this.TxtInsTu.Text + "', titpro = '" + TxtTituProfesional.Text.Trim() + "', anispro = '" + TxtAnisProfesiona.Text.Trim() + "', inspro = '" + TxtInsPro.Text.Trim() + "', titpos = '" + TxtTitPosGrado.Text.Trim() + "', anispos = '" + TxtAnisPosGrado.Text.Trim() + "',  inspos = '" + TxtinsPos.Text + "', titotro =  '" + TxtTitOtro.Text.Trim() + "', anisotro = '" + TxtAnisOtro.Text + "', insotro = '" + TxtInsOtro.Text + "' where codregente = " + TxtNoRegente.Text;
            }
            else
            {
                this.StrSql = "Insert into testudio values (" + this.TxtNoRegente.Text + ", '" + this.TxtTitMedio.Text.Trim() + "', '" + this.TxtAnisMedio.Text.Trim() + "', '" + this.TxtInsMedio.Text.Trim() + "', '" + this.TxtTitTu.Text.Trim() + "', '" + this.TxtAnisTu.Text + "', '" + this.TxtInsTu.Text + "', '" + this.TxtTituProfesional.Text.Trim() + "', '" + this.TxtAnisProfesiona.Text.Trim() + "', '" + this.TxtInsPro.Text.Trim() + "', '" + this.TxtTitPosGrado.Text.Trim() + "', '" + this.TxtAnisPosGrado.Text.Trim() + "', '" + this.TxtinsPos.Text + "', '" + this.TxtTitOtro.Text.Trim() + "', '" + this.TxtAnisOtro.Text + "',  '" + this.TxtInsOtro.Text + "')";
            }
            this.Util.EjecutaIns(this.StrSql);
            this.LblMensajeCv.Text = "Datos de Estudio Grabados";
            this.LblMensajeCv.Visible = true;
        }

        private void OcultaMensajes()
        {
            LblMensaje.Visible = false;
            LblMensajeCv.Visible = false;
            LblMensajeCursos.Visible = false;
            LblExperiencia.Visible = false;
            LblTrabajo.Visible = false;
            LblExperienciaReg.Visible = false;
            LblFoto.Visible = false;
            LblGrabaTodo.Visible = false;
        }

        void GrdExperienciaFores_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CodRegente,Corr,Finca,ubicacion,tipoactividad,area,periodo from texperienciareg where codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = '" + Request.QueryString["idunico"] + "'", "CodRegente").ToString();
            Util.LlenaGrid(StrSql, GrdExperienciaFores);
        }

        void GrdExperiencia_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CodRegente,Corr,Actividad,Entidad,Ubicacion,Observaciones from texperiencia where codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = '" + Request.QueryString["idunico"] + "'", "CodRegente").ToString();
            Util.LlenaGrid(StrSql, GrdExperiencia);
        }

        void GrdCurso_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            StrSql = "Select CodRegente,Corr,Enfoque,Institucion,Duracion from tcurso where codregente = " + Util.ObtieneRegistro("Select * from tperiodo where idunico = '" + Request.QueryString["idunico"] + "'", "CodRegente").ToString();
            Util.LlenaGrid(StrSql, GrdCurso);
        }

        void BtnSubeFoto_Click(object sender, EventArgs e)
        {
            if (UploadFoto.FileName == "")
            {
                LblFoto.Text = "Debe Seleccionar un archivo";
                LblFoto.Visible = true;
            }
            else
            {
                string str = "";
                str = Path.GetExtension(UploadFoto.FileName).Substring(1);
                if ((str == "JPG") || (str == "jpg"))
                {
                    string path = Server.MapPath(".") + @"\FotosRegentes\" + TxtNoRegente.Text;
                    Util.BorrarArchivo(path);
                    Directory.CreateDirectory(path);
                    UploadFoto.SaveAs(path + @"\" + UploadFoto.FileName);
                    string str3 = @"~\FotosRegentes\" + TxtNoRegente.Text + @"\" + UploadFoto.FileName;
                    ImgFoto.ImageUrl = str3;
                    LblFoto.Text = "Foto Subida Exitosamente";
                    LblFoto.Visible = true;
                }
                else
                {
                    LblFoto.Text = "Solo puede subir archivos jpg";
                    LblFoto.Visible = true;
                }
            }
        }

        void CboDep_TextChanged(object sender, EventArgs e)
        {
            if (this.CboDep.Text != "")
            {
                this.CboMun.Items.Clear();
                this.Util.LlenaCombo("Select * from tmunicipio where coddep = " + this.CboDep.SelectedValue, this.CboMun, "CodMun", "Municipio");
            }
        }

        void CargaImagen(string CodRegente)
        {
            string PathLogo = Server.MapPath(".") + @"\FotosRegentes\" + CodRegente;
            if (Directory.Exists(PathLogo))
            {
                DirectoryInfo directory = new DirectoryInfo(PathLogo);
                FileInfo[] files = directory.GetFiles("*.*");
                DirectoryInfo[] directories = directory.GetDirectories();
                for (int i = 0; i < files.Length; i++)
                {

                    string LugFoto = @"~\FotosRegentes\" + CodRegente + @"\" + ((FileInfo)files[i]).Name;
                    ImgFoto.ImageUrl = LugFoto;
                }
            }
        }

        void CargaTrabajoActual(string CodRegente)
        {
            StrSql = "Select * from TTRABAJO where Codregente = " + CodRegente + "";
            cn.Open();
            CmTransaccion.CommandText = StrSql;
            CmTransaccion.Connection = cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                TxtInstitucionActual.Text = reader["institucion"].ToString();
                TxtPuesto.Text = reader["puesto"].ToString();
            }
            cn.Close();
        }

        void CargaEstudios(string CodRegente)
        {
            StrSql = "Select * from testudio where Codregente = " + CodRegente + "";
            cn.Open();
            CmTransaccion.CommandText = StrSql;
            CmTransaccion.Connection = cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                TxtTitMedio.Text = reader["titmedio"].ToString();
                TxtAnisMedio.Text = reader["Anismedio"].ToString();
                TxtInsMedio.Text = reader["insmedio"].ToString();
                TxtTitTu.Text = reader["tittecnico"].ToString();
                TxtAnisTu.Text = reader["anistecnico"].ToString();
                TxtInsTu.Text = reader["instecnico"].ToString();
                TxtTituProfesional.Text = reader["titpro"].ToString();
                TxtAnisProfesiona.Text = reader["Anispro"].ToString();
                TxtInsPro.Text = reader["inspro"].ToString();
                TxtTitPosGrado.Text = reader["titpos"].ToString();
                TxtAnisPosGrado.Text = reader["anispos"].ToString();
                TxtinsPos.Text = reader["inspos"].ToString();
                TxtTitOtro.Text = reader["TitOtro"].ToString();
                TxtAnisOtro.Text = reader["AnisOtro"].ToString();
                TxtInsOtro.Text = reader["InsOtro"].ToString();
            }
            cn.Close();
        }

        void CargaDatos(string CodRegente)
        {
            StrSql = "Select *,genero " +
                     "from tregente a, tgenero b " + 
                     "where a.codgenero = b.codgenero " +
                     "and CodRegente = " + CodRegente + "";
            cn.Open();
            CmTransaccion.CommandText = StrSql;
            CmTransaccion.Connection = cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                TxtNoRegistroRG.Text = reader["CodReg"].ToString();
                TxtNoRegistroEMPF.Text = reader["CodRegempf"].ToString();
                TxtNombres.Text = reader["Nombres"].ToString();
                TxtNoRegistroECUT.Text = reader["CodRegEcut"].ToString();
                if (TxtNoRegistroECUT.Text == "")
                    TxtNoRegistroECUT.Enabled = true;
                TxtApellidos.Text = reader["Apellidos"].ToString();
                TxtDocId.Text = reader["codid"].ToString();
                TxtNit.Text = reader["Nit"].ToString();
                TxtNacionalidad.Text = reader["nacionalidad"].ToString();
                CboGenero.SelectedValue =  reader["CodGenero"].ToString();
                TxtFecNac.SelectedDate = Convert.ToDateTime(reader["Fecnac"]);
                TxtCorreo.Text = reader["Correo"].ToString();
                TxtDireccion.Text = reader["Direccion"].ToString();
                CboDep.SelectedValue = reader["CodDep"].ToString();
                Util.LlenaCombo("Select * from tmunicipio where coddep = " + CboDep.SelectedValue + "", CboMun, "CodMun", "Municipio");
                CboMun.SelectedValue = reader["CodMun"].ToString();
                TxtTel.Text = reader["Telefono"].ToString();
                TxtFax.Text = reader["Fax"].ToString();
                CboCategoria.SelectedValue = reader["CodCategoria"].ToString();
                TxtCodCategoria.Text = reader["CodCategoria"].ToString();
                if (CboCategoria.SelectedValue == "2")
                {
                    CboCategoria.Enabled = false;
                    
                }
                TxtColegiado.Text = reader["nocol"].ToString();
                TxtProfesion.Text = reader["profesion"].ToString();
                TxtEspecializacion.Text = reader["especializacion"].ToString();
                if (reader["experiencia"].ToString() == "1")
                {
                    OptioEspe.Items[0].Selected = true;
                    OptioEspe.Items[1].Selected = false;
                }
                else
                {
                    OptioEspe.Items[0].Selected = false;
                    OptioEspe.Items[1].Selected = true;
                }
                TxtCuales.Text = reader["Cual"].ToString();

            }
            cn.Close();
        }

        void LimpiaCurso()
        {
            TxtEnfoqueCurso.Text = "";
            TxtInstitucionActual.Text = "";
            TxtDuracion.Text = "";
        }

        void LimpiaExperiencia()
        {
            TxtActividad.Text = "";
            TxtEntidad.Text = "";
            TxtUbicacion.Text = "";
            TxtObservaciones.Text = "";
        }

        void LimpiaExperienciaRegente()
        {
            TxtNomFinca.Text = "";
            TxtUbicacionExpReg.Text = "";
            TxtTipoActividad.Text = "";
            TxtArea.Text = "";
            TxtPeriodo.Text = "";
        }
    }
}