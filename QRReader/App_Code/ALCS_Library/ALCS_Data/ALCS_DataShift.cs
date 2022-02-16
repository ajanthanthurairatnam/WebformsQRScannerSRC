using System;
using System.Configuration;

using ALCS_Library.ALCS_Format;  

///////////////////////////////////////////////////////////////////
/// Handle all Data Preparation and Data transformstion
/// ///////////////////////////////////////////////////////////////

namespace ALCS_Library.ALCS_Data
{
    #region "Translate data between SQL and ASP"

    /// <summary>
    /// The Data Shift class mainly shift the Data form from a SQL aware
    /// form to ASP aware form. 
    /// </summary>
    public static class ALCS_DataShift
    {

        #region "Mask a value by another from another type ..."

        /// <summary>
        /// replace a boolen value by and string ....
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static string MaskValue(bool inValue, string sPlus, string sMinus)
        {
            if (inValue)
            {
                return sPlus;
            }
            else
            {
                return sMinus;
            }
        }

        /// <summary>
        /// replace a boolen value by and integer ....
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static int MaskValue(bool inValue, int sPlus, int sMinus)
        {
            if (inValue)
            {
                return sPlus;
            }
            else
            {
                return sMinus;
            }
        }

        /// <summary>
        /// replace a object by alternative string values.
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static string MaskValue(object inValue, string sPlus, string sMinus)
        {
            if (IsVoid(inValue))
            {
                return sMinus;
            }
            else
            {
                return sPlus;
            }
        }

        /// <summary>
        /// replace a object by alternative int values (1/0 flag).
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static int MaskValue(object inValue, int sPlus, int sMinus)
        {
            if (IsVoid(inValue))
            {
                return sMinus;
            }
            else
            {
                return sPlus;
            }
        }

