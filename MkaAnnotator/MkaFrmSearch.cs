using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaCommon;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace MokkAnnotator
{
    /// <summary>
    /// Type of search
    /// </summary>
    public enum SearchType
    {
        Ambiguous,
        Part,
        Complete
    }

    /// <summary>
    /// Search mokkan form
    /// </summary>
    public partial class MkaFrmSearch : Form
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;

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

        private const int _pageSize = 15;
        private int _totalRecords = 0;                
        private int _pageCount = 0;
        private int _currentPage = 0;

        // open selected glass/mokkan event
        public event OpenSelectedEventHandler OpenSelectedEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaFrmSearch()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;

            this.lblResult.Visible = false;
            this.dtgrResult.Rows.Clear();
            this.btnFirst.Enabled = false;
            this.btnPrevious.Enabled = false;
            this.btnNext.Enabled = false;
            this.btnLast.Enabled = false;
            this.lblPage.Visible = false;

            dtpDate.MinDate = MkaDefine.StartDate;
            dtpDate.MaxDate = DateTime.Today;
            dtpDate.Value = DateTime.Today;
        }

        /// <summary>
        /// Search button click event
        /// </summary>        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // get search query string
            if (GetSearchCondition() == 1) return;

            // get return record number
            GetCount();

            this.lblResult.Visible = true;

            // if not found
            if (_totalRecords == 0)
            {
                dtgrResult.Rows.Clear();
                this.lblResult.Text = "見つかりませんでした。";
                          
                this.btnFirst.Enabled = false;
                this.btnPrevious.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
                this.lblPage.Visible = false;
                          
                return;               
            }

            this.lblResult.Text = _totalRecords.ToString("N0") + " 件が見つけました。";
            this.btnFirst.Enabled = true;
            this.btnLast.Enabled = true;

            _pageCount = (int)Math.Ceiling((double)_totalRecords / _pageSize);            
            _currentPage = 0;            
            LoadPage();
        }

        /// <summary>
        /// Get number of returned records
        /// </summary>
        private void GetCount()
        {
            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() == false)
            {
                MkaMessage.ShowError(MkaMessage.ErrDBConnect);
                return;
            }
            con = dbCon.GetConnection();

            this.Cursor = Cursors.WaitCursor;

            // get search record count
            cmd = new MySqlCommand("get_count", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if(_chousaJisuu == 0)
                cmd.Parameters.Add(new MySqlParameter("_chousaJisuu", null));
            else
                cmd.Parameters.Add(new MySqlParameter("_chousaJisuu", _chousaJisuu));
            cmd.Parameters.Add(new MySqlParameter("_ooChikuBangou", _ooChiku));
            cmd.Parameters.Add(new MySqlParameter("_chuushouChikuBangou", _chuushouChiku));
            cmd.Parameters.Add(new MySqlParameter("_ikouBangou", _ikoumei));
            cmd.Parameters.Add(new MySqlParameter("_dosouMei", _dosoumei));
            if(_grid == 0)
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
            cmd.Parameters.Add(new MySqlParameter("_shasinBangouJyouhou",_shashinBangouJyouhou));
            cmd.Parameters.Add(new MySqlParameter("_bikou", _bikou));
            cmd.Parameters.Add(new MySqlParameter("_recCount", _totalRecords));
            cmd.Parameters["_recCount"].Direction = ParameterDirection.Output;            
            cmd.ExecuteNonQuery();
            _totalRecords = (int)cmd.Parameters["_recCount"].Value;

            // close connection
            dbCon.CloseConnection();

            this.Cursor = Cursors.Default;
        }
    
        /// <summary>
        /// Get retured result and display on datagrid
        /// </summary>
        private void LoadPage()
        {
            btnPrevious.Enabled = (_currentPage == 0) ? false : true;
            btnNext.Enabled = (_currentPage == _pageCount - 1) ? false : true;

            MkaDBConnect dbCon = new MkaDBConnect();          
            MySqlConnection con;
            MySqlCommand cmd;
            int intSkip = _currentPage * _pageSize;

            // open connection
            if (dbCon.OpenConnection() == false)
            {
                MkaMessage.ShowError(MkaMessage.ErrDBConnect);
                return;
            }
            con = dbCon.GetConnection();

            this.Cursor = Cursors.WaitCursor;

            // get search records
            cmd = new MySqlCommand("get_records", con);
            cmd.CommandTimeout = 600; // 10 minutes
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
            cmd.Parameters.Add(new MySqlParameter("_pageSize", _pageSize));
            cmd.Parameters.Add(new MySqlParameter("_skip", intSkip));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);
  
            Size _resize = new Size(64,64);
            DateTime _dDate;
            String path = "", _sDate;
            Bitmap bmp = null;
            dtgrResult.Rows.Clear();
            foreach(DataRow row in dt.Rows)
            {
                try
                {
                    // get mokkan image
                    path = String.Format("{0}\\{1}\\{2}\\{3}\\{1}_{2}_{3}_{4:00000}.jpg", MkaDefine.MokkanPath, row[11], row[6], row[5], row[0]);                    
                    bmp = new Bitmap(path);                    
                }
                catch
                {
                    MessageBox.Show("ダータベースの画像ファイルを開くことできません" + Environment.NewLine +
                    path + "を見付けません", "メッセージ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    //_log.Error("Can not open image!: " + path　+Environment.NewLine);                    

                    // delete mokkan of the glass if existed
                    //cmd = new MySqlCommand("delete_mokkan", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new MySqlParameter("_rBangou", row[0]));
                    //cmd.ExecuteNonQuery();

                    //_log.Info("Deleted registered but not existed item.");
                    continue;
                }

                bmp = ResizeImage(bmp, _resize);
                _dDate = DateTime.ParseExact(row[12].ToString(), MkaDefine.DBDateFormat, null);
                _sDate = _dDate.ToString(MkaDefine.DateFormat);

                // add row
                dtgrResult.Rows.Add(bmp, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], _sDate);
            } 

            // close connection            
            dbCon.CloseConnection();

            lblPage.Visible = true;
            lblPage.Text = String.Format("{0}/{1}", _currentPage + 1, _pageCount);

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Resize image to normalized size
        /// </summary>        
        private Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Clear the form
        /// </summary>
        private void Clear()
        {
            // glass information
            this.txtChousaJisuu.Text = "";
            this.txtOoChiku.Text = "";
            this.txtChuushouChiku.Text = "";
            this.txtIkoumei.Text = "";
            this.txtDosoumei.Text = "";
            this.txtGrid.Text = "";
            this.dtpDate.Value = DateTime.Today;
            this.txtBatBangou.Text = "";
            this.txtGlassItaBangou.Text = "";

            // mokkan information
            this.txtRBangou.Text = "";
            this.txtKariShakubun.Text = "";
            this.txtGaihouShoshuuJyouhou.Text = "";
            this.txtShasinBangouJyouhou.Text = "";
            this.txtBikou.Text = "";

            this.rdbAmbiguous.Checked = true;

            // search result
            this.lblResult.Visible = false;            
            this.dtgrResult.Rows.Clear();
        }

        /// <summary>
        /// Get search condition string (Where condition)
        /// </summary>        
        private int GetSearchCondition()
        {
            if (!CheckValidInput())
                return 1;

            // 検索モード
            if (this.rdbAmbiguous.Checked)
                _searchType = SearchType.Ambiguous;
            else if (this.rdbPart.Checked)
                _searchType = SearchType.Part;
            else
                _searchType = SearchType.Complete;

            // 大地区
           _ooChiku = this.txtOoChiku.Text.Trim();

            // 中小地区
           _chuushouChiku = this.txtChuushouChiku.Text.Trim();

            // 遺構名
           _ikoumei = GetCondition(this.txtIkoumei.Text.Trim(), _searchType);          

            // 土層名
           _dosoumei = GetCondition(this.txtDosoumei.Text.Trim(), _searchType);

            // 出土日付
            _date = dtpDate.Value.ToString(MkaDefine.DBDateFormat);
            if (_date == DateTime.Today.ToString(MkaDefine.DBDateFormat))
                _date = "";

            // バット番号
            _batBangou = this.txtBatBangou.Text.Trim();

            // ガラス板番号
            _glassItaBangou = this.txtGlassItaBangou.Text.Trim();

            // 仮釈文
            _kariShakubun = GetCondition(this.txtKariShakubun.Text.Trim(), _searchType);

            // 概報所収情報
            _gaihouShoshuuJyouhou = GetCondition(this.txtGaihouShoshuuJyouhou.Text.Trim(), _searchType);

            // 写真番号情報
            _shashinBangouJyouhou = GetCondition(this.txtShasinBangouJyouhou.Text.Trim(), _searchType);

            // 備考
            _bikou = GetCondition(this.txtBikou.Text.Trim(), _searchType);

            return 0;
        }

        /// <summary>
        /// Get condition string belongs to search type
        /// </summary>        
        public String GetCondition(String condition, SearchType type)
        {
            String temp = "";

            if (type == SearchType.Ambiguous)
            {
                condition = condition.Replace("?", "_");
                condition = condition.Replace("？", "_");
                condition = condition.Replace("*", "%");
                condition = condition.Replace("＊", "%");
                condition = "%" + condition + "%";
            }
            else if (type == SearchType.Part)
            {
                foreach (char c in condition.ToCharArray())
                    temp += c + "%";
                condition = "%" + temp;
            }
            else
            {
                condition = "%" + condition + "%";
            }

            return condition;
        }

        /// <summary>
        /// Check input data
        /// </summary>
        /// <returns>true if all input data were valid</returns>
        private bool CheckValidInput()
        {
            String error = "";
            
            // remains id
            _rBangou = 0;
            if (txtRBangou.Text.Trim() != "" && !Int32.TryParse(txtRBangou.Text.Trim(), out _rBangou))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblRBangou)) + "\n" + error;
                txtRBangou.Focus();
            }            

            // check grid
            _grid = 0;
            if (txtGrid.Text.Trim() != "" && !Int32.TryParse(txtGrid.Text.Trim(), out _grid))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblGrid)) + "\n" + error;
                txtGrid.Focus();
            }

            // check dig order
            _chousaJisuu = 0;
            if (txtChousaJisuu.Text.Trim() != "" && !Int32.TryParse(txtChousaJisuu.Text.Trim(), out _chousaJisuu))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblChousaJisuu)) + "\n" + error;
                txtChousaJisuu.Focus();
            }

            // show error message
            if (error != "")
            {
                MkaMessage.ShowError(error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get label text 
        /// </summary>        
        private String GetLblText(Label label)
        {
            String lblText = label.Text;
            lblText = lblText.Substring(0, lblText.IndexOf('('));
            return lblText;
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            // clear the form
            Clear();
        }

        private void MkaFrmSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        /// <summary>
        /// Open selected file
        /// </summary>        
        private void dtgrResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // row header
            if (e.RowIndex < 0)
                return;

            String gridNo = dtgrResult["colGrid", e.RowIndex].Value.ToString();
            String batFolder = dtgrResult["colBatBangou", e.RowIndex].Value.ToString();
            String glassFile = dtgrResult["colGlassBangou", e.RowIndex].Value.ToString();
            String fileName = MkaDefine.MokkanPath + "\\" + gridNo + "\\" + batFolder + "\\" + glassFile + MkaDefine.MkaFileExt;
            String rId = dtgrResult["colRBangou", e.RowIndex].Value.ToString();

            // raised open event
            if(File.Exists(fileName))
                OpenSelectedEvent(new OpenSelectedEventArgs(fileName, rId));
        }

        /// <summary>
        /// Show first page of search results
        /// </summary>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            _currentPage = 0;
            LoadPage();
        }

        /// <summary>
        /// Show previous page of search results
        /// </summary>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            _currentPage--;
            LoadPage();
        }

        /// <summary>
        /// Show next page of search results
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            _currentPage++;
            LoadPage();
        }

        /// <summary>
        /// Show last page of search results
        /// </summary>
        private void btnLast_Click(object sender, EventArgs e)
        {
            _currentPage = _pageCount - 1;
            LoadPage();
        }

        private void txtDate_Enter(object sender, EventArgs e)
        {
            dtpDate.Focus();

        }

        private void dtpDate_Leave(object sender, EventArgs e)
        {
            txtDate.Visible = true;
            if (dtpDate.Value == DateTime.Today)
                txtDate.Text = "";
            else
                txtDate.Text = dtpDate.Value.ToString(MkaDefine.DateFormat);
        }

        private void dtpDate_Enter(object sender, EventArgs e)
        {
            txtDate.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
        
    public delegate void OpenSelectedEventHandler(OpenSelectedEventArgs e);
    /// <summary>
    /// Open file event arguments.
    /// Used in events raised from Search form.
    /// Class contains name of file and selected mokkan's remain id.
    /// </summary>
    public class OpenSelectedEventArgs : System.EventArgs
    {
        private string fileName;
        private string rId;

        public OpenSelectedEventArgs(string fileName, string rId)
        {
            this.fileName = fileName;
            this.rId = rId;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public string RId
        {
            get
            {
                return rId;
            }
        }
    }

}
