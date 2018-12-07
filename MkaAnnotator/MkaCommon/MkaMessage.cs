using System;
using System.Windows.Forms;

namespace MokkAnnotator.MkaCommon
{
    /// <summary>
    /// Definition of Display Message
    /// </summary>
    public class MkaMessage
    {
        /// <summary>
        /// Caption
        /// </summary>
        public static String AppCaption = "MokkAnnotator";

        #region Error
        /// <summary>
        /// Invalid file error
        /// </summary>
        public static String ErrInvalid = "指定した\"{0}\"が無効です.";

        /// <summary>
        /// Invalid path error
        /// </summary>
        public static String ErrInvalidPath = "指定したフォルダが無効です.";

        public static String ErrIncorrectFilePath = "フォルダとグリッド、バッド、ガラスは違います" + Environment.NewLine + @"フォルダフォマットは　...\グリッド\バッド\ガラス板.jpg";

        /// <summary>
        /// DB connection failed
        /// </summary>
        public static String ErrDBConnect = "データベース接続に失敗しました.";

        /// <summary>
        /// Invalid username or password
        /// </summary>
        public static String ErrDBInvalidID = "無効なユーザー名、またはパスワードです.";

        /// <summary>
        /// Load file failed
        /// </summary>
        public static String ErrOpenFile = "ファイルの読み込みに失敗しました.";

        /// <summary>
        /// Save file failed
        /// </summary>
        public static String ErrSaveFile = "ファイルの保存に失敗しました.";

        /// <summary>
        /// Input string request
        /// </summary>
        public static String ErrInputRequest = "{0}を入力してください.";

        /// <summary>
        /// Input number request
        /// </summary>
        public static String ErrNumberRequest = "{0}は整数を入力してください.";

        /// <summary>
        /// Input datetime request
        /// </summary>
        public static string ErrDateRequest = "{0}は日付を入力してください.";

        /// <summary>
        /// Empty name
        /// </summary>
        public static String ErrEmptyName = "名前を入力してください";

        /// <summary>
        /// Invalid name
        /// </summary>
        public static String ErrInvalidName = "名前が無効です．名前には、次の文字を含めることはできません:\n \\ / : * ? \" < > |";

        /// <summary>
        /// Empty path
        /// </summary>
        public static String ErrEmptyPath = "{0}を指定してください.";

        /// <summary>
        /// Duplication
        /// </summary>
        public static String ErrDublication = "{0}が重複しています。";

        /// <summary>
        /// Invalid pen width
        /// </summary>
        public static String ErrInvalidPenWidth = "｢枠線の太さ」は 0.25 以上または 6 以下でなければなりません";

        /// <summary>
        /// Invalid alpha
        /// </summary>
        public static String ErrInvalidAlpha = "｢透明性」は 0 以上または 255 以下でなければなりません";

        #endregion

        #region Info
        /// <summary>
        /// Save file confirmation
        /// </summary>
        public static String InfoSaveQuestion = "ファイル\"{0}\"を保存しますか?";

        #endregion

        #region Warning

        /// <summary>
        /// Load existed file
        /// </summary>
        public static String WarnExistedFile = "ファイル\"{0}\"は既に存在しています.\nファイルを上書きしますか？";

        /// <summary>
        /// Load/save setting from/to registry
        /// </summary>
        public static String WarnSaveRegistry = "設定をレジストリキー保存に失敗しました．";
        public static String WarnLoadRegistry = "設定をレジストリキー取得に失敗しました．";

        #endregion

        /// <summary>
        /// Show Information
        /// </summary>
        /// <param name="msg">info message</param>
        public static void ShowInfo(String msg)
        {
            MessageBox.Show(msg, AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show Error Message
        /// </summary>
        /// <param name="msg">Error message</param>
        public static void ShowError(String msg)
        {
            MessageBox.Show(msg, AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show Warning Message
        /// </summary>
        /// <param name="msg">Warning message</param>
        public static void ShowWarning(String msg)
        {
            MessageBox.Show(msg, AppCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}