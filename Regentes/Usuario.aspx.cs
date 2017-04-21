using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Telerik.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Regentes
{
    public partial class Usuario : System.Web.UI.Page
    {
        private OleDbCommand CmUsuario = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private string StrSql = "";
        private CUtilitarios Util;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.ItemDataBound += new GridItemEventHandler(this.GrdDetalle_ItemDataBound);
            this.BtnNuevo.Click += new EventHandler(this.BtnNuevo_Click);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.LnkGrabar.Click += new EventHandler(this.LnkGrabar_Click);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);

            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                this.Util.LlenaCombo("Select * from ttipousuario where codtipousuario > 0", this.CboTipoUsuario, "CodTipoUsuario", "TipoUsuario");
                this.Util.LlenaCombo("Select * from tregion", this.CboRegion, "CodRegion", "nombre");
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[7].Visible = false;
            GrdDetalle.Columns[8].Visible = false;
            GrdDetalle.Columns[9].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1500;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[7].Visible = false;
            GrdDetalle.Columns[8].Visible = false;
            GrdDetalle.Columns[9].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();
        }

        private void EnvioCorreo(string Mail, string Usuario, string Clave, string Nombre, string TipoUsuario)
        {
            MailMessage message = new MailMessage();
            string content = "";
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Cuenta"], "CONAP Administrador");
            message.To.Add(Mail);
            message.Subject = "Notificacion de creaci\x00f3n de Usuario";
            content = (("<body><table><tr><td><b>NOTIFICACI\x00d3N ELECTR\x00d3NICA, DEL ADMINISTRADOR DEL SISTEMA:</b></td></tr><tr><td>Estimado: " + Nombre + "</td></tr><tr><td>Le informamos que se ha creado su usuario con rol de: " + TipoUsuario + " para poder acceder al Módulo de Registro de Regentes Forestales en Áreas Protegidas; su usuario es: " + Usuario + " y la contrase\x00f1a: " + Clave + "</td></tr></table>") + "<table><tr><td>Ingrese al sistema por medio del siguiente enlace: http://168.234.196.101:81/Regentes/ para revisar la informaci\x00f3n</td></tr><tr><td></td></tr><tr><td><b>CONSEJO NACIONAL DE ÁREAS PROTEGIDAS</b></td></tr><tr><td><b>MEGADIVERSIDAD PARA SIEMPRE</b></td></tr><tr><td></td></tr><tr><td> <font color=#FF0000>Por favor no responda este correo.</font></td></tr></table>") + "<table><tr><td></td></tr><tr><td>Este correo electr\x00f3nico fue enviado a: " + Nombre + ", a trav\x00e9s del Módulo de Registro de Regentes Forestales en Áreas Protegidas del Consejo Nacional de Areas Protegidas –CONAP-.</td></tr><tr><td>5ta. Avenida 6-06, Zona 1. Edificio IPM, 5to, 6to y 7mo Nivel, Ciudad de Guatemala, C.A.</td></tr><tr><td>Tel\x00e9fono: (502) 2422-6700, Fax (502) 2253-4141</td></tr></table></body>";
            AlternateView item = null;
            item = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
            LinkedResource resource = null;
            resource = new LinkedResource(base.Server.MapPath("Imagenes/principalfinal20122.gif"))
            {
                ContentId = "imagen001"
            };
            //  item.LinkedResources.Add(resource);
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
        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (e.CommandName == "CmdModificar")
            {
                if (Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodTipoUsuario"]) > 0)
                {
                    this.TxtCodUsuario.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodUsuario"].ToString();
                    this.TxtUsuario.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Usuario"].ToString();
                    this.TxtNombres.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Nombres"].ToString();
                    this.TxtApellidos.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Apellidos"].ToString();
                    this.CboTipoUsuario.SelectedValue = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodTipoUsuario"].ToString();
                    this.CboTipoUsuario.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TipoUsuario"].ToString();
                    this.StrSql = "Select * from tusuario where codusuario = " + this.TxtCodUsuario.Text;
                    this.cn.Open();
                    this.CmUsuario.CommandText = this.StrSql;
                    this.CmUsuario.Connection = this.cn;
                    this.CmUsuario.CommandType = CommandType.Text;
                    OleDbDataReader reader = this.CmUsuario.ExecuteReader();
                    if (reader.Read())
                    {
                        this.TxtCorreo.Text = reader["Mail"].ToString();
                        TxtCorreoMod.Text = reader["Mail"].ToString();
                        this.CboRegion.SelectedValue = reader.GetDecimal(6).ToString();
                        if (reader["enfunciones"].ToString() == "1")
                            ChkEnFunciones.Checked = true;
                        else
                            ChkEnFunciones.Checked = false;
                        
                    }
                    reader.Close();
                    this.cn.Close();
                }
                else
                {
                    this.LblMensaje.Text = "A los usuarios tipo Administrador solo puede reestablecer la clave";
                    this.LblMensaje.Visible = true;
                }
            }
            else if (e.CommandName == "CmdClave")
            {
                this.StrSql = "Update tusuario set clave = 1234  where codusuario = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodUsuario"].ToString();
                if (this.Util.EjecutaIns(this.StrSql))
                {
                    this.LblMensaje.Text = "Contrase\x00f1a reestablecida a 1234";
                    this.LblMensaje.Visible = true;
                }
            }
            else if (e.CommandName == "Activar")
            {
                this.StrSql = "Update tusuario set CodEstatus = 1  where codusuario = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodUsuario"].ToString();
                if (this.Util.EjecutaIns(this.StrSql))
                {
                    this.LblMensaje.Text = "Usuario Activado";
                    this.LblMensaje.Visible = true;
                }
                this.GrdDetalle.Rebind();
            }
            else if (e.CommandName == "Desactivar")
            {
                this.StrSql = "Update tusuario set CodEstatus = 2 where codusuario = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodUsuario"].ToString();
                if (this.Util.EjecutaIns(this.StrSql))
                {
                    this.LblMensaje.Text = "Usuario Desactivado";
                    this.LblMensaje.Visible = true;
                }
                this.GrdDetalle.Rebind();
            }
        }

        private void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                if (this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + item.GetDataKeyValue("CodUsuario"), "CodEstatus").ToString() == "1")
                {
                    item["EstUsr"].FindControl("ImgAct").Visible = false;
                    item["EstUsr"].FindControl("ImgDown").Visible = true;
                }
                else
                {
                    item["EstUsr"].FindControl("ImgAct").Visible = true;
                    item["EstUsr"].FindControl("ImgDown").Visible = false;
                }
            }
        }

        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.StrSql = "select CodUsuario,Usuario,Nombres,Apellidos,TipoUsuario,a.CODTIPOUSUARIO,nombre from TUSUARIO a, TTIPOUSUARIO b, tregion c where a.codregion = c.codregion and a.CODTIPOUSUARIO = b.CODTIPOUSUARIO and CODUSUARIO > 1 order by CodEstatus desc";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }

        private void Limpiar()
        {
            this.TxtUsuario.Text = "";
            this.TxtNombres.Text = "";
            this.TxtApellidos.Text = "";
            this.TxtCorreo.Text = "";
            this.TxtCodUsuario.Text = "";
            ChkEnFunciones.Checked = false;
        }

        private void LnkGrabar_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (this.TxtCodUsuario.Text == "")
            {
                if (this.Valida())
                {
                    decimal num = this.Util.MaxCorr("Select Max(CodUsuario) as MAXIMO from tusuario");
                    this.StrSql = string.Concat(new object[] { 
                        "Insert into tusuario values (", num, ", '", this.TxtNombres.Text, "','", this.TxtApellidos.Text, "','", this.TxtCorreo.Text, "', '", this.TxtUsuario.Text, "', '1234', ", this.CboRegion.SelectedValue, ",  ", this.CboTipoUsuario.SelectedValue, ", '", string.Format("{0:MM/dd/yyyy hh:mm:ss}", this.Util.FechaDB()), 
                        "',1," + Util.IIf(ChkEnFunciones.Checked,1,0) + ")"
                     });
                    if (this.Util.EjecutaIns(this.StrSql))
                    {
                        this.LblMensaje.Text = "El Usuario fue ingresado exitosamente";
                        this.LblMensaje.Visible = true;
                        this.GrdDetalle.Rebind();
                        StrSql = "Insert into tpermiso select " + num + ",CodMenu,CodForma,CodRol,CodSistema from tpermisopre where CodTipoUsuario = " +  CboTipoUsuario.SelectedValue + "";
                        Util.EjecutaIns(StrSql);
                        this.EnvioCorreo(this.TxtCorreo.Text, this.TxtUsuario.Text, "1234", this.TxtNombres.Text + " " + this.TxtApellidos.Text, this.CboTipoUsuario.Text);
                        this.Limpiar();
                    }
                }
            }
            else
            {
                if (TxtCorreo.Text != TxtCorreoMod.Text && Util.ExisteDato("Select * from tusuario where mail = '" + TxtCorreo.Text + "'"))
                {
                    LblMensaje.Text = "Este Correo ya existe";
                    LblMensaje.Visible = true;
                }
                else
                {
                    this.StrSql = "Update tusuario set nombres = '" + this.TxtNombres.Text.Trim() + "', apellidos = '" + this.TxtApellidos.Text.Trim() + "', codtipousuario = " + this.CboTipoUsuario.SelectedValue + ", codregion = " + this.CboRegion.SelectedValue + ", mail = '" + TxtCorreo.Text + "', enfunciones = " + Util.IIf(ChkEnFunciones.Checked,1,0) + "  where codusuario = " + this.TxtCodUsuario.Text;
                    if (this.Util.EjecutaIns(this.StrSql))
                    {
                        this.LblMensaje.Text = "El Usuario fue Modificado exitosamente";
                        this.LblMensaje.Visible = true;
                        this.GrdDetalle.Rebind();
                        this.Limpiar();
                    }
                }
                
            }
        }

        int CuantosUsuario(string CodTipoUsuario, string CodRegion)
        {
            return Convert.ToInt32(Util.ObtieneRegistro("Select count(*) as cuantos from tusuario where CodTipoUsuario = " + CodTipoUsuario + " and CodRegion = " + CodRegion + " and codestatus = 1", "cuantos"));
        }

        private bool Valida()
        {
            LblMensaje.Visible = false;
            if (TxtUsuario.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el usuario";
                LblMensaje.Visible = true;
                return false;
            }
            else if (Util.ExisteDato("Select * from tusuario where usuario = '" + TxtUsuario.Text + "'"))
            {
                LblMensaje.Text = "Este Usuario ya existe";
                LblMensaje.Visible = true;
                return false;
            }
            else if (Util.ExisteDato("Select * from tusuario where mail = '" + TxtCorreo.Text + "'"))
            {
                LblMensaje.Text = "Este Correo ya existe";
                LblMensaje.Visible = true;
                return false;
            }
            else if (TxtNombres.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar los nombre";
                LblMensaje.Visible = true;
                return false;
            }
            else if (CboTipoUsuario.Text == "")
            {
                LblMensaje.Text = "Debe Seleccionar el tipo de usuario";
                LblMensaje.Visible = true;
                return false;
            }
            else if (TxtCorreo.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el correo electrónico";
                LblMensaje.Visible = true;
                return false;
            }    
            else if (Util.email_bien_escrito(TxtCorreo.Text) == false)
            {
                LblMensaje.Text = "Debe Ingresar un correo electrónico valido";
                LblMensaje.Visible = true;
                return false;
            }
            else if (CboTipoUsuario.SelectedValue != "0" && CboRegion.Text == "")
            {
                LblMensaje.Text = "Debe Seleccionar la región";
                LblMensaje.Visible = true;
                return false;
            }
            else if (CboTipoUsuario.SelectedValue == "1" && CuantosUsuario(CboTipoUsuario.SelectedValue, CboRegion.SelectedValue) == 1)
            {
                LblMensaje.Text = "Ya existe un usuario tipo secretaria para esta región, solo puede haber uno por región";
                LblMensaje.Visible = true;
                return false;
            }
            else if (CboTipoUsuario.SelectedValue == "3" && CuantosUsuario(CboTipoUsuario.SelectedValue, CboRegion.SelectedValue) == 1)
            {
                LblMensaje.Text = "Ya existe un usuario tipo Director Regional para esta región, solo puede haber uno por región";
                LblMensaje.Visible = true;
                return false;
            }
            //else if (CboTipoUsuario.SelectedValue == "4" && CuantosUsuario(CboTipoUsuario.SelectedValue, CboRegion.SelectedValue) == 1)
            //{
            //    LblMensaje.Text = "Ya existe un usuario tipo Asesor jurídico para esta región, solo puede haber uno por región";
            //    LblMensaje.Visible = true;
            //    return false;
            //}
            else if (CboTipoUsuario.SelectedValue == "5" && CuantosUsuario(CboTipoUsuario.SelectedValue, CboRegion.SelectedValue) == 1)
            {
                LblMensaje.Text = "Ya existe un usuario tipo Director Jurídico para esta región, solo puede haber uno por región";
                LblMensaje.Visible = true;
                return false;
            }
            return true;
        }
    }
}