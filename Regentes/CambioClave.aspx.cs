using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class CambioClave : System.Web.UI.Page
    {
        string StrSql;
        OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        OleDbCommand CmUsuario = new OleDbCommand();
        OleDbCommand cmTransaccion = new OleDbCommand();
        CUtilitarios Util;

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            LnkEnvia.Click += LnkEnvia_Click;
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                string CodTipoUSuario = Session["CodTipoUsuario"].ToString();
            }
        }

        private void LnkEnvia_Click(object sender, System.EventArgs e)
        {
            lblmensaje.Visible = false;
            if (TxtClaveAnt.Text == "")
            {
                lblmensaje.Text = "Debe Ingresar la clave actual";
                lblmensaje.Visible = true;
            }
            else
            {
                if (TxtClaveAnt.Text == ClaveAct(Convert.ToInt32(Session["CodUsuario"])))
                {
                    if (TxtNuevaClave.Text == "")
                    {
                        lblmensaje.Text = "Debe Ingresar la nueva clave";
                        lblmensaje.Visible = true;
                    }
                    else
                    {
                        if (TxtConfClave.Text == "")
                        {
                            lblmensaje.Text = "Debe confirmar la nueva clave";
                            lblmensaje.Visible = true;
                        }
                        else
                        {
                            if (TxtNuevaClave.Text == TxtConfClave.Text)
                            {
                                StrSql = "update tusuario set clave = '" + TxtNuevaClave.Text + "'  where CodUsuario = " + Session["CodUsuario"] + "";
                                cn.Open();
                                cmTransaccion.CommandText = StrSql;
                                cmTransaccion.Connection = cn;
                                cmTransaccion.CommandType = CommandType.Text;
                                cmTransaccion.ExecuteNonQuery();
                                cn.Close();
                                lblmensaje.Text = "Clave Actualiza con exito";
                                lblmensaje.Visible = true;
                                //Response.Redirect("Inicio.aspx");
                            }
                            else
                            {
                                lblmensaje.Text = "las clave no son iguales";
                                lblmensaje.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    lblmensaje.Text = "La clave actual no coincide";
                    lblmensaje.Visible = true;
                }
            }




        }

        public string ClaveAct(int CodUsuario)
        {
            StrSql = "Select * from tusuario where CodUsuario = " + CodUsuario + "";
            cn.Open();
            CmUsuario.CommandText = StrSql;
            CmUsuario.Connection = cn;
            CmUsuario.CommandType = CommandType.Text;

            OleDbDataReader DrUsuario = CmUsuario.ExecuteReader();
            if (DrUsuario.Read())
            {
                string Clave = DrUsuario.GetString(5);
                cn.Close();
                return Clave;

            }
            else
            {
                cn.Close();
                return "";

            }
        }
    }
}