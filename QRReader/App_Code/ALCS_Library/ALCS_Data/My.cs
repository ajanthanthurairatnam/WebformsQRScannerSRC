using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_Data.ALCS_SQLWork;
using ALCS_Library.ALCS_WWW;
using ALCS_Library.ALCS_WWW.ALCS_WWWControls;
using ALCS_Library.ALCS_JavaScript;

namespace ALCS_Library.ALCS_Basics
{

    /// <summary>
    /// Summary description for My
    /// </summary>
    public static class My
    {

        #region "My Values ..."

        /// <summary>
        /// Clean the string and assume "" as empty.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyString(string inValue)
        {
            return ALCS_DataShift.WhenNull(inValue, "");
        }

        /// <summary>
        /// Clean the string convert to int and assume 0 as empty.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int MyInt(string inValue)
        {
            return ALCS_DataShift.WhenNull(inValue, 0);
        }

        /// <summary>
        /// Transform bool as ZERO or ONE.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int MyBool_As01(bool inValue)
        {
            return (inValue ? 1 : 0);
        }


        /// <summary>
        /// My Void  - 0 or empty ...
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool MyVoidString(string inValue)
        {
            return (MyString(inValue) == "") ;
        }

        /// <summary>
        /// My Void  - 0 or empty ...
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static bool MyVoidInt(string inValue)
        {
            return (MyInt(inValue) == 0);
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static int MyEnumInt(Enum inValue)
        {
            return ALCS_DataShift.EnumAsInt(inValue);
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumStr(Enum inValue)
        {
            return ALCS_DataShift.EnumAsStr(inValue);
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumText(Enum inValue)
        {
            return ALCS_DataShift.EnumAsText(inValue);
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumLowLip(Enum inValue)
        {
            return ALCS_DataShift.EnumAsText(inValue).Substring(0,1).ToLower();
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumUpLip(Enum inValue)
        {
            return ALCS_DataShift.EnumAsText(inValue).Substring(0, 1).ToUpper();
        }


        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumWord(Enum inValue)
        {
            return ALCS_DataShift.EnumAsText(inValue).Replace("_", " ");
        }

        /// <summary>
        /// Enum Manipulation 
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyEnumQS(Enum inValue)
        {
            return ALCS_DataShift.EnumAsText(inValue).ToLower();
        }

        /// <summary>
        /// Format the date ...
        /// </summary>
        /// <param name="theDate"></param>
        /// <param name="dateForm"></param>
        /// <param name="defDate"></param>
        /// <returns></returns>
        public static string MyAusDateAsString(DateTime theDate, string dateForm, DateTime defDate )
        {
            return ((theDate == defDate)? "" : (theDate.ToString(dateForm)));
        }

        /// <summary>
        /// Format the Date.
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public static string MyAusDateAsString(DateTime theDate)
        {
            return MyAusDateAsString(theDate, "dd/MM/yyyy", DateTime.MaxValue);
        }

        /// <summary>
        /// More Fomatting.
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public static string MyDBDateAsString(DateTime theDate)
        {
            return MyAusDateAsString(theDate, "dd MMM yyyy", DateTime.MaxValue);
        }


        /// <summary>
        /// when null return the substitute.
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="altText"></param>
        /// <returns></returns>
        public static string MySubstitute(string inValue, string altText)
        {
            return ALCS_DataShift.WhenNull(inValue, altText);
        }


        #endregion

        #region "My Utiilities ..."

        /// <summary>
        /// return the root URL.
        /// </summary>
        /// <returns></returns>
        public static string MyRootURL(bool stripLastSlah)
        {
            string rootURL = "";

            if (stripLastSlah)
            {
                rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/");
            }
            else
            {
                rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~");
            }

            // return the key.
            return rootURL;
        }

        /// <summary>
        /// Another oveload.
        /// </summary>
        /// <returns></returns>
        public static string MyRootURL()
        {
            return MyRootURL(false);
        }

        /// <summary>
        /// Get the Physical Path.
        /// </summary>
        /// <returns></returns>
        public static string MyRootPath()
        {

            return HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        }

        /// <summary>
        /// make sure that the file name is acceptable by the OS.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string MyCleanFileName(string inValue)
        {
            string outValue = inValue;

            // Clean 
            outValue = outValue.Replace(":", "");

            // return 
            return outValue;
        }


        #endregion 

        #region "My SQL"

        #region "Parameter Builder ...."

        /// <summary>
        /// Construct parameter name.
        /// </summary>
        /// <param name="afi"></param>
        /// <returns></returns>
        public static string APN(string afi)
        {
            if (afi.StartsWith("@"))
            {
                return ALCS_DataShift.WhenNull(afi, "");
            }
            else
            {
                return "@" + ALCS_DataShift.WhenNull(afi, "");
            }
        }

        /// <summary>
        /// Construct aparameter from primitive values.
        /// </summary>
        /// <param name="parName"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Varchar(string afi, int parSize, string parVal, string parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.VarChar, parSize);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Date Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Varchar(string afi, int parSize, DateTime parVal, DateTime parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.VarChar, parSize);
            par.Direction = ParameterDirection.Input;
            par.Value = ((parVal == parNull) ? ALCS_DataShift.DBNullIf(0, 0) : parVal.ToString("dd MMM yyyy"));
            return par;
        }

        /// <summary>
        /// Assumed NUll Value.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Varchar(string afi, int parSize, DateTime parVal)
        {
            return BIP_Varchar(afi, parSize, parVal, DateTime.MaxValue);
        }

        /// <summary>
        /// Another version
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Varchar(string afi, int parSize, string parVal)
        {
            return BIP_Varchar(afi, parSize, parVal, "");
        }


        /// <summary>
        /// Maxvarchar.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_MaxVarchar(string afi, string parVal, string parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.VarChar, -1);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Maxvarchar.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_MaxVarchar(string afi, string parVal)
        {
            return BIP_MaxVarchar(afi, parVal, "");
        }

        /// <summary>
        /// Construct a char type parameter.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Char(string afi, int parSize, string parVal, string parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Char, parSize);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }
        

        /// <summary>
        /// Another Vesion
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parSize"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Char(string afi, int parSize, string parVal)
        {
            return BIP_Char(afi, parSize, parVal, "");
        }

        /// <summary>
        /// Build an integer parameter.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Int(string afi, int parVal, int parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Int);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Int(string afi, int parVal)
        {
            return BIP_Int(afi, parVal, 0);
        }

        /// <summary>
        /// Small Int.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_SmallInt(string afi, int parVal, int parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.SmallInt);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Small Int - Another Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_SmallInt(string afi, int parVal)
        {
            return BIP_SmallInt(afi, parVal, 0);
        }

        public static SqlParameter BIP_TinyInt(string afi, int parVal, int parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.TinyInt);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Small Int - Another Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_TinyInt(string afi, int parVal)
        {
            return BIP_TinyInt(afi, parVal, 0);
        }

        /// <summary>
        /// Build a bit version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Bit(string afi, bool parVal)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Bit);
            par.Direction = ParameterDirection.Input;
            par.Value = (parVal ? 1 : 0);
            return par;
        }

        /// <summary>
        /// Build Another Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Bit(string afi, int parVal)
        {
            return BIP_Bit(afi, (parVal == 1));
        }

        /// <summary>
        /// Date Time Parameter ...
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_DateTime(string afi, DateTime parVal, DateTime parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.DateTime);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }
        

        /// <summary>
        /// Another Data Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_DateTime(string afi, DateTime parVal)
        {
            return BIP_DateTime(afi, parVal, DateTime.MaxValue);
        }

        /// <summary>
        /// Output parameters.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BOP_Int(string afi, int parVal)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Int);
            par.Direction = ParameterDirection.Output;
            par.Value = parVal;
            return par;
        }

        /// <summary>
        /// Output parameters.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BOP_Bit(string afi, bool parValue)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Bit);
            par.Direction = ParameterDirection.Output;
            par.Value = parValue;
            return par;
        }
        

