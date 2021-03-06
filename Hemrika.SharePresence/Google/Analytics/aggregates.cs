/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
/* Created by Alex Maitland, maitlandalex@gmail.com */
using System.Collections.Generic;
using System.Xml;
using Hemrika.SharePresence.Google.Client;
using Hemrika.SharePresence.Google.Extensions;

namespace Hemrika.SharePresence.Google.Analytics
{
    /// <summary>
    /// GData schema extension describing aggregate results.
    /// dxp:aggregates  contains aggregate data for all metrics requested in the feed.
    /// </summary>
    public class Aggregates : SimpleContainer
    {

        private List<Metric> metrics;


        /// <summary>
        /// default constructor for dxp:aggregates
        /// </summary>
        public Aggregates() :
            base(AnalyticsNameTable.XmlAggregatesElement,
                 AnalyticsNameTable.gAnalyticsPrefix,
                 AnalyticsNameTable.gAnalyticsNamspace)
        {
            this.ExtensionFactories.Add(new Metric());
        }

        /// <summary>
        ///  property accessor for the Thumbnails 
        /// </summary>
        public List<Metric> Metrics
        {
            get 
            {
                if (this.metrics == null)
                {
                    this.metrics = FindExtensions<Metric>(AnalyticsNameTable.XmlMetricElement, AnalyticsNameTable.gAnalyticsNamspace);
                }
                return this.metrics;
            }
        }
    }
}
