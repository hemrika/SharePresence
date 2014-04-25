// -----------------------------------------------------------------------
// <copyright file="FileInfoExtensions.cs" company="">
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

    public static class FileInfoExtensions
    {
        public static bool FindInFile(this FileInfo file, string term)
        {
            if (term == null)
            {
                return false;
            }


            var fileStream = file.OpenText();


            while (!fileStream.EndOfStream)
            {
                var line = fileStream.ReadLine();


                if (line != null &&
                    line.Contains(term))
                {
                    return true;
                }
            }


            return false;
        }

        /// <summary>
        /// Umbenennen einer Datei (incl. Dateityp).
        /// </summary>
        public static FileInfo Rename(this FileInfo file, string newName)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(file.FullName), newName);
            file.MoveTo(filePath);
            return file;
        }

        /// <summary>
        /// Umbenennen einer Datei (ohne Dateityp).
        /// </summary>
        public static FileInfo RenameFileWithoutExtension(this FileInfo file, string newName)
        {
            var fileName = string.Concat(newName, file.Extension);
            file.Rename(fileName);
            return file;
        }

        /// <summary>
        /// Ändert den Dateitypen
        /// </summary>
        public static FileInfo ChangeExtension(this FileInfo file, string newExtension)
        {
            if (newExtension.StartsWith("."))
            {
                var fileName = string.Concat(Path.GetFileNameWithoutExtension(file.FullName), newExtension);
                file.Rename(fileName);
                return file;
            }
            throw new ArgumentException("The new Extension must start with '.'");
        }
    }

}
