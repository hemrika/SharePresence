using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using  Hemrika.SharePresence.Common.Ribbon.Definitions;

namespace Hemrika.SharePresence.Common.Ribbon.Libraries
{
    /// <summary>
    /// Library, which contains Ribbon Group Templates
    /// </summary>
    /// <remarks>
    /// Control groups in the Ribbon accumulate controls, such as buttons, menus, drop downs, and other stuff.
    /// Definining group template, you provide visual control composition.
    /// For example, in one group it could be one large button and three small ones.
    /// </remarks>
    public class GroupTemplateLibrary
    {
        /// <summary>
        /// All templates from the library
        /// </summary>
        public static GroupTemplateDefinition[] AllTemplates
        {
            get
            {
                return new GroupTemplateDefinition[]
                {
                    SimpleTemplate,
                    OneLargeThreeMedium,
                    OneLargeTwoMedium,
                    TwoLargeThreeMedium,
                    TwoLargeThreeMediumTwoLarge,
                    TwoLargeTwoMedium,
                    TwoLargeTwoRowsFourMedium,
                    TwoRowsFourMediumOneLarge,
                    ThreeLargeTwoMedium,
                    ThreeRowTemplate
                };
            }
        }

        /// <summary>
        /// This simple group template allows you create plain ribbon group with unbounded number of large (32x32 icons) controls inside.
        /// This group template contains only one control section.
        /// </summary>
        public static GroupTemplateDefinition SimpleTemplate = new GroupTemplateDefinition()
        {
            Id = " Hemrika.SharePresence.Common.RibbonSimple",
            SizeId = "Large",            
            SectionIds = new string[] { "c1" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
                {
                    return new XDocument(
                        new XElement("GroupTemplate",
                            new XAttribute("Id", definition.Id),
                            new XElement("Layout",
                                new XAttribute("Title", definition.SizeId),
                                new XElement("OverflowSection",
                                    new XAttribute("Type", "OneRow"),
                                    new XAttribute("TemplateAlias", definition.SectionIds.First()),
                                    new XAttribute("DisplayMode", definition.SizeId)
                                    )
                                )
                            )).ToString();
                }
        };

        /// <summary>
        /// This template provides single three-row section for medium-sized controls.
        /// Number of controls is unbounded.
        /// </summary>
        public static GroupTemplateDefinition ThreeRowTemplate = new GroupTemplateDefinition()
        {
            Id = " Hemrika.SharePresence.Common.RibbonThreeRow",
            SizeId = "Medium",
            SectionIds = new string[] { "c1" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return new XDocument(
                    new XElement("GroupTemplate",
                        new XAttribute("Id", definition.Id),
                        new XElement("Layout",
                            new XAttribute("Title", definition.SizeId),
                            new XElement("OverflowSection",
                                new XAttribute("Type", "ThreeRow"),
                                new XAttribute("TemplateAlias", definition.SectionIds.First()),
                                new XAttribute("DisplayMode", definition.SizeId)
                                )
                            )
                        )).ToString();
            }
        };

        public static GroupTemplateDefinition TwoLargeTwoRowsFourMedium = new GroupTemplateDefinition()
        {
                Id = "TwoLargeTwoRowsFourMedium",
                SizeId = "Large",
                SectionIds = new string[] { "c1", "c2", "c3", "c4", "c5", "c6" },
                GetXMLFunc = (GroupTemplateDefinition definition) =>
                {
                    return
                @"<GroupTemplate Id=""" + definition.Id + @""">
	                            <Layout Title=""Large"">
		                            <Section Type=""OneRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
			                            </Row>
		                            </Section>
		                            <Section Type=""OneRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c2"" DisplayMode=""Large"" />
			                            </Row>
		                            </Section>
		                            <Section Type=""TwoRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
			                            </Row>
			                            <Row>
				                            <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
			                            </Row>
		                            </Section>
		                            <Section Type=""TwoRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c5"" DisplayMode=""Medium"" />
			                            </Row>
			                            <Row>
				                            <ControlRef TemplateAlias=""c6"" DisplayMode=""Medium"" />
			                            </Row>
		                            </Section>
	                            </Layout>
                            </GroupTemplate>";
                }
        };

        public static GroupTemplateDefinition TwoRowsFourMediumOneLarge = new GroupTemplateDefinition()
        {
            Id = "TwoRowsFourMediumOneLarge",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4", "c5" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
            @"<GroupTemplate Id=""" + definition.Id + @""">
	                            <Layout Title=""Large"">
		                            <Section Type=""TwoRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c1"" DisplayMode=""Medium"" />
			                            </Row>
			                            <Row>
				                            <ControlRef TemplateAlias=""c2"" DisplayMode=""Medium"" />
			                            </Row>
		                            </Section>
		                            <Section Type=""TwoRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
			                            </Row>
			                            <Row>
				                            <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
			                            </Row>
		                            </Section>
		                            <Section Type=""OneRow"">
			                            <Row>
				                            <ControlRef TemplateAlias=""c5"" DisplayMode=""Large"" />
			                            </Row>
		                            </Section>
	                            </Layout>
                            </GroupTemplate>";
            }
        };

        public static GroupTemplateDefinition TwoLargeTwoMedium = new GroupTemplateDefinition()
        {
            Id = "TwoLargeTwoMedium",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
            @"<GroupTemplate Id=""" + definition.Id + @""">
	                <Layout Title=""Large"">
		                <Section Type=""OneRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
			                </Row>
		                </Section>
		                <Section Type=""OneRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c2"" DisplayMode=""Large"" />
			                </Row>
		                </Section>
                        <Section Type=""TwoRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
			                </Row>
			                <Row>
				                <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
			                </Row>
		                </Section>
	                </Layout>
                </GroupTemplate>";
            }
        };

        public static GroupTemplateDefinition ThreeLargeTwoMedium = new GroupTemplateDefinition()
        {
            Id = "ThreeLargeTwoMedium",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4", "c5" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
            @"<GroupTemplate Id=""" + definition.Id + @""">
	                <Layout Title=""Large"">
		                <Section Type=""OneRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
			                </Row>
		                </Section>
		                <Section Type=""OneRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c2"" DisplayMode=""Large"" />
			                </Row>
		                </Section>
		                <Section Type=""OneRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c3"" DisplayMode=""Large"" />
			                </Row>
		                </Section>
                        <Section Type=""TwoRow"">
			                <Row>
				                <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
			                </Row>
			                <Row>
				                <ControlRef TemplateAlias=""c5"" DisplayMode=""Medium"" />
			                </Row>
		                </Section>
                    </Layout>
                </GroupTemplate>";
            }
        };

        public static GroupTemplateDefinition OneLargeThreeMedium = new GroupTemplateDefinition()
        {

            Id = "OneLargeThreeMedium",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
                     @"<GroupTemplate Id=""" + definition.Id + @""">
                                <Layout Title=""Large"">
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                     <Section Type=""ThreeRow"">
                                        <Row>
                                            <ControlRef TemplateAlias=""c2"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
                                        </Row>
                                  </Section>
                                </Layout>
                              </GroupTemplate>";

            }
        };

        public static GroupTemplateDefinition TwoLargeThreeMedium = new GroupTemplateDefinition()
        {

            Id = "TwoLargeThreeMedium",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4", "c5" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
                     @"<GroupTemplate Id=""" + definition.Id + @""">
                                <Layout Title=""Large"">
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c2"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                     <Section Type=""ThreeRow"">
                                        <Row>
                                            <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c5"" DisplayMode=""Medium"" />
                                        </Row>
                                  </Section>
                                </Layout>
                              </GroupTemplate>";

            }
        };

        public static GroupTemplateDefinition TwoLargeThreeMediumTwoLarge = new GroupTemplateDefinition()
        {

            Id = "TwoLargeThreeMediumTwoLarge",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3", "c4", "c5", "c6", "c7" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
                     @"<GroupTemplate Id=""" + definition.Id + @""">
                                <Layout Title=""Large"">
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c2"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                     <Section Type=""ThreeRow"">
                                        <Row>
                                            <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c4"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c5"" DisplayMode=""Medium"" />
                                        </Row>
                                  </Section>
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c6"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c7"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                </Layout>
                              </GroupTemplate>";

            }
        };

        public static GroupTemplateDefinition OneLargeTwoMedium = new GroupTemplateDefinition()
        {
            Id = "OneLargeTwoMedium",
            SizeId = "Large",
            SectionIds = new string[] { "c1", "c2", "c3" },
            GetXMLFunc = (GroupTemplateDefinition definition) =>
            {
                return
                     @"<GroupTemplate Id=""" + definition.Id + @""">
                                <Layout Title=""Large"">
                                      <Section Type=""OneRow"">
                                            <Row>
                                              <ControlRef TemplateAlias=""c1"" DisplayMode=""Large"" />
                                            </Row>
                                      </Section>
                                     <Section Type=""TwoRow"">
                                        <Row>
                                            <ControlRef TemplateAlias=""c2"" DisplayMode=""Medium"" />
                                        </Row>
                                        <Row>
                                          <ControlRef TemplateAlias=""c3"" DisplayMode=""Medium"" />
                                        </Row>
                                  </Section>
                                </Layout>
                              </GroupTemplate>";

            }
        };
    }
}
