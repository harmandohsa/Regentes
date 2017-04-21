using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class ActDesRegente : System.Web.UI.Page
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

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
            GrdDetalle.Columns[21].Visible = false;
            GrdDetalle.Columns[22].Visible = false;
            GrdDetalle.Columns[23].Visible = false;
            GrdDetalle.Columns[24].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2000;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
            GrdDetalle.Columns[21].Visible = false;
            GrdDetalle.Columns[22].Visible = false;
            GrdDetalle.Columns[23].Visible = false;
            GrdDetalle.Columns[24].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("ProcesaSecretaria.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"].ToString() + "&nus=" + TxtNus.Text + "");
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepIngresoExp.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"].ToString() + "&nus=" + TxtNus.Text + "");
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
                if (item.GetDataKeyValue("codigo").ToString() != "")
                {
                    item["nusfor"].Text = "RRAP - " + Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                    string MaxPeriodo = Util.ObtieneRegistro("Select Max(Corr) as Max from tperiodo where codregente = " + item.GetDataKeyValue("CodRegente") + "", "Max").ToString();
                    item["FecRecep"].Text = Util.ObtieneRegistro("Select CONVERT(CHAR(11),fecaut,103) as fecaut from tperiodo where codregente = " + item.GetDataKeyValue("CodRegente") + " and corr = " + MaxPeriodo + "", "fecaut").ToString();
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
                    if (Util.ObtieneRegistro("Select * from tdictamentec where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "CodRecomendacion").ToString() == "1")
                        item["PrintCons"].FindControl("ImgPrintCons").Visible = true;
                    if (item.GetDataKeyValue("codest").ToString() == "1")
                    {
                        item["EstUsr"].FindControl("ImgAct").Visible = false;
                        item["EstUsr"].FindControl("ImgDown").Visible = true;
                    }
                    else
                    {
                        item["EstUsr"].FindControl("ImgAct").Visible = true;
                        item["EstUsr"].FindControl("ImgDown").Visible = false;
                    }
                    item["FecRecep"].Text = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FECRECIBE,103) as FECRECIBE from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FECRECIBE").ToString();
                    item["Del"].Text = Util.ObtieneRegistro("select NOMBRE from TREGENTE a, TRELREGIONDEP b, TREGION	c where a.CODDEP = b.CODDEP and c.CODREGION = b.CODREGION and CodRegente = " + item.GetDataKeyValue("CodRegente") + "", "Nombre").ToString(); 
                }
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
                Label3.Visible = true;
            }
            else if (e.CommandName == "Activar")
            {
                this.StrSql = "Update tregente set CodEstatus = 1  where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                if (this.Util.EjecutaIns(this.StrSql))
                {
                    this.LblMensaje.Text = "Regente Forestal Activado";
                    this.LblMensaje.Visible = true;
                }
                this.GrdDetalle.Rebind();
            }
            else if (e.CommandName == "Desactivar")
            {
                this.StrSql = "Update tregente set CodEstatus = 0 where codregente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                if (this.Util.EjecutaIns(this.StrSql))
                {
                    this.LblMensaje.Text = "Regente Forestal Desactivado";
                    this.LblMensaje.Visible = true;
                }
                this.GrdDetalle.Rebind();
            }
        }




        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            if (Session["CodTipoUsuario"].ToString() == "0")
                this.StrSql = "select a.CodRegente,tramite as tipotramite, e.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,e.codestatus,e.nus,recomendacion,codigo,a.codestatus as codest,g.categoria  from tregente a, testatus b, tdictamentec c, trecomendacin d, tsolicitud e, ttipotramite f, tcategoria g  where e.codregente = a.codregente  and e.NUS = c.nus and  f.CODTRAMITE = e.codtramite and d.codrecomendacion = c.codrecomendacion and c.codregente = a.codregente and e.codestatus = 6 and e.codestatus = b.codestatus and a.codcategoria = g.codcategoria and e.codtramite = 1 order by e.codestatus desc ";
            else
                this.StrSql = "select a.CodRegente,tramite as tipotramite, e.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,e.codestatus,e.nus,recomendacion,codigo,a.codestatus as codest,g.categoria  from tregente a, testatus b, tdictamentec c, trecomendacin d, tsolicitud e, ttipotramite f, tcategoria g  where e.codregente = a.codregente  and e.NUS = c.nus and  f.CODTRAMITE = e.codtramite and d.codrecomendacion = c.codrecomendacion and c.codregente = a.codregente and e.codestatus = 6 and e.codestatus = b.codestatus and a.codcategoria = g.codcategoria and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") and e.codtramite = 1 order by e.codestatus desc ";
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}