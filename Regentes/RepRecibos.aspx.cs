using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class RepRecibos : System.Web.UI.Page
    {
        private string StrSql;
        private CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            ImgExpPdf.Click += new ImageClickEventHandler(ImgExpPdf_Click);
            ImgExpExl.Click += new ImageClickEventHandler(ImgExpExl_Click);
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

        void ImgExpExl_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.ExportSettings.ExportOnlyData = true; 
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Reporte de Recibos de Ingresos Varios";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }

        void ImgExpPdf_Click(object sender, ImageClickEventArgs e)
        {
            GrdDetalle.ExportSettings.ExportOnlyData = true; 
            GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Reporte de Recibos de Ingresos Varios";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1150;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            string str = this.Util.ObtieneRegistro("Select * from tusuario where codusuario = " + this.Session["CodUsuario"], "CodRegion").ToString();
            StrSql = "select a.CODREGENTE,TRAMITE,ISNULL(d.nombres,'') + ' ' + isnull(d.apellidos,'') as nombre,CONVERT(CHAR(11),FECHARECIBO,103) as FECHARECIBO ,reciboing,RECOMENDACION,BOLBANRURAL " +
                     "from TDICTAMENTEC a, TSOLICITUD b, TTIPOTRAMITE c, TREGENTE d, TRECOMENDACIN e " +
                     "where a.nus = b.NUS and c.CODTRAMITE = b.CODTRAMITE and d.CODREGENTE = a.CODREGENTE and e.CODRECOMENDACION = a.CODRECOMENDACION and b.CODESTATUS > 0 and Coddep in (Select CodDep from trelregiondep where codregion = " + str + ")";
            Util.LlenaGrid(this.StrSql, this.GrdDetalle);
        }
    }
}