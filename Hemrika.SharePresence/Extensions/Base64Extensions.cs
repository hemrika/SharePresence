// -----------------------------------------------------------------------
// <copyright file="Base64Extensions.cs" company="">
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
    using System.Drawing.Imaging;
    using System.Drawing;

    public static class Base64Extensions
    {
        public static string BitmapToBase64(this Image image)
        {
            try
            {
                var memory = new MemoryStream();
                image.Save(memory, ImageFormat.Jpeg);
                var base64 = Convert.ToBase64String(memory.ToArray());
                memory.Close();
                memory.Dispose();
                return base64;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Bitmap BitmapFromBase64(this String base64)
        {
            try
            {
                var memory = new MemoryStream(Convert.FromBase64String(base64));
                var bitmap = new Bitmap(memory);
                memory.Close();
                memory.Dispose();
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }

}
