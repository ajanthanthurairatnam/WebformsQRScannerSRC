using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Numerics;

namespace ALCS_Library.ALCS_Format
{

    /// <summary>
    /// Summary description for ALCS_Addesses
    /// </summary>
    public class ALCS_Addesses
    {
        /// <summary>
        /// Get the address.
        /// </summary>
        /// <param name="addBlock"></param>
        /// <returns></returns>
        public static string Mail_SquashAddressLines(List<string> addBlock)
        {
            string theAddress = "";

            for (int i = 0; i < addBlock.Count; i++)
            {
                string thisLIne = ALCS_DataShift.WhenNull(addBlock[i], "");
                if (thisLIne != "")
                {
                    theAddress += ((theAddress == "") ? ("") : (Environment.NewLine)) + thisLIne;
                }
            }

            // Return the address ...
            return theAddress;
        }
        
        /// <summary>
        /// Squash all the Lines.
        /// </summary>
        /// <param name="addBlock"></param>
        /// <returns></returns>
        public static string Mail_SquashAddressLines(string[] addBlock)
        {
            string theAddress = "";

            for (int i = 0; i < addBlock.Length; i++)
            {
                string thisLIne = ALCS_DataShift.WhenNull(addBlock[i], "");
                if (thisLIne != "")
                {
                    theAddress += ((theAddress == "") ? ("") : (Environment.NewLine)) + thisLIne;
                }
            }

            // Return the address ...
            return theAddress;
        }


        /// <summary>
        /// Build last line.
        /// </summary>
        /// <param name="inSuburb"></param>
        /// <param name="inState"></param>
        /// <param name="inPostcode"></param>
        /// <returns></returns>
        public static string Mail_SquashLastLine(string inSuburb, string inState, string inPostcode)
        {
            string lastLine = "";

            lastLine = ((inSuburb + " " + inState).Trim() + " " + inPostcode).Trim();

            // Return the address ...
            return lastLine;
        }

        /// <summary>
        /// Just Squash the name.
        /// </summary>
        /// <param name="inTitle"></param>
        /// <param name="inGiven"></param>
        /// <param name="inSurname"></param>
        /// <returns></returns>
        public static string Mail_SquashName(string inTitle, string inGiven, string inSurname)
        {
            string fullName = "";

            fullName = ((inTitle + " " + inGiven).Trim() + " " + inSurname).Trim();

            // Return the address ...
            return fullName;
        }

        /// <summary>
        /// Another Version
        /// </summary>
        /// <param name="inGiven"></param>
        /// <param name="inSurname"></param>
        /// <returns></returns>
        public static string Mail_SquashName(string inGiven, string inSurname)
        {
            return Mail_SquashName("", inGiven, inSurname);
        }

    }

}
