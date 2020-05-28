using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;

namespace Imanage.Shared.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IFileProvider _fileProvider;

        public FileStorageService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public void DeleteFile(string path)
        {
            FileInfo fileInfo = new FileInfo(MapStorage(path));
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            fileInfo.Delete();
        }

        public void RenameFile(string oldPath, string newPath)
        {
            FileInfo sourceFileInfo = new FileInfo(MapStorage(oldPath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", oldPath));
            }

            FileInfo targetFileInfo = new FileInfo(MapStorage(newPath));
            if (targetFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", newPath));
            }

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);
        }

        public string RenameDuplicateFile(string filepath)
        {
            var sourceFileInfo = new FileInfo(MapStorage(filepath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exists", filepath));
            }

            var folder = sourceFileInfo.Directory + "/";
            var oldFileInfo = sourceFileInfo.Name.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var newFilePath = "";
            var counter = 1;

            do
            {
                counter++;
                newFilePath = folder + oldFileInfo[0] + " (" + counter.ToString() + ")" + oldFileInfo[1];
            } while (File.Exists(newFilePath));

            //join full directory
            RenameFile(filepath, newFilePath);

            FileInfo targetFileInfo = new FileInfo(MapStorage(newFilePath));

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);

            return newFilePath;
        }

        public bool ValidateFile(string[] allowedExt, string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return allowedExt.ToList().Any(x => $".{x}".Equals(ext, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool TrySaveStream(string path, Stream inputStream)
        {
            try
            {
                if (FileExists(path))
                {
                    return false;
                }

                SaveStream(path, inputStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public FileInfo CreateFile(string path)
        {
            FileInfo fileInfo = new FileInfo(MapStorage(path));
            if (fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", fileInfo.Name));
            }

            // ensure the directory exists
            var dirName = Path.GetDirectoryName(fileInfo.FullName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllBytes(fileInfo.FullName, new byte[0]);

            return fileInfo;

        }

        public void SaveStream(string path, Stream inputStream)
        {
            // Create the file.
            // The CreateFile method will map the still relative path
            var file = CreateFile(path);

            inputStream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = file.OpenWrite())
            {
                var buffer = new byte[8192];
                for (; ; )
                {

                    var length = inputStream.Read(buffer, 0, buffer.Length);
                    if (length <= 0)
                        break;
                    outputStream.Write(buffer, 0, length);
                }
            }
        }

        public void SaveBytes(string path, byte[] raw)
        {
            var ms = new MemoryStream();
            ms.Write(raw, 0, raw.Length);

            SaveStream(path, ms);
        }

        public bool TrySaveBytes(string path, byte[] raw)
        {

            try
            {
                if (FileExists(path))
                {
                    return false;
                }

                var ms = new MemoryStream();
                ms.Write(raw, 0, raw.Length);

                SaveStream(path, ms);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TryCreateFolder(string path)
        {
            try
            {
                // prevent unnecessary exception
                DirectoryInfo directoryInfo = new DirectoryInfo(MapStorage(path));
                if (directoryInfo.Exists)
                {
                    return false;
                }

                CreateFolder(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void CreateFolder(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(MapStorage(path));
            if (directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", path));
            }

            Directory.CreateDirectory(directoryInfo.FullName);
        }

        public bool FolderExists(string path)
        {
            return new DirectoryInfo(MapStorage(path)).Exists;
        }

        public bool FileExists(string path)
        {
            return File.Exists(MapStorage(path));
            //return File.Exists(path);
        }


        private string MapStorage(string path)
        {
            return _fileProvider.GetFileInfo(path).PhysicalPath;
            //return path;
        }


        public IFileInfo GetFile(string path)
        {
            var fileInfo = _fileProvider.GetFileInfo(path);
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            return fileInfo;
        }
        

        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();



    }
}
