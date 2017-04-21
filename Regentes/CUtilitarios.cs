using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Regentes
{
    public class CUtilitarios
    {
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private OleDbConnection cnPermiso = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        private OleDbDataAdapter daData = new OleDbDataAdapter();
        private DataSet dsGen = new DataSet();
        private string StrSql = "";

        public void BorrarArchivo(string Path)
        {
            if (Directory.Exists(Path))
            {
                DirectoryInfo info = new DirectoryInfo(Path);
                FileInfo[] files = info.GetFiles("*.*");
                DirectoryInfo[] directories = info.GetDirectories();
                for (int i = 0; i < files.Length; i++)
                {
                    files[i].Delete();
                }
            }
        }

        public double Dias(string CodRegente, string Nus)
        {
            DateTime d1 = FechaDB();
            DateTime d2 = Convert.ToDateTime(ObtieneRegistro("Select * from tsolicitud where codregente = " + CodRegente + " and nus = " + Nus + "", "FecTramite"));
            return (d1 - d2).TotalDays;
        }

        public double CalculaDias(DateTime FechaIni, DateTime FechaFin)
        {
            DateTime d1 = FechaFin;
            DateTime d2 = FechaIni;
            return (d1 - d2).TotalDays;
        }

        public string CodRegion(int CodRegente)
        {
            string str = this.ObtieneRegistro("Select * from tregente where codregente = " + CodRegente, "CodDep").ToString();
            return this.ObtieneRegistro("Select * from trelregiondep where Coddep = " + str, "CodRegion").ToString();
        }

        public bool EjecutaIns(string Strsql)
        {
            this.cn.Open();
            this.CmTransaccion.CommandText = Strsql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            if (this.CmTransaccion.ExecuteNonQuery() > 0)
            {
                this.cn.Close();
                return true;
            }
            this.cn.Close();
            return false;
        }

        

        public void EstablecePermisos(MasterPage MasPag, int CodUsuario)
        {
            RadMenu menu = (RadMenu)MasPag.FindControl("Menu");
            StrSql = "Select * from tpermiso where CodUsuario = " + CodUsuario + "";
            cnPermiso.Open();
            CmTransaccion.CommandText = StrSql;
            CmTransaccion.Connection = cnPermiso;
            CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = CmTransaccion.ExecuteReader();
            while (reader.Read())
            {
                
                switch (Convert.ToInt32(reader["CodMenu"]))
                {
                    case 1: //Menu Secretaria
                        if (ExisteDato("Select * from tpermiso where CodUsuario = " + CodUsuario + " and CodMenu = " + reader["CodMenu"] + " and CodRol = 1") == true)
                            menu.Items[0].Visible = true;
                        switch (Convert.ToInt32(reader["CodForma"]))
                        {
                            case 1: //Recepcion de Expeientes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[0].Items[0].Visible = true;
                                else
                                    menu.Items[0].Items[0].Visible = false;
                                break;
                            case 2: //Historial de Expedientes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[0].Items[1].Visible = true;
                                else
                                    menu.Items[0].Items[1].Visible = false;
                                break;
                            case 3: //Expedientes Finalizados
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[0].Items[2].Visible = true;
                                else
                                    menu.Items[0].Items[2].Visible = false;
                                break;
                            case 4: //Expedientes Incompletos
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[0].Items[3].Visible = true;
                                else
                                    menu.Items[0].Items[3].Visible = false;
                                break;
                            case 5: //Reporte de Recibos de Ingresos Varios
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[0].Items[4].Visible = true;
                                else
                                    menu.Items[0].Items[4].Visible = false;
                                break;
                        }
                        break;
                    case 2: //Técnico Forestal
                        if (ExisteDato("Select * from tpermiso where CodUsuario = " + CodUsuario + " and CodMenu = " + reader["CodMenu"] + " and CodRol = 1") == true)
                            menu.Items[1].Visible = true;
                        switch (Convert.ToInt32(reader["CodForma"]))
                        {
                            case 1: //Solicitud de Enmiendas
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[1].Items[0].Visible = true;
                                else
                                    menu.Items[1].Items[0].Visible = false;
                                break;
                            case 2: //Emisión de Dictamen
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[1].Items[1].Visible = true;
                                else
                                    menu.Items[1].Items[1].Visible = false;
                                break;
                            case 3: //Historial de Expedientes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[1].Items[2].Visible = true;
                                else
                                    menu.Items[1].Items[2].Visible = false;
                                break;
                        }
                        break;
                    case 3: //Director Regional
                        if (ExisteDato("Select * from tpermiso where CodUsuario = " + CodUsuario + " and CodMenu = " + reader["CodMenu"] + " and CodRol = 1") == true)
                            menu.Items[2].Visible = true;
                        switch (Convert.ToInt32(reader["CodForma"]))
                        {
                            case 1: //Visto Bueno Dictamen Técnico
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[2].Items[0].Visible = true;
                                else
                                    menu.Items[2].Items[0].Visible = false;
                                break;
                            case 2: //Visto Bueno Dictamen Jurídico
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[2].Items[1].Visible = true;
                                else
                                    menu.Items[2].Items[1].Visible = false;
                                break;
                            case 3: //Historial de Expedientes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[2].Items[2].Visible = true;
                                else
                                    menu.Items[2].Items[2].Visible = false;
                                break;
                            case 4: //Activar/desactivar Regentes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[2].Items[3].Visible = true;
                                else
                                    menu.Items[2].Items[3].Visible = false;
                                break;
                        }
                        break;
                    case 4: //Asesor Jurídico
                        if (ExisteDato("Select * from tpermiso where CodUsuario = " + CodUsuario + " and CodMenu = " + reader["CodMenu"] + " and CodRol = 1") == true)
                            menu.Items[3].Visible = true;
                        switch (Convert.ToInt32(reader["CodForma"]))
                        {
                            case 1: //Dictamen Jurídico
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[3].Items[0].Visible = true;
                                else
                                    menu.Items[3].Items[0].Visible = false;
                                break;
                            case 2: //Historial de Expedientes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[3].Items[1].Visible = true;
                                else
                                    menu.Items[3].Items[1].Visible = false;
                                break;
                            case 3: //Solicitud de Enmiendas
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[1].Visible = true;
                                else
                                    menu.Items[4].Items[1].Visible = false;
                                break;
                        }
                        break;
                    case 5: //Administracion
                        if (ExisteDato("Select * from tpermiso where CodUsuario = " + CodUsuario + " and CodMenu = " + reader["CodMenu"] + " and CodRol = 1") == true)
                            menu.Items[4].Visible = true;
                        switch (Convert.ToInt32(reader["CodForma"]))
                        {
                            case 1: //Cambiar Clave
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[0].Visible = true;
                                else
                                    menu.Items[4].Items[0].Visible = false;
                                break;
                            case 2: //Creacion de Usuarios
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[1].Visible = true;
                                else
                                    menu.Items[4].Items[1].Visible = false;
                                break;
                            case 3: //Administracion de Usuarios Regentes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[2].Visible = true;
                                else
                                    menu.Items[4].Items[2].Visible = false;
                                break;
                            case 10: //Administracion de Usuarios Planes de Manejo
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[3].Visible = true;
                                else
                                    menu.Items[4].Items[3].Visible = false;
                                break;
                            case 4: //Regentes en Proceso
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[4].Visible = true;
                                else
                                    menu.Items[4].Items[4].Visible = false;
                                break;
                            case 5: //Regentes Terminados
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[5].Visible = true;
                                else
                                    menu.Items[4].Items[5].Visible = false;
                                break;
                            case 6: //Activar/desactivar Regentes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[6].Visible = true;
                                else
                                    menu.Items[4].Items[6].Visible = false;
                                break;
                            case 7: //Nombres Alternos
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[7].Visible = true;
                                else
                                    menu.Items[4].Items[7].Visible = false;
                                break;
                            case 8: //Expedientes Finalizados
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[8].Visible = true;
                                else
                                    menu.Items[4].Items[8].Visible = false;
                                break;
                            case 9: //Listado Regentes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[9].Visible = true;
                                else
                                    menu.Items[4].Items[9].Visible = false;
                                break;
                            case 11: //Hist. Datos Personales Regentes
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[10].Visible = true;
                                else
                                    menu.Items[4].Items[10].Visible = false;
                                break;
                            case 12: //Expedientes dados de baja
                                if (Convert.ToInt32(reader["CodRol"]) == 1)
                                    menu.Items[4].Items[11].Visible = true;
                                else
                                    menu.Items[4].Items[11].Visible = false;
                                break;
                        }
                        break;
                }
            }
            cnPermiso.Close();


            //if (CodTipoUsuario == 0)
            //{
            //    menu.Items[0].Visible = false;
            //    menu.Items[1].Visible = false;
            //    menu.Items[2].Visible = false;
            //    menu.Items[3].Visible = false;
            //    menu.Items[4].Visible = true;
            //}
            //else if (CodTipoUsuario == 1)
            //{
            //    menu.Items[0].Visible = true;
            //    menu.Items[1].Visible = false;
            //    menu.Items[2].Visible = false;
            //    menu.Items[3].Visible = false;
            //    menu.Items[4].Items[1].Visible = false;
            //}
            //else if (CodTipoUsuario == 2)
            //{
            //    menu.Items[0].Visible = false;
            //    menu.Items[1].Visible = true;
            //    menu.Items[2].Visible = false;
            //    menu.Items[3].Visible = false;
            //    menu.Items[4].Items[1].Visible = false;
            //}
            //else if (CodTipoUsuario == 3)
            //{
            //    menu.Items[0].Visible = false;
            //    menu.Items[1].Visible = false;
            //    menu.Items[2].Visible = true;
            //    menu.Items[3].Visible = false;
            //    menu.Items[4].Items[1].Visible = false;
            //}
            //else if (CodTipoUsuario == 4)
            //{
            //    menu.Items[0].Visible = false;
            //    menu.Items[1].Visible = false;
            //    menu.Items[2].Visible = false;
            //    menu.Items[3].Visible = true;
            //    menu.Items[4].Items[1].Visible = false;
            //}
        }

        public bool ExisteDato(string StrSql)
        {
            this.cn.Open();
            this.daData.SelectCommand = new OleDbCommand(StrSql, this.cn);
            this.daData.Fill(this.dsGen, "DATA");
            if (this.dsGen.Tables["DATA"].Rows.Count > 0)
            {
                this.cn.Close();
                this.dsGen.Tables["DATA"].Clear();
                return true;
            }
            this.cn.Close();
            this.dsGen.Tables["DATA"].Clear();
            return false;
        }

        public DateTime FechaDB()
        {
            string str = "SELECT getdate() as fecha";
            this.cn.Open();
            this.CmTransaccion.CommandText = str;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                DateTime time = Convert.ToDateTime(reader["fecha"]);
                reader.Close();
                this.cn.Close();
                return time;
            }
            reader.Close();
            this.cn.Close();
            return Convert.ToDateTime("01/01/2000");
        }

        public string GeneraIdElec(int CodUsuario)
        {
            string str = this.ObtieneRegistro("SELECT REVERSE(REPLACE(USUARIO,'-','')) as Rev FROM TUSUARIO where CodUsuario = " + CodUsuario, "Rev").ToString();
            int num = new Random(DateTime.Now.Millisecond).Next(0xf4240, 0x98967f);
            return (str + num);
        }

        public string GeneraIdunico(string Id)
        {
            int num = new Random(DateTime.Now.Millisecond).Next(0xf4240, 0x98967f);
            return (Id + num);
        }

        public int IIf(bool Expression, int TruePart, int FalsePart)
        {
            return (Expression ? TruePart : FalsePart);
        }

        public void LlenaCombo(string Strsql, RadComboBox Combo, string Llave, string Descripcion)
        {
            Combo.ClearSelection();
            this.cn.Open();
            this.daData.SelectCommand = new OleDbCommand(Strsql, this.cn);
            this.daData.Fill(this.dsGen, "DATA");
            Combo.DataSource = this.dsGen;
            Combo.DataMember = "DATA";
            Combo.DataTextField = Descripcion;
            Combo.DataValueField = Llave;
            Combo.DataBind();
            this.cn.Close();
            this.dsGen.Tables["DATA"].Clear();
        }

        public void LlenaGrid(string StrSql, RadGrid Grid)
        {
            Grid.Culture = new CultureInfo("es-PE");
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            this.cn.Open();
            adapter.SelectCommand = new OleDbCommand(StrSql, this.cn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            this.cn.Close();
            Grid.DataSource = dataTable;
        }

        

        public string NomUsuario(int CodUsuario)
        {
            this.StrSql = "Select * from tusuario where codusuario = " + CodUsuario;
            this.cn.Open();
            this.CmTransaccion.CommandText = this.StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                string str = reader.GetString(1);
                string str2 = reader.GetString(2);
                this.cn.Close();
                return (str + " " + str2);
            }
            this.cn.Close();
            return "";
        }

        public string NomRegente(int CodRegente)
        {
            this.StrSql = "Select * from tregente where codregente = " + CodRegente;
            this.cn.Open();
            this.CmTransaccion.CommandText = this.StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                string str = reader.GetString(4);
                string str2 = reader.GetString(3);
                this.cn.Close();
                return (str + " " + str2);
            }
            this.cn.Close();
            return "";
        }

        public decimal MaxCorr(string StrSql)
        {
            dsGen.Tables.Clear();
            cn.Open();
            daData.SelectCommand = new OleDbCommand(StrSql, cn);
            daData.Fill(dsGen, "MAXIM");
            if (dsGen.Tables["MAXIM"].Rows.Count > 0)
            {
                if (dsGen.Tables["MAXIM"].Rows[0]["MAXIMO"] == DBNull.Value)
                {
                    cn.Close();
                    return 1;
                }
                else
                {
                    cn.Close();
                    return Convert.ToDecimal(dsGen.Tables["MAXIM"].Rows[0]["MAXIMO"]) + 1;
                }
            }
            else
            {
                cn.Close();
                return 1;
            }


        }

        public object ObtieneRegistro(string StrSql, string Campo)
        {
            this.cn.Open();
            this.CmTransaccion.CommandText = StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                object obj2 = reader[Campo];
                this.cn.Close();
                return obj2;
            }
            this.cn.Close();
            return "";
        }

        public Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string ObtieneRegion(string CodRegente)
        {
            this.cn.Open();
            StrSql = "Select nombre as region from tdictamentec a, tregion b where a.codregion = b.codregion and Codregente = " + CodRegente + "";
            this.CmTransaccion.CommandText = StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                string obj2 = reader["region"].ToString();
                this.cn.Close();
                return obj2;
            }
            this.cn.Close();
            return "";
        }

        public string ObtieneNus(string CodRegente)
        {
            this.cn.Open();
            StrSql = "Select nus from tregente where Codregente = " + CodRegente + "";
            this.CmTransaccion.CommandText = StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                string obj2 = reader["nus"].ToString();
                this.cn.Close();
                return obj2;
            }
            this.cn.Close();
            return "";
        }

        public bool ExisteArchivo(string Path)
        {
            if (Directory.Exists(Path))
            {
                DirectoryInfo info = new DirectoryInfo(Path);
                FileInfo[] files = info.GetFiles("*.*");
                DirectoryInfo[] directories = info.GetDirectories();
                int num = 0;
                while (num < files.Length)
                {
                    return true;
                }
            }
            return false;
        }
    }
}