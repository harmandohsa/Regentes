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
    public partial class RepIngresoExp : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptIngresoExp curriculu = new RptIngresoExp();

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            curriculu.Close();
            curriculu.Dispose();
            GC.Collect();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DataRow row;
            this.Util = new CUtilitarios();
            string CodRegente = Request.QueryString["CodRegente"].ToString();
            string Nus = Request.QueryString["nus"].ToString();
            string Corr = Request.QueryString["Corr"].ToString();
            string CodEstatus = Util.ObtieneRegistro("Select * from trecibesecre where codregente = " + CodRegente + " and corr = " + Corr + " and nus = " + Nus  + "", "CodEstatus").ToString();

            StrSql = "select fecrecibe, isnull(a.nombres,'') + ' ' + isnull(a.apellidos,'') as regente,region,codid,tramite as TipSol,g.nus,Solicitud,docid,titulo,colegiado,curri, " +
                     "pago,b.nit,isnull(e.nombres,'') + ' ' + isnull(e.apellidos,'') as secretaria,idelec, d.nombre as regiondep, d.dep as depar,codcategoria,tipousuario,g.codtramite,g.codtipomod,enfunciones,b.regente as reg,elaborador,CodRegEmpf,NoTramite,year(fecrecibe) as anis " +
                     "from tregente a, trecibesecre b, trelregiondep c, tregion d, tusuario e, ttipousuario f, tsolicitud g, ttipotramite h  " +
                     "where a.codregente = b.codregente and c.coddep = a.coddep and d.codregion = c.codregion and e.codusuario = b.codusuario and f.codtipousuario = e.codtipousuario  " +
                     "and g.codregente = a.codregente and h.codtramite = g.codtramite " +
                     "and a.codregente = " + CodRegente + " and corr = " + Corr + " AND G.NUS = B.NUS AND G.NUS = " + Nus + "";

            DsReport reportes = new DsReport();
            reportes.Tables["DtRecepcion"].Clear();
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                row = reportes.Tables["DtRecepcion"].NewRow();
                row["Fecha"] = reader["fecrecibe"];
                row["Oficina"] = reader["region"];
                row["Regente"] = reader["regente"];
                row["DPI"] = reader["codid"];
                row["TipoSolicitud"] = reader["TipSol"];
                row["Solicitud"] = reader["Solicitud"];
                row["DocId"] = reader["docid"];
                row["Titulo"] = reader["titulo"];
                row["Colegiado"] = reader["colegiado"];
                row["Curri"] = reader["curri"];
                row["Pago"] = reader["pago"];
                row["NIT"] = reader["nit"];
                row["IdElec"] = reader["idelec"];
                row["Secretaria"] = reader["secretaria"];
                row["Region"] = reader["regiondep"];
                row["Departamento"] = reader["depar"];
                row["CodCategoria"] = reader["codcategoria"];
                //string nus = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and codtramite =  " + Session["CodTramite"] + "", "nus").ToString();
                row["NUS"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                if (reader["enfunciones"].ToString() == "0")
                    row["puesto"] = reader["tipousuario"];
                else
                    row["puesto"] = reader["tipousuario"] + " en funciones";
                if (CodEstatus == "0")
                {
                    row["Cumple"] = "No Cumple";
                    row["Ingreso"] = "no ha sido ingresada";
                    row["SigPaso"] = " deberá ser completada en un plazo no mayor a quince (15) días hábiles después de recibido este oficio, de lo contrario con base a la documentación presentada se resolverá no procedente su solicitud.";
                }
                else
                {
                    row["Cumple"] = "Cumple";
                    row["Ingreso"] = "ha sido ingresada";
                    row["SigPaso"] = "continuará el trámite para resolver la solicitud";
                }
                row["CodTramite"] = reader["CodTramite"];
                row["CodTipoAct"] = reader["codcategoria"];
                row["rfbool"] = reader["reg"];
                row["elabbool"] = reader["elaborador"];
                row["noelab"] = reader["codregempf"];
                row["notramite"] = reader["NoTramite"] + "-" + reader["anis"];
                reportes.Tables["DtRecepcion"].Rows.Add(row);
            }

            string Tramite = Util.ObtieneRegistro("Select * from tsolicitud where CodRegente = " + CodRegente + " and nus = " + Nus + "", "CodTramite").ToString();

            
            curriculu.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            curriculu.SetDataSource((System.Data.DataSet)reportes);
            string str2 = Guid.NewGuid().ToString() + ".pdf";
            string url = base.Server.MapPath(".") + @"\" + this.DirRep + str2;
            DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
            {
                DiskFileName = url
            };
            ExportOptions exportOptions = curriculu.ExportOptions;
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportDestinationOptions = options2;
            curriculu.Export();
            url = this.DirApp + this.DirRep + str2;
            base.Response.Redirect(url);
        }
    }
}