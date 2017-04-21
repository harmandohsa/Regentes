using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class ExpIncompletos : System.Web.UI.Page
    {
        private string StrSql = "";
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            GrdDetalle.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdDetalle_ItemCommand);
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
            GrdDetalle.Columns[9].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1500;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[9].Visible = false;
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }



        void GrdDetalle_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdDel")
            {
                StrSql = "delete from tsolicitud where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from ttrabajo where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from texperienciareg where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from texperiencia where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from tcurso where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from testudio where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from tsolicitud where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                StrSql = "delete from tregente where CodRegente = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"] + "";
                Util.EjecutaIns(StrSql);
                GrdDetalle.Rebind();
            }
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (Session["CodTipoUsuario"].ToString() == "0")
            {
                StrSql = "select CodRegente,ISNULL(nombres,'') + ' ' + ISNULL(apellidos,'') as  Nombre,CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CodId from TREGENTE " +
                     "where CODREGENTE not in (Select CODREGENTE from TENVIOSOL)";
            }
            else
            {
                string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
                StrSql = "select CodRegente,ISNULL(nombres,'') + ' ' + ISNULL(apellidos,'') as  Nombre,CodReg,CodregEMPF,CODregECUT,Correo,Telefono,Profesion,CodId from TREGENTE " +
                     "where Coddep in (Select CodDep from trelregiondep where codregion = " + str + ") and CODREGENTE not in (Select CODREGENTE from TENVIOSOL)";
            }
            
            Util.LlenaGrid(StrSql, GrdDetalle);
        }
    }
}