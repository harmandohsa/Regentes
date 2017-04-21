using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Regentes
{

    public partial class ValidaRegente : System.Web.UI.Page
    {
        CUtilitarios Util;
        string StrSql;
        protected void Page_Load(object sender, EventArgs e)
        {
            Util = new CUtilitarios();
            BtnValida.Click += new EventHandler(BtnValida_Click);
        }

        void BtnValida_Click(object sender, EventArgs e)
        {
            LblMensaje.Visible = false;
            if (TxtIdUnico.Text == "")
            {
                LblMensaje.Text = "Debe Ingresar el texto único";
                LblMensaje.Visible = true;
                TxtIdUnico.Focus();
            }
            else if (Util.ExisteDato("Select * from tperiodo where idunico = '" + TxtIdUnico.Text + "'") == false)
            {
                LblMensaje.Text = "Este texto único no existe";
                LblMensaje.Visible = true;
                TxtIdUnico.Focus();
                TxtIdUnico.Text = "";
            }
            else
            {
                string Corr = Util.ObtieneRegistro("Select Max(Corr) as Max from tperiodo where idunico = '" + TxtIdUnico.Text + "'", "Max").ToString();
                DateTime FecVen = Convert.ToDateTime(Util.ObtieneRegistro("Select FecVen from tperiodo where corr = " + Corr + " and idunico = '" + TxtIdUnico.Text + "'", "FecVen").ToString());
                if (Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(FecVen))
                {
                    string CodRegente = Util.ObtieneRegistro("Select CodRegente from tperiodo where idunico = '" + TxtIdUnico.Text + "'", "CodRegente").ToString();
                    if (Util.ObtieneRegistro("Select * from tregente where codregente = " + CodRegente + "", "CodEstatus").ToString() == "0")
                    {
                        LblMensaje.Text = "Su estatus como regente fue suspendido.";
                        LblMensaje.Visible = true;
                    }
                    else
                    {
                        if (Util.ExisteDato("Select * from tsolicitud where CodRegente = " + CodRegente + " and codestatus in (1,2,3,4,5)") == true)
                        {
                            LblMensaje.Text = "Actualmente tiene un tramite abierto de actualización de datos en CONAP, no puede enviar otro hasta terminar el anterior.";
                            LblMensaje.Visible = true;
                        }
                        else
                            Response.Redirect("ActRegente.aspx?idunico=" + TxtIdUnico.Text + "");
                    }
                    
                }
                else
                {
                    LblMensaje.Text = "Su estatus como regente se encuestra desactivado.";
                    LblMensaje.Visible = true;
                    TxtIdUnico.Focus();
                }
            }
        }
    }
}