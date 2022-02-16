using System;

using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Data;

namespace ALCS_Library.ALCS_Data
{

	public enum PositionType
	{
		Absolute,
		Relative,
		Floating
	}



	#region	 "Build the navigation list bar"
	/// <summary>
	/// Build the navigation links bar 
	/// </summary>

	public class ALCS_NavigationLinks
	{
		private int currentPage; 
		private int pagesCount; 
		private int recsCount; 
		private PositionType posType = PositionType.Floating; 
		private int posTop = 0; 
		private int linksCount = 5; 
		private string linksTrigger = "GotoPage"; 
		private string barClass = "mainHolder"; 
		private string linkClassN = "plinkN"; 
		private string linkClassA = "plinkA"; 
		private string npClassA = "nextPrevA"; 
		private string npClassD = "nextPrevD"; 
		private string moreClass = "moreLinks"; 
		private string sepClass = "sepBar"; 
		private char sepChar = '|'; 
		private bool sepAdd = true;
		private string leftText="&nbsp;";
		private string rightText="&nbsp;";

		/// <summary>
		/// Constructor 0
		/// </summary>
		/// <param name="theRecCount"></param>
		/// <param name="thePageSize"></param>
		/// <param name="theCurrentPage"></param>
		public ALCS_NavigationLinks(int theRecCount, int thePageSize, int theCurrentPage) 
		{ 
			this.currentPage = theCurrentPage; 
			this.recsCount = theRecCount; 
			this.pagesCount = GetPageCount(theRecCount, thePageSize); 
		} 
		
		/// <summary>
		/// Constructor 1
		/// </summary>
		/// <param name="theRecCount"></param>
		/// <param name="thePageSize"></param>
		/// <param name="theCurrentPage"></param>
		/// <param name="thePosType"></param>
		/// <param name="thePosTop"></param>
		public ALCS_NavigationLinks(int theRecCount, int thePageSize, int theCurrentPage, PositionType thePosType, int thePosTop) 
		{ 
			this.currentPage = theCurrentPage; 
			this.recsCount = theRecCount; 
			this.pagesCount = GetPageCount(theRecCount, thePageSize); 
			this.posType = thePosType; 
			this.posTop = thePosTop; 
		} 

		/// <summary>
		/// Constructor 2
		/// </summary>
		/// <param name="theRecCount"></param>
		/// <param name="thePageSize"></param>
		/// <param name="theCurrentPage"></param>
		/// <param name="theLinksCount"></param>
		/// <param name="theLinksTrigger"></param>
		public ALCS_NavigationLinks(int theRecCount, int thePageSize, int theCurrentPage, int theLinksCount, string theLinksTrigger) 
		{ 
			this.currentPage = theCurrentPage; 
			this.recsCount = theRecCount; 
			this.pagesCount = GetPageCount(theRecCount, thePageSize); 
			this.linksCount = theLinksCount; 
			this.linksTrigger = theLinksTrigger; 
		} 

		/// <summary>
		/// Constructor 3
		/// </summary>
		/// <param name="theRecCount"></param>
		/// <param name="thePageSize"></param>
		/// <param name="theCurrentPage"></param>
		/// <param name="thePosType"></param>
		/// <param name="thePosTop"></param>
		/// <param name="theLinksCount"></param>
		/// <param name="theLinksTrigger"></param>
		public ALCS_NavigationLinks(int theRecCount, int thePageSize, int theCurrentPage, PositionType thePosType, int thePosTop, int theLinksCount, string theLinksTrigger) 
		{ 
			this.currentPage = theCurrentPage; 
			this.recsCount = theRecCount; 
			this.pagesCount = GetPageCount(theRecCount, thePageSize); 
			this.posType = thePosType; 
			this.posTop = thePosTop; 
			this.linksCount = theLinksCount; 
			this.linksTrigger = theLinksTrigger; 
		} 


		/// <summary>
		/// Work out the pages count ....
		/// </summary>
		/// <param name="theRecCount"></param>
		/// <param name="?"></param>
		/// <returns></returns>
		private int GetPageCount(int theRecCount, int thePageSize)
		{
			return ((int)(Math.Ceiling(Convert.ToDouble(theRecCount) / Convert.ToDouble(thePageSize))));
		}

		/// <summary>
		/// More setting required?
		/// </summary>
		/// <param name="theBarClass"></param>
		/// <param name="linkClass"></param>
		/// <param name="linkClassA"></param>
		public void SetColorScheme(string theBarClass, string linkClass, string linkClassA) 
		{ 
			this.barClass = theBarClass; 
			this.linkClassN = linkClass; 
			this.linkClassA = linkClassA; 
		} 

		/// <summary>
		/// Set the Surreounding Texts
		/// </summary>
		/// <param name="leftSide"></param>
		/// <param name="rightSide"></param>
		public void SetSurroundingText(string leftSide, string rightSide)
		{
			this.leftText = ALCS_DataShift.WhenNull(leftSide, ALCS_Utils.ExtractHTMLSymbol(specialHTML._Space));
			this.rightText = ALCS_DataShift.WhenNull(rightSide, ALCS_Utils.ExtractHTMLSymbol(specialHTML._Space));  
		}


		/// <summary>
		/// Return what we used to seperate the links.
		/// </summary>
		/// <returns></returns>
		private string GetSepStr() 
		{ 
			if ((this.sepAdd == true)) 
			{
                return "<span class=\"" + this.sepClass + "\" >" + this.sepChar + "</span>"; 
			} 
			else 
			{ 
				return ""; 
			} 
		} 

