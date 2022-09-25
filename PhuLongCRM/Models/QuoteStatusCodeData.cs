using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class QuoteStatusCodeData
    {
        public static List<StatusCodeModel> QuoteStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000",Language.dat_coc,"#ffc43d"), // Reservation
                new StatusCodeModel("100000001",Language.da_thanh_ly,"#F43927"), // Terminated
                new StatusCodeModel("100000002",Language.dang_cho_huy_bo_tien_gui,"#C6DBE4"),//Pending Cancel Deposit
                new StatusCodeModel("100000003",Language.tu_choi,"#E96F60"), // Reject
                new StatusCodeModel("100000004",Language.da_ky_rf,"#ADBED3"),//Signed RF
                new StatusCodeModel("100000005",Language.da_het_han_ky_rf,"#DCEBA3"), // Expired of signing RF
                new StatusCodeModel("100000006",Language.chuyen_coc,"#78CCBA"),//Collected
                new StatusCodeModel("100000007",Language.bao_gia,"#FF8F4F"), //Quotation
                new StatusCodeModel("100000008",Language.bao_gia_het_han,"#E4C2BA"), // Expired Quotation
                new StatusCodeModel("100000009",Language.het_han,"#B3B3B3"), // ~ Het han
                new StatusCodeModel("100000010",Language.da_ky_phieu_coc,"#A6A8CF"),//Đã ký phiếu cọc
                new StatusCodeModel("100000012",Language.nhap,"#808080"),

                new StatusCodeModel("1",Language.dang_xu_ly,"#00CF79"),//In Progress
                new StatusCodeModel("2",Language.dang_xu_ly,"#00CF79"),//In Progress
                new StatusCodeModel("3",Language.tt_du_tien_coc,"#04A8F4"),//Deposited
                new StatusCodeModel("4",Language.thanh_cong,"#8bce3d"), // Won
                new StatusCodeModel("5",Language.mat_khach_hang,"#BED4CA"), // Lost
                new StatusCodeModel("6",Language.da_huy,"#808080"), // Canceled
                new StatusCodeModel("7",Language.da_sua_doi,"#1C0870"), // Revised

                new StatusCodeModel("861450001",Language.da_trinh,"#5AAF2F"),//Submitted
                new StatusCodeModel("861450002",Language.da_duyet,"#27964F"),//Approved
                new StatusCodeModel("861450000",Language.thay_doi_thong_tin,"#F89A5B"),//Change information
            };
        }

        public static StatusCodeModel GetQuoteStatusCodeById(string id)
        {
            return QuoteStatusData().SingleOrDefault(x => x.Id == id);
        }
        public static List<StatusCodeModel> GetQuoteByIds(string ids)
        {
            List<StatusCodeModel> listQueue = new List<StatusCodeModel>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                listQueue.Add(GetQuoteStatusCodeById(item));
            }
            return listQueue;
        }
    }
}
