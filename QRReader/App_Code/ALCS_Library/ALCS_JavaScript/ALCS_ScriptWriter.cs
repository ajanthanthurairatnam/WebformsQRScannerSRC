using System;
using System.Collections;
using System.Collections.Generic; 
using System.Web.UI.WebControls; 
using System.Web.UI;

using ALCS_Library.ALCS_Format;

 
namespace ALCS_Library.ALCS_JavaScript
{

	////////////////////////////////////////////////////
	/// Enumeration reguired for script writting.
	////////////////////////////////////////////////////
	#region "Where to release the script"
	public enum ALCS_ScriptRegion
	{
		Start, 
		End
	}
	#endregion 

	////////////////////////////////////////////////////
	/// Script writter reguired for script writting.
	////////////////////////////////////////////////////
	
	#region "Write a javascript to the client"

	/// <summary>
	/// Summary description for ALCS_ScriptWriter.
	/// </summary>
	public class ALCS_ScriptWriter
	{
		private string theKey; 
		private string theScript;

		/// <summary>
		///  assign a string to a vaiable.
		/// </summary>
		/// <param name="scriptKey"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		public ALCS_ScriptWriter(string scriptKey, string varName, string varValue) 
		{ 
			theKey = scriptKey; 
			theScript += "var " + varName + "='" + varValue + "';\n"; 
		} 

		/// <summary> 
		/// assign a decimal to a variable.
		/// </summary>
		/// <param name="scriptKey"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		public ALCS_ScriptWriter(string scriptKey, string varName, decimal varValue) 
		{ 
			theKey = scriptKey;
            theScript += "var " + varName + "=" + Convert.ToString(varValue) + ";\n"; 
		} 

		/// <summary>
		/// Assign an integer to a variable.
		/// </summary>
		/// <param name="scriptKey"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		public ALCS_ScriptWriter(string scriptKey, string varName, int varValue) 
		{ 
			theKey = scriptKey;
            theScript += "var " + varName + "=" + Convert.ToString(varValue) + ";\n"; 
		} 

		/// <summary>
		/// Assign an boolean to a variable.
		/// </summary>
		/// <param name="scriptKey"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		public ALCS_ScriptWriter(string scriptKey, string varName, bool varValue) 
		{ 
			theKey = scriptKey; 
			//theScript += "\n<script language=JavaScript>\n"; 
			//theScript += "<!--\n";
			if(varValue)
			{
				theScript += "var " + varName + "=true;\n"; 
			}
			else
			{
				theScript += "var " + varName + "=false;\n"; 
			}
			
			//theScript += "//-->\n"; 
			//theScript += "</script>\n"; 
		} 

		/// <summary>
		/// build a script around the Javascript code.
		/// </summary>
		/// <param name="theKeyVal"></param>
		/// <param name="theScriptVal"></param>
		public ALCS_ScriptWriter(string theKeyVal, string theScriptVal) 
		{ 
			theKey = theKeyVal; 
			theScript += theScriptVal + ";\n"; 
		} 

		/// <summary>
		/// Relesase a hash of values to Javascript.
		/// </summary>
		/// <param name="theHash"></param>
		/// <param name="theHashName"></param>
		/// <param name="theKeyVal"></param>
		public ALCS_ScriptWriter(Hashtable theHash, string theHashName, string theKeyVal) 
		{ 
			theKey = theKeyVal; 
			theScript += theHashName + "= new Array();\n"; 
			foreach (DictionaryEntry item in theHash) 
			{ 
				theScript += theHashName + "[\"" + (string)(item.Key) + "\"]=\"" + (string)(item.Value) + "\";\n"; 
			} 
		} 

		/// <summary>
		/// inject the script in the page .... 
		/// </summary>
		/// <param name="thePage"></param>
		/// <param name="thePosition"></param>
		public void RegisterScript(Page thePage, ALCS_ScriptRegion thePosition) 
		{
            ClientScriptManager csm = thePage.ClientScript;  
            
            if ((thePosition == ALCS_ScriptRegion.Start)) 
			{ 
				if ((!(csm.IsStartupScriptRegistered(this.theKey)))) 
				{
                    csm.RegisterStartupScript(thePage.GetType(), this.theKey, this.theScript, true); 
				} 
			} 
			else if ((thePosition == ALCS_ScriptRegion.End)) 
			{
                if ((!(csm.IsClientScriptBlockRegistered(this.theKey)))) 
				{
                    csm.RegisterClientScriptBlock(thePage.GetType(), this.theKey, this.theScript, true); 
				} 
			} 
			else 
			{
                if ((!(csm.IsClientScriptBlockRegistered(this.theKey)))) 
				{
                    csm.RegisterClientScriptBlock(thePage.GetType(), this.theKey, this.theScript, true); 
				} 
			} 
		} 
	} 

