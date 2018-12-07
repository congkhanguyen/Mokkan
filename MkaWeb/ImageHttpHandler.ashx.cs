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
using System.Configuration;

namespace MkaWeb
{
    public class ImageHttpHandler : IHttpHandler
    {
        public Size _resize = new Size(64, 64);

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

            // get search records
            cmd = new MySqlCommand("get_mokkan_simple", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("_rBangou", RBangou));
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(rdr);

            // close connection            
            dbCon.CloseConnection();
                        
            if (dt.Rows.Count == 0) return new Bitmap(_resize.Width, _resize.Height);

            String path = String.Format("{0}\\{1}\\{2}\\{3}\\{1}_{2}_{3}_{4}.jpg", ConfigurationSettings.AppSettings["MokkanPath"], dt.Rows[0][0], dt.Rows[0][1], dt.Rows[0][2], RBangou);
            Bitmap bmp = new Bitmap(path);
            bmp = MkaCommon.ResizeImage(bmp, _resize);

            return bmp;
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
