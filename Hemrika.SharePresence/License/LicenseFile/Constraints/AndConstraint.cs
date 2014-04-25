using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Hemrika.SharePresence.SPLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='AndConstraint'/> contains a collection of constraints that 
	/// will be grouped together as a bitwise AND operation.  It is responsible for 
	/// validating the containing <c>IConstraints</c> and will be valid as long as 
	/// all the constraints contained are valid.  The purpose of this is to allow 
	/// a user to force multiple constraints to pass before allowing the license 
	/// to be valid.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	public class AndConstraint : AbstractContainerConstraint
	{
		/// <summary>
		/// This is the constructor for the <c>AndConstraint</c>.  The constructor
		/// is used to create the object without a valid license attached it to.
		/// </summary>
		public AndConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>AndConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public AndConstraint(SPLicenseFile license )
		{
			base.License		= license;
			base.Name			= "And Constraint";
			base.Description	=  "This AndConstraint contains a collection of constraints that ";
			base.Description	+= "will be grouped together as a bitwise AND operation.  It is ";
			base.Description	+= "validating the containing IConstraints and will be valid ";
			base.Description	+= "all the constraints contained are valid.  The purpose of this ";
			base.Description	+= "is to allow a user to force multiple constraints to pass before ";
			base.Description	+= "allowing the license to be valid.";
		}

		/// <summary>
		/// This verifies the license meets its desired validation criteria.  This test will 
		/// pass providing all contained constraints are valid. If any one of them is invalid then 
		/// the validate will fail and the license failure reason will be set to the first 
		/// constraint which failed while being validated.
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to the details of the
		/// failure for the last constraint which failed.
		/// </remarks>
		public override bool Validate( )
		{
			for( int i = 0; i < this.Items.Count; i++ )
			{
				if( !((IConstraint)this.Items[i]).Validate( ) )
				{
					return false;
				}
			}
			
			if( base.License != null )
				base.License.FailureReason = String.Empty;

			return true;
		}

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>AndConstriant</c>.
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
		/// This creates an <c>AndConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>AndConstraint</c>.
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
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;

			parseConstraintsFields( itemsNode );
		}

		/// <summary>
		/// Parse the XML content of the constraints group/fields section of the license.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the
		/// Constraints List (System.Collections.Generic.List) section of the
		/// license.
		/// </param>
		private void parseConstraintsFields( XmlNode itemsNode )
		{
			// Check if custom fields are defined
			if( itemsNode == null )
				return;

			// If they are then process all of them
			XmlNodeList constraints = itemsNode.ChildNodes;

			for( int i = 0; i < constraints.Count; i++ )
			{
				XmlNode constraint		= constraints[i];
				XmlNode typeTextNode	= constraint.SelectSingleNode( "Type/text()" );

				if( typeTextNode != null )
				{
					Type constraintType = Type.GetType( (String)typeTextNode.Value , false, true );
					ConstructorInfo cInfo = constraintType.GetConstructor( new Type[] {typeof(  Hemrika.SharePresence.SPLicense.LicenseFile.SPLicenseFile )} );
					IConstraint c = (IConstraint)cInfo.Invoke( new Object[] { this.License } );
					c.FromXml( constraint );
					this.Items.Add( c );
				}
			}
		}

		/// <summary>
		/// Converts this <c>AndConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the IConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			if( this.Items != null && this.Items.Count > 0 )
			{
				xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type", 				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );
				for( int i = 0; i < this.Items.Count; i++ )
				{
					xmlWriter.WriteRaw( ( (IConstraint)Items[i] ).ToXmlString( ) );
				}
				xmlWriter.WriteEndElement( ); // constraints
			}

			xmlWriter.Close( );

			return xmlString.ToString( );
		}
	}
} /* namespace Hemrika.SharePresence.SPLicense */
