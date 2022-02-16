using System;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Basics;
using ALCS_Library.ALCS_Numerics;

namespace ALCS_Library.ALCS_Format
{
	/// <summary>
	/// Summary description for ALCS_Coloring.
	/// </summary>
	public class ALCS_Coloring
	{
		/// <summary>
		/// Define some popular colors
		/// </summary>
		public enum TextColor
		{
			Green,
            SpringGreen,
            PastelGreen,
			Red,
            Blood,
            RedMidDark,
            RedDark,
            Maroon,
			Black,
			Blue,
			BlueMid,
			BlueLight,
			Orange,
			OrangeMid,
            OrangeDark,
            PurpleDark,
            Purple,
            Fuchsia,
            BiscuitLight,
            BiscuitDark,
            Burgundy,
            Amber,
            Gray,
			None
		}

		/// <summary>
		/// Common Styles.
		/// </summary>
		public enum TextStyle
		{
			Bold,
			Italic,
			BoldItalic,
			None
		}

        /// <summary>
        /// Text decoration attribute .....
        /// </summary>
        public enum TextDecoration
        {
            Normal,
            Underline,
            Blink,
            UnderlineBlink,
            None
        }

		/// <summary>
		/// Get the color code associated with the symbol.
		/// </summary>
		/// <param name="tc"></param>
		/// <returns></returns>
		public static string PickColorCode(TextColor tc)
		{
			string colorCode; 

			if (tc == TextColor.Green) 
			{ 
				colorCode = "#008000"; 
			}
            else if (tc == TextColor.PastelGreen)
            {
                colorCode = "#00FF00"; 
            }
            else if (tc == TextColor.SpringGreen)
            {
                colorCode = "#00FF7F";
            }
            else if (tc == TextColor.Red) 
			{ 
				colorCode = "#FF0000"; 
			}
			else if (tc == TextColor.Blood) 
			{
                colorCode = "#D90000"; 
			}
            else if (tc == TextColor.RedMidDark)
            {
                colorCode = "#CD0000";
            }
            else if (tc == TextColor.RedDark)
            {
                colorCode = "#8B0000";
            }
            else if (tc == TextColor.Maroon)
            {
                colorCode = "#800000";
            } 
			else if (tc== TextColor.Black)
			{
				colorCode = "#000000"; 
			}
			else if (tc == TextColor.Blue) 
			{ 
				colorCode = "#0000FF"; 
			} 
			else if (tc == TextColor.BlueMid) 
			{ 
				colorCode = "#2561DE"; 
			} 
			else if (tc == TextColor.BlueLight) 
			{ 
				colorCode = "#0D96EE"; 
			} 
			else if (tc == TextColor.Orange) 
			{ 
				colorCode = "#FC9332"; 
			} 
			else if (tc == TextColor.OrangeMid) 
			{ 
				colorCode = "#FFBD4C"; 
			}
            else if (tc == TextColor.OrangeDark)
            {
                colorCode = "#FF6600";
            }
            else if(tc == TextColor.Purple)
            {
                colorCode = "#A020F0"; 
            }
            else if (tc == TextColor.PurpleDark)
            {
                colorCode = "#800080"; 
            }
            else if (tc == TextColor.Fuchsia)
            {
                colorCode = "#FF00FF";
            }
            else if (tc == TextColor.BiscuitLight)
            {
                colorCode = "#FFFFE4";
            }
            else if (tc == TextColor.BiscuitDark)
            {
                colorCode = "#E7E7C7";
            }
            else if (tc == TextColor.Burgundy)
            {
                colorCode = "#C11B17";
            }
            else if (tc == TextColor.Amber)
            {
                colorCode = "#FFCC00";
            }
            else if (tc == TextColor.Gray)
            {
                colorCode = "#808080";
            }
			else 
			{ 
				colorCode = "#000000"; 
			}

			// return 
			return colorCode;
		}


        /// <summary>
        /// Wrap the text with a colored span ....
        /// </summary>
        /// <param name="theText"></param>
        /// <param name="tc"></param>
        /// <param name="ts"></param>
        /// <param name="baseClass"></param>
        /// <returns></returns>
        public static string ColorWrapper(string theText, TextColor tc, TextStyle ts, TextDecoration td, string baseClass)
		{
			string wrapText;
			string colorCode;
            string theStyle = "";

			// Get the Color Picker.
			colorCode = PickColorCode(tc);

			// apply the style ....
			if (ts == TextStyle.Bold) 
			{ 
				theText = "<B>" + theText + "</B>"; 
			} 
			else if (ts == TextStyle.Italic) 
			{ 
				theText = "<I>" + theText + "</I>"; 
			} 
			else if (ts == TextStyle.BoldItalic) 
			{ 
				theText = "<B><I>" + theText + "</I></B>"; 
			}

            // add the blink text tag ....
            if (td == TextDecoration.Blink)
            {
                theText = "<BLINK>" + theText + "</BLINK>"; 
            }
            else if (td == TextDecoration.Underline)
            {
                theText = "<U>" + theText + "</U>"; 
            }
            else if (td == TextDecoration.UnderlineBlink)
            {
                theText = "<U><BLINK>" + theText + "</BLINK></U>"; 
            }


            // Set the Style ...
            theStyle = "COLOR:" + colorCode + ";";
            if (td == TextDecoration.Blink)
            {
                theStyle += "text-decoration:blink";
            }

            wrapText = "<span class=\"" + baseClass + "\" style=\"" + theStyle + "\">" + theText + "</span>"; 

			// Return 
			return wrapText;
		}


