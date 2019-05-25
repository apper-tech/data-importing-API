using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataImporting
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((base.Request.QueryString.Count > 0) ? base.Request.QueryString.Keys[0] : "") == "logout")
            {
                this.Session["User"] = null;
                base.Response.Redirect("Login.aspx");
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (this.inputEmail.Text == "extractuser" && this.inputPassword.Text == "ExTr@ctUsr")
            {
                this.Session["User"] = true;
                base.Response.Redirect("ExtractForm.aspx");
            }
            else
            {
                base.Response.Redirect("Error.aspx?err=login_failed");
            }
        }
    }
}