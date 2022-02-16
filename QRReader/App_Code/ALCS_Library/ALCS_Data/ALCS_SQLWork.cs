using System;
using System.Data;
using System.Collections; 
using System.Data.SqlClient;
using ALCS_Library.ALCS_Data;  


///////////////////////////////////////////////////////////////////
/// Handle all SQL Connection and Data Pulling
/// ///////////////////////////////////////////////////////////////
namespace ALCS_Library.ALCS_Data.ALCS_SQLWork
{
	#region "Handle All connection Business Queries ..."

	/// <summary>
	/// Handle SQL Connections
	/// 1. Create and open a connection.
	/// 2. Destroy a connection.
	/// </summary>
	public class ALCS_SQLConnection
	{
		// Properties 
		private bool health=true;
		private string memo="";
		private string dbSource;
		private string connectionStr;
		
		/// <summary>
        /// default 
		/// </summary>
		public ALCS_SQLConnection()
		{
            this.dbSource = ALCS_DB.System_DataBase;

			// Set Connection 
			SetConnectionStr();
		}

		/// <summary>
		/// specify the DB
		/// </summary>
		public ALCS_SQLConnection(string dbName)
		{
			this.dbSource = dbName;

			// Set connection 
			SetConnectionStr();
		}

		/// <summary>
		/// Set the Connection string for the object.
		/// </summary>
		private void SetConnectionStr()
		{
			this.connectionStr =  ALCS_ConfigReader.GetConnectionStr(this.dbSource);
		}

		/// <summary>
		/// Open the connection and record acrivity
		/// </summary>
		/// <param name="cn"></param>
		public void OpenConnection(ref SqlConnection cn)
		{
			//create 
			cn = new SqlConnection(this.ConnectionString);

			// open
			try
			{
				cn.Open(); 
			}
			catch (SqlException	sx)
			{
				this.health = false;
				this.memo = sx.Message ;
			}
			catch(Exception  ex)
			{
				this.health = false;
				this.memo = ex.Message ;
			}
		}

		/// <summary>
		/// close the connection if its open
		/// </summary>
		/// <param name="cn"></param>
		public void CloseConnection(ref SqlConnection cn)
		{
			if(cn.State == ConnectionState.Open)
			{
				cn.Close();
				cn.Dispose(); 
			}
		}

		/// <summary>
		///  return the health of the object 
		/// </summary>
		public bool isHealthy
		{
			get
			{
				return this.health;
			}
		}

		/// <summary>
		///  return the error message if any.
		/// </summary>
		public string Memo
		{
			get
			{
				return this.memo; 
			}
		}

