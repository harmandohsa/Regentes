using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class ListadoReg : System.Web.UI.Page
    {

        CUtilitarios Util;
        string StrSql;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            BtnGenerar.Click += new EventHandler(BtnGenerar_Click);
            CboDep.TextChanged += new EventHandler(CboDep_TextChanged);
            GrdDetalle.ItemDataBound += new GridItemEventHandler(GrdDetalle_ItemDataBound);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                Util.LlenaCombo("Select * from tdepartamento order by departamento", this.CboDep, "CodDep", "Departamento");
                //Util.LlenaCombo("Select * from tmunicipio where coddep = 1", this.CboMun, "CodMun", "Municipio");
                Util.LlenaCombo("Select * from tregion", CboDelegacion, "CodRegion", "Nombre");
                Util.LlenaCombo("Select * from tcategoria where codcategoria in (1,2)", CboCat, "CodCategoria", "Categoria");
                if (Session["CodTipoUsuario"].ToString() != "0")
                {
                    Session["Genero"] = false;
                    //CboDelegacion.Enabled = false;
                    //CboDelegacion.SelectedValue = Util.ObtieneRegistro("Select * from tusuario where codusuario = " + Session["CodUsuario"] + "", "CodRegion").ToString();
                }
            }
        }

        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                item["nusfor"].Text = "RFAP - " + Convert.ToInt32(item.GetDataKeyValue("CODIGO")).ToString("D8");
                item["Est"].Text = CboEstatus.Text;
            }
        }

        void BtnGenerar_Click(object sender, EventArgs e)
        {
            Session["Genero"] = true;
            GrdDetalle.Rebind();
        }

        protected void imgbExport_Click(object sender, ImageClickEventArgs e)
        {
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Listado de Regentes";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.MasterTableView.ExportToExcel();
        }

        protected void ExpXls_Click(object sender, ImageClickEventArgs e)
        {
           GrdDetalle.ExportSettings.ExportOnlyData = true;GrdDetalle.ExportSettings.IgnorePaging = true;
            GrdDetalle.ExportSettings.FileName = "Listado de Regentes";
            GrdDetalle.ExportSettings.OpenInNewWindow = true;
            GrdDetalle.ExportSettings.Pdf.PageWidth = 1500;
            GrdDetalle.MasterTableView.ExportToPdf();
        }

        void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Convert.ToBoolean(Session["Genero"]) == true)
            {
                StrSql = "Select CODIGO,ISNULL(NOMBRES,'') + ' ' + ISNULL(apellidos,'') as nombre,CORREO,TELEFONO,PROFESION,a.DIRECCION,DEPARTAMENTO,MUNICIPIO,f.NOMBRE as region, " +
                         "(Select top 1 CONVERT(CHAR(11),FECVEN,103) from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc) as FECHAVEN,g.categoria " +
                         "from  TREGENTE a, TDEPARTAMENTO b, TMUNICIPIO c, TSOLICITUD d, TDICTAMENTEC e, TREGION f, tcategoria g " +
                         "where a.CODDEP = b.CODDEP and c.CODMUN = a.CODMUN and c.CODDEP = b.CODDEP and d.CODREGENTE = a.CODREGENTE and d.CODTRAMITE = 1 and e.CODREGENTE = a.CODREGENTE " +
                         "and e.CODREGENTE = d.CODREGENTE and e.nus = d.NUS and f.CODREGION = e.CODREGION and g.codcategoria = a.codcategoria ";
                if (TxtNoReg.Text != "")
                    StrSql = StrSql + " and Codigo like '%" + TxtNoReg.Text + "%'";
                if (TxtNombres.Text != "")
                    StrSql = StrSql + " and nombres like '%" + TxtNombres.Text + "%'";
                if (TxtApellidos.Text != "")
                    StrSql = StrSql + " and apellidos like '%" + TxtApellidos.Text + "%'";
                if (TxtProfesion.Text != "")
                    StrSql = StrSql + " and profesion like '%" + TxtProfesion.Text + "%'";
                if (CboDep.SelectedValue != "")
                    StrSql = StrSql + " and a.CodDep = " + CboDep.SelectedValue + "";
                if (CboMun.SelectedValue != "")
                    StrSql = StrSql + " and a.CodMun = " + CboMun.SelectedValue + "";
                if (CboDelegacion.SelectedValue != "")
                    StrSql = StrSql + " and e.CodRegion = " + CboDelegacion.SelectedValue + "";
                if (CboEstatus.SelectedValue == "1")
                    StrSql = StrSql + " and a.CodEstatus = 1 and (Select top 1 FECVEN from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc)  > '" + string.Format("{0:MM/dd/yyyy}", Util.FechaDB()) + "'";
                if (CboEstatus.SelectedValue == "2")
                    StrSql = StrSql + " and (Select top 1 FECVEN from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc) < '" + string.Format("{0:MM/dd/yyyy}", Util.FechaDB()) + "'";
                if (CboEstatus.SelectedValue == "3")
                    StrSql = StrSql + " and a.CodEstatus = 0";
                if (CboCat.SelectedValue != "")
                    StrSql = StrSql + " and a.CodCategoria = " + CboCat.SelectedValue + "";
                Util.LlenaGrid(StrSql, GrdDetalle);
            }

        }

        void CboDep_TextChanged(object sender, EventArgs e)
        {
            if (this.CboDep.SelectedValue != "")
            {
                this.CboMun.Items.Clear();
                this.Util.LlenaCombo("Select * from tmunicipio where coddep = " + this.CboDep.SelectedValue, this.CboMun, "CodMun", "Municipio");
            }
        }
    }
}