        /// <summary>
        /// Simple Compare .... 0=false Otherwise True
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool MaskValue(int inValue)
        {
            if (inValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Simple Compare .... ""=false Otherwise True
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool MaskValue(string inValue)
        {
            if (inValue == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
#endregion

        #region "Defining Void ..."

        /// <summary>
        /// Check if the object has any data in it.
        /// </summary>
        /// <param name="inField"></param>
        /// <returns></returns>
        public static bool IsVoid(object inField)
        {
            if ((Convert.IsDBNull(inField)) || (inField == null) || (inField.ToString() == string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 

        #region "The When Null Series ..."

        //**************************************************************** 
        // NullSafe integer  
        //**************************************************************** 
        public static int WhenNull(object arg, int returnIfEmpty)
        {
            int returnValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToInt32(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        //**************************************************************** 
        // NullSafe decimal   
        //**************************************************************** 
        public static decimal WhenNull(object arg, decimal returnIfEmpty)
        {
            decimal returnValue;
            string  stringValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                stringValue = arg.ToString();
                stringValue = ALCS_StringRewrite.Crystal(stringValue);
                try
                {
                    returnValue = Convert.ToDecimal(stringValue);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }


        //**************************************************************** 
        // NullSafe double   
        //**************************************************************** 
        public static double WhenNull(object arg, double returnIfEmpty)
        {
            double returnValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToDouble(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        //**************************************************************** 
        // NullSafe boolean 
        //**************************************************************** 
        public static bool WhenNull(object arg, bool returnIfEmpty)
        {
            bool returnValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToBoolean(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }


        //**************************************************************** 
        // NullSafe String 
        //**************************************************************** 
        public static string WhenNull(object arg, string returnIfEmpty)
        {
            string returnValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToString(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue.Trim();
        }


        //**************************************************************** 
        // NullSafe Date 
        //**************************************************************** 
        public static DateTime WhenNull(object arg, DateTime returnIfEmpty)
        {
            DateTime returnValue;

            if (IsVoid(arg))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToDateTime(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        #endregion 

        #region "The DBNULL If Series ..."

        /// <summary>
        /// Transform empty/specified string to DBNULL value.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(object arg, string compareVal)
        {
            string transValue;
            try
            {
                transValue = Convert.ToString(arg);
                if (transValue == compareVal)
                {
                    return DBNull.Value;
                }
                else
                {
                    return transValue;
                }
            }
            catch
            {
                return arg;
            }
        }

        /// <summary>
        /// Transform empty integer/specified to NULL value.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(object arg, int compareVal)
        {
            int transValue;
            try
            {
                transValue = Convert.ToInt32(arg);
                if (transValue == compareVal)
                {
                    return DBNull.Value;
                }
                else
                {
                    return transValue;
                }
            }
            catch
            {
                return arg;
            }
        }

        /// <summary>
        /// Transform empty decimal/specified to NULL value.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(object arg, decimal compareVal)
        {
            decimal transValue;
            try
            {
                transValue = Convert.ToDecimal(arg);
                if (transValue == compareVal)
                {
                    return DBNull.Value;
                }
                else
                {
                    return transValue;
                }
            }
            catch
            {
                return arg;
            }
        }

        /// <summary>
        /// Double .... 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(object arg, double compareVal)
        {
            double transValue;
            try
            {
                transValue = Convert.ToDouble(arg);
                if (transValue == compareVal)
                {
                    return DBNull.Value;
                }
                else
                {
                    return transValue;
                }
            }
            catch
            {
                return arg;
            }
        }

        /// <summary>
        /// To Single ...
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(object arg, float compareVal)
        {
            double transValue;
            try
            {
                transValue = Convert.ToSingle(arg);
                if (transValue == compareVal)
                {
                    return DBNull.Value;
                }
                else
                {
                    return transValue;
                }
            }
            catch
            {
                return arg;
            }
        }

        /// <summary>
        /// Transform empty decimal/specified to NULL value.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static object DBNullIf(DateTime arg, DateTime compareVal)
        {
            if (arg == compareVal)
            {
                return DBNull.Value;
            }
            else
            {
                return arg.ToString("dd MMM yyyy hh:mm:ss");
            }
        }

        #endregion 

        #region "Mask a value by another from another type ..."

        /// <summary>
        /// replace a boolen value by and string ....
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static string BoolTo(bool inValue, string sPlus, string sMinus)
        {
            if (inValue)
            {
                return sPlus;
            }
            else
            {
                return sMinus;
            }
        }

        /// <summary>
        /// replace a boolen value by and integer ....
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static int BoolTo(bool inValue, int sPlus, int sMinus)
        {
            if (inValue)
            {
                return sPlus;
            }
            else
            {
                return sMinus;
            }
        }

        /// <summary>
        /// replace a object by alternative string values.
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static string ObjectTo(object inValue, string sPlus, string sMinus)
        {
            if (IsVoid(inValue))
            {
                return sMinus;
            }
            else
            {
                return sPlus;
            }
        }

        /// <summary>
        /// replace a object by alternative int values (1/0 flag).
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="sPLus"></param>
        /// <param name="sMinus"></param>
        /// <returns></returns>
        public static int ObjectTo(object inValue, int sPlus, int sMinus)
        {
            if (IsVoid(inValue))
            {
                return sMinus;
            }
            else
            {
                return sPlus;
            }
        }

        /// <summary>
        /// Simple Compare .... 0=false Otherwise True
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool AsBool(int inValue)
        {
            if (inValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Simple Compare .... ""=false Otherwise True
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool AsBool(string inValue)
        {
            if (inValue == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Simple Compare .... 0=false Otherwise 1
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int AsBit(int inValue)
        {
            if (inValue == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Simple Compare .... ""=0 Otherwise 1
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int AsBit(string inValue)
        {
            if (inValue == "")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Simple Compare .... false=0 Otherwise 1
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int AsBit(bool inValue)
        {
            if (inValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion 

        #region "Shifting Enum Types ..."

        public static int EnumAsInt(Enum  arg)
        {
            int enumAsInt = Convert.ToInt32(arg);

            // return enumAsInt
            return enumAsInt;
        }

        public static string EnumAsStr(Enum arg)
        {
            string enumAsStr = Convert.ToString(Convert.ToInt32(arg));

            // return enumAsStr
            return enumAsStr;
        }

        public static string EnumAsText(Enum arg)
        {
            string enumAsStr = arg.ToString();

            // return enumAsStr
            return enumAsStr;
        }

        #endregion 
    }

#endregion

}	



