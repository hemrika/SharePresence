using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;


namespace Hemrika.SharePresence.SPLicense.LicenseFile
{
	/// <summary>
	/// The <c>Product</c> object inherits from the <see cref="AbstractLicenseData"/>.  The
	/// <c>Product</c> is the information about the assembly this license is used for.  It
	/// contains values for the Assembly, name, version, etc...
	/// </summary>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	/// <seealso cref="AbstractLicenseData">AbstractLicenseData</seealso>
    [Serializable]
	public class Product : AbstractLicenseData
	{
		// Product information
		private Assembly				assembly		= null;
		private bool					isInGac			= false;
		private string					filePath		= String.Empty;
		private string					shortName		= String.Empty;
		private string					fullName		= String.Empty;
		private string					version			= String.Empty;
		private string					developer		= String.Empty;
		private string					description		= String.Empty;
		private bool					isLicensed		= false;

		/// <summary>
		/// This is a static method that creates an <c>Product</c> from the passed in XML
		/// <see cref="System.String">String</see>.
		/// </summary>
		/// <param>
		/// The <see cref="System.String">String</see> representing the Xml data.
		/// </param>
		/// <returns>
		/// The <c>Product</c> created from parsing this <see cref="System.String">String</see>.
		/// </returns>
		public static Product FromXml( String xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument( );
			xmlDoc.LoadXml( xmlData );

			return FromXml( xmlDoc.SelectSingleNode("/Product") );
		}

		/// <summary>
		/// This is a static method that creates an <c>Product</c> from a <see cref="XmlNode">XmlNode</see>.
		/// </summary>
		/// <param>
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>Product</c>.
		/// </param>
		/// <returns>
		/// The <c>Product</c> created from this <see cref="XmlNode">XmlNode</see>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the license data is null.
		/// </exception>
		public static Product FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			Assembly	assembly	= null;
			bool		isInGac		= false;
			string		filePath	= String.Empty;
			string		shortName	= String.Empty;
			string		fullName	= String.Empty;
			string		version		= String.Empty;
			string		developer	= String.Empty;
			string		description	= String.Empty;
			bool		isLicensed	= false;

			XmlNode assemblyTextNode	= itemsNode.SelectSingleNode( "Assembly/text()" );
			XmlNode gacTextNode			= itemsNode.SelectSingleNode( "IsInGac/text()" );
			XmlNode filePathTextNode	= itemsNode.SelectSingleNode( "FilePath/text()" );
			XmlNode snameTextNode		= itemsNode.SelectSingleNode( "ShortName/text()" );
			XmlNode fnameTextNode		= itemsNode.SelectSingleNode( "FullName/text()" );
			XmlNode versionTextNode		= itemsNode.SelectSingleNode( "Version/text()" );
			XmlNode devevloperTextNode	= itemsNode.SelectSingleNode( "Developer/text()" );
			XmlNode descriptionTextNode	= itemsNode.SelectSingleNode( "Description/text()" );
			XmlNode isLicensedTextNode	= itemsNode.SelectSingleNode( "IsLicensed/text()" );

			if( filePathTextNode != null )
			{
				filePath = filePathTextNode.Value;
				if( filePath != String.Empty )
				{
					assembly = LoadAssemblyFromFile( filePath );
				}
			}

			if( assembly == null && assemblyTextNode != null )
			{
				assembly = LoadAssemblyFromPartialName( assemblyTextNode.Value );
			}

			if( gacTextNode != null )
				isInGac = Convert.ToBoolean( (string)gacTextNode.Value );

			if( snameTextNode != null )
				shortName = snameTextNode.Value;

			if( fnameTextNode != null )
				fullName = fnameTextNode.Value;

			if( versionTextNode != null )
				version = versionTextNode.Value;

			if( devevloperTextNode != null )
				developer = devevloperTextNode.Value;

			if( descriptionTextNode != null )
				description = descriptionTextNode.Value;

			if( isLicensedTextNode != null )
				isLicensed	= Convert.ToBoolean( (string)isLicensedTextNode.Value );

