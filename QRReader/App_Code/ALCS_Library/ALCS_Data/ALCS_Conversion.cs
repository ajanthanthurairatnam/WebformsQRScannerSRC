using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ALCS_Library.ALCS_Data;

namespace ALCS_Library.ALCS_Format
{

    #region "Conversion ..."

    public static class ALCS_Conversion
    {

        /// <summary>
        /// Specify specific precision for the hex converter.
        /// </summary>
        public enum HexForm
        {
            x2 = 2,
            X2 = 2,
            x4 = 4,
            X4 = 4
        }

        /// <summary>
        /// The Form.
        /// </summary>
        /// <param name="xf"></param>
        /// <returns></returns>
        public static string HexFormStr(HexForm xf)
        {
            if (xf == HexForm.x2)
            {
                return "x2";
            }
            else if (xf == HexForm.X2)
            {
                return "X2";
            }
            else if (xf == HexForm.x4)
            {
                return "x4";
            }
            else if (xf == HexForm.X4)
            {
                return "X4";
            }
            else
            {
                return "x2";
            }
        }

        /// <summary>
        /// return the string in a Hex form ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool StringToHex(string inStr, HexForm xf, out string hexStr)
        {
            if (inStr == "")
            {
                hexStr = "";
                return true;
            }
            else
            {
                hexStr = "";
                int chunk = ALCS_DataShift.EnumAsInt(xf);
                string hform = HexFormStr(xf);
                char[] chars = inStr.ToCharArray();

                for (int idx = 0; idx < chars.Length; idx++)
                {
                    int thisChar = (int)chars[idx];

                    // We want to limit that to 8 bytes (0-255) for X2 and x2 forms.;
                    if ((chunk == 2) && (thisChar > 255))
                    {
                        hexStr = "";
                        return false;
                    }

                    // Get the hex representation.
                    hexStr += thisChar.ToString(hform);
                }
            }

            // if we make here all is good ...
            return true;
        }

        /// <summary>
        /// like the above but assume x2 for hes form.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static bool StringToHex(string inStr, out string hexStr)
        {
            return StringToHex(inStr, HexForm.x2, out hexStr);
        }

        /// <summary>
        /// Check if the string is a hex form and return its root in unhexStr.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="xf"></param>
        /// <param name="unhexStr"></param>
        /// <returns></returns>
        public static bool HexToString(string inStr, HexForm xf, out string unhexStr)
        {
            int chunk = ALCS_DataShift.EnumAsInt(xf);

            if (inStr == "")
            {
                unhexStr = "";
                return true;
            }
            else if ((inStr.Length % chunk) != 0)
            {
                unhexStr = "";
                return false;
            }
            else if (!ALCS_Is.isHex(inStr))
            {
                unhexStr = "";
                return false;
            }
            else
            {
                unhexStr = "";
                char[] chars = inStr.ToCharArray();

                for (int idx = 0; idx < chars.Length; idx = idx + chunk)
                {
                    string hexStr = inStr.Substring(idx, chunk);
                    int thisInt = Convert.ToInt32(hexStr, 16);
                    char thisChar = Convert.ToChar(thisInt);
                    unhexStr += thisChar.ToString();
                }
            }

            // if you make here All is Good
            return true;
        }

        /// <summary>
        /// Like the above but x2 is assumed ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="unhexStr"></param>
        /// <returns></returns>
        public static bool HexToString(string inStr, out string unhexStr)
        {
            return HexToString(inStr, HexForm.x2, out unhexStr);
        }
    }

    #endregion
}
