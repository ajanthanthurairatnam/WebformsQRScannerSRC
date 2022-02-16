using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;   

namespace ALCS_Library.ALCS_Basics
{
	/// <summary>
	/// Summary description for ALCS_Printer.
	/// </summary>
	public class ALCS_Printer
	{
		/// <summary>
		/// Just print the content of the hash.
		/// </summary>
		/// <param name="thePage"></param>
		/// <param name="ht"></param>
		public static void PrintHashTable(Page thePage, Hashtable ht)
		{
			ICollection ic =   ht.Keys;
			int idx = 1;
			
			foreach(Object o in ic)
			{
				thePage.Response.Write(idx.ToString() + ". " +    o.ToString()  + "  :  " + ht[o.ToString()].ToString()  + "<BR>");

				idx++;
			}
		}

		/// <summary>
		/// Just print the content of the array list.
		/// </summary>
		/// <param name="thePage"></param>
		/// <param name="ht"></param>
		public static void PrintArrayList(Page thePage, ArrayList al)
		{
			int idx		= 1;
			
			for(idx=0; idx < al.Count ; idx++) 
			{
				thePage.Response.Write( Convert.ToString(idx+1) + ". " +   al[idx].ToString() + "<BR>");
			}
		}
	}
}
