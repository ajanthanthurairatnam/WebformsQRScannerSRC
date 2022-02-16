using System;
using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Format;  

namespace ALCS_Library.ALCS_WWW
{

	#region "Enumerators required By Triggers"


	public enum emailLayout 
	{ 
		TABLE, 
		SPAN 
	} 
	
	public enum wwwLayout 
	{ 
		TABLE, 
		SPAN 
	}
 
	public enum wwwTarget 
	{ 
		_blank, 
		_parent, 
		_self, 
		_top, 
		_none 
	}
 
	public enum ImageAt 
	{ 
		Front, 
		Over, 
		After, 
		None 
	}
 
	public enum triggerType 
	{ 
		http, 
		javascript 
	}

	public enum MouseAction 
	{ 
		MouseOver, 
		MouseClick, 
		MouseOut 
	}

	#endregion

	#region " Help building all types of links and triggers"
	
	/// <summary>
	/// Summary description for ALCS_Triggers.
	/// </summary>
	public class ALCS_Triggers
	{

		/// <summary>
		/// Format a URL by adding the http where needed.
		/// </summary>
		/// <param name="theURL"></param>
		/// <returns></returns>
		public static string FormatURL(string theURL) 
		{ 
			string newURL;

			// Change the UTL
			if ((!(theURL.StartsWith("http")))) 
			{ 
				newURL = "http:\\\\" + theURL; 
			}
			else
			{
				newURL = theURL;
			}

			// Exit 
			return newURL; 
		}



		/// <summary>
		/// Build an email trigger 
		/// </summary>
		/// <param name="theEmail"></param>
		/// <param name="theMask"></param>
		/// <returns></returns>
		public static string BuildEmailLink(string theEmail, string theMask)
		{
			return BuildEmailLink(theEmail, theMask, "", ImageAt.None, emailLayout.SPAN);  	
		}


		/// <summary>
		/// Build an email trigger and optionally associate it with an image.
		/// </summary>
		/// <param name="theEmail"></param>
		/// <param name="theMask"></param>
		/// <param name="theImageURL"></param>
		/// <param name="theImageAt"></param>
		/// <param name="theMode"></param>
		/// <returns></returns>
		public static string BuildEmailLink(string theEmail, string theMask, string theImageURL, ImageAt theImageAt, emailLayout theMode) 
		{ 
			string theEmailLink; 
			string theImageStr; 
			string theEmailStr; 
			string theMaskStr; 
			string LeadingStr; 
			string trailingStr;
				
			if ((theImageURL == "")) 
			{ 
				theImageStr = ""; 
				LeadingStr = ""; 
				trailingStr = ""; 
				theMaskStr = ALCS_DataShift.WhenNull(theMask, theEmail); 
			} 
			else 
			{ 
				theImageStr = "<IMG class=emlImage border=0 src=\"" + theImageURL + "\">"; 
				if ((theImageAt == ImageAt.None)) 
				{ 
					theMaskStr = ALCS_DataShift.WhenNull(theMask, theEmail); 
					LeadingStr = ""; 
					trailingStr = ""; 
				} 
				else if ((theImageAt == ImageAt.Front)) 
				{ 
					theMaskStr = ALCS_DataShift.WhenNull(theMask, theEmail); 
					LeadingStr = theImageStr; 
					trailingStr = ""; 
				} 
				else if ((theImageAt == ImageAt.After)) 
				{ 
					theMaskStr = ALCS_DataShift.WhenNull(theMask, theEmail); 
					LeadingStr = ""; 
					trailingStr = theImageStr; 
				} 
				else if ((theImageAt == ImageAt.Over)) 
				{ 
					theMaskStr = theImageStr; 
					LeadingStr = ""; 
					trailingStr = ""; 
				}
				else
				{
					theMaskStr = ALCS_DataShift.WhenNull(theMask, theEmail); 
					LeadingStr = ""; 
					trailingStr = ""; 
				}
			} 
			theEmailStr = "<A class=fakelink title=\"Click to email this company\" HREF=\"mailto:" + theEmail + "\">" + theMaskStr + "</A>"; 
			if ((theMode == emailLayout.TABLE)) 
			{ 
				theEmailLink = ""; 
				theEmailLink += "<table cellSpacing=\"0\" cellPadding=\"0\" >"; 
				theEmailLink += "<tr>"; 
				if ((LeadingStr != "")) 
				{ 
					theEmailLink += "<td>" + LeadingStr + "</td>"; 
				} 
				theEmailLink += "<td width=100% style=\"PADDING-LEFT:5px;PADDING-RIGHT:5px;\">" + theEmailStr + "</td>"; 
				if ((trailingStr != "")) 
				{ 
					theEmailLink += "<td>" + trailingStr + "</td>"; 
				} 
				theEmailLink += "</tr>"; 
				theEmailLink += "</table>"; 
			} 
			else 
			{ 
				theEmailLink = LeadingStr + theEmailStr + trailingStr; 
				theEmailLink = theEmailLink.Trim(); 
			} 
			return theEmailLink; 
		}


