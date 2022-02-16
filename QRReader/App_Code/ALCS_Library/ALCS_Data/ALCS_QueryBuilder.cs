using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;

using ALCS_Library.ALCS_Basics;

namespace ALCS_Library.ALCS_Data
{

	#region	  "SQL Enumerations"

	// Convert with AND or OR.
	public enum SQLJoin 
	{ 
		jwOR,	//	Join with an OR
		jwAND	//	Join with an AND
	} 

	// How to compare.
	public enum SQLCompare 
	{ 
		eqAsNumbers, 
		eqAsStrings,
        diffAsNumbers,
        diffAsStrings,
		gtAsNumbers,
		gtAsStrings,
		gteAsNumbers,
		gteAsStrings,
		ltAsNumbers,
		ltAsStrings,
		lteAsNumbers,
		lteAsStrings,
		startWith, 
		endWith, 
		contains, 
		fillMode 
	} 

	// Sql Nulls Values.
	public enum SQLNulls 
	{ 
		isNull, 
		isNotNull 
	}
 
	// Subquery...
	public enum SQLSubQuery 
	{ 
		isIn, 
		isNotIn 
	}
 
	//   How To Loop
	public enum LoopMaster 
	{ 
		Fields, 
		Keywords 
	} 

	// Data Mode
	public enum DataMode
	{
		Number,
		String
	}

	#endregion 
	
	#region "Build a Where Clause"

	/// <summary>
	/// Help building a query.
	/// </summary>
	public static class ALCS_QueryBuilder
	{

		/// <summary>
		/// Build the Where Clause for string Values.
		/// </summary>
		/// <param name="theWhere"></param>
		/// <param name="theField"></param>
		/// <param name="theValue"></param>
		/// <param name="theOperator"></param>
		/// <param name="theJoin"></param>
		/// <returns></returns>
		public static string ConstructWhere(string theWhere, string theField, string theValue, string theOperator, SQLJoin theJoin) 
		{ 
			string newWhere; 
			string newClause = ""; 
			string newValue;
 
			// Clean The New Value
			newValue = MaskSpecialCharacters(theValue);
 
			if(newValue != "")
			{
				newClause = SurroundString(theField + " " + theOperator + " '" + newValue + "'");
			}
 
			// The New Where 
			newWhere =  WhereFromWhere(theWhere,newClause, theJoin);
			
			// return the value.
			return newWhere; 
		}

        /// <summary>
        /// Date case.
        /// </summary>
        /// <param name="theWhere"></param>
        /// <param name="theField"></param>
        /// <param name="theValue"></param>
        /// <param name="theOperator"></param>
        /// <param name="theJoin"></param>
        /// <returns></returns>
        public static string ConstructWhere(string theWhere, string theField, DateTime theValue, string theOperator, SQLJoin theJoin)
        {
            string newWhere;
            string newClause = "";
            string newValue;

            // Clean The New Value
            if ((theValue == DateTime.MinValue) || (theValue == DateTime.MaxValue))
            {
                newValue = "";
            }
            else
            {
                newValue = theValue.ToString("dd MMM yyyy");
            }

            if (newValue != "")
            {
                newClause = SurroundString(theField + " " + theOperator + " '" + newValue + "'");
            }

            // The New Where 
            newWhere = WhereFromWhere(theWhere, newClause, theJoin);

            // return the value.
            return newWhere;
        } 

		/// <summary>
		/// Build the Where Clause for integer Values.
		/// </summary>
		/// <param name="theWhere"></param>
		/// <param name="theField"></param>
		/// <param name="theValue"></param>
		/// <param name="theOperator"></param>
		/// <param name="theJoin"></param>
		/// <returns></returns>
		public static string ConstructWhere(string theWhere, string theField, int theValue, string theOperator, SQLJoin theJoin) 
		{ 
			string newWhere; 
			string newClause = ""; 
			int newValue;
 
			// The New Clause
			newValue = theValue; 

			if(newClause != "")
			{
				newClause = SurroundString(theField + theOperator + newValue.ToString()); 
			}
			
			// The New Where 
			newWhere =  WhereFromWhere(theWhere, newClause, theJoin);

			// return the where clause.
			return newWhere; 
		}

