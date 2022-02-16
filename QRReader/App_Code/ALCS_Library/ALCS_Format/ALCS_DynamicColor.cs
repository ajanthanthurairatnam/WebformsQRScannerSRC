using System;
using System.Collections;
using System.Xml.Serialization;
using System.Configuration;
using System.Xml;
using System.Text.RegularExpressions;  
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using ALCS_Library.ALCS_DynamicColor;

namespace ALCS_Library.ALCS_DynamicColor
{
	#region "Classes required for Dynamic color processing"

	/// <summary>
	/// Summary description for ColorSettingsHandler.
	/// </summary>
	public class ColorSettingsHandler : IConfigurationSectionHandler
	{
		private XmlSerializer _ser = new XmlSerializer (typeof (ColorSettings));

		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			ColorSettings cfg = (ColorSettings) _ser.Deserialize (new XmlNodeReader (section));
			return cfg;
		}
	}
	
	[XmlRoot("DynamicStyle")]
	public class ColorSettings
	{
		private ColorVar[] _colorVars;

		[XmlArray ("DynamicColors")]
		[XmlArrayItem ("colorVar", typeof(ColorVar))]
		public ColorVar[] ColorVars
		{
			get { return _colorVars; }
			set { _colorVars = value; }
		}
	}

	/// <summary>
	/// This class represents a color variable and its substituted code
	/// </summary>
	public class ColorVar
	{
		string _key;
		string _code;

		public ColorVar () {}

		[XmlAttribute ("colorKey")]
		public string Key
		{
			get { return _key; }
			set { _key = value; }
		}

		[XmlAttribute ("colorCode")]
		public string Code
		{
			get { return _code; }
			set { _code = value; }
		}
	}

	#endregion 

	#region "The actual style maker tool ..."

	public class StyleMaker
	{
		string styleFilter	= @"_map.css";
        string layoutFilter  = @"_style.xml";
		string rootDir		= System.Web.HttpContext.Current.Request.MapPath("~");
		Hashtable SysColors = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		ArrayList SysStyles	= new ArrayList();
        ArrayList SysLayout = new ArrayList();
        string dupKeys = "";
        bool dupFlag = false;

		bool health			= true;
		string memo			= "";

        /// <summary>
        /// Load the Color Scheme ....
        /// </summary>
		public StyleMaker()
		{
			LoadLists();
		}

		#region "The data loader - populating the list ..."
         
		/// <summary>
		/// Just load the color scheme and the list of styles to be processed.
		/// </summary>
		private void LoadLists()
		{
            // Extract the list of all color schemes ...
            SysLayout = DirNavigator(rootDir, "*" + layoutFilter);

            if (this.SysLayout.Count == 0)
            {
                health = false;
                memo = "The Color scheme is missing or badly formed.";
            }
            else
            {
                ReadLayoutParameters(ref this.SysColors);

                if (this.SysColors.Count == 0)
                {
                    health = false;
                    memo = "The Color scheme is missing or badly formed.";
                }
            }

            /////////////////////////////////////////////////////////
            //  Parse the styles one by one ...
            /////////////////////////////////////////////////////////
			if(this.isHealthy)
			{
				SysStyles	= DirNavigator(rootDir, "*" + styleFilter);

				if(SysStyles.Count == 0)
				{
					health	= false;
					memo	= "The site does not have any style matching the specified filter.";
				}
			}
		}

		/// <summary>
		/// Read all the colors from the configuration table and load them in a table.
		/// </summary>
		/// <returns></returns>
		public void LoadSysColors(ref Hashtable SysColors)
		{
            this.ReadLayoutParameters(ref SysColors);
		}

        #endregion 

        /// <summary>
        /// Scan all color Parameters and load them in the Hash Tables ...
        /// </summary>
        public void ReadLayoutParameters(ref Hashtable SysColors)
        {
            string layoutName;
            int idx = 0;

            if (!this.isHealthy)
            {
                return;
            }

            // Do we have something to do ...
            for (idx = 0; idx < this.SysLayout.Count; idx++)
            {
                layoutName = this.SysLayout[idx].ToString();
                ReadLayout(layoutName, ref SysColors);
            }

            // Any Duplicate?
            if (this.dupFlag)
            {
                this.health = false;
                this.memo = "The following keys are duplicated '" + this.dupKeys + "'. Your styles might nor process properly.";
            }
        }

        /// <summary>
        /// Read the layout parameter from the given file ...
        /// </summary>
        /// <param name="sName"></param>
        public void ReadLayout(string lName, ref Hashtable SysColors)
        {
            XmlTextReader xr;

            //Is the file there 
            if (!File.Exists(lName))
            {
                return;
            }

            // Create the XML reader
            try
            {
                xr = new XmlTextReader(lName);
            }
            catch (Exception ex)
            {
                this.health = false;
                this.memo = ex.Message;
                return;
            }

            // Load the Colors in the Hash ...
            while (xr.Read())
            {
                if (xr.NodeType.ToString().StartsWith("Element"))
                {
                    if (xr.Name.ToString().StartsWith("colorVar"))
                    {
                        string cCode = xr.GetAttribute("colorKey");
                        string cValue = xr.GetAttribute("colorCode");

                        if (SysColors.Contains(cCode))
                        {
                            dupFlag = true;
                            dupKeys += cCode + ";";
                        }
                        SysColors.Add(cCode, cValue);
                    }
                }
            }

            // Close the file 
            xr.Close();

        }

        /// <summary>
		/// Scan all styles in the site and make a coded copy.
		/// </summary>
		public void MakeSiteStyles()
		{
			string styleName;
			int idx = 0;

			if(!this.isHealthy) 
			{
				return;
			}

			// Do we have something to do ...
			for(idx=0; idx < this.SysStyles.Count; idx++)
			{
				styleName = this.SysStyles[idx].ToString();
	        	MakeStyle(styleName);
			}
		}

		/// <summary>
		/// read the file and generate a new one .....
		/// </summary>
		/// <param name="sName"></param>
		public void MakeStyle(string sName)
		{
			string styleText;
			string nstyleText;
			TextReader tr;
			TextWriter tw;
			
			//Is the file there 
			if(!File.Exists(sName))
			{
				return;
			}
			
			// Get the File Name and create the output ...
			FileInfo si			= new FileInfo(sName);
			string sPath		= Path.GetDirectoryName(sName);
			string sSep			= Path.DirectorySeparatorChar.ToString(); 
			string sTail		= Path.GetFileName(sName);
			string nsTail		= Regex.Replace(sTail, this.styleFilter, @".css", RegexOptions.IgnoreCase);     
			string nsName		= sPath + sSep + nsTail;

			// Create the reader
			try
			{
				tr = new StreamReader(sName);
			}
			catch(Exception ex)
			{
				this.health = false;
				this.memo	= ex.Message;
				return;
			}

			// if the file exists ....
			if(File.Exists(nsName))
			{
				FileInfo fi = new FileInfo(nsName);

				if((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)  
				{
					fi.Attributes = fi.Attributes ^ FileAttributes.ReadOnly;  
				}
			}

			// Create the writer ....
			try
			{
				tw = new StreamWriter(nsName);
			}
			catch(Exception ex)
			{
				this.health = false;
				this.memo	= ex.Message;
                tr.Close();
                tr.Dispose();
				return;
			}

			styleText	= tr.ReadToEnd();
			nstyleText	= this.SubstituteColor(styleText);   
			tw.WriteLine(nstyleText); 

			// Clear the area
            tw.Flush();
			tw.Close();
			tr.Close();
            tr.Dispose();
		}

		/// <summary>
		/// Substitute the color keys with their code;
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private string SubstituteColor(string inStr)
		{
			string css = inStr;
			string colorKey, colorCode;
			ICollection ic =  this.SysColors.Keys;
			
			foreach(Object o in ic)
			{
				colorKey	= o.ToString();
				colorCode	= this.SysColors[colorKey].ToString();
				Regex re = new Regex (string.Concat ("\\$\\b", colorKey.Substring(1), "\\b"), RegexOptions.IgnoreCase);
				css = re.Replace (css, colorCode);
			}

			// return the coded string.
			return css;
		}



		#region "The File system navigator ..."

		/// <summary>
		/// Navigate through the directories in the sytem and add the files to the main 
		/// list.
		/// </summary>
		/// <param name="dirName"></param>
		/// <param name="ffilter"></param>
		/// <returns></returns>
		public ArrayList DirNavigator(string dirName, string ffilter)
		{
			ArrayList al = new ArrayList(); 
			
			// Add The Files
			al = AddArray(al, DirList(dirName, ffilter));
   
			string[] dirList = Directory.GetDirectories(dirName);
 
			if(dirList.Length == 0)
			{
				return al;
			}
			else
			{
				foreach(string dir in dirList)
				{
					al = AddArray(al, DirNavigator(dir, ffilter)); 
				}
				return al;
			}
		}

		/// <summary>
		/// Load all files that match the filter to an array list and return it.
		/// </summary>
		/// <param name="dirName"></param>
		/// <param name="ffilter"></param>
		/// <returns></returns>
		public ArrayList DirList(string dirName, string ffilter)
		{
			ArrayList al = new ArrayList();
 
			if(Directory.Exists(dirName))
			{
				string[] dirFiles = Directory.GetFiles(dirName, ffilter); 
 
				foreach(string tok in dirFiles)
				{
					al.Add(tok); 
				}
			}
             
			// return the array
			return al;
		}

		/// <summary>
		/// Combine the two array list in one and return it.
		/// </summary>
		/// <param name="al1"></param>
		/// <param name="al2"></param>
		/// <returns></returns>
		public ArrayList AddArray(ArrayList al1, ArrayList al2)
		{
			ArrayList al = new ArrayList();
 
			if(al1 != null)
			{
				al.AddRange(al1); 
			}

			if(al2 != null)
			{
				al.AddRange(al2); 
			}

			// return 
			return al;
		}

		#endregion 

		#region "Expose the class properties ..."

		/// <summary>
		/// Expose the Health.
		/// </summary>
		public bool isHealthy
		{
			get
			{
				return this.health; 
			}
		}

		/// <summary>
		///	 expose the memo.
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