		/// <summary>
		///	  Build the WebLink  Link 
		/// </summary>
		/// <param name="theWebURL"></param>
		/// <param name="theWebMask"></param>
		/// <param name="theWebTarget"></param>
		/// <param name="theTriggerType"></param>
		/// <param name="theMode"></param>
		/// <param name="theImageURL"></param>
		/// <param name="theImageAt"></param>
		/// <returns></returns>
		public static string BuildWebLink(string theWebURL, string theWebMask, wwwTarget theWebTarget, triggerType theTriggerType, wwwLayout theMode, string theImageURL, ImageAt theImageAt) 
		{ 
			string theWebLink; 
			string theImageStr; 
			string theWebStr; 
			//string theTarget; 
			string theURLStr; 
			string theWebMaskStr; 
			string LeadingStr; 
			string trailingStr; 
			if ((theImageURL == "")) 
			{ 
				theImageStr = ""; 
				LeadingStr = ""; 
				trailingStr = ""; 
				theWebMaskStr = ALCS_DataShift.WhenNull(theWebMask, theWebURL); 
			} 
			else 
			{ 
				theImageStr = "<IMG class=emlImage border=0 src=\"" + theImageURL + "\"></IMG>"; 
				if ((theImageAt == ImageAt.None)) 
				{ 
					theWebMaskStr = ALCS_DataShift.WhenNull(theWebMask, theWebURL); 
					LeadingStr = ""; 
					trailingStr = ""; 
				} 
				else if ((theImageAt == ImageAt.Front)) 
				{ 
					theWebMaskStr = ALCS_DataShift.WhenNull(theWebMask, theWebURL); 
					LeadingStr = theImageStr; 
					trailingStr = ""; 
				} 
				else if ((theImageAt == ImageAt.After)) 
				{ 
					theWebMaskStr = ALCS_DataShift.WhenNull(theWebMask, theWebURL); 
					LeadingStr = ""; 
					trailingStr = theImageStr; 
				} 
				else if ((theImageAt == ImageAt.Over)) 
				{ 
					theWebMaskStr = theImageStr; 
					LeadingStr = ""; 
					trailingStr = ""; 
				} 
				else
				{
					theWebMaskStr = ALCS_DataShift.WhenNull(theWebMask, theWebURL); 
					LeadingStr = ""; 
					trailingStr = ""; 
				}
			} 
			if ((theTriggerType == triggerType.http)) 
			{ 
				theURLStr = FormatURL(theWebURL); 
			} 
			else if ((theTriggerType == triggerType.javascript)) 
			{ 
				theURLStr = "javascript:" + theWebURL + ";"; 
			} 
			else 
			{ 
				theURLStr = theWebURL; 
			} 
			if ((theWebTarget != wwwTarget._none)) 
			{ 
				theWebStr = "<A class=fakelink target=\"" + theWebTarget.ToString() + "\" HREF=\"" + theURLStr + "\">" + theWebMaskStr + "</A>"; 
			} 
			else 
			{ 
				theWebStr = "<A class=fakelink onmouseover=\"'';return true;\" onmouseout=\"'';return true;\" HREF=\"" + theURLStr + "\">" + theWebMaskStr + "</A>"; 
			} 
			if ((theMode == wwwLayout.TABLE)) 
			{ 
				theWebLink = ""; 
				theWebLink += "<table cellSpacing=\"0\" cellPadding=\"0\" >"; 
				theWebLink += "<tr>"; 
				if ((LeadingStr != "")) 
				{ 
					theWebLink += "<td>" + LeadingStr + "</td>"; 
				} 
				theWebLink += "<td width=100% style=\"PADDING-LEFT:5px;PADDING-RIGHT:5px;\">" + theWebStr + "</td>"; 
				if ((trailingStr != "")) 
				{ 
					theWebLink += "<td>" + trailingStr + "</td>"; 
				} 
				theWebLink += "</tr>"; 
				theWebLink += "</table>"; 
			} 
			else 
			{ 
				theWebLink = LeadingStr + theWebStr + trailingStr; 
				theWebLink = theWebLink.Trim(); 
			} 
			return theWebLink; 
		}


