using System;
namespace PhuLongCRM.Models
{
	public class InterestInstallmentModel
	{
        public Guid bsd_paymentschemedetailid { get; set; }
        public decimal bsd_interestchargeamount { get; set; }
        public decimal bsd_interestwaspaid { get; set; }
        public decimal bsd_interestchargeremaining { get; set; }
        public int bsd_actualgracedays { get; set; }
        public string bsd_interestchargestatus { get; set; }

        public string interestChargeAmountFormat { get => Helper.StringFormatHelper.FormatCurrency(this.bsd_interestchargeamount); }
        public string interestChargePaidFormat { get => Helper.StringFormatHelper.FormatCurrency(this.bsd_interestwaspaid); }
        public string interestChargeRemainingFormat { get => Helper.StringFormatHelper.FormatCurrency(this.bsd_interestwaspaid); }
        public string interestChargeStatusFormat { get => InterestChargeStatusData.GetInterestChargeStatusById(bsd_interestchargestatus).Label; }
    }
}

