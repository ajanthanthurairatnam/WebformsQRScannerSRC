using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls; 
using System.Collections;
using System.Security.Principal;

using ALCS_Library.ALCS_Data;  
using ALCS_Library.ALCS_Format;  

namespace ALCS_Library.ALCS_Basics
{

    #region "Constants ..."

    /// <summary>
    /// Server Variables ...
    /// </summary>
    public enum ServerVars
    {
        ALL_HTTP,
        ALL_RAW,
        APPL_MD_PATH,
        APPL_PHYSICAL_PATH,
        AUTH_TYPE,
        AUTH_USER,
        AUTH_PASSWORD,
        LOGON_USER,
        REMOTE_USER,
        CERT_COOKIE,
        CERT_FLAGS,
        CERT_ISSUER,
        CERT_KEYSIZE,
        CERT_SECRETKEYSIZE,
        CERT_SERIALNUMBER,
        CERT_SERVER_ISSUER,
        CERT_SERVER_SUBJECT,
        CERT_SUBJECT,
        CONTENT_LENGTH,
        CONTENT_TYPE,
        GATEWAY_INTERFACE,
        HTTPS,
        HTTPS_KEYSIZE,
        HTTPS_SECRETKEYSIZE,
        HTTPS_SERVER_ISSUER,
        HTTPS_SERVER_SUBJECT,
        INSTANCE_ID,
        INSTANCE_META_PATH,
        LOCAL_ADDR,
        PATH_INFO,
        PATH_TRANSLATED,
        QUERY_STRING,
        REMOTE_ADDR,
        REMOTE_HOST,
        REMOTE_PORT,
        REQUEST_METHOD,
        SCRIPT_NAME,
        SERVER_NAME,
        SERVER_PORT,
        SERVER_PORT_SECURE,
        SERVER_PROTOCOL,
        SERVER_SOFTWARE,
        URL,
        HTTP_CONNECTION,
        HTTP_ACCEPT,
        HTTP_ACCEPT_ENCODING,
        HTTP_ACCEPT_LANGUAGE,
        HTTP_HOST,
        HTTP_USER_AGENT,
    }

    #endregion 


    #region "Untility Class ..."

    /// <summary>
	/// Summary description for ALCS_Utils.
	/// </summary>
    public static class ALCS_Utils
    {
        #region "The famous ....Cache Killer ..."

        // generate a unique id to be used to kill the cache.
        // Kill the caching.
        public static string ck()
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            TimeSpan sinceMorning = now.Subtract(today);

            return Convert.ToString(sinceMorning.Ticks);
        }

        /// <summary>
        /// Replace or add the ck string .....
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string ReplaceCK(string inURL)
        {
            string outURL = "";
            string ckPatten = @"[\?\&]ck=(\d+)";
            Match ckMatch = Regex.Match(inURL, ckPatten);

            if (ckMatch.Success)
            {
                outURL = inURL.Replace(ckMatch.Groups[1].ToString(), ck());
            }
            else
            {
                if (inURL.IndexOf(@"?") >= 0)
                {
                    outURL = inURL + "&ck=" + ck();
                }
                else
                {
                    outURL = inURL + "?ck=" + ck();
                }
            }

            // Exit 
            return outURL;
        }

        /// <summary>
        /// Simply return the identity using the site ...
        /// </summary>
        /// <returns></returns>
        public static string WhoIsThere()
        {
            return WindowsIdentity.GetCurrent().Name.ToString();
        }

        #endregion

        #region "Resolve a URL ..."

