using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Hemrika.SharePresence.SPLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='UsageConstraint'/> constrains the user
	/// to a usage limit.  It supports a Maximum Usage, Hit and Days count.  Once
	/// the maximum number is reached the license expires.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</seealso>
	public class UsageConstraint : AbstractConstraint
	{
		private	int			maxUsageCount		= -1;
		private int			currentUsageCount	= 0;

		private	int			maxHitCount			= -1;
		private int			currentHitCount		= 0;

		private	int			maxDaysCount		= -1;
		private int			currentDaysCount	= 1;

		private DateTime	dateLastAccessed	= DateTime.Now;

		/// <summary>
		/// This is the constructor for the <c>UsageConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public UsageConstraint( ) : this( null ){ }

		/// <summary>
		/// This is the constructor for the <c>UsageConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref=" Hemrika.SharePresence.SPLicenseFile"> Hemrika.SharePresence.SPLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public UsageConstraint(SPLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Usage Constraint";
			base.Description	=  "This UsageConstraint constrains the user to a usage ";
			base.Description	+= "limit. It supports a Maximum Usage, Hit and Days ";
			base.Description	+= "count. Once the maximum number is reached the license ";
			base.Description	+= "expires.";
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the license has not been used more times then the usage constraint
		/// allows. The usage constraints available are:
		/// <list>
		/// <item>Hit Count - The maximum number of hits for a web services.</item>
		/// <item>Usage Count - The maximum number of uses for a application.</item>
		/// <item>Duration - The maximum number of days this may be used.</item>
		/// </list>
		/// If any of these values are exceeded then the license validation will return false
		/// and the failure reason will be set.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to one of the following
		/// depending upon which one failed:
		/// <list type="definition">
		///  <listheader>
		///   <term>Validation Type</term>
		///   <description>Failure String Result</description>
		///  </listheader>
		///  <item>
		///   <term>Hits</term>
		///   <description>The maximum number of hits has been reached.</description>
		///  </item>
		///  <item>
		///   <term>Usage</term>
		///   <description>The maximum number of uses has been reached.</description>
		///  </item>
		///  <item>
		///   <term>Duration</term>
		///   <description>The maximum number of days has been reached.</description>
		///  </item>
		/// </list>
		/// </remarks>
		public override bool Validate( )
		{
			this.IncrementDays( );
			this.IncrementHits( );
			this.IncrementUsage( );

			if( this.MaxUsageCount > 0 && this.CurrentUsageCount > this.MaxUsageCount )
			{
				if( base.License != null )
					base.License.FailureReason = "The maximum number of uses has been reached.";

				return false;
			}
			else if( this.MaxHitCount > 0 && this.CurrentHitCount > this.MaxHitCount )
			{
				if( base.License != null )
					base.License.FailureReason = "The maximum number of hits has been reached.";

				return false;
			}
			else if( this.MaxDays > 0 && this.CurrentDays > this.MaxDays )
			{
				if( base.License != null )
					base.License.FailureReason = "The maximum number of days has been reached.";

				return false;
			}

			if( base.License != null )
				base.License.FailureReason = String.Empty;

			return true;
		}

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>UsageConstriant</c>.
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
		/// This creates a <c>UsageConstriant</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>UsageConstriant</c>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode mUsageTextNode			= itemsNode.SelectSingleNode( "MaxUsageCount/text()" );
			XmlNode cUsageTextNode			= itemsNode.SelectSingleNode( "CurrentUsageCount/text()" );
			XmlNode mHitTextNode			= itemsNode.SelectSingleNode( "MaxHitCount/text()" );
			XmlNode cHitTextNode			= itemsNode.SelectSingleNode( "CurrentHitCount/text()" );
			XmlNode mDaysTextNode			= itemsNode.SelectSingleNode( "MaxDaysCount/text()" );
			XmlNode cDaysTextNode			= itemsNode.SelectSingleNode( "CurrentDaysCount/text()" );
			XmlNode dateAccessedTextNode	= itemsNode.SelectSingleNode( "DateLastAccessed/text()" );

			if( mUsageTextNode != null )
				this.maxUsageCount		= Convert.ToInt32( mUsageTextNode.Value );

			if( cUsageTextNode != null )
				this.currentUsageCount	= Convert.ToInt32( cUsageTextNode.Value );

			if( mHitTextNode != null )
				this.maxHitCount		= Convert.ToInt32( mHitTextNode.Value );

			if( cHitTextNode != null )
				this.currentHitCount	= Convert.ToInt32( cHitTextNode.Value );

			if( mDaysTextNode != null )
				this.maxDaysCount		= Convert.ToInt32( mDaysTextNode.Value );

			if( cDaysTextNode != null )
				this.currentDaysCount	= Convert.ToInt32( cDaysTextNode.Value );

			if( dateAccessedTextNode != null )
			{
				IFormatProvider	culture	= new CultureInfo( "en-US", true );
				this.dateLastAccessed	= DateTime.Parse( dateAccessedTextNode.Value, culture, 
															DateTimeStyles.NoCurrentDateDefault );
				//this.dateLastAccessed	= Convert.ToDateTime( dateAccessedTextNode.Value );
			}
		}

		/// <summary>
		/// Converts this <c>UsageConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the UsageConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';

			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type", 				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );
				xmlWriter.WriteElementString( "MaxUsageCount",		Convert.ToString( MaxUsageCount ) );
				xmlWriter.WriteElementString( "CurrentUsageCount",	Convert.ToString( CurrentUsageCount ) );
				xmlWriter.WriteElementString( "MaxHitCount",		Convert.ToString( MaxHitCount ) );
				xmlWriter.WriteElementString( "CurrentHitCount",	Convert.ToString( CurrentHitCount ) );
				xmlWriter.WriteElementString( "MaxDaysCount",		Convert.ToString( MaxDays ) );
				xmlWriter.WriteElementString( "CurrentDaysCount",	Convert.ToString( CurrentDays ) );
				xmlWriter.WriteElementString( "DateLastAccessed",	this.dateLastAccessed.ToString( ) );
			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

