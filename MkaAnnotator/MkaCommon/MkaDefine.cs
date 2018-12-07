using System;
using System.Drawing;
using System.Configuration;
using System.Windows.Forms;

namespace MokkAnnotator.MkaCommon
{
    /// <summary>
    /// Definition of Data
    /// </summary>
    public class MkaDefine
    {
        #region System Information
        /// <summary>
        /// Title
        /// </summary>
        public const string AssemblyTitle = "MokkAnnotator";
        public const string AssemblyTitleJp = "木簡アノテーター";

        /// <summary>
        /// Version
        /// </summary>
        public const string AssemblyVersion = "3.0";

        /// <summary>
        /// Company
        /// </summary>
        public const string AssemblyCompany = "Nara National Research Institute for Cultural Properties";
        public const string AssemblyCompanyJp = "奈良国立文化財研究所";

        /// <summary>
        /// Product name
        /// </summary>
        public const string AssemblyProduct = "MokkAnnotator";

        /// <summary>
        /// Copyright
        /// </summary>
        public const string AssemblyCopyright = "Copyright © 2015 Nakagawa Laboratory. All rights reserved.";

        #endregion

        #region Program Configuration

        /// <summary>
        /// Logger
        /// </summary>
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Form title
        /// </summary>
        public static string FormFormat = "MokkAnnotator（{0}）";

        /// <summary>
        /// Application execute path
        /// </summary>
        public static string ExecutablePath = Application.ExecutablePath;

        /// <summary>
        /// Product name
        /// </summary>
        public static string ProductName = Application.ProductName;
            

        #region Bat file configuration

        /// <summary>
        /// Bat file extension
        /// </summary>
        public static string BmkFileExt = ".mkb";

        /// <summary>
        /// Bat file program id
        /// </summary>
        public static string BmkFileProgId = "mkbfile";

        /// <summary>
        /// Bat file description
        /// </summary>
        public static string BmkFileDescription = "Mokkan Glass file";

        /// <summary>
        /// Bat file's untitled name
        /// </summary>
        public static string BmkFileUntitled = "UntitledBat";

        /// <summary>
        /// Bat file filter
        /// </summary>
        public static string BmkFilter = "バット管理ファイル(*" + BmkFileExt + ")|*" + BmkFileExt;

        #endregion Bat file configuration

        #region Glass file configuration

        /// <summary>
        /// Glass file extension
        /// </summary>
        public static string MkaFileExt = ".mkg";

        /// <summary>
        /// Glass file program id
        /// </summary>
        public static string MkaFileProgId = "mkgfile";

        /// <summary>
        /// Glass file description
        /// </summary>
        public static string MkaFileDescription = "Mokkan Glass file";

        /// <summary>
        /// Glass file's untitled name
        /// </summary>
        public static string MkaFileUntitled = "UntitledGlass";

        /// <summary>
        /// Glass file filter
        /// </summary>
        public static string MkaFilter = "木簡ガラス板ファイル(*" + MkaFileExt + ")|*" + MkaFileExt;

        #endregion Glass file configuration

        #region Input/Output

        /// <summary>
        /// Path to save mokkan images
        /// </summary>
        public static String MokkanPath = "";

        /// <summary>
        /// Flag to set save mokkan image as saving or not
        /// </summary>
        public static bool SaveMokkanImageAsSaving = false;

        /// <summary>
        /// Flag to set save mokkan boundary coordinates as saving or not
        /// </summary>
        public static bool SaveMokkanBoundaryAsSaving = false;

        /// <summary>
        /// Flag to set registering to database as saving or not
        /// </summary>
        public static bool RegisterToDbAsSaving = true;

        /// <summary>
        /// Mokkan information export extension
        /// </summary>
        public static string IOCSVFileExt = ".csv";

        /// <summary>
        /// Mokkan information export file filter
        /// </summary>
        public static string IOCSVFilter = "木簡データ出力ファイル(*" + IOCSVFileExt + ")|*" + IOCSVFileExt;


        /// <summary>
        /// Mokkan information export excel extension
        /// </summary>
        public static string IOEXCELFileExt = ".xlsx";

