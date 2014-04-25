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
	/// <p>This <see cref='VersionConstraint'/> constrains the user to only use the license
	/// with a given range of versions attached to an Assembly.  For example if an assembly
	/// version is 0.95.0.0 and the version range is 0.94.0.0 to 0.96.0.0 then the license
	/// will pass.  However if the assembly is then upgrade to version 1.0.0.0 then the license
	/// will expire.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	public class VersionConstraint : AbstractConstraint
	{
		private Version	minVersion	= new Version( );
		private Version maxVersion	= new Version( );

		/// <summary>
		/// This is the constructor for the <c>VersionConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public VersionConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>VersionConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public VersionConstraint(SPLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Version Constraint";
			base.Description	=  "This VersionConstraint constrains the user to only use the license ";
			base.Description	+= "with a given range of versions attached to an Assembly.";
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
			if( this.Minimum.Equals( new Version( ) ) && this.Maximum.Equals( new Version( ) ) )
			{
				if( base.License != null )
					base.License.FailureReason = String.Empty;

				return true;
			}
			
			if(	Assembly.GetAssembly(base.License.Type).GetName().Version <= this.Minimum ||
  				Assembly.GetAssembly(base.License.Type).GetName().Version >= this.Maximum )
			{
				if( base.License != null )
					base.License.FailureReason = "The current version is not within the constraints of this license.";

				return false;
			}

			if( base.License != null )
				base.License.FailureReason = String.Empty;

			return true;
		}

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>VersionConstriant</c>.
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
		/// This creates a <c>VersionConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>VersionConstraint</c>.
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
			XmlNode minVersionTextNode		= itemsNode.SelectSingleNode( "MinimumVersion/text()" );
			XmlNode maxVersionTextNode		= itemsNode.SelectSingleNode( "MaximumVersion/text()" );

			if( nameTextNode != null )
				this.Name	= nameTextNode.Value;

			if( descriptionTextNode != null )
				this.Description	= descriptionTextNode.Value;

			if( minVersionTextNode != null )
				this.Minimum		= new Version( minVersionTextNode.Value );

			if( maxVersionTextNode != null )
				this.Maximum		= new Version( maxVersionTextNode.Value );

		}

		/// <summary>
		/// Converts this <c>VersionConstraint</c> to an Xml <c>String</c>.
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
				xmlWriter.WriteElementString( "MinimumVersion",		this.Minimum.ToString( ) );
				xmlWriter.WriteElementString( "MaximumVersion",		this.Maximum.ToString( ) );
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
		public Version Minimum
		{
			get
			{
				return this.minVersion;
			}
			set
			{
				if( !this.Minimum.Equals( value ) )
				{
					this.minVersion = value;
					this.isDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the maximum version allowed for this license.
		/// </summary>
		/// <param>
		///	Sets the maximum version allowed for this license.
		/// </param>
		/// <returns>
		///	Gets the maximum version allowed for this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or sets the maximum version allowed for this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public Version Maximum
		{
			get
			{
				return this.maxVersion;
			}
			set
			{
				if( !this.Maximum.Equals( value ) )
				{
					this.maxVersion = value;
					this.isDirty = true;
				}
			}
		}
#endregion
	}
} /* namespace Hemrika.SharePresence.SPLicense */

