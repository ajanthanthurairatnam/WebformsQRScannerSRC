using System;
using System.Data;
using System.Data.SqlClient; 
using System.Collections;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Format;
using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Data.ALCS_SQLWork;  

namespace ALCS_Library.ALCS_WWW.ALCS_WWWControls
{

	//////////////////////////////////////////////////////////////////
	/// Load an array list with a list of items.
	//////////////////////////////////////////////////////////////////
    #region "Array List Loader"

	public	class ALCS_ArrayListLoader
		{ 
			private string listSource = ""; 
			private string listDB = "";
			private ALCS_lookItem defItem;
			private bool health = true; 
			private string memo = ""; 

			/// <summary>
			///  Constructor 1 - specify the query
			/// </summary>
			/// <param name="theSource"></param>
			public ALCS_ArrayListLoader(string theSource) 
			{ 
				this.listSource = theSource;
                this.listDB = ALCS_DB.System_DataBase;
			}

			/// <summary>
			/// Constructor 2 - Specify source and DB.
			/// </summary>
			/// <param name="theSource"></param>
			/// <param name="theDB"></param>
			public ALCS_ArrayListLoader(string theSource, string theDB) 
			{ 
				this.listSource = theSource; 
				this.listDB = theDB;
			} 

			/// <summary>
			/// Constructor 3 - source and default item Load the Default Item ....
			/// </summary>
			/// <param name="theSource"></param>
			/// <param name="theDB"></param>
			public ALCS_ArrayListLoader(string theSource , string[] itemElems)
			{

				this.listSource = theSource;
                this.listDB = ALCS_DB.System_DataBase;

				// Set the Default Item
				SetDefaultItem(itemElems);
			}

			/// <summary>
			/// Constructor 4 - SET DB, Query and default item.
			/// </summary>
			/// <param name="theSource"></param>
			/// <param name="theDB"></param>
			public ALCS_ArrayListLoader(string theSource , string theDB, string[] itemElems)
			{

				this.listSource = theSource; 
				this.listDB = theDB;

				// Set the Default Item
				SetDefaultItem(itemElems);
			}

			/// <summary>
			/// Set the Default Item 
			/// </summary>
			void SetDefaultItem(string[] itemElems)
			{
				// Create the default top Item.
				if(itemElems.Length ==2)
				{
					this.defItem = new ALCS_lookItem(itemElems[1],itemElems[0]);
				}
				else
				{
					this.health = false;
					this.memo = "ill formed default item structure.";
				}
			}

			/// <summary>
			/// Load the array list and default the first element.
			/// </summary>
			/// <param name="theArrayList"></param>
			public void LoadArrayList(ref ArrayList theArrayList)
			{
				this.LoadArrayList(ref theArrayList, false); 
			}

			/// <summary>
			/// Load the array list and default the first element.
			/// </summary>
			/// <param name="theArrayList"></param>
			public void LoadArrayList(ref ArrayList theArrayList, bool addStart) 
			{ 
				SqlConnection cn= new SqlConnection(); 
				SqlCommand cm; 
				SqlDataReader dr; 
				
				//Clear the data ....
				theArrayList.Clear(); 
				if(addStart)
				{
					theArrayList.Add(this.defItem);  
				}

				//Create an open a connection 
				ALCS_SQLConnection db = new ALCS_SQLConnection(this.listDB); 
				db.OpenConnection(ref cn); 
				
				if (!(db.isHealthy)) 
				{ 
					this.health = false; 
					this.memo = db.Memo; 
					return; 
				} 

				//We have a connection? let' continue.
				cm = new SqlCommand(); 
				cm.Connection = cn; 
				cm.CommandText = this.listSource; 
				cm.CommandType = CommandType.Text; 

				// Open the data reader ....
				try 
				{ 
					dr = cm.ExecuteReader(CommandBehavior.CloseConnection); 
				} 
				catch (SqlException sx) 
				{ 
					this.health = false; 
					this.memo = sx.Message ; 
					if ((cn.State == ConnectionState.Open)) 
					{ 
						cm.Cancel(); 
						cm.Dispose(); 
						db.CloseConnection(ref cn); 
					} 
					return; 
				} 

				//We have some data .... let's continue.
				while (dr.Read()) 
				{ 
					ALCS_lookItem theItem;
 
					if ((dr.FieldCount > 2)) 
					{ 
						theItem = new ALCS_lookItem(((string)(dr["FD_Text"])), ((string)(dr["FD_Value"])), ((int)(dr["FD_Visibility"]))); 
					} 
					else 
					{ 
						theItem = new ALCS_lookItem(dr["FD_Text"].ToString() , dr["FD_Value"].ToString()); 
					} 
					theArrayList.Add(theItem); 
				}
                if (cn.State == ConnectionState.Open)
                {
                    cm.Cancel();
                    cm.Dispose();
                    dr.Close();
                    db.CloseConnection(ref cn); 
                }
			} 

