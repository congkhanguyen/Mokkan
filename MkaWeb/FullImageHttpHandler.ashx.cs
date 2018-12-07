using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Configuration;

namespace MkaWeb
{
    public class FullImageHttpHandler : IHttpHandler
    {
        public static int MAX_SIZE = 400;

        public void ProcessRequest(HttpContext context)
        {
            //int RBangou = Convert.ToInt32(context.Request.QueryString["RBangou"]);
            //if (RBangou > 0)
            //{
            //    context.Response.BinaryWrite(RetrieveProductImage(RBangou));
            //    context.Response.End();
            //}

            int RBangou = Convert.ToInt32(context.Request.QueryString["RBangou"]);
            Image image = GetImage(RBangou);

            context.Response.ContentType = "image/png";
            image.Save(context.Response.OutputStream, ImageFormat.Png);            
        }

        private Image GetImage(int RBangou)
        {
            MkaDBConnect dbCon = new MkaDBConnect();
            MySqlConnection con;
            MySqlCommand cmd;

            // open connection
            if (dbCon.OpenConnection() != 1)
                return null;
            con = dbCon.GetConnection();

            cmd = new MySqlCommand("get_mokkan_simple", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("_rBangou", RBangou));
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

            // close connection            
            dbCon.CloseConnection();

            if (dt.Rows.Count == 0) return new Bitmap(50,50);

            String path = String.Format("{0}\\{1}\\{2}\\{3}\\{1}_{2}_{3}_{4}.jpg", ConfigurationSettings.AppSettings["MokkanPath"], dt.Rows[0][0], dt.Rows[0][1], dt.Rows[0][2], RBangou);
            Bitmap bmp = new Bitmap(path);
            int maxsize = Math.Max(bmp.Width, bmp.Height);
            float percentage = 1.0f;
            if (maxsize > MAX_SIZE)
                percentage = (float)MAX_SIZE / maxsize;
            Image ret = Resize(bmp, percentage);
            bmp.Dispose();

            return ret;
        }

        /// <summary>
        /// Method for resizing an image
        /// </summary>
        /// <param name="img">the image to resize</param>
        /// <param name="percentage">Percentage of change (i.e for 105% of the original provide 105)</param>
        /// <returns></returns>
        private Image Resize(Bitmap img, float percentage)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedW = (int)(originalW * percentage + 0.5);
            int resizedH = (int)(originalH * percentage + 0.5);

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);

            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);

            //dispose and free up the resources
            graphic.Dispose();

            //return the image
            return (Image)bmp;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }

        }
    }

}