	#endregion 


    #region "Inject Data to the client ..."

    public static class ScriptInjector
    {
        /// <summary>
        /// Expose the variable in Javascript contetx.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public static void ExposeVars(Page thePage, string varName, string varValue)
        {
            string theScript = "var " + varName + " = '" + ALCS_StringRewrite.BrowserFriendly_jsAsVarApproved(varValue) + "';\n";
            InjectScript(thePage, varName, theScript);   
        }

        public static void ExposeVars(Page thePage, string varName, int varValue)
        {
            string theScript = "var " + varName + " = " + varValue + ";\n";
            InjectScript(thePage, varName, theScript);   
        }

        public static void ExposeVars(Page thePage, string varName, decimal varValue)
        {
            string theScript = "var " + varName + " = " + varValue + ";\n";
            InjectScript(thePage, varName, theScript);
        }

        public static void ExposeVars(Page thePage, string varName, bool varValue)
        {
            string theScript = "";

            if (varValue)
            {
                theScript = "var " + varName + " = " + "true;\n";
            }
            else
            {
                theScript = "var " + varName + " = " + "false;\n";
            }
            InjectScript(thePage, varName, theScript);   
        }

        /// <summary>
        /// Inject the script in the client stream.
        /// </summary>
        /// <param name="thePage"></param>
        /// <param name="theScript"></param>
        public static void InjectScript(Page thePage, string theKey,  string theScript)
        {
            ClientScriptManager csm = thePage.ClientScript;

            if (!csm.IsStartupScriptRegistered(theKey))
            {
                csm.RegisterStartupScript(thePage.GetType(), theKey, theScript, true);   
            }
        }

        /// <summary>
        /// Inject the script (function Call) in the client stream.
        /// </summary>
        /// <param name="thePage"></param>
        /// <param name="theScript"></param>
        public static void InjectFunctionCall(Page thePage, string funcKey, string funcScript)
        {
            ClientScriptManager csm = thePage.ClientScript;

            if (!csm.IsStartupScriptRegistered(funcKey))
            {
                funcScript = funcScript.Trim();

                if (!funcScript.EndsWith(";"))
                {
                    funcScript += ";";
                }

                csm.RegisterStartupScript(thePage.GetType(), funcKey, funcScript + "\n", true);
            }
        }



        #region "Declare Variable in Javascript contetx ...."

        /// <summary>
        /// Expose the form to the client .....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSVar(Control ctrl)
        {
            //return "var " + ctrl.ID + " = " + "document.all('" + ctrl.ClientID + "');\n";
            // Cross Browser Version 
            return "var " + ctrl.ID + " = " + "document.all ? document.all[\"" + ctrl.ClientID + "\"]: document.getElementById(\"" + ctrl.ClientID + "\") ;\n";
        }

        /// <summary>
        /// Expose an image button.
        /// </summary>
        /// <param name="ctrlName"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string DeclareJSVar(String ctrlName, ImageButton img)
        {
            return "var " + ctrlName + " = " + "document.all ? document.all[\"" + img.ClientID + "\"]: document.getElementById(\"" + img.ClientID + "\") ;\n";
        }


        /// <summary>
        /// Expose the form to the client ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSVar(RadioButtonList rl)
        {
            int idx;
            string varStr = "";
            string arrayStr = "var " + rl.ID + " = Array(";

            for (idx = 0; idx < rl.Items.Count; idx++)
            {
                string eID = rl.ID + "_" + Convert.ToString(idx);
                string ecID = rl.ClientID + "_" + Convert.ToString(idx);
                string ePointer = "document.getElementById('" + ecID  + "')";

                varStr += "var " + eID+ " = " + ePointer + ";\n";
                //arrayStr += (arrayStr.EndsWith("(") ? (thisElem) : ("," + thisElem));
                arrayStr += (arrayStr.EndsWith("(") ? (eID) : ("," + eID));
            }

            // return
            return varStr + arrayStr + ");\n";
        }

        /// <summary>
        /// Expose the form to the client .....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSVar(Page thePage, RadioButtonList rl)
        {
            return "var " + rl.ID + " = " + "document.all ? document.all[\"" + rl.ClientID + "\"]: document." + thePage.Form.ClientID + "." + rl.ClientID + ";\n";
        }
        
        /// <summary>
        /// Expose the the variable to javascript ....
        /// </summary>
        /// <param name="leadStr"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSVar(string leadStr, Control ctrl)
        {
            return "var " + ctrl.ID + " = " + leadStr + ctrl.ClientID + ";\n";
        }

