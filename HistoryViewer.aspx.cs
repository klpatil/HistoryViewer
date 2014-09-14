using Sitecore.Collections;
using Sitecore.Data.Engines;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SC7.Web.Tools
{
    public partial class HistoryViewer : Sitecore.sitecore.admin.AdminPage
    {
        /// <summary>
        /// Method for Security Check
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.CheckSecurity(true);
            base.OnInit(e);
        }
        
        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!Page.IsPostBack)
                {
                    ddlDatabases.DataSource = Sitecore.Configuration.Factory.GetDatabaseNames().Where(x => x != "filesystem");
                    ddlDatabases.DataBind();
                    
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while loading history engine entries", ex, this);
                ShowMessage(ex.Message, "Error");
            }


        }

        /// <summary>
        /// This method loadds all history
        /// engine entries
        /// </summary>
        private void LoadHistoryEngineEntries()
        {

            Sitecore.Data.Database database = 
                Sitecore.Configuration.Factory.GetDatabase(ddlDatabases.SelectedValue);

            double duration = string.IsNullOrWhiteSpace(txtLastDays.Text) ? -1 : 
                double.Parse("-"+txtLastDays.Text.Trim()); 
            HistoryEntryCollection historyEntries =
                HistoryManager.GetHistory(database, DateTime.UtcNow.AddDays(duration),
                DateTime.UtcNow);            
             IOrderedEnumerable<HistoryEntry> historyEntriesSorted = 
                 historyEntries.OrderByDescending(x => x.Created);

             if (historyEntriesSorted == null && !historyEntriesSorted.Any())
            {
                ShowMessage("No history engine records found, it seems that you haven't configured history engine for this database or no actions recorded yet.",
                    "Error");
                lblMessage.Visible = true;
                tblHistoryDetails.Visible = false;
            }
            else
            {

                foreach (HistoryEntry aHistoryEntry in historyEntriesSorted)
                {

                    if (aHistoryEntry != null)
                    {
                        TableRow tableRow = new TableRow();
                        tableRow.ID = "row" + aHistoryEntry.EntryId.ToShortID().ToString();
                        tableRow.TableSection = TableRowSection.TableBody;

                        //ItemPath
                        AddTableCell(tableRow, aHistoryEntry.ItemPath.ToString(), FieldTypes.Text);

                        //ItemLanguage
                        AddTableCell(tableRow, aHistoryEntry.ItemLanguage.ToString(), FieldTypes.Text);

                        //ItemVersion
                        AddTableCell(tableRow, aHistoryEntry.ItemVersion.ToString(), FieldTypes.Text);

                        // Action
                        AddTableCell(tableRow, aHistoryEntry.Action.ToString(), FieldTypes.Text);

                        //TaskStack
                        AddTableCell(tableRow, aHistoryEntry.TaskStack.ToString(), FieldTypes.Text);

                        //UserName
                        AddTableCell(tableRow, aHistoryEntry.UserName.ToString(), FieldTypes.Text);

                        // Created
                        AddTableCell(tableRow, aHistoryEntry.Created.ToString(), FieldTypes.DateTime);

                        //AdditionalInfo
                        AddTableCell(tableRow, aHistoryEntry.AdditionalInfo.ToString(), FieldTypes.Text);


                        tblHistoryDetails.Rows.Add(tableRow);
                    }

                }

                ShowMessage(string.Format(@"Showing <strong>{0}</strong> History records from 
                    <strong>{1}</strong> for last <strong>{2}</strong> days"
                    , historyEntries.Count, ddlDatabases.SelectedValue,
                    string.IsNullOrWhiteSpace(txtLastDays.Text) ? "-1" : txtLastDays.Text), string.Empty);
                lblMessage.Visible = true;
                tblHistoryDetails.Visible = true;

            }
        }

        /// <summary>
        /// This function will be used
        /// to show message
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="messageType">Message type to show</param>
        private void ShowMessage(string message, string messageType)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            if (messageType == "Error")
            {
                lblMessage.CssClass = "alert alert-danger";

            }
            else
            {
                lblMessage.CssClass = "alert alert-success";
            }
        }

        /// <summary>
        /// This function will be used to add
        /// table cell
        /// </summary>
        /// <param name="tableRow">Table Row</param>
        /// <param name="aField">Field</param>
        /// <param name="fieldType">Type of field</param>
        /// <returns></returns>
        private static TableCell AddTableCell(TableRow tableRow,
            string value, FieldTypes fieldType)
        {
            TableCell tableCell1 = new TableCell();

            string valueToPrint = "NA";

            switch (fieldType)
            {
                case FieldTypes.DateTime:
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        DateTime createdDate = DateTime.Now;
                        createdDate = DateTime.Parse(value);
                        valueToPrint = TimeAgo(createdDate);
                    }
                    else
                    {
                        valueToPrint = "NA";
                    }
                    break;
                case FieldTypes.Text:
                    valueToPrint = !string.IsNullOrWhiteSpace(value) ? value : "NA";
                    break;
                default:
                    valueToPrint = !string.IsNullOrWhiteSpace(value) ? value : "NA";
                    break;
            }

            tableCell1.Text = valueToPrint;
            tableRow.Cells.Add(tableCell1);
            return tableCell1;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadHistoryEngineEntries();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while showing history engine entries", ex, this);
                ShowMessage(ex.Message, "Error");
            }
            
        }

        private void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            Response.Write("Oops something went wrong : " + exc.Message);
            Sitecore.Diagnostics.Log.Error("HistoryViewer : " + exc, this);
            Server.ClearError();
            
        }

        /// <summary>
        /// http://aeykay.blogspot.in/2012/10/facebook-like-time-ago-function.html
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string TimeAgo(DateTime date)
        {

            TimeSpan timeSince = DateTime.UtcNow.Subtract(date);
            if (timeSince.TotalMilliseconds < 1)
                return "not yet";
            if (timeSince.TotalMinutes < 1)
                return "just now";
            if (timeSince.TotalMinutes < 2)
                return "1 minute ago";
            if (timeSince.TotalMinutes < 60)
                return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120)
                return "1 hour ago";
            if (timeSince.TotalHours < 24)
                return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays == 1)
                return "yesterday";
            if (timeSince.TotalDays < 7)
                return string.Format("{0} days ago", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "last week";
            if (timeSince.TotalDays < 21)
                return "2 weeks ago";
            if (timeSince.TotalDays < 28)
                return "3 weeks ago";
            if (timeSince.TotalDays < 60)
                return "last month";
            if (timeSince.TotalDays < 365)
                return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "last year";

            //last but not least...
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));

        }


    }

    enum FieldTypes
    {
        DateTime,
        Text

    }
}