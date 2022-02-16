using System;
using System.IO;
using System.Web;
using System.Text;
using System.Web.UI;  
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls; 
using System.Collections;
using System.Collections.Generic;

using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_Data;
 
namespace ALCS_Library.ALCS_WWW
{

    #region "Enum value ..."

    // Visibility
    public enum Ctrl_Visibility
    {
        Visible,
        Hidden
    }

    public enum Ctrl_Display
    {
        Display,
        Hide
    }


    // Data Type ....
    public enum Ctrl_InputType
    {
        Integer,
        Decimal,
        Dollar,
        AnyChar,
        LowerChar,
        UpperChar,
        AnyAlpha,
        LowerAlpha,
        UpperAlpha,
        Date,
        Time,
        Email,
        PhoneFax,
        STDPhone,
        MOBPhone,
        DigitalCode,
        PostCode,
        NoWhiteSpaces,
        ABN,
        ACN,
        Custom
    }

    // Text Alignment 
    public enum Ctrl_TextAlign
    {
        Left,
        Center,
        Right,
    }

    // Text Alignment 
    public enum Ctrl_TextTransform
    {
        UppreCase,
        LowerCase,
        Capitalise
    }

    public enum Ctrl_Event
    {
        OnClick,
        OnChange,
        OnBlur,
        OnFocus,
        OnKeyPress,
        OnKeyDown,
        OnKeyup,
    }
    
    #endregion 

    /// <summary>
	/// Summary description for ALCS_WWWTools.
	/// </summary>
	public static class ALCS_WWWTools
    {
        const string aTextClass = "textBox";
        const string pTextClass = "textBoxRO";

        const string aAreaClass = "textArea";
        const string pAreaClass = "textAreaRO";

        const string aDropDownClass = "dropDown";
        const string pDropDownClass = "dropDownRO";

        const string aCheckBoxClass = "checkBox";
        const string pCheckBoxClass = "checkBoxRO";

        #region "LPD New Tools "

        #region "Sift Input ..."

