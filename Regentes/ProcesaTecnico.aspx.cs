using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Telerik.Web.UI;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class ProcesaTecnico : System.Web.UI.Page
    {
        private OleDbCommand CmTransaccion = new OleDbCommand(); 
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private string StrSql;
        private CUtilitarios Util;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdConclusion.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdConclusion_NeedDataSource);
            this.GrdConclusion.ItemCommand += new GridCommandEventHandler(this.GrdConclusion_ItemCommand);
            this.BtnAddConclusion.Click += new EventHandler(this.BtnAddConclusion_Click);
            this.BtnGrabar.Click += new EventHandler(this.BtnGrabar_Click);
            this.BtnPrint.Click += new EventHandler(this.BtnPrint_Click);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                TxtFecRecibo.SelectedDate = DateTime.Now;
                TxtCodTramite.Text = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();
                this.TxtCodregente.Text = base.Request.QueryString["CodRegente"].ToString();
                TxtNus.Text = Request.QueryString["nus"].ToString();
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                this.Util.LlenaCombo("Select * from trecomendacin", this.CboRecomendacion, "CodRecomendacion", "Recomendacion");
                this.CargaData();
                TxtVigencia.Text = Util.ObtieneRegistro("Select * from tregente where codregente = " + Request.QueryString["CodRegente"] + "", "Anis").ToString();
                if (this.Util.ObtieneRegistro("Select * from tregente where codregente = " + base.Request.QueryString["CodRegente"], "CodCategoria").ToString() == "1")
                    CboCategoria.SelectedValue = "1";
                else
                    CboCategoria.SelectedValue = "2";
                CboCategoria.Enabled = false;
                if (TxtCodTramite.Text == "2")
                {
                    Label6.Visible = false;
                    Label7.Visible = false;
                    TxtConstancia.Visible = false;
                    TxtNoConstancia.Visible = false;
                    Label12.Visible = false;
                    Label13.Visible = false;
                    CboCategoria.Visible = false;
                    TxtVigencia.Visible = false;
                    Label2.Visible = false;
                    Label3.Visible = false;
                    Label4.Visible = false;
                    Label5.Visible = false;
                    TxtRecibo.Visible = false;
                    TxtFecRecibo.Visible = false;
                    TxtBoleta.Visible = false;
                }
                else if (TxtCodTramite.Text == "3")
                {
                    Label13.Visible = false;
                    TxtVigencia.Visible = false;
                }
                else if (TxtCodTramite.Text == "4")
                {
                    TxtVigencia.Text = Util.ObtieneRegistro("Select * from trenovacion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Anis").ToString();
                }
            }
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        private void AgregaProvidencia(int CodRegente)
        {
            string str = Util.CodRegion(Convert.ToInt32(this.TxtCodregente.Text));
            decimal num = Util.MaxCorr("Select Max(corr) as Maximo from tprovidencia");
            string str2 = Util.ObtieneRegistro("Select * from tusuario where codregion = " + str + " and CodtipoUsuario = 3 and codestatus = 1", "CodUsuario").ToString();
            StrSql = string.Concat(new object[] { "Insert into tprovidencia values (", this.TxtCodregente.Text, ", '", string.Format("{0:MM/dd/yyyy HH:mm:ss}", this.Util.FechaDB()), "', ", str2, ", ", num, ", ", TxtNus.Text, ")" });
            Util.EjecutaIns(this.StrSql);
        }

        private void EnvioCorreo(string Mail, string Nombre, string Regente)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Emisión de Dictamen Técnico";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que la gestión de solicitud para ser inscrito como regente del señor " + Regente + " ya cuenta con dictamen técnico para que pueda proceder a dar su Visto Bueno</td></tr></table>") +
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

        private void BtnAddConclusion_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (this.TxtConclusion.Text == "")
            {
                this.LblMensaje.Text = "Debe Agregar una Conclusion";
                this.LblMensaje.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from tdetconclusion where codregente = " + this.TxtCodregente.Text + " and nus = "  + TxtNus.Text + "");
                this.StrSql = string.Concat(new object[] { "Insert into tdetconclusion values (", this.TxtCodregente.Text, ", ", num, ", '", this.TxtConclusion.Text, "', ", TxtNus.Text, ")" });
                this.Util.EjecutaIns(this.StrSql);
                this.GrdConclusion.Rebind();
                this.TxtConclusion.Text = "";
            }
        }

        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (this.Valida())
            {
                if (this.Util.ExisteDato("Select * from tdictamentec where CodRegente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + ""))
                {
                    this.StrSql = "Update tdictamentec set referencia = '" + this.TxtReferencia.Text + "', reciboing = '" + this.TxtRecibo.Text + "', fecharecibo = '" + string.Format("{0:MM/dd/yyyy}", this.TxtFecRecibo.DateInput.SelectedDate) + "', bolbanrural = '" + this.TxtBoleta.Text + "', Constancia = '" + this.TxtConstancia.Text + "', nocons = '" + this.TxtNoConstancia.Text + "', codrecomendacion = " + this.CboRecomendacion.SelectedValue + ", folios = " + this.TxtFolio.Text + ", Categoria = '" + CboCategoria.Text + "', Vigencia = " + this.TxtVigencia.Text + ", puesto = '" + TxtPuesto.Text + "' where Codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                }
                else
                {
                    decimal num = this.Util.MaxCorr("Select Max(noDictamen) as Maximo from tdictamentec where YEAR(FECCRE) = year(getdate())");
                    //if (TxtCodTramite.Text == "1" || TxtCodTramite.Text == "4")
                    this.AgregaProvidencia(Convert.ToInt32(this.TxtCodregente.Text));
                    this.StrSql = string.Concat(new object[] { 
                        "Insert into tdictamentec values (", this.TxtCodregente.Text, ", ", this.Session["CodUsuario"], ", '", string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()), "', '", this.TxtReferencia.Text, "','", this.TxtRecibo.Text, "', '", string.Format("{0:MM/dd/yyyy}", this.TxtFecRecibo.DateInput.SelectedDate), "', '", this.TxtBoleta.Text, "', ", this.CboRecomendacion.SelectedValue, 
                        ", ", this.TxtFolio.Text, ", '", this.TxtConstancia.Text, "', '", this.TxtNoConstancia.Text, "', ", num, ",'", this.Util.GeneraIdElec(Convert.ToInt32(this.Session["CodUsuario"])), "', ", this.Util.CodRegion(Convert.ToInt32(this.TxtCodregente.Text)), ", '", CboCategoria.Text, "', ", this.TxtVigencia.Text, ", ", TxtNus.Text, ", '", TxtPuesto.Text, "')"
                     });
                }
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensaje.Text = "Dictamen grabado con éxito";
                this.LblMensaje.Visible = true;
                //string CodRerion = this.Util.CodRegion(Convert.ToInt32(this.TxtCodregente.Text));
                //string CodUsrDirector = this.Util.ObtieneRegistro("Select * from tusuario where codregion = " + CodRerion + " and CodtipoUsuario = 3", "CodUsuario").ToString();
                //EnvioCorreo(Util.ObtieneRegistro("Select * from tusuario where codusuario = " + CodUsrDirector + "","Mail").ToString(),Util.NomUsuario(Convert.ToInt32(CodUsrDirector)),Util.NomRegente(Convert.ToInt32(TxtCodregente.Text)));
                //StrSql = "Update tregente set codestatus = 3 where codregente = " + TxtCodregente.Text + "";
                //Util.EjecutaIns(StrSql);
                //BtnPrint.Visible = true;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            this.AbreVentana("RepDictamenTec.aspx?CodRegente=" + this.TxtCodregente.Text);
        }

        private void CargaData()
        {
            this.StrSql = "Select * from tdictamentec where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
            this.cn.Open();
            this.CmTransaccion.CommandText = this.StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                this.TxtReferencia.Text = reader["Referencia"].ToString();
                this.TxtFecRecibo.SelectedDate = new DateTime?(Convert.ToDateTime(reader["FechaRecibo"]));
                this.TxtRecibo.Text = reader["ReciboIng"].ToString();
                this.TxtBoleta.Text = reader["BolBanrural"].ToString();
                this.TxtConstancia.Text = reader["Constancia"].ToString();
                this.TxtNoConstancia.Text = reader["nocons"].ToString();
                this.TxtFolio.Text = reader["Folios"].ToString();
                this.CboRecomendacion.SelectedValue = reader["CodRecomendacion"].ToString();
                CboCategoria.Text = reader["Categoria"].ToString();
                if (reader["Categoria"].ToString() == "Técnico")
                    CboCategoria.SelectedValue = "1";
                else
                    CboCategoria.SelectedValue = "2";
                this.TxtVigencia.Text = reader["Vigencia"].ToString();
                TxtPuesto.Text = reader["Puesto"].ToString();
                //this.BtnPrint.Visible = true;
            }
            this.cn.Close();
            TxtNoConstancia.Text = Util.ObtieneRegistro("Select * from tregente where codregente = " + TxtCodregente.Text + "", "nocol").ToString();
        }

        private void GrdConclusion_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from tdetconclusion where codregente = ", this.TxtCodregente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"], " and nus = ", TxtNus.Text });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Recomendación Eliminada Exitosamente";
                this.GrdConclusion.Rebind();
            }
        }

        private void GrdConclusion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.StrSql = "Select  CodRegente,Corr,Conclusion from tdetconclusion where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
            this.Util.LlenaGrid(this.StrSql, this.GrdConclusion);
        }

        private bool Valida()
        {
            this.LblMensaje.Visible = false;
            if (this.TxtReferencia.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar la referencia";
                return false;
            }
            if (TxtCodTramite.Text != "2" && this.TxtRecibo.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar el n\x00famero del recibo";
                return false;
            }
            if (TxtCodTramite.Text != "2" && this.TxtFecRecibo.DateInput.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar la fecha del recibo";
                return false;
            }
            if (TxtCodTramite.Text != "2" && this.TxtBoleta.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar el n\x00famero de boleta de BANRURAL";
                return false;
            }
            if (TxtCodTramite.Text != "2" && TxtConstancia.Text == "" && CboCategoria.SelectedValue == "2")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar la serie de la constancia";
                return false;
            }
            if (TxtCodTramite.Text != "2" && TxtNoConstancia.Text == "" && CboCategoria.SelectedValue == "2")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar el n\x00famero de constancia";
                return false;
            }
            //if (this.GrdConclusion.Items.Count == 0)
            //{
            //    this.LblMensaje.Visible = true;
            //    this.LblMensaje.Text = "Debe Ingresar al menos una Conclusion";
            //    return false;
            //}
            if (this.TxtFolio.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar la cantidad de folios";
                return false;
            }
            if (TxtPuesto.Text == "")
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Debe Ingresar el puesto";
                return false;
            }
            if (TxtFecRecibo.SelectedDate > Util.FechaDB())
            {
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "La Fecha no puede ser posterior a la actual";
                return false;
            }
            return true;
        }
    }
}