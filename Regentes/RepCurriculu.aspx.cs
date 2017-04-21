using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportAppServer.DataDefModel;
using System.Data;

namespace Regentes
{
    public partial class RepCurriculu : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptCurriculu curriculu = new RptCurriculu();

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            curriculu.Close();
            curriculu.Dispose();
            GC.Collect();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            DataRow row;
            this.StrSql = "Select Nombres,Apellidos,titmedio,anismedio,insmedio,tittecnico,anistecnico,instecnico,titpro,anispro,inspro,titpos,anispos,inspos,titotro,anisotro,insotro,fecpresentacion,observacion from tregente a, TESTUDIO b, tenviosol c where a.codregente = b.codregente and c.codregente = a.codregente and a.codregente = " + base.Request.QueryString["CodRegente"];
            DsReport reportes = new DsReport();
            
            reportes.Tables["DtCurriEnca"].Clear();
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                row = reportes.Tables["DtCurriEnca"].NewRow();
                row["Nombres"] = reader["Nombres"];
                row["Apellidos"] = reader["Apellidos"];
                if (reader["titmedio"] == "")
                    row["TitMedio"] = "-----------------";
                else
                    row["TitMedio"] = reader["titmedio"];
                if (reader["anismedio"] != "")
                    row["AnisMedio"] = reader["anismedio"];
                else
                    row["AnisMedio"] = "-------";
                if (reader["anismedio"] != "")
                    row["insmedio"] = reader["insmedio"];
                else
                    row["InsMedio"] = "-----------------";
                if (reader["tittecnico"] != "")
                    row["TitTec"] = reader["tittecnico"];
                else
                    row["TitTec"] = "-----------------";
                if (reader["anistecnico"] != "")
                    row["AnisTec"] = reader["anistecnico"];
                else
                    row["AnisTec"] = "-------";
                if (reader["instecnico"] != "")
                    row["InstTec"] = reader["instecnico"];
                else
                    row["InstTec"] = "-----------------";
                if (reader["titpro"] != "")
                    row["TitPro"] = reader["titpro"];
                else
                    row["TitPro"] = "-----------------";
                if (reader["titpro"] != "")
                    row["AnisPro"] = reader["anispro"];        
                else
                    row["AnisPro"] = "-------";
                if (reader["inspro"] != "")
                    row["InsPro"] = reader["inspro"];
                else
                    row["InsPro"] = "-----------------";
                if (reader["titpos"] != "")
                    row["TitPos"] = reader["titpos"];
                else
                    row["TitPos"] = "-----------------";
                if (reader["anispos"] != "")
                    row["AnisPos"] = reader["anispos"];
                else
                    row["AnisPos"] = "-------";
                if (reader["inspos"] != "")
                    row["InsPos"] = reader["inspos"];
                else
                    row["InsPos"] = "-----------------";
                if (reader["titotro"] != "")
                    row["TitOtro"] = reader["titotro"];
                else
                    row["TitOtro"] = "-----------------";
                if (reader["anisotro"] != "")
                    row["AnisOtro"] = reader["anisotro"];
                else
                    row["AnisOtro"] = "-------";
                if (reader["insotro"] != "")
                    row["InsOtro"] = reader["insotro"];
                else
                    row["InsOtro"] = "-----------------";
                row["FecPresentacion"] = reader["FecPresentacion"];
                row["Observacion"] = reader["Observacion"];
                row["Dep"] = Util.ObtieneRegistro("Select departamento from tregente a, tdepartamento b where a.coddep = b.coddep and codregente = " + Request.QueryString["CodRegente"] + "", "departamento").ToString();
                row["Mun"] = Util.ObtieneRegistro("Select municipio from tregente a, tdepartamento b, tmunicipio c where a.coddep = b.coddep and c.codmun = a.codmun and b.coddep = c.coddep and codregente = " + Request.QueryString["CodRegente"] + "", "municipio").ToString();
                reportes.Tables["DtCurriEnca"].Rows.Add(row);
            }
            reader.Close();
            this.cn.Close();
            reportes.Tables["DtCurso"].Clear();
            this.StrSql = "select enfoque,institucion,duracion from tcurso where codregente = " + base.Request.QueryString["CodRegente"];
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader2 = this.cmTransaccion.ExecuteReader();
            while (reader2.Read())
            {
                row = reportes.Tables["DtCurso"].NewRow();
                row["Enfoque"] = reader2["enfoque"];
                row["Institucion"] = reader2["institucion"];
                row["Duracion"] = reader2["duracion"];
                reportes.Tables["DtCurso"].Rows.Add(row);
            }
            reader.Close();
            this.cn.Close();
            reportes.Tables["DtExperiencia"].Clear();
            this.StrSql = "select * from texperiencia where codregente = " + base.Request.QueryString["CodRegente"];
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader3 = this.cmTransaccion.ExecuteReader();
            while (reader3.Read())
            {
                row = reportes.Tables["DtExperiencia"].NewRow();
                row["Actividad"] = reader3["Actividad"];
                row["Entidad"] = reader3["Entidad"];
                row["Ubicacion"] = reader3["Ubicacion"];
                row["Observaciones"] = reader3["Observaciones"];
                reportes.Tables["DtExperiencia"].Rows.Add(row);
            }
            reader3.Close();
            this.cn.Close();
            reportes.Tables["DtTrabajo"].Clear();
            this.StrSql = "select * from ttrabajo where codregente = " + base.Request.QueryString["CodRegente"];
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader4 = this.cmTransaccion.ExecuteReader();
            while (reader4.Read())
            {
                row = reportes.Tables["DtTrabajo"].NewRow();
                row["Institucion"] = reader4["Institucion"];
                row["Puesto"] = reader4["Puesto"];
                reportes.Tables["DtTrabajo"].Rows.Add(row);
            }
            reader4.Close();
            this.cn.Close();
            reportes.Tables["DtExperienciaReg"].Clear();
            this.StrSql = "select * from texperienciareg where codregente = " + base.Request.QueryString["CodRegente"];
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader5 = this.cmTransaccion.ExecuteReader();
            while (reader5.Read())
            {
                row = reportes.Tables["DtExperienciaReg"].NewRow();
                row["Finca"] = reader5["Finca"];
                row["Ubicacion"] = reader5["Ubicacion"];
                row["TipoActividad"] = reader5["TipoActividad"];
                row["Area"] = reader5["Area"];
                row["Periodo"] = reader5["Periodo"];
                reportes.Tables["DtExperienciaReg"].Rows.Add(row);
            }
            reader5.Close();
            this.cn.Close();
            
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