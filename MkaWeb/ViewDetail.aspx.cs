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
using MySql.Data.MySqlClient;

namespace MkaWeb
{
    public partial class PopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                String rBangou = Request.QueryString["RBangou"];
                if (rBangou != null && rBangou != "")
                {
                    ViewData(rBangou);
                    imgMokkan.ImageUrl = "FullImageHttpHandler.ashx?RBangou=" + rBangou;
                }
            }
        }        
        protected void ViewData(String rBangou)
        {
            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() != 1)
                return;
            con = dbCon.GetConnection();

            // get search records
            cmd = new MySqlCommand("get_mokkan_details", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("_rBangou", rBangou));
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

            if (dt.Rows.Count == 0) return;
            
            lblChousaJisuu.Text = Convert.ToString(dt.Rows[0]["調査次数"]);
            lblChikuBangou.Text = Convert.ToString(dt.Rows[0]["調査地区"]);
            lblIkoumei.Text = Convert.ToString(dt.Rows[0]["遺構名"]);
            lblDosoumei.Text = Convert.ToString(dt.Rows[0]["土層名"]);
            lblGrid.Text = Convert.ToString(dt.Rows[0]["グリッド"]);

            String strDate = Convert.ToString(dt.Rows[0]["出土日付"]);            
            lblDate.Text = strDate.Substring(0, 4) + "年" + strDate.Substring(4, 2) + "月" + strDate.Substring(6, 2) + "日";

            lblBatBangou.Text = Convert.ToString(dt.Rows[0]["バット番号"]);
            lblGlassItaBangou.Text = Convert.ToString(dt.Rows[0]["ガラス板番号"]);
            lblRBangou.Text = Convert.ToString(dt.Rows[0]["R番号"]);
            lblKariShakubun.Text = Convert.ToString(dt.Rows[0]["仮釈文"]);
            lblGaihouShosuuJyouhou.Text = Convert.ToString(dt.Rows[0]["概報所収情報"]);
            lblShasinBangouJyouhou.Text = Convert.ToString(dt.Rows[0]["写真番号情報"]);
            lblBikou.Text = Convert.ToString(dt.Rows[0]["備考"]);
        }
    }
}
