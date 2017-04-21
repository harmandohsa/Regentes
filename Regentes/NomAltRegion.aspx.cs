using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class NomAltRegion : System.Web.UI.Page
    {
        private string StrSql = "";
        private CUtilitarios Util;

        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            GrdDetalle.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdDetalle_ItemCommand);
            LnkGrabar.Click += new EventHandler(LnkGrabar_Click);
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
            GrdDetalle.Columns[3].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            //GrdDetalle.ExportSettings.Pdf.PageWidth = 2000;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[3].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void LnkGrabar_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (CodRegion.Text == "")
            {
                LblMensaje.Text = "Debe seleccionar una región";
                LblMensaje.Visible = true;
            }
            else
            {
                if (Util.ExisteDato("Select * from tnombre where CodRegion = " + CodRegion.Text + "") == true)
                    StrSql = "Update tnombre set nombre = '" + TxtNombre.Text + "' where codregion = " + CodRegion.Text + "";
                else
                    StrSql = "Insert into tnombre values (" + CodRegion.Text + ",'" + TxtNombre.Text + "')";
                Util.EjecutaIns(StrSql);
                LblMensaje.Text = "Datos Actualizados";
                LblMensaje.Visible = true;
                GrdDetalle.Rebind();
                TxtNombre.Text = "";
                CodRegion.Text = "";
            }
        }

        void GrdDetalle_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CmdModificar")
            {
                CodRegion.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codregion"].ToString();
                TxtNombre.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["alterno"].ToString();
            }
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            StrSql = "select a.nombre as alterno,b.NOMBRE as region,b.CODREGION as codregion from tnombre a right join TREGION b on a.codregion = b.CODREGION";
            Util.LlenaGrid(StrSql, GrdDetalle);
        }
    }
}