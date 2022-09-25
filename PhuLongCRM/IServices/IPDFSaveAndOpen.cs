using System;
using System.IO;
using System.Threading.Tasks;

namespace PhuLongCRM.IServices
{
	public interface IPDFSaveAndOpen
	{
		Task SaveAndView(string fileName, byte[] data, string location = "Download/PDFFiles");
	}
	public enum PDFOpenContext
	{
		InApp,
		ChooseApp
	}
}

