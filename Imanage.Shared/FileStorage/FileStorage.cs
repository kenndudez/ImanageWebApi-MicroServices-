using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Imanage.Shared.FileStorage
{
    public class FileStorage
    {
        private readonly string _path;
        private readonly FileInfo _fileInfo;

        public FileStorage(string path, FileInfo fileInfo)
        {
            _path = path;
            _fileInfo = fileInfo;
        }

        public string GetFileType()
        {
            return _fileInfo.Extension;
        }

        public Stream OpenRead()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read);
        }

        public Stream OpenWrite()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
        }

        public Stream CreateFile()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Truncate, FileAccess.ReadWrite);
        }
    }
}
