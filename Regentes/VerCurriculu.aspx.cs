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
    public partial class VerCurriculu : System.Web.UI.Page
    {
        private string StrSql;
        CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdCurso.NeedDataSource += new GridNeedDataSourceEventHandler(GrdCurso_NeedDataSource);
            GrdExperiencia.NeedDataSource += new GridNeedDataSourceEventHandler(GrdExperiencia_NeedDataSource);
            GrdExperienciaFores.NeedDataSource += new GridNeedDataSourceEventHandler(GrdExperienciaFores_NeedDataSource);
            if (!base.IsPostBack)
            {
                RadMenu menu = (RadMenu)base.Master.FindControl("Menu");
                menu.Visible = false;
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Visible = false;
                ImageButton button = (ImageButton)base.Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
                LblNombre.Text = Util.NomRegente(Convert.ToInt32(Request.QueryString["CodRegente"]));
                CargaEstudios(Request.QueryString["CodRegente"].ToString());
                CargaTrabajoActual(Request.QueryString["CodRegente"].ToString());
            }
        }

        void GrdExperienciaFores_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CodRegente,Corr,Finca,ubicacion,tipoactividad,area,periodo from texperienciareg where codregente = " + Request.QueryString["CodRegente"];
            Util.LlenaGrid(StrSql, GrdExperienciaFores);
        }

        void GrdExperiencia_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CodRegente,Corr,Actividad,Entidad,Ubicacion,Observaciones from texperiencia where codregente = " + Request.QueryString["CodRegente"];
            Util.LlenaGrid(StrSql, GrdExperiencia);
        }

        void GrdCurso_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            StrSql = "Select CodRegente,Corr,Enfoque,Institucion,Duracion from tcurso where codregente = " + Request.QueryString["CodRegente"];
            Util.LlenaGrid(StrSql, GrdCurso);
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
    }
}