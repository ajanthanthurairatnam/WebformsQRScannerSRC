using System;
using System.Threading;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;

using ALCS_Library.ALCS_Format;


namespace ALCS_Library.ALCS_Data
{
    // Public ...
    public enum SecretCodeScope
    {
        Set_0_9 = 0,
        Set_a_z = 1,
        Set_A_Z = 2,
        Set_Symbols = 3
    }


    #region "ASCII Range ..."

    /// <summary>
    /// Create an ASCII Range ...
    /// Currently not needed ....
    /// </summary>
    public class ALCS_ASCIIRange
    {
        int lowerCode;
        int upperCode;
        Random rand;

        /// <summary>
        /// Constructor - No check done on range ....
        /// </summary>
        /// <param name="low"></param>
        /// <param name="up"></param>
        public ALCS_ASCIIRange(int low, int up)
        {
            this.lowerCode = low;
            this.upperCode = up;
            rand = new Random();
        }

        /// <summary>
        /// Randomly pick a char from the range ....
        /// </summary>
        /// <returns></returns>
        public char RandomChar()
        {
            // Pick a random number 
            double rn = rand.NextDouble();

            // A random code in the range ...
            int rnIndex = (int) (rn * (this.upperCode - this.lowerCode + 1)) + this.lowerCode;

            // return the char 
            return Convert.ToChar(rnIndex);
        }
    }

    #endregion 

    #region "Generate a Random Code - Scramble - Unscramble"

    public static class  ALCS_SecretCode
    {
        static Random rand = new Random();

        #region "Gnerate repetetive testing ..."

        /// <summary>
        /// Get the Char Scope to generate the code from.
        /// </summary>
        /// <param name="has_0_9"></param>
        /// <param name="has_a_z"></param>
        /// <param name="has_A_Z"></param>
        /// <param name="has_Symbols"></param>
        /// <returns></returns>
        public static List<Char> GetTheCharScope(bool has_0_9, bool has_a_z, bool has_A_Z, bool has_Symbols)
        {
            List<Char> ascii = new List<Char>();

            // Should we allow digits ...
            if (has_0_9)
            {
                ascii.AddRange(AddDigits());
            }

            // Should we allow lower chars a_z ...
            if (has_a_z)
            {
                ascii.AddRange(AddLowerChars());
            }

            // Should we allow Upper chars A_Z ...
            if (has_A_Z)
            {
                ascii.AddRange(AddUpperChars());
            }

            // Should we allow Symbols
            if (has_Symbols)
            {
                ascii.AddRange(AddSymbols());
            }

            // Safety Measure 
            if (!(has_0_9 || has_a_z || has_A_Z || has_Symbols))
            {
                ascii.AddRange(AddDigits());
                ascii.AddRange(AddLowerChars());
                ascii.AddRange(AddUpperChars());
            }
            return ascii;
        }

        /// <summary>
        /// Like the above but default to alphanumeric.
        /// </summary>
        /// <returns></returns>
        public static List<Char> GetTheCharScope()
        {
            return GetTheCharScope(true, true, true, false);
        }

        /// <summary>
        /// Generate a code based on this randomiser.
        /// </summary>
        /// <param name="has_0_9"></param>
        /// <param name="has_a_z"></param>
        /// <param name="has_A_Z"></param>
        /// <param name="has_Symbols"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static string GenerateCode(bool has_0_9, bool has_a_z, bool has_A_Z, bool has_Symbols, Random rand, int codeLen)
        {
            string genCode = "";
            List<Char> ascii = GetTheCharScope(has_0_9, has_a_z, has_A_Z, has_Symbols);

            // We have set the scope now lets Pick them one by one.
            int aCount = ascii.Count;

            // Generate a random from 0 till aCount 
            for (int idx = 0; idx < codeLen; idx++)
            {
                double rn = rand.NextDouble();
                int raIndex = (int)(rn * ascii.Count);

                // Generate a random object 
                Char thisChar = ascii[raIndex];
                genCode += thisChar;
            }

            // return code;
            return genCode;

        }


        #endregion 

        #region "Generate a Random code ..."
        /// <summary>
        /// Gnerate some code that meets certain criteria for length and scope.
        /// </summary>
        /// <param name="codeScope"></param>
        /// <param name="codeCase"></param>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string GenerateCode(bool has_0_9, bool has_a_z, bool has_A_Z, bool has_Symbols, int codeLen)
        {
            string genCode = "";
            List<Char> ascii = new List<Char>();

            // Should we allow digits ...
            if (has_0_9)
            {
                ascii.AddRange(AddDigits());
            }

            // Should we allow lower chars a_z ...
            if (has_a_z)
            {
                ascii.AddRange(AddLowerChars());
            }

            // Should we allow Upper chars A_Z ...
            if (has_A_Z)
            {
                ascii.AddRange(AddUpperChars());
            }

            // Should we allow Symbols
            if (has_Symbols)
            {
                ascii.AddRange(AddSymbols());
            }

            // Safety Measure 
            if (!(has_0_9 || has_a_z || has_A_Z || has_Symbols))
            {
                ascii.AddRange(AddDigits());
                ascii.AddRange(AddLowerChars());
                ascii.AddRange(AddUpperChars());
            }

            // We have set the scope now lets Pick them one by one.
            int aCount = ascii.Count;
            

            // Generate a random from 0 till aCount 
            for (int idx= 0 ; idx < codeLen; idx++)
            {
                
                double rn = rand.NextDouble();
                int raIndex = (int) (rn * ascii.Count);

                // Generate a random object 
                Char thisChar = ascii[raIndex];
                genCode += thisChar;
            }

            // return code;
            return genCode;
        }


        /// <summary>
        /// All possible digits in the random generator ...
        /// </summary>
        /// <param name="ascii"></param>
        private static Char[] AddDigits()
        {
            Char[] digits = new char[] { '2', '3', '4', '5', '7', '8', '9' };

            return digits;
        }

        /// <summary>
        /// All possible Upper Chars in the random generator ...
        /// </summary>
        private static Char[] AddUpperChars()
        {
            Char[] uChars = new char[] {'A','B','C','D','E','F','H','J','K','L','M','N','P','Q','R','S','T','U','V','X','Y'};

            return uChars;
        }

        /// <summary>
        /// All possible Lower Chars in the random generator ...
        /// </summary>
        private static Char[] AddLowerChars()
        {
            Char[] lChars = new char[] {'a','b','c','d','e','f','g','h','j','k','m','n','p','q','r','s','t','u','v','x','y'};

            return lChars;
        }

        /// <summary>
        /// All possible symbols in the random generator ...
        /// </summary>
        private static Char[] AddSymbols()
        {
            Char[] symbols = new char[] { '!', '?', ')', '(', '#', '$', '@', '~'};

            return symbols;
        }

    #endregion 

}
    #endregion

}