        /// <summary>
        /// Mokkan information export excel file filter
        /// </summary>
        public static string IOEXCELFilter = "木簡データ出力エクセルファイル(*" + IOEXCELFileExt + ")|*" + IOEXCELFileExt;

        /// <summary>
        /// Support image filter
        /// </summary>
        public static String IOImageFilter = "JPEG(*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP(*.bmp)|*.bmp|GIF(*.gif)|*.gif|TIFF(*.tif;*tiff)|*.tif;*tiff|PNG(*.png)|*.png";

        /// <summary>
        /// Support image filter (all)
        /// </summary>
        public static String IOImageFilterExt = "*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*tiff;*.png";
        public static String IOImageFilterAll = "すべてのイメージの種類|" + IOImageFilterExt;

        #endregion Input/Output

        #region Properties

        /// <summary>
        /// Supported line weight
        /// </summary>
        public static double[] LineWeight = { 0.25, 0.5, 0.75, 1, 1.5, 2.25, 3, 4.5, 6 };

        /// <summary>
        /// Supported fill area transparency
        /// </summary>
        public static int[] Transparency = { 0, 10, 25, 50, 75, 100, 125, 150, 255 };

        /// <summary>
        /// Zoom scale 
        /// </summary>
        public static int[] Zooms = new int[9] { 25, 50, 75, 100, 125, 150, 200, 300, 500 };

        /// <summary>
        /// Date format
        /// </summary>
        public static String DBDateFormat = "yyyyMMdd";
        public static String DateFormat = "yyyy/MM/dd";
        public static String DatePrintFormat = "yyyy年MM月dd日";

        /// <summary>
        /// Default datetime
        /// </summary>
        public static DateTime StartDate = new DateTime(1990,1,1);

        /// <summary>
        /// Remain id show font
        /// </summary>
        public static String RDefaultFontName = "Arial";
        public static float RDefaultFontSize = 24.0f;

        #endregion Properties

        #region Auto-zoning

        /// <summary>
        /// Threshold value for binarization
        /// </summary>
        public static int THRESHOLD = 140;

        /// <summary>
        /// Number of mokkans on a glass
        /// </summary>
        public static int MOKKAN_NUM = 10;

        /// <summary>
        /// Restrict ratio between width and height of a mokkan
        /// </summary>
        public static int SIZE_RATIO = 20;

        /// <summary>
        /// Restrict size of a mokkan
        /// </summary>
        public static int MIN_SIZE = 20;

        /// <summary>
        /// Restrict area of a mokkan
        /// </summary>
        public static int MIN_AREA = 100;

        /// <summary>
        /// Tolerance of polygon approximation
        /// </summary>
        public static float TOLERANCE = 9;

        #endregion Auto-zoning    

        /// <summary>
        /// Print format
        /// </summary>
        public static Pen PrintBorderPen = new Pen(Color.Black, 1);
        public static Pen PrintBoundPen = new Pen(Color.Black, 1.5f);
        public static String PrintFontName = "Arial"; 
        public static Brush PrintTextBrush = new SolidBrush(Color.Black);
        public static Brush PrintHeaderBrush = new SolidBrush(Color.Blue);
        public static Brush PrintBackColor = new SolidBrush(Color.LightYellow);
        public static int PrintLabelSize = 11;
        public static int PrintContentSize = 12;
        public static int PrintHeaderSize = 12;
        public static int PrintTextSize = 11;
        public static int PrintHeaderHeight = 30;
        public static int PrintRowHeight = 25;

        #endregion

        #region Database
        
        /// <summary>
        /// Server name
        /// </summary>
        public static String DbServer = "localhost";

        /// <summary>
        /// Database name
        /// </summary>
        public static String DbDatabase = "mka";

        /// <summary>
        /// Username
        /// </summary>
        public static String DbUsername = "root";

        /// <summary>
        /// Password
        /// </summary>
        public static String DbPassword = "mokkan";

        #endregion

        #region Registry Information

