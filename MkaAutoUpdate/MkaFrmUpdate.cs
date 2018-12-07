using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaDrawTools;

namespace MkaAutoUpdate
{
    public partial class MkaFrmUpdate : Form
    {
        private String _mokkanFolder;
        private String _infoFile;
        private int _startRId;
        private bool _startRIdSet;
        private String _grid;
        
        private List<BatInfo> _batInfos;
        private BatInfo _batInfo; 
        private List<string> _batPaths;
        private MkaBatManager _batManager;        
        private MkaDocument _activeDoc;

        private StreamWriter _statWriter;
        private StreamWriter _statGlassWriter;

        public MkaFrmUpdate()
        {
            InitializeComponent();
            _mokkanFolder = "";
            _infoFile = "";

            _startRId = -1;
            _startRIdSet = false;
            txtKaishiRBangou.Text = "";

            ckbRegisterDb.Checked = true;
            ckbFileUpdate.Checked = false;
            grpbInput.Enabled = false;

            _batInfos = new List<BatInfo>();

            //_mokkanFolder = txtMokkanLocation.Text = @"D:\Experiment\ABC";
        }

        /// <summary>
        /// Select mokkan location
        /// </summary>        
        private void btnMokkanBrowser_Click(object sender, EventArgs e)
        {              
            // Display select directory dialog  
            FolderBrowserDialog fbd = new FolderBrowserDialog();            
            if (fbd.ShowDialog() != DialogResult.OK) return;
            _mokkanFolder = fbd.SelectedPath;
            txtMokkanLocation.Text = _mokkanFolder;
        }

        /// <summary>
        /// Select excavation information file path
        /// </summary>        
        private void btnInfoBrowser_Click(object sender, EventArgs e)
        {
            // Display open file dialog
            OpenFileDialog ofd = new OpenFileDialog();            
            ofd.Multiselect = false;
            ofd.Filter = "調査情報テーブルファイル (.csv)|*.csv";

            // Get file path
            if (ofd.ShowDialog() != DialogResult.OK) return;
            _infoFile = ofd.FileName;
            txtInfoLocation.Text = _infoFile;
        }

        // The progress form will be created and shown modally while the
        // Synchronous process is running.  This form will notify the background
        // thread if a Cancellation is performed. The Background thread will update
        // the status label and ProgressBar on the Progress Form using Control.Invoke.
        private MkaWndProgress _fmProgress = null;

        /// <summary>
        /// Run write excavation information and statistical information
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (_mokkanFolder == "")
            {
                MessageBox.Show(this, "木簡保存場所を指定ください．", "木簡情報一括更新・登録", MessageBoxButtons.OK);
                btnInfoBrowser.Focus();
                return;
            }
            if(ckbFileUpdate.Checked)
            {
                _startRIdSet = Int32.TryParse(txtKaishiRBangou.Text, out _startRId);
                if(_infoFile == "" && !_startRIdSet)
                {
                    MessageBox.Show(this, "調査情報テーブルファイルまたは開始R番号を指定ください．", "木簡情報一括更新・登録", MessageBoxButtons.OK);
                    btnInfoBrowser.Focus();
                    return;
                }
                // read excavation information
                if(_infoFile != "")
                    ReadExcavationInfo();
                
                // change mokkan's rid
                MkaDefine.SaveMokkanImageAsSaving = _startRIdSet;
            }
                        
            if (ckbRegisterDb.Checked)
            {
                // reset database
                ResetDb();
                MkaDefine.RegisterToDbAsSaving = true;
            }
            else
                MkaDefine.RegisterToDbAsSaving = false;            

            _statWriter = new StreamWriter("統計情報テーブル.csv", false, Encoding.GetEncoding("shift_jis"));
            _statGlassWriter = new StreamWriter("統計情報_削りごと.csv", false, Encoding.GetEncoding("shift_jis"));

            List<string> _gridPaths = Directory.GetDirectories(_mokkanFolder).ToList();
            _batPaths = new List<string>();
            foreach (String _gridPath in _gridPaths)
            {
                _grid = Path.GetFileName(_gridPath);
                _batPaths.AddRange(Directory.GetDirectories(_gridPath).ToList());
            }
            _batPaths.Sort(CompareBat);

            if (_batPaths.Count == 0)
            {
                MessageBox.Show(this, "バットファイルは一つも存在していません．", "木簡情報一括更新・登録", MessageBoxButtons.OK);
                return;
            }

