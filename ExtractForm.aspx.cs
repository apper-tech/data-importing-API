using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataImporting
{
    public partial class ExtractForm : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.Session["User"] != null && (bool)this.Session["User"])
            {
                return;
            }
            base.Response.Redirect("login.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                StaticCalls.InitVars();
            }
        }
    }
}