        /// <summary>
        /// Registry root key
        /// </summary>
        public static Microsoft.Win32.RegistryKey RootKey = Microsoft.Win32.Registry.CurrentUser;
        
        /// <summary>
        /// Registry key name
        /// </summary>
        public static String RegKey = "Software\\Nabunken\\MokkAnnotator";       

        /// <summary>
        /// Registry path to save window state
        /// </summary>
        public static String WindowStateKey = RegKey + @"\WindowState";

        /// <summary>
        /// Registry path to save graphic properties
        /// </summary>
        public static String GraphicsKey = RegKey + @"\Graphics";

        /// <summary>
        /// Registry path to save recent bat files
        /// </summary>
        public static String RecentBats = RegKey + @"\RecentBats";

        /// <summary>
        /// Registry path to save recent glass files
        /// </summary>
        public static String RecentGlasses = RegKey + @"\RecentGlasses";

        /// <summary>
        /// Registry path to save last registered information
        /// </summary>
        public static String LastUsedInfo = RegKey + @"\LastInfo";
              
        /// <summary>
        /// Store last opened path in registry
        /// </summary>
        public static String RecentGlassPath = "RecentGlassPath";
        public static String RecentBatPath = "RecentBatPath";


        #endregion

        #region Graphical configuration

        /// <summary>
        /// Border line's color
        /// </summary>
        public static Color DefaultBorderColor = Color.DodgerBlue;

        /// <summary>
        /// Border line's pen width
        /// </summary>
        public static float DefaultPenWidth = 1;

        /// <summary>
        /// Fill area's color
        /// </summary>
        public static Color DefaultFillColor = Color.DeepSkyBlue;

        /// <summary>
        /// Fill area's transparent
        /// </summary>
        public static int DefaultColorAlpha = 50;

        #endregion

        #region Registry entry name

        /// <summary>
        /// Graphical properties' entry names
        /// </summary>        
        public static String RFontSize = "RFontSize";
        public static String GraphicBorderColor = "BorderColor";        
        public static String GraphicPenWidth = "PenWidth";       
        public static String GraphicFillColor = "FillColor";
        public static String GraphicColorAlpha = "FillColorAlpha";

        /// <summary>
        /// Bat information's entry names
        /// </summary>
        public static String BatChousaJisuu = "ChousaJisuu";
        public static String BatOoChiku = "OoChiku";
        public static String BatChuushouChiku = "ChuushouChiku";
        public static String BatIkoumei = "Ikoumei";
        public static String BatDosoumei = "Dosoumei";
        public static String BatGrid = "Grid";
        public static String BatDate = "Date";
        public static String BatBangou = "BatBangou";
        public static String BatPath = "BatPath";

        /// <summary>
        /// Glass information's entry names
        /// </summary>
        public static String GlassItaBangou = "GlassItaBangou";
        public static String GlassKaishiRBangou = "KaishiRBangou";

        /// <summary>
        /// Mokkan information's entry names
        /// </summary>
        public static String MokkanRBangou = "RBangou";

        #endregion
        
        #region Label name

        /// <summary>
        /// Bat information's label names
        /// </summary>
        public const String BatLblJyouhou = "①バット情報";
        public const String BatLblChousaJisuu = "1．調査次数";
        public const String BatLblOoChiku = "2．調査地区・大地区";
        public const String BatLblChuushouChiku = "3．調査地区・中小地区";
        public const String BatLblIkoumei = "4．遺構名";
        public const String BatLblDosoumei = "5．土層名";
        public const String BatLblGrid = "6．グリッド";
        public const String BatLblDate = "7．出土日付";
        public const String BatLblBatBangou = "8．バット番号";
        public const String BatLblBatPath = "9．バットの場所";

        /// <summary>
        /// Glass information's entry names
        /// </summary>
        public const String GlassLblJyouhou = "②ガラス情報";
        public const String GlassLblItaBangou = "1．ガラス板番号";
        public const String GlassLblMokkanKazu = "2．木簡の数";
        public const String GlassLblKaishiRBangou = "3．開始のR番号";        
        public const String GlassLblImagePath = "4．画像のファイル名";
        public const String GlassLblWidth = "5．画像の広さ";
        public const String GlassLblHeight = "6．画像の高さ";        