        /// <summary>
        /// Another Version ....
        /// </summary>
        /// <param name="theText"></param>
        /// <param name="tc"></param>
        /// <param name="ts"></param>
        /// <param name="td"></param>
        /// <returns></returns>
        public static string ColorWrapper(string theText, TextColor tc, TextStyle ts, TextDecoration td)
        {
            // Return 
            return ColorWrapper(theText, tc, ts, td, "clearText");
        }
        
        /// <summary>
        /// Wrap the passed text in the proper color and style.
        /// </summary>
        /// <param name="theText"></param>
        /// <param name="tc"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string ColorWrapper(string theText, TextColor tc, TextStyle ts)
        {
            // Return 
            return ColorWrapper(theText, tc, ts, TextDecoration.None, "clearText") ;
        }


        public static string ColorWrapper(string theText, TextColor tc, TextDecoration td )
        {
            // Return 
            return ColorWrapper(theText, tc, TextStyle.None, td);
        }
        
        /// <summary>
        /// Wrap the passed text in the proper color and style.
        /// </summary>
        /// <param name="theText"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        public static string ColorWrapper(string theText, TextColor tc)
        {
            // Return 
            return ColorWrapper(theText, tc, TextStyle.None);
        }
        
        /// <summary>
        /// Wrap the passed text in the proper color and style.
        /// </summary>
        /// <param name="theText"></param>
        /// <param name="tc"></param>
        /// <param name="baseClass"></param>
        /// <returns></returns>
        public static string ColorWrapper(string theText, TextColor tc, string baseClass)
        {
            // Return 
            return ColorWrapper(theText, tc, TextStyle.None, TextDecoration.None, baseClass);
        }

        public static string ColorWrapper(string theText, TextColor tc, TextDecoration td, string baseClass)
        {
            // Return 
            return ColorWrapper(theText, tc, TextStyle.None, td, baseClass);
        }


        #region "Change Color Code from Hexto RGB and opposite."

        /// <summary>
        /// Get A hex representation out of the decimal colors.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static bool ColorRGBToHex(int r, int g, int b, out string hexColor)
        {
            string rHex = "", gHex = "", bHex = "";

            // Read the colors.
            bool rOK = ALCS_Numbers.IntToHex_Single(r, out rHex);
            bool gOK = ALCS_Numbers.IntToHex_Single(g, out gHex);
            bool bOK = ALCS_Numbers.IntToHex_Single(b, out bHex);

            // return
            if ((rOK) && (gOK) && (bOK))
            {
                hexColor = rHex + gHex + bHex;
                return true;
            }
            else
            {
                hexColor = "";
                return false;
            }
        }

        /// <summary>
        /// Assuming the RGB Color is comma delimited.
        /// </summary>
        /// <param name="rgbColor"></param>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static bool ColorRGBToHex(string rgbColor, out string hexColor)
        {
            hexColor = "";

            // the RGB Color. 
            if (!rgbColor.Contains(","))
            {
                return false;
            }
            else
            {
                string[] rgb = rgbColor.Split(",".ToCharArray());

                if (rgb.Length != 3)
                {
                    return false;
                }
                else if ((!ALCS_Is.isInteger(rgb[0], WorkMode.Strict)) || (!ALCS_Is.isInteger(rgb[1], WorkMode.Strict)) || (!ALCS_Is.isInteger(rgb[2], WorkMode.Strict)))
                {
                    return false;
                }
                else
                {
                    return ColorRGBToHex(Convert.ToInt32(rgb[0]), Convert.ToInt32(rgb[1]), Convert.ToInt32(rgb[2]), out hexColor);
                }
            }
        }

        /// <summary>
        /// return a RGB representation of the HEX Color passed.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ColorHexToRGB(string hexColor, out int r, out int g, out int b)
        {
            // Initialize.
            r = 0;
            g = 0;
            b = 0;

            // Basic Check.
            if (hexColor == "")
            {
                return false;
            }
            else if (hexColor.Length < 6)
            {
                return false;
            }

            // #?
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            // Valid Color.
            if (hexColor.Length != 6)
            {
                return false;
            }
            else
            {
                // rHex.
                string rHex = hexColor.Substring(0, 2);
                string gHex = hexColor.Substring(2, 2);
                string bHex = hexColor.Substring(4, 2);

                // r, g, b
                return ((ALCS_Numbers.HexToInt_Single(rHex, out r)) && (ALCS_Numbers.HexToInt_Single(gHex, out g)) && (ALCS_Numbers.HexToInt_Single(bHex, out b)));
            }
        }

        /// <summary>
        /// change the hex color to a RGB form.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <param name="rgbColor"></param>
        /// <returns></returns>
        public static bool ColorHexToRGB(string hexColor, out string rgbColor)
        {
            int r = 0, g = 0, b = 0;

            bool rgbOK = ColorHexToRGB(hexColor, out r, out g, out b);

            if (rgbOK)
            {
                rgbColor = r.ToString() + "," + g.ToString() + "," + b.ToString();
                return true;
            }
            else
            {
                rgbColor = "";
                return false;
            }
        }

        #endregion 

	}
}
