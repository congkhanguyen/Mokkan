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
using System.IO;
using System.Drawing;

namespace MkaWeb
{
    public partial class SimpleSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
            ClientScript.RegisterStartupScript(this.GetType(), "goto", "<script type='text/javascript'>window.scrollTo(0, 330);</script>");
        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            GetData();
        } 

        private void GetData()
        {
            // 検索モード
            SearchType _searchType;
            if (this.rblSearch.SelectedIndex == 0)
                _searchType = SearchType.Ambiguous;
            else if (this.rblSearch.SelectedIndex == 1)
                _searchType = SearchType.Part;
            else
                _searchType = SearchType.Complete;

            String _keyword = MkaCommon.GetCondition(txtKeyword.Text.Trim(), _searchType);

            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() != 1)
                return;
            con = dbCon.GetConnection();

            // get search records
            cmd = new MySqlCommand("simple_get_records", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("_keyword", _keyword));
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

            lblResult.Visible = true;
            if (dt.Rows.Count == 0)
                lblResult.Text = "見つかりませんでした.";
            else
                lblResult.Text = dt.Rows.Count.ToString() + "点が見つけました.";
            gvResults.DataSource = dt;
            gvResults.DataBind();

            // close connection            
            dbCon.CloseConnection();    
        }

        protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // R番号
            Label lblRBangou = (Label)(e.Row.FindControl("lblRBangou"));
            if (lblRBangou != null)
            {
                lblRBangou.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "R番号"));
            }

            // 仮釈文
            Label lblKariShakubun = (Label)(e.Row.FindControl("lblKariShakubun"));
            if (lblKariShakubun != null)
            {
                lblKariShakubun.Text = (string)DataBinder.Eval(e.Row.DataItem, "仮釈文");
            }

            // 概報所収情報
            Label lblGaihouShoshuuJyouhou = (Label)(e.Row.FindControl("lblGaihouShoshuuJyouhou"));
            if (lblGaihouShoshuuJyouhou != null)
            {
                lblGaihouShoshuuJyouhou.Text = (string)DataBinder.Eval(e.Row.DataItem, "概報所収情報");
            }

            // 写真番号情報
            Label lblShasinBangouJyouhou = (Label)(e.Row.FindControl("lblShasinBangouJyouhou"));
            if (lblShasinBangouJyouhou != null)
            {
                lblShasinBangouJyouhou.Text = (string)DataBinder.Eval(e.Row.DataItem, "写真番号情報");
            }

            // 備考
            Label lblBikou = (Label)(e.Row.FindControl("lblBikou"));
            if (lblBikou != null)
            {
                lblBikou.Text = (string)DataBinder.Eval(e.Row.DataItem, "備考");
            }

            // ガラス板番号
            Label lblGlassItaBangou = (Label)(e.Row.FindControl("lblGlassItaBangou"));
            if (lblGlassItaBangou != null)
            {
                lblGlassItaBangou.Text = (string)DataBinder.Eval(e.Row.DataItem, "ガラス板番号");
            }

            // バット番号
            Label lblBatBangou = (Label)(e.Row.FindControl("lblBatBangou"));
            if (lblBatBangou != null)
            {
                lblBatBangou.Text = (string)DataBinder.Eval(e.Row.DataItem, "バット番号");
            }

            // 詳細
            HyperLink hplDetail = (HyperLink)(e.Row.FindControl("hplDetail"));
            if (hplDetail != null)
            {
                hplDetail.NavigateUrl = "ViewDetail.aspx?RBangou=" + lblRBangou.Text + "&height=550&width=750";
                hplDetail.CssClass = "thickbox";
                hplDetail.Attributes.Add("title", "R番号が" + lblRBangou.Text + "の木簡");
            }
        }
    }
}
