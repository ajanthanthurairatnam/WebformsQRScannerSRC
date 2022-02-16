using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Data.ALCS_SQLWork;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_JavaScript;
using ALCS_Library.ALCS_WWW;
using ALCS_Library.ALCS_WWW.ALCS_WWWControls;
using ALCS_Library.ALCS_Basics;

namespace ALCS_Library.ALCS_Format
{

    /// <summary>
    /// Summary description for ArraysAndLists
    /// </summary>
    public static  class ALCS_ArraysAndLists
    {

        #region "Convert Genralic Lists to a delimited string ..."

        /// <summary>
        /// Combine the element in one delimited string
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string ListToString(List<string> inList, string delim)
        {
            string delStr = "";

            foreach(string theToken in inList)
            {
                delStr += (delStr == "") ? theToken : delim + theToken;
            }

            // return 
            return delStr;
        }

        /// <summary>
        /// Another version where the delimiter is assumed as ",".
        /// </summary>
        /// <param name="inList"></param>
        /// <returns></returns>
        public static string ListToString(List<string> inList)
        {
            return ListToString(inList, ",");
        }

        /// <summary>
        /// Combine the list as a fixed length tokens.
        /// We will left pad the tokens with the passed 
        /// pad char up to the tokenLength string.
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="tokenLength"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string ListToString(List<string> inList, int tokenLength, char padChar)
        {
            string delStr = "";

            foreach (string theToken in inList)
            {
                delStr += theToken.PadLeft(tokenLength, padChar);
            }

            // return 
            return delStr;
        }

        /// <summary>
        /// Another version of the above where the padding character is assumed as '0'.
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="tokenLength"></param>
        /// <returns></returns>
        public static string ListToString(List<string> inList, int tokenLength)
        {
            return ListToString(inList, tokenLength, '0');
        }

        /// <summary>
        /// Combine the element in one delimited string
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string ListToString(List<int> inList, string delim)
        {
            string delStr = "";

            foreach (int theToken in inList)
            {
                delStr += (delStr == "") ? theToken.ToString() : delim + theToken.ToString();
            }

            // return 
            return delStr;
        }

        /// <summary>
        /// Another version where the delimiter is assumed as ",".
        /// </summary>
        /// <param name="inList"></param>
        /// <returns></returns>
        public static string ListToString(List<int> inList)
        {
            return ListToString(inList, ",");
        }


        /// <summary>
        /// Combine the list as a fixed length tokens.
        /// We will left pad the tokens with the passed 
        /// pad char up to the tokenLength string.
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="tokenLength"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string ListToString(List<int> inList, int tokenLength, char padChar)
        {
            string delStr = "";

            foreach (int theToken in inList)
            {
                delStr += theToken.ToString().PadLeft(tokenLength, padChar);
            }

            // return 
            return delStr;
        }

        /// <summary>
        /// Another version of the above where the padding character is assumed as '0'.
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="tokenLength"></param>
        /// <returns></returns>
        public static string ListToString(List<int> inList, int tokenLength)
        {
            return ListToString(inList, tokenLength, '0');
        }


        #endregion 


        #region "Strings To List ...."

        /// <summary>
        /// Turn a delimited string into a generic list.
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="theDelim"></param>
        /// <returns></returns>
        public static List<String> StringToList(string theString, string theDelim)
        {
            string[] tokens = theString.Split(theDelim.ToCharArray());
            List<string> theList = new List<string>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (!theList.Contains(tokens[i].Trim()))
                {
                    theList.Add(tokens[i].Trim());
                }
            }

            // Return the list.
            return theList;
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static List<String> StringToList(string theString)
        {
            return StringToList(theString, ",");
        }


        /// <summary>
        /// Split the list into tokens of int.
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="theDelim"></param>
        /// <returns></returns>
        public static List<int> StringToListOfInt(string theString, string theDelim)
        {
            string[] tokens = theString.Split(theDelim.ToCharArray());
            List<int> theList = new List<int>();

            for (int i = 0; i < tokens.Length; i++)
            {
                string theToken = tokens[i].Trim();

                if (ALCS_Is.isInteger(theToken, WorkMode.Strict))
                {
                    int theInt = ALCS_DataShift.WhenNull(theToken, 0);
                    if (!theList.Contains(theInt))
                    {
                        theList.Add(theInt);
                    }
                }
            }

            // Return the list.
            return theList;
        }

        /// <summary>
        /// Another version
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static List<int> StringToListOfInt(string theString)
        {
            return StringToListOfInt(theString, ",");
        }

        #endregion 


        #region "String to dictionaries ..."
        /// <summary>
        /// Build the List ...
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="Area"></param>
        /// <param name="Street"></param>
        /// <param name="House"></param>
        /// <returns></returns>
        public static Dictionary<int, List<int>> StringToOneIntManyInt(string theString, string Area, string Street, string House  )
        {
            Dictionary<int, List<int>> ci = new Dictionary<int,List<int>>();

            // The streets.
            string[] streets = theString.Split(Area.ToCharArray());

            for (int i = 0; i < streets.Length; i++)
            {
                string[] houses = streets[i].Split(Street.ToCharArray());

                if ((houses.Length > 1) && (ALCS_Is.isInteger(houses[0], WorkMode.Strict)))
                {
                    ci.Add(ALCS_DataShift.WhenNull(houses[0], 0), ALCS_ArraysAndLists.StringToListOfInt(houses[1]));
                }
            }

            return ci;
        }


        #endregion 
    }
}