using System;
using System.IO;
using System.Threading.Tasks;

namespace PhuLongCRM.Controls
{
    public interface IFileService
    {
        string SaveFile(string name, byte[] data, MemoryStream stream, string location = "Download/PhuLong");
        void OpenFile(string fileName, string location = "Download/PhuLong");
    }
}