		/// <summary>
		/// This function will behave similar to the dotedStr except that the user will have to click 
		/// the ... indicator to view the full text of the string.
		///  
		///  - if the string is longer than the allowed length than it will chancked on the nearest 
		///  space and a thre dots ... marker will be appanded to it. Clicking the marker will 
		///  launch a popup that shows a popup that will display the full text of the screen.
		///  
		///  - Please note that clicking the marker will always call a client side function called 
		///  showetc() which will typically make the appropriate call to aladnToolTip() 
		///  or any other function you choose.
		///  
		///  - This Doco is OLD and may not be valid Jamil.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="inMask"></param>
		/// <param name="inLength"></param>
		/// <param name="inAction"></param>
		/// <returns></returns>
		public static string StringTail(string inStr, string inMask, int inLength, MouseAction inAction, string scrTitle) 
		{ 
			string fullStr; 
			string showStr; 
			string wiredStr; 
			bool toChunck; 
			string theClass; 

			// Initialize ...
			fullStr	= ALCS_DataShift.WhenNull(inStr, ""); 
			inMask	= ALCS_DataShift.WhenNull(inMask, "");
 
			if (inMask != "") 
			{ 
				showStr = ""; 
				toChunck = true; 
				theClass = "fakelink"; 
			} 
			else 
			{ 
				inMask = "&#133;"; 
				theClass = "threeBlueBullet"; 
				if ((fullStr.Length <= inLength)) 
				{ 
					showStr = fullStr; 
					toChunck = false; 
				} 
				else 
				{ 
					toChunck = true; 
					showStr = fullStr.Substring(0, inLength).Trim(); 
					if ((showStr.LastIndexOf(" ") > 0)) 
					{ 
						showStr = fullStr.Substring(0, showStr.LastIndexOf(" ")).Trim(); 
					} 
				} 
				if ((showStr == "")) 
				{ 
					showStr = "Details"; 
				} 
			} 
			if ((toChunck == false)) 
			{ 
				wiredStr = showStr; 
			} 
			else 
			{ 
				fullStr = ALCS_StringRewrite.PreserveHTMLFormat(fullStr, WorkMode.Relaxed); 
				if ((inAction == MouseAction.MouseClick)) 
				{
                    wiredStr = showStr + " <span title=\"Click to view the full text\" class=\"" + theClass + "\" onclick=\"showetc('" + ALCS_StringRewrite.BrowserFriendly_jsApproved(fullStr) + "','" + scrTitle + "')\">" + inMask + "</span>"; 
				} 
				else if ((inAction == MouseAction.MouseOver)) 
				{
                    wiredStr = showStr + " <span title=\"Click to view the full text\" class=\"" + theClass + "\" onmouseout=\"showetc('X')\" onmouseover=\"showetc('" + ALCS_StringRewrite.BrowserFriendly_jsApproved(fullStr) + "','" + scrTitle + "')\">" + inMask + "</span>"; 
				} 
				else 
				{ 
					wiredStr = fullStr; 
				} 
			} 
			return wiredStr; 
		}


        /// <summary>
        /// Just chop the string and show as a title if need be.
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inMask"></param>
        /// <param name="inLength"></param>
        /// <returns></returns>
        public static string StringTail(string inStr, int inLength, string inMask)
        {
            string fullStr;
            string showStr;
            string wiredStr;
            bool toChunck;

            // Initialize ...
            fullStr = ALCS_DataShift.WhenNull(inStr, "");
            inMask = ALCS_DataShift.WhenNull(inMask, "");

            // Set the mask ....
            string dotClass = "threeBlueBullet";

            // set the length of the string ...
            if (fullStr.Length <= inLength)
            {
                showStr = fullStr;
                toChunck = false;
            }
            else
            {
                toChunck = true;
                showStr = fullStr.Substring(0, inLength).Trim();
                if ((showStr.LastIndexOf(" ") > 0))
                {
                    showStr = fullStr.Substring(0, showStr.LastIndexOf(" ")).Trim();
                }
            }

            // Set the string 
            if(toChunck)
            {
                wiredStr = showStr + " <span title=\"" + ALCS_StringRewrite.BrowserFriendly_HtmlApproved(fullStr) + "\" class=\"" + dotClass + "\" >" + inMask + "</span>";
            }
            else
            {
                wiredStr = showStr;
            }

            // return the string ...
            return wiredStr;
        }

        /// <summary>
        /// Set the string as a title to the string .....
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inLength"></param>
        /// <returns></returns>
        public static string StringTail(string inStr, int inLength)
        {
            return StringTail(inStr, inLength, "&#133;");  
        }

    
    }

	#endregion
}
