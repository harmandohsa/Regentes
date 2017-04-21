using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{
    public partial class HisDatPer : System.Web.UI.Page
    {
        CUtilitarios Util;
        string StrSql;
        int CodRegion = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            GrdDetalle.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(GrdDetalle_ItemCommand);
            Grdhistoria.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(Grdhistoria_NeedDataSource);
            Grdhistoria.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(Grdhistoria_ItemDataBound);
            if (this.Session["CodUsuario"] == null)
            {
                base.Response.Redirect("logon.aspx");
            }
            else if (!base.IsPostBack)
            {
                this.Util.EstablecePermisos(base.Master, Convert.ToInt32(this.Session["CodUsuario"]));
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Text = this.Util.NomUsuario(int.Parse(this.Session["CodUsuario"].ToString()));
                
                if (Session["CodTipoUsuario"].ToString() != "0")
                {
                    CodRegion = Convert.ToInt32(Util.ObtieneRegistro("Select * from tusuario where codusuario = " + Session["CodUsuario"] + "", "CodRegion"));
                }
            }
        }

        void Grdhistoria_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item["Print"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("CodRegente") + "','" + item.GetDataKeyValue("Corr") + "' );");
            }
            
        }

        void Grdhistoria_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (TxtCodRegente.Text != "")
            {
                StrSql = "Select CodRegente,Corr from THISTREGENTE where CodRegente = " + TxtCodRegente.Text + "";
                Util.LlenaGrid(StrSql, Grdhistoria);
            }
        }

        void GrdDetalle_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "CmdHistoria")
            {
                TxtCodRegente.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CodRegente"].ToString();
                Grdhistoria.Rebind();
            }
        }

        void GrdDetalle_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select CODIGO,ISNULL(NOMBRES,'') + ' ' + ISNULL(apellidos,'') as nombre,CORREO,TELEFONO,PROFESION,A.DIRECCION,DEPARTAMENTO,MUNICIPIO,f.NOMBRE as region, " +
                         "(Select top 1 CONVERT(CHAR(11),FECVEN,103) from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc) as FECHAVEN,a.codregente " +
                         "from  TREGENTE a, TDEPARTAMENTO b, TMUNICIPIO c, TSOLICITUD d, TDICTAMENTEC e, TREGION f " +
                         "where a.CODDEP = b.CODDEP and c.CODMUN = a.CODMUN and c.CODDEP = b.CODDEP and d.CODREGENTE = a.CODREGENTE and d.CODTRAMITE = 1 and e.CODREGENTE = a.CODREGENTE " +
                         "and e.CODREGENTE = d.CODREGENTE and e.nus = d.NUS and f.CODREGION = e.CODREGION ";
            if (CodRegion != 0)
                StrSql = StrSql + " and e.CodRegion = " + CodRegion + "";
            Util.LlenaGrid(StrSql, GrdDetalle);
        }
    }
}