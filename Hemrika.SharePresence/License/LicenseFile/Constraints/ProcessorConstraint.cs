using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Hemrika.SharePresence.SPLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='ProcessorConstraint'/> constrains the user to only use the license
	/// with a given OS or Processor</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	public class ProcessorConstraint : AbstractConstraint
	{
		/// <summary>
		/// This is the constructor for the <c>ProcessorConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public ProcessorConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>ProcessorConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public ProcessorConstraint(SPLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Processor Constraint";
			base.Description	=  "This ProcessorConstraint constrains the user to only use the license ";
			base.Description	+= "with a given OS or Processor.";
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the assembly version is within the licenses defined range.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to: "The current version is not
		/// within the constraints of this license."
		/// </remarks>
		public override bool Validate( )
		{
			throw new ApplicationException( "Not implemented yet." );
		}

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>ProcessorConstraint</c>.
		/// </summary>
		/// <param name="xmlData">
		/// The Xml data in the form of a <c>String</c>.
		/// </param>
		public override void FromXml( string xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			FromXml( xmlDoc.SelectSingleNode("/Constraint") );
		}

		/// <summary>
		/// This creates a <c>ProcessorConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>ProcessorConstraint</c>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode nameTextNode			= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode descriptionTextNode		= itemsNode.SelectSingleNode( "Description/text()" );

			if( nameTextNode != null )
				this.Name	= nameTextNode.Value;

			if( descriptionTextNode != null )
				this.Description	= descriptionTextNode.Value;

		}

		/// <summary>
		/// Converts this <c>ProcessorConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the IConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type", 				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );
			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

#region Properties
		/// <summary>
		/// Gets or Sets the minimum version allowed for this license.
		/// </summary>
		/// <param>
		///	Sets the minimum version allowed for this license.
		/// </param>
		/// <returns>
		///	Gets the minimum version allowed for this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(0.0),
		Description( "Gets or sets the minimum version allowed for this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int OS
		{
			get
			{
				return -1;
			}
			set
			{
				
			}
		}
#endregion
	}
} /* namespace Hemrika.SharePresence.SPLicense */

