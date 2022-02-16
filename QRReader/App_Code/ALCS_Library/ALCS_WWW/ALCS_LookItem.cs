using System;

namespace ALCS_Library.ALCS_WWW
{
	/// <summary>
	/// This is the base class used for databinding.
	/// </summary>
	#region "The Base LookupItem class"
	public class ALCS_lookItem
	{ 
		private string lookName; 
		private string lookID; 
		private int lookVis = 1; 

		public ALCS_lookItem(string theName, string theID) 
		{ 
			this.lookName = theName; 
			this.lookID = theID; 
			this.lookVis = 1; 
		} 

		public ALCS_lookItem(string theName, string theID, int theVis) 
		{ 
			this.lookName = theName; 
			this.lookID = theID; 
			this.lookVis = theVis; 
		} 

		public string ItemText 
		{ 
			get 
			{ 
				return this.lookName; 
			} 
		} 

		public string ItemValue 
		{ 
			get 
			{ 
				return this.lookID; 
			} 
		} 

		public int ItemVisibility 
		{ 
			get 
			{ 
				return this.lookVis; 
			} 
		} 
	} 

	#endregion 
}
