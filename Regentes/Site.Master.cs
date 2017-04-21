using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ImgCerrarSesion.Click += new ImageClickEventHandler(this.ImgCerrarSesion_Click);
        }

        private void ImgCerrarSesion_Click(object sender, ImageClickEventArgs e)
        {
            base.Session["CodUsuario"] = null;
            base.Session["CodTipoUsuario"] = null;
            base.Response.Redirect("Logon.aspx");
        }
    }
}
