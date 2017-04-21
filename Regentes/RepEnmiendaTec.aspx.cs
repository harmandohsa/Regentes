using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Regentes.ServiceReference1;
using CrystalDecisions.Web;
using System.Data;
using CrystalDecisions.Shared;


namespace Regentes
{
    public partial class RepEnmiendaTec : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private IConsultaRegistro ConsultaRegistro = new ConsultaRegistroClient();
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptEmiendaTec tec = new RptEmiendaTec();

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            tec.Close();
            tec.Dispose();
            GC.Collect();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Util = new CUtilitarios();
            DataRow row;
            StrSql = "Select a.nus,a.feccre,enmienda,(Select e.nombre from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as Region,(Select d.codregion from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as CodRegion,(Select e.region from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as LetraRegion, (Select dep from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as Departamento, isnull(c.nombres,'') + ' ' + isnull(c.apellidos,'') as regente, " +
                     "codid,isnull(f.nombres,'') + ' ' + isnull(f.apellidos,'') as tecnico,a.no as noenmienda,a.idelec,enfunciones " +
                     "from tenmiendatec a, tdetdictamen b, tregente c, tusuario f " +
                     "where a.codregente = b.codregente and a.corr = b.corr and A.nus = B.nus  and  a.codregente  = c.codregente and f.codusuario = a.codusuario " +
                     "and a.codregente = " + Request.QueryString["CodRegente"] + " and a.corr = " + Request.QueryString["Corr"] + " and a.nus = " + Request.QueryString["nus"] +  "";
            DsReport reportes = new DsReport();
            reportes.Tables["DtEnmiendaTec"].Clear();
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            string str = "";
            int i = 1;
            while (reader.Read())
            {
                
                row = reportes.Tables["DtEnmiendaTec"].NewRow();
                row["Fecha"] = reader["feccre"];
                row["Oficina"] = reader["region"];
                row["Regente"] = reader["regente"];
                row["DPI"] = reader["codid"];
                row["Tecnico"] = reader["tecnico"];
                row["Director"] = Util.ObtieneRegistro("select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Director from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 3 and CodEstatus = 1", "Director").ToString();
                row["Enmienda"] = reader["enmienda"];
                row["NoExpediente"] = reader["nus"];
                row["Solicitud"] = "Inscripcion";
                row["Dep"] = reader["departamento"];
                row["CodRegion"] = reader["CodRegion"];
                row["NoEn"] =  reader["LetraRegion"]+ "-RF-" +  reader["noenmienda"] + "-" + Convert.ToDateTime(reader["feccre"]).Year;
                row["IdElec"] = reader["idelec"];
                if (reader["enfunciones"].ToString() == "0")
                    row["enfunciones"] = "Asesor forestal";
                else
                    row["enfunciones"] = "Asesor forestal en funciones";
                row["enfuncionesdir"] = Util.ObtieneRegistro("Select * from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 3 and CodEstatus = 1", "enfunciones").ToString();
                row["no"] = i.ToString() + ".";
                i = i + 1;
                reportes.Tables["DtEnmiendaTec"].Rows.Add(row);
            }
            reader.Close();
            this.cn.Close();
            

            
            
            tec.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            tec.SetDataSource((DataSet)reportes);
            
            string str3 = Guid.NewGuid().ToString() + ".pdf";
            string url = base.Server.MapPath(".") + @"\" + this.DirRep + str3;
            DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
            {
                DiskFileName = url
            };
            ExportOptions exportOptions = tec.ExportOptions;
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportDestinationOptions = options2;
            tec.Export();
            url = this.DirApp + this.DirRep + str3;
            base.Response.Redirect(url);

        }
    }
}