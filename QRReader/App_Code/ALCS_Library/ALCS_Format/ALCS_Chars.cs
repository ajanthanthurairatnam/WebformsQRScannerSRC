using System;
using System.Text;
using System.Text.RegularExpressions; 
using ALCS_Library.ALCS_Data; 

namespace ALCS_Library.ALCS_Format
{
	
    #region "Char Handling "

    /// <summary>
	/// Chars Tools
	/// Created By Jamil 30 Jan 2007.
	/// </summary>
    public class ALCS_CharTools
    {
        /// <summary>
        /// return the ascii code of a char 
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static byte CharToASCII(char chr)
        {
            Char[] chars = new char[] {chr};
            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(chars);
            return bytes[0];
        }

        /// <summary>
        /// return the Char representation of the ascii code
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static Char ASCIIToChar(byte chr)
        {
            // Convert to chars 
            byte[] bytes = new byte[] { chr };
            Char[] chars = System.Text.ASCIIEncoding.ASCII.GetChars(bytes);
            return chars[0];
        }

        /// <summary>
        /// return the String representation of the ascii code.
        /// A mere variation of the above - 
        /// Use with care - all int are assumed bytes ....
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string ASCIIToStr(int chr)
        {
            char theChar = ASCIIToChar((byte) chr);
            return theChar.ToString();
        }
    
    }

    #endregion 

}
