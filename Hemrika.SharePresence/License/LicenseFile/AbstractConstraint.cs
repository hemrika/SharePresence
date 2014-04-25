using System;
using System.ComponentModel;
using System.Xml;
using System.Drawing;

namespace Hemrika.SharePresence.SPLicense.LicenseFile
{
	/// <summary>
	/// <p><c>AbstractConstraint</c> is an abstract class which all licensing
	/// constraints are built from.  The <c>AbstractConstraint</c> defines
	/// the necessary items for a Constraint to be used by
	/// the <see cref=" Hemrika.SharePresence.SPLicenseProvider"> Hemrika.SharePresence.SPLicenseProvider</see>.  The
	/// provider then uses the constraints <c>Validate</c> function to
	/// enforce the Constraint.</p>
	/// </summary>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseProvider"> Hemrika.SharePresence.SPLicenseProvider</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public abstract class AbstractConstraint : AbstractLicenseData, IConstraint
	{
		private	SPLicenseFile		license			= null;
		private	string				name			= String.Empty;
		private string				description		= String.Empty;
		private Icon				icon			= null;
		
		/// <summary>
		/// Defines the validation method of this Constraint.  This
		/// is the method the Constraint uses to accept or reject a
		/// license request.
		/// </summary>
		public abstract bool Validate( );

		/// <summary>
		/// This is used to create the Constraint from a
		/// <see cref="System.String">String</see> representing the Xml data
		/// of a constraint node.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>String</c> representing the XML data for this Constraint.
		/// </param>
		public abstract void FromXml( string xmlData );

		/// <summary>
		/// This loads the XML data for the Constraint from an
		/// <see cref="System.Xml.XmlNode">XmlNode</see>.  The <c>XmlNode</c>
		/// is the piece of the <see cref="System.Xml.XmlDocument">XmlDocument</see>
		/// that is contained within the constraint block of the
		/// <c>XmlDocument</c>.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>XmlNode</c> representing the constraint
		/// of the <c>XmlDocument</c>.
		/// </param>
		public abstract void FromXml( XmlNode xmlData );

		/// <summary>
		/// Destroys this instance of the Constraint.
		/// </summary>
		public void Dispose( )
		{
		}

#region Properties
		/// <summary>
		/// Gets or Sets <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see> this constraint belongs to.
		/// </summary>
		/// <param>
		///	Sets the <c> Hemrika.SharePresence.SPLicenseFile</c> this constraint belongs to.
		/// </param>
		/// <returns>
		///	Gets the <c> Hemrika.SharePresence.SPLicenseFile</c> this constraint belongs to.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		DefaultValue(0),
		Description( "Gets or Sets  Hemrika.SharePresence.SPLicenseFile this constraint belongs to." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public SPLicenseFile License
		{
			get
			{
				return this.license;
			}
			set
			{
				this.license = value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the name of this constraint.
		/// </summary>
		/// <param>
		///	Sets the name of this constraint.
		/// </param>
		/// <returns>
		///	Gets the name of this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(""),
		Description( "Gets or Sets the name of this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// Gets or Sets the description of this constraint.
		/// </summary>
		/// <param>
		///	Sets the description of this constraint.
		/// </param>
		/// <returns>
		///	Gets the description of this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Constraints"),
		DefaultValue(""),
		Description( "Gets or Sets the description of this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// Gets or Sets the icon for this constraint.
		/// </summary>
		/// <param>
		///	Sets the icon for this constraint.
		/// </param>
		/// <returns>
		///	Gets the icon for this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the icon for this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public System.Drawing.Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}
#endregion //Properties
	}
} /* namespace Hemrika.SharePresence.SPLicense */
