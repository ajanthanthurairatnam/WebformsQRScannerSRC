using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;  
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Text;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Numerics;


namespace ALCS_Library.ALCS_Menu
{
    #region "Menu ..."

    /// <summary>
    /// Menu Section ...
    /// </summary>
    public enum enMenuSection
    {
        LEVIES_ONLINE = 1,
        UNKNOWN = 99,
    }

    /// <summary>
    /// Menu Item ...
    /// </summary>
    public enum enMenuItem
    {
        Home = 1,
        My_Returns = 2,
        My_Payments = 3,
        My_Account = 4,
        Reminder_Options = 5,
        ChangePassword = 6,
        Logout = 7,

        Unknown = 99,
    }

    public enum enMenuScope
    {
        Include,
        Exclude,
        FakeAddition,
        All
    }

    #endregion 

    /// <summary>
    /// Summary description for ALCS_Menu_SL
    /// </summary>
    public class ALCS_Menu_SL
    {
        // Input properties ....
        private string queryString = "";

        // Any selected id and Group 
        private int selID    = -1;
        private string selGroup = "";

        // Output result ....
        private List<ALCS_MenuItem_SL> menuStack;

        // Health Monitor ...
        private bool health = true;
        private ArrayList memo = new ArrayList();

        #region "Constructor ..."

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="xmlSource"></param>
        /// <param name="menuFilter"></param>
        public ALCS_Menu_SL(String xmlSource, enMenuSection inSection)
        {
            BuildMenuStack(xmlSource, inSection, enMenuScope.All, new List<enMenuItem>());
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="xmlSource"></param>
        /// <param name="menuFilter"></param>
        public ALCS_Menu_SL(String xmlSource, enMenuSection inSection, enMenuScope ms, List<enMenuItem> mc)
        {
            BuildMenuStack(xmlSource, inSection, ms, mc);
        }
        
        /// <summary>
        /// Build the menu stack ...
        /// </summary>
        /// <param name="xmlSource"></param>
        /// <param name="menuFilter"></param>
        /// <param name="ms"></param>
        /// <param name="mc"></param>
        public void BuildMenuStack(String xmlSource, enMenuSection inSection, enMenuScope ms, List<enMenuItem> mc)
        {
            // Qualify the path of the file ...
            xmlSource = ALCS_Utils.ResolveFilePath(xmlSource);

            // Does the file exists ...
            if (!File.Exists(xmlSource))
            {
                this.health = false;
                this._StackMemo("The source XML file was not found.");
                return;
            }

            // Set the XML Path to follow 
            string xmlPath = "";
            if(inSection ==  enMenuSection.UNKNOWN)
            {
                xmlPath = "/LPD_MENU/MENUSECTION/MENUITEM";
            }
            else
            {
                xmlPath = "/LPD_MENU/MENUSECTION[@ID=" + My.MyEnumInt(inSection) + "]/MENUITEM";
            }

            // Does the file exixts ...
            if (!File.Exists(xmlSource))
            {
                this.health = false;
                this._StackMemo("The XML file cannot be found");
            }

            // Load the XML document 
            XmlDocument xmlDoc;

            // Introduce the Caching Here ...
            if(HttpContext.Current.Cache["menu"] != null)
            {
                xmlDoc = (XmlDocument) HttpContext.Current.Cache["menu"];
            }
            else
            {
                xmlDoc = new XmlDocument();

                // Load the document ...
                xmlDoc.Load(xmlSource);

                HttpContext.Current.Cache.Insert("menu",
                                xmlDoc,
                                new CacheDependency(xmlSource),
                                DateTime.Now.AddDays(30),
                                Cache.NoSlidingExpiration,
                                CacheItemPriority.High,
                                null);
            }

            // Read the nodes in the document ...
            XmlNodeList xmlNodes = xmlDoc.SelectNodes(xmlPath);

            // Load the stack 
            this.menuStack = new List<ALCS_MenuItem_SL>();

            // Loop around and build it one by one.
            foreach (XmlNode xmlN in xmlNodes)
            {
                int itemID = ALCS_DataShift.WhenNull(xmlN.Attributes["ID"].InnerText, 0);

                enMenuItem itemEnum = to_enMenuItem(itemID);

                bool itemEnabled = (ALCS_DataShift.WhenNull(xmlN.Attributes["ENABLED"].InnerText, 0) == 0);

                string itemLabel = ReadAttibute(xmlN,"LABEL", "");
                int itemOrder = ALCS_DataShift.WhenNull(xmlN["ORDER"].InnerText, 1);
                string itemText = ALCS_DataShift.WhenNull(xmlN["TEXT"].InnerText, "");
                string itemURL = ALCS_DataShift.WhenNull(xmlN["URL"].InnerText, "");
                string itemTraget = ALCS_DataShift.WhenNull(xmlN["TARGET"].InnerText, "");

                // Should this item be added to the Stack ...
                if ((ms == enMenuScope.Include) && (!mc.Contains(itemEnum)))
                {
                    continue;
                }

                if ((ms == enMenuScope.Exclude) && (mc.Contains(itemEnum)))
                {
                    continue;
                }

                // Create the menu item and stack it ...
                ALCS_MenuItem_SL mi = new ALCS_MenuItem_SL(itemID, itemLabel, itemOrder, itemText, itemURL, itemTraget, itemEnabled);
                this.menuStack.Add(mi);
            }
        }

        /// <summary>
        /// Read the Attibute ...
        /// </summary>
        /// <param name="xmlN"></param>
        /// <param name="attrCode"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        public string ReadAttibute(XmlNode xmlN, string attrCode, string attrDef)
        {
            if (xmlN.Attributes.GetNamedItem(attrCode) == null)
            {
                return attrDef;
            }
            else
            {
                return ALCS_DataShift.WhenNull(xmlN[attrCode].InnerText, attrDef);
            }
        }

        #endregion 

        #region "Set the menu parameters ..."

        /// <summary>
        /// in this version the user will supply a set of labels and values
        /// and the system will set assign the values to session variables 
        /// made out of those labels ...
        /// </summary>
        /// <param name="menuLabels"></param>
        /// <param name="menuParameters"></param>
        public void _SetMenuParameters(string[] menuLabels, string[] menuParameters, DataChannel dc)
        {
            int idx = 0;

            if (menuLabels.Length != menuParameters.Length)
            {
                this.health = false;
                this._StackMemo("Menu parameters and labels do not match");
            }
            else
            {
                // More checks need to be added here to make sure the session variables 
                // does not already exists ....Choose a mode of overwrite and or alert ...
                for (idx = 0; idx < menuLabels.Length; idx++)
                {
                    if (dc == DataChannel.BySession)
                    {
                        System.Web.HttpContext.Current.Session.Add(menuLabels[idx], menuParameters[idx]);
                    }
                    else
                    {
                        if (idx == 0)
                        {
                            queryString += "?" + menuLabels[idx] + "=" + menuParameters[idx];
                        }
                        else
                        {
                            queryString += "&" + menuLabels[idx] + "=" + menuParameters[idx];
                        }
                    }
                }
            }
        }


        /// <summary>
        /// in this version the user will supply one label and a set of values
        /// system will build an index of the label to hold the values ...
        /// </summary>
        /// <param name="parLabel"></param>
        /// <param name="menuParameters"></param>
        public void _SetMenuParameters(string parLabel, string[] menuParameters, DataChannel dc)
        {
            int idx = 0;

            // More checks need to be added here to make sure the session variables 
            // does not already exists ....Choose a mode of overwrite and or alert ...
            for (idx = 0; idx < menuParameters.Length; idx++)
            {
                if (dc == DataChannel.BySession)
                {
                    System.Web.HttpContext.Current.Session.Add(parLabel + Convert.ToString(idx + 1) + "_VAL", menuParameters[idx]);
                }
                else
                {
                    if (idx == 0)
                    {
                        queryString += "?" + parLabel + Convert.ToString(idx + 1) + "_VAL=" + menuParameters[idx];
                    }
                    else
                    {
                        queryString += "&" + parLabel + Convert.ToString(idx + 1) + "_VAL=" + menuParameters[idx];
                    }
                }
            }
        }

        /// <summary>
        /// another version with the session label assumed ....
        /// </summary>
        /// <param name="menuParameters"></param>
        public void _SetMenuParameters(string[] menuParameters, DataChannel dc)
        {
            this._SetMenuParameters("MENUPAR", menuParameters, dc);

        }

        /// <summary>
        /// Menu Item.
        /// </summary>
        /// <param name="inItem"></param>
        /// <returns></returns>
        public static enMenuItem to_enMenuItem(int inItem)
        {
            enMenuItem mi = enMenuItem.Unknown;

            // For Each ...
            foreach (enMenuItem em in Enum.GetValues(typeof(enMenuItem)))
            {
                if (My.MyEnumInt(em) == inItem)
                {
                    mi = em;
                    break;
                }
            }

            // return 
            return mi;
        }

        #endregion

        #region "Build and render the menu ..."

        /// <summary>
        /// return the HTML string representing the menu
        /// </summary>
        public string _BuildMenuStructur(enMenuItem activeItem)
        {
            // Variables ...
            int selID = My.MyEnumInt(activeItem);
            StringBuilder sbMenu = new StringBuilder();

            sbMenu.AppendLine("<ul class=\"menu_Block\">");

            for (int idx = 0; idx < this.menuStack.Count; idx++)
            {
                // Build the cell 
                ALCS_MenuItem_SL mis = this.menuStack[idx];

                if (mis._MiEnabled)
                {
                    sbMenu.AppendLine("<li class=\"menu_Item\">" + mis._MiText + "</li>");
                }
                else if (mis._MiID == selID)
                {
                    sbMenu.AppendLine("<li class=\"menu_Item_A\">" + mis._MiText + "</li>");
                }
                else
                {
                    sbMenu.AppendLine("<li class=\"menu_Item\"><a href=\"" + mis._BuildHREF() + "\">" + mis._MiText + "</a></li>");
                }
            }

            sbMenu.AppendLine("</ul>");

            // return 
            return sbMenu.ToString();
        }

        /// <summary>
        /// Expose the error message if any in the menu box.
        /// </summary>
        public void _ShowTheUglyFace(ref HtmlTable table)
        {
            if (this.isHealthy)
            {
                return;
            }

            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = new HtmlTableCell();

            td.Attributes.Add("class", "menuError");
            td.InnerHtml = "Menu Error:<br/><br class=\"Gap5\"/>" + this._MemoStr + "<br/><br class=\"Gap5\"/>";

            // add to the row ...
            tr.Cells.Add(td);

            // Add the row ...
            table.Rows.Add(tr);
        }

        #endregion

        #region "General Predicates ..."

        // Search predicate returns true if a string ends in "saurus".
        public bool ItemByID(ALCS_MenuItem_SL mi)
        {
            if (mi._MiID == selID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 
        
        #region "Properties Channels ..."

        // The health of the class 
        public bool isHealthy
        {
            get
            {
                return this.health;
            }
        }

        // Memo as a string 
        public string _MemoStr
        {
            get
            {
                string memStr = "";

                for (int idx = 0; idx < this.memo.Count; idx++)
                {
                    memStr += this.memo[idx].ToString() + "\n";
                }

                // The Memo String 
                return memStr;
            }
        }

        // The last memo added to the stack
        public string _MemoLastEntry
        {
            get
            {
                string memStr = "";
                if (this.memo.Count != 0)
                {
                    memStr = this.memo[this.memo.Count - 1].ToString();
                }

                // The Memo String 
                return memStr;
            }
        }

        
        // The Memo Collection 
        public ArrayList _Memo
        {
            get
            {
                return this.memo;
            }

            set
            {
                this.memo.Add(value);
            }
        }

        // Add an error to the stacl 
        public void _StackMemo(string memoStr)
        {
            this.memo.Add(memoStr);
        }

    #endregion 

    }
}
