using System;
using System.Collections; 

namespace ALCS_Library.ALCS_Data
{
	/// <summary>
	/// Summary description for ALCS_CVS.
	/// </summary>
	public class ALCS_CVS
	{

		/// <summary>
		/// duplicate all double quote.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static string QualifyCSV(string inStr)
		{
			string retStr = ALCS_DataShift.WhenNull(inStr,"");  

			if(retStr != "")
			{
				retStr = retStr.Replace("\"","\"\""); 
			}

			// return 
			return retStr;
		}

		/// <summary>
		/// Wrap the string in a CSV acceptable form .....
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		public static string WrapCSV(string inStr, string altStr )
		{
			string retStr = ALCS_DataShift.WhenNull(inStr,""); 

			if(retStr == "")
			{
				retStr = altStr; 
			}
			else
			{
				retStr = "'" + retStr + "'";
			}

			// return 
			return retStr;
		}

		/// <summary>
		/// Build a CSV line from the passed array list....
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static string MakeCSVLine(ArrayList arr)
		{
			return MakeCSVLine(arr, arr.Count); 
		}

		/// <summary>
		/// Build a CSV line from the passed array list....
		/// </summary>
		/// <param name="arr"></param>
		/// <param name="csvLen"></param>
		/// <returns></returns>
		public static string MakeCSVLine(ArrayList arr, int csvLen)
		{
			string csvLine = "\"";
			int idx;
			string sepStr;

			// Resize the upper limit if needed
			for(idx=0; idx < csvLen; idx++)
			{
				if(idx == csvLen-1)
				{
					sepStr = "\"";
				}
				else
				{
					sepStr = "\",\"";
				}

				// Add the cell 
				if(idx < arr.Count)
				{
					csvLine += QualifyCSV(ALCS_DataShift.WhenNull(arr[idx],"")) + sepStr;
				}
				else
				{
					csvLine += sepStr; 
				}
			}
			
			// return 
			return csvLine;
		}

		/// <summary>
		/// Build a CSV line from the passed array list....
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static string MakeCSVLine(string[] arr)
		{
			return MakeCSVLine(arr, arr.Length); 
		}

		/// <summary>
		/// Build a CSV line from the passed array ....
		/// </summary>
		/// <param name="arr"></param>
		/// <param name="csvLen"></param>
		/// <returns></returns>
		public static string MakeCSVLine(string[] arr, int csvLen)
		{
			string csvLine = "\"";
			int idx;
			string sepStr;

			// Resize the upper limit if needed
			for(idx=0; idx < csvLen; idx++)
			{
				if(idx == csvLen-1)
				{
					sepStr = "\"";
				}
				else
				{
					sepStr = "\",\"";
				}

				// Add the cell 
				if(idx < arr.Length)
				{
					csvLine += QualifyCSV(ALCS_DataShift.WhenNull(arr[idx],"")) + sepStr;
				}
				else
				{
					csvLine += sepStr; 
				}
			}
			
			// return 
			return csvLine;
		}


		/// <summary>
		/// Add  header Line .......
		/// </summary>
		/// <param name="sepChar"></param>
		/// <param name="headLen"></param>
		/// <returns></returns>
		public static string MakeCSVDivider(char sepChar, int headLen, int csvLen)
		{
			return MakeCSVDivider(sepChar, headLen, csvLen, 1);   
		}
		
		/// <summary>
		/// Add  header Line .......
		/// </summary>
		/// <param name="sepChar"></param>
		/// <param name="headLen"></param>
		/// <returns></returns>
		public static string MakeCSVDivider(char sepChar, int headLen, int csvLen, int repeat)
		{
			string repStr = "".PadRight(headLen, sepChar);  
			
			return MakeCSVDivider(repStr, csvLen, repeat);
		}

		/// <summary>
		/// Add  header Line .......
		/// </summary>
		/// <param name="inHead"></param>
		/// <param name="csvLen"></param>
		/// <returns></returns>
		public static string MakeCSVDivider(string inHead, int csvLen)
		{
			return MakeCSVDivider(inHead, csvLen, 1);
		}
		
		
		/// <summary>
		/// Add  header Line .......
		/// </summary>
		/// <param name="inHead"></param>
		/// <param name="csvLen"></param>
		/// <returns></returns>
		public static string MakeCSVDivider(string inHead, int csvLen, int repeat)
		{
			string[] divArr = new string[csvLen];
			string divBlock = "";

			// Load Array ..
			for(int idx=0; idx < csvLen; idx++)
			{
				divArr[idx] = inHead;
			}

			// return 
			string divLine =  MakeCSVLine(divArr, csvLen);  
 
			// repeat the lines
			for(int idx=0; idx < repeat; idx++)
			{
				if(idx == 0)
				{
					divBlock = divLine;
				}
				else
				{
					divBlock += "\n" + divLine;
				}
			}

			// return 
			return divBlock;
		}
	}
}
