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
using System.Text.RegularExpressions;

namespace MkaWeb
{
    public partial class DetailSearch : System.Web.UI.Page
    {
        private int _chousaJisuu;
        private String _ooChiku;
        private String _chuushouChiku;
        private String _ikoumei;
        private String _dosoumei;
        private int _grid;
        private String _date;
        private String _batBangou;
        private String _glassItaBangou;
        private int _rBangou;
        private String _kariShakubun;
        private String _gaihouShoshuuJyouhou;
        private String _shashinBangouJyouhou;
        private String _bikou;
        private SearchType _searchType;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
            ClientScript.RegisterStartupScript(this.GetType(), "goto", "<script type='text/javascript'>window.scrollTo(0, 500);</script>");
        }
        
        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            GetData();
        }

        private void GetData()
        {
            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            GetKeywords();

            // open connection
            if (dbCon.OpenConnection() != 1)
                return;
            con = dbCon.GetConnection();

            // get search records
            cmd = new MySqlCommand("detail_get_records", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (_chousaJisuu == 0)
                cmd.Parameters.Add(new MySqlParameter("_chousaJisuu", null));
            else
                cmd.Parameters.Add(new MySqlParameter("_chousaJisuu", _chousaJisuu));
            cmd.Parameters.Add(new MySqlParameter("_ooChikuBangou", _ooChiku));
            cmd.Parameters.Add(new MySqlParameter("_chuushouChikuBangou", _chuushouChiku));
            cmd.Parameters.Add(new MySqlParameter("_ikouBangou", _ikoumei));
            cmd.Parameters.Add(new MySqlParameter("_dosouMei", _dosoumei));
            if (_grid == 0)
                cmd.Parameters.Add(new MySqlParameter("_grid", null));
            else
                cmd.Parameters.Add(new MySqlParameter("_grid", _grid));
            cmd.Parameters.Add(new MySqlParameter("_date", _date));
            cmd.Parameters.Add(new MySqlParameter("_batBangou", _batBangou));
            cmd.Parameters.Add(new MySqlParameter("_glassBangou", _glassItaBangou));
            if (_rBangou == 0)
                cmd.Parameters.Add(new MySqlParameter("_rBangou", null));
            else
                cmd.Parameters.Add(new MySqlParameter("_rBangou", _rBangou));
            cmd.Parameters.Add(new MySqlParameter("_kariShakubun", _kariShakubun));
            cmd.Parameters.Add(new MySqlParameter("_gaihouShoshuuJyouhou", _gaihouShoshuuJyouhou));
            cmd.Parameters.Add(new MySqlParameter("_shasinBangouJyouhou", _shashinBangouJyouhou));
            cmd.Parameters.Add(new MySqlParameter("_bikou", _bikou));
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void GetKeywords()
        {
            // 検索モード
            if (this.rblSearch.SelectedIndex == 0)
                _searchType = SearchType.Ambiguous;
            else if (this.rblSearch.SelectedIndex == 1)
                _searchType = SearchType.Part;
            else
                _searchType = SearchType.Complete;

            // 調査次数        
            if (txtChousaJisuu.Text.Trim() != "" && !Int32.TryParse(txtChousaJisuu.Text.Trim(), out _chousaJisuu))
                _chousaJisuu = 0;

            // 大地区
            _ooChiku = this.txtOoChiku.Text.Trim();

            // 中小地区
            _chuushouChiku = this.txtChuushouChiku.Text.Trim();

            // 遺構名
            _ikoumei = MkaCommon.GetCondition(this.txtIkoumei.Text.Trim(), _searchType);

            // 土層名
            _dosoumei = MkaCommon.GetCondition(this.txtDosoumei.Text.Trim(), _searchType);

            // グリッド         
            if (txtGrid.Text.Trim() != "" && !Int32.TryParse(txtGrid.Text.Trim(), out _grid))
                _grid = 0;

            // 出土日付
            _date = Regex.Replace(this.txtDate.Text, @"[/]", "");
            if(_date.Length > 6)
                _date = _date.Substring(0, 6);

            // バット番号
            _batBangou = this.txtBatBangou.Text.Trim();

            // ガラス板番号
            _glassItaBangou = this.txtGlassBangou.Text.Trim();

            // R番号
            if (txtRBangou.Text.Trim() != "" && !Int32.TryParse(txtRBangou.Text.Trim(), out _rBangou))
                _rBangou = 0;
                       
            // 仮釈文
            _kariShakubun = MkaCommon.GetCondition(this.txtKariShakubun.Text.Trim(), _searchType);

            // 概報所収情報
            _gaihouShoshuuJyouhou = MkaCommon.GetCondition(this.txtGaihouShoshuuJyouhou.Text.Trim(), _searchType);

            // 写真番号情報
            _shashinBangouJyouhou = MkaCommon.GetCondition(this.txtShasinBangouJyouhou.Text.Trim(), _searchType);

            // 備考
            _bikou = MkaCommon.GetCondition(this.txtBikou.Text.Trim(), _searchType);
        }

        private void ResetForm()
        {
            // glass information
            this.txtChousaJisuu.Text = "";
            this.txtOoChiku.Text = "";
            this.txtChuushouChiku.Text = "";
            this.txtIkoumei.Text = "";
            this.txtDosoumei.Text = "";
            this.txtGrid.Text = "";
            this.txtDate.Text = "";
            this.txtBatBangou.Text = "";
            this.txtGlassBangou.Text = "";

            // mokkan information
            this.txtRBangou.Text = "";
            this.txtKariShakubun.Text = "";
            this.txtGaihouShoshuuJyouhou.Text = "";
            this.txtShasinBangouJyouhou.Text = "";
            this.txtBikou.Text = "";

            this.rblSearch.SelectedIndex = 1;

            // search result    
            this.lblResult.Text = "";
            this.lblResult.Visible = false;
            gvResults.DataSource = null;
            gvResults.DataBind();
        }
    }
}