        /// <summary>
        /// decimal case.
        /// </summary>
        /// <param name="theWhere"></param>
        /// <param name="theField"></param>
        /// <param name="theValue"></param>
        /// <param name="theOperator"></param>
        /// <param name="theJoin"></param>
        /// <returns></returns>
        public static string ConstructWhere(string theWhere, string theField, decimal theValue, string theOperator, SQLJoin theJoin)
        {
            string newWhere;
            string newClause = "";
            decimal newValue;

            // The New Clause
            newValue = theValue;

            if (newClause != "")
            {
                newClause = SurroundString(theField + theOperator + newValue.ToString());
            }

            // The New Where 
            newWhere = WhereFromWhere(theWhere, newClause, theJoin);

            // return the where clause.
            return newWhere;
        } 


		/// <summary>
		///  Construct the Where for strings with various operators.
		/// </summary>
		/// <param name="theWhere"></param>
		/// <param name="theField"></param>
		/// <param name="theValue"></param>
		/// <param name="theOperator"></param>
		/// <param name="theJoin"></param>
		/// <returns></returns>
		public static string ConstructWhere(string theWhere, string theField, string theValue, SQLCompare theOperator, SQLJoin theJoin) 
		{ 
			string newWhere; 
			string newClause = ""; 
			string newValue; 

			// Clean the New Value
			newValue = MaskSpecialCharacters(theValue); 

			// Get the new Clause 
            newClause = FieldKeywordMatching(theField, newValue, theOperator);

			// The New Where 
			newWhere =  WhereFromWhere(theWhere, newClause, theJoin);

			// return the where clause.....
			return newWhere; 
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theWhere"></param>
        /// <param name="theField"></param>
        /// <param name="theValue"></param>
        /// <param name="theOperator"></param>
        /// <param name="theJoin"></param>
        /// <returns></returns>
        public static string ConstructWhere(string theWhere, string theField, int theValue, SQLCompare theOperator, SQLJoin theJoin)
        {
            string newWhere;
            string newClause = "";
            string newValue;

            // Clean the New Value
            newValue = Convert.ToString(theValue);

            // Get the new Clause 
            newClause = FieldKeywordMatching(theField, newValue, theOperator);

            // The New Where 
            newWhere = WhereFromWhere(theWhere, newClause, theJoin);

            // return the where clause.....
            return newWhere;
        }

        /// <summary>
        /// Date Version.
        /// </summary>
        /// <param name="theWhere"></param>
        /// <param name="theField"></param>
        /// <param name="theValue"></param>
        /// <param name="theOperator"></param>
        /// <param name="theJoin"></param>
        /// <returns></returns>
        public static string ConstructWhere(string theWhere, string theField, DateTime theValue, SQLCompare theOperator, SQLJoin theJoin)
        {
            string newWhere;
            string newClause = "";
            string newValue;

            // Clean The New Value
            if ((theValue == DateTime.MinValue) || (theValue == DateTime.MaxValue))
            {
                newValue = "";
            }
            else
            {
                newValue = theValue.ToString("dd MMM yyyy");
            }

            // Get the new Clause 
            newClause = FieldKeywordMatching(theField, newValue, theOperator);

            // The New Where 
            newWhere = WhereFromWhere(theWhere, newClause, theJoin);

            // return the where clause.....
            return newWhere;
        }



        /// <summary>
        /// Decimal case.
        /// </summary>
        /// <param name="theWhere"></param>
        /// <param name="theField"></param>
        /// <param name="theValue"></param>
        /// <param name="theOperator"></param>
        /// <param name="theJoin"></param>
        /// <returns></returns>
        public static string ConstructWhere(string theWhere, string theField, decimal theValue, SQLCompare theOperator, SQLJoin theJoin)
        {
            string newWhere;
            string newClause = "";
            string newValue;

            // Clean the New Value
            newValue = Convert.ToString(theValue);

            // Get the new Clause 
            newClause = FieldKeywordMatching(theField, newValue, theOperator);

            // The New Where 
            newWhere = WhereFromWhere(theWhere, newClause, theJoin);

            // return the where clause.....
            return newWhere;
        } 

        
        /// <summary>
		/// Construct the Where for strings with SQLNulls Functionality..
		/// </summary>
		/// <param name="theWhere"></param>
		/// <param name="theField"></param>
		/// <param name="theOperator"></param>
		/// <param name="theJoin"></param>
		/// <returns></returns>
		public static string ConstructWhere(string theWhere, string theField, SQLNulls theOperator, SQLJoin theJoin) 
		{ 
			string newWhere; 
			string newClause = "";
 
			if ((theOperator == SQLNulls.isNull)) 
			{ 
				newClause = "(" + theField + " IS NULL )"; 
			} 
			else if ((theOperator == SQLNulls.isNotNull)) 
			{ 
				newClause = "(" + theField + " IS NOT NULL )"; 
			} 

			// The New Where 
			newWhere =  WhereFromWhere(theWhere, newClause, theJoin);

			// return the Where Clause.
			return newWhere; 
		} 

		/// <summary>
		/// Construct a where clause using subquery
		/// </summary>
		/// <param name="theWhere"></param>
		/// <param name="theField"></param>
		/// <param name="theValue"></param>
		/// <param name="theOperator"></param>
		/// <param name="theJoin"></param>
		/// <returns></returns>
		public static string ConstructWhere(string theWhere, string theField, string theValue, SQLSubQuery theOperator, SQLJoin theJoin) 
		{ 
			string newWhere; 
			string newClause = ""; 
			string newValue;
 
			// Clean the New Value.
			// newValue = MaskSpecialCharacters(theValue);
            newValue = theValue;

            // Where Caluse.
            if (newValue == "")
            {
                return theWhere;
            }

            // The Operator.
			if ((theOperator == SQLSubQuery.isIn)) 
			{ 
				newClause = "(" + theField + " IN (" + newValue + "))"; 
			} 
			else if ((theOperator == SQLSubQuery.isNotIn)) 
			{ 
				newClause = "(" + theField + " NOT IN (" + newValue + "))"; 
			} 

			// The New Where 
			newWhere =  WhereFromWhere(theWhere, newClause, theJoin);
		
			// return the Where Clause.
			return newWhere; 
		}

		/// <summary>
		/// Adding a new where componenet to an existing where clause
		/// </summary>
		private static string WhereFromWhere(string theWhere, string newClause, SQLJoin theJoin)
		{
			string newWhere = "";

			if (theWhere == "") 
			{ 
				if(newClause == "")
				{
					newWhere = "";
				}
				else
				{
					newWhere = SurroundString(newClause);
				}
			} 
			else 
			{ 
				if(newClause == "")
				{
					newWhere = SurroundString(theWhere);
				}
				else
				{
					if (theJoin == SQLJoin.jwAND)
					{
                        newWhere = SurroundString(theWhere) + " AND " + SurroundString(newClause); 
					} 
					else if (theJoin == SQLJoin.jwOR)
					{
                        newWhere = SurroundString(theWhere) + " OR " + SurroundString(newClause); 
					} 
				}
			}

			// Return the Where Clause.
			return newWhere;
		}

	#endregion 

	#region "Perfom and Extensive search - Many Values against many Fields"
	
	/// <summary>
	/// This function will return the where clause of a serch sql statement. The function 
	/// will search for each token (word) in pkeyWords in each of the field found in 
	/// pfieldsLis.
	/// - theWhere is the existing where caluse you want to add to.
	/// - theFields is the array of fields we need to look into.
	/// - theKeyWords is the array of values to match to.
	/// - inJoin, outJoin is either "AND" or "OR". "AND" will return all records containing EVERY 
	///   word and "OR" return all records containing ANY of the word.
	/// - theOperator is the operator to be used in the comparison.
	/// - lp indicates if the field are to loop against the field or vice-verca.
	/// </summary>
	/// <param name="theWhere"></param>
	/// <param name="theFields"></param>
	/// <param name="theKeyWords"></param>
	/// <param name="inJoin"></param>
	/// <param name="outJoin"></param>
	/// <param name="theOperator"></param>
	/// <returns></returns>
	public static string ExtensiveSearch(string theWhere, string[] theFields, string[] theKeyWords, SQLJoin inJoin, SQLJoin outJoin, SQLCompare theOperator) 
	{ 
		string newWhere; 
		string finalWhere;
 
		// Build the New Where Clause.
		newWhere = loopAround(theFields, theKeyWords, inJoin, outJoin, theOperator, LoopMaster.Fields); 
		
		// Finalize the Where Clause.
		finalWhere = WhereFromWhere(theWhere, newWhere, SQLJoin.jwAND); 

		// return the values
		return finalWhere; 
	} 

	
	public static string ExtensiveSearch(string theWhere, string[] theFields, string[] theKeyWords, SQLJoin inJoin, SQLJoin outJoin, SQLCompare theOperator, LoopMaster lp) 
	{ 
		string newWhere; 
		string finalWhere;
 
		// Build the New Where Clause.
		newWhere = loopAround(theFields, theKeyWords, inJoin, outJoin, theOperator, lp); 


		// Finalize the Where Clause.
		finalWhere = WhereFromWhere(theWhere, newWhere, SQLJoin.jwAND); 
		
		// return the string 
		return finalWhere; 
	} 

	/// <summary>
	/// Loop around and update the matching structure ....
	/// We have two form: Fields around Values OR Values around Fields.
	/// </summary>
	/// <param name="theFields"></param>
	/// <param name="theKeyWords"></param>
	/// <param name="inJoin"></param>
	/// <param name="outJoin"></param>
	/// <param name="theOperator"></param>
	/// <param name="lp"></param>
	/// <returns></returns>
	private static string loopAround(string[] theFields, string[] theKeyWords, SQLJoin inJoin, SQLJoin outJoin, SQLCompare theOperator, LoopMaster lp) 
	{ 
		string newWhere; 
		string fieldWhere; 
		string compareStr; 
		newWhere = ""; 
		fieldWhere = ""; 

		if (lp == LoopMaster.Fields)
		{ 
			foreach (string fieldName in theFields) 
			{ 
				fieldWhere = ""; 
	
				foreach (string keyWord in theKeyWords) 
				{ 
					compareStr = FieldKeywordMatching(fieldName, keyWord, theOperator); 
			
					if (compareStr != "") 
					{ 
						fieldWhere += compareStr; 
						
						if ((inJoin == SQLJoin.jwOR)) 
						{ 
							fieldWhere += " OR "; 
						}
						else if (inJoin == SQLJoin.jwAND)
						{ 
							fieldWhere += " AND "; 
						} 
					} 
				}
 
				fieldWhere = fieldWhere.Trim(); 
				
				if (fieldWhere != "")
				{ 
					fieldWhere = fieldWhere.Substring(0, fieldWhere.LastIndexOf(" ")).Trim(); 
				} 

				if (fieldWhere != "")
				{ 
					newWhere += "(" + fieldWhere + ")"; 
					
					if (outJoin == SQLJoin.jwOR) 
					{ 
						newWhere += " OR "; 
					} 
					else if (outJoin == SQLJoin.jwAND)
					{ 
						newWhere += " AND "; 
					} 
				} 
			} 
		} 
		else if (lp == LoopMaster.Keywords)
		{ 
			foreach (string keyWord in theKeyWords) 
			{ 
				fieldWhere = ""; 

				foreach (string fieldName in theFields) 
				{ 
					compareStr = FieldKeywordMatching(fieldName, keyWord, theOperator); 
					
					if (compareStr != "")
					{ 
						fieldWhere += compareStr; 
						
						if (inJoin == SQLJoin.jwOR)
						{ 
							fieldWhere += " OR "; 
						} 
						else if (inJoin == SQLJoin.jwAND)
						{ 
							fieldWhere += " AND "; 
						} 
					} 
				} 

				fieldWhere = fieldWhere.Trim(); 
	
				if (fieldWhere != "")
				{ 
					fieldWhere = fieldWhere.Substring(0, fieldWhere.LastIndexOf(" ")).Trim(); 
				} 

				if (fieldWhere != "")
				{ 
					newWhere += SurroundString(fieldWhere); 
					
					if (outJoin == SQLJoin.jwOR)
					{ 
						newWhere += " OR "; 
					} 
					else if (outJoin == SQLJoin.jwAND)
					{ 
						newWhere += " AND "; 
					} 
				} 
			} 
		}
 
		newWhere = newWhere.Trim(); 
	
		if (newWhere != "")
		{ 
			newWhere = newWhere.Substring(0, newWhere.LastIndexOf(" ")).Trim(); 
		}
 
		// Return the string 
		return newWhere; 
	} 

	/// <summary>
	/// Build a coparison component.
	/// </summary>
	/// <param name="fieldName"></param>
	/// <param name="keyWord"></param>
	/// <param name="theOperator"></param>
	/// <returns></returns>
	private static string FieldKeywordMatching(string fieldName, string keyWord, SQLCompare theOperator) 
	{ 
		string compareStr = ""; 
		fieldName = fieldName.Trim(); 
		keyWord = keyWord.Trim();
 
		if ((fieldName == "") || (keyWord == "")) 
		{ 
			compareStr = ""; 
		} 
		else 
		{ 
			keyWord = MaskSpecialCharacters(keyWord); 

			if (theOperator == SQLCompare.eqAsStrings)
			{ 
				compareStr += "(" + fieldName + " = '" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.eqAsNumbers)
			{ 
				compareStr += "(" + fieldName + " = " + keyWord + ")"; 
			}
            else if (theOperator == SQLCompare.diffAsNumbers)
            {
                compareStr += "(" + fieldName + " <> " + keyWord + ")";
            }
            else if (theOperator == SQLCompare.diffAsStrings)
            {
                compareStr += "(" + fieldName + " <> '" + keyWord + "')"; 
            } 
			else if (theOperator == SQLCompare.gtAsNumbers)
			{ 
				compareStr += "(" + fieldName + " > " + keyWord + ")"; 
			} 
			else if (theOperator == SQLCompare.gtAsStrings)
			{ 
				compareStr += "(" + fieldName + " > '" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.gteAsNumbers)
			{ 
				compareStr += "(" + fieldName + " >= " + keyWord + ")"; 
			} 
			else if (theOperator == SQLCompare.gteAsStrings)
			{ 
				compareStr += "(" + fieldName + " >= '" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.ltAsNumbers)
			{ 
				compareStr += "(" + fieldName + " < " + keyWord + ")"; 
			} 
			else if (theOperator == SQLCompare.ltAsStrings)
			{ 
				compareStr += "(" + fieldName + " < '" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.lteAsNumbers)
			{ 
				compareStr += "(" + fieldName + " <= " + keyWord + ")"; 
			} 
			else if (theOperator == SQLCompare.lteAsStrings)
			{ 
				compareStr += "(" + fieldName + " <= '" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.startWith)
			{ 
				compareStr += "(" + fieldName + " LIKE '" + keyWord + "%')"; 
			} 
			else if (theOperator == SQLCompare.endWith)
			{ 
				compareStr += "(" + fieldName + " LIKE '%" + keyWord + "')"; 
			} 
			else if (theOperator == SQLCompare.contains)
			{ 
				compareStr += "(" + fieldName + " LIKE '%" + keyWord + "%')"; 
			} 
			else if ((theOperator == SQLCompare.fillMode)) 
			{ 
				compareStr += "(" + fieldName + " LIKE '%" + keyWord.Replace(" ", "%") + "%')"; 
			} 
			else 
			{ 
				compareStr += "(" + fieldName + " LIKE '%" + keyWord + "%')"; 
			} 
		}
			
		// return the Query 
		return compareStr; 
	}

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="pkeyWords"></param>
    /// <param name="pfieldsList"></param>
    /// <param name="pjoinOperator"></param>
    /// <param name="pcompareOperator"></param>
    /// <param name="pWhere"></param>
    /// <returns></returns>
    public static string ExhaustiveSearch(String pkeyWords, String pfieldsList, String pjoinOperator, String pcompareOperator, String pWhere)
    {
        string sqlStr;
        string[] fieldsList;
        string[] keyWords;

        pjoinOperator = pjoinOperator.ToUpper().Trim();
        pcompareOperator = pcompareOperator.Trim();
        pkeyWords = pkeyWords.Replace("  ", " ").Trim();
        pfieldsList = pfieldsList.Replace(" ", "");
        pfieldsList = pfieldsList.Replace(";;", ";");

        pkeyWords = MaskSpecialCharacters(pkeyWords);

        char[] delimiterChars = { ' ' };
        char[] delimiterChars2 = { ';' };

        keyWords = pkeyWords.Split(delimiterChars);
        fieldsList = pfieldsList.Split(delimiterChars2);

        sqlStr = "";
        string strField;
        string strKeyWord;

        for (int i = 0; i < fieldsList.Length; i++)
        {
            strField = Convert.ToString(fieldsList[i]).Trim();
            if (strField != "")
            {
                for (int j = 0; j < keyWords.Length; j++)
                {
                    strKeyWord = Convert.ToString(keyWords[j]).Trim();

                    if (strKeyWord != "")
                    {
                        if (pcompareOperator == "eq")
                        {
                            sqlStr += "(" + strField + " = '" + strKeyWord + "')";
                        }
                        else if (pcompareOperator == "fil")
                        {
                            sqlStr += "(" + strField + " LIKE '%" + strKeyWord.Replace(" ", "%") + "%')";

                            if (pjoinOperator == "OR")
                            {
                                sqlStr = sqlStr + " OR ";
                            }
                            else if (pjoinOperator == "AND")
                            {
                                sqlStr = sqlStr + " AND ";
                            }
                        }
                        else if (pcompareOperator == "lsim")
                        {
                            sqlStr += "(" + strField + " LIKE '" + strKeyWord + "%')";
                        }
                        else if (pcompareOperator == "rsim")
                        {
                            sqlStr += "(" + strField + " LIKE '%" + strKeyWord + "')";
                        }
                        else
                        {
                            sqlStr += "(" + strField + " LIKE '%" + strKeyWord + "%')";
                        }

                        if (pjoinOperator == "OR")
                        {
                            sqlStr = sqlStr + " OR ";
                        }
                        else if (pjoinOperator == "AND")
                        {
                            sqlStr = sqlStr + " AND ";
                        }
                    }
                }

                if (sqlStr.Length > 4)
                {
                    sqlStr = sqlStr.Substring(0, sqlStr.Length - 4) + " OR ";
                }
            }
        }

        if (pWhere == "")
        {
            if (sqlStr == "")
            {
                return "";
            }
            else
            {
                return "(" + sqlStr.Substring(0, sqlStr.Length - 5) + ")";
            }
        }
        else
        {
            if (sqlStr == "")
            {
                return pWhere;
            }
            else
            {
                return pWhere + " AND (" + sqlStr.Substring(0, sqlStr.Length - 5) + ")";
            }
        }
    }

	#endregion

	#region "Construct the final query from the SELECT, WHERE, ...."

	/// <summary>
	/// Function Needed To build the final SQL after you have made your 
	/// SELECT, WHERE, GROUPBY, HAVING, ORDERBY
	/// Different versions are available here.
	/// </summary>
	/// <param name="theSelect"></param>
	/// <param name="theWhere"></param>
	/// <returns></returns>
    public static string ConstructQuery(string theSelect, string theWhere) 
	{ 
		string theSQLStr; 
		theSQLStr = theSelect.Trim(); 

		if (theWhere != "") 
		{ 
			theSQLStr += " WHERE " + SurroundString(theWhere) + " "; 
		}
 
		// return the string 
		return theSQLStr; 
	} 

	/// <summary>
	///	 Another version .....
	/// </summary>
	/// <param name="theSelect"></param>
	/// <param name="theWhere"></param>
	/// <param name="theOrderBy"></param>
	/// <returns></returns>
	public static string ConstructQuery(string theSelect, string theWhere, string theOrderBy) 
	{ 
		string theSQLStr; 
		theSQLStr = theSelect.Trim(); 

		if (theWhere != "") 
		{ 
			theSQLStr += " WHERE " + SurroundString(theWhere) + " ";
		} 
		
		if (theOrderBy != "") 
		{ 
			theSQLStr += " ORDER BY " + theOrderBy + " "; 
		} 

		// return the string 
		return theSQLStr; 
	} 

	/// <summary>
	/// And Another Version
	/// </summary>
	/// <param name="theSelect"></param>
	/// <param name="theWhere"></param>
	/// <param name="theOrderBy"></param>
	/// <param name="theGroupBy"></param>
	/// <param name="theHaving"></param>
	/// <returns></returns>
	public static string ConstructQuery(string theSelect, string theWhere, string theOrderBy, string theGroupBy, string theHaving) 
	{ 
		string theSQLStr; 
		theSQLStr = theSelect.Trim(); 

		if (theWhere != "") 
		{ 
			theSQLStr += " WHERE " + SurroundString(theWhere) + " "; 
		}
 
		if ((theGroupBy != "")) 
		{ 
			theSQLStr += " GROUP BY " + theGroupBy + " "; 
		} 

		if (theHaving != "")
		{ 
			theSQLStr += " HAVING " + SurroundString(theHaving) + " "; 
		} 

		if (theOrderBy != "")
		{ 
			theSQLStr += " ORDER BY " + theOrderBy + " "; 
		}
		
		//return the value
		return theSQLStr; 
	}

	#endregion 

	#region "Build a direct Stored Procedure Call"

		/// <summary>
		/// Assemble the parameter.
		/// </summary>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="dm"></param>
		/// <returns></returns>
		private static string AssembleParameters(string varName, string varValue, DataMode dm)
		{
			string paramStr; 

			// Clean the Value 
			varValue = ALCS_DataShift.WhenNull(varValue, "");

			// Start the New Parameter
			paramStr = varName + " = ";

			// Add the Value 
			if(varValue == "")
			{
				paramStr += "NULL";
			}
			else
			{
				varValue = MaskSpecialCharacters(varValue);

				if(dm == DataMode.Number)
				{
					paramStr += varValue;
				}
				else if(dm == DataMode.String)
				{
					paramStr += "'" + varValue + "'";
				}
				else
				{
					paramStr += "'" + varValue + "'";
				}
			}

			// Return the Parameter 
			return paramStr; 
		}

		/// <summary>
		/// Assemble the parameter.
		/// </summary>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		private static string AssembleParameters(string varName, int varValue, int nullValue)
		{
			string paramStr; 

			// Start the New Parameter
			paramStr = varName + " = ";

			// Add the Value 
			if(varValue == nullValue)
			{
				paramStr += "NULL";
			}
			else
			{
				paramStr += Convert.ToString(varValue);
			}

			// Return the Parameter 
			return paramStr; 
		}


		/// <summary>
		/// Assemble the parameter.
		/// </summary>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		private static string AssembleParameters(string varName, decimal varValue, decimal nullValue)
		{
			string paramStr; 

			// Start the New Parameter
			paramStr = varName + " = ";

			// Add the Value 
			if(varValue == nullValue)
			{
				paramStr += "NULL";
			}
			else
			{
				paramStr += Convert.ToString(varValue);
			}

			// Return the Parameter 
			return paramStr; 
		}

		/// <summary>
		/// Just append the parameter ...
		/// </summary>
		/// <param name="spCall"></param>
		/// <param name="paramStr"></param>
		private static string AppendParameter(string spCall, string paramStr)
		{
			// Update the stored procedure call 
			if(spCall.IndexOf(@"@")>0)
			{
				return spCall += ", " + paramStr; 
			}
			else
			{
				return spCall += " " + paramStr;
			}
		}

		/// <summary>
		/// Append a parameter to the stored procedure call ....
		/// </summary>
		/// <param name="spCall"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="dm"></param>
		/// <returns></returns>
		public static string SPAppendParameter(string spCall, string varName, string varValue, DataMode dm)
		{
			string paramStr = AssembleParameters(varName, varValue, dm);  
			
			// Return
			return AppendParameter(spCall, paramStr);
		}

		/// <summary>
		/// Another verion to handle integer value.
		/// </summary>
		/// <param name="spCall"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		public static   string SPAppendParameter(string spCall, string varName, int varValue, int nullValue)
		{
			string paramStr = AssembleParameters(varName, varValue, nullValue);  
			
			// Return
			return AppendParameter(spCall, paramStr);
		}

		/// <summary>
		/// Another verion to handle decimal value.
		/// </summary>
		/// <param name="spCall"></param>
		/// <param name="varName"></param>
		/// <param name="varValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		public static   string SPAppendParameter(string spCall, string varName, decimal varValue, decimal nullValue)
		{
			string paramStr = AssembleParameters(varName, varValue, nullValue);  
			
			// Return
			return AppendParameter(spCall, paramStr);
		}


	#endregion 

	#region "Build an direct update query ...."


		/// <summary>
		///	 Append a field to the update query.
		/// </summary>
		/// <param name="upSQL"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		/// <param name="dm"></param>
		/// <returns></returns>
		public static string AppendFieldUpdate(string upSQL, string fieldName, string fieldValue, DataMode dm)
		{
			string upStr = AssembleParameters(fieldName, fieldValue, dm);  
			
			// Return
			return AppendUpdate(upSQL, upStr);
		}

		/// <summary>
		/// Another verion to handle integer value.
		/// </summary>
		/// <param name="upSQL"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		public static   string AppendFieldUpdate(string upSQL, string fieldName, int fieldValue, int nullValue)
		{
			string upStr = AssembleParameters(fieldName, fieldValue, nullValue);  
			
			// Return
			return AppendUpdate(upSQL, upStr);
		}

		/// <summary>
		/// Another verion to handle decimal value.
		/// </summary>
		/// <param name="upSQL"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		/// <param name="nullValue"></param>
		/// <returns></returns>
		public static   string AppendFieldUpdate(string upSQL, string fieldName, decimal fieldValue, decimal nullValue)
		{
			string upStr = AssembleParameters(fieldName, fieldValue, nullValue);  
			
			// Return
			return AppendUpdate(upSQL, upStr);
		}

		/// <summary>
		/// append the field update to the current query ....
		/// </summary>
		/// <param name="upSQL"></param>
		/// <param name="upStr"></param>
		/// <returns></returns>
		public static string AppendUpdate(string upSQL, string upStr)
		{
			// Update the stored procedure call 
			if(upSQL.IndexOf(@" SET ")>0)
			{
				return upSQL += ", " + upStr; 
			}
			else
			{
				return upSQL += " SET " + upStr;
			}
		}

		#endregion 
	
	#region "Slave functions"

		
		/// <summary>
		/// Mask special characters that have specila meaning for SQL.
		/// </summary>
		/// <param name="theValue"></param>
		/// <returns></returns>
		public static string MaskSpecialCharacters(string theValue) 
		{ 
			string newStr; 
			newStr = ALCS_DataShift.WhenNull(theValue, "").Trim(); 
			if ((newStr != "") && (!newStr.StartsWith("dbo."))) 
			{ 
				newStr = newStr.Replace("'", "''"); 
				newStr = newStr.Replace("%", "[%]"); 
				newStr = newStr.Replace("_", "[_]"); 
			} 
			return newStr; 
		}

        /// <summary>
        /// Add escape charatcter to single quote.
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static string MakeQuoteSafe(string theValue)
        {
            string newStr;
            newStr = ALCS_DataShift.WhenNull(theValue, "").Trim();
            if ((newStr != "") && (!newStr.StartsWith("dbo.")))
            {
                newStr = newStr.Replace("'", "''");
            }
            return newStr;
        } 


		/// <summary>
		/// see below. default to ().
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static string SurroundString(string inStr)
		{
			return SurroundString(inStr, "(", ")");
		}

		/// <summary>
		/// suurround the passed string with the specified beginning and ending unless 
		/// it is already surrounded.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="bStr"></param>
		/// <param name="eStr"></param>
		/// <returns></returns>
		private static string SurroundString(string inStr, string bStr, string eStr)
		{
			string newStr;
            bool wrapStr = false;
			
			newStr = ALCS_DataShift.WhenNull(inStr,"");
   
			// check for the surrounding
            if (newStr != "")
            {
                if ((!newStr.StartsWith(bStr)) || (!newStr.EndsWith(eStr)))
                {
                    wrapStr = true;
                }
                else if ((newStr.Contains(" AND ")) || (newStr.Contains(" OR ")))
                {
                    wrapStr = true; 
                }
            }

            // should we surround the string 
            if (wrapStr)
            {
                newStr = bStr + newStr + eStr;
            }

			// return
			return newStr;
		}

	#endregion 

   

  }//Class 
}