#region Properties
		/// <summary>
		/// Gets or Sets the maximum allowed uses for this <c>UsageConstraint</c>.
		/// </summary>
		/// <param>
		///	Sets the maximum allowed uses for this <c>UsageConstraint</c>.
		/// </param>
		/// <returns>
		///	Gets the maximum allowed uses for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(-1),
		Description( "Gets or Sets the maximum allowed uses for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int MaxUsageCount
		{
			get
			{
				return this.maxUsageCount;
			}
			set
			{
				if( !this.MaxUsageCount.Equals( value ) )
				{
					this.maxUsageCount	= value;
					this.isDirty		= true;
				}
			}
		}

		/// <summary>
		/// Gets the current usage count for this <c>UsageConstraint</c>.
		/// </summary>
		/// <returns>
		///	Gets the current usage count for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the current usage count for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int CurrentUsageCount
		{
			get
			{
				return this.currentUsageCount;
			}
		}

		/// <summary>
		/// Increments the current usage counter by one.
		/// </summary>
		private void IncrementUsage( )
		{
			this.currentUsageCount++;
		}

		/// <summary>
		/// Gets or Sets the maximum allowed hits for this <c>UsageConstraint</c>.
		/// </summary>
		/// <param>
		///	Sets the maximum allowed hits for this <c>UsageConstraint</c>.
		/// </param>
		/// <returns>
		///	Gets the maximum allowed hits for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(-1),
		Description( "Gets or Sets the maximum allowed hits for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int MaxHitCount
		{
			get
			{
				return this.maxHitCount;
			}
			set
			{
				if( !this.MaxHitCount.Equals( value ) )
				{
					this.maxHitCount	= value;
					this.isDirty		= true;
				}
			}
		}

		/// <summary>
		/// Gets the current hit count for this <c>UsageConstraint</c>.
		/// </summary>
		/// <returns>
		///	Gets the current hit count for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the current hit count for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int CurrentHitCount
		{
			get
			{
				return this.currentHitCount;
			}
		}


		/// <summary>
		/// Increments the current hit counter by one.
		/// </summary>
		private void IncrementHits( )
		{
			this.currentHitCount++;
		}

		/// <summary>
		/// Gets or Sets the maximum number of days allowed for this <c>UsageConstraint</c>.
		/// </summary>
		/// <param>
		///	Sets the maximum number of days allowed for this <c>UsageConstraint</c>.
		/// </param>
		/// <returns>
		///	Gets the maximum number of days allowed for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(-1),
		Description( "Gets or Sets the maximum number of days allowed for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int MaxDays
		{
			get
			{
				return this.maxDaysCount;
			}
			set
			{
				if( !this.MaxDays.Equals( value ) )
				{
					this.maxDaysCount	= value;
					this.isDirty		= true;
				}
			}
		}

		/// <summary>
		/// Gets the current days count for this <c>UsageConstraint</c>.
		/// </summary>
		/// <returns>
		///	Gets the current days count for this <c>UsageConstraint</c>.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the current days count for this UsageConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int CurrentDays
		{
			get
			{
				return this.currentDaysCount;
			}
		}

		/// <summary>
		/// Increments the days counter by one when a new day has occurred.
		/// </summary>
		private void IncrementDays( )
		{
			if( this.dateLastAccessed.Month	!= DateTime.Now.Month	||
			    this.dateLastAccessed.Day	!= DateTime.Now.Day		||
			    this.dateLastAccessed.Year	!= DateTime.Now.Year )
			{
				this.dateLastAccessed = DateTime.Now;
				this.currentDaysCount++;
				this.isDirty	= true;
			}
		}
#endregion
	}
} /* namespace Hemrika.SharePresence.SPLicense */
