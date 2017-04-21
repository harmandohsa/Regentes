using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class DictamenJur : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            BtnAddConFundamento.Click += new EventHandler(BtnAddConFundamento_Click);
            GrdFundamento.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdFundamento_ItemCommand);
            GrdFundamento.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdFundamento_NeedDataSource);
            BtnAddAnalisis.Click += new EventHandler(BtnAddAnalisis_Click);
            GrdAnalisis.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdAnalisis_NeedDataSource);
            GrdAnalisis.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdAnalisis_ItemCommand);
            BtnAddRecomendacion.Click += new EventHandler(BtnAddRecomendacion_Click);
            GrdRecomendacion.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdRecomendacion_NeedDataSource);
            GrdRecomendacion.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdRecomendacion_ItemCommand);
            BtnGrabaReferencia.Click += new EventHandler(BtnGrabaReferencia_Click);
            btnGrabaDictamen.Click += new EventHandler(btnGrabaDictamen_Click);
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
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                TxtReferencia.Text = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "", "Referencia").ToString();
                TxtPuesto.Text = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "", "Puesto").ToString();
                TxtControlInterno.Text = Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "", "nocontrol").ToString();
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
            
            if (GrdFundamento.Items.Count == 0)
            {
                LblMensaje.Text = "Debe Ingresar Fundamento";
                LblMensaje.Visible = true;
                return false;
            }
            if (GrdAnalisis.Items.Count == 0)
            {
                LblMensaje.Text = "Debe Ingresar Análisis";
                LblMensaje.Visible = true;
                return false;
            }
            if (GrdRecomendacion.Items.Count == 0)
            {
                LblMensaje.Text = "Debe Ingresar Recomendaciones";
                LblMensaje.Visible = true;
                return false;
            }
            if (TxtPuesto.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el puesto";
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
                StrSql = "Update tdictamenleg set referencia = '" + TxtReferencia.Text + "', puesto = '" + TxtPuesto.Text + "', NOCONTROL = '" + TxtControlInterno.Text + "' where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                Util.EjecutaIns(StrSql);
                if (Util.ObtieneRegistro("Select * from tdictamenleg where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "", "NoDictamen").ToString() == "")
                {
                    decimal MaxDictamenJur = Util.MaxCorr("Select Max(NoDictamen) as Maximo from tdictamenleg where YEAR(FECCRE) = year(getdate())");
                    StrSql = "Update tdictamenleg set nodictamen = " + MaxDictamenJur + " where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
                    Util.EjecutaIns(StrSql);
                }
                LblMensaje.Text = "Dictamen guardado con éxito";
                LblMensaje.Visible = true;
                //Response.Redirect("VerJuridico.aspx");
            }
           
        }

        void BtnGrabaReferencia_Click(object sender, EventArgs e)
        {
            StrSql = "Update tdictamenleg set referencia = '" + TxtReferencia.Text + "' where codregente = " + TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
            Util.EjecutaIns(StrSql);
            LblMensaje.Text = "Referencia Actualizada";
            LblMensaje.Visible = true;
        }

        void GrdRecomendacion_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from trecomendacion where codregente = ", this.TxtCodregente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"], " and nus = ", TxtNus.Text });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensaje.Visible = true;
                GrdRecomendacion.Rebind();
                this.LblMensaje.Text = "Recomendación Eliminda";
                this.LblMensaje.Visible = true;
            }
        }

        void GrdRecomendacion_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.StrSql = "Select  CodRegente,Corr,Recomendacion from trecomendacion where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text;
            this.Util.LlenaGrid(this.StrSql, GrdRecomendacion);
        }

        void BtnAddRecomendacion_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (this.TxtRecomendaciones.Text == "")
            {
                this.LblMensaje.Text = "Debe Agregar una Recomendación";
                this.LblMensaje.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from trecomendacion where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text);
                this.StrSql = string.Concat(new object[] { "Insert into trecomendacion values (", this.TxtCodregente.Text, ", ", num, ", '", this.TxtRecomendaciones.Text, "', ", TxtNus.Text, ")" });
                this.Util.EjecutaIns(this.StrSql);
                GrdRecomendacion.Rebind();
                this.TxtRecomendaciones.Text = "";
                this.LblMensaje.Text = "Recomendación agregada";
                this.LblMensaje.Visible = true;
            }
        }

        void GrdAnalisis_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from tdetanalisleg where codregente = ", this.TxtCodregente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"], " and nus = ", TxtNus.Text });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensaje.Visible = true;
                GrdAnalisis.Rebind();
                this.LblMensaje.Text = "Análisis Eliminda";
                this.LblMensaje.Visible = true;
            }
        }

        void GrdAnalisis_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.StrSql = "Select  CodRegente,Corr,AnalisisLeg from tdetanalisleg where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "";
            this.Util.LlenaGrid(this.StrSql, GrdAnalisis);
        }

        void BtnAddAnalisis_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (this.TxtAnalisis.Text == "")
            {
                this.LblMensaje.Text = "Debe Agregar un Análisis";
                this.LblMensaje.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from tdetanalisleg where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "");
                this.StrSql = string.Concat(new object[] { "Insert into tdetanalisleg values (", this.TxtCodregente.Text, ", ", num, ", '", this.TxtAnalisis.Text, "', ", TxtNus.Text, ")" });
                this.Util.EjecutaIns(this.StrSql);
                this.GrdAnalisis.Rebind();
                this.TxtAnalisis.Text = "";
                this.LblMensaje.Text = "Análisis agregado";
                this.LblMensaje.Visible = true;
            }
        }

        void GrdFundamento_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.StrSql = "Select  CodRegente,Corr,Fundamento from tdetfundamento where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text;
            this.Util.LlenaGrid(this.StrSql, GrdFundamento);
        }

        void GrdFundamento_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (e.CommandName == "CmdDelete")
            {
                this.StrSql = string.Concat(new object[] { "Delete from tdetfundamento where codregente = ", this.TxtCodregente.Text, " and Corr = ", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"], " and nus = ", TxtNus.Text });
                this.Util.EjecutaIns(this.StrSql);
                this.LblMensaje.Visible = true;
                this.LblMensaje.Text = "Fundamento Eliminado Exitosamente";
                this.GrdFundamento.Rebind();
            }
        }

        void BtnAddConFundamento_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Visible = false;
            if (this.TxtFundamento.Text == "")
            {
                this.LblMensaje.Text = "Debe Agregar un Fundamento";
                this.LblMensaje.Visible = true;
            }
            else
            {
                decimal num = this.Util.MaxCorr("Select Max(Corr) as Maximo from tdetfundamento where codregente = " + this.TxtCodregente.Text + " and nus = " + TxtNus.Text + "");
                this.StrSql = string.Concat(new object[] { "Insert into tdetfundamento values (", this.TxtCodregente.Text, ", ", num, ", '", this.TxtFundamento.Text, "', ", TxtNus.Text, ")" });
                this.Util.EjecutaIns(this.StrSql);
                this.GrdFundamento.Rebind();
                this.TxtFundamento.Text = "";
                this.LblMensaje.Text = "Fundamento agregado";
                this.LblMensaje.Visible = true;
            }
        }
    }
}