        /// <summary>
        /// Resolve the URL by applying 
        /// </summary>
        /// <param name="theURL"></param>
        /// <param name="theLocation"></param>
        /// <returns></returns>
        public static string ResolveURL(string theURL, urlLocation theLocation)
        {
            string resolvedURL;

            if ((theLocation == urlLocation.External) || (theLocation == urlLocation.FlyingExternal))
            {
                resolvedURL = theURL;
            }
            else if ((theLocation == urlLocation.Internal) || (theLocation == urlLocation.FlyingInternal))
            {
                if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                {
                    resolvedURL = HttpContext.Current.Request.ApplicationPath + theURL;
                }
                else
                {
                    resolvedURL = HttpContext.Current.Request.ApplicationPath + "/" + theURL;
                }
            }
            else
            {
                if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                {
                    resolvedURL = HttpContext.Current.Request.ApplicationPath + theURL;
                }
                else
                {
                    resolvedURL = HttpContext.Current.Request.ApplicationPath + "/" + theURL;
                }
            }

            // return the URL.
            return resolvedURL;
        }


        /// <summary>
        /// Get the relative path of the URL .....
        /// </summary>
        /// <param name="thePage"></param>
        /// <returns></returns>
        public static string GetBareURL(Page thePage)
        {
            string rawurl = thePage.Request.RawUrl.ToString();
            string bareURL = "";

            if (rawurl.StartsWith("/"))
            {
                bareURL = rawurl.Substring(rawurl.IndexOf('/', 1));
            }
            else
            {
                bareURL = rawurl.Substring(rawurl.IndexOf('/', 0));
            }

            // update the ck value;
            bareURL = ReplaceCK(bareURL);

            // return the URL.
            return bareURL;
        }

        #endregion

        #region "General Tools ..."

        /// <summary>
        /// Join the array list using the separator ....
        /// </summary>
        /// <param name="al"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string ArrayListJoiner(ArrayList al, string sep)
        {
            string joinStr = "";

            // Loop through 
            for (int idx = 0; idx < al.Count; idx++)
            {
                string token = ALCS_DataShift.WhenNull(al[idx], "");
                joinStr += token + sep;
            }

            if (joinStr != "")
            {
                joinStr = joinStr.Substring(0, joinStr.Length - sep.Length);
            }

            // return 
            return joinStr;
        }

        /// <summary>
        /// Connect the lines to build a sentence ....
        /// connect the lines using connect
        /// In Relaxed workmode connect all otherwise only non blank.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="connect"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static string ParagraphBuilder(string[] lines, string connect, WorkMode wm)
        {
            string parStr = "";

            // Loop through 
            for (int idx = 0; idx < lines.Length; idx++)
            {
                string token = ALCS_DataShift.WhenNull(lines[idx], "");

                if ((wm == WorkMode.Strict) && (token == ""))
                {
                    continue;
                }
                else
                {
                    parStr += (parStr == "") ? token : connect + token;
                }
            }

            // return 
            return parStr;
        }

        /// <summary>
        /// Connect the HTML line break ....
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string ParagraphBuilder(string[] lines)
        {
            return ParagraphBuilder(lines, "<br/>", WorkMode.Strict);
        }

        /// <summary>
        /// return the HTML codes for certain symbol
        /// </summary>
        public static string ExtractHTMLSymbol(specialHTML sh)
        {
            string htmlCode = "";

            switch (sh)
            {
                case specialHTML._At:
                    htmlCode = "&#64;";
                    break;
                case specialHTML._CopyRight:
                    htmlCode = "&#169;";
                    break;
                case specialHTML._Space:
                    htmlCode = "&nbsp;";
                    break;
                case specialHTML._Registered:
                    htmlCode = "&#174;";
                    break;
                case specialHTML._UpArrow:
                    htmlCode = "&#8595;";
                    break;
                case specialHTML._DownArrow:
                    htmlCode = "&#8593;";
                    break;
                case specialHTML._DashShort:
                    htmlCode = "&#150;";
                    break;
                case specialHTML._DashLong:
                    htmlCode = "&#151;";
                    break;
                case specialHTML._Bullet:
                    htmlCode = "&#149;";
                    break;
                case specialHTML._BulletDisk:
                    htmlCode = "&#8226;";
                    break;
                case specialHTML._MiddleDot:
                    htmlCode = "&#183;";
                    break;
                case specialHTML._DoubleAngleOpen:
                    htmlCode = "&#171;";
                    break;
                case specialHTML._DoubleAngleClose:
                    htmlCode = "&#187;";
                    break;
                default:
                    htmlCode = "&nbsp;";
                    break;
            }

            // Return HTML Code
            return htmlCode;
        }

