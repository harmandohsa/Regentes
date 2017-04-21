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
using System.Text;
using System.IO;
using System.Xml;

namespace Regentes
{
    public partial class RepDictamenTec : System.Web.UI.Page
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
        RptDictamenTec tec = new RptDictamenTec();
        RptDictamenTecAct tecAct = new RptDictamenTecAct();
        Registro.WebConap ConsultaRegistroINAB = new Registro.WebConap();

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            tec.Close();
            tec.Dispose();
            tecAct.Close();
            tecAct.Dispose();
            GC.Collect();
        }

        string DatosRegistro(string NoRegistro, int DatoSolicitado)
        {
            bool Resultado = false;
            string DevService = ConsultaRegistroINAB.ConsultarRegistroRegentes(NoRegistro);
            byte[] codificar = Encoding.UTF8.GetBytes(DevService);
            MemoryStream ms = new MemoryStream(codificar);
            ms.Flush();
            ms.Position = 0;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ms);

            XmlNodeList lista = xmldoc.GetElementsByTagName("Lista");
            XmlNodeList datos = ((XmlElement)lista[0]).GetElementsByTagName("Conap");
            foreach (XmlElement nodo in datos)
            {
                Resultado = true;
                string a = nodo.InnerText;
                int i = 0;
                XmlNodeList fecha = nodo.GetElementsByTagName("FechaVencimiento");
                XmlNodeList nombre = nodo.GetElementsByTagName("Nombres");
                XmlNodeList apellido = nodo.GetElementsByTagName("Apellidos");
                XmlNodeList fechaIns = nodo.GetElementsByTagName("FechaInscripcion");
                if (DatoSolicitado == 1)
                    return fechaIns[i].InnerText.ToString();
                else if (DatoSolicitado == 2)
                    return fecha[i].InnerText.ToString();
                else
                    return "";
            }
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            DataRow row;
            
            DsReport reportes = new DsReport();
            DsReport reportes2 = new DsReport();
            string Tramite = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();
            if (Tramite == "1" || Tramite == "4")
            {
                this.StrSql = "select b.nombre,idelec,referencia,region,a.feccre,nodictamen,isnull(c.nombres,'') + ' ' + isnull(c.apellidos,'') as Regente, codid,'Inscripción' as Proceso,departamento,reciboing,fecharecibo,bolbanrural,constancia,nocons,profesion, nocol,codreg,codregempf,recomendacion,categoria,vigencia,f.corr,f.feccre as fecprov,isnull(g.nombres,'') + ' ' + isnull(g.apellidos,'') as Director,isnull(i.nombres,'') + ' ' + isnull(i.apellidos,'') as tecnico,a.nus,folios,a.codregion,dep,year(a.feccre) as anis,b.nombre as reg,puesto,i.enfunciones as enfuncionestec,g.enfunciones as enfuncionesdir " +
                          "from tdictamentec a, tregion b, tregente c, tdepartamento d, trecomendacin e, TPROVIDENCIA f, tusuario g, tusuario i where a.codregion = b.codregion and f.CODREGENTE = a.CODREGENTE and f.nus = a.nus and c.codregente = a.codregente and d.coddep = c.coddep and e.codrecomendacion = a.codrecomendacion and f.codregente = a.codregente and g.codusuario = f.codusuario  and i.codusuario = a.codusuario and a.codregente =  " + base.Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "";
                reportes.Tables["DtDictamenTec"].Clear();
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
                string str = "";
                while (reader.Read())
                {
                    //DatosRegistro
                    

                    row = reportes.Tables["DtDictamenTec"].NewRow();
                    row["Referencia"] = reader["referencia"];
                    row["Region"] = reader["region"];
                    row["Feccre"] = reader["feccre"];
                    row["NoDictamen"] = reader["nodictamen"];
                    //row["NoDictamen"] = reader["region"] + "-RF-" + reader["nodictamen"] + "-" + reader["anis"];
                    row["Regente"] = reader["Regente"];
                    row["Dui"] = reader["codid"];
                    if (Tramite == "1")
                        row["Proceso"] = reader["Proceso"];
                    else
                        row["Proceso"] = "renovación vigencia del ejercicio de regente forestal en áreas protegidas";
                    row["Departemento"] = reader["Departamento"];
                    row["ReciboIng"] = reader["reciboing"];
                    row["FechaRecibo"] = reader["fecharecibo"];
                    row["BolBanrural"] = reader["bolbanrural"];
                    row["Constancia"] = reader["constancia"];
                    row["NoConstancia"] = reader["nocons"];
                    row["Profesion"] = reader["profesion"];
                    row["NoCol"] = reader["nocol"];
                    row["CodReg"] = reader["codreg"];
                    row["FecAutReg"] = DatosRegistro(reader["codreg"].ToString(), 1);
                    row["FecVenReg"] = DatosRegistro(reader["codreg"].ToString(), 2);
                    row["CodRegEpmf"] = reader["codregempf"];
                    row["FecAutRegEpmf"] = DatosRegistro(reader["codregempf"].ToString(), 1);
                    row["FecVenRegEpmf"] = DatosRegistro(reader["codregempf"].ToString(), 2);
                    row["Recomendacion"] = reader["Recomendacion"].ToString().ToUpper();
                    row["Categoria"] = reader["Categoria"];
                    if (Tramite == "1")
                        row["Vigencia"] = reader["Vigencia"];
                    else
                        row["Vigencia"] = Util.ObtieneRegistro("Select * from trenovacion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Anis").ToString();
                    //row["NoProv"] = reader["Corr"];
                    row["NoProv"] = reader["region"] + "-RF-" + reader["Corr"] + "-" + reader["anis"];
                    row["FecProv"] = reader["FecProv"];
                    if (Util.ExisteDato("Select  * from TVOBOTEC where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "") == true)
                        row["Director"] = reader["Director"];
                    else
                        row["Director"] = "";

                    //if (reader["Codregion"].ToString() == "1")
                    //    row["Tecnico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 2 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                    //else if (reader["Codregion"].ToString() == "8")
                    //    row["Tecnico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 2 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                    //else
                     row["Tecnico"] = reader["Tecnico"];
                    //row["Tecnico"] = reader["Tecnico"];
                    row["Nus"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                    row["Folio"] = reader["Folios"];
                    row["IdElec"] = reader["IdElec"];
                    row["IdElecDirector"] = Util.ObtieneRegistro("Select * from tvobotec where codregente = " + base.Request.QueryString["CodRegente"] + "", "IdElec").ToString();
                    if (Tramite == "1")
                    {
                        if (Convert.ToInt32(reader["Vigencia"]) == 1)
                        {
                            row["Pago"] = Util.ObtieneRegistro("Select * from tcategoria where categoria = '" + reader["Categoria"] + "'", "Pago").ToString();
                        }
                        else
                        {
                            double Pago = Convert.ToDouble(Util.ObtieneRegistro("Select * from tcategoria where categoria = '" + reader["Categoria"] + "'", "Pago"));
                            double Adicional  = (Convert.ToInt32(row["Vigencia"]) -1) * 50;
                            row["Pago"] = (Pago + Adicional).ToString();
                        }
                        
                    }
                    else
                        row["Pago"] = Convert.ToInt32(row["Vigencia"]) * Convert.ToInt32(Util.ObtieneRegistro("Select * from tcategoria where codcategoria = 3", "Pago").ToString());
                    
                    if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and codregion = " + reader["codregion"] + " and codestatus = 1") == true)
                    {
                        row["Juridico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 5 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                        row["TipoJuridico"] = "Director(a) Juridico(a)";
                    }
                    else if (Util.ExisteDato("Select * from tusuario where codtipousuario = 4 and codregion = " + reader["codregion"] + " and codestatus = 1") == true)
                    {
                        row["Juridico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 4 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                        row["TipoJuridico"] = "Asesor(a) Jurídico(a)";
                    }
                    else
                    {
                        row["Juridico"] = "";
                        row["TipoJuridico"] = "";
                    }
                    row["nombre"] = reader["nombre"];
                    row["CodRegion"] = reader["codregion"];
                    row["Dep"] = reader["dep"];
                    row["Reg"] = reader["reg"];
                    if (reader["enfuncionestec"].ToString() == "0")
                        row["Puesto"] = reader["puesto"];
                    else
                        row["Puesto"] = reader["puesto"] + " en funciones";
                    if (Tramite == "4")
                    {
                        string nusincrip = Util.ObtieneRegistro("Select * from tsolicitud where codregente = "  + Request.QueryString["CodRegente"] + " and CodTramite = 1 ","nus").ToString();
                        row["FecRes"] = Util.ObtieneRegistro("Select * from tresolucion where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                        row["NoRes"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from tresolucion where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "noresolucion").ToString() + Convert.ToDateTime(row["FecRes"]).Year;
                        row["FecDicLeg"] = Util.ObtieneRegistro("Select * from TDICTAMENLEG where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                        row["NoDicLeg"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from TDICTAMENLEG where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "nodictamen").ToString() + Convert.ToDateTime(row["FecDicLeg"]).Year;
                        row["FecDicTec"] = Util.ObtieneRegistro("Select * from TDICTAMENTEC where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                        row["NoDicTec"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from TDICTAMENTEC where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "nodictamen").ToString() + "-" + Convert.ToDateTime(row["FecDicTec"]).Year;
                        row["NoIns"] = "RRAP - " +  Convert.ToInt32(nusincrip).ToString("D8");
                        row["NoRegente"] = Util.ObtieneRegistro("Select * from tregente where codregente = " + Request.QueryString["CodRegente"] + "", "Codigo").ToString();
                    }
                    row["enfunciones"] = reader["enfuncionesdir"];
                    reportes.Tables["DtDictamenTec"].Rows.Add(row);
                }
                reader.Close();
                this.cn.Close();
                reportes.Tables["DtDetDictamenTec"].Clear();
                this.StrSql = "Select * from tdetconclusion where codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader2 = this.cmTransaccion.ExecuteReader();
                int Conclusion = 1;
                while (reader2.Read())
                {
                    row = reportes.Tables["DtDetDictamenTec"].NewRow();
                    row["Conclusion"] = reader2["Conclusion"];
                    row["no"] = "4." + Conclusion + ".";
                    Conclusion = Conclusion + 1;
                    reportes.Tables["DtDetDictamenTec"].Rows.Add(row);
                }
                reader.Close();
                this.cn.Close();

                int NoAntecedentes = 1;
                //Antecedentes
                string FecCre = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FecCreacion,3) as FecCreacion from tregente a, tsolicitud b where a.codregente = b.codregente and a.codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "FecCreacion").ToString() + "";
                string Dato = "Solicitud enviada a través del sistema el  " + FecCre;
                reportes2.Tables["DtAntecedenteTec"].Clear();
                row = reportes2.Tables["DtAntecedenteTec"].NewRow();
                row["Antecedente"] = Dato;
                row["no"] = "1." + NoAntecedentes.ToString() + ".";
                reportes2.Tables["DtAntecedenteTec"].Rows.Add(row);
                NoAntecedentes = NoAntecedentes + 1;

                StrSql = "select CONVERT(CHAR(11),fecrecibe,3) as fecrecibe,codestatus from trecibesecre where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + " order by fecrecibe ";
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader3 = this.cmTransaccion.ExecuteReader();
                while (reader3.Read())
                {
                    row = reportes2.Tables["DtAntecedenteTec"].NewRow();
                    if (reader3["codestatus"].ToString() == "1")
                    {
                        row["Antecedente"] = "Ingreso de solicitud en Ventanilla Única del CONAP con fecha " + reader3["FecRecibe"];
                        row["no"] = "1." + NoAntecedentes.ToString() + ".";
                        NoAntecedentes = NoAntecedentes + 1;
                    }
                    else
                    {
                        row["Antecedente"] = "Solicitud no ingresada por incumplimiento de requisitos, " + reader3["FecRecibe"];
                        row["no"] = "1." + NoAntecedentes.ToString() + ".";
                        NoAntecedentes = NoAntecedentes + 1;
                    }
                    reportes2.Tables["DtAntecedenteTec"].Rows.Add(row);
                }
                reader3.Close();
                cn.Close();

                StrSql = "select CONVERT(CHAR(11),FecCre,3) as FecCre,CONVERT(CHAR(11),FECCIERRE,3) as FecCierre from tenmiendatec where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + " order by feccre ";
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader4 = this.cmTransaccion.ExecuteReader();
                while (reader4.Read())
                {
                    row = reportes2.Tables["DtAntecedenteTec"].NewRow();
                    row["Antecedente"] = "Enmienda técnica solicitada con fecha " + reader4["FecCre"];
                    row["no"] = "1." + NoAntecedentes.ToString() + ".";
                    NoAntecedentes = NoAntecedentes + 1;
                    reportes2.Tables["DtAntecedenteTec"].Rows.Add(row);
                    row = reportes2.Tables["DtAntecedenteTec"].NewRow();
                    row["Antecedente"] = "Recepción de enmiendas técnicas solicitadas con fecha  " + reader4["FecCre"];
                    row["no"] = "1." + NoAntecedentes.ToString() + ".";
                    NoAntecedentes = NoAntecedentes + 1;
                    reportes2.Tables["DtAntecedenteTec"].Rows.Add(row);
                }
                reader4.Close();
                cn.Close();




                tec.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                tec.SetDataSource((DataSet)reportes);
                tec.Subreports["RptAntecedenteTec.rpt"].SetDataSource((DataSet)reportes2);
                tec.Subreports["RptDetConclusionTec.rpt"].SetDataSource((DataSet)reportes);
                if (Tramite == "1")
                {
                    tec.ReportDefinition.Sections["DetailSection18"].SectionFormat.EnableSuppress = true;
                    tec.ReportDefinition.Sections["DetailSection20"].SectionFormat.EnableSuppress = true;
                    tec.ReportDefinition.Sections["DetailSection22"].SectionFormat.EnableSuppress = true;
                    tec.ReportDefinition.Sections["DetailSection21"].SectionFormat.EnableSuppress = true;
                }
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
            else if (Tramite == "2" || Tramite == "3")
            {
                this.StrSql = "select b.nombre,idelec,referencia,region,a.feccre,nodictamen,isnull(c.nombres,'') + ' ' + isnull(c.apellidos,'') as Regente, codid,'Inscripción' as Proceso,departamento,reciboing,fecharecibo,bolbanrural,constancia,nocons,profesion, nocol,codreg,codregempf,recomendacion,categoria,vigencia,isnull(i.nombres,'') + ' ' + isnull(i.apellidos,'') as tecnico,a.nus,folios,a.codregion,dep,year(a.feccre) as anis,b.nombre as reg,j.corr,puesto " +
                          "from tdictamentec a, tregion b, tregente c, tdepartamento d, trecomendacin e, tusuario i, tprovidencia j where a.codregion = b.codregion and c.codregente = a.codregente and d.coddep = c.coddep and e.codrecomendacion = a.codrecomendacion and i.codusuario = a.codusuario and a.codregente = j.codregente and a.nus = j.nus and a.codregente =  " + base.Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"] + "";
                reportes.Tables["DtDictamenTecAct"].Clear();
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
                string str = "";
                while (reader.Read())
                {
                    row = reportes.Tables["DtDictamenTecAct"].NewRow();
                    row["Referencia"] = reader["referencia"];
                    row["Delegacion"] = reader["region"];
                    row["FecDictamen"] = reader["feccre"];
                    row["NoDictamen"] = reader["nodictamen"];
                    row["Regente"] = reader["Regente"];
                    row["Dui"] = reader["codid"];
                    //row["Proceso"] = reader["Proceso"];
                    row["Tramite"] = Util.ObtieneRegistro("Select * from TTIPOTRAMITE where codtramite = " + Tramite + "", "Tramite").ToString();
                    row["Nus"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                    string nusincrip = Util.ObtieneRegistro("Select * from tsolicitud where codregente = "  + Request.QueryString["CodRegente"] + " and CodTramite = 1 ","nus").ToString();
                    row["FecRes"] = Util.ObtieneRegistro("Select * from tresolucion where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                    row["NoRes"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from tresolucion where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "noresolucion").ToString() + "-" + Convert.ToDateTime(row["FecRes"]).Year;
                    row["Categoria"] = Util.ObtieneRegistro("select CATEGORIA from TDICTAMENTEC where  CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "CATEGORIA").ToString();
                    row["cat"] = Util.ObtieneRegistro("select CATEGORIA from TDICTAMENTEC where  CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "CATEGORIA").ToString();
                    //row["BolBanrural"] = reader["bolbanrural"];
                    //row["Categoria"] = reader["constancia"];
                    row["FecDicLeg"] = Util.ObtieneRegistro("Select * from TDICTAMENLEG where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                    row["NoDicLeg"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from TDICTAMENLEG where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "nodictamen").ToString() + "-" + Convert.ToDateTime(row["FecDicLeg"]).Year;
                    row["FecDicTec"] = Util.ObtieneRegistro("Select * from TDICTAMENTEC where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "FecCre").ToString();
                    row["NoDicTec"] = reader["region"] + "-RF-" + Util.ObtieneRegistro("Select * from TDICTAMENTEC where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + nusincrip + "", "nodictamen").ToString() + "-" + Convert.ToDateTime(row["FecDicTec"]).Year;
                    row["Recomendacion"] = reader["Recomendacion"];
                    if (Util.ExisteDato("Select  * from TVOBOTEC where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "") == true)
                    {
                        row["Director"] = Util.ObtieneRegistro("select isnull(NOMBRES,'') +  ' ' + isnull(APELLIDOS,'') as nombre from TVOBOTEC a, TUSUARIO b where a.CODUSUARIO = b.CODUSUARIO and CODREGENTE = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "nombre").ToString();
                        row["IdElecDir"] = Util.ObtieneRegistro("select IDELEC from TVOBOTEC a, TUSUARIO b where a.CODUSUARIO = b.CODUSUARIO and CODREGENTE = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "IDELEC").ToString();
                    }
                    else
                    {
                        row["Director"] = "";
                        row["IdElecDir"] = "";
                    }
                    if (reader["Codregion"].ToString() == "1")
                        row["Tecnico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 2 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                    else if (reader["Codregion"].ToString() == "8")
                        row["Tecnico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 2 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                    else

                        row["Tecnico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 2 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString(); ;
                    //row["Tecnico"] = reader["Tecnico"];
                    if (Tramite == "2")
                    {
                        row["Juridico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 3 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                        if (reader["codregion"].ToString() == "1" || reader["codregion"].ToString() == "8")
                            row["TipoJuridico"] = "Director Forestal";
                        else
                            row["TipoJuridico"] = "Director 'Regional";
                        row["Tecnico"] = reader["Tecnico"];
                    }
                    else
                    {
                        if (Util.ExisteDato("Select * from tusuario where codtipousuario = 5 and codregion = " + reader["codregion"] + "") == true)
                        {
                            row["Juridico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 5 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                            row["TipoJuridico"] = Util.ObtieneRegistro("Select tipousuario from ttipousuario where codtipousuario = 5", "tipousuario").ToString();
                        }
                        else if (Util.ExisteDato("Select * from tusuario where codtipousuario = 4 and codregion = " + reader["codregion"] + "") == true)
                        {
                            row["Juridico"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 4 and codregion = " + reader["codregion"] + " and codestatus = 1", "Nombre").ToString();
                            row["TipoJuridico"] = Util.ObtieneRegistro("Select tipousuario from ttipousuario where codtipousuario = 4", "tipousuario").ToString();
                        }
                        else
                        {
                            row["Juridico"] = "";
                            row["TipoJuridico"] = "";
                        }
                    }
                    
                    row["nombre"] = reader["nombre"];
                    row["CodRegion"] = reader["codregion"];
                    row["Dep"] = reader["dep"];
                    //row["Reg"] = reader["reg"];
                    row["CodTramite"] = Tramite;
                    row["NUS"] = "RRAP - " + Convert.ToInt32(Request.QueryString["nus"]).ToString("D8");
                    row["NoProv"] = reader["region"] + "-RF-" + reader["Corr"] + "-" + reader["anis"];
                    row["reg"] = reader["reg"];
                    row["Folio"] = reader["folios"];
                    row["IdElec"] = reader["IdElec"];
                    row["ReciboIng"] = reader["reciboing"];
                    row["FechaRecibo"] = reader["fecharecibo"];
                    row["BolBanrural"] = reader["bolbanrural"];
                    row["Pago"] = Util.ObtieneRegistro("Select * from tcategoria where codcategoria = 2", "Pago").ToString();
                    row["Puesto"] = reader["puesto"];
                    reportes.Tables["DtDictamenTecAct"].Rows.Add(row);
                }
                reader.Close();
                this.cn.Close();
                reportes.Tables["DtDetDictamenTec"].Clear();
                this.StrSql = "Select * from tdetconclusion where codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "";
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader2 = this.cmTransaccion.ExecuteReader();
                while (reader2.Read())
                {
                    row = reportes.Tables["DtDetDictamenTec"].NewRow();
                    row["Conclusion"] = reader2["Conclusion"];
                    reportes.Tables["DtDetDictamenTec"].Rows.Add(row);
                }
                reader.Close();
                this.cn.Close();

                reportes.Tables["DtDocumentos"].Clear();
                StrSql = "Select * from trecibesecre where codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + " and CODESTATUS = 1";
                this.cn.Open();
                this.cmTransaccion.CommandText = this.StrSql;
                this.cmTransaccion.Connection = this.cn;
                this.cmTransaccion.CommandType = CommandType.Text;
                OleDbDataReader reader3 = this.cmTransaccion.ExecuteReader();
                if (reader3.Read())
                {
                    if (reader3["SOLICITUD"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Solicitud escrita en formulario del Consejo Nacional de Áreas Protegidas.";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["DOCID"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Copia legalizada del Documento Personal de Identificación ";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["TITULO"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Copia legalizada del Título que acredite el grado académico y especializaciones.";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["COLEGIADO"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Constancia original de colegiado activo (para profesionales).";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["CURRI"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Currículum Vitae según formato del Consejo Nacional de Áreas Protegidas.";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["PAGO"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Constancia de Cuota de Pago por Registro de Inscripción en BANRURAL y Recibo 63 A2 extendido por CONAP.";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                    if (reader3["NIT"].ToString() == "1")
                    {
                        row = reportes.Tables["DtDocumentos"].NewRow();
                        row["Documento"] = "Copia de Carné de Identificación Tributaria, extendido por la SAT.";
                        reportes.Tables["DtDocumentos"].Rows.Add(row);
                    }
                }
                reader.Close();
                this.cn.Close();





                tecAct.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                tecAct.SetDataSource((DataSet)reportes);
                if (Tramite == "2")
                    tecAct.ReportDefinition.Sections["DetailSection14"].SectionFormat.EnableSuppress = true;
                tecAct.Subreports["RptDetDictamenTec.rpt"].SetDataSource((DataSet)reportes);
                string str3 = Guid.NewGuid().ToString() + ".pdf";
                string url = base.Server.MapPath(".") + @"\" + this.DirRep + str3;
                DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
                {
                    DiskFileName = url
                };
                ExportOptions exportOptions = tecAct.ExportOptions;
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOptions.ExportDestinationOptions = options2;
                tecAct.Export();
                url = this.DirApp + this.DirRep + str3;
                base.Response.Redirect(url);
            }
            
            
        }
    }
}