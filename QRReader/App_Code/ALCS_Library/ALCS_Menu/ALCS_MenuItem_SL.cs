using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ALCS_Library.ALCS_Basics;

namespace ALCS_Library.ALCS_Menu
{

    /// <summary>
    /// Summary description for ALCS_MenuItemL1
    /// </summary>
    public class ALCS_MenuItem_SL
    {
        private int miID;        // A Unique identifier of the item ...
        private bool miEnabled; // is that option enabled ....
        private string miLabel;     // a string identifier : not really needed
        private int miOrder;        // a number to sort the items on 
        private string miText;      // The actual visible text ...
        private string miURL;       // The URL to navigate to ...
        private string miTarget;    // The target of the URL I:internal or E:external

        /// <summary>
        /// Constructor One 
        /// </summary>
        ///<param name="inID"></param>
        /// <param name="inLabel"></param>
        /// <param name="inOrder"></param>
        /// <param name="inText"></param>
        /// <param name="inURL"></param>
        /// <param name="inTarget"></param>
        public ALCS_MenuItem_SL(int inID, string inLabel, int inOrder, string inText, string inURL, string inTarget, bool inEnabled)
        {
            this.miID = inID;
            this.miLabel = inLabel;
            this.miOrder = inOrder;
            this.miText = inText;
            this.miURL = inURL;
            this.miTarget = inTarget;
            this.miEnabled = inEnabled;
        }


        /// <summary>
        /// Constructor One 
        /// </summary>
        ///<param name="inID"></param>
        /// <param name="inLabel"></param>
        /// <param name="inOrder"></param>
        /// <param name="inText"></param>
        /// <param name="inURL"></param>
        /// <param name="inTarget"></param>
        public ALCS_MenuItem_SL(string inID, string inLabel, int inOrder, string inText, string inURL, string inTarget, bool inEnabled)
        {
            this.miID = Convert.ToInt32(inID);
            this.miLabel = inLabel;
            this.miOrder = inOrder;
            this.miText = inText;
            this.miURL = inURL;
            this.miTarget = inTarget;
            this.miEnabled = inEnabled;
        }

        /// <summary>
        /// Constructor Two 
        /// </summary>
        ///<param name="inID"></param>
        /// <param name="inLabel"></param>
        /// <param name="inOrder"></param>
        /// <param name="inText"></param>
        /// <param name="inURL"></param>
        /// <param name="inTarget"></param>
        public ALCS_MenuItem_SL(int inID, string inLabel, int inOrder, string inText, string inURL, bool inEnabled)
        {
            this.miID = inID;
            this.miLabel = inLabel;
            this.miOrder = inOrder;
            this.miText = inText;
            this.miURL = inURL;
            this.miTarget = "I";
        }

        #region "The set get series"

        // The ID 
        public int _MiID
        {
            get { return miID; }
            set { miID = value; }
        }

        // The Label 
        public string _MiLabel
        {
            get { return miLabel; }
            set { miLabel = value; }
        }

        // The Order 
        public int _MiOrder
        {
            get { return miOrder; }
            set { miOrder = value; }
        }

        // The Text 
        public string _MiText
        {
            get { return miText; }
            set { miText = value; }
        }

        // The URL 
        public string _MiURL
        {
            get { return miURL; }
            set { miURL = value; }
        }

        // The Target 
        public string _MiTarget
        {
            get { return miTarget; }
            set { miTarget = value; }
        }

        // Enabled?
        public bool _MiEnabled
        {
            get { return miEnabled; }
            set { miEnabled= value; }
        }

        // URLTarget
        public urlLocation _URLTarget
        {
            get 
            {
                if (this.miTarget == "E")
                {
                    return urlLocation.External;
                }
                else if (this.miTarget == "FI")
                {
                    return urlLocation.FlyingInternal;
                }
                else if (this.miTarget == "FE")
                {
                    return urlLocation.FlyingExternal;
                }
                else if (this.miTarget == "I")
                {
                    return urlLocation.Internal;
                }
                else
                {
                    return urlLocation.Internal;
                }
            }
        }


        #endregion

        #region "General Tools"

        // Build the Href .....

        public string _BuildHREF()
        {
            return _BuildHREF(new string[]{}, ""); 
        }

        public string _BuildHREF(string[] menuParame, string parLabel)
        {
            string miHREF = ALCS_Utils.ResolveURL(this.miURL, this._URLTarget);

            /// More code might be needed here to add data if required ....


            // return 
            return miHREF;
        }
        #endregion

    }
}
