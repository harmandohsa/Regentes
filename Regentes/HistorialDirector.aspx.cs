using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class HistorialDirector : System.Web.UI.Page
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
            GrdEnmiendaLeg.NeedDataSource += new GridNeedDataSourceEventHandler(GrdEnmiendaLeg_NeedDataSource);
            GrdEnmiendaLeg.ItemCommand += new GridCommandEventHandler(GrdEnmiendaLeg_ItemCommand);
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
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 2150;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[16].Visible = false;
            GrdDetalle.Columns[17].Visible = false;
            GrdDetalle.Columns[18].Visible = false;
            GrdDetalle.Columns[19].Visible = false;
            GrdDetalle.Columns[20].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void GrdEnmiendaLeg_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmiendaLeg.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepEnmiendaLeg.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
        }

        void GrdEnmiendaLeg_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendaleg where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text;
                Util.LlenaGrid(StrSql, GrdEnmiendaLeg);
            }
        }

        void Grdhistoria_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                base.Response.Redirect("SolicitaEnmienda.aspx?llamada=1&CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() + "&nus=" + TxtNus.Text);
            }
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepEnmiendaTec.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregente"].ToString() + "&Corr=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["corr"].ToString() +"&nus=" + TxtNus.Text);
            }
        }

        void Grdhistoria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "select codregente,corr,CONVERT(CHAR(11),FeccRe,3) as FecRecibe, " +
                         "case when codestatus = 1 then 'Abierta' else 'Cerrada' end as estatus " +
                         "from tenmiendatec where codregente = " + TxtCodRegente.Text + " and nus = " + TxtNus.Text;
                Util.LlenaGrid(StrSql, Grdhistoria);
            }
        }

        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["nusfor"].Text = "RRAP - " +  Convert.ToInt32(item.GetDataKeyValue("nus")).ToString("D8");
                if (Convert.ToInt32(Util.ObtieneRegistro("Select * from tsolicitud where codregente = " + item.GetDataKeyValue("CodRegente") + " and nus =  " + item.GetDataKeyValue("nus") + "", "CodEstatus").ToString()) > 5)
                {
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = true;
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = true;
                }
                if (item.GetDataKeyValue("codtramite").ToString() == "2")
                {
                    item["PrintJur"].FindControl("ImgPrintJur").Visible = false;
                    item["PrintRes"].FindControl("ImgPrintRes").Visible = false;
                }
                item["PrintJur"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["PrintRes"].Attributes.Add("onclick", "javascript:url2('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("nus") + "' );");
                item["FecRecep"].Text = Util.ObtieneRegistro("Select CONVERT(CHAR(11),FECRECIBE,103) as FECRECIBE from TRECIBESECRE where CodRegente = " + item.GetDataKeyValue("CodRegente") + " and nus = " + item.GetDataKeyValue("nus") + "", "FECRECIBE").ToString();
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
            }
        }

        private void AbreVentana(string Ventana)
        {
            string script = "<script>window.open('" + Ventana + "')</script>";
            this.RegisterStartupScript("WOpen", script);
        }

        private void GrdDetalle_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdPrint")
            {
                this.AbreVentana("RepDictamenTec.aspx?CodRegente=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "&nus=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"]);
            }
           
            else if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                TxtNus.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["nus"].ToString();
                Grdhistoria.Rebind();
                TblHist.Visible = true;
                Grdhistoria.Visible = true;
                Label3.Visible = true;
                GrdEnmiendaLeg.Rebind();
            }
        }

        


        private void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            //this.StrSql = "select CodRegente, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,a.codestatus,nus  from tregente a, testatus b where  a.codestatus > 3 and a.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")  order by a.codestatus desc ";
            if (Util.ObtieneRegistro("Select * from tusuario where codusuario = " + Session["CodUsuario"] + "","CodTipoUsuario").ToString() == "5")
                this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus > 3 and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") and c.codtramite not in (2) AND A.CODESTATUS = 1 order by c.codestatus desc ";
            else
                this.StrSql = "select a.CodRegente,tramite as tipotramite, c.codtramite, isnull(nombres,'') + ' ' + isnull(apellidos,'') as Nombre, CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CONVERT(CHAR(11),a.FecCreacion,3) as FecCreacion,Estatus,c.codestatus,nus  from tregente a, testatus b, tsolicitud c, ttipotramite d where c.codregente = a.codregente and  d.CODTRAMITE = c.codtramite and c.codestatus > 3 and c.codestatus = b.codestatus and  Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") AND A.CODESTATUS = 1 order by c.codestatus desc ";
            
            this.Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}