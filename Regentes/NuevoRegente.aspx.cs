using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Regentes.ServiceReference1;
using Telerik.Web.UI;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace Regentes
{
    public partial class NuevoRegente : System.Web.UI.Page
    {
        private IConsultaRegistro ConsultaRegistro = new ConsultaRegistroClient();
        Registro.WebConap ConsultaRegistroINAB = new Registro.WebConap();
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.Util = new CUtilitarios();
            this.BtnGrabar.Click += new EventHandler(this.BtnGrabar_Click);
            this.CboDep.TextChanged += new EventHandler(this.CboDep_TextChanged);
            this.BtnSaveEstudi.Click += new EventHandler(this.BtnSaveEstudi_Click);
            this.GrdCurso.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdCurso_NeedDataSource);
            this.BtnGrabaCurso.Click += new EventHandler(this.BtnGrabaCurso_Click);
            this.GrdCurso.ItemCommand += new GridCommandEventHandler(this.GrdCurso_ItemCommand);
            this.GrdExperiencia.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdExperiencia_NeedDataSource);
            this.BtnAddExperiencia.Click += new EventHandler(this.BtnAddExperiencia_Click);
            this.GrdExperiencia.ItemCommand += new GridCommandEventHandler(this.GrdExperiencia_ItemCommand);
            this.BtnSaveTrabajoActual.Click += new EventHandler(this.BtnSaveTrabajoActual_Click);
            this.BtnAddExpReg.Click += new EventHandler(this.BtnAddExpReg_Click);
            this.GrdExperienciaFores.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdExperienciaFores_NeedDataSource);
            this.GrdExperienciaFores.ItemCommand += new GridCommandEventHandler(this.GrdExperienciaFores_ItemCommand);
            this.BtnGrabarTodo.Click += new EventHandler(this.BtnGrabarTodo_Click);
            this.BtnSubeFoto.Click += new EventHandler(this.BtnSubeFoto_Click);
            this.BtnImprimeCV.Click += new EventHandler(this.BtnImprimeCV_Click);
            ChkAutoriza.CheckedChanged += new EventHandler(ChkAutoriza_CheckedChanged);
            if (!base.IsPostBack)
            {
                this.Util.LlenaCombo("Select * from tgenero", this.CboGenero, "CodGenero", "Genero");
                this.Util.LlenaCombo("Select * from tdepartamento order by departamento", this.CboDep, "CodDep", "Departamento");
                this.Util.LlenaCombo("Select * from tmunicipio where coddep = 1", this.CboMun, "CodMun", "Municipio");
                Util.LlenaCombo("Select * from tcategoria where codcategoria in (1,2)", CboCategoria, "CodCategoria", "Categoria");
                this.TxtFecNac.MinDate = Convert.ToDateTime("01/01/1930");
                this.TxtFecNac.SelectedDate = new DateTime?(DateTime.Now);
                this.TxtFecPresentacion.SelectedDate = new DateTime?(DateTime.Now);
                RadMenu menu = (RadMenu)base.Master.FindControl("Menu");
                menu.Visible = false;
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Visible = false;
                ImageButton button = (ImageButton)base.Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
            }
        }

        void ChkAutoriza_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAutoriza.Checked == true)
            {
                BtnGrabarTodo.Visible = true;
                ChkAutoriza.Enabled = false;
            }
            
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        private void BtnAddExperiencia_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.TxtActividad.Text == "")
            {
                this.LblExperiencia.Text = "Debe ingresar la actividad";
                this.LblExperiencia.Visible = true;
            }
            else if (this.TxtEntidad.Text == "")
            {
                this.LblExperiencia.Text = "Debe ingresar la entidad";
                this.LblExperiencia.Visible = true;
            }
            else if (this.TxtUbicacion.Text == "")
            {
                this.LblExperiencia.Text = "Debe ingresar la ubicaci\x00f3n";
                this.LblExperiencia.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from texperiencia where codregente = " + this.TxtNoRegente.Text);
                this.StrSql = string.Concat(new object[] { "Insert into texperiencia values (", this.TxtNoRegente.Text, ", ", num, ", '", this.TxtActividad.Text.Trim(), "', '", this.TxtEntidad.Text.Trim(), "', '", this.TxtUbicacion.Text.Trim(), "', '", this.TxtObservaciones.Text.Trim(), "')" });
                this.Util.EjecutaIns(this.StrSql);
                this.LblExperiencia.Text = "Experiencia agregada exitosamente";
                this.LblExperiencia.Visible = true;
                this.GrdExperiencia.Rebind();
                LimpiaExperiencia();
            }
        }

        private void BtnAddExpReg_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.TxtNomFinca.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe ingresar el nombre de la finca";
            }
            else if (this.TxtUbicacionExpReg.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe ingresar la ubicaci\x00f3n de la finca";
            }
            else if (this.TxtTipoActividad.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe ingresar el tipo de actividad";
            }
            else if (this.TxtArea.Text == "")
            {
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Debe ingresar el \x00e1rea";
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
                this.LblExperienciaReg.Text = "Experiencia forestal agregada exitosamente";
                this.LblExperienciaReg.Visible = true;
                this.GrdExperienciaFores.Rebind();
                LimpiaExperienciaRegente();
            }
        }

        private void BtnGrabaCurso_Click(object sender, EventArgs e)
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
                this.LblMensajeCursos.Text = "Curso agregado exitosamente";
                this.LblMensajeCursos.Visible = true;
                this.GrdCurso.Rebind();
                LimpiaCurso();

            }
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

        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            this.LblGrabo.Visible = false;
            this.LblMensaje.Visible = false;
            if (this.TxtNoRegente.Text == "")
            {
                if (this.Valida())
                {
                    decimal num = this.Util.MaxCorr("Select Max(CodRegente) as Maximo from tregente");
                    StrSql = string.Concat(new object[] { 
                    "Insert into tregente values (", num, ", '", this.TxtNoRegistroRG.Text.Trim(), "', '", this.TxtNoRegistroEMPF.Text.Trim(), "', '", this.TxtApellidos.Text.Trim(), "', '", this.TxtNombres.Text.Trim(), "',  '", this.TxtDocId.Text.Trim(), "', '", this.TxtNit.Text.Trim(), "', '", this.TxtNacionalidad.Text.Trim(), 
                    "', ", this.CboGenero.SelectedValue, ", '", string.Format("{0:MM/dd/yyyy}", this.TxtFecNac.DateInput.SelectedDate), "', '", this.TxtCorreo.Text.Trim(), "', '", this.TxtDireccion.Text.Trim(), "', ", this.CboDep.SelectedValue, ", ", this.CboMun.SelectedValue, ", '", this.TxtTel.Text.Trim(), "', '", this.TxtFax.Text, 
                    "', '", this.TxtProfesion.Text.Trim(), "', '", this.TxtColegiado.Text, "', '", this.TxtEspecializacion.Text.Trim(), "', ", this.Util.IIf(this.OptioEspe.Items[0].Selected, 1, 0), ", '", this.TxtCuales.Text.Trim(), "', null, '", string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()), "', '", this.TxtNoRegistroECUT.Text.Trim(), "', NULL," + TxtAnis.Text + ", " + CboCategoria.SelectedValue + ", NULL,1, " + Util.IIf(ChkPublico.Checked,1,0) + ")"
                    });
                    this.Util.EjecutaIns(this.StrSql);
                    //decimal nus = this.Util.MaxCorr("Select Max(Nus) as Maximo from tsolicitud");
                    //TxtNus.Text = nus.ToString();
                    //StrSql = "Insert into tsolicitud values (" + num + ", " + nus + ", 1,'" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "',0,NULL)";
                    //Util.EjecutaIns(StrSql);
                    this.TxtNoRegente.Text = num.ToString();
                    this.Div1.Visible = true;
                    //this.TxtNoRegistroECUT.Enabled = false;
                    //this.TxtNoRegistroRG.Enabled = false;
                    //this.TxtNoRegistroEMPF.Enabled = false;
                    this.LblGrabo.Text = "Solicitud grabada con éxito, por favor continue llenando su curriculum vitae";
                    this.LblGrabo.Visible = true;
                    this.GrdCurso.Rebind();
                    this.GrdExperiencia.Rebind();
                    this.GrdExperienciaFores.Rebind();
                    LblAutorizacion.Text = "Autorizo por este medio que se me notifique a la dirección de correo electrónico " + TxtCorreo.Text + ", teniendo como bien hechas las que se realicen en la misma.";
                    ChkAutoriza.Text = "Autorizo por este medio que se me notifique a la dirección de correo electrónico " + TxtCorreo.Text + ", teniendo como bien hechas las que se realicen en la misma.";
                }
            }
            else if (this.ValidaV2())
            {
                this.StrSql = string.Concat(new object[] { 
                    "Update tregente set Apellidos = '", this.TxtApellidos.Text.Trim(), "', Nombres = '", this.TxtNombres.Text.Trim(), "', CodId =  '", this.TxtDocId.Text.Trim(), "', Nit = '", this.TxtNit.Text.Trim(), "', Nacionalidad = '", this.TxtNacionalidad.Text.Trim(), "', CodGenero = ", this.CboGenero.SelectedValue, ", fecnac =  '", string.Format("{0:MM/dd/yyyy}", this.TxtFecNac.DateInput.SelectedDate), "', Correo = '", this.TxtCorreo.Text.Trim(), 
                    "', Direccion = '", this.TxtDireccion.Text.Trim(), "', Coddep = ", this.CboDep.SelectedValue, ", CodMun = ", this.CboMun.SelectedValue, ", telefono = '", this.TxtTel.Text.Trim(), "', fax = '", this.TxtFax.Text.Trim(), "', profesion = '", this.TxtProfesion.Text.Trim(), "', nocol = '", this.TxtColegiado.Text.Trim(), "', Especializacion = '", this.TxtEspecializacion.Text.Trim(), 
                    "', Experiencia = ", this.Util.IIf(this.OptioEspe.Items[0].Selected, 1, 0), ", Cual = '", this.TxtCuales.Text.Trim(), "', CodRegECUT = '", this.TxtNoRegistroECUT.Text.Trim(), "', VERCV = ", Util.IIf(ChkPublico.Checked, 1, 0)," where CodRegente = ", this.TxtNoRegente.Text
                 });
                this.Util.EjecutaIns(this.StrSql);
                this.Div1.Visible = true;
                ////this.TxtNoRegistroECUT.Enabled = false;
                ////this.TxtNoRegistroRG.Enabled = false;
                ////this.TxtNoRegistroEMPF.Enabled = false;
                this.LblGrabo.Text = "Solicitud modificada con éxito, por favor continue llenando su curriculum vitae";
                this.LblGrabo.Visible = true;
            }
        }

        private bool EsMayor(DateTime FecNac)
        {
            DateTime time = new DateTime(1, 1, 1);
            DateTime time2 = new DateTime(Convert.ToInt32(FecNac.Year), Convert.ToInt32(FecNac.Month), Convert.ToInt32(FecNac.Day));
            DateTime time3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan span = (TimeSpan)(time3 - time2);
            DateTime time4 = time + span;
            int num = time4.Year - 1;
            return (num >= 0x12);
        }

        private void BtnGrabarTodo_Click(object sender, EventArgs e)
        {

            string path = base.Server.MapPath(".") + @"\FotosRegentes\" + this.TxtNoRegente.Text;
            if (Util.ExisteArchivo(path) == true)
            {
                this.OcultaMensajes();
                bool flag = true;
                if (this.Util.ExisteDato("Select * from tenviosol where codregente = " + this.TxtNoRegente.Text))
                {
                    this.StrSql = "Update tenviosol set observacion = '" + this.TxtObservacionGen.Text.Trim() + "', fecpresentacion =  '" + string.Format("{0:MM/dd/yyyy}", this.TxtFecPresentacion.DateInput.SelectedDate) + "' where codregente = " + this.TxtNoRegente.Text;
                }
                else
                {
                    this.StrSql = "Insert into tenviosol values (" + this.TxtNoRegente.Text + ", '" + this.TxtObservacionGen.Text.Trim() + "',  '" + string.Format("{0:MM/dd/yyyy}", this.TxtFecPresentacion.DateInput.SelectedDate) + "')";
                }
                this.Util.EjecutaIns(this.StrSql);

                if (!this.Util.ExisteDato("Select * from testudio where codregente = " + this.TxtNoRegente.Text))
                {
                    this.LblGrabaTodo.Text = "No ha grabado la informaci\x00f3n de estudios";
                    this.LblGrabaTodo.Visible = true;
                    flag = false;
                }
                if (!this.Util.ExisteDato("Select * from ttrabajo where codregente = " + this.TxtNoRegente.Text))
                {
                    this.LblGrabaTodo.Text = "No ha grabado la informaci\x00f3n del trabajo actual";
                    this.LblGrabaTodo.Visible = true;
                    flag = false;
                }
                if (flag)
                {
                    TxtNus.Text = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + TxtNoRegente.Text + "", "nus").ToString();
                    if (Util.ExisteDato("Select * from TSOLICITUD where CodRegente = " + TxtNoRegente.Text + " and codtramite = 1") == false)
                    {
                        decimal nus = this.Util.MaxCorr("Select Max(Nus) as Maximo from tsolicitud");
                        TxtNus.Text = nus.ToString();
                        StrSql = "Insert into tsolicitud values (" + TxtNoRegente.Text + ", " + nus + ", 1,'" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "',1,NULL,0)";
                        Util.EjecutaIns(StrSql);
                        //StrSql = "Update tsolicitud set codestatus = 1 where CodRegente = " + TxtNoRegente.Text + " and codtramite = 1";
                        //Util.EjecutaIns(StrSql);

                    }
                    this.BtnImprimeCV.Visible = true;
                    string Region = Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "Region").ToString();
                    string CodRegion = Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "CodRegion").ToString();
                    Region = Region + " (" + Util.ObtieneRegistro("select * from trelregiondep a, tregion b where a.codregion = b.codregion and coddep = " + CboDep.SelectedValue + "", "Direccion").ToString() + ")";

                    EnvioCorreo(TxtCorreo.Text, TxtNombres.Text + " " + TxtApellidos.Text, Region);
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
                    this.AbreVentana("RepSolicitud.aspx?CodRegente=" + this.TxtNoRegente.Text + "&nus=" + TxtNus.Text + "&tramite=1");
                    this.LblGrabaTodo.Text = "Datos enviados con éxito. Se ha enviado información importante a su correo electrónico para que pueda continuar con el trámite respectivo. Mientras no cierre esta ventana podra hacer cambios a la información enviada, al cerrar la ventana ya no podrá modificar la información.";
                    this.LblGrabaTodo.Visible = true;
                    BtnGrabarTodo.Text = "Modificar/Imprimir solicitud";
                }
            }
            else 
            {
                LblGrabaTodo.Text = "Debe Subir su fotografía";
                LblGrabaTodo.Visible = true;
            }


            
        }

        private void EnvioCorreo(string Mail, string Nombre, string Region)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de solicitud de inscripción de Regente";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado (a): " + Nombre + "</td></tr><tr><td>Reciba un cordial saludo del Departamento de Manejo Forestal de la Secretaría Ejecutiva del Consejo Nacional de Áreas Protegidas.</td></tr><tr><td>Le informamos que su gestión para ser inscrito como regente forestal ha sido enviada exitosamente al CONAP. Para poder dar continuidad al trámite de su solicitud es necesario que presente en un tiempo no mayor a 15 días hábiles la documentación que se describe a continuación, en la oficina regional " + Region + ".</td></tr><tr><td>  a)  Solicitud de inscripción en formulario generado por el SEAF-CONAP</td></tr><tr><td>  b)  Copia legalizada del Documento Personal de Identificación</td></tr><tr><td>  c)  Copia legalizada del Título que acredite el grado académico y especializaciones</td></tr><tr><td>  d)  Constancia original de colegiado activo, mínimo con un mes de vigencia (para profesionales)</td></tr><tr><td>  e)  Curriculum vitae según formato generado por el SEAF-CONAP.</td></tr><tr><td>  f)  Constancia de Cuota de Pago por Registro de Inscripción en BANRURAL y Recibo 63 A2 extendido por CONAP. Cuenta Fondos Privativos CONAP, Banrural No. 3-099-03774-6. Deberán cancelar un monto de Q 250.00 que por concepto de inscripción y un año de vigencia, además podrá cancelar Q 50.00 por anualidad, podrá estar vigente hasta un máximo de cuatro años.</td></tr><tr><td>  g)  Copia de Carné de Identificación Tributaria, extendido por la SAT.</td></tr><tr><td>  h)  Copia legalizada del certificado de inscripción vigente ante el Registro Nacional Forestal del INAB como Regente Forestal</td></tr><tr><td>  i)  Copia legalizada del certificado de inscripción vigente ante el Registro Nacional Forestal del INAB como Elaborador de Planes de Manejo Forestal.</td></tr></table><div><hr /></div><table><tr><td><b>Si es regente ya inscrito en el Consejo Nacional de Áreas protegidas deberá atender los siguientes requisitos:</b></td></tr><tr><td>  a)  Copia legalizada del DPI</td></tr><tr><td>  b)  Constancia de colegiado activo (para profesionales)</td></tr><tr><td>  c)  Constancia de Cuota de Pago por Registro de Inscripción en BANRURAL y Recibo 63 A2 extendido por CONAP. Cuenta Fondos Privativos CONAP, Banrural No. 3-099-03774-6. Deberán cancelar un monto de Q 50.00 por anualidad, podrá estar vigente hasta un máximo de cuatro años.</td></tr><tr><td>  d)  Solicitud de inscripción en formulario generado por el SEAF-CONAP.</td></tr><tr><td>  e)  Curriculum vitae según formato generado por el SEAF-CONAP.</td></tr></table>") +
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
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de inscripción.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
                        "<table><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Áreas Protegidas –CONAP-.</td></tr><tr><td>Oficinas Centrales 5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            }
            else
            {
                content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Se le informa que en fecha " + string.Format("{0:dd/MM/yyyy}", Util.FechaDB()) + " ingresó a su bandeja de entrada una nueva solicitud de inscripción. Asimismo tiene " + Cuantas + " solicitudes aún no atendidas.</td></tr><tr><td>De antemano se agradece su pronta atención a la misma.</td></tr></table>") +
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

        private void BtnImprimeCV_Click(object sender, EventArgs e)
        {
            this.AbreVentana("RepCurriculu.aspx?CodRegente=" + this.TxtNoRegente.Text);
        }

        private void BtnSaveEstudi_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.Util.ExisteDato("Select * from testudio where codregente = " + this.TxtNoRegente.Text))
            {
                this.StrSql = "Update testudio set titmedio = '" + this.TxtTitMedio.Text.Trim() + "', Anismedio =  '" + this.TxtAnisMedio.Text.Trim() + "',insmedio = '" + this.TxtInsMedio.Text.Trim() + "', tittecnico =  '" + this.TxtTitTu.Text.Trim() + "', anistecnico = '" + this.TxtAnisTu.Text + "', instecnico = '" + this.TxtInsTu.Text + "', titpro = '" + this.TxtTituProfesional.Text.Trim() + "', anispro = '" + this.TxtAnisProfesiona.Text.Trim() + "', inspro = '" + this.TxtInsPro.Text.Trim() + "', titpos = '" + this.TxtTitPosGrado.Text.Trim() + "', anispos = '" + this.TxtAnisPosGrado.Text.Trim() + "',  inspos = '" + this.TxtinsPos.Text + "', titotro =  '" + this.TxtTitOtro.Text.Trim() + "', anisotro = '" + this.TxtAnisOtro.Text + "', insotro = '" + this.TxtInsOtro.Text + "' where codregente = " + this.TxtNoRegente.Text;
            }
            else
            {
                this.StrSql = "Insert into testudio values (" + this.TxtNoRegente.Text + ", '" + this.TxtTitMedio.Text.Trim() + "', '" + this.TxtAnisMedio.Text.Trim() + "', '" + this.TxtInsMedio.Text.Trim() + "', '" + this.TxtTitTu.Text.Trim() + "', '" + this.TxtAnisTu.Text + "', '" + this.TxtInsTu.Text + "', '" + this.TxtTituProfesional.Text.Trim() + "', '" + this.TxtAnisProfesiona.Text.Trim() + "', '" + this.TxtInsPro.Text.Trim() + "', '" + this.TxtTitPosGrado.Text.Trim() + "', '" + this.TxtAnisPosGrado.Text.Trim() + "', '" + this.TxtinsPos.Text + "', '" + this.TxtTitOtro.Text.Trim() + "', '" + this.TxtAnisOtro.Text + "',  '" + this.TxtInsOtro.Text + "')";
            }
            this.Util.EjecutaIns(this.StrSql);
            this.LblMensajeCv.Text = "Datos de estudio grabados";
            this.LblMensajeCv.Visible = true;
        }

        private void BtnSaveTrabajoActual_Click(object sender, EventArgs e)
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
            this.LblTrabajo.Text = "Datos de trabajo actual grabados";
            this.LblTrabajo.Visible = true;
        }

        private void BtnSubeFoto_Click(object sender, EventArgs e)
        {
            this.OcultaMensajes();
            if (this.UploadFoto.FileName == "")
            {
                this.LblFoto.Text = "Debe seleccionar un archivo";
                this.LblFoto.Visible = true;
            }
            else
            {
                string str = "";
                str = Path.GetExtension(this.UploadFoto.FileName).Substring(1);
                if (UploadFoto.PostedFile.ContentLength > 1048576)
                {
                    this.LblFoto.Text = "Solo puede subir archivos menores a 1MB";
                    this.LblFoto.Visible = true;
                }
                else
                {
                    if ((str == "JPG") || (str == "jpg"))
                    {
                        string path = base.Server.MapPath(".") + @"\FotosRegentes\" + this.TxtNoRegente.Text;
                        this.Util.BorrarArchivo(path);
                        Directory.CreateDirectory(path);
                        this.UploadFoto.SaveAs(path + @"\" + this.UploadFoto.FileName);
                        string str3 = @"~\FotosRegentes\" + this.TxtNoRegente.Text + @"\" + this.UploadFoto.FileName;
                        this.ImgFoto.ImageUrl = str3;
                        this.LblFoto.Text = "Foto subida exitosamente";
                        this.LblFoto.Visible = true;
                    }
                    else
                    {
                        this.LblFoto.Text = "Solo puede subir archivos jpg";
                        this.LblFoto.Visible = true;
                    }
                }
                
            }
        }

        private void CboDep_TextChanged(object sender, EventArgs e)
        {
            if (this.CboDep.Text != "")
            {
                this.CboMun.Items.Clear();
                this.Util.LlenaCombo("Select * from tmunicipio where coddep = " + this.CboDep.SelectedValue, this.CboMun, "CodMun", "Municipio");
            }
        }

        private void GrdCurso_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.OcultaMensajes();
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from tcurso where codregente = ", this.TxtNoRegente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"] });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensajeCursos.Visible = true;
                this.LblMensajeCursos.Text = "Curso eliminado exitosamente";
                this.GrdCurso.Rebind();
            }
        }

        private void GrdCurso_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.TxtNoRegente.Text != "")
            {
                this.StrSql = "Select CodRegente,Corr,Enfoque,Institucion,Duracion from tcurso where codregente = " + this.TxtNoRegente.Text;
                this.Util.LlenaGrid(this.StrSql, this.GrdCurso);
            }
        }

        private void GrdExperiencia_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.OcultaMensajes();
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from texperiencia where codregente = ", this.TxtNoRegente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"] });
                this.Util.EjecutaIns(this.StrSql);
                this.LblExperiencia.Visible = true;
                this.LblExperiencia.Text = "Experiencia eliminada exitosamente";
                this.GrdExperiencia.Rebind();
            }
        }

        private void GrdExperiencia_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.TxtNoRegente.Text != "")
            {
                this.StrSql = "Select CodRegente,Corr,Actividad,Entidad,Ubicacion,Observaciones from texperiencia where codregente = " + this.TxtNoRegente.Text;
                this.Util.LlenaGrid(this.StrSql, this.GrdExperiencia);
            }
        }

        private void GrdExperienciaFores_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.OcultaMensajes();
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from texperienciareg where codregente = ", this.TxtNoRegente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"] });
                this.Util.EjecutaIns(this.StrSql);
                this.LblExperienciaReg.Visible = true;
                this.LblExperienciaReg.Text = "Experiencia forestal eliminada exitosamente";
                this.GrdExperienciaFores.Rebind();
            }
        }

        private void GrdExperienciaFores_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.TxtNoRegente.Text != "")
            {
                this.StrSql = "Select CodRegente,Corr,Finca,ubicacion,tipoactividad,area,periodo from texperienciareg where codregente = " + this.TxtNoRegente.Text;
                this.Util.LlenaGrid(this.StrSql, this.GrdExperienciaFores);
            }
        }

        private void OcultaMensajes()
        {
            this.LblMensaje.Visible = false;
            this.LblGrabo.Visible = false;
            this.LblMensajeCv.Visible = false;
            this.LblMensajeCursos.Visible = false;
            this.LblExperiencia.Visible = false;
            this.LblTrabajo.Visible = false;
            this.LblExperienciaReg.Visible = false;
            this.LblFoto.Visible = false;
            this.LblGrabaTodo.Visible = false;
        }

        bool ValidaRegistroRF(string NoRegistro, string Nombres, string Apellidos, ref string Mensaje)
        {
            bool Resultado = false;
            string DevService = ConsultaRegistroINAB.ConsultarRegistroRegentes(NoRegistro);
            byte[] codificar = Encoding.UTF8.GetBytes(DevService);
            MemoryStream ms = new MemoryStream(codificar);
            ms.Flush();
            ms.Position = 0;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ms);

            try
            {
                XmlNodeList lista = xmldoc.GetElementsByTagName("ConapInfo");
                XmlNodeList datos = ((XmlElement)lista[0]).GetElementsByTagName("Estado");
                foreach (XmlElement nodo in datos)
                {
                    Mensaje = nodo.InnerText;
                    if (Mensaje == "INACTIVO")
                        Mensaje = "El Registro se encuentra inactivo";
                    else
                        Mensaje = "El Registro no existe";
                    Resultado = false;
                }
            }
            catch (Exception ex)
            {
                XmlNodeList lista = xmldoc.GetElementsByTagName("Lista");
                XmlNodeList datos = ((XmlElement)lista[0]).GetElementsByTagName("Conap");
                foreach (XmlElement nodo in datos)
                {
                    Resultado  = true;
                    string a = nodo.InnerText;
                    int i = 0;
                    XmlNodeList fecha = nodo.GetElementsByTagName("FechaVencimiento");
                    XmlNodeList nombre = nodo.GetElementsByTagName("Nombres");
                    XmlNodeList apellido = nodo.GetElementsByTagName("Apellidos");
                    if (nombre[i].InnerText.Trim() != Nombres.Trim())
                    {
                        Mensaje = "Los nombres no coinciden con ese número de registro";
                        return false;
                    }
                    if (apellido[i].InnerText.Trim() != Apellidos.Trim())
                    {
                        Mensaje = "Los apellidos no coinciden con ese número de registro";
                        return false;
                    }
                }
            }
            return Resultado;
        }

        private bool Valida()
        {
            try
            {
                this.LblMensaje.Visible = false;
                if (this.TxtNoRegistroRG.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su n\x00famero de Regente Forestal ante el INAB";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.Util.ExisteDato("Select * from tregente where codreg = '" + this.TxtNoRegistroRG.Text + "' and CODESTATUS = 1"))
                {
                    this.LblMensaje.Text = "Este n\x00famero de Regente Forestal ya existe";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if ((this.TxtNoRegistroEMPF.Text != "") && this.Util.ExisteDato("Select * from tregente where codregempf = '" + this.TxtNoRegistroEMPF.Text + "' and CODESTATUS = 1"))
                {
                    this.LblMensaje.Text = "Este n\x00famero de Elaborador de Planes de Manejo Forestal ya existe";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNombres.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su nombre";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtApellidos.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su apellido";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtDocId.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su documento de identificaci\x00f3n";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtDocId.Text.Length != 13)
                {
                    this.LblMensaje.Text = "El Documento de Identificación debe tener 13 numeros.";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNit.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su Número de Identificación Tributaria";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNacionalidad.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su nacionalidad";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtFecNac.DateInput.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su fecha de nacimiento";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtCorreo.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su correo electr\x00f3nico";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (Util.email_bien_escrito(TxtCorreo.Text) == false)
                {
                    this.LblMensaje.Text = "Debe ingresar un correo electr\x00f3nico valido";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.Util.ExisteDato("Select * from tregente where correo = '" + this.TxtCorreo.Text + "' and  CODESTATUS = 1"))
                {
                    this.LblMensaje.Text = "Este correo electrónico ya existe";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtDireccion.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su direcci\x00f3n";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtTel.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar algun tel\x00e9fono";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (!this.EsMayor(Convert.ToDateTime(this.TxtFecNac.DateInput.SelectedDate)))
                {
                    this.LblMensaje.Text = "Debe Ser Mayor de Edad";
                    this.LblMensaje.Visible = true;
                    this.TxtFecNac.Focus();
                    return false;
                }
                if (!(this.OptioEspe.Items[0].Selected || this.OptioEspe.Items[1].Selected))
                {
                    this.LblMensaje.Text = "Debe seleccionar una opci\x00f3n de especializaci\x00f3n";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                string MensajeRegistro = "";
                if (ValidaRegistroRF(TxtNoRegistroRG.Text,TxtNombres.Text,TxtApellidos.Text, ref MensajeRegistro) == false)
                {
                    this.LblMensaje.Text = MensajeRegistro;
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if ((this.TxtNoRegistroEMPF.Text != "") && ValidaRegistroRF(TxtNoRegistroEMPF.Text, TxtNombres.Text, TxtApellidos.Text, ref MensajeRegistro) == false)
                {
                    this.LblMensaje.Text = MensajeRegistro;
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if ((this.TxtNoRegistroECUT.Text != "") && ValidaRegistroRF(TxtNoRegistroECUT.Text, TxtNombres.Text, TxtApellidos.Text, ref MensajeRegistro) == false)
                {
                    this.LblMensaje.Text = MensajeRegistro;
                    this.LblMensaje.Visible = true;
                    return false;
                }
                
                if (TxtAnis.Text == "")
                {
                    LblMensaje.Text = "El periodo de años no puede estar en blanco";
                    LblMensaje.Visible = true;
                    return false;
                }
                if (Convert.ToInt32(TxtAnis.Text) <= 0)
                {
                    LblMensaje.Text = "El periodo de años no puede ser menor o igual a 0";
                    LblMensaje.Visible = true;
                    return false;
                }
                if (Convert.ToInt32(TxtAnis.Text) > 4)
                {
                    LblMensaje.Text = "El periodo de años no puede ser mayor a 4";
                    LblMensaje.Visible = true;
                    return false;
                }
                if ((CboCategoria.SelectedValue == "2") && (TxtColegiado.Text == ""))
                {
                    LblMensaje.Text = "Debe ingresar su número de colegiado";
                    LblMensaje.Visible = true;
                    return false;
                }
                if (TxtProfesion.Text == "")
                {
                    LblMensaje.Text = "Debe ingresar su profesión";
                    LblMensaje.Visible = true;
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.LblMensaje.Text = "Existe un problema de comunicaci\x00f3n con INAB, por favor comunicarse con las oficinas de CONAP, " + exception.Message;
                this.LblMensaje.Visible = true;
                return false;
            }
        }

        private bool ValidaV2()
        {
            try
            {
                this.LblMensaje.Visible = false;
                if (this.TxtNombres.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su nombre";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtApellidos.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su apellido";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtDocId.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su documento de identificaci\x00f3n";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNit.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su n\x00famero de NIT";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNacionalidad.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su nacionalidad";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtFecNac.DateInput.Text == "")
                {
                    this.LblMensaje.Text = "Debe Ingresar su fecha de nacimiento";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtCorreo.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su correo electr\x00f3nico";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtDireccion.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su direcci\x00f3n";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtTel.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar algun tel\x00e9fono";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (!(this.OptioEspe.Items[0].Selected || this.OptioEspe.Items[1].Selected))
                {
                    this.LblMensaje.Text = "Debe indicar si tiene o no experiencia en el campo forestal";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                if (this.TxtNoRegistroRG.Text == "")
                {
                    this.LblMensaje.Text = "Debe ingresar su n\x00famero de Regente Forestal ante el INAB";
                    this.LblMensaje.Visible = true;
                    return false;
                }
                string MensajeRegistro = "";
                if (ValidaRegistroRF(TxtNoRegistroRG.Text, TxtNombres.Text, TxtApellidos.Text, ref MensajeRegistro) == false)
                {
                    this.LblMensaje.Text = MensajeRegistro;
                    this.LblMensaje.Visible = true;
                }
                
                if (TxtProfesion.Text == "")
                {
                    LblMensaje.Text = "Debe ingresar su profesión";
                    LblMensaje.Visible = true;
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.LblMensaje.Text = "Existe un problema de comunicaci\x00f3n con INAB, por favor comunicarse a oficinas del CONAP, " + exception.Message;
                this.LblMensaje.Visible = true;
                return false;
            }
        }
    }
}