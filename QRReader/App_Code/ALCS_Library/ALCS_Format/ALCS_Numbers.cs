using System;

using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Format;

namespace ALCS_Library.ALCS_Numerics
{
	/// <summary>
	/// Summary description for ALCS_Numbers.
	/// </summary>
	public class ALCS_Numbers
	{

		/// <summary>
		/// Get the whole number resulting from dividing two integers 
		/// rounded UP to the nearest integer 
		/// </summary>
		/// <param name="num1"></param>
		/// <param name="num2"></param>
		/// <returns></returns>
		public static int DivideInt_Ceiling(int num1, int num2)
		{
			int theDiv = (int) Math.Ceiling((num1 + 0.0) / (num2 + 0.0));

			// return.
			return theDiv;
		}

		/// <summary>
		/// Get the whole number resulting from dividing two integers
		/// rounded DOWN to the nearest integer 
		/// </summary>
		/// <param name="num1"></param>
		/// <param name="num2"></param>
		/// <returns></returns>
		public static int DivideInt_Floor(int num1, int num2)
		{
			int theDiv = (int) Math.Floor((num1 + 0.0) / (num2 + 0.0));

			// return.
			return theDiv;
		}

		/// <summary>
		/// Place the number in the proper cell inside the table.
		/// 1 is the strating index.
		/// Returns [row, col]
		/// </summary>
		/// <param name="tblWidth"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int[] TablePlacement(int tblWidth, int num)
		{
			int row = DivideInt_Ceiling(num, tblWidth);
			//int remainder = num % tblWidth;
			//int col = ((remainder == 0)? tblWidth:remainder);
			int col = ((num-1) % tblWidth) + 1;
			
			return new int[] {row, col};
		}


        /// <summary>
        /// a simple map between a 
        /// </summary>
        /// <param name="isSet"></param>
        /// <returns></returns>
        public static int NullBoolToInt(bool? isSet)
        {
            if ((isSet == null) || (!isSet.HasValue))
            {
                return -1;
            }
            else if (isSet.Value)
            {
                return 1;
            }
            else if (!isSet.Value)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }



        /// <summary>
        /// ///////Only displays decimal point if required
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        public static string FormatDecimals(decimal inVal)
        {
            return FormatDecimals(inVal, false);
        }

        /// <summary>
        /// ///////Only displays decimal point if required
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        public static string FormatDecimals(decimal inVal, bool delimit)
        {
            string strVal = "";
            if ((inVal % 1) == 0)
            {
                strVal = delimit ? inVal.ToString("#,0") : inVal.ToString("0");
            }
            else
            {
                strVal = delimit ? inVal.ToString("#,0.00") : inVal.ToString("0.00");
            }

            return strVal;
        }


        /// <summary>
        /// ///////Only displays decimal point if required
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        public static string FormatDecimals_TwotoFour(decimal inVal, bool delimit)
        {
            string strVal = "";

            decimal theRound = (100.00m * inVal) % 1;


            if (theRound == 0.0m)
            {
                strVal = delimit ? inVal.ToString("#,0.00##") : inVal.ToString("0.00##");
            }
            else
            {
                strVal = delimit ? inVal.ToString("#,0.0000") : inVal.ToString("0.0000");
            }

            return strVal;
        }

        /// <summary>
        /// Format only if not an extereme number.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="delimit"></param>
        /// <returns></returns>
        public static string WriteDecimals(decimal inVal, int decPlaces, bool delimit)
        {
            // return value.
            string strVal = "";

            // Format Str 
            string formatStr = "";

            // Decimal Places ...
            if (delimit)
            {
                formatStr = "#,0";
            }
            else
            {
                formatStr = "0";
            }

            // Decimal Places 
            if (decPlaces <= 0)
            {
                if ((inVal % 1) != 0)
                {
                    formatStr += "." + new String('0', Math.Abs(decPlaces));
                }
            }
            else
            {
                formatStr += "." + new String('0', decPlaces);
            }

            // Anything ...
            if ((inVal == Decimal.MinValue) || (inVal == Decimal.MaxValue))
            {
                strVal = "";
            }
            else
            {
                strVal = inVal.ToString(formatStr);
            }

            return strVal;
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="decPlaces"></param>
        /// <returns></returns>
        public static string WriteDecimals(decimal inVal, int decPlaces)
        {
            return WriteDecimals(inVal, decPlaces, true);
        }

        /// <summary>
        /// another version.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="delimit"></param>
        /// <returns></returns>
        public static string WriteDecimals(decimal inVal)
        {
            return WriteDecimals(inVal, -2, true);
        }

        /// <summary>
        /// Format to a $ figure.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="decPlaces"></param>
        /// <param name="delimit"></param>
        /// <returns></returns>
        public static string WriteMoney(decimal inVal, int decPlaces, bool delimit)
        {
            string strVal = WriteDecimals(inVal, decPlaces, delimit);

            return ((strVal == "") ? ("") : ("$" + strVal));
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="decPlaces"></param>
        /// <returns></returns>
        public static string WriteMoney(decimal inVal, int decPlaces)
        {
            return WriteMoney(inVal, decPlaces, true);
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        public static string WriteMoney(decimal inVal)
        {
            return WriteMoney(inVal, 2, true);
        }


        #region "Tinting the Values ..."

        /// <summary>
        /// Format the number.
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static string STYLE_AddParentheses(decimal inValue)
        {
            return STYLE_AddParentheses(inValue, "C");
        }

        /// <summary>
        /// Format the decimal.
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="inFormatString"></param>
        /// <returns></returns>
        public static string STYLE_AddParentheses(decimal inValue, string inFormatString)
        {
            string outValue = inValue.ToString(inFormatString);

            if (inValue < 0.0m)
            {
                outValue = "<font style=\"color:#FF0000\">(" + outValue + ")</font>";
            }

            return outValue;
        }

        #endregion 


        #region "Hex conversion ..."

        /// <summary>
        /// Find the hex representation of a number between 0 and 255.
        /// </summary>
        /// <returns></returns>
        public static bool IntToHex_Single(int num, out string hexStr)
        {
            bool hexOK = true;
            hexStr = "";

            // the number.
            if ((num < 0) || (num > 255))
            {
                return false;
            }
            else
            {
                try
                {
                    hexStr = num.ToString("X");
                }
                catch
                {
                    hexOK = false;
                }

                // return.
                if (hexOK)
                {
                    hexStr = hexStr.PadLeft(2, '0');
                }

                // return.
                return hexOK;
            }
        }

        /// <summary>
        /// Find the integer representation of a hex number.
        /// </summary>
        /// <returns></returns>
        public static bool HexToInt_Single(string hexStr, out int num)
        {
            num = 0;
            bool hexOK = true;

            // The hex string.
            if (hexStr == "")
            {
                return false;
            }
            else if (hexStr.Length != 2)
            {
                return false;
            }
            else if (!ALCS_Is.isHex(hexStr))
            {
                return false;
            }
            else
            {
                try
                {
                    num = Convert.ToInt32(hexStr, 16);
                }
                catch
                {
                    hexOK = false;
                }

                // return 
                return hexOK;
            }
        }


        #endregion 

	}
}
