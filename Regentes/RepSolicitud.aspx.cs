using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using CrystalDecisions.Web;
using System.Data;
using System.IO;
using CrystalDecisions.Shared;

namespace Regentes
{
    public partial class RepSolicitud : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        RptSolicitud solicitud = new RptSolicitud();
        RptActRegente solicitudAct = new RptActRegente();
        RptRenRegente solrenReg = new RptRenRegente();
        private CUtilitarios Util;

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            solicitud.Close();
            solicitud.Dispose();
            solicitudAct.Close();
            solicitudAct.Dispose();
            solrenReg.Close();
            solrenReg.Dispose();
            GC.Collect();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.StrSql = "Select nus, tramite as tipo,CodReg,CodRegEmpf,CodRegEcut,Apellidos,Nombres,CodId,DATEDIFF(YEAR, FECNAC , GETDATE()) AS Edad,NIt,Nacionalidad,genero,direccion,departamento,municipio,telefono,fax,correo,profesion,nocol, especializacion,case experiencia when 1 then 'Sí' else 'No' end as Expe,Cual,FecPresentacion,f.fectramite from tregente a, tgenero b, tdepartamento c, tmunicipio d, tenviosol e, tsolicitud f, ttipotramite g where g.codtramite = f.codtramite and a.codregente = f.codregente and a.codgenero = b.codgenero and c.coddep = a.coddep and d.codmun = a.codmun and d.coddep = c.coddep and e.codregente = a.codregente and a.codregente = " + base.Request.QueryString["CodRegente"] + " and nus = " + base.Request.QueryString["nus"] + "";
            DsReport reportes = new DsReport();
            reportes.Tables["DtSolicitud"].Clear();
            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            string str = "";
            while (reader.Read())
            {
                DataRow row = reportes.Tables["DtSolicitud"].NewRow();
                row["Tipo"] = reader["tipo"];
                row["NoRegRG"] = reader["CodReg"];
                row["NoRegEPMF"] = reader["CodRegEmpf"];
                row["NoRegECUT"] = reader["CodRegEcut"];
                row["Nombre"] = reader["Nombres"];
                row["Apellidos"] = reader["Apellidos"];
                row["Identificacion"] = reader["CodId"];
                row["NIT"] = reader["NIt"];
                row["Nacionalidad"] = reader["Nacionalidad"];
                row["Sexo"] = reader["genero"];
                row["Edad"] = reader["Edad"];
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Direccion = Util.ObtieneRegistro("Select * from tactregente where Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Direccion").ToString();
                    if (Direccion != reader["Direccion"].ToString())
                    {
                        row["Direccion"] = Direccion;
                        row["AlterDireccion"] = 1;
                    }
                    else
                    {
                        row["Direccion"] = reader["direccion"];
                        row["AlterDireccion"] = 0;
                    }
                }
                else
                {
                    row["Direccion"] = reader["direccion"];
                    row["AlterDireccion"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Dep = Util.ObtieneRegistro("Select departamento from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "departamento").ToString();
                    if (Dep != reader["Departamento"].ToString())
                    {
                        row["Departamento"] = Dep;
                        row["AlterDep"] = 1;
                    }
                    else
                    {
                        row["Departamento"] = reader["Departamento"];
                        row["AlterDep"] = 0;
                    }
                }
                else
                {
                    row["Departamento"] = reader["departamento"];
                    row["AlterDep"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Mun = Util.ObtieneRegistro("Select municipio from tactregente a, TDEPARTAMENTO b, tmunicipio c where a.Coddep = b.coddep and c.coddep = a.coddep and c.codmun = a.codmun and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "municipio").ToString();
                    if (row["AlterDep"].ToString() == "1")
                    {
                        row["Municipio"] = Mun;
                        row["AlterMun"] = 1;
                    }
                    else
                    {
                        if (Mun != reader["Municipio"].ToString())
                        {
                            row["Municipio"] = Mun;
                            row["AlterMun"] = 1;
                        }
                        else
                        {
                            row["Municipio"] = reader["municipio"];
                            row["AlterMun"] = 0;
                        }
                    }
                }
                else
                {
                    row["Municipio"] = reader["municipio"];
                    row["AlterMun"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Tel = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "telefono").ToString();
                    if (Tel != reader["Telefono"].ToString())
                    {
                        row["Telefono"] = Tel;
                        row["AlterTel"] = 1;
                    }
                    else
                    {
                        row["Telefono"] = reader["Telefono"];
                        row["AlterTel"] = 0;
                    }
                }
                else
                {
                    row["Telefono"] = reader["telefono"];
                    row["AlterTel"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Fax = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "fax").ToString();
                    if (Fax != reader["Fax"].ToString())
                    {
                        row["Fax"] = Fax;
                        row["AlterFax"] = 1;
                    }
                    else
                    {
                        row["Fax"] = reader["fax"];
                        row["AlterFax"] = 0;
                    }
                }
                else
                {
                    row["Fax"] = reader["fax"];
                    row["AlterFax"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Correo = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Correo").ToString();
                    if (Correo != reader["Correo"].ToString())
                    {
                        row["Correo"] = Correo;
                        row["AlterCorreo"] = 1;
                    }
                    else
                    {
                        row["Correo"] = reader["correo"];
                        row["AlterCorreo"] = 0;
                    }
                }
                else
                {
                    row["Correo"] = reader["correo"];
                    row["AlterCorreo"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Prefesion = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Profesion").ToString();
                    if (Prefesion != reader["Profesion"].ToString())
                    {
                        row["Profesion"] = reader["profesion"];
                        row["AlterProfesion"] = 1;
                    }
                    else
                    {
                        row["Profesion"] = reader["profesion"];
                        row["AlterProfesion"] = 0;
                    }
                }
                else
                {
                    row["Profesion"] = reader["profesion"];
                    row["AlterProfesion"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string NoCol = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "NoCol").ToString();
                    if (NoCol != reader["nocol"].ToString())
                    {
                        row["NoCol"] = NoCol;
                        row["AlterColegiado"] = 1;
                    }
                    else
                    {
                        row["NoCol"] = reader["nocol"];
                        row["AlterColegiado"] = 0;
                    }
                }
                else
                {
                    row["NoCol"] = reader["nocol"];
                    row["AlterColegiado"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Especial = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Especializacion").ToString();
                    if (Especial != reader["especializacion"].ToString())
                    {
                        row["Especializacion"] = Especial;
                        row["AlterEspecializacion"] = 1;
                    }
                    else
                    {
                        row["Especializacion"] = reader["especializacion"];
                        row["AlterEspecializacion"] = 0;
                    }
                }
                else
                {
                    row["Especializacion"] = reader["especializacion"];
                    row["AlterEspecializacion"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Exp = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Experiencia").ToString();
                    if (Exp == "1")
                        Exp = "Si";
                    else
                        Exp = "No";
                    if (Exp != reader["Expe"].ToString())
                    {
                        row["Experiencia"] = Exp;
                        str = Exp;
                        row["AlterExperiencia"] = 1;
                    }
                    else
                    {
                        row["Experiencia"] = reader["Expe"];
                        str = reader["Expe"].ToString();
                        row["AlterExperiencia"] = 0;
                    }
                }
                else
                {
                    row["Experiencia"] = reader["Expe"];
                    str = reader["Expe"].ToString();
                    row["AlterExperiencia"] = 0;
                }
                if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
                {
                    string Cual = Util.ObtieneRegistro("Select * from tactregente a, TDEPARTAMENTO b where a.Coddep = b.coddep and Codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Cual").ToString();
                    if (Cual != reader["Cual"].ToString())
                    {
                        row["Cuales"] = Cual;
                        row["AlterCual"] = 1;
                    }
                    else
                    {
                        row["Cuales"] = reader["Cual"];
                        row["AlterCual"] = 0;
                    }
                }
                else
                {
                    row["Cuales"] = reader["Cual"];
                    row["AlterCual"] = 0;
                }
                
                
                row["FecPresentacion"] = reader["fectramite"];
                //string nus = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and codtramite = 1","nus").ToString();
                row["nus"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                string path = base.Server.MapPath(".") + @"\FotosRegentes\\" + base.Request.QueryString["CodRegente"];
                if (Directory.Exists(path))
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] files = info.GetFiles("*.*");
                    DirectoryInfo[] directories = info.GetDirectories();
                    for (int i = 0; i < files.Length; i++)
                    {
                        row["Foto"] = this.ImageToByte(System.Drawing.Image.FromFile(files[i].FullName));
                    }
                }
                else
                {
                    row["Foto"] = "";
                }
                row["Anis"] = Util.ObtieneRegistro("Select * from trenovacion where CodRegente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "Anis").ToString();
                reportes.Tables["DtSolicitud"].Rows.Add(row);
            }
            reader.Close();
            this.cn.Close();

            if (Request.QueryString["tramite"].ToString() == "1")
            {
                solicitud.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                solicitud.SetDataSource((DataSet)reportes);
                if (str == "No")
                {
                    solicitud.ReportDefinition.Sections["DetailSection7"].SectionFormat.EnableSuppress = true;
                }
                string str4 = Guid.NewGuid().ToString() + ".pdf";
                string url = base.Server.MapPath(".") + @"\" + this.DirRep + str4;
                DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
                {
                    DiskFileName = url
                };
                ExportOptions exportOptions = solicitud.ExportOptions;
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOptions.ExportDestinationOptions = options2;
                solicitud.Export();
                url = this.DirApp + this.DirRep + str4;
                base.Response.Redirect(url);
            }
            else if (Request.QueryString["tramite"].ToString() == "2" || Request.QueryString["tramite"].ToString() == "3")
            {
                solicitudAct.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                solicitudAct.SetDataSource((DataSet)reportes);
                if (str == "No")
                {
                    solicitudAct.ReportDefinition.Sections["DetailSection7"].SectionFormat.EnableSuppress = true;
                }
                if (Request.QueryString["tramite"].ToString() == "2")
                {
                    solicitudAct.ReportDefinition.Sections["DetailSection13"].SectionFormat.EnableSuppress = true;
                }
                
                string str4 = Guid.NewGuid().ToString() + ".pdf";
                string url = base.Server.MapPath(".") + @"\" + this.DirRep + str4;
                DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
                {
                    DiskFileName = url
                };
                ExportOptions exportOptions = solicitudAct.ExportOptions;
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOptions.ExportDestinationOptions = options2;
                solicitudAct.Export();
                url = this.DirApp + this.DirRep + str4;
                base.Response.Redirect(url);
            }
            else if (Request.QueryString["tramite"].ToString() == "4")
            {
                solrenReg.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                solrenReg.SetDataSource((DataSet)reportes);
                
                string str4 = Guid.NewGuid().ToString() + ".pdf";
                string url = base.Server.MapPath(".") + @"\" + this.DirRep + str4;
                DiskFileDestinationOptions options2 = new DiskFileDestinationOptions
                {
                    DiskFileName = url
                };
                ExportOptions exportOptions = solrenReg.ExportOptions;
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOptions.ExportDestinationOptions = options2;
                solrenReg.Export();
                url = this.DirApp + this.DirRep + str4;
                base.Response.Redirect(url);
            }
         
            
           
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen1", script);
        }

        public byte[] ImageToByte(System.Drawing.Image pImagen)
        {
            byte[] buffer = null;
            try
            {
                if (pImagen != null)
                {
                    MemoryStream stream = new MemoryStream();
                    pImagen.Save(stream, pImagen.RawFormat);
                    buffer = stream.GetBuffer();
                    stream.Close();
                    return buffer;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}