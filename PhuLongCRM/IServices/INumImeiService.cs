using System;
using System.Threading.Tasks;

namespace PhuLongCRM.IServices
{
    public interface INumImeiService
    {
        Task<string> GetImei();
    }
}
