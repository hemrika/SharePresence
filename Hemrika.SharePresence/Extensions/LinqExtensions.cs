﻿// -----------------------------------------------------------------------
// <copyright file="LinqExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#if LINQ
	public static partial class Extensions 
	{
		public static IEnumerable<Tuple<T,T>> RandomlyPair<T>(this IEnumerable<T> a, IEnumerable<T> b, int desiredSize)
		{
			return a.RandomlyOrder(desiredSize).Combine(b.RandomlyOrder(desiredSize));
		}
		public static IEnumerable<Tuple<int,T>> RandomlyOrder<T>(this IEnumerable<T> a, int desiredSize)
		{
			return (from x in a.IndexEach()
					join y in RandomOrder(desiredSize) on x.Item1 equals y
					select new Tuple<int,T>(y, x.Item2));	
		}
		public static IEnumerable<T> RandomReorder<T>(this IEnumerable<T> a)
		{
			Random r = new Random(Guid.NewGuid().GetHashCode());
			Queue<T> no = new Queue<T>();
			foreach(var v in a)
			{
				if(r.Next(2) == 1)
					yield return v;
				else
					no.Enqueue(v);
			}
			//this is pretty interesting stuff
			T value = default(T);
			for(value = no.Dequeue(); no.Count > 0; value = no.Dequeue())
			{
				if(r.Next(2) == 1)
					yield return value;
				else
					no.Enqueue(value);
			}
			if(!value.Equals(default(T))) //this will prevent a deadlock and also make sure that all elements are returned
				yield return value;
		}
		public static IEnumerable<T> AlternateWith<T>(this IEnumerable<T> a, IEnumerable<T> b)
		{
			//which is longer?
			int aCount = a.Count();
			int bCount = b.Count();
			int difference = Math.Abs(aCount - bCount);
			var mapping = a.Combine(b);
			foreach(var m in mapping)
			{
				yield return m.Item1;
				yield return m.Item2;
			}
			if(difference > 0)
			{
				var rest = aCount > bCount ? a.Skip(bCount) : b.Skip(aCount);
				foreach(var v in rest)
					yield return v;
			}
		}
		public static IEnumerable<Tuple<T0,T1>> Combine<T0,T1>(this IEnumerable<T0> a, IEnumerable<T1> b)
		{
			return a.Zip(b, (x,y) => new Tuple<T0,T1>(x,y));
			//return Combine(a.IndexEach(), b.IndexEach());
		}
		public static IEnumerable<Tuple<T0,T1>> Combine<T0,T1>(this IEnumerable<Tuple<int,T0>> a, IEnumerable<Tuple<int,T1>> b)
		{
			return (from i in a
					join j in b on i.Item1 equals j.Item1
					select new Tuple<T0,T1>(i.Item2, j.Item2));
		}
		public static IEnumerable<Tuple<T0,T1,T2>> Combine<T0,T1,T2>(this IEnumerable<Tuple<int,T0>> a, IEnumerable<Tuple<int,T1>> b, IEnumerable<Tuple<int,T2>> c)
		{
			return (from i in a
					join j in b on i.Item1 equals j.Item1
					join k in c on j.Item1 equals k.Item1
					select new Tuple<T0,T1,T2>(i.Item2,j.Item2,k.Item2));
		}
		public static IEnumerable<Tuple<T0,T1,T2>> Combine<T0,T1,T2>(this IEnumerable<T0> a, IEnumerable<T1> b, IEnumerable<T2> c)
		{
			return a.Combine(b).Zip(c, (x,y) => new Tuple<T0,T1,T2>(x.Item1, x.Item2, y));
		}
		public static IEnumerable<Tuple<T0,T1,T2,T3>> Combine<T0,T1,T2,T3>(this IEnumerable<T0> a, IEnumerable<T1> b, IEnumerable<T2> c,
				IEnumerable<T3> d)
		{
			return a.Combine(b).Zip(c.Combine(d), (x,y) => new Tuple<T0,T1,T2,T3>(x.Item1, x.Item2, y.Item1, y.Item2));
		}
		public static IEnumerable<Tuple<T0,T1,T2,T3>> Combine<T0,T1,T2,T3>(this IEnumerable<Tuple<int,T0>> a, IEnumerable<Tuple<int,T1>> b, IEnumerable<Tuple<int,T2>> c,
				IEnumerable<Tuple<int,T3>> d)
		{
			return (from i in a
					join j in b on i.Item1 equals j.Item1
					join k in c on j.Item1 equals k.Item1 //transitivity
					join h in d on k.Item1 equals h.Item1 //once again transitivity
					select new Tuple<T0,T1,T2,T3>(i.Item2, j.Item2, k.Item2, h.Item2));
		}
		public static IEnumerable<R> Map<T0,T1,R>(this IEnumerable<T0> a, IEnumerable<T1> b, Func<T0,T1,R> fn)
		{
			return a.Combine(b).Select((x) => fn(x.Item1, x.Item2));
		}
		public static IEnumerable<R> Map<T0,T1,T2,R>(this IEnumerable<T0> a, IEnumerable<T1> b, IEnumerable<T2> c,
				Func<T0,T1,T2,R> fn)
		{
			return a.Combine(b,c).Select((x) => fn(x.Item1, x.Item2, x.Item3));
		}
		public static IEnumerable<R> Map<T0,T1,T2,T3,R>(this IEnumerable<T0> a, IEnumerable<T1> b, IEnumerable<T2> c,
				IEnumerable<T3> d, Func<T0,T1,T2,T3,R> fn)
		{
			return a.Combine(b,c,d).Select((x) => fn(x.Item1, x.Item2, x.Item3, x.Item4));
		}
		public static IEnumerable<Tuple<int,T>> IndexEach<T>(this IEnumerable<T> elements)
		{
			int counter = 0; 
			foreach(var v in elements)
			{
				yield return new Tuple<int,T>(counter,v);
				counter++;
			}
		}

		public static IEnumerable<int> RandomOrder(this int desiredNumber)
		{
			bool[] found = new bool[desiredNumber];
			Random r = new Random(Guid.NewGuid().GetHashCode());
			for(int i = 0; i < desiredNumber; )
			{
				int value = r.Next(desiredNumber);
				if(!found[value])
				{
					i++;
					found[value] = true;
					yield return value;
				}
			}		
		}
		public static IEnumerable<Tuple<T0,T1,T2,T3>> Invert<T0,T1,T2,T3>(this Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> tuple)
		{
			var a = tuple.Item1;
			var b = tuple.Item2;
			var c = tuple.Item3;
			var d = tuple.Item4;
			return a.Combine(b,c,d);
		}
		public static Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> Invert<T0,T1,T2,T3>(
				this IEnumerable<Tuple<T0,T1,T2,T3>> values)
		{
			return new Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(
					(from v in values select v.Item1),
					(from v in values select v.Item2),
					(from v in values select v.Item3),
					(from v in values select v.Item4));
		}
		public static IEnumerable<Tuple<T0,T1,T2>> Invert<T0,T1,T2>(this Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>> tu)
		{
			var a = tu.Item1;
			var b = tu.Item2;
			var c = tu.Item3;
			return a.Combine(b,c);
		}	
		public static Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>> Invert<T0,T1,T2>(this IEnumerable<Tuple<T0,T1,T2>> values)
		{
			return new Tuple<IEnumerable<T0>, IEnumerable<T1>, IEnumerable<T2>>(
					(from v in values select v.Item1),
					(from v in values select v.Item2),
					(from v in values select v.Item3));
		}
		public static Tuple<IEnumerable<T0>, IEnumerable<T1>> Invert<T0,T1>(this IEnumerable<Tuple<T0,T1>> values)
		{
			return new Tuple<IEnumerable<T0>, IEnumerable<T1>>(
					(from v in values select v.Item1), 
					(from v in values select v.Item2));
		}
		public static IEnumerable<Tuple<T0,T1>> Invert<T0,T1>(this Tuple<IEnumerable<T0>, IEnumerable<T1>> tu)
		{
			var a = tu.Item1;
			var b = tu.Item2;
			return a.Combine(b);
		}

	}
#endif
}
