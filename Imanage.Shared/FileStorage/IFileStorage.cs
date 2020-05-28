using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Imanage.Shared.FileStorage
{
    public interface IFileStorage : IFileInfo
    {
        string GetFileType();

        Stream OpenRead();

        Stream OpenWrite();

        Stream CreateFile();
    }
}
