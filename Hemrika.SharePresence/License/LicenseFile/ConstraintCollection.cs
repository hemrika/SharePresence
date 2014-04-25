//using System;
//using System.Collections;
//
//namespace Hemrika.SharePresence.SPLicense.LicenseFile
//{
//	/// <summary>
//	///   A collection that stores <see cref='IConstraint'/> objects.
//	/// </summary>
//	[Serializable()]
//	public class ConstraintCollection : CollectionBase {
//		
//		/// <summary>
//		///   Initializes a new instance of <see cref='ConstraintCollection'/>.
//		/// </summary>
//		public ConstraintCollection()
//		{
//		}
//		
//		/// <summary>
//		///   Initializes a new instance of <see cref='ConstraintCollection'/> based on another <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <param name='val'>
//		///   A <see cref='ConstraintCollection'/> from which the contents are copied
//		/// </param>
//		public ConstraintCollection(ConstraintCollection val)
//		{
//			this.AddRange(val);
//		}
//		
//		/// <summary>
//		///   Initializes a new instance of <see cref='ConstraintCollection'/> containing any array of <see cref='IConstraint'/> objects.
//		/// </summary>
//		/// <param name='val'>
//		///       A array of <see cref='IConstraint'/> objects with which to intialize the collection
//		/// </param>
//		public ConstraintCollection(IConstraint[] val)
//		{
//			this.AddRange(val);
//		}
//		
//		/// <summary>
//		///   Represents the entry at the specified index of the <see cref='IConstraint'/>.
//		/// </summary>
//		/// <param name='index'>The zero-based index of the entry to locate in the collection.</param>
//		/// <value>The entry at the specified index of the collection.</value>
//		/// <exception cref='ArgumentOutOfRangeException'><paramref name='index'/> is outside the valid range of indexes for the collection.</exception>
//		public IConstraint this[int index] {
//			get {
//				return ((IConstraint)(List[index]));
//			}
//			set {
//				List[index] = value;
//			}
//		}
//		
//		/// <summary>
//		///   Adds a <see cref='IConstraint'/> with the specified value to the 
//		///   <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <param name='val'>The <see cref='IConstraint'/> to add.</param>
//		/// <returns>The index at which the new element was inserted.</returns>
//		public int Add(IConstraint val)
//		{
//			return List.Add(val);
//		}
//		
//		/// <summary>
//		///   Copies the elements of an array to the end of the <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <param name='val'>
//		///    An array of type <see cref='IConstraint'/> containing the objects to add to the collection.
//		/// </param>
//		/// <seealso cref='ConstraintCollection.Add'/>
//		public void AddRange(IConstraint[] val)
//		{
//			for (int i = 0; i < val.Length; i++) {
//				this.Add(val[i]);
//			}
//		}
//		
//		/// <summary>
//		///   Adds the contents of another <see cref='ConstraintCollection'/> to the end of the collection.
//		/// </summary>
//		/// <param name='val'>
//		///    A <see cref='ConstraintCollection'/> containing the objects to add to the collection.
//		/// </param>
//		/// <seealso cref='ConstraintCollection.Add'/>
//		public void AddRange(ConstraintCollection val)
//		{
//			for (int i = 0; i < val.Count; i++)
//			{
//				this.Add(val[i]);
//			}
//		}
//		
//		/// <summary>
//		///   Gets a value indicating whether the 
//		///    <see cref='ConstraintCollection'/> contains the specified <see cref='IConstraint'/>.
//		/// </summary>
//		/// <param name='val'>The <see cref='IConstraint'/> to locate.</param>
//		/// <returns>
//		/// <see langword='true'/> if the <see cref='IConstraint'/> is contained in the collection; 
//		///   otherwise, <see langword='false'/>.
//		/// </returns>
//		/// <seealso cref='ConstraintCollection.IndexOf'/>
//		public bool Contains(IConstraint val)
//		{
//			return List.Contains(val);
//		}
//		
//		/// <summary>
//		///   Copies the <see cref='ConstraintCollection'/> values to a one-dimensional <see cref='Array'/> instance at the 
//		///    specified index.
//		/// </summary>
//		/// <param name='array'>The one-dimensional <see cref='Array'/> that is the destination of the values copied from <see cref='ConstraintCollection'/>.</param>
//		/// <param name='index'>The index in <paramref name='array'/> where copying begins.</param>
//		/// <exception cref='ArgumentException'>
//		///   <para><paramref name='array'/> is multidimensional.</para>
//		///   <para>-or-</para>
//		///   <para>The number of elements in the <see cref='ConstraintCollection'/> is greater than
//		///         the available space between <paramref name='arrayIndex'/> and the end of
//		///         <paramref name='array'/>.</para>
//		/// </exception>
//		/// <exception cref='ArgumentNullException'><paramref name='array'/> is <see langword='null'/>. </exception>
//		/// <exception cref='ArgumentOutOfRangeException'><paramref name='arrayIndex'/> is less than <paramref name='array'/>'s lowbound. </exception>
//		/// <seealso cref='Array'/>
//		public void CopyTo(IConstraint[] array, int index)
//		{
//			List.CopyTo(array, index);
//		}
//		
//		/// <summary>
//		///    Returns the index of a <see cref='IConstraint'/> in 
//		///       the <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <param name='val'>The <see cref='IConstraint'/> to locate.</param>
//		/// <returns>
//		///   The index of the <see cref='IConstraint'/> of <paramref name='val'/> in the 
//		///   <see cref='ConstraintCollection'/>, if found; otherwise, -1.
//		/// </returns>
//		/// <seealso cref='ConstraintCollection.Contains'/>
//		public int IndexOf(IConstraint val)
//		{
//			return List.IndexOf(val);
//		}
//		
//		/// <summary>
//		///   Inserts a <see cref='IConstraint'/> into the <see cref='ConstraintCollection'/> at the specified index.
//		/// </summary>
//		/// <param name='index'>The zero-based index where <paramref name='val'/> should be inserted.</param>
//		/// <param name='val'>The <see cref='IConstraint'/> to insert.</param>
//		/// <seealso cref='ConstraintCollection.Add'/>
//		public void Insert(int index, IConstraint val)
//		{
//			List.Insert(index, val);
//		}
//		
//		/// <summary>
//		///  Returns an enumerator that can iterate through the <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <seealso cref='IEnumerator'/>
//		public new IConstraintEnumerator GetEnumerator()
//		{
//			return new IConstraintEnumerator(this);
//		}
//		
//		/// <summary>
//		///   Removes a specific <see cref='IConstraint'/> from the <see cref='ConstraintCollection'/>.
//		/// </summary>
//		/// <param name='val'>The <see cref='IConstraint'/> to remove from the <see cref='ConstraintCollection'/>.</param>
//		/// <exception cref='ArgumentException'><paramref name='val'/> is not found in the Collection.</exception>
//		public void Remove(IConstraint val)
//		{
//			List.Remove(val);
//		}
//		
//		/// <summary>
//		///   Enumerator that can iterate through a ConstraintCollection.
//		/// </summary>
//		/// <seealso cref='IEnumerator'/>
//		/// <seealso cref='ConstraintCollection'/>
//		/// <seealso cref='IConstraint'/>
//		public class IConstraintEnumerator : IEnumerator
//		{
//			IEnumerator baseEnumerator;
//			IEnumerable temp;
//			
//			/// <summary>
//			///   Initializes a new instance of <see cref='IConstraintEnumerator'/>.
//			/// </summary>
//			public IConstraintEnumerator(ConstraintCollection mappings)
//			{
//				this.temp = ((IEnumerable)(mappings));
//				this.baseEnumerator = temp.GetEnumerator();
//			}
//			
//			/// <summary>
//			///   Gets the current <see cref='IConstraint'/> in the <seealso cref='ConstraintCollection'/>.
//			/// </summary>
//			public IConstraint Current {
//				get {
//					return ((IConstraint)(baseEnumerator.Current));
//				}
//			}
//			
//			object IEnumerator.Current {
//				get {
//					return baseEnumerator.Current;
//				}
//			}
//			
//			/// <summary>
//			///   Advances the enumerator to the next <see cref='IConstraint'/> of the <see cref='ConstraintCollection'/>.
//			/// </summary>
//			public bool MoveNext()
//			{
//				return baseEnumerator.MoveNext();
//			}
//			
//			/// <summary>
//			///   Sets the enumerator to its initial position, which is before the first element in the <see cref='ConstraintCollection'/>.
//			/// </summary>
//			public void Reset()
//			{
//				baseEnumerator.Reset();
//			}
//		}
//	}
//}
