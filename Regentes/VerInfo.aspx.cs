using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.OleDb;
using System.Data;

namespace Regentes
{
    public partial class VerInfo : System.Web.UI.Page
    {
        CUtilitarios Util;
        string StrSql;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                RadMenu menu = (RadMenu)base.Master.FindControl("Menu");
                menu.Visible = false;
                ImageButton button = (ImageButton)base.Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
                CargaDatos();
            }
        }

        void CargaDatos()
        {
            StrSql = "Select CODREG,CODREGEMPF,CODREGECUT,APELLIDOS,NOMBRES,CATEGORIA,CODID,NIT,NACIONALIDAD,GENERO,CONVERT(CHAR(11),FECNAC,103) as FecNac,CORREO,DIRECCION,DEPARTAMENTO,MUNICIPIO, " +
                     "TELEFONO,FAX,PROFESION,NOCOL,ESPECIALIZACION,case when EXPERIENCIA =  1 then 'SI' else 'NO' end as expe, CUAL " +
                     "from THISTREGENTE a, tgenero b, TDEPARTAMENTO c,TMUNICIPIO d,TCATEGORIA e " +
                     "where a.CODGENERO = b.CODGENERO and c.CODDEP = a.CODDEP and d.CODMUN = a.CODMUN and d.CODDEP = a.CODDEP and e.CODCATEGORIA = a.CODCATEGORIA " +
                     "and CODREGENTE = " + Request.QueryString["CodRegente"] + " and CORR = " + Request.QueryString["Corr"] + "";
            cn.Open();
            CmTransaccion.CommandText = StrSql;
            CmTransaccion.Connection = cn;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                LblRegForestal.Text = reader["CODREG"].ToString();
                LblRegEmpf.Text = reader["CODREGEMPF"].ToString();
                LblRegEcut.Text = reader["CODREGECUT"].ToString();
                LblApellidos.Text = reader["APELLIDOS"].ToString();
                LblNombre.Text = reader["NOMBRES"].ToString();
                LblCategoria.Text = reader["CATEGORIA"].ToString();
                LblDpi.Text = reader["CODID"].ToString();
                lblNit.Text = reader["NIT"].ToString();
                LblNacionalidad.Text = reader["NACIONALIDAD"].ToString();
                LblGenero.Text = reader["GENERO"].ToString();
                LblFecNAc.Text = reader["FecNac"].ToString();
                LblCorreo.Text = reader["CORREO"].ToString();
                LblDireccion.Text = reader["DIRECCION"].ToString();
                LblDepartamento.Text = reader["DEPARTAMENTO"].ToString();
                LblMunicipio.Text = reader["MUNICIPIO"].ToString();
                LblTel.Text = reader["TELEFONO"].ToString();
                LblFax.Text = reader["FAX"].ToString();
                LblPrfesion.Text = reader["PROFESION"].ToString();
                LblNoCol.Text = reader["NOCOL"].ToString();
                LblEspecializacion.Text = reader["ESPECIALIZACION"].ToString();
                LblExp.Text = reader["expe"].ToString();
                LblCuales.Text = reader["CUAL"].ToString();
            }
            cn.Close();
        }
    }
}