using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using ALCS_Library.ALCS_Data;
using ALCS_Library.ALCS_Basics;


namespace ALCS_Library.ALCS_Format
{
    //////////////////////////////////////////////////////////////////////////////////
    /// Enumeration reguired for date formatting
    //////////////////////////////////////////////////////////////////////////////////
    #region " Enumeration used by the date formatting class ..."

    /// <summary>
    /// Used By DayQualifier to determin the form of the returned data.
    /// </summary>
    public enum FormMode
    {
        AsString,
        AsPattern,
        AsSuperText
    }

    /// <summary>
    /// Used in Formatting the date as TEXT or as HTML
    /// </summary>
    public enum DisplayMode
    {
        TEXT,
        HTML
    }

    public enum DurationMode
    {
        xHours_yMinutes_zSeconds,
        Default
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////
    /// Date Formatting Functionality.
    /////////////////////////////////////////////////////////////////////////////////
    #region "Read a date from a given string"

    /// <summary>
    /// Date Formatting Functionalities.
    /// </summary>
    public class ALCS_DateReaderWriter
    {
        static IFormatProvider isCulture = CultureInfo.CurrentCulture;
        public static readonly DateTime defDate = DateTime.MaxValue;

        #region "Set Your Culture ..."

        public static void SetCheckCulture(IFormatProvider curCulture)
        {
            isCulture = curCulture;
        }

        #endregion 

        #region "Read a date from a string "

        /// <summary>
        /// Read the date using the default culture.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="convDate"></param>
        /// <returns></returns>
        public static bool ReadDate(string dateStr, out DateTime convDate)
        {
            bool dateOK = DateTime.TryParse(dateStr, isCulture, DateTimeStyles.None, out convDate);

            if (!dateOK)
            {
                convDate = defDate;
            }

            return dateOK;
        }

