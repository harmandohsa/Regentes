using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace Regentes
{
    public partial class ConsultaRegente : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private CUtilitarios Util;
        private string StrSql = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            StrSql = "select region,a.codregente,CodReg,CodRegEmpf,CodRegEcut,c.Nombres,c.Apellidos,codid,profesion,especializacion,CONVERT(CHAR(11),fecaut,3) as fecaut, " +
                     "CONVERT(CHAR(11),fecven,3) as fecven,e.idelec,Categoria,isnull(f.nombres,'') + ' ' + isnull(f.apellidos,'') as Director, idunico,b.nombre " +
                     "from tdictamentec a, tregion b, tregente c, tperiodo d, tvoboleg e,tusuario f " +
                     "where a.codregion = b.codregion and c.codregente = a.codregente and d.codregente = a.codregente and d.codregente = c.codregente  " +
                     "and e.codregente = a.codregente and e.codregente = d.codregente and e.codregente = a.codregente and f.codusuario = e.codusuario " +
                     "and a.codregente =  " + base.Request.QueryString["CodRegente"];

            cn.Open();
            cmTransaccion.CommandText = StrSql;
            cmTransaccion.Connection = cn;
            cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                lblRegion.Text = "Delegación Regional de CONAP: " + reader["nombre"].ToString();
                LblRegConap.Text = "RRAP - " + Convert.ToInt32(reader["codregente"]).ToString("D8");
                LblRegInab.Text = reader["CodReg"].ToString();
                LblRegEpmf.Text = reader["CodRegEmpf"].ToString();
                LblRegEcut.Text = reader["CodRegEcut"].ToString();
                LblNombres.Text = reader["Nombres"].ToString();
                LblApellido.Text = reader["Apellidos"].ToString();
                LblDui.Text = reader["codid"].ToString();
                lblProfesion.Text = reader["profesion"].ToString();
                LblCategoria.Text = reader["Categoria"].ToString();
                LblEspe.Text = reader["especializacion"].ToString();
                lblFecIns.Text = reader["fecaut"].ToString();
                LblFecVen.Text = reader["fecven"].ToString();
                string path = base.Server.MapPath(".") + @"\FotosRegentes\\" + Request.QueryString["CodRegente"];
                if (Directory.Exists(path))
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] files = info.GetFiles("*.*");
                    DirectoryInfo[] directories = info.GetDirectories();
                    for (int i = 0; i < files.Length; i++)
                    {
                        //"~/Imagenes/logo.jpg"

                        string a = files[i].Name;
                        string Carpeta = Request.QueryString["CodRegente"];
                        ImgRegente.ImageUrl = "~/FotosRegentes/" + Carpeta + "/" + a;
                    }
                }
                else
                {
                }
            }
            cn.Close();
        }
    }
}