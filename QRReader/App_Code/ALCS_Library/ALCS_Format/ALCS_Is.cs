using System;
using System.Net;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Basics;


namespace ALCS_Library.ALCS_Format
{
	/// <summary>
	/// Summary description for ALCS_Is.
	/// </summary>
	public class ALCS_Is
	{
        static IFormatProvider isCulture = CultureInfo.CurrentCulture;

        #region "Set Your Culture ..."

        public static void SetCheckCulture(IFormatProvider curCulture)
        {
            isCulture = curCulture;
        }

        #endregion 

        #region "Checking digits and numbers ...."

        /// <summary>
		/// Check if the passed string is group of digits.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isDigits(string inStr, WorkMode wm)
		{
            inStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr);

			Regex re = new Regex(@"\D");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((!re.IsMatch(inStr)) && (inStr != "")); 
			}
			else
			{
				return ((!re.IsMatch(inStr)) || (inStr == ""));
			}
		}

        /// <summary>
        /// Another Form ....
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isDigits(string inStr)
        {
            return isDigits(inStr, WorkMode.Strict);    
        }

		/// <summary>
		/// Check if the passed string is group of digits , and ..
		/// Charcter allowed are [0-9,.]
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isDigitsAndSigns(string inStr, WorkMode wm)
		{
			Regex re = new Regex(@"[^\d\.,]");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((!re.IsMatch(inStr)) && (inStr != "")); 
			}
			else
			{
				return ((!re.IsMatch(inStr)) || (inStr == ""));
			}
		}

        /// <summary>
        /// Another Form ....
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isDigitsAndSigns(string inStr)
        {
            return isDigitsAndSigns(inStr, WorkMode.Strict); 
        }

		/// <summary>
		/// Is the string a positive or negative integer?
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static bool isInteger(string inStr, WorkMode wm)
		{
			Regex re = new Regex(@"^[+-]{0,1}\d+$");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((re.IsMatch(inStr)) && (inStr != "")); 
			}
			else
			{
				return ((re.IsMatch(inStr)) || (inStr == ""));
			}
		}

        /// <summary>
        /// Another form 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isInteger(string inStr)
        {
            return isInteger(inStr, WorkMode.Strict); 
        }

        /// <summary>
        /// Is a bit Field.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isBit(string inStr, WorkMode wm)
        {
            // Settle it first ....
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (wm == WorkMode.Strict)
            {
                return ((inStr == "0") || (inStr == "1"));
            }
            else
            {
                return ((inStr == "0") || (inStr == "1") || (inStr == ""));;
            }
        }

        /// <summary>
        /// is that a it field.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isBit(string inStr)
        {
            return isBit(inStr, WorkMode.Strict);
        }


        /// <summary>
        /// Is the string a valid ID that is a sequence of digits
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isID(string inStr, WorkMode wm)
        {
            Regex re = new Regex(@"^[1-9][0-9]*$");

            // Settle it first ....
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (wm == WorkMode.Strict)
            {
                return ((re.IsMatch(inStr)) && (inStr != ""));
            }
            else
            {
                return ((re.IsMatch(inStr)) || (inStr == ""));
            }
        }

        /// <summary>
        /// Another form 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isID(string inStr)
        {
            return isID(inStr, WorkMode.Strict);
        }

        /// <summary>
        /// Is the string a valid ID that is a sequence of digits?
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isPaddedID(string inStr, WorkMode wm)
        {
            Regex re = new Regex(@"^0*[1-9][0-9]*$");

            // Settle it first ....
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (wm == WorkMode.Strict)
            {
                return ((re.IsMatch(inStr)) && (inStr != ""));
            }
            else
            {
                return ((re.IsMatch(inStr)) || (inStr == ""));
            }
        }

        /// <summary>
        /// Another form 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isPaddedID(string inStr)
        {
            return isPaddedID(inStr, WorkMode.Strict);
        }

		/// <summary>
		/// is the string a positive or negative integer?
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static bool isDecimal(string inStr, WorkMode wm)
		{
			Regex re = new Regex(@"^[+-]{0,1}\d+\.{0,1}\d+$");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

            // Every integer is a decimal.
            if(isInteger(inStr, wm))
            {
                return true;
            }

			if(wm == WorkMode.Strict)
			{
				return ((re.IsMatch(inStr)) && (inStr != ""));  
			}
			else
			{
				return ((re.IsMatch(inStr)) || (inStr == ""));
			}
		}

        /// <summary>
        /// Another mode ....
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isDecimal(string inStr)
        {
            return isDecimal(inStr, WorkMode.Strict);
        }


        /// <summary>
        /// Proper Decimal.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isProperDecimal(string inStr, WorkMode wm)
        {
            // Is Decimal.
            bool isDec = false;

            // Settle It First?
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            // Relaxed?
            if ((inStr == "") && (wm == WorkMode.Relaxed))
            {
                return true;
            }

            // Attempt?
            try
            {
                Decimal d = Decimal.MaxValue;
                isDec = Decimal.TryParse(inStr, out d);
            }
            catch
            {
                isDec = false;
            }

            // return 
            return isDec;
        }

        /// <summary>
        /// Proper Decimal.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isProperDecimal(string inStr)
        {
            return isProperDecimal(inStr, WorkMode.Strict);
        }


        /// <summary>
        /// Validate Is Positive Number
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns>true/false</returns>
        public static bool isPositive(Decimal dValue)
        {
            return dValue>=0;
        }

        /// <summary>
        /// A Fixed length Digit sequence
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isFixedDigits(string inStr, int len, WorkMode wm)
        {
            Regex re = new Regex(@"^\d{" + len.ToString() + "," + len.ToString() + "}$");

            // Settle it first ....
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (wm == WorkMode.Strict)
            {
                return ((re.IsMatch(inStr)) && (inStr != ""));
            }
            else
            {
                return ((re.IsMatch(inStr)) || (inStr == ""));
            }
        }

        /// <summary>
        /// Another version 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isFixedDigits(string inStr, int len)
        {
            return isFixedDigits(inStr, len, WorkMode.Strict);
        }

        // Derived of the above ....
        
        /// <summary>
        /// Phone/Fax.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isPhoneFax(string inStr, WorkMode wm)
        {
            string alterStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr);
            return isFixedDigits(alterStr, 8, wm);
        }

        public static bool isPhoneFax(string inStr)
        {
            return isPhoneFax(inStr, WorkMode.Strict);
        }


        /// <summary>
        /// STD Phone/Fax.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isSTDPhoneFax(string inStr, WorkMode wm)
        {
            string alterStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr);
            return isFixedDigits(alterStr, 10, wm);
        }

        public static bool isSTDPhoneFax(string inStr)
        {
            return isSTDPhoneFax(inStr, WorkMode.Strict);
        }


        /// <summary>
        /// Mobile Phone
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isMobile(string inStr, WorkMode wm)
        {
            string alterStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr);
            return isFixedDigits(alterStr, 10, wm);
        }

        public static bool isMobile(string inStr)
        {
            return isMobile(inStr, WorkMode.Strict);
        }

        


        /// <summary>
        /// Poscode
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isAusPostcode(string inStr, WorkMode wm)
        {
            string alterStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr);
            return isFixedDigits(alterStr, 4, wm);
        }

        /// <summary>
        /// Another Version 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isAusPostcode(string inStr)
        {
            return isAusPostcode(inStr,WorkMode.Strict);
        }

        /// <summary>
        /// Determin if the code is that of an australian state.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isAusState(string inStr, WorkMode wm)
        {
            string alterStr = ALCS_StringRewrite.ClearWhiteSpaces(inStr).ToUpper();

            if ((inStr == "") && (wm == WorkMode.Relaxed))
            {
                return true;
            }
            else
            {
                return LoadAusStates().Contains(alterStr);
            }
        }

        /// <summary>
        /// Is Aus State.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isAusState(string inStr)
        {
            return isAusState(inStr, WorkMode.Strict);
        }

        /// <summary>
        /// Merely retuen a list of all australian states 
        /// </summary>
        /// <returns></returns>
        public static ArrayList LoadAusStates()
        {
            ArrayList AusStates = new ArrayList();

            AusStates.Add("NSW");
            AusStates.Add("VIC");
            AusStates.Add("QLD");
            AusStates.Add("SA");
            AusStates.Add("WA");
            AusStates.Add("TAS");
            AusStates.Add("NT");
            AusStates.Add("ACT");

            // Return 
            return AusStates;
        }

		#endregion

		#region "Checking alphanumeric values ...."
		
		/// <summary>
		/// Check if the passed string is group of digits , and ..
		/// Charcter allowed are [a-z,A-Z]
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isAlpha(string inStr, WorkMode wm)
		{
			Regex re = new Regex(@"[^a-zA-Z]");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((!re.IsMatch(inStr)) && (inStr != ""));  
			}
			else
			{
				return ((!re.IsMatch(inStr)) || (inStr == ""));
			}
		}

		/// <summary>
		/// Check if the passed string is group of digits , and ..
		/// Charcter allowed are [a-z,A-Z]
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isAlphaNumeric(string inStr, WorkMode wm)
		{
			Regex re = new Regex(@"[^a-zA-Z0-9]");
			
			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((!re.IsMatch(inStr)) && (inStr != "")); 
			}
			else
			{
				return ((!re.IsMatch(inStr)) || (inStr == ""));
			}
		}

		#endregion 

        #region "Hex and what else ..."
        /// <summary>
        /// Check if the passed char is a hex form i.e. 0-9, A-F.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool isHex(char c)
        {
            return ((Char.IsLetterOrDigit(c)) && (Char.ToLower(c) <= 'f'));
        }

        /// <summary>
        /// Check if the string passed is a HEX form. i.e. a serios of 0-9A-F
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isHex(string s)
        {
            if (s == "")
            {
                return false;
            }
            else
            {
                char[] chars = s.ToCharArray();

                for (int idx = 0; idx < chars.Length; idx++)
                {
                    if (!isHex(chars[idx]))
                    {
                        return false;
                    }
                }

            }

            // if we make he than all is good ...
            return true;
        }

        #endregion 

        #region "ABN ..."

        /// <summary>
        /// This Function Validate a given ABN passed to it and return true if the ABN is a valid ABN.
        /// The Steps involved in the Checking are :
        /// 1. Create the Weights Array this is given by the client and is the same for all ABN.
        /// 2. make sure that the ABN is made of 11 digits.
        /// 3. Claculate the sum of weighted digist of the ABN this is :
        ///    Sum = ( (d11-1)*(w11) + d10*w10 + d9*w9 + .... + d1*w1). Please Note that we substruct
        ///    1 from the eleventh digit of the ABN and only from this one.
        /// 4. Once Calculated The ABN is considered Valid if the remainder of the sum by 89 is 0
        /// </summary>
        /// <param name="abnNo"></param>
        /// <returns></returns>
        public static bool isABN(string abnNo, WorkMode wm)
        {
            bool abnFlag = true;
            Regex characters = new Regex(@"\D");
            int[] Weights = new int[] {10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19};
            char[] elevenChar;
            int[] elevenInt;
            int sum = 0;
            int by89 = -1;
            int idx;

            // Clean the ABN No
            abnNo = ALCS_DataShift.WhenNull(abnNo, "");
            abnNo = ALCS_StringRewrite.ClearWhiteSpaces(abnNo);

            // if string is ZERO-length return.
            if (abnNo == "") 
            {
                return (wm == WorkMode.Relaxed);
            }

            //If ABN is not made out of 11 digits than set flag to false.
            if ((characters.IsMatch(abnNo)) || (abnNo.Length != 11))
            {
                abnFlag = false;
                return abnFlag;
            }

            // Get all the pieces 
            elevenChar = abnNo.ToCharArray();
            elevenInt = Array.ConvertAll(elevenChar, new Converter<char, int>(ALCS_StringRewrite.ChartoInt));

            for (idx = 0; idx < 11; idx++)
            {
                if (idx == 0)
                {
                    sum += (elevenInt[idx] - 1) * Weights[idx];
                }
                else
                {
                    sum += (elevenInt[idx]) * Weights[idx];
                }
            }

            by89 = sum % 89;

            if (by89 != 0)
            {
                abnFlag = false;
            }


            // return the abnFlag
            return abnFlag;
        }

        /// <summary>
        /// Realxed mode.
        /// </summary>
        /// <param name="abnNo"></param>
        /// <returns></returns>
        public static bool isABN(string abnNo)
        {
            return isABN(abnNo, WorkMode.Strict);
        }

    #endregion 

        #region "Validating ACN ...."

        /// <summary>
        /// Validate the acn.
        /// 1. Apply weighting to didts 1 to 8. WEIGHTS98,7,6,5,4,3,2,1]
        /// 2. Sum the product of each digits with its corresponding weight.
        /// 3. divide by 10 and get the reamined.
        /// 4. complement the remained to 10. If complement = 10 set to ZERO.
        /// 5. the adjusted remainder has to equal digit nine fo rthe ACN to be valid.
        /// </summary>
        /// <param name="acnNo"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isACN(string acnNo, WorkMode wm)
        {
            bool acnFlag = true;
            Regex characters = new Regex(@"\D");
            int[] Weights = new int[] { 8, 7, 6, 5, 4, 3, 2, 1 };
            char[] nineChar;
            int[] nineInt;
            int sum = 0;
            int rem10 = -1;
            int comp10 = -1;
            int idx;

            // Clean the ABN No
            acnNo = ALCS_DataShift.WhenNull(acnNo, "");
            acnNo = ALCS_StringRewrite.ClearWhiteSpaces(acnNo);

            // if string is ZERO-length return.
            if (acnNo == "")
            {
                return (wm == WorkMode.Relaxed);
            }

            //If ACN is not made out of 11 digits than set flag to false.
            if ((characters.IsMatch(acnNo)) || (acnNo.Length != 9))
            {
                acnFlag = false;
                return acnFlag;
            }

            //// Get all the pieces. 
            //nineChar = acnNo.ToCharArray();
            //nineInt = Array.ConvertAll(nineChar, new Converter<char, int>(ALCS_StringRewrite.ChartoInt));

            //for (idx = 0; idx < 8; idx++)
            //{
            //    sum += (nineInt[idx]) * Weights[idx];
            //}

            //// Check remained.
            //rem10 = sum % 10;
            //comp10 = 10 - rem10;

            //if(comp10 == 10)
            //{
            //    comp10 = 0;
            //}

            //acnFlag = (comp10 == nineInt[8]);

            // return the abnFlag
            return acnFlag;
        }

        /// <summary>
        /// Realxed mode.
        /// </summary>
        /// <param name="acnNo"></param>
        /// <returns></returns>
        public static bool isACN(string acnNo)
        {
            return isACN(acnNo, WorkMode.Strict);
        }

    #endregion 

        #region "is Email and Other ....."

        /// <summary>
		/// Check if the Email has a valid format ...
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isEmail(string inStr, WorkMode wm)
		{
			//Regex re = new Regex(@"\S+@\S+\.\S{2,4}");
            Regex re_old = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");

            Regex re = new Regex(@"(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})");


            // - vvdolar - start of new code -- 7/Apr/2015
            Regex reRelaxed = new Regex(@"\S+@\S+\.\S+");

			// Settle it first ....
			inStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(wm == WorkMode.Strict)
			{
				return ((re.IsMatch(inStr)) && (inStr != "")); 
			}
			else
			{
				//return ((re.IsMatch(inStr)) || (inStr == ""));
                return ((reRelaxed.IsMatch(inStr)) || (inStr == ""));  // <- vvdolar:  7/Apr/2015
			}
		}

        /// <summary>
        /// Validate the Email. Simple Check.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isSimpleEmail(string inStr, WorkMode wm)
        {
            Regex re = new Regex(@"\S+@\S+\.\S+");

            // Settle it first ....
            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (wm == WorkMode.Strict)
            {
                return ((re.IsMatch(inStr)) && (inStr != ""));
            }
            else
            {
                return ((re.IsMatch(inStr)) || (inStr == ""));  // <- vvdolar:  7/Apr/2015
            }
        }

        /// <summary>
        /// Attempt to validate the host ...
        /// </summary>
        /// <param name="emailStr"></param>
        /// <returns></returns>
        public static bool isHost(string emailStr)
        {
            string[] host = (emailStr.Split('@'));

            // Host?
            if (host.Length < 2)
            {
                return false;
            }

            string hostname = host[1];

            IPHostEntry IPhst = new IPHostEntry();

            try
            {
                IPhst = Dns.GetHostEntry(hostname);
            }
            catch
            {
                return false;
            }

            IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);

            Socket s = new Socket(endPt.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

            try
            {
                s.Connect(endPt);
                 return true;
            }
            catch
            {
                return false;
            }
        }

		#endregion 

        #region "Check if the control is of the specified type ..."

        /// <summary>
        /// is the control a text box ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static bool isTextBox(WebControl ctrl)
        {
            TextBox tb = new TextBox();
            bool itIS = false;

            if (ctrl.GetType() == tb.GetType())
            {
                itIS = true;
            }
            else
            {
                itIS = false;
            }
            
            // return 
            tb.Dispose();
            return itIS;
        }

        /// <summary>
        /// is the control a drop down list ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static bool isDropDown(WebControl ctrl)
        {
            DropDownList ddl = new DropDownList();
            bool itIS = false;

            if (ctrl.GetType() == ddl.GetType())
            {
                itIS = true;
            }
            else
            {
                itIS = false;
            }

            // return 
            ddl.Dispose();
            return itIS;
        }

        #endregion 

        #region "Validate Dates ..."

        /// <summary>
        /// Check a date by the default culture.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool isDate(string dateStr)
        {
            DateTime convDate;

            return DateTime.TryParse(dateStr, isCulture, DateTimeStyles.None, out convDate);
        }

        /// <summary>
        /// Check if a date can be parsed using the default culture and the specified form.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateForm"></param>
        /// <returns></returns>
        public static bool isDate(string dateStr, string dateForm)
        {
            DateTime convDate;

            return DateTime.TryParseExact(dateStr, dateForm, isCulture, DateTimeStyles.None, out convDate);
        }

        /// <summary>
        /// Check if any of the format is there.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateForm"></param>
        /// <returns></returns>
        public static bool isDate(string dateStr, DateParseMode pm)
        {
            DateTime convDate;
            String[] dateForms = GetDateFormatList(pm);

            return DateTime.TryParseExact(dateStr, dateForms, isCulture, DateTimeStyles.None, out convDate);
        }

        /// <summary>
        /// Check if any of the format is there ....
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateForm"></param>
        /// <returns></returns>
        public static bool isDate(string dateStr, out DateTime convDate, DateParseMode pm)
        {
            String[] dateForms = GetDateFormatList(pm);

            // Choose the Culture ...
            IFormatProvider curCulture = new CultureInfo(Convert.ToInt32(ALCS_Culture.English_Australia));

            return DateTime.TryParseExact(dateStr, dateForms, curCulture, DateTimeStyles.None, out convDate);
        }

        /// <summary>
        /// Get the format list ...
        /// </summary>
        /// <returns></returns>
        public static string[] GetDateFormatList(DateParseMode pm)
        {
            List<string> sep = new List<string>();
            List<string> days = new List<string>();
            List<string> months = new List<string>();
            List<string> years = new List<string>();
            List<string> dates = new List<string>();

            // delim. 
            sep.Add(".");
            sep.Add("/");
            sep.Add(" ");
            sep.Add("-");

            // days. 
            days.Add("d");
            days.Add("dd");

            // months. 
            if(pm == DateParseMode.Small)
            {
                months.Add("M");
                months.Add("MM");
            }
            else if (pm == DateParseMode.Middle)
            {
                months.Add("MMM");
            }
            else if (pm == DateParseMode.Long)
            {
                months.Add("MMMM");
            }
            else 
            {
                months.Add("M");
                months.Add("MM");
                months.Add("MMM");
                months.Add("MMMM");
            }

            // years. 
            years.Add("yyyy");

            // build the form 
            for (int s = 0; s < sep.Count; s++)
            {
                for (int d = 0; d < days.Count; d++)
                {
                    for (int m = 0; m < months.Count; m++)
                    {
                        for (int y = 0; y < years.Count; y++)
                        {
                            dates.Add(days[d] + sep[s] + months[m] + sep[s] + years[y]);
                        }
                    }
                }
            }

            // return 
            return dates.ToArray();
        }

        /// <summary>
        /// Edge Date.
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static bool isEdgeDate(DateTime inDate)
        {
            return ((inDate == DateTime.MinValue) || (inDate == DateTime.MaxValue));
        }

        /// <summary>
        /// Verge Date - Another Definition Of Edge.
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static bool isVergeDate(DateTime inDate)
        {
            return isEdgeDate(inDate);
        }


        /// <summary>
        /// Is Edge Date.
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static bool isEdge(DateTime inDate)
        {
            return isEdgeDate(inDate);
        }

        /// <summary>
        /// Edge Integer.
        /// </summary>
        /// <param name="inInteger"></param>
        /// <returns></returns>
        public static bool isEdge(int inInteger)
        {
            return ((inInteger == int.MinValue) || (inInteger == int.MaxValue));
        }

        /// <summary>
        /// Edge Decimal.
        /// </summary>
        /// <param name="inDecimal"></param>
        /// <returns></returns>
        public static bool isEdge(Decimal inDecimal)
        {
            return ((inDecimal == Decimal.MinValue) || (inDecimal == Decimal.MaxValue));
        }

        /// <summary>
        /// Double ...
        /// </summary>
        /// <param name="inDecimal"></param>
        /// <returns></returns>
        public static bool isEdge(Double  inDecimal)
        {
            return ((inDecimal == Double.MinValue) || (inDecimal == Double.MaxValue));
        }

        /// <summary>
        /// Edge String.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isEdge(String inString)
        {
            return String.IsNullOrEmpty(inString);
        }
        


        #endregion

        #region "Validate Times ..."

        /// <summary>
        /// Check if the passed string form an acceptable date.
        /// </summary>
        /// <param name="inTime"></param>
        /// <param name="tp"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isTime(string inTime, WorkMode wm, TimeParseMode tpm)
        {
            bool timeOK = false;

            // Clear undesired 
            inTime = inTime.Trim();

            if ((wm == WorkMode.Relaxed) && (inTime == ""))
            {
                timeOK = true;
            }
            else if ((wm == WorkMode.Strict) && (inTime == ""))
            {
                timeOK = false;
            }
            else
            {
                // Create Fake Day 
                DateTime dummyDate = new DateTime(1, 1, 1);

                // Fix the time string 
                if (inTime.IndexOf(":") < 0)
                {
                    inTime += ":00";
                }

                // String Date time
                string datetimeStr = dummyDate.ToString("dd MMM yyyy") + " " + inTime;
                DateTime madeDate;

                bool goodDate = ALCS_DateReaderWriter.ReadDate(datetimeStr, out madeDate);

                if (!goodDate)
                {
                    timeOK = false;
                }
                else if ((tpm == TimeParseMode.Hours_12) && (madeDate.Hour > 12))
                {
                    timeOK = false;
                }
                else
                {
                    timeOK = true;
                }
            }

            // return 
            return timeOK;
        }

        /// <summary>
        /// Another Variation. 
        /// </summary>
        /// <param name="inTime"></param>
        /// <returns></returns>
        public static bool isTime(string inTime)
        {
            return isTime(inTime, WorkMode.Strict, TimeParseMode.Hours_24);
        }

        /// <summary>
        /// Another Variation. 
        /// </summary>
        /// <param name="inTime"></param>
        /// <param name="tpm"></param>
        /// <returns></returns>
        public static bool isTime(string inTime, TimeParseMode tpm)
        {
            return isTime(inTime, WorkMode.Strict, tpm);
        }

        /// <summary>
        /// Another Variation. 
        /// </summary>
        /// <param name="inTime"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isTime(string inTime, WorkMode wm)
        {
            return isTime(inTime, wm, TimeParseMode.Hours_24);
        }

        #endregion 

		#region "Validate UserName and Password "
		
		/// <summary>
		/// Check the Validaty of the Password .....
		/// </summary>
		/// <param name="passStr"></param>
		/// <param name="minLength"></param>
		/// <param name="maxLength"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isPassword(string passStr, int minLength, int maxLength, WorkMode wm)
		{
			passStr = ALCS_DataShift.WhenNull(passStr, "");
  	
            // Blank 
            if ((wm == WorkMode.Relaxed) && (passStr.Length == 0))
            {
                return true;
            }

			// Check the length ... against the minimum allowed
			if(passStr.Length < minLength)
			{
				return false;
			}

			// and the maximum allowed .....
			if(passStr.Length > maxLength)
			{
				return false;
			}

			// Do we have any white spaces ..... 
			if(Regex.IsMatch(passStr, @"\s"))
			{
				return false;
			}

			if(wm == WorkMode.Strict)
			{
				// must have a digit ...
				if(!Regex.IsMatch(passStr,@"\d"))
				{
					return false;
				}
                // must have at least one Uppercase alpha ....
                if (!Regex.IsMatch(passStr, @"[A-Z]"))
                {
                    return false;
                }

                // must have at least one Lowercase alpha ....
                if (!Regex.IsMatch(passStr, @"[a-z]"))
                {
                    return false;
                }

                // vvdolar - removed the following 2 if Statements (7/Apr/2015 - WO214784_Pk661_ChgPasswordComplexity) 
                // must have an alpha ....
                //if(!Regex.IsMatch(passStr,@"\D"))
                //{
                //    return false;
                //}

                //// must be all in the Range (a-zA-Z_0-9)
                //if (Regex.IsMatch(passStr, @"\W"))
                //{
                //    return false;
                //}

                // vvdolar - added following requirement, WO214784_Pk661_ChgPasswordComplexity - 7/Apr/2015
                // must have at least one of the character range (special characters  =  !"#$%&'()*+,-./:;<=>?@[\]^_`{|}~  )

                if (!Regex.IsMatch(passStr, @"[!#\$%&'()\*\+,-\./\:\;<=>\?@[\]\^_`{|}~\\""]"))
                {
                    return false;
                }

            }

			// Exit 
			return true;
		}

		/// <summary>
		/// Another version of the function with the minimum and maximum length defaulted
		/// </summary>
		/// <param name="passStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static bool isPassword(string passStr, WorkMode wm)
		{
			return isPassword(passStr, ALCS_Basics.ALCS_Vault.minPass, ALCS_Basics.ALCS_Vault.maxPass,wm);
		}

        /// <summary>
        /// Another Ovelload
        /// </summary>
        /// <param name="passStr"></param>
        /// <returns></returns>
		public static bool isPassword(string passStr)
		{
            return isPassword(passStr, ALCS_Basics.ALCS_Vault.minPass, ALCS_Basics.ALCS_Vault.maxPass, WorkMode.Strict);
		}

		/// <summary>
		/// The user name can be any sequence of characters [A-Z,a-z,0-9]
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="minLength"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static bool isUsername(string inStr, int minLength, int maxLength)
		{
			inStr = ALCS_DataShift.WhenNull(inStr, "");
  	
			// Check the length ... against the minimum allowed
			if(inStr.Length < minLength)
			{
				return false;
			}

			// and the maximum allowed .....
			if(inStr.Length > maxLength)
			{
				return false;
			}

			// Do we have any white spaces ..... 
			if(Regex.IsMatch(inStr, @"\s"))
			{
				return false;
			}

			// Do we have any non word base characters 
			if(Regex.IsMatch(inStr, @"\W"))
			{
				return false;
			}
			
			// Exit 
			return true;
		}

		/// <summary>
		/// Another version of the function with the minimum and maximum length assumed
		/// to be 6 and 20 	
		/// /// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static bool isUsername(string inStr)
		{
            return isUsername(inStr, ALCS_Basics.ALCS_Vault.minUname, ALCS_Basics.ALCS_Vault.maxUname);
		}

        /// <summary>
        /// Simple Password.
        /// </summary>
        /// <param name="passStr"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isSimplePassword(string passStr, int minLength, int maxLength, WorkMode wm)
        {
            passStr = ALCS_DataShift.WhenNull(passStr, "");

            // Blank 
            if ((wm == WorkMode.Relaxed) && (passStr.Length == 0))
            {
                return true;
            }

            // Check the length ... against the minimum allowed
            if (passStr.Length < minLength)
            {
                return false;
            }

            // and the maximum allowed .....
            if (passStr.Length > maxLength)
            {
                return false;
            }

            // Do we have any white spaces ..... 
            if (Regex.IsMatch(passStr, @"\s"))
            {
                return false;
            }

            // Exit 
            return true;
        }

        /// <summary>
        /// Another version of the function with the minimum and maximum length defaulted
        /// </summary>
        /// <param name="passStr"></param>
        /// <param name="wm"></param>
        /// <returns></returns>
        public static bool isSimplePassword(string passStr, WorkMode wm)
        {
            return isSimplePassword(passStr, ALCS_Basics.ALCS_Vault.minSimplePass, ALCS_Basics.ALCS_Vault.maxSimplePass, wm);
        }

        /// <summary>
        /// Another Ovelload
        /// </summary>
        /// <param name="passStr"></param>
        /// <returns></returns>
        public static bool isSimplePassword(string passStr)
        {
            return isSimplePassword(passStr, ALCS_Basics.ALCS_Vault.minSimplePass, ALCS_Basics.ALCS_Vault.maxSimplePass, WorkMode.Strict);
        }


		#endregion 
	}
}
