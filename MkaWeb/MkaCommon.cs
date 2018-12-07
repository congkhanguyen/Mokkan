using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MkaWeb
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
    
    public class MkaCommon
    {
        /// <summary>
        /// Logger
        /// </summary>
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Resize image to normalized size
        /// </summary>        
        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
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
        /// Get condition string belongs to search type
        /// </summary>        
        public static String GetCondition(String condition, SearchType type)
        {
            int count = 0;
            String temp = "";

            count = condition.Length;
            if (count <= 1)
                return condition;

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
    }

    public class MkaMessage
    {
        /// <summary>
        /// DB connection succcess
        /// </summary>
        public static String SuccessDBConnect = "データベース接続が成功しました.";

        /// <summary>
        /// DB connection failed
        /// </summary>
        public static String ErrDBConnect = "データベース接続に失敗しました.";

        /// <summary>
        /// Invalid username or password
        /// </summary>
        public static String ErrDBInvalidID = "無効なユーザー名、またはパスワードです.";
    }
}
