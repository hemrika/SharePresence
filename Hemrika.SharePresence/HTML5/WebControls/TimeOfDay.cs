using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a Time with Hour and Minutes of the day
    /// </summary>
    public struct TimeOfDay
    {
        /// <summary>
        /// A value between 0 and 23 represiting hour of the day
        /// </summary>
        public byte Hour { get { return _Hour; } }
        /// <summary>
        /// A value between 0 and 59 representing minute of hour
        /// </summary>
        public byte Minute { get { return _Minute; } }
        private byte _Minute;
        private byte _Hour;
        private TimeOfDay(byte createEmpty)
        {
            _Hour = 0;
            _Minute = 0;
        }
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TimeOfDay" />
        /// </summary>
        /// <param name="hour">A value between 0 and 23 represiting hour of the day</param>
        /// <param name="minute">A value between 0 and 59 representing minute of hour</param>
        public TimeOfDay(byte hour, byte minute)
        {
            _Hour = hour;
            _Minute = minute;
            CheckValidity();
        }
        private void CheckValidity()
        {
            if (_Hour < 0 || _Hour > 23)
                throw new ArgumentOutOfRangeException("Hour must be non-negarive and less than 24");
            if (_Minute < 0 || _Minute > 59)
                throw new ArgumentOutOfRangeException("Minute  must be non-negarive and less than 60");
        }
        /// <summary>
        /// Creates a <see cref="System.TimeSpan" /> object from the current TimeOfDay instance
        /// </summary>
        /// <returns>The <see cref="System.TimeSpan" /> object filled with current TimeOfDay instance</returns>
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(_Hour, _Minute, 0);
        }
        /// <summary>
        /// Returns a string representation of current TimeOfDay instance
        /// </summary>
        /// <returns>A string in HH:mm format</returns>
        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}", _Hour, _Minute);
        }
        /// <summary>
        /// Initialized a new <see cref="Hemrika.SharePresence.Html5.WebControls.TimeOfDay" /> from the <see cref="System.TimeSpan" /> object passed
        /// </summary>
        /// <param name="value">the intended TimeSpan value</param>
        /// <returns>A new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.TimeOfDay" /></returns>
        public static TimeOfDay FromTimeSpan(TimeSpan value)
        {
            return new TimeOfDay((byte)value.Hours, (byte)value.Minutes);
        }
        /// <summary>
        /// Parses the string passed and returns a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TimeOfDay" />
        /// </summary>
        /// <param name="value">the formatted string representation of a time of day</param>
        /// <returns>A new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.TimeOfDay" /></returns>
        public static TimeOfDay Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Value");
            }
            value = value.Trim();
            byte hour, minute, index = (byte)value.IndexOf(':');
            if (index < 0
                || !byte.TryParse(value.Substring(0, index), out hour)
                || !byte.TryParse(value.Substring(index + 1), out minute))
                throw new FormatException("Input string is not in correct format");
            return new TimeOfDay(hour, minute);
        }

        private static readonly TimeOfDay _Empty = new TimeOfDay(0);
        /// <summary>
        /// Represents the Empty Time of day value. The field is read-only
        /// </summary>
        public static TimeOfDay Empty { get { return _Empty; } }

        /// <summary>
        /// Determines if the provided value represents an Empty TimeOfDay
        /// </summary>
        /// <param name="value">the TimeOfDay instance to check</param>
        /// <returns>true if the provided object is Empty; otherwise false</returns>
        public static bool IsEmpty(TimeOfDay value)
        {
            return ReferenceEquals(value, _Empty);
        }
    }
}