        /// <summary>
        /// Read the date as per the default provider.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateForm"></param>
        /// <param name="convDate"></param>
        /// <returns></returns>
        public static bool ReadDate(string dateStr, string dateForm, out DateTime convDate)
        {
            bool dateOK = DateTime.TryParseExact(dateStr, dateForm, isCulture, DateTimeStyles.None, out convDate);

            if (!dateOK)
            {
                convDate = defDate;
            }

            return dateOK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="pm"></param>
        /// <param name="convDate"></param>
        /// <returns></returns>
        public static bool ReadDate(string dateStr, DateParseMode pm, out DateTime convDate)
        {
            String[] dateForms = ALCS_Is.GetDateFormatList(pm);

            bool dateOK = DateTime.TryParseExact(dateStr, dateForms, isCulture, DateTimeStyles.None, out convDate);

            if (!dateOK)
            {
                convDate = defDate;
            }

            return dateOK;
        }

        #endregion 

        #region "Create a Date focused Date ..."

        public static bool ReadTimeFocusedDate(string inTime, out DateTime timeDate)
        {
            bool timeOK = false;

            // Set a Default Value ...
            inTime = ALCS_DataShift.WhenNull(inTime, "00:00:00");

            // Create Fake Day 
            DateTime dummyDate = new DateTime(1, 1, 1);

            // String Date time
            string datetimeStr = dummyDate.ToString("dd MMM yyyy") + " " + inTime;

            timeOK = ALCS_DateReaderWriter.ReadDate(datetimeStr, out timeDate);

            return timeOK;
        }

        #endregion 

        #region "Day Qualifier - not used at the moment"


        /// <summary>
        /// Merely adding the th, nd, or st qulaified based on the passed number.
        /// </summary>
        /// <param name="theNumber"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        public static string DayQualifier(int theDay, FormMode fm)
        {
            string thePattern;
            if ((theDay == 1) || (theDay == 21) || (theDay == 31))
            {
                if ((fm == FormMode.AsPattern))
                {
                    thePattern = @"\s\t";
                }
                else if ((fm == FormMode.AsString))
                {
                    thePattern = "st";
                }
                else
                {
                    thePattern = "";
                }
            }
            else if ((theDay == 2) || (theDay == 22))
            {
                if ((fm == FormMode.AsPattern))
                {
                    thePattern = @"\n\d";
                }
                else if ((fm == FormMode.AsString))
                {
                    thePattern = "nd";
                }
                else
                {
                    thePattern = "";
                }
            }
            else if ((theDay == 3) || (theDay == 23))
            {
                if ((fm == FormMode.AsPattern))
                {
                    thePattern = @"\r\d";
                }
                else if ((fm == FormMode.AsString))
                {
                    thePattern = "rd";
                }
                else
                {
                    thePattern = "";
                }
            }
            else if ((theDay <= 30))
            {
                if ((fm == FormMode.AsPattern))
                {
                    thePattern = @"\t\h";
                }
                else if ((fm == FormMode.AsString))
                {
                    thePattern = "th";
                }
                else
                {
                    thePattern = "";
                }
            }
            else
            {
                thePattern = "";
            }

            // Return the Patten.
            return thePattern;
        }

        /// <summary>
        /// Get the date qulifier 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DayQualifier(DateTime dt)
        {
            int theDay = dt.Day;
            return DayQualifier(theDay, FormMode.AsString);
        }

        /// <summary>
        /// Set a super Text ...
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        public static string DayQualifier(DateTime dt, FormMode fm)
        {
            string dayMark = DayQualifier(dt);

            if (fm == FormMode.AsSuperText)
            {
                return "<sup>" + dayMark + "</sup>";
            }
            else
            {
                return dayMark;
            }
        }


        #endregion 

        #region "Date time component ..."

        /// <summary>
        /// Check if the DataTime object has a time component. 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool HasTime(DateTime dt)
        {
            TimeSpan ts = dt.TimeOfDay;

            if (ts.TotalMilliseconds == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Extract the 
        /// </summary>
        /// <returns></returns>
        public static string ExtractHoursMinutesComponent(TimeSpan ts)
        {
            string tsString = "";
            string tsSign = "";
            
            // Set the sign
            if (ts.TotalMinutes < 0)
            {
                tsSign = "-";
            }
            
            //
            if (ts.Hours != 0)
            {
                tsString = String.Format("{0:#0}:{1:00}", Math.Abs(ts.Hours), Math.Abs(ts.Minutes));
            }
            else
            {
                tsString = String.Format("{0:#0}", Math.Abs(ts.Minutes));
            }

            // return 
            return tsSign + tsString;
        }
        #endregion 
    }
    #endregion

    #region "Handling Time span and intervals ..."

    /// <summary>
    /// The time intervals ....
    /// </summary>
    public enum TimeSlotSize : int
    {
        min60 = 60,
        min30 = 30,
        min20 = 20,
        min15 = 15,
        min10 = 10,
        min5 = 5
    }

    /// <summary>
    /// Display form of the slot.
    /// </summary>
    public enum TimeSlotForm
    {
        hh_mm,
        hh_m,
        h_mm,
        h_m,
        HH_SPACE_MM
    }


    /// <summary>
    /// Build a class that handles all time slot business.
    /// </summary>
    public class DayTimeSlots
    {
        private int slotMinutes;
        private int slotsCount;
        private TimeSlotForm tsf = TimeSlotForm.h_mm;
        private TimeSpan slotStart = new TimeSpan(0, 0, 0);
        private TimeSpan slotEnd = new TimeSpan(23, 59, 59);
        private bool health = true;
        private string memo = "";

        /// <summary>
        /// Constructor 0
        /// </summary>
        public DayTimeSlots()
        {
            this.slotMinutes = 30;
            AssignSlotsCount();
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        public DayTimeSlots(TimeSlotSize tss)
        {
            this.slotMinutes = Convert.ToInt32(tss);
            AssignSlotsCount();
        }

        /// <summary>
        /// How many slots there is in a day ....
        /// </summary>
        private void AssignSlotsCount()
        {
            try
            {
                this.slotsCount = Convert.ToInt32((slotEnd.TotalMinutes - slotStart.TotalMinutes) / this.slotMinutes);
            }
            catch (Exception ex)
            {
                this.health = false;
                this.memo = ex.Message;
            }
        }

        /// <summary>
        /// Load the Day as a list of slots ....
        /// </summary>
        public void LoadTimeDropDownList(ref DropDownList slotList, bool addBlank)
        {
            this.LoadTimeDropDownList(ref slotList, addBlank, this.tsf);
        }

        /// <summary>
        /// Load the Day as a list of slots ....
        /// </summary>
        public void LoadTimeDropDownList(ref DropDownList slotList, bool addBlank, TimeSlotForm tsf)
        {
            int idx;
            string slotText, slotValue;
            TimeSpan ts;

            // First Empty the Drop Down List.	
            slotList.Items.Clear();

            // Should we add a blank entry
            if (addBlank)
            {
                slotList.Items.Add(new ListItem("", ""));
            }

            // Loop and add the slots one by one
            idx = 0;
            ts = this.slotStart;

            while (ts <= this.slotEnd)
            {
                slotValue = Convert.ToString(idx);
                slotText = FormatTimeSlot(ts, tsf);

                slotList.Items.Add(new ListItem(slotText, slotValue));

                // Next one ...
                ts += new TimeSpan(0, this.slotMinutes, 0);
                idx++;
            }
        }

        /// <summary>
        /// given the slot index return the text associated with it.
        /// </summary>
        /// <returns></returns>
        public string ReturnSlotText(int sIndex, TimeSlotForm tsf)
        {
            string slotText = "";

            TimeSpan ts = this.slotStart + new TimeSpan(0, sIndex * this.slotMinutes, 0);

            if (ts > this.slotEnd)
            {
                this.health = false;
                this.memo = "The time slot is outside the specified range.";
            }
            else
            {
                slotText = FormatTimeSlot(ts, tsf);
            }

            // return 
            return slotText;
        }


        /// <summary>
        /// Given a date find the associated time slot.
        /// </summary>
        /// <returns></returns>
        public string ReturnSlotIndex(DateTime dt)
        {
            string slotIndex = "";

            TimeSpan ts = new TimeSpan(dt.Hour, dt.Minute, 0) - this.slotStart;

            slotIndex = Convert.ToString(Math.Floor(ts.TotalMinutes / this.slotMinutes));

            // return 
            return slotIndex;
        }

       
        /// <summary>
        /// Format the time to the desired format.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public string FormatTimeSlot(TimeSpan ts, TimeSlotForm tsf)
        {
            string slotText = "";

            if (tsf == TimeSlotForm.hh_mm)
            {
                slotText = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);
            }
            else if (tsf == TimeSlotForm.hh_m)
            {
                slotText = String.Format("{0:00}:{1:#0}", ts.Hours, ts.Minutes);
            }
            else if (tsf == TimeSlotForm.h_mm)
            {
                slotText = String.Format("{0:#0}:{1:00}", ts.Hours, ts.Minutes);
            }
            else if (tsf == TimeSlotForm.h_m)
            {
                slotText = String.Format("{0:#0}:{1:#0}", ts.Hours, ts.Minutes);
            }
            else if(tsf == TimeSlotForm.HH_SPACE_MM)
            {
                slotText = String.Format("{0:00} {1:00}", ts.Hours, ts.Minutes);
            }

            // Return the string
            return slotText;
        }

        /// <summary>
        /// Expose the public property of the class.
        /// </summary>
        #region "Expose the Data and Errors "

        // Expose Health
        public bool isHealthy
        {
            get
            {
                return this.health;
            }
        }

        // Expose Error Message
        public string Memo
        {
            get
            {
                return this.memo;
            }
        }

        // Expose the slot minutes ...
        public int _SlotMinutes
        {
            get
            {
                return this.slotMinutes;
            }
        }

        // Expose the slots count.
        public int _SlotsCount
        {
            get
            {
                return this.slotsCount;
            }
        }

        // Expose the index of the last slot
        public int _LastSlotIndex
        {
            get
            {
                return this.slotsCount - 1;
            }
        }

        #endregion

    }

    #endregion

    #region "General tool ....."

    public enum DiffMode
    {
        Exact,
        DaysCount
    }

    public static class ALCS_DateGeneric
    {
        /// <summary>
        /// Return the start of day ....
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(DateTime dateIn)
        {
            DateTime baseDate;
            baseDate = new DateTime(dateIn.Year, dateIn.Month, dateIn.Day, 0, 0, 0);

            // return 
            return baseDate;
        }

        /// <summary>
        /// Return the start of next day ....
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime StartOfNextDay(DateTime dateIn)
        {
            DateTime baseDate;
            baseDate = new DateTime(dateIn.Year, dateIn.Month, dateIn.Day, 0, 0, 0);

            // return 
            return baseDate.AddDays(1);
        }

        /// <summary>
        /// Return the end of day ....
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(DateTime dateIn)
        {
            DateTime baseDate;
            baseDate = dateIn.AddDays(1);
            baseDate = baseDate.AddSeconds(-1);

            // return 
            return baseDate;
        }

        /// <summary>
        /// Start Of Month.
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime Assemble_StartOfMonth(DateTime dateIn)
        {
            DateTime baseDate;
            baseDate = new DateTime(dateIn.Year, dateIn.Month, 1, 0, 0, 0);

            // return 
            return baseDate;
        }

        /// <summary>
        /// Start of Month from a month and a year.
        /// </summary>
        /// <param name="theMonth"></param>
        /// <param name="theYear"></param>
        /// <returns></returns>
        public static DateTime Assemble_StartOfMonth(int theMonth, int theYear)
        {
            DateTime baseDate;

            try
            {
                baseDate = new DateTime(theYear, theMonth, 1, 0, 0, 0);
            }
            catch
            {
                baseDate = DateTime.MaxValue;
            }

            // return 
            return baseDate;
        }

        /// <summary>
        /// End Of Month.
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime Assemble_EndOfMonth(DateTime dateIn)
        {
            DateTime baseDate;
            baseDate = new DateTime(dateIn.Year, dateIn.Month, 1, 0, 0, 0);
            baseDate = baseDate.AddMonths(1);
            baseDate = baseDate.AddDays(-1);

            // return 
            return baseDate;
        }

        /// <summary>
        /// End Of Month.
        /// </summary>
        /// <param name="dateIn"></param>
        /// <returns></returns>
        public static DateTime Assemble_EndOfMonth(int theMonth, int theYear)
        {
            DateTime baseDate;

            try
            {
                baseDate = new DateTime(theYear, theMonth, 1, 0, 0, 0);
                baseDate = baseDate.AddMonths(1);
                baseDate = baseDate.AddDays(-1);
            }
            catch
            {
                baseDate = DateTime.MaxValue;
            }

            // return 
            return baseDate;
        }

        /// <summary>
        /// Count the days between two days .....
        /// Any fraction of a day is considered a full day ....
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateSub"></param>
        /// <returns></returns>
        public static int DiffInDays(DateTime dateFrom, DateTime dateSub, DiffMode dm )
        {
            // Strip the time componenet ...
            dateFrom = StartOfDay(dateFrom);
            dateSub = StartOfDay(dateSub);
            
            TimeSpan ts = dateFrom.Subtract(dateSub);

            // return
            int diffDays = ts.Days;

            if ((dm == DiffMode.DaysCount))
            {
                diffDays++; 
            }
            return diffDays;
        }

        /// <summary>
        /// Another version that assume days counting ...
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateSub"></param>
        /// <returns></returns>
        public static int DiffInDays(DateTime dateFrom, DateTime dateSub)
        {
            return DiffInDays(dateFrom, dateSub, DiffMode.DaysCount);
        }

        /// <summary>
        /// Pick first date of week for the selected day ...
        /// selDay = sunday, monday , ..... or saturday
        /// </summary>
        /// <param name="inDate"></param>
        /// <param name="selDay"></param>
        /// <returns></returns>
        public static DateTime PickFirstDayOfWeek(DateTime inDate, string selDay)
        {
            DateTime outDate = inDate;

            if ((outDate == DateTime.MinValue) || (outDate == DateTime.MaxValue))
            {
                return outDate;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    outDate = inDate.AddDays(-i);
                    if (outDate.ToString("dddd").ToLower().Contains(selDay))
                    {
                        return outDate;
                    }
                }
            }

            // return 
            return outDate;
        }

        /// <summary>
        /// Overwrite with Monday assumed...
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static DateTime PickFirstDayOfWeek(DateTime inDate)
        {
            return PickFirstDayOfWeek(inDate, "monday");
        }

        #region "Date and time styling ..."

        /// <summary>
        /// format the date to dddd ddth MMMM yyyy
        /// </summary>
        /// <param name="inDate"></param>
        /// <returns></returns>
        public static string StyleDate_FullFormat(DateTime inDate)
        {
            return inDate.ToString("dddd d") + "<sup>" + ALCS_DateReaderWriter.DayQualifier(inDate) + "</sup>" + inDate.ToString(" MMMM yyyy");
        }

        #endregion 



        #region "Span and duration handling ...."


        /// <summary>
        /// Extract the 
        /// </summary>
        /// <returns></returns>
        public static string DurationAsString(TimeSpan ts, DurationMode dm)
        {
            string tsString = "";

            int tsDays, tsHours, tsMinutes, tsSeconds, tsMillis;
            string tsDaysStr="", tsHoursStr="", tsMinutesStr="", tsSecondsStr="";

            // Extract Duration component.
            tsDays = ts.Days;
            tsHours = ts.Hours;
            tsMinutes = ts.Minutes;
            tsSeconds = ts.Seconds;
            tsMillis = ts.Milliseconds;

            // Set the strings 
            tsDaysStr = (tsDays > 1) ? "days" : "day";
            tsHoursStr = (tsHours > 1) ? "hours" : "hour";
            tsMinutesStr = (tsMinutes > 1) ? "minutes" : "minute";
            tsSecondsStr = (tsSeconds > 1) ? "seconds" : "second";

            // Do we have days ...
            if (tsDays > 0)
            {
                tsString += (tsString == "") ? tsDays.ToString(): ", " + tsDays.ToString();
                tsString += " " + tsDaysStr;
            }

            if (tsHours > 0)
            {
                tsString += (tsString == "") ? tsHours.ToString(): ", " + tsHours.ToString();
                tsString += " " + tsHoursStr;
            }

            if (tsMinutes > 0)
            {
                tsString += (tsString == "") ? tsMinutes.ToString(): ", " + tsMinutes.ToString();
                tsString += " " + tsMinutesStr;
            }

            if (tsSeconds > 0)
            {
                tsString += (tsString == "") ? "less than 1 minute" : " and " + tsSeconds.ToString() + " " + tsSecondsStr;
            }

            // return 
            return tsString;
        }

        #endregion
    }
    #endregion 
}
