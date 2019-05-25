using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataImporting
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString[0] == "login_failed")
            {
                this.error.Text = "Login Failed!";
                this.retru.Text = "Return to login";
                this.retru.NavigateUrl = "login.aspx";
            }
        }
    }
}