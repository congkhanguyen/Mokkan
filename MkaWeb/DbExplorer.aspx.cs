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
using System.Drawing.Drawing2D;

namespace MkaWeb
{
    public partial class DbExplorer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadBatGlass();
        }

        private void LoadBatGlass()
        {
            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() != 1)
                return;
            con = dbCon.GetConnection();

            // get search records
            cmd = new MySqlCommand("get_bat_glass", con);
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

            String _lastBat = "";
            TreeNode _batNode, _glassNode;
            _batNode = new TreeNode();
            foreach (DataRow row in dt.Rows)
            {
                if (row[0].ToString() != _lastBat)
                {
                    _lastBat = row[0].ToString();
                    _batNode = new TreeNode(_lastBat, _lastBat, "~/images/bat.png");
                    treeBatGlass.Nodes.Add(_batNode);
                }
                _glassNode = new TreeNode(row[1].ToString(), row[1].ToString(), "~/images/glass.png");
                _batNode.ChildNodes.Add(_glassNode);
            }

            // close connection            
            dbCon.CloseConnection();
        }

        protected void treeBatGlass_SelectedNodeChanged(object sender, EventArgs e)
        {
            GetData();
        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            GetData();
        }

        private void GetData()
        {
            TreeNode node = treeBatGlass.SelectedNode;
            String _batBangou, _glassBangou;
            if (node.Depth == 0)
            {
                _batBangou = node.Text;
                _glassBangou = "";
                node.Expand();
            }
            else
            {
                _batBangou = node.Parent.Text;
                _glassBangou = node.Text;
            }

            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() != 1)
                return;
            con = dbCon.GetConnection();

            // get search records
            cmd = new MySqlCommand("get_mokkan_from_batglass", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("_batBangou", _batBangou));
            cmd.Parameters.Add(new MySqlParameter("_glassBangou", _glassBangou));
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

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
