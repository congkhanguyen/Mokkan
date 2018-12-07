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
using MySql.Data.MySqlClient;

namespace MkaWeb
{
    /// <summary>
    /// Type of sql query command 
    /// </summary>
    public enum QueryCommandType
    {
        Select, // select command
        Insert, // insert command
        Update, // update command
        Delete  // delete command
    }
    
    public class MkaDBConnect
    {
        // logger
        private static log4net.ILog _log = MkaCommon.Logger;

        private MySqlConnection _connection;    // Database connection
        private string _server;                 // Database server
        private string _database;               // Database name
        private string _uid;                    // Username
        private string _password;               // Password

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaDBConnect()
        {
            string connectionString;

            // set database connection parameters
            _server = ConfigurationSettings.AppSettings["Server"];
            _database = ConfigurationSettings.AppSettings["Database"];
            _uid = ConfigurationSettings.AppSettings["Uid"];
            _password = ConfigurationSettings.AppSettings["Password"];

            // set connection string            
            connectionString = "SERVER=" + _server + ";" + "DATABASE=" + _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

            // new instance of connection
            _connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Open connection to database
        /// </summary>
        /// <returns>true if opening connection is success, otherwise false.</returns>
        public int OpenConnection()
        {
            try
            {
                _connection.Open();
                return 1;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:     // cannot connect to server
                        _log.Error(MkaMessage.ErrDBConnect);
                        return 0;
                    case 1045:  // invalid username/password
                        _log.Error(MkaMessage.ErrDBInvalidID);
                        return 1045;
                    default:
                        return 0;
                }                
            }
        }

        /// <summary>
        /// Open connection to database
        /// </summary>
        /// <returns>true if opening connection is success, otherwise false.</returns>
        public static int TryOpenConnection(String server, String database, String id, String password)
        {
             String connectStr = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + id + ";" + "PASSWORD=" + password + ";";
             MySqlConnection connect = new MySqlConnection(connectStr);

            try
            {
                connect.Open();
                return 1;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:     // cannot connect to server
                        _log.Error(MkaMessage.ErrDBConnect);
                        return 0;
                    case 1045:  // invalid username/password
                        _log.Error(MkaMessage.ErrDBInvalidID);
                        return 1045;
                    default:
                        return 0;
                }
            }
            finally
            {
                connect.Close();
            }
        }

        /// <summary>
        /// Close connection to database
        /// </summary>
        /// <returns>true if closing connection is success, otherwise false.</returns>
        public bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get MySql connection
        /// </summary>        
        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        /// <summary>
        /// Execute sql query
        /// </summary>
        /// <param name="queryString">query string</param>
        /// <param name="queryType">query command type</param>
        /// <param name="tblResult">result of query</param>
        public bool GetSqlResult(String queryString, QueryCommandType queryType, DataTable tblResult)
        {
            try
            {
                // create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(queryString, _connection);

                // execute command
                if (queryType != QueryCommandType.Select)
                    cmd.ExecuteNonQuery();
                else
                {
                    // get data from select query
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(tblResult);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string EscapeSQL(string str)
        {
            str = str.Replace("%", @"\%");
            str = str.Replace("_", @"\_");
            str = str.Replace(@"\", @"\\");
            str = str.Replace("\'", "\\\'");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("\t", "\\t");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\n", "\\n");

            return str;
        }
    }
}
