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
using System.IO;

namespace Regentes
{
    public partial class RepConstancia : System.Web.UI.Page
    {
        private OleDbCommand cmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected CrystalReportSource CrystalReportSource1;
        protected CrystalReportViewer CrystalReportViewer1;
        private string DirApp = System.Configuration.ConfigurationManager.AppSettings["DirApp"];
        private string DirRep = System.Configuration.ConfigurationManager.AppSettings["DirRep"];
        private string StrSql = "";
        private CUtilitarios Util;
        RptConstancia Jur = new RptConstancia();

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
            string CodTramite = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();
            string RegInterno = Util.ObtieneRegistro("Select * from tresolucion where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "REGINTERNO").ToString();
            StrSql = "select region,a.codregente,CodReg,CodRegEmpf,CodRegEcut,c.Nombres,c.Apellidos,codid,profesion,especializacion,CONVERT(CHAR(11),fecaut,3) as fecaut, " +
                     "CONVERT(CHAR(11),fecven,3) as fecven,e.idelec,Categoria,isnull(f.nombres,'') + ' ' + isnull(f.apellidos,'') as Director, idunico, b.nombre as reg, a.codregion as regi,codigo,FECACTUALIZACION,enfunciones, " +
                     "a.nus,nacionalidad,c.direccion,correo,telefono,departamento,municipio,nocol " +  
                     "from tdictamentec a, tregion b, tregente c, tperiodo d, TVOBOTEC e,tusuario f, tdepartamento h, tmunicipio i " +
                     "where a.codregion = b.codregion and c.codregente = a.codregente and d.codregente = a.codregente and d.codregente = c.codregente  " +
                     "and e.codregente = a.codregente and e.codregente = d.codregente and e.codregente = a.codregente and f.codusuario = e.codusuario " +
                     "and h.coddep = c.coddep and i.codmun = c.codmun and h.coddep = i.coddep " +
                     "and d.codestatus = 1 and a.nus = e.nus and a.codregente =  " + base.Request.QueryString["CodRegente"] + " and a.nus = " + Request.QueryString["nus"];


            DsReport reportes = new DsReport();
            reportes.Tables["DTConstancia"].Clear();

            this.cn.Open();
            this.cmTransaccion.CommandText = this.StrSql;
            this.cmTransaccion.Connection = this.cn;
            this.cmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.cmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                row = reportes.Tables["DTConstancia"].NewRow();
                row["Region"] = reader["reg"];
                row["RegistroCONAP"] = "RFAP " + Convert.ToInt32(reader["codigo"]).ToString("D8");
                row["RegistroRF"] = reader["CodReg"];
                row["RegistroEPMF"] = reader["CodRegEmpf"];
                row["RegistroECUT"] = reader["CodRegEcut"];
                row["Nombres"] = reader["Nombres"];
                row["Apellidos"] = reader["Apellidos"];
                row["DPI"] = reader["codid"];
                row["Profesion"] = reader["profesion"];
                row["Categoria"] = reader["Categoria"];
                row["Especializacion"] = reader["especializacion"];
                row["FecInscripcion"] = reader["fecaut"];
                row["FecVencimiento"] = reader["fecven"];
                if (CodTramite == "2" || CodTramite == "3" || CodTramite == "4")
                    row["FecRenovacion"] = reader["FECACTUALIZACION"];
                //else
                    //row["FecRenovacion"] = reader["FECACTUALIZACION"];
                if (Util.ObtieneRegistro("Select * from tnombre where CodRegion = " + reader["regi"] + "", "nombre").ToString() == "")
                {
                    row["Director"] = Util.ObtieneRegistro("Select isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre from tusuario where codtipousuario = 3 and codregion = " + reader["regi"] + " and codestatus = 1", "Nombre").ToString();
                    if (Util.ObtieneRegistro("Select * from tusuario where codtipousuario = 3 and codregion = " + reader["regi"] + " and codestatus = 1", "enfunciones").ToString() == "1")
                        row["enfunciones"] = "Nombre y Firma Director Regional en funciones";
                    else
                        row["enfunciones"] = "Nombre y Firma Director Regional";
                }
                else
                {
                    row["Director"] = Util.ObtieneRegistro("Select nombre from tnombre where codregion = " + reader["regi"] + "", "Nombre").ToString();
                    row["enfunciones"] = "Nombre y Firma Director Regional";
                }
                row["IdElec"] = reader["idelec"];
                row["IdUnico"] = reader["IdUnico"];
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
                string PathArchivo = Server.MapPath(".") + @"\CodigosQR\" + Request.QueryString["CodRegente"] + "";
                string FileName = PathArchivo + @"\" + Request.QueryString["CodRegente"] + ".jpg";
                row["QrCode"] = ImageToByte(System.Drawing.Image.FromFile(FileName));
                row["NoGestion"] = "RRAP - " + Convert.ToInt32(reader["nus"]).ToString("D8");
                row["Nacionalidad"] = reader["nacionalidad"].ToString();
                row["Direccion"] = reader["direccion"].ToString() + ", " + reader["departamento"].ToString() + ", " + reader["municipio"].ToString();
                row["Correo"] = reader["Correo"].ToString();
                row["Telefono"] = reader["telefono"].ToString();
                string Region = Util.ObtieneRegistro("Select Region from tdictamentec a, tregion b where a.codregion = b.codregion and a.codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "region").ToString();
                row["NoRes"] = Region + "-RF-" + Util.ObtieneRegistro("Select NoResolucion from tresolucion where  codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "noresolucion").ToString() + "-" + Util.ObtieneRegistro("Select year(feccre) as anis from tresolucion where  codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "anis").ToString();
                row["FechaRes"] = Convert.ToDateTime(Util.ObtieneRegistro("Select feccre from tresolucion where  codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "feccre"));
                row["nocol"] = reader["nocol"].ToString();
                string RegenteComo = "Regente Forestal";
                if (reader["CodRegEmpf"].ToString() != "")
                    RegenteComo = RegenteComo + ", Elaborador de Planes de Manejo Forestal";
                if (reader["CodRegEcut"].ToString() != "")
                    RegenteComo = RegenteComo + ", Elaborador de Estudios de Capacidad de Uso  de la Tierra";
                row["regentecomo"] = RegenteComo + ".";
                row["NoRegitroInterno"] = RegInterno;
                reportes.Tables["DTConstancia"].Rows.Add(row);
            }

            reader.Close();
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