using System;
using System.Text;
using System.Text.RegularExpressions; 
using ALCS_Library.ALCS_Data; 

namespace ALCS_Library.ALCS_Format
{
	////////////////////////////////////////////////////////////////////
	/// Enumeration reguired for date formatting.
	////////////////////////////////////////////////////////////////////
	#region "Enumeraion and contants needed by the string formatting"
	
	// Strict: Replace all blindly.
	// Relaxed: Choose most common.
	public enum WorkMode
	{
		Strict,
		Relaxed
	}

	// Used by the Squeeze function.
	public enum MatchMode
	{
		ALL,
		AllButCr,
		OnlySpaces
	}

    /// <summary>
    /// A simple lookup list ....
    /// </summary>
    public enum PaddingSide
    {
        Left,
        Right
    }
	

    /// <summary>
    /// 
    /// </summary>
    public enum TextFormat
    {
        LowerCase,
        UpperCase,
        UpperLower,
        FirstCharUp
    }

    #endregion

    /////////////////////////////////////////////////////////////////
	/// String Formatting Functionality.
	/////////////////////////////////////////////////////////////////
	#region "Rewrite a string to the desired format"
	
	/// <summary>
	/// All string handlings functions
	/// Created By Jamil 17 Sep 2005.
	/// </summary>
	public class ALCS_StringRewrite
	{

        public static string jsAsVarApproved(string inStr)
        {
            string apprStr;

            //Initialize
            apprStr = ALCS_DataShift.WhenNull(inStr, "");

            if (apprStr != "")
            {
                apprStr = Regex.Escape(apprStr);
                apprStr = apprStr.Replace(@"""", "\\\"");
                apprStr = apprStr.Replace(@"'", "\\\'");
            }

            // return
            return apprStr;
        }


        /// <summary>
        ///  Prepare the string for Javascript handling. This will basically insert
        ///  an escape characterin front of each character with a special meaning 
        ///  to JavaScript.
        ///  
        ///	 Written By jamil 15/09/2005.
        ///	 N.B. the list of characters is not complete ny anu means, please add to 
        ///	 it as you see fit.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string BrowserFriendly_jsAsVarApproved(string inStr)
        {
            string apprStr;

            //Initialize
            apprStr = ALCS_DataShift.WhenNull(inStr, "");

            if (apprStr != "")
            {
                apprStr = Regex.Escape(apprStr);
                apprStr = apprStr.Replace(@"""", "\\\"");
                apprStr = apprStr.Replace(@"'", "\\\'");
            }

            // return
            return apprStr;
        }