            // Create a background thread
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // Create a progress form on the UI thread
            _fmProgress = new MkaWndProgress();
            _fmProgress.Text = "実行中";

            // Kick off the Async thread
            bw.RunWorkerAsync();

            // Lock up the UI with this modal progress form.
            _fmProgress.ShowDialog(this);
            _fmProgress = null;

            _statWriter.Close();
            _statGlassWriter.Close();
        }

        /// <summary>
        /// Executing work
        /// </summary>        
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int iCount = _batPaths.Count;
            int _start = _startRId;

            for (int i = 0; i < iCount; i++)
            {
                // The Work to be performed...
                String _batPath = _batPaths[i];
                String _batName = Path.GetFileName(_batPaths[i]);

                String _batFilePath = _batPath + "\\" + _batName + MkaDefine.BmkFileExt;
                FileOpenBat(_batFilePath);
                if (_batManager == null) continue;


                // Update the description and progress on the modal form
                // using Control.Invoke.  Invoke will run the anonymous
                // function to set the label's text on the UI thread.  
                // Since it's illegal to touch the UI control on the worker 
                // thread that we're on right now.
                // Moron Anonymous functions: http://www.codeproject.com/books/cs2_anonymous_method.asp
                _fmProgress.LabelDescription.Invoke(
                    (MethodInvoker)delegate()
                    {
                        _fmProgress.LabelDescription.Text = String.Format("実行 {0}_{1} ({2}/{3})...", _batManager.BatInfo.Grid, _batName, i + 1, iCount);
                        _fmProgress.ProgressBar.Value = Convert.ToInt32((i + 1) * (100.0 / iCount));
                    }
                );   

                // change bat information
                if (ckbFileUpdate.Checked)
                {
                    _batInfo = _batInfos.Find(delegate(BatInfo b) { return b.Grid == _batManager.BatInfo.Grid; });
                    if (_batInfo != null)
                    {
                        _batManager.BatInfo.BatDirectory = _batPath;
                        _batManager.BatInfo.BatBangou = _batName;
                        _batManager.BatInfo.Grid = _batInfo.Grid;
                        _batManager.BatInfo.ChousaJisuu = _batInfo.ChousaJisuu;
                        _batManager.BatInfo.OoChiku = _batInfo.OoChiku;
                        _batManager.BatInfo.ChuushouChiku = _batInfo.ChuushouChiku;
                        _batManager.BatInfo.Ikoumei = _batInfo.Ikoumei;
                        _batManager.BatInfo.Dosoumei = _batInfo.Dosoumei;
                        _batManager.BatInfo.Date = _batInfo.Date;
                        _batManager.Save();
                    }
                }

                List<string> _glassPaths = Directory.GetFiles(_batPath, "*" + MkaDefine.MkaFileExt).ToList();
                _glassPaths.Sort(CompareGlass);

                int _sum = 0;
                foreach (String _glassPath in _glassPaths)
                {
                    _activeDoc = FileLoadGlass(_glassPath);
                    if (_activeDoc == null) continue;

                    // change bat information
                    if (ckbFileUpdate.Checked && _batInfo != null)
                    {
                        _activeDoc.BatInfo.BatDirectory = _batPath;
                        _activeDoc.BatInfo.BatBangou = _batName;
                        _activeDoc.BatInfo.Grid = _batInfo.Grid;
                        _activeDoc.BatInfo.ChousaJisuu = _batInfo.ChousaJisuu;
                        _activeDoc.BatInfo.OoChiku = _batInfo.OoChiku;
                        _activeDoc.BatInfo.ChuushouChiku = _batInfo.ChuushouChiku;
                        _activeDoc.BatInfo.Ikoumei = _batInfo.Ikoumei;
                        _activeDoc.BatInfo.Dosoumei = _batInfo.Dosoumei;
                        _activeDoc.BatInfo.Date = _batInfo.Date;                        
                    }

                    // change glass and mokkans' start id
                    if (ckbFileUpdate.Checked && _startRIdSet)
                    {
                        _activeDoc.GlassInfo.KaishiRBangou = _start;
                        _activeDoc.MokkanList.ReArrangeId(_start);
                    }

                    _activeDoc.Save(false);
                    _start += _activeDoc.MokkanList.Count;
                    _sum += _activeDoc.MokkanList.Count;

                    _statWriter.WriteLine(String.Format("{0},{1},{2},{3},{4},{5}", _batManager.BatInfo.Grid, _batName, Path.GetFileNameWithoutExtension(_glassPath),
                                                                                    _activeDoc.GlassInfo.KaishiRBangou, _activeDoc.GlassInfo.KaishiRBangou + _activeDoc.MokkanList.Count - 1, _activeDoc.MokkanList.Count));
                }
                _statWriter.Flush();

                if(_startRIdSet)
                    _statGlassWriter.WriteLine(String.Format("{0},{1},{2},{3},{4},{5}", _batManager.BatInfo.Grid, _batName, _batManager.GlassFileNames.Count, _startRId, _start - 1, _sum));
                else
                    _statGlassWriter.WriteLine(String.Format("{0},{1},{2},{3}", _batManager.BatInfo.Grid, _batName, _batManager.GlassFileNames.Count, _sum));
                _statGlassWriter.Flush();            

                // Periodically check for a Cancellation
                // If the user clicks the cancel button, or tries to close
                // the progress form the m_fmProgress.Cancel flag will be set to true.
                if (_fmProgress.Cancel)
                {
                    // Set the e.Cancel flag so that the WorkerCompleted event
                    // knows that the process was canceled.
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Finish the work
        /// </summary>        
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. First we should hide the
            // modal Progress Form to unlock the UI. The we need to inspect our
            // response to see if an error occurred, a cancel was requested or
            // if we completed successfully.

            // Hide the Progress Form
            if (_fmProgress != null)
            {
                _fmProgress.Hide();
                _fmProgress = null;
            }

            // Check to see if an error occurred in the 
            // background process.
            if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message);                
                return;
            }

