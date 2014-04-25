namespace Hemrika.SharePresence.SPLicense.LicenseFile
{
	/// <summary>
	/// <p><c>ILicenseData</c> is an interface class which all licensing
	/// data, other then Constraints) must inherit.  The <c>ILicenseData</c>
	/// defines the necessary items for any license data to be used by
	/// the <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see>.</p>
	/// </summary>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public interface ILicenseData
	{
		/// <summary>
		/// Converts this instance of License Data to a <see cref="System.String">String</see>
		/// representing the Xml of the specific License Data object.
		/// </summary>
		/// <return>
		/// The <c>String</c> representing this License Data.
		/// </return>
		string ToXmlString( );

#region Properties
		/// <summary>
		/// Gets if the license data has changed since the last save.
		/// </summary>
		/// <returns>
		/// Gets if the license data has changed since the last save.
		/// </returns>
		bool IsDirty
		{
			get;
		}

		///	<summary>
		/// Resets the IsDirty flag to know this item has been saved.
		/// </summary>
		void Saved( );
#endregion
	}
} /* namespace Hemrika.SharePresence.SPLicense */

