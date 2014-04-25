using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;

namespace Hemrika.SharePresence.SPLicense.LicenseFile
{
	/// <summary>
	/// <p>This <see cref="AbstractContainerConstraint">AbstractContainerConstraint</see> 
	/// is used to define a container for other constraints.  This is used to provide
	/// a method to create grouping of constraints to provide bitwise operations.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	public abstract class AbstractContainerConstraint : AbstractConstraint
	{
		private	List<IConstraint> constraints	= new List<IConstraint>( );
		
#region Properties		
		/// <summary>
		/// Gets or Sets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </summary>
		/// <param>
		/// Sets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </param>
		/// <returns>
		///	Gets the <c>ConstraintCollection</c> for this ContainerConstraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the ConstraintCollection for this ContainerConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		ReadOnly(true)
		]
		public List<IConstraint> Items
		{
			get 
			{
				return this.constraints;
			}
			set
			{
				this.constraints = value;
			}
		}
#endregion
	}
} /* namespace Hemrika.SharePresence.SPLicense */
