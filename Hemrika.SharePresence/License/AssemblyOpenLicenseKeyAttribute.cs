using System;
using System.ComponentModel;
using System.Reflection;

namespace Hemrika.SharePresence.SPLicense
{
	/// <summary>
	/// This is an assembly attribute to be used to defined the encryption key that 
	/// should be used to decrypt the license file when it is encrypted.  If the value 
	/// is defined then the license file will be decrypted using the defined key.  If 
	/// the decrypt fails then an exception will be thrown. 
	/// </summary>
	/// <example>
	/// c#
	/// <code>
	/// &#91;assembly:  Hemrika.SharePresence.SPLicense.Assembly Hemrika.SharePresence.SPLicenseKey("test")&#93;
	/// </code>
	/// vb#
	/// <code>
	/// &lt;assembly:  Hemrika.SharePresence.SPLicense.Assembly Hemrika.SharePresence.SPLicenseKey("test")&gt;
	/// </code>
	/// </example>
	[
	AttributeUsage( AttributeTargets.Assembly )
	]
	public class AssemblyLicenseKeyAttribute : System.Attribute
	{
		private string	encryptionKey		= String.Empty;
		
		/// <summary>
		/// This is responsible for returning the encryption key defined in 
		/// the Assembly.  If not defined then an Empty string is returned.
		/// </summary>
		/// <param name="type">
		/// The object type being licensed.
		/// </param>
		public static string GetEncryptionKeyAttribute( Type type )
		{
			string key	= String.Empty;
			
			// Look for key defined in Assembly.
			AssemblyLicenseKeyAttribute attr =
				(AssemblyLicenseKeyAttribute)Attribute.GetCustomAttribute(
					Assembly.GetAssembly( type ),
					Type.GetType( " Hemrika.SharePresence.SPLicense.AssemblyLicenseKeyAttribute" ) );
			
			if( attr != null )
				return attr.EncryptionKey;	
			else
				return String.Empty;
		}
		
		/// <summary>
		/// The constructor for an empty <c>Assembly Hemrika.SharePresence.SPLicenseKeyAttribute</c>.
		/// </summary>
		public AssemblyLicenseKeyAttribute( ) : this( "" ) {}
		
		/// <summary>
		/// The constructor for an <c>Assembly Hemrika.SharePresence.SPLicenseKeyAttribute</c> with a given encryption key.
		/// </summary>
		public AssemblyLicenseKeyAttribute( string key )
		{
			this.encryptionKey = key;
		}
		
		/// <summary>
		/// Gets the Encryption Key to be used for the license.
		/// </summary>
		/// <returns>
		/// Gets the Encryption Key to be used for the license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets the Encryption Key to be used for the license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string EncryptionKey
		{
			get
			{
				return this.encryptionKey;	
			}
		}
	}
} /* namespace Hemrika.SharePresence.SPLicense */

