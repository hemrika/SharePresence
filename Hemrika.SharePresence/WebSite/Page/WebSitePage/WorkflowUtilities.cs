// -----------------------------------------------------------------------
// <copyright file="WorkflowUtilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.Xml;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Workflow;
    using System.IO;
    using Microsoft.SharePoint.Utilities;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WorkflowUtilities
    {
        internal const string EditCancelsNoCodeWorkflowNodeName = "CancelonChange";
        internal const string EditCancelsO12WorkflowNodeName = "ItemChangeStop";

        internal static void AddElementsRecursively(ref Hashtable ht, System.Xml.XmlNode node)
        {
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                string str = XmlConvert.DecodeName(node2.LocalName);
                if (node2.HasChildNodes)
                {
                    if ((node2.ChildNodes.Count == 1) && !node2.FirstChild.HasChildNodes)
                    {
                        ht[str] = node2.InnerText;
                    }
                    else
                    {
                        AddElementsRecursively(ref ht, node2);
                    }
                    continue;
                }
                ht[str] = node2.InnerText;
            }
        }

        internal static void CancelApprovalWorkflowsOnCurrentItem()
        {
            SPListItem contextualListItem = SPContext.Current.ListItem;
            if (contextualListItem != null)
            {
                foreach (SPWorkflow workflow in contextualListItem.Workflows)
                {
                    if (((workflow.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running) && DoesWorkflowCancelWhenItemEdited(workflow.ParentAssociation))
                    {
                        using (new SPSecurity.SuppressAccessDeniedRedirectInScope())
                        {
                            try
                            {
                                CancelWorkflowAppropriately(workflow, SPContext.Current.Site);
                            }
                            catch (UnauthorizedAccessException)
                            {
                            }
                            continue;
                        }
                    }
                }
            }
        }

        public static void CancelWorkflowAppropriately(SPWorkflow workflow, SPSite site)
        {
            bool flag = false;
            try
            {
                //E43856D2-1BB4-40ef-B08B-016D89A0
                Guid objB = new Guid("a938eabe-8db1-45b9-87cb-b930728afe10");
                foreach (SPWorkflowModification modification in workflow.Modifications)
                {
                    if (object.Equals(modification.TypeId, objB))
                    {
                        site.WorkflowManager.ModifyWorkflow(workflow, modification, null);
                        flag = true;
                        break;
                    }
                }
                //ULS.SendTraceTag(0x38747333, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Verbose, "Simple cancellation on workflow: {0} for item: {1} ", new object[] { workflow.AssociationId.ToString(), workflow.ParentItem.Url });
            }
            catch (ArgumentNullException)
            {
                flag = false;
            }
            catch (ArgumentException)
            {
                flag = false;
            }
            catch (SPException)
            {
                flag = false;
            }
            if (!flag)
            {
                SPWorkflowManager.CancelWorkflow(workflow);
                //ULS.SendTraceTag(0x38747334, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Verbose, "SPWorkflowManager.CancelWorkflow(), full cancellation on workflow: {0} for item: {1} ", new object[] { workflow.AssociationId.ToString(), workflow.ParentItem.Url });
            }
        }

        internal static bool DoesWorkflowCancelWhenItemEdited(SPWorkflow wf)
        {
            return DoesWorkflowCancelWhenItemEdited(wf.ParentAssociation);
        }

        internal static bool DoesWorkflowCancelWhenItemEdited(SPWorkflowAssociation wf)
        {
            return DoesWorkflowCancelWhenItemEdited(wf.AssociationData);
        }

        internal static bool DoesWorkflowCancelWhenItemEdited(string associationXml)
        {
            Hashtable hashtable = FlattenXmlToHashtable(associationXml);
            bool flag = false;
            string str = string.Empty;
            if (hashtable.Contains("CancelonChange"))
            {
                str = (string)hashtable["CancelonChange"];
            }
            else if (hashtable.Contains("ItemChangeStop"))
            {
                str = (string)hashtable["ItemChangeStop"];
            }
            if (!string.IsNullOrEmpty(str))
            {
                flag = bool.Parse(str);
            }
            return flag;
        }

        internal static Hashtable FlattenXmlToHashtable(string strXml)
        {
            Hashtable ht = new Hashtable();
            XmlDocument document = new XmlDocument();
            using (XmlTextReader reader = new XmlTextReader(new StringReader(strXml)))
            {
                reader.DtdProcessing = DtdProcessing.Prohibit;
                //reader.ProhibitDtd = true;
                document.Load(reader);
            }
            System.Xml.XmlNode firstChild = document.FirstChild;
            AddElementsRecursively(ref ht, firstChild);
            return ht;
        }

        internal static SPWorkflowTask GetCurrentUserApprovalTask(SPListItem listItem)
        {
            SPWorkflowTask task = null;
            SPUser contextualUser = SPContext.Current.Web.CurrentUser;

            if ((contextualUser != null) && (listItem.ParentList.DefaultContentApprovalWorkflowId != Guid.Empty))
            {
                int iD = contextualUser.ID;
                foreach (SPWorkflow workflow in listItem.Workflows)
                {
                    if ((workflow.AssociationId == listItem.ParentList.DefaultContentApprovalWorkflowId) && ((workflow.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running))
                    {
                        foreach (SPWorkflowTask task2 in workflow.Tasks)
                        {
                            if (!Convert.ToBoolean(task2[SPBuiltInFieldId.Completed]) && GetIsTaskAssignedToUserOrGroup(task2, iD))
                            {
                                task = task2;
                                break;
                            }
                        }
                        continue;
                    }
                }
            }
            return task;
        }

        internal static List<SPWorkflowTask> GetCurrentUserWorkflowTasks(SPListItem listItem)
        {
            List<SPWorkflowTask> list = new List<SPWorkflowTask>();
            SPUser contextualUser = SPContext.Current.Web.CurrentUser;
            if (contextualUser != null)
            {
                int iD = contextualUser.ID;
                try
                {
                    foreach (SPWorkflow workflow in listItem.Workflows)
                    {
                        foreach (SPWorkflowTask task in workflow.Tasks)
                        {
                            if (!Convert.ToBoolean(task[SPBuiltInFieldId.Completed]) && GetIsTaskAssignedToUserOrGroup(task, iD))
                            {
                                list.Add(task);
                            }
                        }
                    }
                }
                catch (NullReferenceException)
                {
                }
            }
            return list;
        }

        private static bool GetIsTaskAssignedToUserOrGroup(SPWorkflowTask task, int userId)
        {
            SPFieldLookupValue value2 = new SPFieldLookupValue(task[SPBuiltInFieldId.AssignedTo].ToString());
            int lookupId = value2.LookupId;
            return ((lookupId == userId) || SPContext.Current.Web.IsCurrentUserMemberOfGroup(lookupId));
        }

        internal static SPWorkflow GetRunningDefaultApprovalWorkflow(SPListItem item)
        {
            if (item.ParentList.DefaultContentApprovalWorkflowId != Guid.Empty)
            {
                foreach (SPWorkflow workflow2 in item.Workflows)
                {
                    if (((workflow2.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running) && (workflow2.AssociationId == item.ParentList.DefaultContentApprovalWorkflowId))
                    {
                        return workflow2;
                    }
                }
            }
            return null;
        }

        /*
        internal static SPWorkflow GetRunningDefaultApprovalWorkflow(SPListItem item)
        {
            Guid defaultContentApprovalWorkflowId = item.ParentList.DefaultContentApprovalWorkflowId;
            if (defaultContentApprovalWorkflowId != Guid.Empty)
            {
                foreach (SPWorkflow workflow2 in item.Workflows)
                {
                    if (((workflow2.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running) && (workflow2.AssociationId == defaultContentApprovalWorkflowId))
                    {
                        return workflow2;
                    }
                }
            }
            return null;
        }
        */

        /*
        internal static string GetTaskNavigationUrl(SPWorkflowTask task)
        {
            SPList parentList = task.ParentList;
            return (SiteAdminUrls.GetUrl(SiteAdminPages.WorkflowTask) + "?List=" + SPHttpUtility.UrlKeyValueEncode(parentList.ID.ToString()) + "&ID=" + SPHttpUtility.UrlKeyValueEncode(task.ListItemId.ToString(CultureInfo.InvariantCulture)) + "&Source=" + SPHttpUtility.UrlKeyValueEncode(ConsoleUtilities.RemoveAuthoringErrors(ConsoleUtilities.CleanDisplayModeUrl(HttpContext.Current.Request.Url.PathAndQuery))));
        }
        */

        internal static bool IsApprovalWorkflowTaskActiveForUser(SPListItem listItem)
        {
            return (GetCurrentUserApprovalTask(listItem) != null);
        }

        internal static bool IsDefaultApprovalWorkflowConfigured(SPListItem listItem)
        {
            return (listItem.ParentList.DefaultContentApprovalWorkflowId != Guid.Empty);
        }

        internal static bool IsDefaultApprovalWorkflowRunning(SPListItem item)
        {
            return (null != GetRunningDefaultApprovalWorkflow(item));
        }

        internal static void SetApprovalWorkflowAssociationData(SPWorkflowAssociation approvalWorkflow, SPWeb rootWeb)
        {
            string approverGroupName;
            XmlDocument document = new XmlDocument();
            using (XmlTextReader reader = new XmlTextReader(new StringReader(approvalWorkflow.AssociationData)))
            {
                reader.DtdProcessing = DtdProcessing.Prohibit;
                //reader.ProhibitDtd = true;
                document.Load(reader);
            }
            System.Xml.XmlNode documentElement = document.DocumentElement;
            System.Xml.XmlNode parentNode = FindChildXmlNodeByName(documentElement, "dataFields");
            if (parentNode == null)
            {
                //ULS.SendTraceTag(0x63796437, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetApprovalWorkflowAssociationData: could not find node {0} in parent node {1}", new object[] { "dataFields", documentElement.Name });
            }
            else
            {
                System.Xml.XmlNode node3 = FindChildXmlNodeByName(parentNode, "SharePointListItem_RW");
                if (node3 == null)
                {
                    //ULS.SendTraceTag(0x63796438, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetApprovalWorkflowAssociationData: could not find node {0} in parent node {1}", new object[] { "SharePointListItem_RW", parentNode.Name });
                }
                else
                {
                    System.Xml.XmlNode node4 = FindChildXmlNodeByName(node3, "Approvers");
                    if (node4 == null)
                    {
                        //ULS.SendTraceTag(0x63796439, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetApprovalWorkflowAssociationData: could not find node {0} in parent node {1}", new object[] { "Approvers", node3.Name });
                    }
                    else
                    {
                        System.Xml.XmlNode node5 = FindChildXmlNodeByName(node4, "Assignment");
                        if (node5 == null)
                        {
                            //ULS.SendTraceTag(0x63796530, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetApprovalWorkflowAssociationData: could not find node {0} in parent node {1}", new object[] { "Assignment", node4.Name });
                        }
                        else
                        {
                            System.Xml.XmlNode node6 = FindChildXmlNodeByName(node5, "Assignee");
                            if (node6 == null)
                            {
                                //ULS.SendTraceTag(0x63796531, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetApprovalWorkflowAssociationData: could not find node {0} in parent node {1}", new object[] { "Assignee", node5.Name });
                            }
                            else
                            {
                                approverGroupName = string.Empty;
                                RunWithWebCulture(rootWeb, delegate
                                {
                                    approverGroupName = SPResource.GetString("GroupNameApprovers");
                                });
                                System.Xml.XmlNode newChild = document.CreateNode(XmlNodeType.Element, "pc", "Person", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
                                System.Xml.XmlNode node8 = document.CreateNode(XmlNodeType.Element, "pc", "DisplayName", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
                                node8.InnerText = approverGroupName;
                                System.Xml.XmlNode node9 = document.CreateNode(XmlNodeType.Element, "pc", "AccountId", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
                                node9.InnerText = approverGroupName;
                                System.Xml.XmlNode node10 = document.CreateNode(XmlNodeType.Element, "pc", "AccountType", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
                                node10.InnerText = "SpUser";
                                newChild.AppendChild(node8);
                                newChild.AppendChild(node9);
                                newChild.AppendChild(node10);
                                node6.AppendChild(newChild);
                                SetAssociationDataXmlNode(node5, "AssignmentType", "Parallel");
                                SetAssociationDataXmlNode(node3, "EnableContentApproval", "true");
                                SetAssociationDataXmlNode(node3, "CancelonChange", "true");
                                SetAssociationDataXmlNode(node3, "CancelonRejection", "true");
                                SetAssociationDataXmlNode(node3, "ExpandGroups", "false");
                                approvalWorkflow.AssociationData = documentElement.OuterXml;
                            }
                        }
                    }
                }
            }
        }

        internal static void RunWithWebCulture(SPWeb web, Microsoft.SharePoint.Utilities.SPSafeThread.CodeToRun webCultureDependentCode)
        {
            RunWithWebCultureScope(web, delegate
            {
                webCultureDependentCode();
            });
        }

        public delegate void CodeToRunWithCultureScope();

        public static void RunWithWebCultureScope(SPWeb web, CodeToRunWithCultureScope code)
        {
            if (web == null)
            {
                throw new ArgumentNullException("web");
            }
            if (code == null)
            {
                throw new ArgumentNullException("code");
            }
            RunWithCultureScope(delegate
            {
                Thread.CurrentThread.CurrentCulture = web.Locale;
                Thread.CurrentThread.CurrentUICulture = web.UICulture;
                code();
            });
        }

        public static void RunWithCultureScope(CodeToRunWithCultureScope code)
        {
            if (code == null)
            {
                throw new ArgumentNullException("code");
            }
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                code();
            }
            catch
            {
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
            }
        }

 

 

        private static XmlNode FindChildXmlNodeByName(System.Xml.XmlNode parentNode, string nodeName)
        {
            System.Xml.XmlNode firstChild = parentNode.FirstChild;
            System.Xml.XmlNode node2 = null;
            while ((firstChild != null) && (node2 == null))
            {
                if (string.Compare(firstChild.LocalName, nodeName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    node2 = firstChild;
                }
                firstChild = firstChild.NextSibling;
            }
            return node2;
        }

        internal static void SetAssociationDataXmlNode(System.Xml.XmlNode parentNode, string xmlNodeName, string xmlNodeInnerText)
        {
            System.Xml.XmlNode node = FindChildXmlNodeByName(parentNode, xmlNodeName);
            if (node != null)
            {
                node.InnerText = xmlNodeInnerText;
                System.Xml.XmlAttribute attribute = node.Attributes["xsi:nil"];
                if (attribute != null)
                {
                    node.Attributes.Remove(attribute);
                }
            }
            else
            {
                //ULS.SendTraceTag(0x63796532, ULSCat.msoulscat_CMS_Provisioning, ULSTraceLevel.Unexpected, "SetAssociationDataXmlNode: could not find requested node {0} in parent node {1}", new object[] { xmlNodeName, parentNode.Name });
            }
        }
    }
}
