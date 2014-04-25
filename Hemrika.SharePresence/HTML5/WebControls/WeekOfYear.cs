using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a Week of Year object containing year and week number
    /// </summary>
    public struct WeekOfYear
    {
        /// <summary>
        /// A Value from 1 to 53 representing week of the year
        /// </summary>
        public byte WeekNumber { get { return _WeekNumber; } }
        /// <summary>
        /// Year
        /// </summary>
        public short Year { get { return _Year; } }
        private short _Year;
        private byte _WeekNumber;
        private WeekOfYear(byte createEmpty)
        {
            _Year = 0;
            _WeekNumber = 0;
        }
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.WeekOfYear" />
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="weekNumber">A Value from 1 to 53 representing week of the year</param>
        public WeekOfYear(short year, byte weekNumber)
        {
            _Year = year;
            _WeekNumber = weekNumber;
            CheckValidity();
        }
        private void CheckValidity()
        {
            if (_Year < 0)
                throw new ArgumentOutOfRangeException("Year must be positive");
            if (_WeekNumber < 1)
                throw new ArgumentOutOfRangeException("WeekNumber must be positive and not zero");

            var maxWeekNumber = GetWeeksCountInYear(_Year);
            if (_WeekNumber > maxWeekNumber)
                throw new ArgumentOutOfRangeException(string.Format("In year {0} there are only {1} weeks, so {2} is not a valid week number", _Year, maxWeekNumber, _WeekNumber));
        }
        /// <summary>
        /// Adds specified number of weeks to the instance
        /// </summary>
        /// <param name="value">Number of weeks to add; value can be negative</param>
        /// <returns>A new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.WeekOfYear"/></returns>
        public WeekOfYear AddWeeks(byte value)
        {
            var newWeekNumber = (byte)(_WeekNumber + value);
            short newYear = _Year;
            if (newWeekNumber < 1)
            {
                while (newWeekNumber < 1)
                {
                    newYear--;
                    newWeekNumber += GetWeeksCountInYear(newYear);
                }
            }
            else
            {
                byte maxWeekNumber;
                while (newWeekNumber > (maxWeekNumber = GetWeeksCountInYear(newYear)))
                {
                    newYear++;
                    newWeekNumber -= maxWeekNumber;
                }
            }
            return new WeekOfYear(newYear, newWeekNumber);
        }
        /// <summary>
        /// Returns number of weeks in the Gregorian year
        /// </summary>
        /// <param name="year">Gregorian year</param>
        /// <returns>Number of weeks in specified year</returns>
        public static byte GetWeeksCountInYear(short year)
        {
            switch (year % 400)
            {
                case 004:
                case 009:
                case 015:
                case 020:
                case 026:
                case 032:
                case 037:
                case 043:
                case 048:
                case 054:
                case 060:
                case 065:
                case 071:
                case 076:
                case 082:
                case 088:
                case 093:
                case 099:
                case 105:
                case 111:
                case 116:
                case 122:
                case 128:
                case 133:
                case 139:
                case 144:
                case 150:
                case 156:
                case 161:
                case 167:
                case 172:
                case 178:
                case 184:
                case 189:
                case 195:
                case 201:
                case 207:
                case 212:
                case 218:
                case 224:
                case 229:
                case 235:
                case 240:
                case 246:
                case 252:
                case 257:
                case 263:
                case 268:
                case 274:
                case 280:
                case 285:
                case 291:
                case 296:
                case 303:
                case 308:
                case 314:
                case 320:
                case 325:
                case 331:
                case 336:
                case 342:
                case 348:
                case 353:
                case 359:
                case 364:
                case 370:
                case 376:
                case 381:
                case 387:
                case 392:
                case 398:
                    return 53;
                default:
                    return 52;
            }
        }
        /// <summary>
        /// Returns a formatted string for the current WeekOfYear instance
        /// </summary>
        /// <returns>A string in YYYY-Www format</returns>
        public override string ToString()
        {
            return string.Format("{0:0000}-W{1:00}", _Year, _WeekNumber);
        }
        /// <summary>
        /// Parses a formatted string and creates new WeekOfYear instance
        /// </summary>
        /// <param name="value">The formatted WeekOfYear string</param>
        /// <returns>New instance of <see cref="Hemrika.SharePresence.Html5.WebControls.WeekOfYear" /></returns>
        public static WeekOfYear Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Value");
            }
            value = value.Trim();
            short year;
            byte weekNumber, index = (byte)value.IndexOf('-');
            if (index < 0
                || value[index + 1] != 'W'
                || !short.TryParse(value.Substring(0, index), out year)
                || !byte.TryParse(value.Substring(index + 2), out weekNumber))
                throw new FormatException("Input string is not in correct format");
            return new WeekOfYear(year, weekNumber);
        }

        private static readonly WeekOfYear _Empty = new WeekOfYear(0);
        /// <summary>
        /// Represents the Empty week of year value. The field is read-only
        /// </summary>
        public static WeekOfYear Empty { get { return _Empty; } }

        /// <summary>
        /// Determines if the provided value represents an Empty WeekOfYear
        /// </summary>
        /// <param name="value">the WeekOfYear instance to check</param>
        /// <returns>true if the provided object is Empty; otherwise false</returns>
        public static bool IsEmpty(WeekOfYear value)
        {
            return ReferenceEquals(value, _Empty);
        }        
    }
}