        /// <summary>
        /// Mokkan information's entry names
        /// </summary>
        public const String MokkanLblJyouhou = "①木簡情報";
        public const String MokkanLblRBangou = "1．R番号";
        public const String MokkanLblKariShakubun = "2．仮釈文";
        public const String MokkanLblGaihouShoshuuJyouhou = "3．概報所収情報";
        public const String MokkanLblShasinBangouJyouhou = "4．写真番号情報";
        public const String MokkanLblBikou = "5．備考";

        /// <summary>
        /// Selection format's entry names
        /// </summary>
        public const String FormatLblInformation = "②選択範囲";
        public const String FormatLblRShowPosition = "1．R番号表示位置";
        public const String FormatLblBorderColor = "2．枠線の色";
        public const String FormatLblPenWidth = "3．枠線の太さ";
        public const String FormatLblFillColor = "4．塗りつぶしの色";
        public const String FormatLblFillColorAlpha = "5．塗りつぶしの透明性";

        #endregion

        #region XML tag

        /// <summary>
        /// Bat information's tag names
        /// </summary>
        public const String BatTagChousaJisuu = "chousa_jisuu";
        public const String BatTagOoChiku = "oo_chiku";
        public const String BatTagChuushouChiku = "chuushou_chiku";
        public const String BatTagIkoumei = "ikoumei";
        public const String BatTagDosoumei = "dosoumei";
        public const String BatTagGrid = "grid";
        public const String BatTagDate = "date";        
        public const String BatTagBatBangou = "bat_bangou";        

        /// <summary>
        /// Glass information's tag names
        /// </summary>
        public const String GlassTagItaBangou = "glass_ita_bangou";
        public const String GlassTagKaishiRBangou = "kaishi_r_bangou";
        public const String GlassTagImageName = "image_file";
        public const String GlassTagWidth = "width";
        public const String GlassTagHeight = "height";
        public const String GlassRFontSize = "r_fontsize";

        /// <summary>
        /// Mokkan information's tag names
        /// </summary>
        public const String MokkanTagRBangou = "r_bangou";
        public const String MokkanTagKariShakubun = "kari_shakubun";
        public const String MokkanTagGaihouShoshuuJyouhou = "gaihou_shoshuu_jyouhou";
        public const String MokkanTagShasinBangouJyouhou = "shasin_bangou_jyouhou";
        public const String MokkanTagBikou = "bikou";

        /// <summary>
        /// Selection format's tag names
        /// </summary>
        public const String FormatTagRShowPosition = "r_bangou_hyouji_ichi";
        public const String FormatTagRShowLocation = "r_bangou_location";
        public const String FormatTagRFontSize = "r_fontsize";
        public const String FormatTagBorderColor = "border_color";
        public const String FormatTagPenWidth = "pen_width";
        public const String FormatTagFillColor = "fill_color";
        public const String FormatTagFillColorAlpha = "alpha";

        #endregion

        public static String GetString(object str)
        {
            if(str == null) return "";
            else return str.ToString();
        }

        /// <summary>
        /// Load application settings
        /// </summary>
        public static void LoadAppSettings()
        {
            // graphics property
            LoadGraphicsProperty();

            // database
            LoadDatabase();

            // saving 
            LoadSave();

            // auto select
            LoadAutoSelect();
        }

        /// <summary>
        /// Load graphic settings
        /// </summary>
        public static void LoadGraphicsProperty()
        {
            try { RDefaultFontSize = float.Parse(ConfigurationManager.AppSettings["RFontSize"]); }
            catch { RDefaultFontSize = 24.0f; }

            try { DefaultBorderColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["BorderColor"]); }
            catch { DefaultBorderColor = Color.DodgerBlue; }

            try { DefaultPenWidth = (float)Double.Parse(ConfigurationManager.AppSettings["PenWidth"]); }
            catch { DefaultPenWidth = 1; }

            try { DefaultFillColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["FillColor"]); }
            catch { DefaultFillColor = Color.DeepSkyBlue; }

