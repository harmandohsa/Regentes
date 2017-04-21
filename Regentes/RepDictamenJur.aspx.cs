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
    public partial class RepDictamenJur : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptDictamenJur Jur = new RptDictamenJur();

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
            string enfuncionesdir = "";
            StrSql = "select a.referencia,a.nodictamen,year(a.feccre) as anis, isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as regente,codid,'Inscripción' as TipoSol, " +
                     "Categoria,a.idelec,a.feccre,isnull(d.nombres,'') + ' ' + isnull(d.apellidos,'') as Juridico, e.nombre as region, dep, e.codregion, e.region as letra,a.puesto as pue,enfunciones,nocontrol  " +
                     "from tdictamenleg a, tregente b, tdictamentec c, tusuario d, tregion e " +
                     "where a.codregente = b.codregente and a.nus = c.nus  and  c.codregente = a.codregente and d.codusuario = a.codusuario and e.codregion = c.codregion " +
                     "and a.codregente = " + base.Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "";

            DsReport reportes = new DsReport();
            reportes.Tables["DtDictamenJur"].Clear();
           
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                row = reportes.Tables["DtDictamenJur"].NewRow();
                row["Referencia"] = reader["referencia"];
                row["NoDictamen"] = reader["letra"] + "-RF-" + reader["nodictamen"] + "-" + reader["anis"];
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
                row["IdElec"] = reader["idelec"];
                row["FecCre"] = reader["feccre"];
                row["Juridico"] = reader["Juridico"];
                string CodRegion = Util.CodRegion(Convert.ToInt32(Request.QueryString["CodRegente"]));
                //if (Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + CodRegion + "", "Nombre").ToString() == "")
                //    row["Director"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + "", "Director").ToString();
                //else
                //    row["Director"] = "";


                row["Director"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + "", "Director").ToString();
                enfuncionesdir = Util.ObtieneRegistro("select * from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + "", "enfunciones").ToString();
                //row["Director"] = Util.ObtieneRegistro("select isnull(b.nombres,'') + ' ' + isnull(b.apellidos,'') as Director from tvoboleg a, tusuario b where a.codusuario = b.codusuario and a.codregente = " + Request.QueryString["CodRegente"] + "", "Director").ToString();
                row["IdElecDirector"] = Util.ObtieneRegistro("select * from tvoboleg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "IdElec").ToString();

                if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and CodRegion = " + CodRegion + " and codestatus = 1") == true)
                    if (enfuncionesdir == "0" || enfuncionesdir == "")
                        row["Cargo"] = "Director Departamento Jurídico";
                    else
                        row["Cargo"] = "Director Departamento Jurídico en funciones";
                else
                    if (enfuncionesdir == "0" || enfuncionesdir == "")
                        row["Cargo"] = "Director Regional";
                    else
                        row["Cargo"] = "Director Regional en funciones";
                row["Region"] = reader["region"];
                row["Dep"] = reader["dep"];
                row["CodRegion"] = reader["CodRegion"];
                if (reader["enfunciones"].ToString() == "0")
                    row["Puesto"] = reader["pue"];
                else
                    row["Puesto"] = reader["pue"] + " en funciones";
                row["ControlInterno"] = reader["nocontrol"];
                reportes.Tables["DtDictamenJur"].Rows.Add(row);
            }

            reader.Close();
            cn.Close();

           //Antecedentes
            int Antecedente = 1;
            string FecCre = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FecTramite,3) as FecCreacion from TSOLICITUD where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + "", "FecCreacion").ToString();
            string Dato = "Solicitud enviada a través del sistema el  " + FecCre;
            reportes.Tables["DtAntecedenteJur"].Clear();
            row = reportes.Tables["DtAntecedenteJur"].NewRow();
            row["Antecedente"] = Dato;
            row["no"] = "1." + Antecedente + ".";
            Antecedente = Antecedente + 1;
            reportes.Tables["DtAntecedenteJur"].Rows.Add(row);

            StrSql = "select CONVERT(CHAR(11),fecrecibe,3) as fecrecibe,codestatus from trecibesecre where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + " order by fecrecibe ";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader2 = this.cmTransaccion.ExecuteReader();
            while (reader2.Read())
            {
                row = reportes.Tables["DtAntecedenteJur"].NewRow();
                if (reader2["codestatus"].ToString() == "1")
                {
                    row["Antecedente"] = "Ingreso de solicitud en Ventanilla Única del CONAP con fecha " + reader2["FecRecibe"];
                    row["no"] = "1." + Antecedente + ".";
                    Antecedente = Antecedente + 1;
                }
                else
                {
                    row["Antecedente"] = "Solicitud no ingresada por incumplimiento de requisitos con fecha, " + reader2["FecRecibe"];
                    row["no"] = "1." + Antecedente + ".";
                    Antecedente = Antecedente + 1;
                }
                
                reportes.Tables["DtAntecedenteJur"].Rows.Add(row);
            }
            reader2.Close();
            cn.Close();

            StrSql = "select CONVERT(CHAR(11),FecCre,3) as FecCre,CONVERT(CHAR(11),FECCIERRE,3) as FecCierre from tenmiendatec where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + " order by feccre ";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader3 = this.cmTransaccion.ExecuteReader();
            while (reader3.Read())
            {
                row = reportes.Tables["DtAntecedenteJur"].NewRow();
                row["Antecedente"] = "Enmienda técnica solicitada con fecha " + reader3["FecCre"];
                row["no"] = "1." + Antecedente + ".";
                Antecedente = Antecedente + 1;
                reportes.Tables["DtAntecedenteJur"].Rows.Add(row);
                row = reportes.Tables["DtAntecedenteJur"].NewRow();
                row["Antecedente"] = "Recepción de enmiendas técnicas solicitadas con fecha  " + reader3["FecCre"];
                row["no"] = "1." + Antecedente + ".";
                Antecedente = Antecedente + 1;
                reportes.Tables["DtAntecedenteJur"].Rows.Add(row);
            }
            reader3.Close();
            cn.Close();

            StrSql = "select CONVERT(CHAR(11),FecCre,3) as FecCre,CONVERT(CHAR(11),FECCIERRE,3) as FecCierre from tenmiendaleg where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + " order by feccre ";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader4 = this.cmTransaccion.ExecuteReader();
            while (reader4.Read())
            {
                row = reportes.Tables["DtAntecedenteJur"].NewRow();
                row["Antecedente"] = "Enmienda juridica solicitada con fecha " + reader4["FecCre"];
                reportes.Tables["DtAntecedenteJur"].Rows.Add(row);
                row["no"] = "1." + Antecedente + ".";
                Antecedente = Antecedente + 1;
                row = reportes.Tables["DtAntecedenteJur"].NewRow();
                row["Antecedente"] = "Recepción de enmiendas juridicas solicitadas con fecha  " + reader4["FecCre"];
                row["no"] = "1." + Antecedente + ".";
                Antecedente = Antecedente + 1;
                reportes.Tables["DtAntecedenteJur"].Rows.Add(row);
            }
            reader4.Close();
            cn.Close();

            string FecDictamen = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FecCre,3) as FecCre from tdictamentec where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + "", "FecCre").ToString();
            string DatoDic = "Dictamen Técnico realizado el " + FecDictamen;
            row = reportes.Tables["DtAntecedenteJur"].NewRow();
            row["Antecedente"] = DatoDic;
            row["no"] = "1." + Antecedente + ".";
            Antecedente = Antecedente + 1;
            reportes.Tables["DtAntecedenteJur"].Rows.Add(row);

            string FecVoboTec = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FecVobo,3) as FecVobo from tvobotec where codregente = " + Request.QueryString["CodRegente"] + "  and nus = " + Request.QueryString["nus"] + "", "FecVobo").ToString();
            string DatoVobo = "Dictamen Técnico con Visto Bueno el " + FecVoboTec;
            row = reportes.Tables["DtAntecedenteJur"].NewRow();
            row["Antecedente"] = DatoVobo;
            row["no"] = "1." + Antecedente + ".";
            Antecedente = Antecedente + 1;
            reportes.Tables["DtAntecedenteJur"].Rows.Add(row);

            //Fundamentos
            reportes.Tables["DtFundamento"].Clear();
            StrSql = "select * from tdetfundamento where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader5 = this.cmTransaccion.ExecuteReader();
            while (reader5.Read())
            {
                row = reportes.Tables["DtFundamento"].NewRow();
                row["Fundamento"] = reader5["Fundamento"];
                reportes.Tables["DtFundamento"].Rows.Add(row);
            }
            reader5.Close();
            cn.Close();

            reportes.Tables["DtAnalisis"].Clear();
            StrSql = "select * from tdetanalisleg where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader6 = this.cmTransaccion.ExecuteReader();
            while (reader6.Read())
            {
                row = reportes.Tables["DtAnalisis"].NewRow();
                row["Analisis"] = reader6["AnalisisLeg"];
                reportes.Tables["DtAnalisis"].Rows.Add(row);
            }
            reader6.Close();
            cn.Close();

            reportes.Tables["DtRecomendacion"].Clear();
            StrSql = "select * from trecomendacion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader7 = this.cmTransaccion.ExecuteReader();
            while (reader7.Read())
            {
                row = reportes.Tables["DtRecomendacion"].NewRow();
                row["Recomendacion"] = reader7["Recomendacion"];
                reportes.Tables["DtRecomendacion"].Rows.Add(row);
            }
            reader7.Close();
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