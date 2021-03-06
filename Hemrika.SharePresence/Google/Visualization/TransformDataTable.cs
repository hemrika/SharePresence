﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hemrika.SharePresence.Google.Visualization
{
    /**************************************
     * Sample Output
          var dataTable = new google.visualization.DataTable();
          dataTable.addColumn('string');
          dataTable.addColumn('number');
          dataTable.addRows([
            ['January',12],
            ['February',8],
            ['March',3]
  ]);
     **************************************/
 

    public class TransformDataTable
    {
        class ColumnInfoStruct
        {
            public ColumnInfoStruct(DataColumn dc)
            {
                this.ColumnName = dc.ColumnName;
                this.ColumnJavascriptType = googleConvertType(dc);
                this.RequiresQuotes = googleRequiresQuotes(dc);
            }
            public string ColumnName;
            public string ColumnJavascriptType;
            public bool RequiresQuotes;
            private static string googleConvertType(DataColumn col)
            {
                if (col.DataType == typeof(int)
                    || col.DataType == typeof(int)
                    || col.DataType == typeof(Int16)
                    || col.DataType == typeof(Int32)
                    || col.DataType == typeof(Int64)
                    || col.DataType == typeof(long)
                    || col.DataType == typeof(decimal)
                    || col.DataType == typeof(Decimal)
                    || col.DataType == typeof(short)
                    || col.DataType == typeof(double)
                    || col.DataType == typeof(Double)
                    || col.DataType == typeof(Single)
                    || col.DataType == typeof(float))
                {
                    return "number";
                }
                else if (col.DataType == typeof(DateTime))
                {
                    return "datetime";
                }
                else if (col.DataType == typeof(Boolean) || col.DataType == typeof(bool))
                {
                    return "boolean";
                }
                else if (col.DataType == typeof(String) || col.DataType == typeof(string))
                {
                    return "string";
                }
                else
                {
                    return "object";
                }
            }
            private static bool googleRequiresQuotes(DataColumn col)
            {
                if (string.Compare(googleConvertType(col), "string",true) == 0)
                    return true;
                if (string.Compare(googleConvertType(col), "datetime", true) == 0)
                    return true;
                return false;
            }
            public override string ToString()
            {
                string convertedCType = this.ColumnJavascriptType == "object"? "string": this.ColumnJavascriptType;
                return string.Format(@"dataTable.addColumn('{0}', '{1}');", convertedCType, this.ColumnName);
            }
        }

        public static string ToGoogleDataTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"
                var dataTable = new google.visualization.DataTable();
                // Declare columns and rows.
                ");

            // build columns mashup
            List<ColumnInfoStruct> gcols = new List<ColumnInfoStruct>();
            foreach (DataColumn c in dt.Columns)
            {
                ColumnInfoStruct gc = new ColumnInfoStruct(c);
                gcols.Add(gc);
                sb.AppendLine(gc.ToString());
            }

            // add datarows
            List<string> tmp = new List<string>();
            foreach (DataRow dr in dt.Rows)
                tmp.Add(googleDataRow(gcols, dr));

            sb.AppendLine(string.Format(
            @"
            // Add data.
            dataTable.addRows([ 
                {0}
            ]);", 
                string.Join(",", tmp.ToArray())));


            sb.AppendLine("return dataTable;");

            return sb.ToString();
        }



        #region Private Methods

        
        private static string googleDataRow(List<ColumnInfoStruct> gcols, DataRow dr)
        {
            // ['January',12] //
            List<string> tmp = new List<string>();
            foreach (ColumnInfoStruct s in gcols)
            {
               
                if (string.Compare(s.ColumnJavascriptType,"datetime",true) == 0)
                {
                    if (dr[s.ColumnName] != null)
                    {
                        DateTime dt = (DateTime)dr[s.ColumnName];
                        TimeSpan t = (dt - new DateTime(1970, 1, 1));
                        int timestamp = (int)t.TotalSeconds;
                        tmp.Add(string.Format(@"new Date({0})", timestamp));
                    }
                }
                else if (s.RequiresQuotes)
                    tmp.Add(string.Format("'{0}'", dr[s.ColumnName].ToString().Replace("'", @"\'")));
                else
                    tmp.Add(dr[s.ColumnName].ToString());
            }
            return string.Format("[ {0} ]", string.Join(",", tmp.ToArray()));
        }
       
        #endregion
    }
}