            try { DefaultColorAlpha = Int32.Parse(ConfigurationManager.AppSettings["Alpha"]); }
            catch { DefaultColorAlpha = 50; }
        }

        /// <summary>
        /// Load database settings
        /// </summary>
        public static void LoadDatabase()
        {
            try { DbServer = ConfigurationManager.AppSettings["Server"]; }
            catch { }

            try { DbDatabase = ConfigurationManager.AppSettings["Database"]; }
            catch { }

            try { DbUsername = ConfigurationManager.AppSettings["Uid"]; }
            catch { }

            try { DbPassword = ConfigurationManager.AppSettings["Password"]; }
            catch { }
        }

        /// <summary>
        /// Load saving settings
        /// </summary>
        public static void LoadSave()
        {
            try { MokkanPath = (ConfigurationManager.AppSettings["MokkanPath"]); }
            catch { }

            try { SaveMokkanImageAsSaving = (ConfigurationManager.AppSettings["SaveMokkanImageAsSaving"] == "true" ? true : false); }
            catch { }

            try { SaveMokkanBoundaryAsSaving = (ConfigurationManager.AppSettings["SaveMokkanBoundaryAsSaving"] == "true" ? true : false); }
            catch { }

            try { RegisterToDbAsSaving = (ConfigurationManager.AppSettings["RegisterToDbAsSaving"] == "true" ? true : false); }
            catch { }
        }

        /// <summary>
        /// Load auto select setting
        /// </summary>
        public static void LoadAutoSelect()
        {
            try { MOKKAN_NUM = Int32.Parse(ConfigurationManager.AppSettings["MokkanNumber"]); }
            catch { MOKKAN_NUM = 10; }

            try { MIN_AREA = Int32.Parse(ConfigurationManager.AppSettings["MinArea"]); }
            catch { MIN_AREA = 100; }

            try { SIZE_RATIO = Int32.Parse(ConfigurationManager.AppSettings["SizeRatio"]); }
            catch { SIZE_RATIO = 10; }

            try { THRESHOLD = Int32.Parse(ConfigurationManager.AppSettings["Threshold"]); }
            catch { THRESHOLD = 154; }

            try { TOLERANCE = (float)Double.Parse(ConfigurationManager.AppSettings["Tolerance"]); }
            catch { TOLERANCE = 9; }
        }

        /// <summary>
        /// Save application settings
        /// </summary>
        public static void SaveAppSettings()
        {
            ConfigurationManager.GetSection("appSettings");
            Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // graphics property
            if (Config.AppSettings.Settings["RFontSize"] == null)
            {
                Config.AppSettings.Settings.Add("RFontSize", RDefaultFontSize.ToString());
            }            
            Config.AppSettings.Settings["BorderColor"].Value = ColorTranslator.ToHtml(DefaultBorderColor);
            Config.AppSettings.Settings["PenWidth"].Value = DefaultPenWidth.ToString();
            Config.AppSettings.Settings["FillColor"].Value = ColorTranslator.ToHtml(DefaultFillColor);
            Config.AppSettings.Settings["Alpha"].Value = DefaultColorAlpha.ToString();

            // database
            Config.AppSettings.Settings["Server"].Value = DbServer;
            Config.AppSettings.Settings["Database"].Value = DbDatabase;
            Config.AppSettings.Settings["Uid"].Value = DbUsername;
            Config.AppSettings.Settings["Password"].Value = DbPassword;

            // auto select
            Config.AppSettings.Settings["MokkanNumber"].Value = MOKKAN_NUM.ToString();
            Config.AppSettings.Settings["MinArea"].Value = MIN_AREA.ToString();
            Config.AppSettings.Settings["SizeRatio"].Value = SIZE_RATIO.ToString();
            Config.AppSettings.Settings["Threshold"].Value = THRESHOLD.ToString();
            Config.AppSettings.Settings["Tolerance"].Value = TOLERANCE.ToString();

            // Save path
            Config.AppSettings.Settings["MokkanPath"].Value = MokkanPath;

            // save configuration
            Config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}