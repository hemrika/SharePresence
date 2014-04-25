// -----------------------------------------------------------------------
// <copyright file="ByteArrayExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class ByteArrayExtensions
    {
        public static T ToObject<T>(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var formatter = new BinaryFormatter();


            stream.Position = 0;


            return (T)formatter.Deserialize(stream);
        }
    }
}
