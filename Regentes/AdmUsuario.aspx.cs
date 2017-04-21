using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class AdmUsuario : System.Web.UI.Page
    {
        private CUtilitarios Util;
        string StrSql;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            GrdDetalle.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdDetalle_ItemCommand);
            CboUsuario.TextChanged += new EventHandler(CboUsuario_TextChanged);
            GrdUsaurio.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdUsaurio_NeedDataSource);
            GrdUsaurio.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdUsaurio_ItemCommand);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);

            if (Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                Util.EstablecePermisos(base.Master, Convert.ToInt32(Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = Util.NomUsuario(int.Parse(Session["CodUsuario"].ToString()));
                Util.LlenaCombo("Select CodUsuario, isnull(nombres,'') + ' ' + isnull(apellidos,'')  + ' - ' + isnull(tipousuario,'') + ' - ' + isnull(nombre,'') as Nombre  from tusuario a, ttipousuario b, tregion c  where a.codtipousuario = b.codtipousuario and c.codregion = a.codregion", CboUsuario, "CodUsuario", "Nombre");
                if (Request.QueryString["llamada"].ToString() == "1")
                    Label1.Text = "Administración de Usuarios [Regentes]";
                else
                    Label1.Text = "Administración de Usuarios [Planes de Manejo]";
            }
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[6].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1000;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.Columns[6].Visible = false;
            GrdDetalle.ExportSettings.ExportOnlyData = true;
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = Label1.Text;
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }


        void GrdUsaurio_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CmdModificar")
            {
                TxtCodUsuario.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodUsuario"].ToString();
                LblUsuario.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Nombres"].ToString() + " " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Apellidos"].ToString();
                GrdDetalle.Rebind();
            }
                
        }

        void GrdUsaurio_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            StrSql = "select CodUsuario,Usuario,Nombres,Apellidos,TipoUsuario,a.CODTIPOUSUARIO,nombre from TUSUARIO a, TTIPOUSUARIO b, tregion c where a.codregion = c.codregion and a.CODTIPOUSUARIO = b.CODTIPOUSUARIO and CODUSUARIO > 1 and CodEstatus  = 1 order by CodEstatus,a.CodRegion,Nombres";
            Util.LlenaGrid(StrSql, GrdUsaurio);
        }

        void GrdDetalle_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            LblMensaje.Visible = false;
            if (e.CommandName == "CambiarRol")
            {
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codrol"].ToString() == "1")
                
                    StrSql = "Update tpermiso set codrol = 2 where Codusuario = " + TxtCodUsuario.Text + " and CodMenu = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codmenu"] + " and CodForma = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codforma"] + "";
                else
                    StrSql = "Update tpermiso set codrol = 1 where Codusuario = " + TxtCodUsuario.Text + " and CodMenu = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codmenu"] + " and CodForma = " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codforma"] + "";
                Util.EjecutaIns(StrSql);
                LblMensaje.Text = "Rol Actualizado";
                GrdDetalle.Rebind();
            }
        }

        void CboUsuario_TextChanged(object sender, EventArgs e)
        {
            if (CboUsuario.SelectedValue != "")
                GrdDetalle.Rebind();
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (TxtCodUsuario.Text != "")
            {
                StrSql = "Select a.codmenu,menu,a.codforma,nombre,a.codrol,rol,descripcion " +
                        "from tpermiso a, tmenu b, tforma c, trol d " +
                        "where a.codmenu = b.codmenu and c.codforma = a.codforma and c.codmenu = b.codmenu and d.codrol = a.codrol and a.CODSISTEMA = b.CODSISTEMA and codusuario = " + TxtCodUsuario.Text + " and c.codsistema = " + Request.QueryString["llamada"] + " ";
                Util.LlenaGrid(StrSql, GrdDetalle);
            }
            
        }
    }
}