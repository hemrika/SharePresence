using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents an object containing month and year
    /// </summary>
    public struct MonthOfYear
    {
        /// <summary>
        /// A value between 1 and 12, month
        /// </summary>
        public byte MonthNumber { get { return _MonthNumber; } }
        /// <summary>
        /// Year
        /// </summary>
        public short Year { get { return _Year; } }
        private short _Year;
        private byte _MonthNumber;
        private MonthOfYear(byte createEmpty)
        {
            _Year = 0;
            _MonthNumber = 0;
        }
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.MonthOfYear" />
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="monthNumber">A value between 1 and 12, month</param>
        public MonthOfYear(short year, byte monthNumber)
        {
            _Year = year;
            _MonthNumber = monthNumber;
            CheckValidity();
        }
        private void CheckValidity()
        {
            if (_Year < 0)
                throw new ArgumentOutOfRangeException("Year must be positive");
            if (_MonthNumber < 1)
                throw new ArgumentOutOfRangeException("MonthNumber must be positive and not zero");

            if (_MonthNumber > 12)
                throw new ArgumentOutOfRangeException("Month number must be between 1 and 12");
        }
        /// <summary>
        /// Add the specified number of months to the current instance
        /// </summary>
        /// <param name="value">Number of months to add; value can be negative</param>
        /// <returns>A new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.MonthOfYear" /></returns>
        public MonthOfYear AddMonths(byte value)
        {
            var newMonthNumber = (byte)(_MonthNumber + value);
            short newYear = _Year;
            if (newMonthNumber < 1)
            {
                while (newMonthNumber < 1)
                {
                    newYear--;
                    newMonthNumber += 12;
                }
            }
            else
            {
                while (newMonthNumber > 12)
                {
                    newYear++;
                    newMonthNumber -= 12;
                }
            }
            return new MonthOfYear(newYear, newMonthNumber);
        }
        /// <summary>
        /// Returns a formatted string for the current MonthOfYear instance
        /// </summary>
        /// <returns>A string in YYYY-MM format</returns>
        public override string ToString()
        {
            return string.Format("{0:0000}-{1:00}", _Year, _MonthNumber);
        }
        /// <summary>
        /// Parses a formatted string and creates new MonthOfYear instance
        /// </summary>
        /// <param name="value">The formatted MonthOfYear string</param>
        /// <returns>New instance of <see cref="Hemrika.SharePresence.Html5.WebControls.MonthOfYear" /></returns>
        public static MonthOfYear Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Value");
            }
            value = value.Trim();
            short year;
            byte monthNumber, index = (byte)value.IndexOf('-');
            if (index < 0
                || !short.TryParse(value.Substring(0, index), out year)
                || !byte.TryParse(value.Substring(index + 1), out monthNumber))
                throw new FormatException("Input string is not in correct format");
            return new MonthOfYear(year, monthNumber);
        }

        private static readonly MonthOfYear _Empty = new MonthOfYear(0);

        /// <summary>
        /// Represents the Empty month of year value. The field is read-only
        /// </summary>
        public static MonthOfYear Empty { get { return _Empty; } }

        /// <summary>
        /// Determines if the provided value represents an Empty MonthOfYear
        /// </summary>
        /// <param name="value">the MonthOfYear instance to check</param>
        /// <returns>true if the provided object is Empty; otherwise false</returns>
        public static bool IsEmpty(MonthOfYear value)
        {
            return ReferenceEquals(value, _Empty);
        }
    }
}