        /// <summary>
        /// Build an integer parameter.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Money(string afi, decimal parVal, decimal parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Money);
            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }
        

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Money(string afi, decimal parVal)
        {
            return BIP_Money(afi, parVal, 0.0m);
        }

        /// <summary>
        /// Decimal Value.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <param name="parPrecision"></param>
        /// <param name="parScale"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Decimal(string afi, decimal parVal, byte parPrecision, byte parScale, decimal parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Decimal);

            par.Precision = parPrecision;
            par.Scale = parScale;

            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }
        
        /// <summary>
        /// Float Handling ....
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Double(string afi, double parVal, double parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Float);

            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Return ...
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Double(string afi, double parVal)
        {
            return BIP_Double(afi, parVal, 0.0d);
        }

        /// <summary>
        /// Float ...
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parNull"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Float(string afi, double parVal, float parNull)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Float);

            par.Direction = ParameterDirection.Input;
            par.Value = ALCS_DataShift.DBNullIf(parVal, parNull);
            return par;
        }

        /// <summary>
        /// Return ...
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BIP_Float(string afi, float parVal)
        {
            return BIP_Float(afi, parVal, 0.0f);
        }


        /// <summary>
        /// Output parameters.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BOP_Varchar(string afi, string parVal, int parLength)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.VarChar, parLength);
            par.Direction = ParameterDirection.Output;
            par.Value = parVal;
            return par;
        }

        /// <summary>
        /// Money Output Parameter.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <returns></returns>
        public static SqlParameter BOP_Money(string afi, decimal parVal)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Money);
            par.Direction = ParameterDirection.Output;
            par.Value = parVal;
            return par;
        }

        /// <summary>
        /// Output Decimal Parameter.
        /// </summary>
        /// <param name="afi"></param>
        /// <param name="parVal"></param>
        /// <param name="parPrecision"></param>
        /// <param name="parScale"></param>
        /// <returns></returns>
        public static SqlParameter BOP_Decimal(string afi, decimal parVal, byte parPrecision, byte parScale)
        {
            SqlParameter par = new SqlParameter(APN(afi), SqlDbType.Decimal);

            par.Precision = parPrecision;
            par.Scale = parScale;

            par.Direction = ParameterDirection.Output;
            par.Value = parVal;
            return par;
        }

        #endregion

        #endregion 
    }

}