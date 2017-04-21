using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using CrystalDecisions.Web;
using System.Data;
using CrystalDecisions.Shared;

namespace Regentes
{
    public partial class RepResolucion : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptResolucion Jur = new RptResolucion();

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            Jur.Close();
            Jur.Dispose();
            GC.Collect();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            DataRow row;

            StrSql = "Select referncia,noresolucion,year(a.feccre) as anis,a.feccre,isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as regente,codid,'Inscripcion' as TipoSol, "+
                     "ConsiderandoUno,ConsiderandoDos,ConsiderandoTres,Portanto,Resuelve,Categoria,nombre,d.codregion,dep,d.region,a.nus,enfunciones,nocontrol,a.codusuario as codjuridico " +
                     "from tresolucion a, tregente b, tdictamentec c, tregion d, tusuario e " +
                     "where a.codregente = b.codregente and c.codregente = a.codregente and c.nus = a.nus and d.codregion = c.codregion and e.codusuario = a.codusuario " +
                     "and a.codregente =  " + base.Request.QueryString["CodRegente"] + "  and a.nus = " + Request.QueryString["nus"] + "";

            DsReport reportes = new DsReport();
            reportes.Tables["DtResolucion"].Clear();

            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                row = reportes.Tables["DtResolucion"].NewRow();
                row["Referencia"] = reader["referncia"];
                row["NoResolucion"] =  reader["region"] + "-RF-" + reader["noresolucion"] + "-" + reader["anis"];
                row["FecCre"] = reader["feccre"];
                row["Regente"] = reader["regente"];
                row["DPI"] = reader["codid"];
                string TipoSol = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();
                if (TipoSol == "1")
                    row["TipoSol"] = "inscripción";
                else if (TipoSol == "3")
                    row["TipoSol"] = "cambio de categoría";
                else if (TipoSol == "4")
                    row["TipoSol"] = "renovación";
                row["Categoria"] = reader["Categoria"];
                row["ConsiderandoUno"] = reader["ConsiderandoUno"];
                row["ConsiderandoDos"] = reader["ConsiderandoDos"];
                row["ConsiderandoTres"] = reader["ConsiderandoTres"];
                row["PorTanto"] = reader["Portanto"];
                row["Resuelve"] = reader["Resuelve"];
                string CodRegion = Util.CodRegion(Convert.ToInt32(Request.QueryString["CodRegente"]));
                
                //row["DirectorFores"] = Util.ObtieneRegistro("select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Director from tusuario where codtipousuario = 3 and codregion = " + CodRegion + " and codestatus = 1", "Director").ToString();
                if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and CodRegion = " + CodRegion + " and codestatus = 1") == true)
                {
                    //if (Util.ObtieneRegistro("Select * from tnombre where CodRegion = " + CodRegion + "", "nombre").ToString() == "")
                    //{
                    //    row["Director"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "Director").ToString();
                    //    row["PuestoDir"] = "NoCambio";
                    //}
                    //else
                    //{
                    //    row["Director"] = Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + CodRegion + "", "Nombre").ToString();
                    //    row["PuestoDir"] = "Cambio";
                    //}
                    row["PuestoDir"] = "NoCambio";
                    row["DirectorFores"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "Director").ToString();
                    row["enfunciones"] = Util.ObtieneRegistro("select * from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "enfunciones").ToString();
                    row["IdElec"] = Util.ObtieneRegistro("select * from tvoboleg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "IDELECRES").ToString();
                }
                else
                {
                    row["DirectorFores"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "Director").ToString();
                    row["enfunciones"] = Util.ObtieneRegistro("select * from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "enfunciones").ToString();
                    row["PuestoDir"] = "NoCambio";
                    //if (Util.ObtieneRegistro("Select * from tnombre where CodRegion = " + CodRegion + "", "nombre").ToString() == "")
                    //{
                    //    row["Director"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "", "Director").ToString();
                    //    row["PuestoDir"] = "NoCambio";
                    //}
                    //else
                    //{
                    //    row["Director"] = Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + CodRegion + "", "Nombre").ToString();
                    //    row["PuestoDir"] = "Cambio";
                    //}
                    row["IdElec"] = Util.ObtieneRegistro("select * from tvoboleg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "IdElec").ToString();
                }
                if (Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + CodRegion + "", "Nombre").ToString() != "")
                {
                    row["DirectorFores"] = Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + CodRegion + "", "Nombre").ToString();
                    row["PuestoDir"] = "Director Regional";
                }
                else

                    row["NoProv"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from tprovidencialeg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Corr").ToString() + "-" + reader["anis"];
                row["Region"] = Util.ObtieneRegion(Request.QueryString["CodRegente"].ToString());
                row["FecProv"] = Util.ObtieneRegistro("Select * from tprovidencialeg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "FecCre").ToString();
                row["Nus"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                row["Folio"] = Util.ObtieneRegistro("Select * from tresolucion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "nofolio").ToString();
                row["Juridico"] = Util.NomUsuario(Convert.ToInt32(reader["codjuridico"]));
                row["NoDicLeg"] = Util.ObtieneRegistro("Select * from tdictamenleg where Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "NoDictamen").ToString();
                row["AnisDicLeg"] = Util.ObtieneRegistro("Select year(feccre) as anis from tdictamenleg where Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "anis").ToString();
                row["Dep"] = reader["Dep"];
                row["CodRegion"] = reader["CodRegion"];
                if (reader["enfunciones"].ToString() == "1")
                    row["Puesto"] = Util.ObtieneRegistro("Select * from tdictamenleg  where Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "puesto").ToString() + " en funciones";
                else
                    row["Puesto"] = Util.ObtieneRegistro("Select * from tdictamenleg  where Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "puesto").ToString();
                row["ControlInterno"] = reader["nocontrol"];
                reportes.Tables["DtResolucion"].Rows.Add(row);
            }

            reader.Close();
            cn.Close();


            reportes.Tables["DtConsiderando"].Clear();
            StrSql = "Select * from tdetconsiderando where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
            cn.Open();
            cmTransaccion.CommandText = this.StrSql;
            cmTransaccion.Connection = this.cn;
            cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader2 = this.cmTransaccion.ExecuteReader();
            while (reader2.Read())
            {
                row = reportes.Tables["DtConsiderando"].NewRow();
                row["Considerando"] = reader2["Considerando"];
                reportes.Tables["DtConsiderando"].Rows.Add(row);
            }
            reader.Close();
            cn.Close();

            reportes.Tables["DtPorTanto"].Clear();
            StrSql = "Select * from tresolucion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
            cn.Open();
            cmTransaccion.CommandText = this.StrSql;
            cmTransaccion.Connection = this.cn;
            cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader3 = this.cmTransaccion.ExecuteReader();
            while (reader3.Read())
            {
                row = reportes.Tables["DtPorTanto"].NewRow();
                row["PorTanto"] = reader3["PorTanto"];
                reportes.Tables["DtPorTanto"].Rows.Add(row);
            }
            reader3.Close();
            cn.Close();

           
           
            Jur.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            Jur.SetDataSource((DataSet)reportes);
            string str3 = Guid.NewGuid().ToString() + ".pdf";
            string url = base.Server.MapPath(".") + @"\" + this.DirRep + str3;
            DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
            {
                DiskFileName = url
            };
            ExportOptions exportOptions = Jur.ExportOptions;
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportDestinationOptions = options2;
            Jur.Export();
            url = this.DirApp + this.DirRep + str3;
            base.Response.Redirect(url);
        }
    }
}