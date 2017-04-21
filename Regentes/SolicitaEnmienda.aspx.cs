using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class SolicitaEnmienda : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            BtnAddEnmienda.Click += new EventHandler(BtnAddEnmienda_Click);
            GrdEnmienda.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdEnmienda_NeedDataSource);
            GrdEnmienda.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdEnmienda_ItemCommand);
            BtnTerminarEnmienda.Click += new EventHandler(BtnTerminarEnmienda_Click);
            BtnCerrarEnmiendas.Click += new EventHandler(BtnCerrarEnmiendas_Click);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.TxtCodregente.Text = base.Request.QueryString["CodRegente"].ToString();
                TxtNus.Text = Request.QueryString["nus"].ToString();
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));

                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                if (Request.QueryString["llamada"] != null)
                {
                    if (Request.QueryString["llamada"].ToString() == "1")
                    {
                        Oculta();
                        TxtCodregente.Text = Request.QueryString["CodRegente"].ToString();
                        TxtMaxEnmienda.Text = Request.QueryString["Corr"].ToString();
                        TxtNus.Text = Request.QueryString["nus"].ToString();
                        GrdEnmienda.Rebind();
                        if (Util.ObtieneRegistro("Select * from tenmiendatec where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "", "CodEstatus").ToString() == "1")
                            BtnCerrarEnmiendas.Visible = true;
                        string CodUsuarioHizo = Util.ObtieneRegistro("Select * from tenmiendatec where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + " and Corr = " + TxtMaxEnmienda.Text + "", "CodUsuario").ToString();
                        if (CodUsuarioHizo != Session["CodUsuario"].ToString())
                            BtnCerrarEnmiendas.Visible = false;
                    }
                }
            }
        }

        void BtnCerrarEnmiendas_Click(object sender, EventArgs e)
        {
            StrSql = "Update tenmiendatec set codestatus = 2, FECCIERRE = '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "' where CodRegente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "";
            Util.EjecutaIns(StrSql);
            BtnCerrarEnmiendas.Visible = false;
            LblMensaje.Text = "Las enmiendas se han cerrado exitosamente";
            LblMensaje.Visible = true;
        }
        
        void Oculta()
        {
            GrdEnmienda.Columns[4].Visible = false;
            BtnAddEnmienda.Visible = false;
            BtnTerminarEnmienda.Visible = false;
        }

        private void EnvioCorreo(string Mail, string Nombre, string Delegacion, string Enmiendas)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de Enmiendas Técnicas de Inscripción de Regente";
            content = "<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>La " + Delegacion + " del Consejo Nacional de Áreas Protegidas hace constar que la documentación presentada para Inscripción como Regente Forestal en Áreas Protegidas NO cumple con los requisitos establecidos por el CONAP, por lo que es necesario que se atienda lo siguiente. Es necesario que se atienda lo siguiente:</td></tr></table><table>";
            StrSql = "Select * from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "";
            cn.Open();
            CmTransaccion.CommandText = this.StrSql;
            CmTransaccion.Connection = this.cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                content = content + "<tr><td>-  " + reader["enmienda"].ToString() + "</td></tr>";
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
        }

        void BtnTerminarEnmienda_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (GrdEnmienda.Items.Count == 0)
            {
                LblMensaje.Text = "No puede terminar enmiendas vacias";
                LblMensaje.Visible = true;
            }
            else
            {
                //string NombreReg = Util.NomRegente(Convert.ToInt32(TxtCodregente.Text));
                //string Correo = Util.ObtieneRegistro("Select * from tregente where codregente = " + TxtCodregente.Text + "", "Correo").ToString();
                //string Region = Util.ObtieneRegistro("Select nombre from tregion where CodRegion = " + Util.ObtieneRegistro("Select CodRegion from tusuario where CodUsuario = " + Session["CodUsuario"] + "", "CodRegion") + "", "nombre").ToString();
                
                //EnvioCorreo(Correo, NombreReg,Region,"");
                decimal MaxEnmienda = Util.MaxCorr("Select Max(Corr) as Maximo from tenmiendatec");
                decimal MaxNoEnmienda = Util.MaxCorr("Select Max(No) as Maximo from tenmiendatec where YEAR(FECCRE) = year(getdate())");
                string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                StrSql = "Insert into tenmiendatec values (" + MaxEnmienda + ", " + TxtCodregente.Text + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "',null," + Session["CodUsuario"] + ",1," + TxtNus.Text + ", '" + IdElec + "',NULL,0, " + MaxNoEnmienda + ")";
                Util.EjecutaIns(StrSql);
                TxtMaxEnmienda.Text = MaxEnmienda.ToString();

                for (int i = 0; i < GrdEnmienda.Items.Count; i++)
                {
                    decimal MAxDetEnmienda = Util.MaxCorr("Select Max(CorrEnmienda) as Maximo from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "");
                    StrSql = "Insert into tdetdictamen values (" + TxtMaxEnmienda.Text + ", " + MAxDetEnmienda + ", " + TxtCodregente.Text + ", '" + GrdEnmienda.Items[i]["Enmienda"].Text + "', '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "'," + Session["CodUSuario"] + ", " + TxtNus.Text + ")";
                    Util.EjecutaIns(StrSql);
                }
                TxtEnmienda.Text = "";
                GrdEnmienda.Columns[4].Visible = false;
                BtnAddEnmienda.Visible = false;
                BtnTerminarEnmienda.Visible = false;
                LblMensaje.Text = "Enmiendas guardadas con éxito";
                LblMensaje.Visible = true;
                GrdEnmienda.Rebind();
                TxtEnmienda.Text = "";
                StrSql = "Delete from TEMPENMIENDATEC where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
            }
            
        }

        string Enmiendas(string CodRegente, string CodEnmienda, string nus)
        {
            string Enmienda = "";
            StrSql = "Select * from tdetdictamen where codregente = " + CodRegente + " and Corr = " + CodEnmienda + " and nus = " + nus  + "";
            cn.Open();
            CmTransaccion.CommandText = this.StrSql;
            CmTransaccion.Connection = this.cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                if (Enmienda == "")
                    Enmienda = "-  " + reader["enmienda"].ToString();
                else
                    Enmienda = Enmienda + "- " + reader["enmienda"].ToString();
            }
            cn.Close();
            return Enmienda;
        }


        void GrdEnmienda_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                StrSql = "delete from TEMPENMIENDATEC where codregente = " + TxtCodregente.Text + " and Corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"] + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                LblMensaje.Visible = true;
                LblMensaje.Text = "Enmienda eliminada exitosamente";
                GrdEnmienda.Rebind();
                //StrSql = "delete from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"] + " and CorrEnmienda = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CorrEnmienda"] + " and nus = " + TxtNus.Text + "";
                //this.Util.EjecutaIns(this.StrSql);
                //this.LblMensaje.Visible = true;
                //this.LblMensaje.Text = "Enmienda eliminida exitosamente";
                //GrdEnmienda.Rebind();
                //if (GrdEnmienda.Items.Count == 0)
                //{
                //    StrSql = "delete from tenmiendatec where CodRegente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "";
                //    Util.EjecutaIns(StrSql);
                //}
            }
        }

        void GrdEnmienda_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (TxtMaxEnmienda.Text != "")
            {
                StrSql = "Select CodRegente,Corr,CorrEnmienda,Enmienda from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, GrdEnmienda);
            }
            else
            {
                StrSql =  "Select CodRegente,Corr,Corr as CorrEnmienda,Enmienda from TEMPENMIENDATEC where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, GrdEnmienda);
            }
            
        }

        void BtnAddEnmienda_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (TxtEnmienda.Text == "")
            {
                LblMensaje.Text = "Debe ingresar la enmienda";
                LblMensaje.Visible = true;
            }
            else
            {
                decimal MaxEnmienda = Util.MaxCorr("Select Max(Corr) as Maximo from TEMPENMIENDATEC where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "");
                StrSql = "Insert into TEMPENMIENDATEC values (" + TxtCodregente.Text + ", " + MaxEnmienda + "," + TxtNus.Text + ",'" + TxtEnmienda.Text + "')";
                Util.EjecutaIns(StrSql);
                LblMensaje.Text = "Enmienda agregada";
                LblMensaje.Visible = true;
                GrdEnmienda.Rebind();
                TxtEnmienda.Text = "";
                //if (GrdEnmienda.Items.Count == 0)
                //{
                //    decimal MaxEnmienda = Util.MaxCorr("Select Max(Corr) as Maximo from tenmiendatec");
                //    string IdElec = Util.GeneraIdElec(Convert.ToInt32(Session["CodUsuario"]));
                //    StrSql = "Insert into tenmiendatec values (" + TxtCodregente.Text + ", " + MaxEnmienda + ", '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "',null," + Session["CodUsuario"] + ",1," + TxtNus.Text + ", '" + IdElec + "',NULL)";
                //    Util.EjecutaIns(StrSql);
                //    TxtMaxEnmienda.Text = MaxEnmienda.ToString();
                //    decimal MAxDetEnmienda = Util.MaxCorr("Select Max(CorrEnmienda) as Maximo from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "");
                //    StrSql = "Insert into tdetdictamen values (" + TxtCodregente.Text + ", " + TxtMaxEnmienda.Text + ", " + MAxDetEnmienda + ", '" + TxtEnmienda.Text + "', '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "'," + Session["CodUSuario"] + ", " + TxtNus.Text + ")";
                //    Util.EjecutaIns(StrSql);
                //    LblMensaje.Text = "Enmienda agregada";
                //    LblMensaje.Visible = true;
                //    GrdEnmienda.Rebind();
                //    TxtEnmienda.Text = "";
                //}
                //else
                //{
                //    decimal MAxDetEnmienda = Util.MaxCorr("Select Max(CorrEnmienda) as Maximo from tdetdictamen where codregente = " + TxtCodregente.Text + " and Corr = " + TxtMaxEnmienda.Text + " and nus = " + TxtNus.Text + "");
                //    StrSql = "Insert into tdetdictamen values (" + TxtCodregente.Text + ", " + TxtMaxEnmienda.Text + ", " + MAxDetEnmienda + ", '" + TxtEnmienda.Text + "', '" + string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()) + "'," + Session["CodUSuario"] + ", " + TxtNus.Text + ")";
                //    Util.EjecutaIns(StrSql);
                //    LblMensaje.Text = "Enmienda agregada";
                //    LblMensaje.Visible = true;
                //    GrdEnmienda.Rebind();
                //    TxtEnmienda.Text = "";
                //}
            }
        }
    }
}