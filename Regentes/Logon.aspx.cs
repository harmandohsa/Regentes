using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Telerik.Web.UI;
using System.Data;

namespace Regentes
{
    public partial class Logon : System.Web.UI.Page
    {
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private string StrSql;
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.BtnIngresa.Click += new EventHandler(this.BtnIngresa_Click);
            if (!base.IsPostBack)
            {
                RadMenu menu = (RadMenu)base.Master.FindControl("Menu");
                menu.Visible = false;
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Visible = false;
                ImageButton button = (ImageButton)base.Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
            }
        }

        private void BtnIngresa_Click(object sender, EventArgs e)
        {
            string str = "";
            bool flag = false;
            this.LblMensaje.Visible = false;
            if (this.TxtUsuario.Text == "")
            {
                this.LblMensaje.Text = "Debe Ingresar un Usuario";
                this.LblMensaje.Visible = true;
            }
            else if (this.TxtClave.Text == "")
            {
                this.LblMensaje.Text = "Debe Ingresar su clave";
                this.LblMensaje.Visible = true;
            }
            else
            {
                this.StrSql = "Select * from tusuario where Usuario = '" + this.TxtUsuario.Text + "'";
                this.cn.Open();
                this.CmTransaccion.CommandText = this.StrSql;
                this.CmTransaccion.Connection = this.cn;
                this.CmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
                if (reader.Read())
                {
                    flag = true;
                    if (reader["CodEstatus"].ToString() == "2")
                    {
                        this.LblMensaje.Text = "Su usuario se encuentra desactivado";
                        this.LblMensaje.Visible = true;
                        return;
                    }
                    this.Session["CodUsuario"] = reader["CodUsuario"].ToString();
                    str = reader["Clave"].ToString();
                    this.Session["CodTipoUsuario"] = reader["CodTipoUsuario"].ToString();
                }
                else
                {
                    flag = false;
                    this.LblMensaje.Text = "El Usuario no existe, vuelva a intentarlo";
                    this.LblMensaje.Visible = true;
                    this.TxtUsuario.Focus();
                }
                this.cn.Close();
                if (flag)
                {
                    if (this.TxtClave.Text == str)
                    {
                        base.Response.Redirect("Inicio.aspx");
                    }
                    else
                    {
                        this.LblMensaje.Text = "Contrase\x00f1a incorrrecta, vuelva a intentarlo";
                        this.LblMensaje.Visible = true;
                        this.TxtClave.Focus();
                    }
                }
            }
        }
    }
}