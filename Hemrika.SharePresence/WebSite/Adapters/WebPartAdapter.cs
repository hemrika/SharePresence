// -----------------------------------------------------------------------
// <copyright file="WebPartAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPartAdapter: System.Web.UI.Adapters.ControlAdapter
    {
        public WebPartAdapter()
        {

        }

        /*
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        */

        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sb));
            base.Render(writer);
            //AdvancedSearchBox: Adds a label to the Property Drop down box
            sb.Replace("<select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_plb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_plb_0\" title=\"Pick a Property\"", "<label for=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_plb_0\" >Property </label><select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_plb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_plb_0\" title=\"Pick a Property\"");
            //AdvancedSearchBox: Adds a label to the Operator Drop down box
            sb.Replace("<select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_olb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_olb_0\" title=\"Inclusion Operator\"", "<label for=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_olb_0\">Operator </label><select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_olb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_olb_0\" title=\"Inclusion Operator\"");
            //AdvancedSearchBox: Adds a label to the Search Textbox
            sb.Replace("<td class=\"ms-advsrchText\"><input name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_pvtb_0\" type=\"text\" maxlength=\"200\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_pvtb_0\" title=\"Enter Search Phrase\"", "<td class=\"ms-advsrchText\"><td class=\"ms-advsrchText\"><label for=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_pvtb_0\">Search </label><input name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_pvtb_0\" type=\"text\" maxlength=\"200\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_pvtb_0\" title=\"Enter Search Phrase\"");
            //AdvancedSearchBox: Adds a label to the And Or Operator Drop Down Box
            sb.Replace("<select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_lolb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_lolb_0\" title=\"And Or Operator\"", "<label for=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_lolb_0\">And Or </label><select name=\"ctl00$ctl26$g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6$ASB_PS_lolb_0\" id=\"ctl00_ctl26_g_b14d7b0e_c2ec_4afa_9cf6_c661e1c2b8c6_ASB_PS_lolb_0\" title=\"And Or Operator\"");
            //AdvancedSearchBox:  Adds a class ms-advsrchPropValue which resizes the combo box
            sb.Replace("<b>All</b> of these words:</label></td><td class=\"ms-advsrchText\"><input ", "<b>All</b> of these words:</label></td><td class=\"ms-advsrchText\"><input class=\"ms-advsrchPropValue\" ");
            //AdvancedSearchBox:  Adds a class ms-advsrchPropValue which resizes the combo box
            sb.Replace("The exact <b>phrase</b>:</label></td><td class=\"ms-advsrchText\"><input ", "The exact <b>phrase</b>:</label></td><td class=\"ms-advsrchText\"><input class=\"ms-advsrchPropValue\" ");
            //AdvancedSearchBox:  Adds a class ms-advsrchPropValue which resizes the combo box
            sb.Replace("<b>Any</b> of these words:</label></td><td class=\"ms-advsrchText\"><input ", "<b>Any</b> of these words:</label></td><td class=\"ms-advsrchText\"><input class=\"ms-advsrchPropValue\" ");
            //AdvancedSearchBox:  Adds a class ms-advsrchPropValue which resizes the combo box
            sb.Replace("<b>None</b> of these words:</label></td><td class=\"ms-advsrchText\"><input ", "<b>None</b> of these words:</label></td><td class=\"ms-advsrchText\"><input class=\"ms-advsrchPropValue\" ");
            //ChoiceFilter: Adds text before the selected values control saying "Selected Values"
            sb.Replace("<input name=\"ctl00$ctl26$g_673c74e7_4c40_4b8f_8bfe_86e5f1441861$Picker$SelectionBox\"", "Selected Values <input name=\"ctl00$ctl26$g_673c74e7_4c40_4b8f_8bfe_86e5f1441861$Picker$SelectionBox\"");
            //ChoiceFilter: Adds a new class called "ms-choicefiltertxtbx" which resizes the textbox
            sb.Replace("id=\"ctl00_ctl26_g_673c74e7_4c40_4b8f_8bfe_86e5f1441861_Picker_SelectionBox\" title=\"Selected values\" ", "id=\"ctl00_ctl26_g_673c74e7_4c40_4b8f_8bfe_86e5f1441861_Picker_SelectionBox\" title=\"Selected values\" class=\"ms-choicefiltertxtbx\" ");
            //DateFilter:  Adds a label before the date textbox
            sb.Replace("<input name=\"ctl00$ctl26$g_da2a84a5_3f3f_4310_bdd8_267fe6ea0995$DateFilterPicker$DateFilterPickerDate\"", "<label for=\"ctl00_ctl26_g_da2a84a5_3f3f_4310_bdd8_267fe6ea0995_DateFilterPicker_DateFilterPickerDate\">Enter Date </label><input name=\"ctl00$ctl26$g_da2a84a5_3f3f_4310_bdd8_267fe6ea0995$DateFilterPicker$DateFilterPickerDate\"");
            //DateFilter:  Adds an element to the IFRAME
            sb.Replace("<IFRAME id=ctl00_ctl26_g_da2a84a5_3f3f_4310_bdd8_267fe6ea0995_DateFilterPicker_DateFilterPickerDateDatePickerFrame SRC=\"/_layouts/images/blank.gif\" FRAMEBORDER=0 SCROLLING=no style=\"DISPLAY:none;POSITION:absolute; width:200px; Z-INDEX:101;\" title=\"Select a date from the calendar.\"></IFRAME>", "<IFRAME id=ctl00_ctl26_g_da2a84a5_3f3f_4310_bdd8_267fe6ea0995_DateFilterPicker_DateFilterPickerDateDatePickerFrame SRC=\"/_layouts/images/blank.gif\" FRAMEBORDER=0 SCROLLING=no style=\"DISPLAY:none;POSITION:absolute; width:200px; Z-INDEX:101;\" title=\"Select a date from the calendar.\">The Date Picker requires IFRAME support, please ask your System Administrator for help.</IFRAME>");
            //ExcelWebAccess:  Adds alt text to the image of binoculars
            sb.Replace("<img src=\"/_layouts/images/ewr034.gif\" text=\"\" alt=\"\" />", "<img src=\"/_layouts/images/ewr034.gif\" text=\"\" alt=\"Picture of binoculars\" />");
            //ExcelWebAccess:  Adds an element to the IFRAME
            sb.Replace("IsEwrMainIframe=\"true\" EwrStatus=\"\" DummyPage=\"\"></IFrame>", "IsEwrMainIframe=\"true\" EwrStatus=\"\" DummyPage=\"\">The Excel Web Access requires IFRAME support, please ask your System Administrator for help.</IFrame>");
            //ExcelWebAccess:  Adds an element to the IFRAMe
            sb.Replace("style=\"display:none;\" DummyPage=\"\"></IFRAME>", "style=\"display:none;\" DummyPage=\"\">The Excel Web Access requires IFRAME support, please ask your System Administrator for help.</IFRAME>");
            //FormWebPart:  Adds a label for the default textbox of "Enter Value"
            sb.Replace("<input type=\"text\" name=\"WPQ2T1\"/>", "<label for=\"T1\">Enter Value </label><input type=\"text\" id=\"T1\" name=\"WPQ2T1\"/>");
            //INeedTo:  Adds a label to the "Choose task" drop down box
            sb.Replace("<select id=\"TasksAndToolsDDID\"", "<Label for=\"TasksAndToolsDDID\" id=\"TasksAndToolsDDIDlbl\">Select Option</label><select id=\"TasksAndToolsDDID\"");
            //PeopleSearchBox:  Adds a label of "Enter Search Words" before the "Enter Search Values" textbox
            sb.Replace("<td class=\"ms-sbcell\"><input name=\"ctl00$ctl26$g_37899a25_94a5_469a_a598_0abc39713416$S9B7CCA8C_InputKeywords\" type=\"text\" maxlength=\"200\" id=\"ctl00_ctl26_g_37899a25_94a5_469a_a598_0abc39713416_S9B7CCA8C_InputKeywords\" accesskey=\"S\" title=\"Enter search words\" class=\"ms-sbplain\" alt=\"Enter search words\"", "<label for=\"ctl00_ctl26_g_37899a25_94a5_469a_a598_0abc39713416_S9B7CCA8C_InputKeywords\">Enter Search Words </label><td class=\"ms-sbcell\"><input name=\"ctl00$ctl26$g_37899a25_94a5_469a_a598_0abc39713416$S9B7CCA8C_InputKeywords\" type=\"text\" maxlength=\"200\" id=\"ctl00_ctl26_g_37899a25_94a5_469a_a598_0abc39713416_S9B7CCA8C_InputKeywords\" accesskey=\"S\" class=\"ms-sbplain\"");
            //PeopleSearchBox:  Removes the image of the black arrow on the right of the textbox
            sb.Replace("<img border=0 valign='top' style='padding-right:4px;' src='/_layouts/images/blk_rgt.gif' alt=''/>", "");
            //PeopleSearchBox:  Removes the image of the black arrow on the right of the textbox
            sb.Replace("<img valign='top' border=0 src='/_layouts/images/blk_dwn.gif' alt='Search Options'/>", "");
            //RelevantDocuments:  Adds "Checked Out" alt text to the green box with a white arrow
            sb.Replace("<img src=\"/_layouts/images/checkoutoverlay.gif\" class=\"ms-vb-icon-overlay\">", "<img src=\"/_layouts/images/checkoutoverlay.gif\" class=\"ms-vb-icon-overlay\" alt=\"Checked Out\">");
            //SearchBox:  Adds a label "Search Scope" before the "Search Scope" drop down box
            sb.Replace("<select name=\"ctl00$ctl26$g_c61df438_ae30_41c9_87aa_dabce3d02297$SBScopesDDL\"", "<label for=\"ctl00_ctl26_g_c61df438_ae30_41c9_87aa_dabce3d02297_SBScopesDDL\">Search Scope</label><select name=\"ctl00$ctl26$g_c61df438_ae30_41c9_87aa_dabce3d02297$SBScopesDDL\"");
            //SearchBox:  Adds a label "Enter Search Words" before the textbox
            sb.Replace("<input name=\"ctl00$ctl26$g_c61df438_ae30_41c9_87aa_dabce3d02297$S4282A5AC_InputKeywords\"", "<label for=\"ctl00_ctl26_g_c61df438_ae30_41c9_87aa_dabce3d02297_S4282A5AC_InputKeywords\" >Enter search words</label><input name=\"ctl00$ctl26$g_c61df438_ae30_41c9_87aa_dabce3d02297$S4282A5AC_InputKeywords\"");
            //SharepointListFilter:  Adds text displaying "Selected values" before the Selected values box
            sb.Replace("<input name=\"ctl00$ctl26$g_ca77288e_f4b2_4cb9_940b_b5be27ce5aff$Picker$SelectionBox\"", "Selected values<input name=\"ctl00$ctl26$g_ca77288e_f4b2_4cb9_940b_b5be27ce5aff$Picker$SelectionBox\"");
            //SharepointListFilter:  Adds a class "ms-sharepointlistfiltertxtbx" that sizes the textbox
            sb.Replace("id=\"ctl00_ctl26_g_ca77288e_f4b2_4cb9_940b_b5be27ce5aff_Picker_SelectionBox\" title=\"Selected values\" ", "id=\"ctl00_ctl26_g_ca77288e_f4b2_4cb9_940b_b5be27ce5aff_Picker_SelectionBox\" title=\"Selected values\" class=\"ms-sharepointlistfiltertxtbx\"");
            //SiteAggregator:  Adds an element to the IFRAME
            sb.Replace("(this, 0, 325);}\" src = '/_layouts/images/blank.gif' ></IFrame>", "(this, 0, 325);}\" src = '/_layouts/images/blank.gif' >The Site Aggregator requires IFRAME support, please ask your System Administrator for help.</IFrame>");
            //SiteAggregator:  Adds a label of "Enter URL" before the "SiteURL" textbox.  Also adds the id of SiteURL to the input.
            sb.Replace("<input name=\"ctl00$ctl26$g_873f387b_56ff_4f7c_ab28_618967863c96$ctl07\"", "<label for=\"SiteURL\">Enter URL </label><input id=\"SiteURL\" name=\"ctl00$ctl26$g_873f387b_56ff_4f7c_ab28_618967863c96$ctl07\"");
            //SiteAggregator:  Adds a label of "Enter Name" before the "Site Name" textbox.
            sb.Replace("<input name=\"ctl00$ctl26$g_873f387b_56ff_4f7c_ab28_618967863c96$ctl11\"", "<label for=\"ctl00_ctl26_g_873f387b_56ff_4f7c_ab28_618967863c96_ctl11\">Enter Name </label><input name=\"ctl00$ctl26$g_873f387b_56ff_4f7c_ab28_618967863c96$ctl11\"");
            //TextFilter:  Adds a label "Text Filter" before the Textbox
            sb.Replace("<input name=\"ctl00$ctl26$g_734d8e45_4336_49ba_be3f_33531977077f$SPTextSlicerValueTextControl\"", "<label for=\"ctl00_ctl26_g_734d8e45_4336_49ba_be3f_33531977077f_SPTextSlicerValueTextControl\">Text Filter</label><input name=\"ctl00$ctl26$g_734d8e45_4336_49ba_be3f_33531977077f$SPTextSlicerValueTextControl\"");
            //Textfilter:  Adds a class "ms-textfiltertextbox" which allows the textbox to enlarge
            sb.Replace("<input name=\"ctl00$ctl26$g_734d8e45_4336_49ba_be3f_33531977077f$SPTextSlicerValueTextControl\" ", "<input name=\"ctl00$ctl26$g_734d8e45_4336_49ba_be3f_33531977077f$SPTextSlicerValueTextControl\" class=\"ms-textfiltertextbox\"");

            output.Write(sb.ToString());
        }
    }
}