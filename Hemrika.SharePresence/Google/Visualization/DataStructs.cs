﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hemrika.SharePresence.Google.Visualization
{
    public struct MapCoordinate
    {
        public decimal Lon;
        public decimal Lat;
        public MapCoordinate(decimal Lon, decimal Lat)
        {
            this.Lon = Lon;
            this.Lat = Lat;
        }
    }
    public class TimelineEvent
    {
        public TimelineEvent() { }
        public TimelineEvent(string EventCategory, DateTime EventDate, decimal ChartValue) 
        {
            this.EventCategory = EventCategory;
            this.EventDate = EventDate;
            this.YValue = ChartValue;
        }
        public TimelineEvent(string EventCategory, DateTime EventDate, int ChartValue)
        {
            this.EventCategory = EventCategory;
            this.EventDate = EventDate;
            this.YValue = ChartValue;
        }
        
        public TimelineEvent(string EventCategory, DateTime EventDate, Decimal value, string AnnotationTitle, string AnnotationText)
            : this(EventCategory, EventDate, value)
        {
            this.OptionalAnnotationTitle = AnnotationTitle;
            this.OptionalAnnotationText = AnnotationText;
        }
        public TimelineEvent(string EventCategory, DateTime EventDate, int value, string AnnotationTitle, string AnnotationText)
            : this(EventCategory, EventDate, value)
        {
            this.OptionalAnnotationTitle = AnnotationTitle;
            this.OptionalAnnotationText = AnnotationText;
        }

        public string EventCategory { get; set; }       // required
        public DateTime EventDate { get; set; }         // required
        public decimal YValue { get; set; }             // required
        public string OptionalAnnotationTitle { get; set; }
        public string OptionalAnnotationText { get; set; }
    }
    public class GviConfigOption : Attribute
    {
        public GviConfigOption() { }
    }
    public class GviEventOption : Attribute
    {
        public string EventName;
        public GviEventOption() { }
        public GviEventOption(string EventName) 
        {
            this.EventName = EventName;
        }
        public override string ToString()
        {
            return EventName;
        }
    }
    public static class GviExtensions
    {
        public static string LowerCaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string GVINameParse(this string str)
        {
            if (str.StartsWith("Gvi", true, System.Globalization.CultureInfo.CurrentCulture))
                str =  str.Substring(3).LowerCaseFirst();
            if (str.StartsWith("GV", true, System.Globalization.CultureInfo.CurrentCulture))
                str =  str.Substring(2).LowerCaseFirst();
            else
                str =  str.LowerCaseFirst();

            return str.Replace("_", ".");   // replace the '_' with '.' for subclassing
        }

    }
}
