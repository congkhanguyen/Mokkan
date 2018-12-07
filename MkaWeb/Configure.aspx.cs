using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;

namespace MkaWeb
{
    public partial class Configure : System.Web.UI.Page
    {
        private String strAddress;
        private String strDatabase;
        private String strUsername;
        private String strPassword;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtAddress.Attributes.Add("value", ConfigurationSettings.AppSettings["Server"]);
            txtDatabase.Attributes.Add("value", ConfigurationSettings.AppSettings["Database"]);
            txtUsername.Attributes.Add("value", ConfigurationSettings.AppSettings["Uid"]);
            txtPassword.Attributes.Add("value", ConfigurationSettings.AppSettings["Password"]); 
        }

        /// <summary>
        /// Save database setting to configure file
        /// </summary>        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            RetrieveData();

            this.imgResult.Visible = false;
            this.lblTestResult.Text = "";

            ExeConfigurationFileMap FileMap = new ExeConfigurationFileMap();
            FileMap.ExeConfigFilename = Server.MapPath(@"~\Web.config");
            Configuration Config = ConfigurationManager.OpenMappedExeConfiguration(FileMap, ConfigurationUserLevel.None);


            Config.AppSettings.Settings["Server"].Value = strAddress;
            Config.AppSettings.Settings["Database"].Value = strDatabase;
            Config.AppSettings.Settings["Uid"].Value = strUsername;
            Config.AppSettings.Settings["Password"].Value = strPassword;

            Config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Test connection with user-input configure information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTest_Click(object sender, EventArgs e)
        {
            RetrieveData();

            int ret = MkaDBConnect.TryOpenConnection(strAddress, strDatabase, strUsername, strPassword);
            this.imgResult.Visible = true;
            if (ret == 1)
            {
                this.imgResult.ImageUrl = "~/images/connect_success.png";
                this.lblTestResult.Text = MkaMessage.SuccessDBConnect;
                this.lblTestResult.ForeColor = Color.Green;
            }
            else
            {
                this.imgResult.ImageUrl = "~/images/connect_error.png";
                this.lblTestResult.ForeColor = Color.Red;
                if (ret == 1045) 
                    this.lblTestResult.Text = MkaMessage.ErrDBInvalidID;                    
                else
                    this.lblTestResult.Text = MkaMessage.ErrDBConnect; 
            }
        }

        /// <summary>
        /// Get user-input information
        /// </summary>        
        protected void RetrieveData()
        {
            strAddress = txtAddress.Text.Trim();
            strDatabase = txtDatabase.Text.Trim();
            strUsername = txtUsername.Text.Trim();
            strPassword = txtPassword.Text.Trim();
        }
    }
}
