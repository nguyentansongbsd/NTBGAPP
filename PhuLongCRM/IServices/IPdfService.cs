using System;
using System.Threading.Tasks;

namespace PhuLongCRM.IServices
{
    public interface IPdfService
    {
        Task View(string url, string name);
    }
}