        /// <summary>
        /// Turn a relative Path to an absolute one ...
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        public static string ResolveFilePath(string relPath)
        {
            string resolvedPath = System.Web.HttpContext.Current.Request.MapPath("~" + relPath);
            
            // return 
            return resolvedPath;
        }


        #endregion

        #region "Pickers ..."

        /// <summary>
        /// Pick both sides 
        /// </summary>
        /// <param name="delStr"></param>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <returns></returns>
        public static bool Picker_PickBothSides(string originStr, Char delChar, int repeat, out string leftSide, out string rightSide)
        {
            // Initialize.
            leftSide = "";
            rightSide = "";
            Regex re = new Regex(@"^(\S+)([" + delChar.ToString() + @"]{" + repeat.ToString() + "," + repeat.ToString() + @"})(\S+)$");

            // Match and Pick ...
            if (!re.IsMatch(originStr))
            {
                return false;
            }
            else
            {
                leftSide = re.Match(originStr).Groups[1].Value;
                rightSide = re.Match(originStr).Groups[3].Value;
                return true;
            }
        }


        /// <summary>
        /// Pick both sides 
        /// </summary>
        /// <param name="delStr"></param>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <returns></returns>
        public static bool Picker_PickBothSides(string originStr, string delStr, out string leftSide, out string rightSide)
        {
            // Initialize.
            leftSide = "";
            rightSide = "";
            Regex re = new Regex(@"(\S+)" + delStr + @"(\S+)");

            // Match and Pick ...
            if (!re.IsMatch(originStr))
            {
                return false;
            }
            else
            {
                leftSide = re.Match(originStr).Groups[1].Value;
                rightSide = re.Match(originStr).Groups[2].Value;
                return true;
            }
        }

        /// <summary>
        /// Another Version ...
        /// </summary>
        /// <param name="originStr"></param>
        /// <param name="delStr"></param>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        /// <returns></returns>
        public static bool Picker_PickBothSides(string originStr, string delStr, out int leftSide, out int rightSide)
        {
            // Initialize.
            leftSide = -1;
            rightSide = -1;

            // Pickup the String.
            string leftSideStr = "";
            string rightSideStr = "";
            bool pickOK = Picker_PickBothSides(originStr, delStr, out leftSideStr, out rightSideStr);

            // Pick the sides.
            if ((!pickOK) || (!ALCS_Is.isInteger(leftSideStr, WorkMode.Strict)) || (!ALCS_Is.isInteger(rightSideStr, WorkMode.Strict)))
            {
                return false;
            }
            else
            {
                leftSide = Convert.ToInt32(leftSideStr);
                rightSide = Convert.ToInt32(rightSideStr);
                return true;
            }
        }

        #endregion 


        #region "Server Variables ..."

        /// <summary>
    /// Get Server Variables Key ...
    /// </summary>
    /// <param name="sv"></param>
    /// <returns></returns>
    public static string WWW_GetServerVariableKey(ServerVars sv)
    {
        return ALCS_DataShift.EnumAsText(sv);
    }

    /// <summary>
    /// Get Server Variables Values ...
    /// </summary>
    /// <param name="sv"></param>
    /// <returns></returns>
    public static string WWW_GetServerVariable(ServerVars sv)
    {
        return ALCS_DataShift.WhenNull(HttpContext.Current.Request.ServerVariables[WWW_GetServerVariableKey(sv)], "");
    }

    /// <summary>
    /// Get Server Variables Values ...
    /// </summary>
    /// <param name="svStr"></param>
    /// <returns></returns>
    public static string WWW_GetServerVariable(string svStr)
    {
        return ALCS_DataShift.WhenNull(HttpContext.Current.Request.ServerVariables[svStr], "");
    }


    #endregion 

    }
    #endregion 
}
