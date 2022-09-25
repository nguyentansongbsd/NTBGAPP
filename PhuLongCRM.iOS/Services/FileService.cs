using System;
using System.IO;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace PhuLongCRM.iOS.Services
{
	public class FileService : IFileService
	{
        public void OpenFile(string fileName, string location = "Download/PhuLong")
        {
            //throw new NotImplementedException();
        }

        public string SaveFile(string name, byte[] data, MemoryStream stream, string location = "Download/PhuLong")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(documentsPath, name.Replace(' ', '_'));
            File.WriteAllBytes(path, data);
            return path;
        }
    }
}

