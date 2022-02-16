using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace ALCS_Library.ALCS_Basics
{
   
    #region "Used For URL resolution ..."

    public enum DateParseMode
    {
        Small,
        Middle,
        Long,
        AnyForm
    }

    public enum TimeParseMode
    {
        Hours_24,
        Hours_12
    }


    /// <summary>
    /// How to pass the parameter to the new page
    /// </summary>
    public enum DataChannel
    {
        BySession,
        ByQuery
    }


    public enum urlLocation
    {
        Internal,
        External,
        FlyingInternal,
        FlyingExternal
    }

    public enum specialHTML
    {
        _Space,
        _At,
        _CopyRight,
        _Registered,
        _UpArrow,
        _DownArrow,
        _Bullet,
        _BulletDisk,
        _MiddleDot,
        _DashShort,
        _DashLong,
        _DoubleAngleClose,
        _DoubleAngleOpen,
    }

    enum ALCS_Culture
    {
        English = 0x0009,
        English_Australia = 0x0C09, 
        English_New_Zealand = 0x1409,
        English_United_Kingdom = 0x0809,
        English_United_States = 0x0409,
    }

    #endregion 

    #region "Some General Enumerators ..."

    /// <summary>
    /// Side Left/Right 
    /// </summary>
    public enum LeftRight
    {
        Left,
        Middle,
        Right,
    }

    /// <summary>
    /// Side Up/Down 
    /// </summary>
    public enum UpDown
    {
        Up,
        Middle,
        Down,
    }

    #endregion 

    #region "Some Constants  ..."

    /// <summary>
    /// Some values to be used as marker for - No valid values found ...
    /// </summary>
    public static class ALCS_Vault
    {
        public const int minPass = 10;  /*  This must be changed to 10 now (from 6) */
        public const int maxPass = 16; /*  This must be changed to 16 now (from 12) */

        public const int minSimplePass = 8;  /*  This must be changed to 10 now (from 6) */
        public const int maxSimplePass = 16; /*  This must be changed to 16 now (from 12) */


        public const int minUname = 5;
        public const int maxUname = 20;

        public const string ddBack1 = "#FFFFFF";
        public const string ddBack2 = "#EAEAEA";

        /* added from LRSONLINE system */
        public const int safeID = -1;
        public const int safeInt = -1;

        public static readonly DateTime safeDate = new DateTime(1, 1, 1);


    }


    #endregion 


}
