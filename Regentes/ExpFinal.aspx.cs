using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class ExpFinal : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            this.GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(this.GrdDetalle_NeedDataSource);
            this.GrdDetalle.ItemCommand += new GridCommandEventHandler(this.GrdDetalle_ItemCommand);
            GrdDetalle.ItemDataBound += new GridItemEventHandler(GrdDetalle_ItemDataBound);
            Grdhistoria.NeedDataSource += new GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            Grdhistoria.ItemCommand += new GridCommandEventHandler(Grdhistoria_ItemCommand);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);
            GrdEnmiendasJur.NeedDataSource += new GridNeedDataSourceEventHandler(GrdEnmiendasJur_NeedDataSource);
            GrdEnmiendasJur.ItemCommand += new GridCommandEventHandler(GrdEnmiendasJur_ItemCommand);
            
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
            }
        }

        void GrdEnmiendasJur_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmiendaLeg.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
            if (e.CommandName == "CmdPrint")
            {
                Response.Redirect("RepEnmiendaLeg.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
        }

        void GrdEnmiendasJur_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendaleg where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, GrdEnmiendasJur);
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
            GrdDetalle.Columns[21].Visible = false;
            GrdDetalle.Columns[22].Visible = false;
            GrdDetalle.Columns[23].Visible = false;
            GrdDetalle.Columns[24].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Expedientes Finalizados";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2450;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
            GrdDetalle.Columns[21].Visible = false;
            GrdDetalle.Columns[22].Visible = false;
            GrdDetalle.Columns[23].Visible = false;
            GrdDetalle.Columns[24].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Expedientes Finalizados";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmienda.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
            if (e.CommandName == "CmdPrint")
            {
                Response.Redirect("RepEnmiendaTec.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
        }

        void Grdhistoria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendatec where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text + "";
                Util.LlenaGrid(StrSql, Grdhistoria);
            }
        }

        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " + Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                if (Convert.ToInt32(Util.ObtieneRegistro("Select * from tregente a, tsolicitud b where a.codregente = b.codregente and nus = " + item.GetDataKeyValue("nus") + " and  a.codregente = " + item.GetDataKeyValue("CodRegente") + "", "CodEstatus").ToString()) > 5)
                {
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = true;
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = true;
                }
                if (item.GetDataKeyValue("codtramite").ToString() == "2")
                {
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = false;
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = false;
                }
                else
                {
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = true;
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = true;
                }
                item["PrintJur"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintRes"].Attributes.Add("onclick", "javascript:url2('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["Print"].Attributes.Add("onclick", "javascript:url3('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintCons"].Attributes.Add("onclick", "javascript:url4('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintSol"].Attributes.Add("onclick", "javascript:url7('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "','" + item.GetDataKeyValue("codtramite") + "' );");
                item["PrintCV"].Attributes.Add("onclick", "javascript:url8('" + item.GetDataKeyValue("CodRegente") + "' );");
                if (Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " +  item.GetDataKeyValue("nus") + "","CodRecomendacion").ToString() == "1")
                    item["PrintCons"].FindControl("ImgPrintCons").Visible = true;
                item["FecRecep"].Text = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FECRECIBE,103) as FECRECIBE from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FECRECIBE").ToString();

                item["Del"].Text = Util.ObtieneRegistro("select NOMBRE from TREGENTE a, TRELREGIONDEP b, TREGION	c where a.CODDEP = b.CODDEP and c.CODREGION = b.CODREGION and CodRegente = " + item.GetDataKeyValue("CodRegente") + "", "Nombre").ToString(); 
                if (Util.ObtieneRegistro("Select * from tsolicitud where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "CodEstatus").ToString() == "6")
                {
                    Int32 dias = Convert.ToInt32(Math.Truncate(Util.CalculaDias(Convert.ToDateTime(Util.ObtieneRegistro("Select * from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecRecibe")), Convert.ToDateTime(Util.ObtieneRegistro("Select * from TSOLICITUD where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecFin")))));
                    if (dias == 0)
                        item["dia"].Text = "0";
                    else
                        item["dia"].Text = (dias).ToString();

                }
                else
                {
                    Int32 dias2 = Convert.ToInt32(Math.Truncate(Util.CalculaDias(Convert.ToDateTime(Util.ObtieneRegistro("Select * from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FecRecibe")), Util.FechaDB())));
                    if (dias2 == 0)
                        item["dia"].Text = "0";
                    else
                        item["dia"].Text = (dias2).ToString();
                }
                if (item.GetDataKeyValue("codestatus").ToString() == "-1")
                {
                    item["est"].Text = Util.ObtieneRegistro("Select * from tresolucion a, TRECOMENDACIN b where a.codrecomendacion = b.codrecomendacion and a.codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "Recomendacion").ToString();
                }
                //if ((item.GetDataKeyValue("codtramite").ToString() == "1") || (item.GetDataKeyValue("codtramite").ToString() == "4"))
                //{
                    
                //}

            }
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                Grdhistoria.Rebind();
                TblHist.Visible = true;
                Grdhistoria.Visible = true;
                GrdEnmiendasJur.Visible = true;
                GrdEnmiendasJur.Rebind();
                Label3.Visible = true;
            }
        }




        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Request.QueryString["llamada"].ToString() == "1")
            {
                string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
                this.StrSql = "select a.CodRegente,tramite as tipotramite, e.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,e.codestatus,e.nus,recomendacion  from tregente a, testatus b, tdictamentec c, trecomendacin d, tsolicitud e, ttipotramite f  where e.codregente = a.codregente  and e.NUS = c.nus and  f.CODTRAMITE = e.codtramite and d.codrecomendacion = c.codrecomendacion and c.codregente = a.codregente and e.codestatus in (6,-1) and e.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by e.codestatus desc ";
            }
            else
                this.StrSql = "select a.CodRegente,tramite as tipotramite, e.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,e.codestatus,e.nus,recomendacion  from tregente a, testatus b, tdictamentec c, trecomendacin d, tsolicitud e, ttipotramite f  where e.codregente = a.codregente  and e.NUS = c.nus and  f.CODTRAMITE = e.codtramite and d.codrecomendacion = c.codrecomendacion and c.codregente = a.codregente and e.codestatus in (6,-1) and e.codestatus = b.codestatus AND A.CODESTATUS = 1 order by e.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}