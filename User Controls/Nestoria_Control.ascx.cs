using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataImporting.User_Controls
{
    public partial class Nestoria_Control : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IScheduler defaultScheduler = StdSchedulerFactory.GetDefaultScheduler();
            DateTime nextFireTimeForJob = this.getNextFireTimeForJob(defaultScheduler, "jobdata", "groupdata");
            this.info.Text = "Next Run Time: " + nextFireTimeForJob.ToString();
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            IScheduler defaultScheduler = StdSchedulerFactory.GetDefaultScheduler();
            DateTime nextFireTimeForJob = this.getNextFireTimeForJob(defaultScheduler, "jobdata", "groupdata");
            this.info.Text = "Great! data Saved, Next Run Time: " + nextFireTimeForJob.ToString();
        }

        private DateTime getNextFireTimeForJob(IScheduler scheduler, string jobName, string groupName = "")
        {
            JobKey jobKey = new JobKey("job");
            DateTime result = DateTime.MinValue;
            if (scheduler.CheckExists(jobKey))
            {
                scheduler.GetJobDetail(jobKey);
                IList<ITrigger> triggersOfJob = scheduler.GetTriggersOfJob(jobKey);
                if (triggersOfJob.Count > 0)
                {
                    DateTimeOffset? nextFireTimeUtc = triggersOfJob[0].GetNextFireTimeUtc();
                    result = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTimeUtc.Value.DateTime);
                }
            }
            return result;
        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)((DetailsView)sender).FindControl("txtName");
                TextBox textBox2 = (TextBox)((DetailsView)sender).FindControl("txtCount");
                DropDownList dropDownList = (DropDownList)((DetailsView)sender).FindControl("ddlListing");
                DropDownList dropDownList2 = (DropDownList)((DetailsView)sender).FindControl("ddlCountry");
                CheckBoxList obj = (CheckBoxList)((DetailsView)sender).FindControl("chlkeywords");
                string text = "";
                foreach (ListItem item in obj.Items)
                {
                    if (item.Selected)
                    {
                        text = text + item.Value + ",";
                    }
                }
                this.SqlDataSource1.UpdateParameters["place"].DefaultValue = textBox.Text.Trim();
                this.SqlDataSource1.UpdateParameters["count"].DefaultValue = textBox2.Text.Trim();
                this.SqlDataSource1.UpdateParameters["listing"].DefaultValue = dropDownList.SelectedValue;
                this.SqlDataSource1.UpdateParameters["country"].DefaultValue = dropDownList2.SelectedValue;
                this.SqlDataSource1.UpdateParameters["keywords"].DefaultValue = text;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void DetailView_DataBound(object sender, EventArgs e)
        {
            if (((DetailsView)sender).CurrentMode == DetailsViewMode.Edit)
            {
                DataRowView dataRowView = (DataRowView)((DetailsView)sender).DataItem;
                TextBox textBox = (TextBox)((DetailsView)sender).FindControl("txtName");
                TextBox textBox2 = (TextBox)((DetailsView)sender).FindControl("txtCount");
                DropDownList dropDownList = (DropDownList)((DetailsView)sender).FindControl("ddlListing");
                DropDownList obj = (DropDownList)((DetailsView)sender).FindControl("ddlCountry");
                CheckBoxList checkBoxList = (CheckBoxList)((DetailsView)sender).FindControl("chlkeywords");
                textBox.Text = dataRowView["place"].ToString();
                textBox2.Text = dataRowView["count"].ToString();
                dropDownList.SelectedValue = dataRowView["listing"].ToString();
                obj.SelectedValue = dataRowView["country_id"].ToString();
                foreach (string item in (from w in dataRowView["keywords"].ToString().Split(',')
                                         select w.ToString()).ToList())
                {
                    if (checkBoxList.Items.FindByText(item) != null)
                    {
                        checkBoxList.Items.FindByText(item).Selected = true;
                    }
                }
            }
        }

        protected void DetailView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            TextBox textBox = (TextBox)((DetailsView)sender).FindControl("txtName");
            TextBox textBox2 = (TextBox)((DetailsView)sender).FindControl("txtCount");
            DropDownList dropDownList = (DropDownList)((DetailsView)sender).FindControl("ddlListing");
            DropDownList dropDownList2 = (DropDownList)((DetailsView)sender).FindControl("ddlCountry");
            CheckBoxList obj = (CheckBoxList)((DetailsView)sender).FindControl("chlkeywords");
            string text = "";
            foreach (ListItem item in obj.Items)
            {
                if (item.Selected)
                {
                    text = text + item.Value + ",";
                }
            }
            this.SqlDataSource1.InsertParameters["place"].DefaultValue = textBox.Text.Trim();
            this.SqlDataSource1.InsertParameters["count"].DefaultValue = textBox2.Text.Trim();
            this.SqlDataSource1.InsertParameters["listing"].DefaultValue = dropDownList.SelectedValue;
            this.SqlDataSource1.InsertParameters["country"].DefaultValue = dropDownList2.SelectedValue;
            this.SqlDataSource1.InsertParameters["keywords"].DefaultValue = text;
        }

        protected void view_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("~/log.txt");
        }

        protected void down_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("~/logs/");
        }

        protected void run_Click(object sender, EventArgs e)
        {
            Nestoria.RunTask();
        }
        protected void images_Click(object sender, EventArgs e)
        {
            StaticCalls.Move_Images();
        }
        protected void del_Click(object sender, EventArgs e)
        {
            StaticCalls.DeleteProperties();
        }
    }
}