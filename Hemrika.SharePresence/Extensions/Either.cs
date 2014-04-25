﻿// -----------------------------------------------------------------------
// <copyright file="Either.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public static class Either
    {
        /// <summary>
        /// Create an <c>Left</c> based on the given <paramref name="leftValue"/>.
        /// </summary>
        public static Either<TL, TR> Left<TL, TR>(this TL leftValue)
        {
            return new Either<TL, TR>(leftValue);
        }

        /// <summary>
        /// Create an <c>Right</c> based on the given <paramref name="rightValue"/>.
        /// </summary>
        public static Either<TL, TR> Right<TL, TR>(this TR rightValue)
        {
            return new Either<TL, TR>(rightValue);
        }

        /// <summary>
        /// Return the value of a <c>Left</c> or throw an exception returned from the function <paramref name="efunc"/>.
        /// </summary>
        public static TL Left<TL, TR>(this Either<TL, TR> either, Func<Exception> efunc)
        {
            if (either.IsRight)
            {
                throw efunc();
            }
            return either.Left;
        }

        /// <summary>
        /// Return the value of a <c>Right</c> or throw an exception returned from the function <paramref name="efunc"/>.
        /// </summary>
        public static TR Right<TL, TR>(this Either<TL, TR> either, Func<Exception> efunc)
        {
            if (either.IsLeft)
            {
                throw efunc();
            }
            return either.Right;
        }

        /// <summary>
        /// Return the value of a <c>Left</c> or in the case of a <c>Right</c> a default value generated by the function <paramref name="default"/>.
        /// </summary>
        public static TL Left<TL, TR>(this Either<TL, TR> either, Func<TL> @default)
        {
            return either.IsLeft ? either.Left : @default();
        }

        /// <summary>
        /// Return the value of a <c>Right</c> or in the case of a <c>Left</c> a default value generated by the function <paramref name="default"/>.
        /// </summary>
        public static TR Right<TL, TR>(this Either<TL, TR> either, Func<TR> @default)
        {
            return either.IsRight ? either.Right : @default();
        }

        /// <summary>
        /// Return the value of a <c>Left</c> or in the case of a <c>Right</c> the default value <paramref name="default"/>.
        /// </summary>
        public static TL Left<TL, TR>(this Either<TL, TR> either, TL @default)
        {
            return either.IsLeft ? either.Left : @default;
        }

        /// <summary>
        /// Return the value of a <c>Right</c> or in the case of a <c>Left</c> the default value <paramref name="default"/>.
        /// </summary>
        public static TR Right<TL, TR>(this Either<TL, TR> either, TR @default)
        {
            return either.IsRight ? either.Right : @default;
        }

        /// <summary>
        /// Return the value of a <c>Left</c> or in the case of a <c>Right</c> the default value of the type <typeparamref name="TL"/>.
        /// </summary>
        public static TL LeftOrDefault<TL, TR>(this Either<TL, TR> either)
        {
            return either.Left(default(TL));
        }

        /// <summary>
        /// Return the value of a <c>Right</c> or in the case of a <c>Left</c> the default value of the type <typeparamref name="TR"/>.
        /// </summary>
        public static TR RightOrDefault<TL, TR>(this Either<TL, TR> either)
        {
            return either.Right(default(TR));
        }

        /// <summary>
        /// Create an <see cref="IEnumerable{TL}"/> from a <c>Either Left</c>. A <c>Right</c> yields an empty enumerable,
        /// a <c>Left</c> yields a enumeration  with the single value.
        /// </summary>
        public static IEnumerable<TL> LeftToEnumerable<TL, TR>(this Either<TL, TR> either)
        {
            if (either.IsLeft) { yield return either.Left; }
        }

        /// <summary>
        /// Create an <see cref="IEnumerable{TR}"/> from a <c>Either Right</c>. A <c>Left</c> yields an empty enumerable,
        /// a <c>Right</c> yields a enumeration  with the single value.
        /// </summary>
        public static IEnumerable<TR> RightToEnumerable<TL, TR>(this Either<TL, TR> either)
        {
            if (either.IsRight) { yield return either.Right; }
        }

        /// <summary>
        /// Return the value of a <c>Left</c> or in the case of <c>Right</c> <c>null</c>.
        /// </summary>
        public static TL NullableLeft<TL, TR>(this Either<TL, TR> either) where TL : class
        {
            return either.IsLeft ? either.Left : null;
        }

        /// <summary>
        /// Return the value of a <c>Right</c> or in the case of <c>Left</c> <c>null</c>.
        /// </summary>
        public static TR NullableRight<TL, TR>(this Either<TL, TR> either) where TR : class
        {
            return either.IsRight ? either.Right : null;
        }

        /// <summary>
        /// Convert <see cref="Either{TL,TR}"/> <c>Left</c> to an <see cref="Nullable{TL}"/>.
        /// </summary>
        public static TL? LeftToNullable<TL, TR>(this Either<TL, TR> either) where TL : struct
        {
            return either.IsLeft ? new Nullable<TL>(either.Left) : new Nullable<TL>();
        }

        /// <summary>
        /// Convert <see cref="Either{TL,TR}"/> <c>Right</c> to an <see cref="Nullable{TR}"/>.
        /// </summary>
        public static TR? RightToNullable<TL, TR>(this Either<TL, TR> either) where TR : struct
        {
            return either.IsRight ? new Nullable<TR>(either.Right) : new Nullable<TR>();
        }

        /// <summary>
        /// Return the result of applying the value of a <c>Left</c> to the function <paramref name="fn"/> or in the case of
        /// a <c>Right</c> the default value generated from the function <paramref name="default"/>.
        /// </summary>
        public static TResult SelectLeft<TL, TR, TResult>(this Either<TL, TR> either, Func<TL, TResult> fn, Func<TResult> @default)
        {
            return either.IsLeft ? fn(either.Left) : @default();
        }

        /// <summary>
        /// Return the result of applying the value of a <c>Left</c> to the function <paramref name="fn"/> or in the case of
        /// a <c>Right</c> the default value generated from the function <paramref name="default"/>.
        /// </summary>
        public static TResult SelectRight<TL, TR, TResult>(this Either<TL, TR> either, Func<TR, TResult> fn, Func<TResult> @default)
        {
            return either.IsRight ? fn(either.Right) : @default();
        }

        /// <summary>
        /// Return the result of applying the value of a <c>Left</c> to the function <paramref name="fn"/> or in the case of
        /// a <c>Right</c> the default value <paramref name="default"/>.
        /// </summary>
        public static TResult SelectLeft<TL, TR, TResult>(this Either<TL, TR> either, Func<TL, TResult> fn, TResult @default)
        {
            return either.IsLeft ? fn(either.Left) : @default;
        }

        /// <summary>
        /// Return the result of applying the value of a <c>Left</c> to the function <paramref name="fn"/> or in the case of
        /// a <c>Right</c> the default value generated from the function <paramref name="default"/>.
        /// </summary>
        public static TResult SelectRight<TL, TR, TResult>(this Either<TL, TR> either, Func<TR, TResult> fn, TResult @default)
        {
            return either.IsRight ? fn(either.Right) : @default;
        }

        /// <summary>
        /// Execute the side effect <paramref name="action"/> with the value of the <c>Right</c> or do nothing.
        /// </summary>
        public static void Run<TL, TR>(this Either<TL, TR> either, Action<TR> action)
        {
            if (either.IsRight)
            {
                action(either.Right);
            }
        }

        /// <summary>
        /// Excecute the side effect <paramref name="action"/> with the value of a <c>Right</c> or throw an
        /// exception (either <paramref name="e"/> or <see cref="InvalidOperationException"/>)
        /// </summary>
        public static void RunOrThrow<TL, TR>(this Either<TL, TR> either, Action<TR> action, Exception e = null)
        {
            if (either.IsLeft)
            {
                throw e ?? new InvalidOperationException("RunOrThrow: Either.Left can not be run.");
            }

            action(either.Right);
        }

        /// <summary>
        /// Execute the side effect <paramref name="action"/> if the value in the <c>Right</c> of type <c>bool</c> is
        /// <c>true</c>. Otherwise nothing is done.
        /// </summary>
        public static void RunWhenTrue<TL>(this Either<TL, bool> either, Action action)
        {
            if (either.IsRight && either.Right)
            {
                action();
            }
        }

        /// <summary>
        /// Return a new <c>Right</c> containing the transformed value of <paramref name="source"/>. If <paramref name="source"/> is a
        /// <c>Left</c>, a <c>Left</c> will be returned. The function <paramref name="selector"/> will be used to transform the
        /// <c>Right</c>.
        /// </summary>
        public static Either<TL, TResult> Select<TL, TR, TResult>(this Either<TL, TR> source, Func<TR, TResult> selector)
        {
            return source.IsLeft ? Left<TL, TResult>(source.Left) : Right<TL, TResult>(selector(source.Right));
        }

        /// <summary>
        /// Apply the <paramref name="selector"/> to every either in the given enumerable <paramref name="eithers"/>.
        /// </summary>
        public static IEnumerable<Either<TL, TResult>> Select<TL, TR, TResult>(this IEnumerable<Either<TL, TR>> eithers, Func<TR, TResult> selector)
        {
            return eithers.Select(either => either.Select(selector));
        }

        /// <summary>
        /// Return an enumerable containing only the with the function <paramref name="func"/> transformed <c>Rights</c>'s of the
        /// given <paramref name="eithers"/>.
        /// </summary>
        public static IEnumerable<TResult> SelectValid<TL, TR, TResult>(this IEnumerable<Either<TL, TR>> eithers, Func<TR, TResult> func)
        {
            return from either in eithers
                   where either.IsRight
                   select func(either.Right);
        }

        /// <summary>
        /// Return an enumerable containing only the <c>Rights</c>'s of the given <paramref name="eithers"/>.
        /// </summary>
        public static IEnumerable<TR> SelectValid<TL, TR>(this IEnumerable<Either<TL, TR>> eithers)
        {
            return SelectValid(eithers, m => m);
        }

        /// <summary>
        /// Filter an enumerable <paramref name="xs"/> of values with the predicate function <paramref name="pred"/>.
        /// If <paramref name="pred"/> returns <c>Left</c>, the result of <see cref="WhereEither{TL,TR}"/> will also be this <c>Left</c>.
        /// Else the boolean value inside the <c>Right</c> returned by the predicate function <paramref name="pred"/> will be used
        /// to filter the values in <paramref name="xs"/>. Only values for which the predicate returns <c>true</c> are then
        /// included in the resulting enumeration in the <c>Right</c>.
        /// </summary>
        public static Either<TL, IEnumerable<TR>> WhereEither<TL, TR>(this IEnumerable<TR> xs, Func<TR, Either<TL, bool>> pred)
        {
            var l = new List<TR>();
            foreach (var x in xs)
            {
                var r = pred(x);
                if (r.IsLeft)
                {
                    return Left<TL, IEnumerable<TR>>(r.Left);
                }
                if (r.Right)
                {
                    l.Add(x);
                }
            }
            return Right<TL, IEnumerable<TR>>(l);
        }

        /// <summary>
        /// Filter a single <see cref="Either"/>. <see cref="Where"/> returns <c>Left</c> if either <paramref name="either"/> is <c>Left</c> or
        /// <paramref name="pred"/> return <c>false</c>. Otherwise <paramref name="either"/> is returned.
        /// </summary>
        public static Either<TL, TR> Where<TL, TR>(this Either<TL, TR> either, Func<TR, bool> pred)
        {
            if (either.IsLeft)
            {
                return either;
            }
            return pred(either.Right) ? either : Left<TL, TR>(default(TL));
        }

        public static Either<TL, TResult> SelectMany<TL, TR, TResult>(this Either<TL, TR> source, Func<TR, Either<TL, TResult>> selector)
        {
            return source.IsLeft ? Left<TL, TResult>(source.Left) : selector(source.Right);
        }

        public static Either<TL, TResult> SelectMany<TL, TR, TM, TResult>(this Either<TL, TR> source, Func<TR, Either<TL, TM>> collectionSelector, Func<TR, TM, TResult> resultSelector)
        {
            return source.SelectMany(x => collectionSelector(x).SelectMany(y => resultSelector(x, y).Right<TL, TResult>()));
        }

        /// <summary>
        /// Turn a enumeration of <c>Right</c>'s into a <c>Right</c> containing an enumeration of the <c>Rights</c>'s values. If the
        /// enumeration <paramref name="eithers"/> contains a <c>Left</c>, <see cref="Sequence{TL,TR}"/> will return the first <c>Left</c> value.
        /// </summary>
        public static Either<TL, IEnumerable<TR>> Sequence<TL, TR>(this IEnumerable<Either<TL, TR>> eithers)
        {
            var list = new List<TR>();
            foreach (var either in eithers)
            {
                if (either.IsLeft)
                {
                    return Left<TL, IEnumerable<TR>>(either.Left);
                }
                list.Add(either.Right);
            }

            return Right<TL, IEnumerable<TR>>(list);
        }
    }

    [Serializable]
    public class EitherValueAccessException : Exception
    {
        public EitherValueAccessException() { }
        public EitherValueAccessException(string message) : base(message) { }
        public EitherValueAccessException(string message, Exception innerException) : base(message, innerException) { }
        protected EitherValueAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// The Either type represents values with two possibilities: a value of type <c>Either{TL, TR}</c> is
    /// either <c>Left{TL}</c> or <c>Right{TR}</c>.
    /// <para>
    /// The Either type is sometimes used to represent a value which is either correct or an error;
    /// by convention, the <c>Left</c> is used to hold an error value and the <c>Right</c> is used to hold a correct value
    /// (mnemonic: "right" also means "correct"). 
    /// </para>
    /// </summary>
    /// <typeparam name="TL">Type of the left value</typeparam>
    /// <typeparam name="TR">Type of the right value</typeparam>
    public struct Either<TL, TR> : IEquatable<Either<TL, TR>>
    {
        private readonly TL _left;
        private readonly TR _right;
        private readonly bool _isLeft;

        /// <summary>
        /// Don't use!
        /// (Initialize an <c>Left</c> Either.)
        /// </summary>
        public Either(TL leftValue)
        {
            _left = leftValue;
            _right = default(TR);
            _isLeft = true;
        }

        /// <summary>
        /// Don't use!
        /// (Initialize an <c>Right</c> Either.)
        /// </summary>
        public Either(TR rightValue)
        {
            _right = rightValue;
            _left = default(TL);
            _isLeft = false;
        }

        /// <summary>
        /// <c>true</c> if the option is a <c>Left</c>.
        /// </summary>
        public bool IsLeft { get { return _isLeft; } }

        /// <summary>
        /// <c>true</c> if the option is a <c>Right</c>.
        /// </summary>
        public bool IsRight { get { return !_isLeft; } }

        /// <summary>
        /// The value of an <c>Left</c>.
        /// <para>Throws an exception on an <c>Right</c>. Don't catch this exception!</para>
        /// </summary>
        public TL Left
        {
            get
            {
                if (_isLeft)
                {
                    return _left;
                }
                throw new EitherValueAccessException("Tried to get the left value from a right either.");
            }
        }

        /// <summary>
        /// The value of an <c>Right</c>.
        /// <para>Throws an exception on an <c>Left</c>. Don't catch this exception!</para>
        /// </summary>
        public TR Right
        {
            get
            {
                if (!_isLeft)
                {
                    return _right;
                }
                throw new EitherValueAccessException("Tried to get the right value from a left either.");
            }
        }

        public override string ToString()
        {
            var tleft = typeof(TL).ToString();
            var tright = typeof(TR).ToString();
            return _isLeft ? string.Format("Either<{0}, {1}>.Left({2})", tleft, tright, _left)
                           : string.Format("Either<{0}, {1}>.Right({2})", tleft, tright, _right);
        }

        public bool Equals(Either<TL, TR> other)
        {
            return other._isLeft.Equals(_isLeft) && (_isLeft ? Equals(other._left, _left) : Equals(other._right, _right));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Either<TL, TR>)) return false;
            return Equals((Either<TL, TR>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_isLeft.GetHashCode() * 397) ^ (_isLeft ? _left.GetHashCode() : _right.GetHashCode());
            }
        }

        public static bool operator ==(Either<TL, TR> left, Either<TL, TR> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Either<TL, TR> left, Either<TL, TR> right)
        {
            return !left.Equals(right);
        }
    }

}