			if( assembly == null )
			{
				//TODO: Support getting an error up to OLB to be displayed.
			}

			return new Product( assembly, isInGac, filePath, shortName, fullName, version, developer, description,
			                   	isLicensed );
		}

		/// <summary>
		/// A static method for obtaining the Assembly from a given path.
		/// </summary>
		/// <param name="path">
		/// The path to the given assembly to be loaded.
		/// </param>
		/// <returns>
		/// The Assembly if it was loaded.  Otherwise null.
		/// </returns>
		public static Assembly LoadAssemblyFromFile( string path )
		{
			Assembly a = null;

			try
			{
				a = Assembly.LoadFile( path );
			}
			catch
			{
				return null;
			}

			return a;
		}

		/// <summary>
		/// A static method for obtaining the Assembly from a partial name.  This method
		/// will only work if the Assembly is located in the GAC or the application bin
		/// directory.
		/// </summary>
		/// <param name="name">
		/// The path to the given assembly to be loaded.
		/// </param>
		/// <returns>
		/// The Assembly if it was loaded.  Otherwise null.
		/// </returns>
		public static Assembly LoadAssemblyFromPartialName( string name )
		{
			Assembly a = null;
			try
			{
				//a = Assembly.LoadWithPartialName( name );
				a = Assembly.Load( name );
			}
			catch
			{
				return null;
			}

			return a;
		}