            // Check to see if the background process was canceled.
            if (e.Cancelled)
            {
                MessageBox.Show(this, "実行キャンセル");
                return;
            }

            // Everything completed normally.
            // process the response using e.Result
            MessageBox.Show(this, "実行完了");
        }
        
        /// <summary>
        /// Open bat file
        /// </summary>        
        private void FileOpenBat(String filename)
        {
            XmlDocument doc = new XmlDocument();
            IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
            IXmlContext cnt;
            XmlElement _eleBat;

            // Deserialize object from xml format        
            doc.Load(filename);
            cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);
            doc = cnt.Document;
            _eleBat = doc.DocumentElement;
            _batManager = cnt.FromXml(_eleBat) as MkaBatManager;
            _batManager.BatInfo.BatDirectory = Path.GetDirectoryName(filename);
            for (int i = 0; i < _batManager.ImageFileNames.Count; i++)
            {
                _batManager.ImageFilePaths.Add(_batManager.BatInfo.BatDirectory + "\\" + _batManager.ImageFileNames[i]);
                _batManager.GlassFilePaths.Add(_batManager.BatInfo.BatDirectory + "\\" + Path.GetFileNameWithoutExtension(_batManager.ImageFileNames[i]) + MkaDefine.MkaFileExt);
            }
        } 

        /// <summary>
        /// Load glass information from xml file
        /// </summary>
        private MkaDocument FileLoadGlass(String filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // Deserialize object from xml format                    
            IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
            IXmlContext cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);

            XmlElement _eleBat = cnt.Document.DocumentElement;
            XmlNode _eleGlass = _eleBat.FirstChild;
            XmlNode _eleMk = _eleGlass.FirstChild;
            MkaDocument mkaDoc = new MkaDocument(null);
            mkaDoc.GlassInfo = cnt.FromXml(_eleGlass as XmlElement) as MkaGlassInfo;
            mkaDoc.BatInfo = cnt.FromXml(_eleBat as XmlElement) as MkaBatInfo;
            mkaDoc.MokkanList = cnt.FromXml(_eleMk as XmlElement) as MokkanList;
            mkaDoc.BatInfo.BatDirectory = Path.GetDirectoryName(filename);
            mkaDoc.GlassInfo.GlassFilePath = filename;
            mkaDoc.GlassInfo.MokkanKazu = mkaDoc.MokkanList.Count;
            mkaDoc.GlassImage = Path.GetDirectoryName(filename) + "\\" + mkaDoc.GlassInfo.ImageFileName;

            int maxRid = mkaDoc.MokkanList.GetMaxRBangou() + 1;
            if (maxRid == 1)
                MkaMokkanInfo.LastRBangou = mkaDoc.GlassInfo.KaishiRBangou;
            else
                MkaMokkanInfo.LastRBangou = maxRid;
            mkaDoc.MokkanList.KaishiRBangou = MkaMokkanInfo.LastRBangou;

            return mkaDoc;
        }

        /// <summary>
        /// Reset database
        /// </summary>
        private void ResetDb()
        {
            MkaDBConnect dbCon = new MkaDBConnect();

            // open connection
            if (dbCon.OpenConnection() == false)
            {
                MkaMessage.ShowError(MkaMessage.ErrDBConnect);
                return;
            }
            MySqlConnection con = dbCon.GetConnection();

            // reset database
            MySqlCommand cmd = new MySqlCommand("reset_db", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
   
            // close connection
            dbCon.CloseConnection();
        }         

        /// <summary>
        /// Compare to sort bat file list
        /// </summary>        
        private int CompareBat(String bat1, String bat2)
        {
            try
            {
                if (bat1.Contains("削り"))
                    bat1 = Path.GetFileName(bat1).Remove(0, 2);
                else
                    bat1 = Path.GetFileName(bat1).Remove(0, 1);
                if (bat2.Contains("削り"))
                    bat2 = Path.GetFileName(bat2).Remove(0, 2);
                else
                    bat2 = Path.GetFileName(bat2).Remove(0, 1);
                int batid1 = Int32.Parse(bat1);
                int batid2 = Int32.Parse(bat2);
                return batid1 - batid2;
            }
            catch
            {
                return bat1.CompareTo(bat2);
            }
            
        }

        /// <summary>
        /// Compare to sort glass file list
        /// </summary>        
        private int CompareGlass(String glass1, String glass2)
        {
            try
            {
                glass1 = Path.GetFileNameWithoutExtension(glass1).Remove(0, 4);
                glass2 = Path.GetFileNameWithoutExtension(glass2).Remove(0, 4);
                int glassid1 = Int32.Parse(glass1);
                int glassid2 = Int32.Parse(glass2);
                return glassid1 - glassid2;
            }
            catch
            {
                return glass1.CompareTo(glass2);
            }            
        }

        /// <summary>
        /// Read excavation information
        /// </summary>
        private void ReadExcavationInfo()
        {
            try
            {
                _batInfos = new List<BatInfo>();
                StreamReader _reader = new StreamReader(_infoFile, Encoding.GetEncoding("Shift-JIS"));
                while(!_reader.EndOfStream)
                {
                    String line = _reader.ReadLine();
                    String[] infos = line.Split(',');
                    BatInfo batinfo = new BatInfo();
                    batinfo.Grid = Int32.Parse(infos[0]);
                    batinfo.ChousaJisuu = Int32.Parse(infos[1]);
                    batinfo.OoChiku = infos[2];
                    batinfo.ChuushouChiku = infos[3];
                    batinfo.Ikoumei = infos[4];
                    batinfo.Dosoumei = infos[5];
                    batinfo.Date = DateTime.Parse(infos[6]);
                    _batInfos.Add(batinfo);
                }
            }
            catch
            {
                _batInfos.Clear();
            }
        }

        /// <summary>
        /// Enable/disable file update
        /// </summary>
        private void ckbFileUpdate_CheckedChanged(object sender, EventArgs e)
        {
            grpbInput.Enabled = ckbFileUpdate.Checked;
            if (!ckbFileUpdate.Checked && !ckbRegisterDb.Checked)
            {
                MessageBox.Show(this, "調査情報を更新する，またはデータベースへ一括に登録するか，指定ください．", "木簡情報一括更新・登録", MessageBoxButtons.OK);
                ckbFileUpdate.Checked = true;
                ckbRegisterDb.Checked = false;
            }
        }

        /// <summary>
        /// Have to check one check box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbRegisterDb_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbFileUpdate.Checked && !ckbRegisterDb.Checked)
            {
                MessageBox.Show(this, "調査情報を更新する，またはデータベースへ一括に登録するか，指定ください．", "木簡情報一括更新・登録", MessageBoxButtons.OK);
                ckbFileUpdate.Checked = false;
                ckbRegisterDb.Checked = true;
            }
        }
    }

    class BatInfo
    {
        public int Grid;
        public int ChousaJisuu;
        public String OoChiku;
        public String ChuushouChiku;
        public String Ikoumei;
        public String Dosoumei;
        public DateTime Date;
    }
}
