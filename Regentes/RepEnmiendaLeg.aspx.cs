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
    public partial class RepEnmiendaLeg : System.Web.UI.Page
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
        RptEnmiendaLeg tec = new RptEnmiendaLeg();

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
            StrSql = "Select a.nus,a.feccre,enmienda,(Select e.nombre from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as Region,(Select d.codregion from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as CodRegion,(Select e.region from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as LetraRegion,(Select Dep from trelregiondep d, tregion e where d.codregion = e.codregion and coddep = c.coddep) as Dep, isnull(c.nombres,'') + ' ' + isnull(c.apellidos,'') as regente, " +
                     "codid,isnull(f.nombres,'') + ' ' + isnull(f.apellidos,'') as tecnico,a.NO as noenmienda,a.idelec,enfunciones " +
                     "from TENMIENDALEG a, TDETENMIENDALEG b, tregente c, tusuario f " +
                     "where a.codregente = b.codregente and a.nus = b.nus and a.corr = b.corr and a.codregente  = c.codregente and f.codusuario = a.codusuario " +
                     "and a.codregente = " + Request.QueryString["CodRegente"] + " and a.corr = " + Request.QueryString["Corr"] + " and a.nus = " + Request.QueryString["nus"] + "";
            DsReport reportes = new DsReport();
            reportes.Tables["DtEnmiendaLeg"].Clear();
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            string str = "";
            int numero = 1;
            while (reader.Read())
            {
                row = reportes.Tables["DtEnmiendaLeg"].NewRow();
                row["FechaEn"] = reader["feccre"];
                row["Oficina"] = reader["region"];
                row["Regente"] = reader["regente"];
                row["DPI"] = reader["codid"];
                row["Juridico"] = reader["tecnico"];
                if (Util.ExisteDato("Select * from tusuario where CodTipoUsuario = 5 and CodRegion = " + reader["codregion"] + " and codestatus = 1") == true)
                {
                    row["Director"] = Util.ObtieneRegistro("select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Director from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 5 and CodEstatus = 1", "Director").ToString();
                    row["enfuncionesdir"] = Util.ObtieneRegistro("select * from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 5 and CodEstatus = 1", "enfunciones").ToString();
                }
                else
                {
                    row["Director"] = Util.ObtieneRegistro("select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Director from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 3 and CodEstatus = 1", "Director").ToString();
                    row["enfuncionesdir"] = Util.ObtieneRegistro("select * from tusuario where CodRegion = " + reader["codregion"] + " and CodTipoUsuario = 3 and CodEstatus = 1", "enfunciones").ToString();
                }
                row["Enminda"] = reader["enmienda"];
                row["NoExpediente"] = reader["nus"];
                row["Solicitud"] = "Inscripcion";
                row["Dep"] = reader["Dep"];
                row["CodRegion"] = reader["CodRegion"];
                //row["NoEn"] = reader["noenmienda"];
                row["IdElec"] = reader["idelec"];
                row["NoEn"] = reader["LetraRegion"] + "-RF-" + reader["noenmienda"] + "-" + Convert.ToDateTime(reader["feccre"]).Year;
                if (reader["enfunciones"].ToString() == "0")
                    row["enfunciones"] = "Asesor Juridico";
                else
                    row["enfunciones"] = "Asesor Juridico en funciones";
                row["no"] = numero.ToString() + ".";
                numero = numero + 1;
                reportes.Tables["DtEnmiendaLeg"].Rows.Add(row);
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