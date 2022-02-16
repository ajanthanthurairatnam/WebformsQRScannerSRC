using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ALCS_Library.ALCS_Data
{

    #region "Config Reader"

    /// <summary>
    /// Read the configuration file and return the proper string.
    /// </summary>
    public static class ALCS_ConfigReader
    {
        /// <summary>
        /// The Default case point to the app DB. 
        /// </summary>
        public static string GetConnectionStr()
        {
            return GetConnectionStr("driver_DB");
        }

        /// <summary>
        /// specify the dbname you want to connect to.
        /// </summary>
        /// <param name="dbName"></param>
        public static string GetConnectionStr(string dbName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
            }
            catch
            {
                return "";
            }
        }
    }
    #endregion
}



