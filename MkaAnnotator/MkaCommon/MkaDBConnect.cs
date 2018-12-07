using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace MokkAnnotator.MkaCommon
{
    /// <summary>
    /// Type of sql query command 
    /// </summary>
    public enum QueryCommandType
    {
        Select, // select command
        Insert, // inssert command
        Update, // update command
        Delete  // delete command
    }

    public class MkaDBConnect
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;

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
        public bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {                
                switch (ex.Number)
                {
                    case 0:     // cannot connect to server
                        _log.Error(MkaMessage.ErrDBConnect);
                        break;

                    case 1045:  // invalid username/password
                        _log.Error(MkaMessage.ErrDBInvalidID);
                        break;
                }
                return false;
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