		/// <summary>
		/// This initializes an empty <c>Product</c>.
		/// </summary>
		public Product( )
				: this( null, false, "", "", "", "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in value.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		public Product( Assembly a )
				: this( a, false, "", "", "", "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		public Product( Assembly a, bool gac )
				: this( a, gac, "", "", "", "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		public Product( Assembly a, bool gac, string path )
				: this( a, gac, path, "", "", "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname )
				: this( a, gac, path, sname, "", "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		/// <param name="fname">
		/// The full name of this <c>Assembly</c>.
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname, string fname )
				: this( a, gac, path, sname, fname, "", "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		/// <param name="fname">
		/// The full name of this <c>Assembly</c>.
		/// </param>
		/// <param name="ver">
		/// The version of this <c>Assembly</c>.
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname, string fname, string ver )
				: this( a, gac, path, sname, fname, ver, "", "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		/// <param name="fname">
		/// The full name of this <c>Assembly</c>.
		/// </param>
		/// <param name="ver">
		/// The version of this <c>Assembly</c>.
		/// </param>
		/// <param name="dev">
		/// The developer of this <c>Assembly</c>
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname, string fname, string ver, string dev )
				: this( null, gac, path, sname, fname, ver, dev, "", false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		/// <param name="fname">
		/// The full name of this <c>Assembly</c>.
		/// </param>
		/// <param name="ver">
		/// The version of this <c>Assembly</c>.
		/// </param>
		/// <param name="dev">
		/// The developer of this <c>Assembly</c>
		/// </param>
		/// <param name="desc">
		/// The description of the <c>Product</c>.
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname, string fname, string ver, string dev,
		                string desc )
				: this( null, gac, path, sname, fname, ver, dev, desc, false ) { }

		/// <summary>
		/// This initializes a <c>Product</c> with the passed in values.
		/// </summary>
		/// <param name="a">
		/// The <c>Assembly</c> of the <c>Product</c> object.
		/// </param>
		/// <param name="gac">
		/// True if the assembly is found in the GAC.  Otherwise false
		/// </param>
		/// <param name="path">
		/// The path to the location where the Assembly was found.
		/// </param>
		/// <param name="sname">
		/// The short name of this <c>Assembly</c>.
		/// </param>
		/// <param name="fname">
		/// The full name of this <c>Assembly</c>.
		/// </param>
		/// <param name="ver">
		/// The version of this <c>Assembly</c>.
		/// </param>
		/// <param name="dev">
		/// The developer of this <c>Assembly</c>
		/// </param>
		/// <param name="desc">
		/// The description of the <c>Product</c>.
		/// </param>
		/// <param name="isLicensed">
		/// True if this <c>Product</c> should be completely licensed.  Otherwise false.
		/// </param>
		public Product( Assembly a, bool gac, string path, string sname, string fname, string ver, string dev,
		                string desc, bool isLicensed )
		{
			this.IsInGAC		= gac;
			this.FilePath		= path;
			this.ShortName		= sname;
			this.FullName		= fname;
			this.Version		= ver;
			this.Developer		= dev;
			this.Description	= desc;
			this.isLicensed		= isLicensed;
			this.Assembly		= a;
			
			this.isDirty		= false;
		}

		/// <summary>
		/// This creates a <see cref="System.String">String</see> representing the
		/// XML form for this <c>Product</c>.
		/// </summary>
		/// <returns>
		/// The <see cref="System.String">String</see> representing this <c>Product</c> in it's XML form.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder	xmlString	= new StringBuilder( );
			XmlTextWriter	xmlWriter	= new XmlTextWriter( new StringWriter( xmlString ) );
			XmlDocument 	tempDoc = new XmlDocument( );

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';

			XmlNode productNode			= tempDoc.CreateElement( "", "Product", "" );
				XmlNode assemblyNode	= tempDoc.CreateElement( "", "Assembly", "" );
				XmlNode gacNode			= tempDoc.CreateElement( "", "IsInGac", "" );
				XmlNode pathNode		= tempDoc.CreateElement( "", "FilePath", "" );
				XmlNode snameNode		= tempDoc.CreateElement( "", "ShortName", "" );
				XmlNode fnameNode		= tempDoc.CreateElement( "", "FullName", "" );
				XmlNode versionNode		= tempDoc.CreateElement( "", "Version", "" );
				XmlNode developerNode	= tempDoc.CreateElement( "", "Developer", "" );
				XmlNode descNode		= tempDoc.CreateElement( "", "Description", "" );
				XmlNode	licensedNode	= tempDoc.CreateElement( "", "IsLicensed", "" );

			if( this.Assembly != null )
				assemblyNode.InnerText	= this.Assembly.ToString( );
			else
				assemblyNode.InnerText	= String.Empty;

			gacNode.InnerText		= this.IsInGAC.ToString( );
			pathNode.InnerText		= this.FilePath;
			snameNode.InnerText		= this.ShortName;
			fnameNode.InnerText		= this.FullName;
			versionNode.InnerText	= this.Version;
			developerNode.InnerText	= this.Developer;
			descNode.InnerText		= this.Description;
			licensedNode.InnerText	= this.IsLicensed.ToString( );

			productNode.AppendChild( assemblyNode );
			productNode.AppendChild( gacNode );
			productNode.AppendChild( pathNode );
			productNode.AppendChild( snameNode );
			productNode.AppendChild( fnameNode );
			productNode.AppendChild( versionNode );
			productNode.AppendChild( developerNode );
			productNode.AppendChild( descNode );
			productNode.AppendChild( licensedNode );

			tempDoc.AppendChild( productNode );

			tempDoc.WriteTo( xmlWriter );
			xmlWriter.Flush( );
			xmlWriter.Close( );

			return xmlString.ToString( );
		}

#region Properties
		/// <summary>
		/// Gets or Sets the Assembly for this <c>Product</c>.  When the Assembly
		/// is first assigned to a product the information of the Assembly is parsed
		/// to fill in other areas of the <c>Product</c> automatically like the full
		/// name, version, etc...  However this will only perform this action if
		/// the values have not yet been set (String.Empty).
		/// </summary>
		/// <param>
		///	Sets the Assembly for this <c>Product</c>.
		/// </param>
		/// <returns>
		/// Gets the Assembly for this <c>Product</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(null),
		Description( "Gets or Sets the Assembly for this Product." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public Assembly Assembly
		{
			get
			{
				return this.assembly;
			}
			set
			{
				if( this.assembly == value )
					return;

				this.assembly	= value;
				this.isDirty	= true;

				if( this.assembly == null )
					return;

				if( this.FilePath == String.Empty )
					this.FilePath = this.Assembly.CodeBase;

				if( this.FullName == String.Empty )
				{
					this.FullName = this.Assembly.FullName;

					if( this.ShortName == String.Empty )
					{
						int i = this.FullName.IndexOf( "," );
						if( i != -1 )
							this.ShortName = this.FullName.Substring( 0, i );
						else
							this.ShortName = this.FullName;
					}
				}

				if( this.Version == String.Empty )
				{
					this.Version = ((AssemblyName)this.assembly.GetName()).Version.ToString( );
				}

				this.IsInGAC = this.assembly.GlobalAssemblyCache;
				//TODO: Support setting the rest of the values...just need to find where it is...Use Debugger to get it...
			}
		}

		/// <summary>
		/// Gets or Sets if this Assembly can be found in the Global Assembly Cache (GAC).
		/// </summary>
		/// <param>
		/// Sets if this Assembly can be found in the Global Assembly Cache (GAC).
		/// </param>
		/// <returns>
		/// Gets if this Assembly can be found in the Global Assembly Cache (GAC).
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(false),
		Description( "Gets or Sets if this Assembly can be found in the Global Assembly Cache (GAC)." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool IsInGAC
		{
			get
			{
				return this.isInGac;
			}
			set
			{
				if( this.isInGac != value )
				{
					this.isInGac	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the file path for this Assembly.
		/// </summary>
		/// <param>
		/// Sets the file path for this Assembly.
		/// </param>
		/// <returns>
		/// Gets the file path for this Assembly.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the file path for this Assembly." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string FilePath
		{
			get
			{
				return this.filePath;
			}
			set
			{
				if( value != this.filePath )
				{
					this.filePath	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the short name of this Assembly.
		/// </summary>
		/// <param>
		/// Sets the short name of this Assembly.
		/// </param>
		/// <returns>
		/// Gets the short name of this Assembly.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the short name of this Assembly." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string ShortName
		{
			get
			{
				return this.shortName;
			}
			set
			{
				if( value != this.shortName )
				{
					this.shortName	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the full name of this Assembly.
		/// </summary>
		/// <param>
		/// Sets the full name of this Assembly.
		/// </param>
		/// <returns>
		/// Gets the full name of this Assembly.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the full name of this Assembly." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string FullName
		{
			get
			{
				return this.fullName;
			}
			set
			{
				if( value != this.fullName )
				{
					this.fullName = value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the version of this Assembly.
		/// </summary>
		/// <param>
		/// Sets the version of this Assembly.
		/// </param>
		/// <returns>
		/// Gets the version of this Assembly.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the version of this Assembly." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				if( value != this.version )
				{
					this.version	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the developer of this Assembly.
		/// </summary>
		/// <param>
		/// Sets the developer of this Assembly.
		/// </param>
		/// <returns>
		/// Gets the developer of this Assembly.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the developer of this Assembly." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Developer
		{
			get
			{
				return this.developer;
			}
			set
			{
				if( value != this.developer )
				{
					this.developer	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets a description for this <c>Product</c>.
		/// </summary>
		/// <param>
		/// Sets a description for this <c>Product</c>.
		/// </param>
		/// <returns>
		/// Gets a description for this <c>Product</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets a description for this Product." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if( value != this.description )
				{
					this.description	= value;
					this.isDirty		= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets if this <c>Product</c> has been fully licensed with no restrictions.
		/// </summary>
		/// <param>
		/// Sets if this <c>Product</c> has been fully licensed with no restrictions.
		/// </param>
		/// <returns>
		/// Gets if this <c>Product</c> has been fully licensed with no restrictions.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(false),
		Description( "Gets or Sets if this <c>Product</c> has been fully licensed with no restrictions." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool IsLicensed
		{
			get
			{
				return this.isLicensed;
			}
			set
			{
				if( value != this.isLicensed )
				{
					this.isLicensed	= value;
					this.isDirty	= true;
				}
			}
		}
#endregion
	}
}