        /// <summary>
        /// Mask special characters so that it will be handled properly by ALERT.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string BrowserFriendly_jsAlertApproved(string inStr)
        {
            string apprStr;

            //Initialize
            apprStr = ALCS_DataShift.WhenNull(inStr, "");

            if (apprStr != "")
            {
                apprStr = Regex.Escape(apprStr);
                apprStr = apprStr.Replace(@"""", "\\\"");
                apprStr = apprStr.Replace(@"'", "\\\'");
            }

            // return
            return apprStr;
        }

		/// <summary>
		///	  Squeeze the string by removing white spaces.
		///	  
		///		- repMode:
		///			1. S = Srict replace all matches by nothing.
		///			2. R = Relaxed replace all matches by single space.
		///			
		///		- matchMode
		///			1. ALL Match all white spaces
		///			2. NCR Matches all white spaces but not CR.
		///			3. OSP Match only spaces 
		///			
		///		Written By Jamil on 17 Sep 2005.
		/// </summary>
		/// <param name="repMod"></param>
		/// <param name="mm"></param>
		/// <returns></returns>
		public static string StringRewriter(string inStr, WorkMode rm, MatchMode mm)
		{
			string repStr;
			string outStr;

			// What should we replace with.
			if(rm == WorkMode.Strict )
			{
				repStr = ""; //Blank
			}
			else if(rm == WorkMode.Relaxed)
			{
				repStr = " "; //SPACE
			}
			else
			{
				return	inStr;
			}
				
			// Load the string 
			outStr = ALCS_DataShift.WhenNull(inStr, "");
 
			if(mm == MatchMode.ALL)
			{
				outStr = Regex.Replace(outStr, @"\s+", " ");
				outStr = Regex.Replace(outStr, @"[ ]+", repStr);					
			}
			else if(mm == MatchMode.AllButCr)
			{
				outStr = Regex.Replace(outStr, @"[ \f\r\t\v]+", " ");
				outStr = Regex.Replace(outStr, @"  ", repStr);					
			}
			else if(mm == MatchMode.OnlySpaces)
			{
				outStr = Regex.Replace(outStr, @"[ ]+", " ");
				outStr = Regex.Replace(outStr, @"\s+", repStr);
			}
						
			// return 
			return outStr;
		}

        /// <summary>
        /// Another version Just remove dublicate spaces.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="rm"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static string Squeeze(string inStr)
        {
            return StringRewriter(inStr, WorkMode.Relaxed, MatchMode.ALL);
        }

        /// <summary>
        /// clear all white spaces from a string ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string ClearWhiteSpaces(string inStr)
        {
            return StringRewriter(inStr, WorkMode.Strict, MatchMode.ALL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inPhone"></param>
        /// <returns></returns>
        public static string Cleaner_PhoneCleaner(string inPhone)
        {
            Regex re = new Regex("[^0-9]");
            string outPhone = re.Replace(inPhone, "");
            return outPhone;
        }

        /// <summary>
        /// Car Rego Cleaner.
        /// </summary>
        /// <param name="inPhone"></param>
        /// <returns></returns>
        public static string Cleaner_CarRegoCleaner(string inPhone)
        {
            Regex re = new Regex("[^0-9a-zA-Z]");
            string outPhone = re.Replace(inPhone, "");
            return outPhone;
        }

        /// <summary>
        /// The strings are made of the same characters
        /// </summary>
        /// <param name="inStr1"></param>
        /// <param name="inStr2"></param>
        /// <returns></returns>
        public static bool SameCore(string inStr1, string inStr2)
        {
            return (ClearWhiteSpaces(inStr1).ToLower() == ClearWhiteSpaces(inStr2).ToLower());
        }

        /// <summary>
        /// Make a sentence out of the array of words ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string SentenceMaker(string[] inWords)
        {
            string outSentence = "";

            for (int idx=0; idx < inWords.Length; idx++)
            {
                outSentence += " " + inWords[idx];
            }

            return Squeeze(outSentence);
        }

        /// <summary>
        /// Make a sentence out of the array of words ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string SentenceMaker(string[] inWords, string join)
        {
            string outSentence = "";

            for (int idx = 0; idx < inWords.Length; idx++)
            {
                if (inWords[idx].Trim() != "")
                {
                    outSentence += ((outSentence == "") ? "" : join) + inWords[idx];
                }
            }
            return Squeeze(outSentence);
        }


		/// <summary>
		/// delimit the string by the passed pattern
		/// Written By Jamil on 17 Sep 2005.
		/// </summary>
		/// <param name="initStr"></param>
		/// <param name="delimStr"></param>
		/// <param name="delimForm"></param>
		/// <returns></returns>
		public static string DelimitStr (string initStr, string delimStr, int [] delimForm)
		{
			string chunckStr; 
			string wholeStr; 
			int idx = 0; 
			int chunckLength; 
			string newStr;

			// Initialize
			wholeStr = StringRewriter(initStr,WorkMode.Strict,MatchMode.ALL);
			chunckStr = "";
			newStr = "";
		
			// Loop Through ....
			while ((idx < delimForm.Length) && (wholeStr != "")) 
			{ 
				chunckLength = delimForm[idx]; 
				chunckStr = wholeStr.Substring(wholeStr.Length - chunckLength, chunckLength); 
				wholeStr = wholeStr.Substring(0, wholeStr.Length - chunckStr.Length); 
				if ((newStr == "")) 
				{ 
					newStr = chunckStr; 
				} 
				else 
				{ 
					newStr = chunckStr + delimStr + newStr; 
				} 

				// Next One
				idx = idx + 1; 
			} 

			// Any Residus
			if (wholeStr != "") 
			{ 
				newStr = wholeStr.Trim() + newStr; 
			} 

			// Exit the function.
			return newStr;
		}

        /// <summary>
        /// Format as an ABN
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsABN(string initStr)
        {
            string alterStr = ClearWhiteSpaces(initStr);

            if (alterStr.Length == 11)
            {
                return DelimitStr(alterStr, " ", new int[] {3,3,3,2});
            }
            else
            {
                return initStr;
            }
        }

        /// <summary>
        /// Format as an ABN
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsACN(string initStr)
        {
            string alterStr = ClearWhiteSpaces(initStr);

            if (alterStr.Length == 9)
            {
                return DelimitStr(alterStr, " ", new int[] { 3, 3, 3 });
            }
            else
            {
                return initStr;
            }
        }


        /// <summary>
        /// Format as Phone/Fax
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsPhoneFax(string initStr)
        {
            string alterStr = ClearWhiteSpaces(initStr);

            if (alterStr.Length == 8)
            {
                return DelimitStr(alterStr, " ", new int[] {4,4 });
            }
            else if ((alterStr.Length == 10) && (alterStr.StartsWith("1800")))
            {
                return DelimitStr(alterStr, " ", new int[] { 3, 3, 4 });
            }
            else if (alterStr.Length == 10)
            {
                return DelimitStr(alterStr, " ", new int[] { 4, 4, 2 });
            }
            else
            {
                return initStr;
            }
        }

        /// <summary>
        /// Format as STD Phone/Fax
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsSTDPhoneFax(string initStr)
        {
            string alterStr = ClearWhiteSpaces(initStr);

            if (alterStr.Length == 10)
            {
                return DelimitStr(alterStr, " ", new int[] { 4, 4, 2 });
            }
            else
            {
                return initStr;
            }
        }


        /// <summary>
        /// Format as STD Phone/Fax
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsMobile(string initStr)
        {
            string alterStr = ClearWhiteSpaces(initStr);

            if (alterStr.Length == 10)
            {
                return DelimitStr(alterStr, " ", new int[] { 3, 3, 4 });
            }
            else
            {
                return initStr;
            }
        }

        /// <summary>
        /// Simply Bring to lower case.
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsEmail(string initStr)
        {
            return ClearWhiteSpaces(initStr).ToLower();
        }

        /// <summary>
        /// Simply Bring to lower case.
        /// </summary>
        /// <param name="initStr"></param>
        /// <returns></returns>
        public static string WriteAsURL(string initStr)
        {
            return ClearWhiteSpaces(initStr).ToLower();
        }


		/// <summary>
		/// Proper Casing the string.
		/// In a strict mode the proper case will apply regardless
		/// otherwise if the string is in upper case it will be left as IS.
		/// 
		/// Written By Jamil on 17 Sep 2005.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static string ProperCase(string inStr, WorkMode wm)
		{
				
			// Initialize
			inStr = ALCS_DataShift.WhenNull(inStr,"");
 
			if(inStr == "")
			{
				return inStr;
			}
			else if(wm == WorkMode.Strict) 
			{
				return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(inStr.ToLower());    
			}
			else
			{
				return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(inStr);
			}
		}

		/// <summary>
		/// Conserve the Special Characters such as CR, LF.
		/// 
		/// Written By Jamil on 17 Sep 2005.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="wm"></param>
		/// <returns></returns>
		public static string PreserveHTMLFormat(string inStr, WorkMode wm)
		{
			string outStr;
			Regex reRET	= new Regex("\r\n|\n\r|\r\f|\f\r");
			Regex reBR	= new Regex("\n|\f|\r");
			Regex reTAB	= new Regex("\t");
			Regex reSPACE = new Regex(" ");
 
			// Initialize
			outStr = inStr;

			// Preserve	the new line
			outStr = reRET.Replace(outStr,"<BR>");
			outStr = reBR.Replace(outStr,"<BR>");

			// Preserve the tab.
			outStr = reTAB.Replace(outStr,"&nbsp;&nbsp;&nbsp;&nbsp;");

			// Preserve the space
			if(wm == WorkMode.Strict)
			{
				outStr = reSPACE.Replace(outStr, "&nbsp;");
			}

			// Finish up.
			return outStr;
		}

		/// <summary>
		/// Clean The field and prepare for client display, eliminate all 
		/// possible Error causes 
		/// 
		/// Written By jamil 15/09/2005.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static string Crystal(string inStr) 
		{ 
			string crystalStr; 
			crystalStr = ALCS_DataShift.WhenNull(inStr, "");

			if (crystalStr != "") 
			{ 
				crystalStr = crystalStr.Replace("\n", ""); 
				crystalStr = crystalStr.Replace("\r", ""); 
				crystalStr = crystalStr.Replace("\t", ""); 
				crystalStr = crystalStr.Replace("\"", ""); 
				crystalStr = crystalStr.Replace("%", ""); 
				crystalStr = crystalStr.Replace("$", ""); 
				crystalStr = crystalStr.Replace("'", ""); 
				crystalStr = crystalStr.Replace(",", ""); 
				crystalStr = crystalStr.Replace(";", ""); 
			} 
			return crystalStr.Trim(); 
		} 

        /// <summary>
        /// Remove tabs, CR and LF.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string SQLFriendly_RemoveWideWS(string inStr)
        {
            string sqlText;
            sqlText = ALCS_DataShift.WhenNull(inStr, "");

            if (sqlText != "")
            {
                sqlText = sqlText.Replace("\n", "");
                sqlText = sqlText.Replace("\r", "");
                sqlText = sqlText.Replace("\t", "");
            }

            // return.
            return Squeeze(sqlText.Trim()); 
        }

		/// <summary>
		///  Prepare the string for Javascript handling. This will basically insert
		///  an escape characterin front of each character with a special meaning 
		///  to JavaScript.
		///  
		///	 Written By jamil 15/09/2005.
		///	 N.B. the list of characters is not complete ny anu means, please add to 
		///	 it as you see fit.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static string BrowserFriendly_jsApproved(string inStr) 
		{ 
			string apprStr;
 
			//Initialize
			apprStr = ALCS_DataShift.WhenNull(inStr, ""); 
				
			if ((apprStr != "")) 
			{ 
				apprStr = apprStr.Replace("%", "%25"); 
				apprStr = apprStr.Replace("\"", "%22"); 
				apprStr = apprStr.Replace("'", "%27"); 
				apprStr = apprStr.Replace("\n", "%0A"); 
			} 
				
			// return
			return apprStr; 
		}

        /// <summary>
        ///  Prepare the string for Javascript handling. This will basically insert
        ///  an escape characterin front of each character with a special meaning 
        ///  to JavaScript.
        ///  
        ///	 Written By jamil 15/09/2005.
        ///	 N.B. the list of characters is not complete ny anu means, please add to 
        ///	 it as you see fit.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>




		/// <summary>
		/// Mask special Characters so that JS will accept ' & ""
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
        public static string BrowserFriendly_Escape(string inStr)
		{
            string midStr = BrowserFriendly_jsApproved(inStr);

			return "unescape('" + midStr + "')";
		}

        /// <summary>
        /// Mask special Characters so that JS will accept ' & ""
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string BrowserFriendly_jscriptFunctionParameter(string inStr)
        {
            string midStr = BrowserFriendly_jsApproved(inStr);

            return "unescape('" + midStr + "')";
        }

        
        /// <summary>
		///	 Prepare the string for JavaScript handling. This will basically insert an 
		///	 escape caharcter in front of each character with a special meaning to 
		///	 JavaScript.
		///	 
		///	 Written By jamil 15/09/2005
		///	 N.B. the list of characters is not complete ny anu means, please add to 
		///	 it as you see fit.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
        public static string BrowserFriendly_HtmlApproved(string inStr) 
		{ 
			string apprStr; 
				
			// Initialize
			apprStr = ALCS_DataShift.WhenNull(inStr, ""); 
			if ((apprStr != "")) 
			{ 
				apprStr = apprStr.Replace("\"", "&#34;"); 
				apprStr = apprStr.Replace("'", "&#39;"); 
			}
 
			// return
			return apprStr;
        }


        /// <summary>
        ///	 Prepare the string for JavaScript handling. This will basically insert an 
        ///	 escape caharcter in front of each character with a special meaning to 
        ///	 JavaScript.
        ///	 
        ///	 Written By jamil 15/09/2005
        ///	 N.B. the list of characters is not complete ny anu means, please add to 
        ///	 it as you see fit.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string HtmlApproved(string inStr)
        {
            string apprStr;

            // Initialize
            apprStr = ALCS_DataShift.WhenNull(inStr, "");
            if ((apprStr != ""))
            {
                apprStr = apprStr.Replace("\"", "&#34;");
                apprStr = apprStr.Replace("'", "&#39;");
            }

            // return
            return apprStr;
        }

        /// <summary>
        ///	 Prepare the string to be rendered as a tool tip
        ///	 
        ///	 Written By jamil 15/08/2008
        ///	 N.B. the list of characters is not complete ny anu means, please add to 
        ///	 it as you see fit.
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string BrowserFriendly_HtmlTooTip(string inStr)
        {
            string apprStr;

            // Initialize
            apprStr = ALCS_DataShift.WhenNull(inStr, "");
            if ((apprStr != ""))
            {
                apprStr = apprStr.Replace("\"", "&#34;");
                apprStr = apprStr.Replace("'", "&#39;");
            }

            // return
            return apprStr;
        }



        #region "General String Tools ..."

        public static string ReverseString(string inStr)
        {
            int inLen = inStr.Length;
            char[] charArray = new char[inLen];

            for (int i = 0; i < inLen; i++)
                charArray[i] = inStr[inLen - 1 - i];

            // return the reversed string
            return new string(charArray);
        }

        #endregion 

        #region "String Padding ...."

        /// <summary>
        /// Pad the passed string to the specified length and mode ....
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="pm"></param>
        public static string ZeroPad(string inStr, int inLength, PaddingSide ps)
        {
            string newStr;

            inStr = ALCS_DataShift.WhenNull(inStr, "");

            if (inStr.Length < inLength)
            {
                if(ps == PaddingSide.Left) 
                {
                    newStr = inStr.PadLeft(inLength, '0'); 
                }
                else if(ps == PaddingSide.Right)
                {
                    newStr = inStr.PadRight(inLength, '0'); 
                }
                else
                {
                    newStr = inStr;
                }
            }
            else
            {
                newStr = inStr;
            }

            // return the string 
            return newStr;
        }

        /// <summary>
        /// Another version 
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inLength"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string ZeroPad(int inStr, int inLength, PaddingSide ps)
        {
            return ZeroPad(Convert.ToString(inStr), inLength, ps);
        }

        /// <summary>
        /// Assume the left padding ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inLength"></param>
        /// <returns></returns>
        public static string ZeroPad(string inStr, int inLength)
        {
            return ZeroPad(inStr, inLength, PaddingSide.Left);    
        }

        /// <summary>
        /// Assume the left padding ...
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inLength"></param>
        /// <returns></returns>
        public static string ZeroPad(int inStr, int inLength)
        {
            return ZeroPad(Convert.ToString(inStr), inLength);
        }


#endregion 

        #region "Upper Lower Casing a Text Transformation "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inSentence"></param>
        /// <param name="tm"></param>
        /// <param name="presCapital"></param>
        /// <returns></returns>
        public static string TransformSentence(string inSentence, TextFormat tm)
        {
            return TransformSentence(inSentence, tm, true);

        }

        /// <summary>
        /// Transform the Sentence to the specified format.
        /// </summary>
        /// <param name="inWord"></param>
        /// <param name="tm"></param>
        /// <param name="presCapital"></param>
        /// <returns></returns>
        public static string TransformSentence(string inSentence, TextFormat tm, bool presCapital)
        {
            int idx;
            string[] inWords;

            // Define ....
            inSentence = ALCS_DataShift.WhenNull(inSentence, "");

            // Split 
            inWords = inSentence.Split(" ".ToCharArray());

            // Transform Tokens 
            for (idx = 0; idx < inWords.Length; idx++)
            {
                inWords[idx] = TransformWord(inWords[idx], tm, presCapital);
            }

            // return ....
            return String.Join(" ", inWords);
        }

        /// <summary>
        /// Transform a single word to the specified format.
        /// </summary>
        /// <param name="inWord"></param>
        /// <param name="tm"></param>
        /// <param name="presCapital"></param>
        /// <returns></returns>
        public static string TransformWord(string inWord, TextFormat tm, bool presCapital)
        {
            string outWord;
            string upWord;
            string token1, token2;

            // Define ....
            inWord = ALCS_DataShift.WhenNull(inWord, "");
            upWord = inWord.ToUpper();

            // Transform the Text 
            if(inWord == "")
            {
                outWord = inWord;
            }
            else if((presCapital) && (inWord == upWord))
            {
                outWord = inWord;
            }
            else if(tm == TextFormat.UpperCase)
            {
                outWord = inWord.ToUpper();
            }
            else if (tm == TextFormat.LowerCase)
            {
                outWord = inWord.ToLower();
            }
            else if ((tm == TextFormat.UpperLower) || (tm == TextFormat.FirstCharUp))
            {
                if (inWord.Length == 1)
                {
                    token1 = inWord;
                    token2 = "";
                }
                else
                {
                    token1 = inWord.Substring(0, 1);
                    token2 = inWord.Substring(1);
                }

                if (tm == TextFormat.UpperLower)
                {
                    outWord = token1.ToUpper() + token2.ToLower();
                }
                else
                {
                    outWord = token1.ToUpper() + token2;
                }
            }
            else
            {
                outWord = inWord;
            }
            
            // Return 
            return outWord;
        }

        #endregion 

        #region "Converters to other type ..."
        // Those need to be written out with an out variables.
        // Talk to Jamil if you need them now 

        /// <summary>
        /// convert a char to an int.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="badValue"></param>
        /// <returns></returns>
        public static int ChartoInt(char from)
        {
            return (int)Char.GetNumericValue(from);
        }


        ///// <summary>
        ///// convert a string to an intor return the exception value badValue.
        ///// </summary>
        ///// <param name="from"></param>
        ///// <param name="badValue"></param>
        ///// <returns></returns>
        //public static int StrToInt(string from, int badValue)
        //{
        //    int intVal;

        //    if (int.TryParse(from, out intVal))
        //    {
        //        return intVal;
        //    }
        //    else
        //    {
        //        return badValue;
        //    }
        //}

        ///// <summary>
        ///// Like the above but return -1 as the bad value.
        ///// </summary>
        ///// <param name="from"></param>
        ///// <returns></returns>
        //public static int StrToInt(string from)
        //{
        //    return StrToInt(from, -1);
        //}

        #endregion 
    }
	#endregion 


   
}
