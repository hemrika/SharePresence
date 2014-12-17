// -----------------------------------------------------------------------
// <copyright file="FileSystemWrapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Video
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Security.Permissions;
    using System.IO;

    public interface IFileSystemWrapper
    {
        void CreateDirectory(string path);
        void DeleteDirectoryIfExists(string path);
        bool DirectoryExists(string path);

        void DemandFileIOPermission(FileIOPermissionAccess access, string path);

        void CreateFile(string path);
        bool FileExists(string path);
        Stream FileOpenRead(string path);
        void WriteStreamToFile(Stream stream, string filePath);
    }

    class FileSystemWrapper : IFileSystemWrapper
    {
        public void CreateDirectory(string path) { Directory.CreateDirectory(path); }
        public void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public bool DirectoryExists(string path) { return Directory.Exists(path); }

        public void CreateFile(string path)
        {
            FileStream stream = File.Create(path);
            stream.Flush();
            stream.Close();
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool FileExists(string path) { return File.Exists(path); }
        public Stream FileOpenRead(string path) { return File.OpenRead(path); }
        public void DemandFileIOPermission(FileIOPermissionAccess access, string path)
        {
            FileIOPermission permission = new FileIOPermission(access, path);
            permission.Demand();
        }

        public void WriteStreamToFile(Stream stream, string filePath)
        {
            // Write the stream to a temporary file.
            using (FileStream fs = File.OpenWrite(filePath))
            {
                byte[] buffer = new byte[32 * 1024]; // 32 KB is a good balance between speed and memory usage.
                int read;

                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    fs.Write(buffer, 0, read);

                //fs.Flush();
                //fs.Close();
            }
        }
    }
}