		/// <summary>
		/// This Function will create a Navigation Status Bar in a table and return the html String for it
		/// Next and Prev to go bacward or Forward.
		/// The Function assumes the existence of a clientside function gotoPage(pageno) which given a 
		/// page no will submit information to let the resulting Recordset advance to the given page
		/// The Parameters are :
		/// - pCurrentpage the current page to be viewed.
		/// - pPagesCount how many pages are there in your record set.
		/// - pRecordsCount number of records in yourrecord set.
		/// - pColor the backgound color of the Navigation bar.
		/// - pposType :  1 absolute ; 0 relative ; other no positioning you need to suply a container
		///               for the returned Bar
		///
		/// - pposTop  :  The top postioning for the returned element.			
		/// - plinksCount how many links do you want to set between the next and previous pages
		/// </summary>
		/// <returns></returns>
		public string BuildNavigationPanel() 
		{ 
			string leftLink; 
			string rightLink; 
			string middleLinks; 
			int inext; 
			int iprev; 
			int lowerLimit; 
			int upperLimit; 
			int idx; 
			string idxStr; 
			System.Text.StringBuilder sbr = new System.Text.StringBuilder("", 64000); 
			if ((this.linksCount <= 0)) 
			{ 
				lowerLimit = 0; 
				upperLimit = -1; 
			} 
			else if ((this.currentPage <= this.linksCount)) 
			{ 
				lowerLimit = 1; 
				upperLimit = Math.Min(this.linksCount, this.pagesCount); 
			} 
			else 
			{ 
				lowerLimit = this.currentPage - ((this.currentPage - 1) % this.linksCount); 
				upperLimit = Math.Min(lowerLimit + this.linksCount - 1, this.pagesCount); 
			} 
			inext = Math.Min(this.currentPage + 1, this.pagesCount); 
			iprev = Math.Max(this.currentPage - 1, 1); 
			if ((this.posType == PositionType.Absolute)) 
			{ 
				sbr.Append("<table class=" + barClass + " cellspacing=0 align=center cellpadding=0 width=100% border=0 style=\"position:absolute;top:" + (Convert.ToString(this.posTop)) + "px;width:\"100%\"\">"); 
			} 
			else if ((this.posType == PositionType.Relative)) 
			{ 
				sbr.Append("<table class=" + barClass + " cellspacing=0 align=center cellpadding=0 width=100% border=0 style=\"position:relative;top:" + (Convert.ToString(this.posTop)) + "px;width:\"100%\"\">"); 
			} 
			else 
			{ 
				sbr.Append("<table class=" + barClass + " cellspacing=0 align=center cellpadding=0 width=100% border=0 style=\"width:\"100%\"\">"); 
			} 
			if ((iprev != this.currentPage) & (this.recsCount != 0)) 
			{
                leftLink = "<span class=\"" + npClassA + "\" onclick=\"" + this.linksTrigger + "(" + (Convert.ToString(iprev)) + ")\">Prev</span>"; 
			} 
			else 
			{
                leftLink = "<span class=\"" + npClassD + "\" >Prev</span>"; 
			} 
			middleLinks = ""; 
			if ((this.linksCount > 0)) 
			{ 
				if ((this.recsCount != 0)) 
				{ 
					for (idx = lowerLimit; idx <= upperLimit; idx++) 
					{ 
						idxStr = (Convert.ToString(idx)); 
						if ((idx == this.currentPage)) 
						{
                            middleLinks += GetSepStr() + "<span class=\"" + linkClassA + "\" >" + idxStr + "</span>"; 
						} 
						else 
						{
                            middleLinks += GetSepStr() + "<span class=\"" + linkClassN + "\" onclick=\"" + this.linksTrigger + "(" + idxStr + ")\">" + idxStr + "</span>"; 
						} 
					} 
					if ((upperLimit < this.pagesCount)) 
					{
                        middleLinks += GetSepStr() + "<span class=\"" + moreClass + "\" onclick=\"" + this.linksTrigger + "(" + (Convert.ToString(upperLimit + 1)) + ")\">(More ...)</span>"; 
					} 
					else 
					{ 
						middleLinks += GetSepStr(); 
					} 
				} 
				else 
				{
                    middleLinks = GetSepStr() + "&nbsp;<span class=\"" + linkClassA + "\" >1</span>" + GetSepStr(); 
				} 
			} 
			if ((inext != this.currentPage) & (this.recsCount != 0)) 
			{
                rightLink = "<span class=\"" + npClassA + "\" onclick=\"" + this.linksTrigger + "(" + (Convert.ToString(inext)) + ")\">Next</span>"; 
			} 
			else 
			{
                rightLink = "<span class=\"" + npClassD + "\" >Next</span>"; 
			}
            sbr.Append("<tr valign=middle ><td width=25% class=\"leftHolder\" style=\"PADDING-LEFT:10px;\">" + this.leftText + "</td><td valign=middle align=center width=50%>" + leftLink + middleLinks + rightLink + "</td><td class=\"rightHolder\" width=25%>" + this.rightText + "</td></tr></table>"); 
			return sbr.ToString(); 
		}

		/// <summary>
		/// Get and set the bar class
		/// </summary>
		public string BarClass
		{
			get
			{
				return this.barClass;
			}
			set
			{
				this.barClass = value;
			}
		} 
	}

	#endregion 
}
