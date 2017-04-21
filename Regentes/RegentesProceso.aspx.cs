using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class RegentesProceso : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            Grdhistoria.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            GrdDetalle.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(GrdDetalle_ItemDataBound);
            GrdDetalle.ItemCommand += new GridCommandEventHandler(GrdDetalle_ItemCommand);
            GrdIngreso.NeedDataSource += new GridNeedDataSourceEventHandler(GrdIngreso_NeedDataSource);
            GrdIngreso.ItemDataBound += new GridItemEventHandler(GrdIngreso_ItemDataBound);
            GrdIngreso.ItemCommand += new GridCommandEventHandler(GrdIngreso_ItemCommand);
            Grdhistoria.ItemCommand += new GridCommandEventHandler(Grdhistoria_ItemCommand);
            Grdhistoria.ItemDataBound += new GridItemEventHandler(Grdhistoria_ItemDataBound);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);

            if (Session["CodUsuario"] == null)
            {
                Response.Redirect("logon.aspx");
            }
            else if (!IsPostBack)
            {
                Util.EstablecePermisos(Master, Convert.ToInt32(Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = Util.NomUsuario(int.Parse(Session["CodUsuario"].ToString()));
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[13].Visible = false;
            GrdDetalle.Columns[14].Visible = false;
            GrdDetalle.Columns[15].Visible = false;
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1600;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[13].Visible = false;
            GrdDetalle.Columns[14].Visible = false;
            GrdDetalle.Columns[15].Visible = false;
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void Grdhistoria_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["Print"].Attributes.Add("onclick", "javascript:url8('" + item.GetDataKeyValue("codregente") + "','" + item.GetDataKeyValue("corr") + "','" + TxtNus.Text + "' );");
            }
        }

        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                Response.Redirect("SolicitaEnmienda.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
        }

        void GrdIngreso_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                Response.Redirect("ProcesaSecretaria.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Corr"].ToString() + "&nus=" + TxtNus.Text);
            }
        }

        void GrdIngreso_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["Print"].Attributes.Add("onclick", "javascript:url6('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("Corr") + "', '" + TxtNus.Text + "' );");
            }
        }

        void GrdIngreso_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),a.FecRecibe,3) as FecRecibe from TRECIBESECRE a where  codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text;
                Util.LlenaGrid(StrSql, GrdIngreso);
            }
        }

        void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                Grdhistoria.Rebind();
                TblHist.Visible = true;
                Grdhistoria.Visible = true;
                Label3.Visible = true;
                TblIngreso.Visible = false;
            }
            else if (e.CommandName == "CmdIngreso")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                GrdIngreso.Rebind();
                TblHist.Visible = false;
                Grdhistoria.Visible = false;
                Label3.Visible = true;
                Label3.Text = "Historial de Ingreso";
                TblIngreso.Visible = true;
            }
        }

        void GrdDetalle_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " +  Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                item["Solicitud"].Attributes.Add("onclick", "javascript:url3('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") +"','" + item.GetDataKeyValue("codtramite") + "' );");
                item["CV"].Attributes.Add("onclick", "javascript:url4('" + item.GetDataKeyValue("CodRegente") + "' );");
                if (Util.ExisteDato("Select * from tdictamentec where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus =  " + item.GetDataKeyValue("CodRegente") + ""))
                    item["Print"].FindControl("ImgPrint").Visible = true;
                else
                    item["Print"].FindControl("ImgPrint").Visible = false;
                if (Util.ExisteDato("Select * from tdictamenleg where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus =  " + item.GetDataKeyValue("CodRegente") + ""))
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = true;
                else
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = false;
                if (Util.ExisteDato("Select * from tresolucion where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus =  " + item.GetDataKeyValue("CodRegente") + ""))
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = true;
                else
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = false;
                if (Util.ExisteDato("Select * from trecibesecre where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus =  " + item.GetDataKeyValue("CodRegente") + ""))
                    item["Recep"].FindControl("ImgRecep").Visible = true;
                else
                    item["Recep"].FindControl("ImgRecep").Visible = false;
                if (Util.ExisteDato("Select * from tenmiendatec where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus")  + ""))
                    item["Histori"].FindControl("Imghistory").Visible = true;
                else
                    item["Histori"].FindControl("Imghistory").Visible = false;
                item["Print"].Attributes.Add("onclick", "javascript:url5('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintJur"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintRes"].Attributes.Add("onclick", "javascript:url2('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["Del"].Text = Util.ObtieneRegistro("select NOMBRE from TREGENTE a, TRELREGIONDEP b, TREGION	c where a.CODDEP = b.CODDEP and c.CODREGION = b.CODREGION and CodRegente = " + item.GetDataKeyValue("CodRegente") + "", "Nombre").ToString();
            }
        }

        void Grdhistoria_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendatec where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text;
                Util.LlenaGrid(StrSql, Grdhistoria);
            }
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where  a.codestatus < 6 and a.codestatus = b.codestatus order by FecCreacion desc,a.codestatus desc ";
            StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus between 0 and 5 and c.codestatus = b.codestatus AND A.CODESTATUS = 1 and a.CODREGENTE in (Select CODREGENTE from TENVIOSOL) order by c.codestatus desc ";
            Util.LlenaGrid(StrSql, GrdDetalle);
        }
    }
}