using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;

namespace Regentes
{
    public partial class Resolucion : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        private OleDbCommand CmTransaccion = new OleDbCommand();
        private OleDbConnection cn = new OleDbConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            btnGrabaDictamen.Click += new EventHandler(btnGrabaDictamen_Click);
            BtnAddConsiderando.Click += new EventHandler(BtnAddConsiderando_Click);
            GrdConsiderando.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdConsiderando_ItemCommand);
            GrdConsiderando.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdConsiderando_NeedDataSource);
            this.Util = new CUtilitarios();
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.TxtCodregente.Text = base.Request.QueryString["CodRegente"].ToString();
                TxtNus.Text = Request.QueryString["nus"].ToString();
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Util.LlenaCombo("Select * from trecomendacin", this.CboRecomendacion, "CodRecomendacion", "Recomendacion");
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text= this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                CargaData();
            }
        }

        

        void GrdConsiderando_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CodRegente,Corr,Considerando from tdetconsiderando where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text;
            Util.LlenaGrid(StrSql, GrdConsiderando);
        }

        void GrdConsiderando_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                StrSql = string.Concat(new object[] { "Delete from tdetconsiderando where codregente = ", this.TxtCodregente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"], " and nus = ", TxtNus.Text });
                Util.EjecutaIns(StrSql);
                LblMensaje.Visible = true;
                LblMensaje.Text = "Considerando Eliminado Exitosamente";
                GrdConsiderando.Rebind();
            }
        }

        void BtnAddConsiderando_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (TxtConsiderandoUno.Text == "")
            {
                LblMensaje.Text = "Debe Agregar texto en considerando";
                LblMensaje.Visible = true;
            }
            else
            {
                decimal num = Util.MaxCorr("Select Max(Corr) as Maximo from tdetconsiderando where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "");
                StrSql = string.Concat(new object[] { "Insert into tdetconsiderando values (", TxtCodregente.Text, ", ", num, ", '", TxtConsiderandoUno.Text, "', ", TxtNus.Text, ")" });
                Util.EjecutaIns(this.StrSql);
                GrdConsiderando.Rebind();
                TxtConsiderandoUno.Text = "";
            }
        }

        void CargaData()
        {
            StrSql = "Select * from tresolucion where codregente  = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
            this.cn.Open();
            this.CmTransaccion.CommandText = this.StrSql;
            this.CmTransaccion.Connection = this.cn;
            this.CmTransaccion.CommandType = CommandType.Text;
            OleDbDataReader reader = this.CmTransaccion.ExecuteReader();
            if (reader.Read())
            {
                TxtReferencia.Text = reader["Referncia"].ToString();
                TxtPorTando.Text = reader["PorTanto"].ToString();
                TxtResuelve.Text = reader["Resuelve"].ToString();
                TxtFolio.Text = reader["nofolio"].ToString();
                TxtControlInterno.Text = reader["nocontrol"].ToString();
                CboRecomendacion.SelectedValue = reader["CodRecomendacion"].ToString();
            }
        }

        bool Valida()
        {
            LblMensaje.Visible = false;
            if (TxtReferencia.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar la Referencia";
                LblMensaje.Visible = true;
                return false;
            }
            if (TxtRegInterno.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el número de registro interno";
                LblMensaje.Visible = true;
                return false;
            }
            else if (GrdConsiderando.Items.Count == 0)
            {
                LblMensaje.Text = "Debe Ingresar al menos un considerando";
                LblMensaje.Visible = true;
                return false;
            }
            else if (TxtPorTando.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el enunciado por tanto";
                LblMensaje.Visible = true;
                return false;
            }
            else if (TxtResuelve.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el enunciado resuelve";
                LblMensaje.Visible = true;
                return false;
            }
            if (TxtFolio.Text == "0" || TxtFolio.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el no. de folios";
                LblMensaje.Visible = true;
                return false;
            }
            else
                return true;
        }

        void btnGrabaDictamen_Click(object sender, EventArgs e)
        {
            if (Valida() == true)
            {
                StrSql = "Update tresolucion set nocontrol = '" + TxtControlInterno.Text + "', referncia = '" + TxtReferencia.Text + "',  PorTanto = '" + TxtPorTando.Text + "', Resuelve = '" + TxtResuelve.Text + "', nofolio = " + TxtFolio.Text + ", codrecomendacion = " + CboRecomendacion.SelectedValue + ", REGINTERNO = '" + TxtRegInterno.Text + "'  where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                if (Util.ObtieneRegistro("Select * from tresolucion where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "", "NoResolucion").ToString() == "")
                {
                    decimal MaxResolucion = Util.MaxCorr("Select Max(NoResolucion) as Maximo from tresolucion where YEAR(FECCRE) = year(getdate())");
                    StrSql = "Update tresolucion set NoResolucion = " + MaxResolucion + " where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                    Util.EjecutaIns(StrSql);

                    string TipoTramite = Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + Request.QueryString["CodRegente"] + " and nus = " + Request.QueryString["nus"] + "", "codtramite").ToString();

                    decimal MaxProv = Util.MaxCorr("Select max(Corr) as Maximo from tprovidencialeg where YEAR(FECCRE) = year(getdate())");
                    StrSql = "Insert into tprovidencialeg values (" + TxtCodregente.Text + ",'" + string.Format("{0:MM/dd/yyyy H:mm:ss}", Util.FechaDB()) + "'," + Session["CodUsuario"] + "," + MaxProv + ", " + TxtNus.Text + ")";
                    Util.EjecutaIns(StrSql);
                    
                    
                }
                


                LblMensaje.Text = "Resolución guardada con éxito";
                LblMensaje.Visible = true;
                //Response.Redirect("VerJuridico.aspx");
            }
        }

        
    }
}