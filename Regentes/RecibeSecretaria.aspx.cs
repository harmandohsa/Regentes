using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class RecibeSecretaria : System.Web.UI.Page
    {
        private string StrSql = "";
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemDataBound += new GridItemEventHandler(this.GrdDetalle_ItemDataBound);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            this.Grdhistoria.NeedDataSource += new GridNeedDataSourceEventHandler(this.Grdhistoria_NeedDataSource);
            this.Grdhistoria.ItemCommand += new GridCommandEventHandler(this.Grdhistoria_ItemCommand);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                if (Request.QueryString["llamada"] == "")
                { }
                else if (Request.QueryString["llamada"] == "1")
                {
                    this.AbreVentana("RepIngresoExp.aspx?CodRegente=" + Request.QueryString["CodRegente"] + "&Corr=" + Request.QueryString["Corr"] + "&nus=" + Request.QueryString["nus"] + "");
                }
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[15].Visible = false;
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Recepción de expedientes";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2000;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[15].Visible = false;
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Recepción de expedientes";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdProcesar")
            {
                //Session["CodTramite"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString();
                base.Response.Redirect("ProcesaSecretaria.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&nus= " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString() + "");
            }
            else if (e.CommandName == "CmdHistoria")
            {
                //Session["CodTramite"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString();
                this.TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                this.Grdhistoria.Rebind();
            }
            else if (e.CommandName == "CmdBaja")
            {
                //Session["CodTramite"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString();
                this.TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codtramite"].ToString() == "1")
                {
                    StrSql = "Update tregente set codestatus = 0 where codregente = " + TxtCodRegente.Text + "";
                    Util.EjecutaIns(StrSql);
                    StrSql = "Update tsolicitud set codestatus = -2 where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                    Util.EjecutaIns(StrSql);
                }
                else
                {
                    StrSql = "Update tsolicitud set codestatus = -2 where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                    Util.EjecutaIns(StrSql);
                }
                
                this.Grdhistoria.Rebind();
                GrdDetalle.Rebind();
            }
        }

        private void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " + Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                item["Print"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "','" + item.GetDataKeyValue("codtramite") + "' );");
                item["PrintCV"].Attributes.Add("onclick", "javascript:url2('" + item.GetDataKeyValue("CodRegente") + "' );");
                item["dia"].Text = Convert.ToInt32(Math.Truncate(Util.Dias(item.GetDataKeyValue("CodRegente").ToString(), item.GetDataKeyValue("nus").ToString()))).ToString();
            }
        }

        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            StrSql = "select a.CodRegente, tramite as tipotramite,c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),c.FecTramite,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") and c.codestatus = 1 AND A.CODESTATUS = 1  order by c.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }

        private void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("ProcesaSecretaria.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"].ToString() + "&nus="+ TxtNus.Text + "");
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepIngresoExp.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"].ToString() + "&nus="+ TxtNus.Text + "");
            }
        }

        private void Grdhistoria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.TxtCodRegente.Text != "")
            {
                this.StrSql = "select codregente,corr,CONVERT(CHAR(11),a.FecRecibe,3) as FecRecibe from TRECIBESECRE a where  codregente = " + this.TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                this.Util.LlenaGrid(this.StrSql, this.Grdhistoria);
            }
        }
    }
}