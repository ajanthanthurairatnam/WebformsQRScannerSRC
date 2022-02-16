using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;   

namespace ALCS_Library.ALCS_WWW.ALCS_Messages
{
	/// <summary>
	/// Summary description for ALCS_TopMessage.
	/// </summary>
	public class ALCS_TopMessage
	{
		public bool healthy; 
		private string ErrMessage; 
		private string messTitle; 
		private string messBody; 
		private string messImage = ""; 
		private string messColor = "#FF0000"; 

		/// <summary>
		/// Constructor.
		/// </summary>
		public ALCS_TopMessage() 
		{ 
			healthy = true; 
			ErrMessage = ""; 
			messTitle = ""; 
			messBody = ""; 
		} 

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="messTitleStr"></param>
		/// <param name="messBodyStr"></param>
		public ALCS_TopMessage(string messTitleStr, string messBodyStr) 
		{ 
			healthy = true; 
			ErrMessage = ""; 
			messTitle = messTitleStr; 
			messBody = messBodyStr; 
		} 

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="messTitleStr"></param>
		/// <param name="messBodyStr"></param>
		/// <param name="messImageStr"></param>
		public ALCS_TopMessage(string messTitleStr, string messBodyStr, string messImageStr) 
		{ 
			healthy = true; 
			ErrMessage = ""; 
			messTitle = messTitleStr; 
			messBody = messBodyStr; 
			messImage = messImageStr; 
		} 

		/// <summary>
		///	 constructor
		/// </summary>
		/// <param name="messTitleStr"></param>
		/// <param name="messBodyStr"></param>
		/// <param name="messImageStr"></param>
		/// <param name="messColorStr"></param>
		public ALCS_TopMessage(string messTitleStr, string messBodyStr, string messImageStr, string messColorStr) 
		{ 
			healthy = true; 
			ErrMessage = ""; 
			messTitle = messTitleStr; 
			messBody = messBodyStr; 
			messImage = messImageStr; 
			messColor = messColorStr; 
		} 

		/// <summary>
		/// Reset the table width to the desired width ....
		/// </summary>
		public void ResetTableWidth(Page thePage, string theWidth)
		{
			HtmlTable tblMessage = (HtmlTable)(thePage.FindControl("xtblMessage"));
 
			if (tblMessage == null)
			{ 
				thePage.Response.Write("The static message include file \"message.inc\" was altered and/or not included"); 
				return; 
			} 
			else
			{
				tblMessage.Width = theWidth;
			}
		}


		/// <summary>
		/// Finnaly put the message out.
		/// </summary>
		/// <param name="thePage"></param>
		public void WriteMessage(Page thePage) 
		{ 
			HtmlTableCell cellTitle = new HtmlTableCell(); 
			HtmlTableCell cellBody = new HtmlTableCell(); 
			HtmlTable tblMessage = new HtmlTable(); 
			HtmlImage leftImage = new HtmlImage(); 
			bool showImage = false; 
			tblMessage = ((HtmlTable)(thePage.FindControl("xtblMessage"))); 
			cellTitle = ((HtmlTableCell)(thePage.FindControl("xmessHeader"))); 
			cellBody = ((HtmlTableCell)(thePage.FindControl("xmessBody"))); 
			leftImage = ((HtmlImage)(thePage.FindControl("xmessLeftImage"))); 
			if ((tblMessage == null) | (tblMessage == null) | (tblMessage == null) | (leftImage == null)) 
			{ 
				thePage.Response.Write("The static message include file \"message.inc\" was altered and/or not included"); 
				return; 
			} 
			cellTitle.InnerHtml = messTitle; 
			cellBody.InnerHtml = messBody; 
			cellBody.Style.Add("COLOR", messColor); 
			if ((messImage == "")) 
			{ 
				leftImage.Style.Add("DISPLAY", "none"); 
			} 
			else 
			{ 
				if ((messImage == "STOP")) 
				{ 
					leftImage.Src = "~/images/stop.gif"; 
					showImage = true; 
				} 
				else if ((messImage == "INFO")) 
				{ 
					leftImage.Src = "~/images/info_2.gif"; 
					showImage = true; 
				} 
				else if ((messImage == "QUEST")) 
				{ 
					leftImage.Src = "~/images/QuestionMark_2.gif"; 
					showImage = true; 
				} 
				if ((showImage == true)) 
				{ 
					leftImage.Style.Add("DISPLAY", ""); 
				} 
			} 
			tblMessage.Style.Add("display", ""); 
		} 

		/// <summary>
		/// Wipe out the message.
		/// </summary>
		/// <param name="thePage"></param>
		public void WipeMessage(Page thePage) 
		{ 
			HtmlTableCell cellTitle = new HtmlTableCell(); 
			HtmlTableCell cellBody = new HtmlTableCell(); 
			HtmlTable tblMessage = new HtmlTable(); 
			HtmlImage leftImage = new HtmlImage(); 
			tblMessage = ((HtmlTable)(thePage.FindControl("xtblMessage"))); 
			cellTitle = ((HtmlTableCell)(thePage.FindControl("xmessHeader"))); 
			cellBody = ((HtmlTableCell)(thePage.FindControl("xmessBody"))); 
			leftImage = ((HtmlImage)(thePage.FindControl("xmessLeftImage"))); 
			if ((tblMessage == null) | (tblMessage == null) | (tblMessage == null) | (leftImage == null)) 
			{ 
				thePage.Response.Write("The static message include file \"message.inc\" was altered and/or not included"); 
				return; 
			} 
			cellTitle.InnerHtml = ""; 
			cellBody.InnerHtml = ""; 
			leftImage.Src = "~/images/stop.gif"; 
			leftImage.Style.Add("display", "none"); 
			tblMessage.Style.Add("display", "none"); 
		}


		/// <summary>
		/// Expose the state of the object.
		/// </summary>
		public bool isHealthy
		{
			get
			{
				return this.healthy; 
			}
		} 


		/// <summary>
		/// Expose the error message.
		/// </summary>
		public string Memo
		{
			get
			{
				return this.ErrMessage; 
			}
		} 



	}
}