		/// <summary>
		/// Expose the Connection string 
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return this.connectionStr; 
			}
		}
	}

	#endregion

	#region "Excecute queries and return the result in a a collection"
	/// <summary>
	/// turn a query into something usable in ASP.
	/// </summary>
	public class ALCS_QueryExecuter
	{
		private bool qe_health = true; 
		private string qe_memo; 
		private string qe_DB;
        private bool HasRecords = false;

		/// <summary>
		///  Construct with default values.
		/// </summary>
		public ALCS_QueryExecuter() 
		{ 
			this.qe_health = true; 
			this.qe_memo = "";
            this.qe_DB = ALCS_DB.System_DataBase;
		} 

		/// <summary>
		/// Construct with nominated DB.
		/// </summary>
		/// <param name="theDB"></param>
		public ALCS_QueryExecuter(string theDB) 
		{ 
			this.qe_health = true; 
			this.qe_memo = ""; 
			this.qe_DB  = theDB; 
		}

        /// <summary>
        /// Run the query and return details in a hash table.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public Hashtable SelectSingleRow(string sqlStr)
        {
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;
            SqlDataReader dr;
            Hashtable selResult = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            string fieldName;

            // open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                return null;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                return null;
            }

            // If we make here then we are OK.
            if (!dr.HasRows)
            {
                this.HasRecords = false;
                return null;
            }
            else
            {
                this.HasRecords = true;
                dr.Read();
                for (int idx = 0; idx < dr.FieldCount; idx++)
                {
                    fieldName = dr.GetName(idx).ToUpper();
                    selResult.Add(fieldName, dr[fieldName]);
                }

                // Is There More Rows?
                if (dr.Read())
                {
                    selResult.Clear();
                    this.qe_health = false;
                    this.qe_memo = "The query returned more than one records.";
                    return null;
                }
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                dr.Close();
                sqlCon.CloseConnection(ref cn);
            }
            return selResult;
        } 

		/// <summary>
		/// Extract a single scalar value returned by the query. 
		/// </summary>
		/// <param name="sqlStr"></param>
		public object ExtractScalar(string sqlStr) 
		{ 
			ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB); 
			SqlConnection cn = new SqlConnection();  
			SqlCommand cm;
			Object obj = null;
			
			// open the connection.
			sqlCon.OpenConnection(ref cn);
			if (!sqlCon.isHealthy)  
			{ 
				this.qe_health = false; 
				this.qe_memo =   sqlCon.Memo; 
				cn.Dispose(); 
				return obj;
			} 

			// Set the Connection object.
			cm = new SqlCommand(); 
			cm.Connection = cn; 
			cm.CommandText = sqlStr; 
			cm.CommandType = CommandType.Text; 
			try 
			{ 
				obj = (Object) cm.ExecuteScalar(); 
			} 
			catch (Exception ex) 
			{ 
				this.qe_health = false; 
				this.qe_memo = ex.Message; 
				if ((cn.State == ConnectionState.Open)) 
				{ 
					cm.Cancel(); 
					cm.Dispose(); 
					sqlCon.CloseConnection(ref cn); 
				} 
				return obj; 
			}

			// Clear the connection 
			if(cn.State  == ConnectionState.Open)
			{
				cm.Cancel(); 
				cm.Dispose(); 
				sqlCon.CloseConnection(ref cn); 
			}

			// Return the Object 
			return obj;

		}

        /// <summary>
        /// Extract a single integer value returned by the query. 
        /// </summary>
        /// <param name="sqlStr"></param>
        public bool ExtractInteger(string sqlStr, out int theValue)
        {
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                theValue = -1;
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = (Int32)cm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                theValue = -1;
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        }


        /// <summary>
        /// Extract a single decimal value returned by the query. 
        /// </summary>
        /// <param name="sqlStr"></param>
        public bool ExtractDecimal(string sqlStr, out decimal theValue)
        {
            // Default Value ...
            theValue = 0.0m;

            // Run SQL Query ...
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // Open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = (Decimal)cm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        }

        /// <summary>
        /// Extract a double value rom the DB.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public bool ExtractDouble(string sqlStr, out double theValue)
        {
            // Default Value ...
            theValue = 0.0d;

            // Run SQL Query ...
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // Open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = (Double)cm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        }

        /// <summary>
        /// Extract a single Boolean value returned by the query. 
        /// </summary>
        /// <param name="sqlStr"></param>
        public bool ExtractBool(string sqlStr, out bool theValue)
        {
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                theValue = false;
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = Convert.ToBoolean(cm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                theValue = false;
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        }

        /// <summary>
        /// Extract a single integer value returned by the query. 
        /// </summary>
        /// <param name="sqlStr"></param>
        public bool ExtractString(string sqlStr, out string theValue)
        {
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                theValue = "";
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = Convert.ToString(cm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                theValue = "";
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        } 

        /// <summary>
        /// Extract a date ...
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public bool ExtractDate(string sqlStr, out DateTime theValue)
        {
            ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm;

            // open the connection.
            sqlCon.OpenConnection(ref cn);
            if (!sqlCon.isHealthy)
            {
                this.qe_health = false;
                this.qe_memo = sqlCon.Memo;
                cn.Dispose();
                theValue = DateTime.MaxValue;
                return false;
            }

            // Set the Connection object.
            cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandText = sqlStr;
            cm.CommandType = CommandType.Text;
            try
            {
                theValue = Convert.ToDateTime(cm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                this.qe_health = false;
                this.qe_memo = ex.Message;
                if ((cn.State == ConnectionState.Open))
                {
                    cm.Cancel();
                    cm.Dispose();
                    sqlCon.CloseConnection(ref cn);
                }
                theValue = DateTime.MaxValue;
                return false;
            }

            // Clear the connection 
            if (cn.State == ConnectionState.Open)
            {
                cm.Cancel();
                cm.Dispose();
                sqlCon.CloseConnection(ref cn);
            }

            // Return the Object 
            return true;
        }


		/// <summary>
		/// A Silent Execute that is an action query with no returmed set ....
		/// </summary>
		/// <param name="sqlStr"></param>
		public void SilentExecute(string sqlStr) 
		{ 
			ALCS_SQLConnection sqlCon = new ALCS_SQLConnection(this.qe_DB); 
			SqlConnection cn = new SqlConnection();  
			SqlCommand cm; 
			
			// open the connection.
			sqlCon.OpenConnection(ref cn);
			if (!sqlCon.isHealthy)  
			{ 
				this.qe_health = false; 
				this.qe_memo =   sqlCon.Memo; 
				cn.Dispose(); 
				return;
			} 

			// Set the Connection object.
			cm = new SqlCommand(); 
			cm.Connection = cn; 
			cm.CommandText = sqlStr; 
			cm.CommandType = CommandType.Text; 
			try 
			{ 
				cm.ExecuteNonQuery(); 
			} 
			catch (Exception ex) 
			{ 
				this.qe_health = false; 
				this.qe_memo = ex.Message; 
				if ((cn.State == ConnectionState.Open)) 
				{ 
					cm.Cancel(); 
					cm.Dispose(); 
					sqlCon.CloseConnection(ref cn); 
				} 
				return; 
			}

			// Clear the connection 
			if(cn.State  == ConnectionState.Open)
			{
				cm.Cancel(); 
				cm.Dispose(); 
				sqlCon.CloseConnection(ref cn); 
			}
		}

        /// <summary>
        /// Simple overwrite.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="ds"></param>
        public void LoadDataSet(string sqlStr, ref DataSet ds)
        {
            LoadDataSet(sqlStr, ref ds, -1);
        }

        /// <summary>
        /// Execute the query and load the result in the data set.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dr"></param>
		public void LoadDataSet(string sqlStr, ref DataSet ds, int cmTimeout)
		{
			string cnStr = ALCS_ConfigReader.GetConnectionStr(this.qe_DB);

			SqlDataAdapter da = new SqlDataAdapter(sqlStr, cnStr); 

			// Create a dataset if you have not already did.
			if(ds == null)
			{
				ds = new DataSet(); 
			}
			
			try 
			{
                if (cmTimeout > 0)
                {
                    da.SelectCommand.CommandTimeout = cmTimeout;
                }

				da.Fill(ds);
			} 
			catch (Exception ex) 
			{ 
				this.qe_health = false; 
				this.qe_memo = ex.Message;
                return;
			}

            // Do we have records 
            if (ds.Tables.Count == 0)
            {
                this.HasRecords = false;
            }
            else if (ds.Tables[0].Rows.Count == 0)
            {
                this.HasRecords = false;
            }
            else
            {
                this.HasRecords = true;
            }
		}

        /// <summary>
        /// Another version of the load table where the default is 30 seconds.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dt"></param>
        public void LoadDataTable(string sqlStr, ref DataTable dt)
        {
            LoadDataTable(sqlStr, ref dt, -1);
        }

        /// <summary>
        /// If the Timeout is >= 0 then extend the command timeout.
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dt"></param>
        /// <param name="timeOut"></param>
		public void LoadDataTable(string sqlStr, ref DataTable dt, int timeOut)
		{
			string cnStr = ALCS_ConfigReader.GetConnectionStr(this.qe_DB); 
			SqlDataAdapter da = new SqlDataAdapter(sqlStr, cnStr); 

			// Create a dataset if you have not already did.
			try 
			{
                if (timeOut >= 0)
                {
                    da.SelectCommand.CommandTimeout = 0;
                }
				da.Fill(dt);
			} 
			catch (Exception ex) 
			{ 
				this.qe_health = false; 
				this.qe_memo = ex.Message;
				return;
			}

            // Do we have a record ..
            if (dt.Rows.Count == 0)
            {
                this.HasRecords = false;
            }
            else
            {
                this.HasRecords = true;
            }
		}

		/// <summary>
		/// Expose the health of the Object.
		/// </summary>
		/// <returns></returns>
		public bool isHealthy
		{
			get
			{
				return this.qe_health;
			}
		} 

		/// <summary>
		/// Expose the error message.
		/// </summary>
		/// <returns></returns>
		public string Memo 
		{ 
			get
			{
				return this.qe_memo; 
			}
		}

        /// <summary>
        /// return the records flag ...
        /// </summary>
        public bool _HasRecords
        {
            get
            {
                return this.HasRecords;
            }
        }
	}

	#endregion 

	#region "General Tools"

	/// <summary>
	/// General tools mainly relevant for the programmer for debugging purposes.
	/// </summary>
	public class ALCS_SQLTools
	{
		/// <summary>
		/// Build an SP Call from the command object ....
		/// </summary>
		/// <param name="cm"></param>
		/// <returns></returns>
		public static string BuildSPCallFromCommand(SqlCommand cm)
		{
			string spName;
			string theValue;

			spName = "EXEC " + cm.CommandText + "  ";

			foreach(SqlParameter para in cm.Parameters)
			{
                if ((para.SqlDbType == SqlDbType.VarChar) || (para.SqlDbType == SqlDbType.Char) || (para.SqlDbType == SqlDbType.Date))
				{
					theValue = ALCS_DataShift.WhenNull(para.Value, "");
					if(theValue == "")
					{
						spName += para.ParameterName + "=NULL, ";    
					}
					else
					{
						spName += para.ParameterName + "='" + theValue + "', ";    
					}
				}
				else
				{
					theValue = ALCS_DataShift.WhenNull(para.Value, "");
					if(theValue == "")
					{
						spName += para.ParameterName + "=NULL, ";
					}
					else
					{
						spName += para.ParameterName + "=" + theValue + ", ";
					}
				}
			}

			// Some cleaning 
			spName = spName.Trim();
			if(spName.EndsWith(","))
			{
				spName = spName.Substring(0,spName.Length -1); 
			}

			// exit 
			return spName;

		}

	}
	#endregion 
}