			/// <summary>
			/// the isHealthy Accessor.
			/// </summary>
			public bool isHealthy
			{ 
				get
				{
					return this.health; 
				}
			} 

			/// <summary>
			/// Set or get the Memo.
			/// </summary>
			public string Memo
			{
				get
				{
					return this.memo;
				}
			} 
		} 

    #endregion 
    
    #region "Feed the drop down from the query ..."

    public enum DropDownMode
    {
        Limited,
        Extended,
    }

    public class ALCS_DropDownFeeder
    {
        private string listDB = ALCS_DB.System_DataBase;
        private string blankValue   = "";
        private string blankText    = "";
        private bool blankStart     = true;
        private bool health         = true;
        private string memo         = "";

        #region "Constructors ..."

        /// <summary>
        /// Constructor 0
        /// </summary>
        public ALCS_DropDownFeeder()
        {
            this.listDB = ALCS_DB.System_DataBase;
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="blankEntry"></param>
        public ALCS_DropDownFeeder(bool blankEntry)
        {
            this.listDB = ALCS_DB.System_DataBase;
            this.blankStart = blankEntry; 
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="theDB"></param>
        public ALCS_DropDownFeeder(string theDB)
        {
            this.listDB = theDB; 
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        /// <param name="theSource"></param>
        public ALCS_DropDownFeeder(string theSource, bool blankEntry)
        {
            this.listDB = theSource;
            this.blankStart = blankEntry; 
        }

        #endregion 

        #region "Accessors ..."

        /// <summary>
        /// Set and get the Blank Start Flag ....
        /// </summary>
        public bool BlankStart
        {
            get
            {
                return this.blankStart;
            }

            set
            {
                this.blankStart = value;

            }
        }

        /// <summary>
        /// Set the Properties of the blank property.
        /// </summary>
        /// <param name="bValue"></param>
        /// <param name="bText"></param>
        public void SetBlankProperties(string bValue, string bText)
        {
            this.blankValue     = bValue;
            this.blankText      = bText;
            this.blankStart = true;
        }


        #endregion 

        #region "Feed the drop down List ..."

        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder)
        {
            FeedDropDown(ref ddList, listFeeder, 0, 1, DropDownMode.Extended);
        }

        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder)
        {
            FeedDropDown(ref ddList, listFeeder, 0, 1, DropDownMode.Extended);
        }


        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder, DropDownMode ddMode)
        {
            FeedDropDown(ref ddList, listFeeder, 0, 1, ddMode);
        }

        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder, DropDownMode ddMode)
        {
            FeedDropDown(ref ddList, listFeeder, 0, 1, ddMode);
        }

        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder, int valIndex, int textIndex)
        {
            FeedDropDown(ref ddList, listFeeder, valIndex, textIndex, DropDownMode.Extended);
        }

        /// <summary>
        /// Feed the drop down list from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder, int valIndex, int textIndex)
        {
            FeedDropDown(ref ddList, listFeeder, valIndex, textIndex, DropDownMode.Extended);
        }

        /// <summary>
        /// Feed the Drop down from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder, string valField, string textField)
        {
            FeedDropDown(ref ddList, listFeeder, valField, textField, DropDownMode.Extended);
        }

        /// <summary>
        /// Feed the Drop down from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder, string valField, string textField)
        {
            FeedDropDown(ref ddList, listFeeder, valField, textField, DropDownMode.Extended);
        }


        /// <summary>
        /// Feed the Drop down from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder, int valIndex, int textIndex, DropDownMode ddMode)
        {
            this.FeedDropDown(ref ddList, listFeeder, valIndex.ToString(), textIndex.ToString(), ddMode);
        }

        /// <summary>
        /// Feed the Drop down from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valIndex"></param>
        /// <param name="textIndex"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder, int valIndex, int textIndex, DropDownMode ddMode)
        {
            this.FeedDropDown(ref ddList, listFeeder, valIndex.ToString(), textIndex.ToString(), ddMode);
        }


        /// <summary>
        /// Feed the Drop down from the query ....
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valField"></param>
        /// <param name="textField"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref DropDownList ddList, string listFeeder, string valField, string textField, DropDownMode ddMode)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;
            SqlDataReader dr;
            ListItem li;

            string itemText;
            string itemValue;
            string colName, colValue;

            //Clear the list.
            ddList.Items.Clear();

            //Create an open a connection 
            ALCS_SQLConnection db = new ALCS_SQLConnection(this.listDB);
            db.OpenConnection(ref cn);

            if (!db.isHealthy)
            {
                this.health = false;
                this.memo = db.Memo;
                return;
            }

            //We have a connection? let' continue.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = listFeeder;
            cm.CommandType = CommandType.Text;

            // Open the data reader ....
            try
            {
                dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sx)
            {
                this.health = false;
                this.memo = sx.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    db.CloseConnection(ref cn);
                }
                return;
            }

            // Did we add a blank entry ..
            bool blankAdded = false;

            //We have some data .... let's continue.
            while (dr.Read())
            {
                if ((this.blankStart) && (!blankAdded))
                {
                    blankAdded = true;
                    li = new ListItem(this.blankText, this.blankValue);
                    ddList.Items.Add(li);
                }

                if ((ALCS_Is.isInteger(valField, WorkMode.Strict)) && (ALCS_Is.isInteger(textField, WorkMode.Strict)))
                {
                    itemValue = ALCS_DataShift.WhenNull(dr[ALCS_DataShift.WhenNull(valField, 0)], "");
                    itemText = ALCS_DataShift.WhenNull(dr[ALCS_DataShift.WhenNull(textField, 0)], "");
                }
                else
                {
                    itemValue = ALCS_DataShift.WhenNull(dr[valField], "");
                    itemText = ALCS_DataShift.WhenNull(dr[textField], "");
                }

                // Create a List Item ....
                li = new ListItem(itemText, itemValue, true);

                if ((ddMode == DropDownMode.Extended) && (dr.FieldCount > 2))
                {
                    for (int idx = 2; idx < dr.FieldCount; idx++)
                    {
                        Type tp = dr.GetFieldType(idx);
                        colName = dr.GetName(idx);
                        colValue = ALCS_DataShift.WhenNull(dr[idx], "");
                        li.Attributes.Add(colName, colValue);

                        if (colName.ToUpper().EndsWith("_COLOR"))
                        {
                            li.Attributes.CssStyle.Add("color", colValue);
                        }
                    }
                }

                // Add the item ....
                ddList.Items.Add(li);
            }
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                dr.Close();
                db.CloseConnection(ref cn);
            }
        }

        /// <summary>
        /// Mimic for a List Box.
        /// </summary>
        /// <param name="ddList"></param>
        /// <param name="listFeeder"></param>
        /// <param name="valField"></param>
        /// <param name="textField"></param>
        /// <param name="ddMode"></param>
        public void FeedDropDown(ref ListBox ddList, string listFeeder, string valField, string textField, DropDownMode ddMode)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;
            SqlDataReader dr;
            ListItem li;

            string itemText;
            string itemValue;
            string colName, colValue;

            //Clear the list.
            ddList.Items.Clear();

            //Create an open a connection 
            ALCS_SQLConnection db = new ALCS_SQLConnection(this.listDB);
            db.OpenConnection(ref cn);

            if (!db.isHealthy)
            {
                this.health = false;
                this.memo = db.Memo;
                return;
            }

            //We have a connection? let' continue.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = listFeeder;
            cm.CommandType = CommandType.Text;

            // Open the data reader ....
            try
            {
                dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException sx)
            {
                this.health = false;
                this.memo = sx.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    db.CloseConnection(ref cn);
                }
                return;
            }

            // Did we add a blank entry ..
            bool blankAdded = false;

            //We have some data .... let's continue.
            while (dr.Read())
            {
                if ((this.blankStart) && (!blankAdded))
                {
                    blankAdded = true;
                    li = new ListItem(this.blankText, this.blankValue);
                    ddList.Items.Add(li);
                }

                if ((ALCS_Is.isInteger(valField, WorkMode.Strict)) && (ALCS_Is.isInteger(textField, WorkMode.Strict)))
                {
                    itemValue = ALCS_DataShift.WhenNull(dr[ALCS_DataShift.WhenNull(valField, 0)], "");
                    itemText = ALCS_DataShift.WhenNull(dr[ALCS_DataShift.WhenNull(textField, 0)], "");
                }
                else
                {
                    itemValue = ALCS_DataShift.WhenNull(dr[valField], "");
                    itemText = ALCS_DataShift.WhenNull(dr[textField], "");
                }

                // Create a List Item ....
                li = new ListItem(itemText, itemValue, true);

                if ((ddMode == DropDownMode.Extended) && (dr.FieldCount > 2))
                {
                    for (int idx = 2; idx < dr.FieldCount; idx++)
                    {
                        Type tp = dr.GetFieldType(idx);
                        colName = dr.GetName(idx);
                        colValue = ALCS_DataShift.WhenNull(dr[idx], "");
                        li.Attributes.Add(colName, colValue);

                        if (colName.ToUpper().EndsWith("_COLOR"))
                        {
                            li.Attributes.CssStyle.Add("color", colValue);
                        }
                    }
                }

                // Add the item ....
                ddList.Items.Add(li);
            }
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                dr.Close();
                db.CloseConnection(ref cn);
            }
        } 


        #endregion 

        #region "Expose the Data ..."
        /// <summary>
        /// the isHealthy Accessor.
        /// </summary>
        public bool isHealthy
        {
            get
            {
                return this.health;
            }
        }

        /// <summary>
        /// Set or get the Memo.
        /// </summary>
        public string Memo
        {
            get
            {
                return this.memo;
            }
        }
        #endregion 
    }
    
    #endregion 

    
}
