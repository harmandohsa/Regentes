using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Regentes
{

    public partial class ListadoRegentes : System.Web.UI.Page
    {
        private string StrSql;
        CUtilitarios Util;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            GrdDetalle.NeedDataSource += new GridNeedDataSourceEventHandler(GrdDetalle_NeedDataSource);
            GrdDetalle.ItemDataBound += new GridItemEventHandler(GrdDetalle_ItemDataBound);
            if (!base.IsPostBack)
            {
                RadMenu menu = (RadMenu)base.Master.FindControl("Menu");
                menu.Visible = false;
                Label label = (Label)base.Master.FindControl("LblUsuario");
                label.Visible = false;
                ImageButton button = (ImageButton)base.Master.FindControl("ImgCerrarSesion");
                button.Visible = false;
            }
        }

        void GrdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                GridDataItem item = e.Item as GridDataItem;

                if (Util.ObtieneRegistro("Select * from tregente where codregente = " + item.GetDataKeyValue("codregente") + "", "VERCV").ToString() == "0")
                {

                    item["PrintCV"].FindControl("ImgPrintCV").Visible = false;
                }
                else
                {
                    item["PrintCV"].Attributes.Add("onclick", "javascript:url('" + item.GetDataKeyValue("codregente") + "' );");
                }
                    
            }
            
        }


       

        void GrdDetalle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            StrSql = "Select a.codregente,CODIGO,ISNULL(NOMBRES,'') + ' ' + ISNULL(apellidos,'') as nombre,CORREO,TELEFONO,PROFESION,a.DIRECCION,DEPARTAMENTO,MUNICIPIO,f.NOMBRE as region, " +
                         "(Select top 1 CONVERT(CHAR(11),FECVEN,103) from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc) as FECHAVEN " +
                         "from  TREGENTE a, TDEPARTAMENTO b, TMUNICIPIO c, TSOLICITUD d, TDICTAMENTEC e, TREGION f " +
                         "where a.CODDEP = b.CODDEP and c.CODMUN = a.CODMUN and c.CODDEP = b.CODDEP and d.CODREGENTE = a.CODREGENTE and d.CODTRAMITE = 1 and e.CODREGENTE = a.CODREGENTE " +
                         "and e.CODREGENTE = d.CODREGENTE and e.nus = d.NUS and f.CODREGION = e.CODREGION " +
                         "and a.CodEstatus = 1 and (Select top 1 FECVEN from TPERIODO g where g.CODREGENTE = a.CODREGENTE order by CORR desc)  > '" + string.Format("{0:MM/dd/yyyy}", Util.FechaDB()) + "'";
            Util.LlenaGrid(StrSql, GrdDetalle);
        }
    }
}