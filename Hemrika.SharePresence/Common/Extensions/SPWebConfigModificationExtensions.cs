// -----------------------------------------------------------------------
// <copyright file="SPServiceExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.SharePoint.Administration;
namespace Hemrika.SharePresence.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;
    using System.Globalization;
    using Hemrika.SharePresence.Common.Logging;
    using System.Diagnostics;
    using System.Threading;
    using System.Text.RegularExpressions;

    /// <summary> 
    /// SPService extension methods 
    /// </summary> 
    public static class SPWebConfigModificationExtensions
    {
        /// <summary>
        /// Adds a Web.config modification to the specified Web application.
        /// </summary>
        /// <remarks>The caller is responsible for updating the Web application
        /// and subsequently applying the Web.config modifications.</remarks>
        /// <param name="webApp">The Web application to add the Web.config
        /// modification to.</param>
        /// <param name="owner">The owner of the Web.config modification.
        /// </param>
        /// <param name="name">The name of the attribute or element.</param>
        /// <param name="path">The XPath expression that is used to locate the
        /// node that is being operated on.</param>
        /// <param name="type">The type of modification to make.</param>
        /// <param name="value">The value of the Web.config element or
        /// attribute.</param>
        public static void AddWebConfigModification(
            SPWebApplication webApp,
            string owner,
            string name,
            string path,
            SPWebConfigModification.SPWebConfigModificationType type,
            string value)
        {
            if (webApp == null)
            {
                throw new ArgumentNullException("webApp");
            }

            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            else if (string.IsNullOrEmpty(owner) == true)
            {
                throw new ArgumentException(
                    "The owner must be specified.",
                    "owner");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentException(
                    "The name must be specified.",
                    "name");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (string.IsNullOrEmpty(path) == true)
            {
                throw new ArgumentException(
                    "The path must be specified.",
                    "path");
            }

            SPWebConfigModification modification = new SPWebConfigModification(
                name,
                path);

            modification.Owner = owner;
            modification.Sequence = 0;
            modification.Type = type;

            if (string.IsNullOrEmpty(value) == false)
            {
                modification.Value = value;
            }

            if (!webApp.WebConfigModifications.Contains(modification))
            {
                webApp.WebConfigModifications.Add(modification);
            }

            //webApp.WebConfigModifications.Add(modification);
        }

        /// <summary>
        /// Applies all Web.config modifications to the Web application.
        /// </summary>
        /// <param name="webApp">The Web application to apply the Web.config
        /// modifications to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ApplyWebConfigModifications(
            SPWebApplication webApp)
        {
            if (webApp == null)
            {
                throw new ArgumentNullException("webApp");
            }

            webApp.WebService.ApplyWebConfigModifications();

            if (webApp.Farm.TimerService.Instances.Count > 1)
            {
                // HACK:
                //
                // When there are multiple front-end Web servers in the
                // SharePoint farm, we need to wait for the timer job that
                // performs the Web.config modifications to complete before
                // continuing. Otherwise, we may encounter the following error
                // (e.g. when applying Web.config changes from two different
                // features in rapid succession):
                // 
                // "A web configuration modification operation is already
                // running."
                //
                WaitForOnetimeJobToFinish(
                   webApp.Farm,
                   "job-webconfig-modification",
                   60);
            }
        }

        /// <summary>
        /// Removes all Web.config modifications with the specified owner from
        /// the Web application.
        /// </summary>
        /// <remarks>If any Web.config modifications are removed, changes to the
        /// Web application are subsequently applied.
        /// </remarks>
        /// <param name="webApp">The Web application to remove the Web.config
        /// modification from.</param>
        /// <param name="owner">The owner of the Web.config modifications to
        /// remove.</param>
        public static void RemoveWebConfigModifications(
            SPWebApplication webApp,
            string owner)
        {
            if (webApp == null)
            {
                throw new ArgumentNullException("webApp");
            }

            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            else if (string.IsNullOrEmpty(owner) == true)
            {
                throw new ArgumentException(
                    "The owner must be specified.",
                    "owner");
            }

            int numberOfModificationsRemoved = 0;

            for (int i = webApp.WebConfigModifications.Count - 1; i >= 0; i--)
            {
                if (webApp.WebConfigModifications[i].Owner == owner)
                {
                    webApp.WebConfigModifications.RemoveAt(i);
                    numberOfModificationsRemoved++;
                }
            }

            if (numberOfModificationsRemoved == 0)
            {
            }
            else
            {
                webApp.Update();

                ApplyWebConfigModifications(webApp);
            }
        }

        private static bool IsJobDefined(
    SPFarm farm,
    string jobTitle)
        {
            Debug.Assert(farm != null);
            Debug.Assert(string.IsNullOrEmpty(jobTitle) == false);

            SPServiceCollection services = farm.Services;
            string pattern = "job-webconfig";
            foreach (SPService service in services)
            {
                foreach (SPJobDefinition job in service.JobDefinitions)
                {
                    return Regex.IsMatch(job.Title, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified timer job is currently running (or
        /// scheduled to run).
        /// </summary>
        /// <param name="farm">The farm to check if the job is running on.</param>
        /// <param name="jobTitle">The title of the timer job.</param>
        /// <returns><c>true</c> if the specified timer job is currently running
        /// (or scheduled to run); otherwise <c>false</c>.</returns>
        public static bool IsJobRunning(
            SPFarm farm,
            string jobTitle)
        {
            Debug.Assert(farm != null);
            Debug.Assert(string.IsNullOrEmpty(jobTitle) == false);

            SPServiceCollection services = farm.Services;
            string pattern = "Web.Config";
            foreach (SPService service in services)
            {
                foreach (SPRunningJob job in service.RunningJobs)
                {
                    return Regex.IsMatch(job.JobDefinitionTitle, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
            }

            return false;
        }

        /// <summary>
        /// Waits for a one-time SharePoint timer job to finish.
        /// </summary>
        /// <param name="farm">The farm on which the timer job runs.</param>
        /// <param name="jobTitle">The title of the timer job (e.g. "Windows
        /// SharePoint Services Web.Config Update").</param>
        /// <param name="maximumWaitTime">The maximum time (in seconds) to wait
        /// for the timer job to finish.</param>
        public static void WaitForOnetimeJobToFinish(
            SPFarm farm,
            string jobTitle,
            byte maximumWaitTime)
        {
            if (farm == null)
            {
                throw new ArgumentNullException("farm");
            }

            if (jobTitle == null)
            {
                throw new ArgumentNullException("jobTitle");
            }
            else if (string.IsNullOrEmpty(jobTitle) == true)
            {
                throw new ArgumentException(
                    "The job title must be specified.",
                    "jobTitle");
            }

            float waitTime = 0;

            do
            {
                bool isJobDefined = IsJobDefined(
                    farm,
                    jobTitle);

                if (isJobDefined == false)
                {
                    break;
                }

                bool isJobRunning = IsJobRunning(
                    farm,
                    jobTitle);

                const int sleepTime = 1000; // milliseconds

                Thread.Sleep(sleepTime);
                waitTime += (sleepTime / 1000.0F); // seconds

            } while (waitTime < maximumWaitTime);

            if (waitTime >= maximumWaitTime)
            {
            }
            else
            {
            }
        }
    }
}