        /// <summary>
        /// Sift Input ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="filter"></param>
        public static void ClientSiftInput(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType filter)
        {
            if (ctrl.ReadOnly)
            {
                return;
            }


            if (filter == Ctrl_InputType.Integer)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'n');");
            }
            else if (filter == Ctrl_InputType.Decimal)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'d2');");
            }
            else if (filter == Ctrl_InputType.Dollar)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'d2');");
            }
            else if (filter == Ctrl_InputType.Date)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'dt');");
            }
            else if ((filter == Ctrl_InputType.PhoneFax) || (filter == Ctrl_InputType.STDPhone) || (filter == Ctrl_InputType.MOBPhone))
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'pf');");
            }
            else if (filter == Ctrl_InputType.DigitalCode)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'dc');");
            }
            else if (filter == Ctrl_InputType.AnyChar)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'c');");
            }
            else if (filter == Ctrl_InputType.LowerChar)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'lc');");
            }
            else if (filter == Ctrl_InputType.UpperChar)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'uc');");
            }
            else if (filter == Ctrl_InputType.AnyAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'a');");
            }
            else if (filter == Ctrl_InputType.LowerAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'la');");
            }
            else if (filter == Ctrl_InputType.UpperAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'ua');");
            }
            else if (filter == Ctrl_InputType.Email)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'pr');");
            }
            else if (filter == Ctrl_InputType.NoWhiteSpaces)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'pr');");
            }
            else if (filter == Ctrl_InputType.Time)
            {
                ctrl.Attributes.Add("onkeypress", "return SiftInput(event, this,'tm');");
            }
        }


        #endregion

        #region "Add Control Trigger"


        /// <summary>
        /// Add Trigger.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="trigName"></param>
        /// <param name="trigFunction"></param>
        /// <param name="propMode"></param>
        public static void ClientAddTrigger(WebControl ctrl, string trigName, string trigFunction, AddPropMode addMode)
        {
            string curValue = ALCS_DataShift.WhenNull(ctrl.Attributes[trigName], "");
            string newValue;

            if (addMode == AddPropMode.Append)
            {
                if (curValue == "")
                {
                    newValue = trigFunction;
                }
                else if ((!IsSameProperty(curValue, trigFunction)) && (!IsIncluded(curValue, trigFunction)))
                {
                    newValue = trigFunction + (trigFunction.EndsWith(";") ? "" : ";") + curValue;
                    ctrl.Attributes.Remove(trigName);
                }
                else
                {
                    // Do Nothing.
                    newValue = curValue;
                }
            }
            else
            {
                newValue = trigFunction;
            }

            ctrl.Attributes.Add(trigName, newValue);
        }

        /// <summary>
        /// Trigger Addition.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="trigName"></param>
        /// <param name="trigFunction"></param>
        public static void ClientAddTrigger(WebControl ctrl, string trigName, string trigFunction)
        {
            ClientAddTrigger(ctrl, trigName, trigFunction, AddPropMode.Append);
        }


        /// <summary>
        /// Pass as Events.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ce"></param>
        /// <param name="trigFunction"></param>
        /// <param name="addMode"></param>
        public static void ClientAddTrigger(WebControl ctrl, Ctrl_Event ce, string trigFunction, AddPropMode addMode)
        {
            ClientAddTrigger(ctrl, ce.ToString().ToLower(), trigFunction, addMode);
        }

        /// <summary>
        /// Another Version.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ce"></param>
        /// <param name="trigFunction"></param>
        public static void ClientAddTrigger(WebControl ctrl, Ctrl_Event ce, string trigFunction)
        {
            ClientAddTrigger(ctrl, ce.ToString().ToLower(), trigFunction);
        }


        #endregion



        #endregion 




        #region "Visibility Management Of Controls ..."


        public static void SetControlVisibility(System.Web.UI.Control ctrl, Ctrl_Visibility ctrlVis)
        {
            if (ctrlVis == Ctrl_Visibility.Visible)
            {
                ctrl.Visible = true;
            }
            else if (ctrlVis == Ctrl_Visibility.Hidden)
            {
                ctrl.Visible = false;
            }
        }

        /// <summary>
        /// Set the display mode for the control ....
        /// </summary>
        /// <param name="wctrl"></param>
        /// <param name="ctrlDis"></param>
        public static void SetControlDisplay(WebControl wctrl, Ctrl_Display ctrlDis)
        {
            wctrl.Attributes.CssStyle.Remove("display");

            if (ctrlDis == Ctrl_Display.Display)
            {
                wctrl.Attributes.CssStyle.Add("display","''");
            }
            else if (ctrlDis == Ctrl_Display.Hide)
            {
                wctrl.Attributes.CssStyle.Add("display", "none");
            }
        }

        /// <summary>
        /// Set the display mode for the control ....
        /// </summary>
        /// <param name="wctrl"></param>
        /// <param name="ctrlDis"></param>
        public static void SetControlDisplay(HtmlControl htmlCtrl, Ctrl_Display ctrlDis)
        {
            htmlCtrl.Attributes.CssStyle.Remove("display");

            if (ctrlDis == Ctrl_Display.Display)
            {
                htmlCtrl.Attributes.CssStyle.Add("display", "''");
            }
            else if (ctrlDis == Ctrl_Display.Hide)
            {
                htmlCtrl.Attributes.CssStyle.Add("display", "none");
            }
        }


        /// <summary>
        /// Set the visibility mode for the control ....
        /// </summary>
        /// <param name="wctrl"></param>
        /// <param name="ctrlDis"></param>
        public static void SetControlVisibility(WebControl wctrl, Ctrl_Visibility ctrlVis)
        {
            wctrl.Attributes.CssStyle.Remove("visibility");

            if (ctrlVis == Ctrl_Visibility.Visible)
            {
                wctrl.Attributes.CssStyle.Add("visibility", "visible");
            }
            else if (ctrlVis == Ctrl_Visibility.Hidden)
            {
                wctrl.Attributes.CssStyle.Add("visibility", "hidden");
            }
        }

        /// <summary>
        /// a special case the check box.
        /// </summary>
        /// <param name="wctrl"></param>
        /// <param name="ctrlVis"></param>

        public static void SetControlVisibility(CheckBox wctrl, Ctrl_Visibility ctrlVis)
        {
            wctrl.InputAttributes.CssStyle.Remove("visibility");
            wctrl.LabelAttributes.CssStyle.Remove("visibility");

            if (ctrlVis == Ctrl_Visibility.Visible)
            {
                wctrl.InputAttributes.CssStyle.Add("visibility", "visible");
                wctrl.LabelAttributes.CssStyle.Add("visibility", "visible");
            }
            else if (ctrlVis == Ctrl_Visibility.Hidden)
            {
                wctrl.InputAttributes.CssStyle.Add("visibility", "hidden");
                wctrl.LabelAttributes.CssStyle.Add("visibility", "hidden");
            }
        }


        /// <summary>
        /// Set the visibility mode for the control ....
        /// </summary>
        /// <param name="wctrl"></param>
        /// <param name="ctrlDis"></param>
        public static void SetControlVisibility(HtmlControl ctrl, Ctrl_Visibility ctrlVis)
        {
            ctrl.Style.Remove("visibility");

            if (ctrlVis == Ctrl_Visibility.Visible)
            {
                ctrl.Style.Add("visibility", "visible");
            }
            else if (ctrlVis == Ctrl_Visibility.Hidden)
            {
                ctrl.Style.Add("visibility", "hidden");
            }
        }

        #endregion 

        #region "Render Control as HTML strings ..."

        /// <summary>
        /// Simply render the Web Control as a HTML string.
        /// </summary>
        /// <param name="wc"></param>
        /// <returns></returns>
        public static string RenderWebControl(WebControl wc)
        {
            string htmlString = "";

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            wc.RenderControl(htw);
            htmlString = sb.ToString();

            // Return 
            return htmlString;
        }

        /// <summary>
        /// HTMl Control
        /// </summary>
        /// <param name="wc"></param>
        /// <returns></returns>
        public static string RenderWebControl(HtmlControl wc)
        {
            string htmlString = "";

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            wc.RenderControl(htw);
            htmlString = sb.ToString();

            // Return 
            return htmlString;
        }


        #endregion 

        #region "Locking Management Of Controls ..."


        #region "Locking Control ..."

        /// <summary>
        /// Lock the Text Box control 
        /// </summary>
        /// <param name="ctrl"></param>
        public static void LockControl(System.Web.UI.WebControls.TextBox ctrl)
        {
            string chosenClass;

            if (ctrl.TextMode == TextBoxMode.MultiLine)
            {
                chosenClass = pAreaClass; 
            }
            else
            {
                chosenClass = pTextClass; 
            }

            // Loc the control 
            LockControl(ctrl, chosenClass);  
        }
      
        /// <summary>
        /// Lock the Text Box control 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(System.Web.UI.WebControls.TextBox ctrl, string pClass)
        {
            ctrl.ReadOnly = true;
            ctrl.TabIndex = -1;
            ctrl.CssClass = pClass;
        }

        /// <summary>
        /// Lock the drop down list ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void LockControl(System.Web.UI.WebControls.DropDownList ctrl)
        {
            LockControl(ctrl, pDropDownClass);   
        }

        /// <summary>
        /// Lock the drop down list ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(System.Web.UI.WebControls.DropDownList ctrl, string pClass)
        {
            ctrl.Enabled = false;
            ctrl.CssClass = pClass;
        }

        /// <summary>
        /// Lock a Button ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(System.Web.UI.WebControls.Button ctrl, string pClass)
        {
            ctrl.Enabled = false;
        }
        
        /// <summary>
        /// Lock the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        public static void LockControl(System.Web.UI.WebControls.CheckBox ctrl)
        {
            LockControl(ctrl, pCheckBoxClass);  
        }

        /// <summary>
        /// Lock the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(System.Web.UI.WebControls.CheckBox ctrl, string pClass)
        {
            ctrl.Enabled = false;
            ctrl.CssClass = pClass;
        }

        /// <summary>
        /// Lock a generaic web control ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void LockControl(WebControl ctrl, string pClass)
        {
            TextBox tb = new TextBox();
            DropDownList ddl = new DropDownList();
            CheckBox cb = new CheckBox();

            if (ctrl.GetType() == tb.GetType())
            {
                LockControl((TextBox)ctrl, pClass);
            }
            else if(ctrl.GetType() == ddl.GetType())
            {
                LockControl((DropDownList)ctrl, pClass);
            }
            else if (ctrl.GetType() == cb.GetType())
            {
                LockControl((CheckBox)ctrl, pClass);
            }
        }

        /// <summary>
        /// Lock a generaic web control ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void LockControl(WebControl ctrl)
        {
            TextBox tb = new TextBox();
            DropDownList ddl = new DropDownList();
            CheckBox cb = new CheckBox();

            if (ctrl.GetType() == tb.GetType())
            {
                LockControl((TextBox)ctrl);
            }
            else if (ctrl.GetType() == ddl.GetType())
            {
                LockControl((DropDownList)ctrl);
            }
            else if (ctrl.GetType() == cb.GetType())
            {
                LockControl((CheckBox)ctrl);
            }
        }

        /// <summary>
        /// Lock a list of controls ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(List<WebControl> ctrls, string pClass)
        {
            foreach (WebControl ctrl in ctrls)
            {
                LockControl(ctrl, pClass);
            }
        }

        /// <summary>
        /// Lock a list of controls ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void LockControl(List<WebControl> ctrls)
        {
            foreach (WebControl ctrl in ctrls)
            {
                LockControl(ctrl);
            }
        }
        #endregion 

        #region "Unlock the Control ..."

        /// <summary>
        /// Unloack the Text Box ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="tabIndex"></param>
       
        public static void UnlockControl(System.Web.UI.WebControls.TextBox ctrl, short tabIndex)
        {
            string chosenClass;

            if (ctrl.TextMode == TextBoxMode.MultiLine)
            {
                chosenClass = aAreaClass;
            }
            else
            {
                chosenClass = aTextClass;
            }

            // Loc the control 
            UnlockControl(ctrl, tabIndex, chosenClass);
        }

        /// <summary>
        /// Unlock the text Box ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        /// <param name="tabIndex"></param>
        public static void UnlockControl(System.Web.UI.WebControls.TextBox ctrl, short tabIndex, string pClass)
        {
            ctrl.ReadOnly = false;
            ctrl.CssClass = pClass;
            ctrl.TabIndex = tabIndex;
        }

        /// <summary>
        /// Unlock the drop down list ...
        /// </summary>
        /// <param name="ctrl"></param>
        public static void UnlockControl(System.Web.UI.WebControls.DropDownList ctrl, short tabIndex)
        {
            UnlockControl(ctrl, tabIndex, aDropDownClass);
        }

        /// <summary>
        /// Unlock the drop down list ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void UnlockControl(System.Web.UI.WebControls.DropDownList ctrl, short tabIndex, string aClass)
        {
            ctrl.Enabled = true;
            ctrl.CssClass = aClass;
            ctrl.TabIndex = tabIndex; 
        }


        /// <summary>
        /// Lock the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        public static void UnlockControl(System.Web.UI.WebControls.CheckBox ctrl, short tabIndex)
        {
            UnlockControl(ctrl, tabIndex, aCheckBoxClass);
        }

        /// <summary>
        /// Lock the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void UnlockControl(System.Web.UI.WebControls.CheckBox ctrl, short tabIndex, string aClass)
        {
            ctrl.Enabled = true;
            ctrl.CssClass = aClass;
            ctrl.TabIndex = tabIndex; 
        }

        /// <summary>
        /// Lock a generaic web control ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void UnlockControl(WebControl ctrl, short tabIndex, string pClass)
        {
            TextBox tb = new TextBox();
            DropDownList ddl = new DropDownList();
            CheckBox cb = new CheckBox();

            if (ctrl.GetType() == tb.GetType())
            {
                UnlockControl((TextBox)ctrl, tabIndex, pClass);
            }
            else if (ctrl.GetType() == ddl.GetType())
            {
                UnlockControl((DropDownList)ctrl, tabIndex, pClass);
            }
            else if (ctrl.GetType() == cb.GetType())
            {
                UnlockControl((CheckBox)ctrl, tabIndex, pClass);
            }
        }

        /// <summary>
        /// Lock a generaic web control ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void UnlockControl(WebControl ctrl, short tabIndex)
        {
            TextBox tb = new TextBox();
            DropDownList ddl = new DropDownList();
            CheckBox cb = new CheckBox();

            if (ctrl.GetType() == tb.GetType())
            {
                UnlockControl((TextBox)ctrl, tabIndex);
            }
            else if (ctrl.GetType() == ddl.GetType())
            {
                UnlockControl((DropDownList)ctrl, tabIndex);
            }
            else if (ctrl.GetType() == cb.GetType())
            {
                UnlockControl((CheckBox)ctrl, tabIndex);
            }
        }

        /// <summary>
        /// Lock a list of controls ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void UnlockControl(List<WebControl> ctrls, short tabIndex, string pClass)
        {
            foreach (WebControl ctrl in ctrls)
            {
                UnlockControl(ctrl, tabIndex, pClass);
            }
        }

        /// <summary>
        /// Lock a list of controls ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pClass"></param>
        public static void UnlockControl(List<WebControl> ctrls, short tabIndex)
        {
            foreach (WebControl ctrl in ctrls)
            {
                UnlockControl(ctrl, tabIndex);
            }
        }

        #endregion 

        #region "Clear/Reset The Controls Control ..."

        /// <summary>
        /// Assign the default value for the text box.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="defValue"></param>
        public static void ResetControl(System.Web.UI.WebControls.TextBox ctrl, string defValue)
        {
            ctrl.Text = defValue;
        }

        /// <summary>
        /// Another version to assign "" to the text box.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ResetControl(System.Web.UI.WebControls.TextBox ctrl)
        {
            ResetControl(ctrl, "");
        }

        /// <summary>
        /// Selected the specified index.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="selIndex"></param>
        public static void ResetControl(System.Web.UI.WebControls.DropDownList ctrl, int selIndex)
        {
            if (ctrl.Items.Count > 0)
            {
                ctrl.SelectedIndex = selIndex;
            }
        }

        /// <summary>
        /// Selected the specified value when found.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="selIndex"></param>
        public static void ResetControl(System.Web.UI.WebControls.DropDownList ctrl, string selValue)
        {
            SelectOption(ref ctrl, selValue);
        }

        /// <summary>
        /// Selected the specified value when found.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ResetControl(System.Web.UI.WebControls.DropDownList ctrl)
        {
            ResetControl(ctrl, 0);
        }

        /// <summary>
        /// Reset the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="selValue"></param>
        public static void ResetControl(System.Web.UI.WebControls.CheckBox ctrl, bool selValue)
        {
            ctrl.Checked = selValue;
        }

        /// <summary>
        /// Reset the check box ...
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ResetControl(System.Web.UI.WebControls.CheckBox ctrl)
        {
            ResetControl(ctrl, false);
        }

        /// <summary>
        /// Reset generaic web control ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ResetControl(WebControl ctrl)
        {
            TextBox tb = new TextBox();
            DropDownList ddl = new DropDownList();
            CheckBox cb = new CheckBox();

            if (ctrl.GetType() == tb.GetType())
            {
                ResetControl((TextBox)ctrl);
            }
            else if (ctrl.GetType() == ddl.GetType())
            {
                ResetControl((DropDownList)ctrl);
            }
            else if (ctrl.GetType() == cb.GetType())
            {
                ResetControl((CheckBox)ctrl);
            }
        }

        /// <summary>
        /// Lock a list of controls ...
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ResetControl(List<WebControl> ctrls)
        {
            foreach (WebControl ctrl in ctrls)
            {
                ResetControl(ctrl);
            }
        }
        #endregion 

        #endregion

        #region "Limiting User Input on a control ..."

        public static void ClientFilterInput(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType filter)
        {
            if (ctrl.ReadOnly)
            {
                return;
            }


            if (filter == Ctrl_InputType.Integer)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'n');");  
            }
            else if (filter == Ctrl_InputType.Decimal)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'d2');");
            }
            else if (filter == Ctrl_InputType.Dollar)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'d2');");
            }
            else if (filter == Ctrl_InputType.Date)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'dt');");
            }
            else if ((filter == Ctrl_InputType.PhoneFax) || (filter == Ctrl_InputType.STDPhone) || (filter == Ctrl_InputType.MOBPhone))
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'pf');");
            }
            else if (filter == Ctrl_InputType.DigitalCode)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'dc');");
            }
            else if (filter == Ctrl_InputType.AnyChar)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'c');");
            }
            else if (filter == Ctrl_InputType.LowerChar)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'lc');");
            }
            else if (filter == Ctrl_InputType.UpperChar)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'uc');");
            }
            else if (filter == Ctrl_InputType.AnyAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'a');");
            }
            else if (filter == Ctrl_InputType.LowerAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'la');");
            }
            else if (filter == Ctrl_InputType.UpperAlpha)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'ua');");
            }
            else if (filter == Ctrl_InputType.Email)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'pr');");
            }
            else if (filter == Ctrl_InputType.NoWhiteSpaces)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'pr');");
            }
            else if (filter == Ctrl_InputType.Time)
            {
                ctrl.Attributes.Add("onkeypress", "return filterInput(this,'tm');");
            }
        }

        /// <summary>
        /// Cause a client validation.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="filter"></param>
        public static void ClientValidateInput(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType filter)
        {
            if (filter == Ctrl_InputType.Integer)
            {
                ctrl.Attributes.Add("is", "n");
            }
            else if (filter == Ctrl_InputType.Decimal)
            {
                ctrl.Attributes.Add("is", "d2");
            }
            else if (filter == Ctrl_InputType.Dollar)
            {
                ctrl.Attributes.Add("is", "$");
            }
            else if (filter == Ctrl_InputType.Email)
            {
                ctrl.Attributes.Add("is", "e");
            }
            else if (filter == Ctrl_InputType.Date)
            {
                ctrl.Attributes.Add("is", "D");
            }
            else if (filter == Ctrl_InputType.STDPhone)
            {
                ctrl.Attributes.Add("is", "std");
            }
            else if (filter == Ctrl_InputType.MOBPhone)
            {
                ctrl.Attributes.Add("is", "mob");
            }
            else if (filter == Ctrl_InputType.PostCode)
            {
                ctrl.Attributes.Add("is", "pos");
            }
        }

        /// <summary>
        /// An Override ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="filter"></param>
        /// <param name="inFilter"></param>
        public static void ClientValidateInput(TextBox ctrl, Ctrl_InputType filter, string inFilter)
        {
            if (filter == Ctrl_InputType.Custom)
            {
                ctrl.Attributes.Add("is", inFilter);
            }
        }
        
        /// <summary>
        /// Limit the length odf the content of this text area.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="maxLen"></param>
        public static void ClientLimitInput(ref System.Web.UI.WebControls.TextBox ctrl, int maxLen)
        {
            if (ctrl.TextMode == TextBoxMode.MultiLine)
            {
                ctrl.Attributes.Add("onpaste", "CheckAgainst(this," + Convert.ToString(maxLen) + ",1);");
                ctrl.Attributes.Add("onkeyup", "CheckAgainst(this," + Convert.ToString(maxLen) + ",0);");
            }
            else
            {
                ctrl.MaxLength = maxLen; 
            }
        }

        #endregion 

       

        #region "Extend the control properties ..."

        public enum AddPropMode
        {
            Append,
            Replace
        }

        /// <summary>
        /// Add the property to a web control .....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        public static void ClientAddProperty(WebControl ctrl, string propName, string propValue)
        {
            ClientAddProperty(ctrl, propName, propValue, AddPropMode.Append);    
        }

        /// <summary>
        /// Add the property to a web control .....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <param name="propMode"></param>
        public static void ClientAddProperty(WebControl ctrl, string propName, string propValue, AddPropMode propMode)
        {
            string curValue = ALCS_DataShift.WhenNull(ctrl.Attributes[propName], "");
            string newValue;

            if(propMode == AddPropMode.Append)
            {
                if (curValue == "")
                {
                    newValue = propValue;
                }
                else if ((!IsSameProperty(curValue,propValue)) && (!IsIncluded(curValue, propValue)))
                {
                    newValue = propValue + (propValue.EndsWith(";")? "" : ";") + curValue;
                    ctrl.Attributes.Remove(propName);
                }
                else 
                {
                    // Do Nothing.
                    newValue = curValue;
                }
            }
            else
            {
                newValue = propValue;
            }

            ctrl.Attributes.Add(propName, newValue); 
        }

        /// <summary>
        /// Compare the properties and see if it is the same.
        /// </summary>
        /// <param name="prop1"></param>
        /// <param name="prop2"></param>
        /// <returns></returns>
        public static bool IsSameProperty(string prop1, string prop2)
        {
            prop1 = prop1.Replace(";", "");
            prop1 = prop1.Replace(",", "");
            prop1 = prop1.Replace(" ", "");
            prop1 = prop1.ToLower();

            prop2 = prop2.Replace(";", "");
            prop2 = prop2.Replace(",", "");
            prop2 = prop2.Replace(" ", "");
            prop2 = prop2.ToLower();

            return (prop1 == prop2);
        }

        /// <summary>
        /// Compare the properties and see if it is the same.
        /// </summary>
        /// <param name="prop1"></param>
        /// <param name="prop2"></param>
        /// <returns></returns>
        public static bool IsIncluded(string curProp, string newProp)
        {
            newProp = newProp.Replace(";", "");
            newProp = newProp.Replace(",", "");
            newProp = newProp.Replace(" ", "");
            newProp = newProp.ToLower();

            curProp = curProp.Replace(";", "");
            curProp = curProp.Replace(",", "");
            curProp = curProp.Replace(" ", "");
            curProp = curProp.ToLower();

            return (curProp.Contains(newProp));
        }

        /// <summary>
        /// Add a property to the list item.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        public static void ClientAddProperty(ListItem li, string propName, string propValue)
        {
            string curValue = ALCS_DataShift.WhenNull(li.Attributes[propName], "");
            string newValue;

            if (curValue == "")
            {
                newValue = propValue;
            }
            else
            {
                newValue = propValue + curValue;
                li.Attributes.Remove(propName);
            }

            li.Attributes.Add(propName, newValue);
        }

        /// <summary>
        /// Adding the must property ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientMakeRequired(System.Web.UI.WebControls.WebControl ctrl)
        {
            ClientAddProperty(ctrl, "must", "1", AddPropMode.Replace); 
        }

        /// <summary>
        /// Make a whole Array not Required ...
        /// </summary>
        /// <param name="ctrls"></param>
        public static void ClientMakeRequired(ref List<WebControl> ctrls)
        {
            foreach (WebControl ctrl in ctrls)
            {
                ClientMakeRequired(ctrl);
            }
        }

        /// <summary>
        /// Set the must to 0.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientMakeRequired_Not(System.Web.UI.WebControls.WebControl ctrl)
        {
            ClientAddProperty(ctrl, "must", "0", AddPropMode.Replace);
        }

        /// <summary>
        /// Make the whole control set required .....
        /// </summary>
        /// <param name="ctrls"></param>
        public static void ClientMakeRequired_Not(ref List<WebControl> ctrls)
        {
            foreach (WebControl ctrl in ctrls)
            {
                ClientMakeRequired_Not(ctrl);
            }
        }

        /// <summary>
        /// Seed a dirty mechanism ......
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(WebControl ctrl, Ctrl_Event ce, string dirtyTrigger)
        {
            ClientAddProperty(ctrl, ce.ToString().ToLower(), dirtyTrigger.Trim() + "(this);");
        }

        /// <summary>
        /// Seed a dirty mechanism ......
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ce"></param>
        public static void ClientSetDirty(WebControl ctrl, Ctrl_Event ce)
        {
            ClientSetDirty(ctrl, ce, "SetDirty");
        }

        /// <summary>
        /// Seed a dirty mechanism ......
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="dirtyTrigger"></param>
        public static void ClientSetDirty(WebControl ctrl, string dirtyTrigger)
        {
            ClientSetDirty(ctrl, Ctrl_Event.OnChange, dirtyTrigger);
        }


        /// <summary>
        /// Handle a checkboxlist 
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(CheckBoxList ctrl)
        {
            int idx = 0;

            for (idx = 0; idx < ctrl.Items.Count; idx++ )
            {
                ListItem li = ctrl.Items[idx];
                li.Attributes.Add("onclick", "SetDirty()");
            }
        }

        /// <summary>
        /// Seed a dirty mechanism ......
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(WebControl ctrl)
        {
            ClientSetDirty(ctrl, Ctrl_Event.OnChange, "SetDirty");
        }

        /// <summary>
        /// Seed a dirty mechanism ......
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(ref CheckBox ctrl)
        {
            ClientSetDirty(ctrl, Ctrl_Event.OnClick, "SetDirty");
        }

        
        /// <summary>
        /// And Another Version
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(ref List<WebControl> ctrls, Ctrl_Event ce, string dirtyTrigger)
        {
            foreach (WebControl ctrl in ctrls)
            {
                ClientSetDirty(ctrl, ce, dirtyTrigger);
            }
        }

        /// <summary>
        /// And Another Version
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientSetDirty(ref List<WebControl> ctrls, Ctrl_Event ce)
        {
            ClientSetDirty(ref ctrls, ce, "SetDirty");
        }

        /// <summary>
        /// And Another Version
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="dirtyTrigger"></param>
        public static void ClientSetDirty(ref List<WebControl> ctrls, string dirtyTrigger)
        {
            ClientSetDirty(ref ctrls, Ctrl_Event.OnChange, dirtyTrigger);
        }

        /// <summary>
        /// And Another Version
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="dirtyTrigger"></param>
        public static void ClientSetDirty(ref List<WebControl> ctrls)
        {
            ClientSetDirty(ref ctrls, Ctrl_Event.OnChange, "SetDirty");
        }

        /// <summary>
        /// And Another Version
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="dirtyTrigger"></param>
        public static void ClientSetDirty(ref RadioButtonList ctrl, Ctrl_Event ce, string dirtyTrigger)
        {
            ListItem li;
            int idx;

            // loop through and add the property to each item.
            for (idx = 0; idx < ctrl.Items.Count; idx++)
            {
                li = ctrl.Items[idx];
                ClientAddProperty(li, ce.ToString().ToLower(), dirtyTrigger.Trim() + "(this);");   
            }
        }

        #endregion 

        #region "Text Formatting and handling "

        /// <summary>
        /// Align the text in the text box to the desired alignment.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="txtAlign"></param>
        public static void ClientAlignText(System.Web.UI.WebControls.TextBox ctrl, Ctrl_TextAlign txtAlign)
        {
            if (txtAlign == Ctrl_TextAlign.Left)
            {
                ctrl.Style.Add("text-align", "left");
            }
            else if(txtAlign == Ctrl_TextAlign.Center)
            {
                ctrl.Style.Add("text-align", "center");
            }
            else if (txtAlign == Ctrl_TextAlign.Right)
            {
                ctrl.Style.Add("text-align", "right");
            }
        }

        public static void ClientFormatText(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType ctrlType)
        {
            if (ctrlType == Ctrl_InputType.ABN)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatABN_ACN(this, 'ABN');");
            }
            else if (ctrlType == Ctrl_InputType.ACN)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatABN_ACN(this, 'ACN');");
            }
            else if (ctrlType == Ctrl_InputType.PhoneFax)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatPhone(this, 'PHO');");
            }
            else if (ctrlType == Ctrl_InputType.STDPhone)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatPhone(this, 'STD');");
            }
            else if (ctrlType == Ctrl_InputType.MOBPhone)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatPhone(this, 'MOB');");
            }
            else if (ctrlType == Ctrl_InputType.Time)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatTime(this);");
            }
            else if (ctrlType == Ctrl_InputType.Dollar)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatDollar(this);");
            }
            else if (ctrlType == Ctrl_InputType.Date)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatDate(this);");
            }
            
        }


        /// <summary>
        /// More Formatting ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlType"></param>
        /// <param name="defValue"></param>
        public static void ClientFormatText(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType ctrlType, string defValue)
        {
            ClientFormatText(ctrl, ctrlType, defValue, "");
        }

        /// <summary>
        /// More Formatting ...
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlType"></param>
        /// <param name="defValue"></param>
        /// <param name="leadSign"></param>
        public static void ClientFormatText(System.Web.UI.WebControls.TextBox ctrl, Ctrl_InputType ctrlType, string defValue, string leadSign)
        {
            if (ctrlType == Ctrl_InputType.Dollar)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatDollar(this, '" + defValue +"','" + leadSign + "');");
            }
            else if (ctrlType == Ctrl_InputType.Decimal)
            {
                ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "ClientFormatDollar(this, '" + defValue + "','" + leadSign + "');");
            }
        }


        /// <summary>
        /// Transform the typed text to the desired capitalization.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="txtTransform"></param>
        public static void ClientTransformText(System.Web.UI.WebControls.TextBox ctrl, Ctrl_TextTransform txtTransform)
        {
            if (txtTransform == Ctrl_TextTransform.Capitalise)
            {
                ctrl.Style.Add("text-transform", "capitalize");
            }
            else if (txtTransform == Ctrl_TextTransform.LowerCase)
            {
                ctrl.Style.Add("text-transform", "lowercase");
            }
            else if (txtTransform == Ctrl_TextTransform.UppreCase)
            {
                ctrl.Style.Add("text-transform", "uppercase");
            }
        }

        /// <summary>
        /// Format the text in this text box on the client side ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="tf"></param>
        /// <param name="keepUpper"></param>
        public static void ClientChangeCase(System.Web.UI.WebControls.TextBox ctrl, TextFormat tf, bool keepUpper)
        {
            string ku = keepUpper.ToString().ToLower();
            string tfStr;

            ///////////////////////////////////////////////
            // Set upper Case .
            ///////////////////////////////////////////////
            if (tf == TextFormat.FirstCharUp)
            {
                tfStr = "FC";
            }
            else if (tf == TextFormat.UpperLower)
            {
                tfStr = "UL";
            }
            else if (tf == TextFormat.UpperCase)
            {
                tfStr = "U";
            }
            else if (tf == TextFormat.LowerCase)
            {
                tfStr = "L";
            }
            else
            {
                tfStr = "FC";
            }

            // Go Ahead and do it ...
            ALCS_WWWTools.ClientAddProperty(ctrl, "onblur", "TransformTextBox(this,'" + tfStr + "'," + ku + ")");
        }

        /// <summary>
        /// Override ....
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientChangeCase(System.Web.UI.WebControls.TextBox ctrl)
        {
            ClientChangeCase(ctrl, TextFormat.FirstCharUp, false);
        }

        /// <summary>
        /// Override ....
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="tf"></param>
        public static void ClientChangeCase(System.Web.UI.WebControls.TextBox ctrl, TextFormat tf)
        {
            ClientChangeCase(ctrl, tf, false);
        }

        /// <summary>
        /// Disable the exposition of the onmouseover.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientBlindMouse(System.Web.UI.WebControls.WebControl ctrl)
        {
            ctrl.Attributes.Add("onmouseover", "window.status='';return true;");
            ctrl.Attributes.Add("onmouseout", "window.status='';return true;");  
        }

        /// <summary>
        /// Disable the exposition of the onmouseover.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ClientBlindMouse(System.Web.UI.WebControls.WebControl[] ctrls)
        {
            for (int idx=0; idx < ctrls.Length; idx++)
            {
                ClientBlindMouse(ctrls[idx]);
            }
        }

        /// <summary>
        /// Change the text in the link as shown in the status bar
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void ClientDeceiveMouse(System.Web.UI.WebControls.WebControl ctrl, string text)
        {
            ctrl.Attributes.Add("onmouseover", "window.status='" + text + "';return true;");
            ctrl.Attributes.Add("onmouseout", "window.status='';return true;");  
        }

        /// <summary>
        /// Change the text in the link as shown in the status bar
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="text"></param>
        public static void ClientDeceiveMouse(System.Web.UI.WebControls.WebControl[] ctrls, string text)
        {
            for (int idx = 0; idx < ctrls.Length; idx++)
            {
                ClientDeceiveMouse(ctrls[idx], text);
            }
        }

        /// <summary>
        /// standarise the look of the check box ...
        /// </summary>
        /// <param name="cb"></param>
        public static void ClientAdjustCheckboxLook(CheckBox cb)
        {
            // Feel & Look.
            cb.InputAttributes.CssStyle.Add("height", "13px");
            cb.InputAttributes.CssStyle.Add("width", "13px");
            cb.InputAttributes.CssStyle.Add("vertical-align", "text-top");

            cb.LabelAttributes.CssStyle.Add("padding-left", "2px");
        }

        /// <summary>
        ///  standarise the look of the list of all checkboxes ...
        /// </summary>
        /// <param name="cbs"></param>
        public static void ClientAdjustCheckboxLook(CheckBox[] cbs)
        {
            for (int idx = 0; idx < cbs.Length; idx++)
            {
                ClientAdjustCheckboxLook(cbs[idx]);
            }
        }

        #endregion

        #region "Color Painting and other things "

        /// <summary>
        /// Create an alternate color feels fro the drop down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        public static void AlternateListBackground(ref DropDownList ddl, string color1, string color2)
        {
            int index= 0;

            foreach(ListItem li in ddl.Items)
            {
                ++index;

                // Color 
                if (index % 2 == 1)
                {
                    li.Attributes.CssStyle.Add("background-color", color1);
                }
                else
                {
                    li.Attributes.CssStyle.Add("background-color", color2);
                }
            }
        }

        /// <summary>
        /// Another Variation ....
        /// </summary>
        /// <param name="ddl"></param>
        public static void AlternateListBackground(ref DropDownList ddl)
        {
            AlternateListBackground(ref ddl, ALCS_Vault.ddBack1, ALCS_Vault.ddBack2);
        }
        

        #endregion 

        #region "Altering and or Preserving Content of Controls"

        /// <summary>
        /// Search the drop down list for the selected item and if found set to it.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="inVal"></param>
        public static void SelectOption(ref DropDownList ddl, int inVal)
        {
            SelectOption(ref ddl, inVal.ToString());
        }

        /// <summary>
        /// Search the drop down list for the selected item and if found set to it.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="inVal"></param>
        public static void SelectOption(ref DropDownList ddl, string inVal)
		{
			ListItem li = null;

			try
			{
				li = ddl.Items.FindByValue(inVal);
                if (li != null)
                {
                    ddl.SelectedValue = inVal;
                }
			}
			catch
			{
				ddl.SelectedIndex = 0;  
			}
        }

        /// <summary>
        /// Set any of the selected values ...
        /// </summary>
        /// <param name="chkList"></param>
        /// <param name="inVal"></param>
        public static void SelectOption(ref ListBox lstBox, string inVal)
        {
            int idx = 0;

            for (idx = 0; idx < lstBox.Items.Count; idx++)
            {
                ListItem li = lstBox.Items[idx];

                if (inVal == li.Value)
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// Search the drop down list for the selected item and if found set it. If not found, add the item but disable it.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="inVal"></param>
        /// <param name="inTextIfNotFound"></param>
        public static void SelectOption(ref DropDownList ddl, string inVal, string inTextIfNotFound)
        {
            ListItem li = null;

            try
            {
                li = ddl.Items.FindByValue(inVal);
                if (li != null)
                {
                    ddl.SelectedValue = inVal;
                }
                else
                {
                    li = new ListItem(inTextIfNotFound, inVal);
                    li.Selected = true;

                    ddl.Items.Add(li);
                }
            }
            catch
            {
                ddl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Set any of the selected values ...
        /// </summary>
        /// <param name="chkList"></param>
        /// <param name="inVal"></param>
        public static void SelectOption(ref CheckBoxList chkList, List<string> inVal)
        {
            int idx = 0;

            for (idx = 0; idx < chkList.Items.Count; idx++)
            {
                ListItem li = chkList.Items[idx];

                if (inVal.Contains(li.Value))
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// Set any of the selected values ...
        /// </summary>
        /// <param name="chkList"></param>
        /// <param name="inVal"></param>
        public static void SelectOption(ref RadioButtonList rdbList, string inVal)
        {
            int idx = 0;

            for (idx = 0; idx < rdbList.Items.Count; idx++)
            {
                ListItem li = rdbList.Items[idx];

                if (inVal == li.Value)
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// Set Selected Value in a list Box.
        /// </summary>
        /// <param name="theList"></param>
        /// <param name="inValues"></param>
        public static void SetSelectedOption(ref ListBox theList, List<string> inValues)
        {
            foreach (ListItem li in theList.Items )
            {
                if (inValues.Contains(li.Value))
                {
                    li.Selected = true;
                }
            }
        }


        
        /// <summary>
        /// Restore the value from the submitted form ...
        /// </summary>
        /// <param name="thePage"></param>
        /// <param name="tb"></param>
        public static void FeedFromForm(ref TextBox tb)
        {
            tb.Text = tb.Page.Request.Form[tb.UniqueID];  
        }

        /// <summary>
        /// Restore the value from the submitted form ...
        /// </summary>
        /// <param name="thePage"></param>
        /// <param name="tb"></param>
        public static void FeedFromForm(ref DropDownList  ddl)
        {
            SelectOption(ref ddl, ddl.Page.Request.Form[ddl.UniqueID]);     
        }

        /// <summary>
        /// unckeck all items in the list .....
        /// </summary>
        /// <param name="rl"></param>
        public static void RadioList_ClearSelection(RadioButtonList rl)
        {
            int idx;

            // Loop around ..
            for (idx = 0; idx < rl.Items.Count; idx++)
            {
                rl.Items[idx].Selected = false;
            }

        }

        #endregion

        #region "Read Control Selected Value/Text ..."

        /// <summary>
        /// Read the int in the text box.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadControlValue(TextBox tb, int defValue)
        {
            return ALCS_DataShift.WhenNull(tb.Text, defValue);
        }

        /// <summary>
        /// Read the string in the text box. 
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadControlValue(TextBox tb, string defValue)
        {
            return ALCS_DataShift.WhenNull(tb.Text, defValue);
        }

        /// <summary>
        /// Read the decimal in the text box.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal ReadControlValue(TextBox tb, decimal defValue)
        {
            return ALCS_DataShift.WhenNull(tb.Text, defValue);
        }


        /// <summary>
        /// Read Date From Field.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ReadControlDate(TextBox tb, DateParseMode pm, DateTime defValue)
        {
            string dateStr = ReadControlValue(tb, "");

            if (dateStr == "")
            {
                return defValue;
            }

            // Get the Date ...
            DateTime outDate = DateTime.MaxValue;
            bool dateOK = ALCS_DateReaderWriter.ReadDate(dateStr, pm, out outDate);

            if (dateOK)
            {
                return outDate;
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// Another Version ...
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static DateTime ReadControlDate(TextBox tb, DateParseMode pm)
        {
            return ReadControlDate(tb, pm, DateTime.MaxValue);
        }

        /// <summary>
        /// Another Version ...
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static DateTime ReadControlDate(TextBox tb)
        {
            return ReadControlDate(tb, DateParseMode.Small, DateTime.MaxValue);
        }

        /// <summary>
        /// Another Text Return Value.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public static string ReadControlDateString(TextBox tb, DateParseMode pm)
        {
            DateTime outDate = ReadControlDate(tb, pm, DateTime.MaxValue);

            if (outDate == DateTime.MaxValue)
            {
                return "";
            }
            else
            {
                return outDate.ToString("dd MMM yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Another Vesion.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string ReadControlDateString(TextBox tb)
        {
            return ReadControlDateString(tb, DateParseMode.Small);
        }

        /// <summary>
        /// Read the submitted int in drop down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadControlValue(DropDownList ddl, int defValue)
        {
            return ALCS_DataShift.WhenNull(ddl.SelectedValue, defValue);
        }

        /// <summary>
        /// Read the submitted int in drop down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadControlValue(DropDownList ddl, string defValue)
        {
            return ALCS_DataShift.WhenNull(ddl.SelectedValue, defValue);
        }

        /// <summary>
        /// List Box case.
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadControlValue(ListBox lst, string defValue)
        {
            string selItems = "";

            foreach (ListItem li in lst.Items)
            {
                if (li.Selected)
                {
                    selItems += ((selItems == "") ? (li.Value) : ("," + li.Value));
                }
            }

            return ALCS_DataShift.WhenNull(selItems, "");
        }


        /// <summary>
        /// is the check box on or off.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        public static bool ReadControlValue(CheckBox cb)
        {
            return cb.Checked;
        }

        /// <summary>
        /// Read the submitted int in check box list.
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadControlValue(RadioButtonList rbl, string defValue)
        {
            return ALCS_DataShift.WhenNull(rbl.SelectedValue, defValue);
        }

        /// <summary>
        /// Read the submitted int in check box list.
        /// </summary>
        /// <param name="cbl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadControlValue(RadioButtonList rbl, int defValue)
        {
            return ALCS_DataShift.WhenNull(rbl.SelectedValue, defValue);
        }

        /// <summary>
        /// Read the value stored in the hideent field.
        /// </summary>
        /// <param name="hf"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadControlValue(HiddenField hf, int defValue)
        {
            return ALCS_DataShift.WhenNull(hf.Value, defValue);
        }

        /// <summary>
        /// Read the value stored in the hideent field.
        /// </summary>
        /// <param name="hf"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadControlValue(HiddenField hf, string defValue)
        {
            return ALCS_DataShift.WhenNull(hf.Value, defValue);
        }

        // A new wave of readers ...

        /// <summary>
        /// Text Boxes 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int PickControlValue(Page p, TextBox cntrl, int defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Text Boxes.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string PickControlValue(Page p, TextBox cntrl, string defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Hidden Field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cntrl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int PickControlValue(Page p, HiddenField  cntrl, int defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Hidden Field.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cntrl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string PickControlValue(Page p, HiddenField  cntrl, string defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Drop Down List.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cntrl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int PickControlValue(Page p, DropDownList cntrl, int defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Drop Down List.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cntrl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string PickControlValue(Page p, DropDownList cntrl, string defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl, defValue);
            }
            else
            {
                return ReadControlValue(cntrl, defValue);
            }
        }

        /// <summary>
        /// Read Checkbox TICK.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cntrl"></param>
        /// <returns></returns>
        public static bool PickControlValue(Page p, CheckBox cntrl)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedValue(cntrl);
            }
            else
            {
                return ReadControlValue(cntrl);
            }
        }


        /// <summary>
        /// Date.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tb"></param>
        /// <param name="pm"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime PickControlDate(Page p, TextBox tb, DateParseMode pm, DateTime defValue)
        {
            if (p.IsPostBack)
            {
                return ReadSubmittedDate(tb, pm, defValue);
            }
            else
            {
                return ReadControlDate(tb, pm, defValue);
            }
        }

        /// <summary>
        /// Date - Another Version.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tb"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public static DateTime PickControlDate(Page p, TextBox tb, DateParseMode pm)
        {
            return PickControlDate(p, tb, pm, DateTime.MaxValue);
        }

        /// <summary>
        /// Date - Another Version.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static DateTime PickControlDate(Page p, TextBox tb)
        {
            return PickControlDate(p, tb, DateParseMode.Small);
        }


        #endregion 

        #region "Read Submitted Data ..."

        /// <summary>
        /// Read the submitted int in the text box.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadSubmittedValue(TextBox tb, int defValue)
        {
           return  ALCS_DataShift.WhenNull(tb.Page.Request.Form[tb.UniqueID], defValue);
        }

        /// <summary>
        /// Read the submitted int in the text box. 
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(TextBox tb, string defValue)
        {
            return ALCS_DataShift.WhenNull(tb.Page.Request.Form[tb.UniqueID], defValue);
        }

        /// <summary>
        /// Read Submitteed Decimal.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal ReadSubmittedValue(TextBox tb, decimal defValue)
        {
            return ALCS_DataShift.WhenNull(tb.Page.Request.Form[tb.UniqueID], defValue);
        }


        /// <summary>
        /// Read Date From Field.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ReadSubmittedDate(TextBox tb, DateParseMode pm, DateTime defValue)
        {
            string dateStr = ReadSubmittedValue(tb, "");

            if (dateStr == "")
            {
                return defValue;
            }
           
            // Get the Date ...
            DateTime outDate = DateTime.MaxValue;
            bool dateOK = ALCS_DateReaderWriter.ReadDate(dateStr, pm, out outDate);

            if (dateOK)
            {
                return outDate;
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// Another Version ...
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static DateTime ReadSubmittedDate(TextBox tb, DateParseMode pm)
        {
            return ReadSubmittedDate(tb, pm, DateTime.MaxValue);
        }

        /// <summary>
        /// Another Version ...
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static DateTime ReadSubmittedDate(TextBox tb)
        {
            return ReadSubmittedDate(tb, DateParseMode.Small, DateTime.MaxValue);
        }

        /// <summary>
        /// Another Text Return Value.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public static string ReadSubmittedDateString(TextBox tb, DateParseMode pm)
        {
            DateTime outDate = ReadSubmittedDate(tb, pm, DateTime.MaxValue);

            if (outDate == DateTime.MaxValue)
            {
                return "";
            }
            else
            {
                return outDate.ToString("dd MMM yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Another Vesion.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string ReadSubmittedDateString(TextBox tb)
        {
            return ReadSubmittedDateString(tb, DateParseMode.Small);
        }


        /// <summary>
        /// Read the submitted int in drop down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadSubmittedValue(DropDownList ddl, int defValue)
        {
            if (ddl.Enabled)
            {
                return ALCS_DataShift.WhenNull(ddl.Page.Request.Form[ddl.UniqueID], defValue);
            }
            else
            {
                return ALCS_DataShift.WhenNull(ddl.SelectedValue, defValue);
            }
        }

        /// <summary>
        /// Read the submitted int in drop down list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(DropDownList ddl, string defValue)
        {
            if (ddl.Enabled)
            {
                return ALCS_DataShift.WhenNull(ddl.Page.Request.Form[ddl.UniqueID], defValue);
            }
            else
            {
                return ALCS_DataShift.WhenNull(ddl.SelectedValue, defValue);
            }
        }

        /// <summary>
        /// Same for List Box.
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(ListBox lst, string defValue)
        {
            if (lst.Enabled)
            {
                return ALCS_DataShift.WhenNull(lst.Page.Request.Form[lst.UniqueID], defValue);
            }
            else
            {
                return ALCS_DataShift.WhenNull(lst.SelectedValue, defValue);
            }
        }


        /// <summary>
        /// Was the checkbox checked.
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ReadSubmittedValue(CheckBox cb)
        {
            if (cb.Enabled)
            {
                return ALCS_DataShift.AsBool(ALCS_DataShift.WhenNull(cb.Page.Request.Form[cb.UniqueID], ""));
            }
            else
            {
                return cb.Checked;
            }
        }

        /// <summary>
        /// Input Checkbox.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        public static bool ReadSubmittedValue(HtmlInputCheckBox cb)
        {
            if (!cb.Disabled)
            {
                return ALCS_DataShift.AsBool(ALCS_DataShift.WhenNull(cb.Page.Request.Form[cb.UniqueID], ""));
            }
            else
            {
                return cb.Checked;
            }
        }

        /// <summary>
        /// Read the submitted int in check box list
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(CheckBoxList chl, string defValue)
        {
            string selStream = "";

            foreach (ListItem li in chl.Items)
            {
                if (li.Selected)
                {
                    selStream += ((selStream == "") ? "" : ",") + li.Value.ToString();
                }
            }

            return selStream;
        }

        /// <summary>
        /// Was the radio button checked.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ReadSubmittedValue(RadioButton rb)
        {
            if (rb.Enabled)
            {
                if (rb.GroupName == "")
                {
                    return ALCS_DataShift.AsBool(ALCS_DataShift.WhenNull(rb.Page.Request.Form[rb.UniqueID], ""));
                }
                else
                {
                    string subKey = rb.UniqueID.Replace(rb.ID, rb.GroupName);
                    string subValue = ALCS_DataShift.WhenNull(rb.Page.Request.Form[subKey], "");
                    return (subValue.ToLower() == rb.ID.ToLower());
                }
            }
            else
            {
                return rb.Checked;
            }
        }

        /// <summary>
        /// Read the submitted int in check box list
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadSubmittedValue(RadioButtonList rbl, int defValue)
        {
            if (rbl.Enabled)
            {
                return ALCS_DataShift.WhenNull(rbl.Page.Request.Form[rbl.UniqueID], defValue);
            }
            else
            {
                return ALCS_DataShift.WhenNull(rbl.SelectedValue, defValue);
            }
        }

        /// <summary>
        /// Read the submitted int in check box list
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(RadioButtonList rbl, string defValue)
        {
            if (rbl.Enabled)
            {
                return ALCS_DataShift.WhenNull(rbl.Page.Request.Form[rbl.UniqueID], defValue);
            }
            else
            {
                return ALCS_DataShift.WhenNull(rbl.SelectedValue, defValue);
            }
        }

        /// <summary>
        /// Read the submitted int in the hidden field. 
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ReadSubmittedValue(HiddenField hid, int defValue)
        {
            return ALCS_DataShift.WhenNull(hid.Page.Request.Form[hid.UniqueID], defValue);
        }

        /// <summary>
        /// Read the submitted int in the hidden field. 
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ReadSubmittedValue(HiddenField hid, string defValue)
        {
            return ALCS_DataShift.WhenNull(hid.Page.Request.Form[hid.UniqueID], defValue);
        }

        #endregion 

        #region "Read Query String Value"

        /// <summary>
        /// Read the value from the query string.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="theKey"></param>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool ReadQueryStringValue(Page p, string theKey, out int theValue)
        {
            string rawValue = "";

            // read the value and convert it.
            try
            {
                rawValue = ALCS_DataShift.WhenNull(p.Request.QueryString[theKey], "");
                theValue = Convert.ToInt32(rawValue);
            }
            catch
            {
                theValue = Int32.MinValue;
                return false;
            }

            // return.
            return true;
        }

        /// <summary>
        /// Read the value from the query string.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="theKey"></param>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool ReadQueryStringValue(Page p, string theKey, out string theValue)
        {
            // read the value and convert it.
            try
            {
                theValue = ALCS_DataShift.WhenNull(p.Request.QueryString[theKey], "");
            }
            catch
            {
                theValue = "?";
                return false;
            }

            // return.
            return true;
        }

        /// <summary>
        /// Read the value from the query string.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="theKey"></param>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool ReadQueryStringValue(Page p, string theKey, out DateTime theValue)
        {
            string dateStr = "";

            // Read the value and convert it.
            try
            {
                dateStr = ALCS_DataShift.WhenNull(p.Request.QueryString[theKey], "");
            }
            catch
            {
                theValue = DateTime.MinValue;
                return false;
            }

            // Return 
            return DateTime.TryParse(dateStr, out theValue);
        }


        #endregion 

        #region "Find Controls ..."

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="ContainerCtl"></param>
        /// <param name="IdToFind"></param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
            {
                return Root;
            }

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);

                if (FoundCtl != null)
                {
                    return FoundCtl;
                }
            }

            return null;
        }

        #endregion 

        #region "Drop down tools ..."

        /// <summary>
        /// Set Value and look of the first element in a list.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="firstText"></param>
        /// <param name="firstColor"></param>
        /// <param name="firstFontStyle"></param>
        public static void DropDown_FirstItemCosmetic(ref DropDownList ddl, string firstText, string firstColor, string firstFontStyle, bool forceIt)
        {
            string theStyle = "";

            // force an entry if there is none.
            if ((forceIt) && (ddl.Items.Count == 0))
            {
                ddl.Items.Add(new ListItem("", ""));
            }

            // Set the First Item.
            if((ddl.Items.Count > 0) && (ddl.Items[0].Value == ""))
            {
                ddl.Items[0].Text = firstText;

                if(firstColor != "")
                {
                    theStyle += "color:" + firstColor + ";";
                }

                if(firstFontStyle != "")
                {
                    theStyle += "font-style:" + firstFontStyle + ";";
                }

                if (theStyle != "")
                {
                    ddl.Items[0].Attributes.Add("style", theStyle );
                }
            }
        }

        #endregion 
    }
}