        /// <summary>
        /// Expose the the control pointers to javascript ....
        /// </summary>
        /// <param name="ctrls"></param>
        /// <returns></returns>
        public static string DeclareJSVar(Control[] ctrls)
        {
            string jsStr = "";

            for (int idx = 0; idx < ctrls.Length; idx++)
            {
                jsStr += DeclareJSVar(ctrls[idx]);
            }

            return jsStr;
        }

        /// <summary>
        /// Expose the the control pointers to javascript ....
        /// </summary>
        /// <param name="leadStr"></param>
        /// <param name="ctrls"></param>
        /// <returns></returns>
        public static string DeclareJSVar(string leadStr, ref List<WebControl> ctrls)
        {
            string jsStr = "";

            foreach (WebControl ctrl in ctrls)
            {
                jsStr += DeclareJSVar(leadStr, ctrl);
            }

            return jsStr;
        }

        /// <summary>
        /// Expose the the control pointers to javascript ....
        /// </summary>
        /// <param name="leadStr"></param>
        /// <param name="ctrls"></param>
        /// <returns></returns>
        public static string DeclareJSVar(ref List<WebControl> ctrls)
        {
            string jsStr = "";

            foreach (WebControl ctrl in ctrls)
            {
                jsStr += DeclareJSVar(ctrl);
            }

            return jsStr;
        }

        /// <summary>
        /// Expose the the variable to javascript ....
        /// </summary>
        /// <param name="leadStr"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSIFrame(Control ctrl)
        {
            return "var " + ctrl.ID + " = " + "document.frames[\"" + ctrl.ClientID + "\"];\n";
        }


        /// <summary>
        /// Expose the the variable to javascript ....
        /// </summary>
        /// <param name="leadStr"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSIFrame_Strict(Control ctrl)
        {
            return "var " + ctrl.ID + " = document.getElementById(\"" + ctrl.ClientID + "\");\n";
        }      
        /// <summary>
        /// Expose the form as a vriable .....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareJSForm(Control ctrl)
        {
            return DeclareJSForm(ctrl, ctrl.ID);
        }

        /// <summary>
        /// Expose the form as a variable ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="frmName"></param>
        /// <returns></returns>
        public static string DeclareJSForm(Control ctrl, string formName)
        {
            return "var " + formName + " = " + "document.forms[\"" + ctrl.ClientID + "\"];\n";
        }

        /// <summary>
        /// Expose the variable in Javascript contetx.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public static string DeclareJSVar(string varName, string varValue)
        {
            string theScript = "var " + varName + " = '" + ALCS_StringRewrite.BrowserFriendly_jsAsVarApproved(varValue) + "';\n";

            //exit
            return theScript; 
        }

        public static string DeclareJSVar(string varName, int varValue)
        {
            string theScript = "var " + varName + " = " + varValue + " - 0;\n";

            //exit
            return theScript; 
        }

        public static string DeclareJSVar(string varName, decimal varValue)
        {
            return DeclareJSVar(varName, varValue, 2);
        }

        public static string DeclareJSVar(string varName, decimal varValue, int decPlaces)
        {
            string theScript = "";

            if (decPlaces == 2)
            {
                theScript = "var " + varName + " = " + varValue.ToString("#0.00") + " - 0;\n";
            }
            else
            {
                theScript = "var " + varName + " = " + varValue + " - 0;\n";
            }

            //exit
            return theScript;
        }


        public static string DeclareJSVar(string varName, bool varValue)
        {
            string theScript = "";

            if (varValue)
            {
                theScript = "var " + varName + " = " + "true;\n";
            }
            else
            {
                theScript = "var " + varName + " = " + "false;\n";
            }
            
            //exit 
            return theScript; 
        }


        public static string DeclareElemById(Control ctrl)
        {
            //return "var " + ctrl.ID + " = " + "document.all('" + ctrl.ClientID + "');\n";
            // Cross Browser Version 
            return "var " + ctrl.ID + " = document.getElementById(\"" + ctrl.ClientID + "\") ;\n";
        }

        /// <summary>
        /// Declare Controls.
        /// </summary>
        /// <param name="ctrlName"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareControl(String ctrlName, Control ctrl)
        {
            // Cross Browser Version 
            return "var " + ctrlName + " = document.getElementById(\"" + ctrl.ClientID + "\") ;\n";
        }

        /// <summary>
        /// Web Control.
        /// </summary>
        /// <param name="ctrlName"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string DeclareControl(String ctrlName, WebControl ctrl)
        {
            // Cross Browser Version 
            return "var " + ctrlName + " = document.getElementById(\"" + ctrl.ClientID + "\") ;\n";
        }
        
        #endregion 
    }
    
    #endregion 
}
