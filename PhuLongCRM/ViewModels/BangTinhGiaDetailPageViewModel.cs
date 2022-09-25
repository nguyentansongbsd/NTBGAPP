using Newtonsoft.Json;
using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class BangTinhGiaDetailPageViewModel : BaseViewModel
    {
        public Guid ReservationId { get; set; }
        private ReservationDetailPageModel _reservation;
        public ReservationDetailPageModel Reservation { get => _reservation; set { _reservation = value; OnPropertyChanged(nameof(Reservation)); } }
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; }

        public OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }
        public OptionSet _customerReferral;
        public OptionSet CustomerReferral { get => _customerReferral; set { _customerReferral = value; OnPropertyChanged(nameof(CustomerReferral)); } }
        public ObservableCollection<ReservationCoownerModel> CoownerList { get; set; }
        public ObservableCollection<ReservationInstallmentDetailPageModel> InstallmentList { get; set; }

        private int _numberInstallment;
        public int NumberInstallment { get => _numberInstallment; set { _numberInstallment = value; OnPropertyChanged(nameof(NumberInstallment)); } }

        private bool _showInstallmentList;
        public bool ShowInstallmentList { get => _showInstallmentList; set { _showInstallmentList = value; OnPropertyChanged(nameof(ShowInstallmentList)); } }

        private StatusCodeModel _quoteStatus;
        public StatusCodeModel QuoteStatus { get => _quoteStatus; set { _quoteStatus = value; OnPropertyChanged(nameof(QuoteStatus)); } }

        public ObservableCollection<DiscountModel> ListDiscount { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountModel> ListDiscountPaymentScheme { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountModel> ListDiscountInternel { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountModel> ListDiscountExchange { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<OptionSet> ListPromotion { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<DiscountSpecialModel> ListSpecialDiscount { get; set; } = new ObservableCollection<DiscountSpecialModel>();
        //popup
        private PromotionModel _promotionItem;
        public PromotionModel PromotionItem { get => _promotionItem; set { _promotionItem = value; OnPropertyChanged(nameof(PromotionItem)); } }

        private HandoverConditionModel _handoverConditionItem;
        public HandoverConditionModel HandoverConditionItem { get => _handoverConditionItem; set { _handoverConditionItem = value; OnPropertyChanged(nameof(HandoverConditionItem)); } }

        private DiscountSpecialModel _discountSpecialItem;
        public DiscountSpecialModel DiscountSpecialItem { get => _discountSpecialItem; set { _discountSpecialItem = value; OnPropertyChanged(nameof(DiscountSpecialItem)); } }

        private DiscountModel _discount;
        public DiscountModel Discount { get => _discount; set { _discount = value; OnPropertyChanged(nameof(Discount)); } }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await RefreshBTG();
            IsRefreshing = false;
        });

        public string UpdateQuote = "1";
        public string UpdateQuotation = "2";
        public string ConfirmReservation = "3";
        public string UpdateReservation = "4";
        public string CodeContact = LookUpMultipleTabs.CodeContac;
        public string CodeAccount = LookUpMultipleTabs.CodeAccount;
        public BangTinhGiaDetailPageViewModel()
        {
            CoownerList = new ObservableCollection<ReservationCoownerModel>();
            InstallmentList = new ObservableCollection<ReservationInstallmentDetailPageModel>();
            Reservation = new ReservationDetailPageModel();
            Customer = new OptionSet();
            ButtonCommandList = new ObservableCollection<FloatButtonItem>();
        }

        #region Chinh Sach
        public async Task LoadReservation(Guid ReservationId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' />
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <attribute name='quoteid' />
                                    <attribute name='bsd_reservationno' />  
                                    <attribute name='bsd_quotationnumber' />
                                    <attribute name='quotenumber' />
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_managementfee' />
                                    <attribute name='bsd_rejectdate' />
                                    <attribute name='bsd_rejectreason' />
                                    <attribute name='bsd_salesdepartmentreceiveddeposit' />
                                    <attribute name='bsd_receiptdate' />
                                    <attribute name='bsd_depositfeereceived' />
                                    <attribute name='bsd_detailamount' />
                                    <attribute name='bsd_discount' />
                                    <attribute name='bsd_interneldiscount' />
                                    <attribute name='bsd_selectedchietkhaupttt' />
                                    <attribute name='bsd_exchangediscount' />
                                    <attribute name='bsd_packagesellingamount' />
                                    <attribute name='bsd_totalamountlessfreight' />
                                    <attribute name='bsd_landvaluededuction' />
                                    <attribute name='totaltax' />
                                    <attribute name='bsd_freightamount' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_followuplist' />
                                    <attribute name='bsd_unitstatus' />
                                    <attribute name='bsd_netusablearea' />
                                    <attribute name='bsd_actualarea'/>
                                    <attribute name='bsd_bookingfee' />
                                    <attribute name='bsd_depositfee' />
                                    <attribute name='bsd_totalamountpaid' />
                                    <attribute name='bsd_quotationprinteddate' />
                                    <attribute name='bsd_expireddateofsigningqf' />
                                    <attribute name='bsd_quotationsigneddate' />
                                    <attribute name='bsd_reservationtime' />
                                    <attribute name='bsd_deposittime' />
                                    <attribute name='bsd_nameofstaffagent' />
                                    <attribute name='bsd_referral' />
                                    <attribute name='bsd_reservationformstatus' />
                                    <attribute name='bsd_reservationprinteddate' />
                                    <attribute name='bsd_signingexpired' />
                                    <attribute name='bsd_rfsigneddate' />
                                    <attribute name='bsd_reservationuploadeddate' />
                                    <attribute name='bsd_discountlist' alias='bsd_discounttypeid'/>
                                    <attribute name='bsd_discounts'/>
                                    <attribute name='bsd_discount'/>
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='aa'>
      	                                <attribute name='bsd_name' alias='purchaser_account_name'/>
    	                                <attribute name='accountid' alias='purchaser_accountid'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ab'>
     	                                <attribute name='bsd_fullname' alias='purchaser_contact_name'/>
    	                                <attribute name='contactid' alias='purchaser_contactid'/>
                                    </link-entity>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='outer' alias='ac'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                        <attribute name='bsd_projectid' alias='project_id' />
                                        <attribute name='bsd_quotationvalidatetime' alias='quotationvalidate'/>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' link-type='outer' alias='ad'>
                                        <attribute name='pricelevelid' alias='pricelevel_id_apply'/>
                                        <attribute name='name' alias='pricelevel_name_apply'/>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistphaselaunch' link-type='outer' alias='ae'>
                                        <attribute name='pricelevelid' alias='pricelevel_id_phaseslaunch'/>
                                        <attribute name='name' alias='pricelevel_name_phaseslaunch'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner' alias='ak'>
                                        <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id'/>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' link-type='outer' alias='af'>
                                        <attribute name='bsd_taxcodeid' alias='taxcode_id'/>
                                        <attribute name='bsd_name' alias='taxcode_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_salessgentcompany' link-type='outer' alias='ag'>
                                        <attribute name='bsd_name' alias='salescompany_account_name'/>
                                        <attribute name='accountid' alias='salescompany_accountid'/>
                                    </link-entity>
                                    <link-entity name='opportunity' from='opportunityid' to='opportunityid' link-type='outer' alias='al'>
                                        <attribute name='name' alias='queue_name' />
                                        <attribute name='opportunityid' alias='queue_id' /> 
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitno' link-type='outer' alias='ap'>
                                        <attribute name='name' alias='unit_name' />
                                        <attribute name='productid' alias='unit_id' />
                                    </link-entity>
                                    <filter type='and'>
	                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
	                                </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDetailPageModel>>("quotes", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }

            Reservation = result.value.SingleOrDefault();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' />
                                    <attribute name='bsd_discounts'/>
                                    <attribute name='quoteid' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' link-type='outer' alias='apo'>
                                        <attribute name='bsd_name' alias='paymentscheme_name'/>
                                        <attribute name='bsd_paymentschemeid' alias='paymentscheme_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='discountlist_name' />
                                    </link-entity>
                                    <link-entity name='bsd_interneldiscount' from='bsd_interneldiscountid' to='bsd_interneldiscountlist' visible='false' link-type='outer' alias='a_c014fc37ba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_name' alias='interneldiscount_name'/>
                                        <attribute name='bsd_interneldiscountid' alias='interneldiscount_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discountpromotion' from='bsd_discountpromotionid' to='bsd_exchangediscountlist' visible='false' link-type='outer' alias='a_2e80b433b075eb11a812000d3ac8b5f4'>
                                        <attribute name='bsd_name' alias='discountpromotion_name'/>
                                        <attribute name='bsd_discountpromotionid' alias='discountpromotion_id'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_collaborator' visible='false' link-type='outer' alias='a_ceb0dc55ba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_fullname' alias='collaborator_name'/>
                                        <attribute name='contactid' alias='collaborator_id' />
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_customerreferral' visible='false' link-type='outer' alias='a_ef3c042cba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_name' alias='customerreferral_account_name'/>
                                        <attribute name='accountid' alias='customerreferral_account_id'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customerreferral' visible='false' link-type='outer' alias='a_d6b0dc55ba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_fullname' alias='customerreferral_contact_name'/>
                                        <attribute name='contactid' alias='customerreferral_contact_id' />
                                    </link-entity>
                                    <link-entity name='salesorder' from='quoteid' to='quoteid' link-type='outer'>
                                        <attribute name='salesorderid' alias='salesorder_id'/>
                                    </link-entity>
                                    <filter type='and'>
	                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
	                                </filter>
                                  </entity>
                                </fetch>";

            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDetailPageModel>>("quotes", fetch);
            if (result2 != null && result2.value.Count > 0)
            {
                var data = result2.value.SingleOrDefault();
                Reservation.paymentscheme_id = data.paymentscheme_id;
                Reservation.paymentscheme_name = data.paymentscheme_name;
                Reservation.discountlist_name = data.discountlist_name;
                Reservation.interneldiscount_id = data.interneldiscount_id;
                Reservation.interneldiscount_name = data.interneldiscount_name;
                Reservation.discountpromotion_id = data.discountpromotion_id;
                Reservation.discountpromotion_name = data.discountpromotion_name;
                Reservation.collaborator_id = data.collaborator_id;
                Reservation.collaborator_name = data.collaborator_name;
                Reservation.salesorder_id = data.salesorder_id;
                if (data.customerreferral_account_id != Guid.Empty)
                {
                    this.CustomerReferral = new OptionSet() { Val = data.customerreferral_account_id.ToString(), Label = data.customerreferral_account_name, Title = "2" };
                }
                else if (data.customerreferral_contact_id != Guid.Empty)
                {
                    this.CustomerReferral = new OptionSet() { Val = data.customerreferral_contact_id.ToString(), Label = data.customerreferral_contact_name, Title = "3" };
                }
            }

            if (!string.IsNullOrEmpty(Reservation.purchaser_account_name))
            {
                Customer.Val = Reservation.purchaser_accountid.ToString();
                Customer.Label = Reservation.purchaser_account_name;
                Customer.Title = CodeAccount;

            }
            else
            {
                Customer.Val = Reservation.purchaser_contactid.ToString();
                Customer.Label = Reservation.purchaser_contact_name;
                Customer.Title = CodeContact;
            }

            this.QuoteStatus = QuoteStatusCodeData.GetQuoteStatusCodeById(this.Reservation.statuscode.ToString());
        }
        public async Task LoadHandoverCondition(Guid ReservationId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_packageselling'>
                                        <attribute name='bsd_name' alias='Label' />
                                        <attribute name='bsd_packagesellingid' alias='Val' />
                                        <order attribute='bsd_name' descending='true' />
                                        <link-entity name='bsd_quote_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                                            <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0) return;

            var data = result.value.FirstOrDefault();
            Reservation.handovercondition_id = Guid.Parse(data.Val);
            Reservation.handovercondition_name = data.Label;
        }
        public async Task LoadSpecialDiscount(Guid ReservationId)
        {
            // statuscode = Approved / 100000000
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_discountspecial'>
                                        <attribute name='bsd_discountspecialid' />
                                        <attribute name='bsd_name' />
                                        <attribute name='bsd_percentdiscount' />
                                        <attribute name='bsd_cchtnh' />
                                        <attribute name='bsd_amountdiscount' />
                                        <attribute name='statuscode' />
                                        <attribute name='bsd_totalamount' />
                                        <order attribute='bsd_name' descending='false' />
                                        <filter type='and'>
                                        <condition attribute='statuscode' operator='eq' value='100000000' />
                                            <condition attribute='bsd_quote' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                                        </filter>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountSpecialModel>>("bsd_discountspecials", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListSpecialDiscount.Add(item);
            }
        }
        public async Task LoadPromotions(Guid ReservationId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_promotion'>
                                        <attribute name='bsd_name' alias='Label' />
                                        <attribute name='bsd_promotionid' alias='Val' />
                                        <order attribute='createdon' descending='true' />
                                        <link-entity name='bsd_quote_bsd_promotion' from='bsd_promotionid' to='bsd_promotionid' visible='false' intersect='true'>
                                            <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_promotions", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListPromotion.Add(item);
            }
        }
        public async Task LoadDiscounts()
        {
            if (string.IsNullOrWhiteSpace(this.Reservation.bsd_discounts)) return;
            string[] arrDiscounts = new string[] { };
            string conditionValue = string.Empty;
            arrDiscounts = this.Reservation.bsd_discounts.Split(',');
            for (int i = 0; i < arrDiscounts.Count(); i++)
            {
                conditionValue += $"<value uitype='bsd_discount'>{arrDiscounts[i]}</value>";
            }

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid'/>
                                    <attribute name='bsd_discountnumber' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percentage' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_discounttype' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountid' operator='in'>
                                        {conditionValue}
                                      </condition>
                                    </filter>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountModel>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListDiscount.Add(item);
            }
        }
        public async Task LoadDiscountsPaymentScheme()
        {
            if (string.IsNullOrWhiteSpace(this.Reservation.bsd_selectedchietkhaupttt)) return;
            string[] arrDiscountsPaymentscheme = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsPaymentscheme = this.Reservation.bsd_selectedchietkhaupttt.Split(',');
            for (int i = 0; i < arrDiscountsPaymentscheme.Count(); i++)
            {
                conditionValue += $"<value uitype='bsd_discount'>{arrDiscountsPaymentscheme[i]}</value>";
            }

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid'/>
                                    <attribute name='bsd_discountnumber' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percentage' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_discounttype' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountid' operator='in'>
                                        {conditionValue}
                                      </condition>
                                    </filter>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountModel>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListDiscountPaymentScheme.Add(item);
            }
        }
        public async Task LoadDiscountsInternel()
        {
            if (string.IsNullOrWhiteSpace(this.Reservation.bsd_interneldiscount)) return;
            string[] arrDiscountsInternel = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsInternel = this.Reservation.bsd_interneldiscount.Split(',');
            for (int i = 0; i < arrDiscountsInternel.Count(); i++)
            {
                conditionValue += $"<value uitype='bsd_discount'>{arrDiscountsInternel[i]}</value>";
            }

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid'/>
                                    <attribute name='bsd_discountnumber' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percentage' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_discounttype' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountid' operator='in'>
                                        {conditionValue}
                                      </condition>
                                    </filter>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountModel>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListDiscountInternel.Add(item);
            }
        }
        public async Task LoadDiscountsExChange()
        {
            if (string.IsNullOrWhiteSpace(this.Reservation.bsd_exchangediscount)) return;
            string[] arrDiscountsExchange = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsExchange = this.Reservation.bsd_exchangediscount.Split(',');
            for (int i = 0; i < arrDiscountsExchange.Count(); i++)
            {
                conditionValue += $"<value uitype='bsd_discount'>{arrDiscountsExchange[i]}</value>";
            }

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid'/>
                                    <attribute name='bsd_discountnumber' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percentage' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_discounttype' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountid' operator='in'>
                                        {conditionValue}
                                      </condition>
                                    </filter>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountModel>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.ListDiscountExchange.Add(item);
            }
        }
        public async Task LoadCoOwners(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_coowner'>
                                <attribute name='bsd_coownerid' />
                                <attribute name='bsd_name' />
                                <attribute name='bsd_relationship' />
                                <order attribute='bsd_name' descending='false' />
                                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                                  <attribute name='bsd_name' alias='account_name' />
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                                  <attribute name='bsd_fullname' alias='contact_name' />
                                </link-entity>
                                 <filter type='and'>
                                    <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                                    <condition attribute='statuscode' operator='eq' value='1' />
                                  </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationCoownerModel>>("bsd_coowners", xml);
            if (result != null)
            {
                var data = result.value;
                foreach (var x in result.value)
                {
                    if (!string.IsNullOrEmpty(x.account_name))
                    {
                        x.customer = x.account_name;
                    }
                    else
                    {
                        x.customer = x.contact_name;
                    }
                    CoownerList.Add(x);
                }
            }
        }
        #endregion

        #region Lich
        public async Task LoadInstallmentList(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentschemedetail'>
                <attribute name='bsd_paymentschemedetailid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_duedate' />
                <attribute name='statuscode' />
                <attribute name='bsd_amountofthisphase' />
                <attribute name='bsd_amountwaspaid' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_ordernumber' />
                <attribute name='bsd_amountpercent' />
                <attribute name='bsd_managementamount' />
                <attribute name='bsd_maintenanceamount' />
                <attribute name='bsd_signcontractinstallment' />
                <attribute name='bsd_duedatecalculatingmethod' />
                <order attribute='bsd_ordernumber' descending='false' />
                <filter type='and'>
                  <condition attribute='statecode' operator='eq' value='0' />
                  <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationInstallmentDetailPageModel>>("bsd_paymentschemedetails", xml);
            if (result == null || result.value.Count == 0)
                return;

            foreach (var x in result.value)
            {
                if (x.bsd_duedate.HasValue)
                    x.bsd_duedate = x.bsd_duedate.Value.ToLocalTime();
                InstallmentList.Add(x);
            }
            NumberInstallment = InstallmentList.Count();
            if (NumberInstallment > 0)
            {
                ShowInstallmentList = true;
            }
            else
            {
                ShowInstallmentList = false;
            }
        }
        #endregion

        public async Task<bool> ConfirmSinging()
        {
            string path = $"/quotes({Reservation.quoteid})";

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_quotationprinteddate"] = DateTime.Now.Date.ToUniversalTime();
            data["bsd_expireddateofsigningqf"] = DateTime.Now.Date.AddDays(this.Reservation.quotationvalidate).ToUniversalTime();

            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, data);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CrmApiResponse> SignQuotation()
        {
            var model = new
            {
                datesign = DateTime.Now.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:ss") 
            };

            var json = JsonConvert.SerializeObject(model);

            var apiResponse = await CrmHelper.PostData($"/quotes({Reservation.quoteid})//Microsoft.Dynamics.CRM.bsd_Action_QuotationReservation_ConvertToReservation", json);
            return apiResponse;
        }

        public async Task<bool> UpdateQuotes(string option)
        {
            string path = $"/quotes({Reservation.quoteid})";

            IDictionary<string, object> data = new Dictionary<string, object>();
            if (option == UpdateQuote)
            {
                data["statecode"] = Reservation.statecode;
                data["statuscode"] = Reservation.statuscode;
            }

            if (option == ConfirmReservation)
            {
                data["bsd_reservationuploadeddate"] = Reservation.bsd_reservationuploadeddate.Value.ToUniversalTime(); ;
            }

            if (option == UpdateReservation)
            {
                data["bsd_reservationformstatus"] = Reservation.bsd_reservationformstatus;
                data["bsd_rfsigneddate"] = Reservation.bsd_rfsigneddate.Value.ToUniversalTime(); ;
            }

            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, data);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CrmApiResponse> UpdatePaymentScheme()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["mobile"] = "yes";
            CrmApiResponse  apiResponse = await CrmHelper.PostData($"/quotes({Reservation.quoteid})/Microsoft.Dynamics.CRM.bsd_Action_Resv_Gene_PMS", data);
            return apiResponse;
        }

        public async Task<bool> DeactiveInstallment()
        {
            if (InstallmentList == null || InstallmentList.Count == 0)
                await LoadInstallmentList(Reservation.quoteid);

            if (InstallmentList != null && InstallmentList.Count > 0)
            {
                return await Deactive();
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Deactive()
        {
           if (Reservation.paymentscheme_id != Guid.Empty)
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                CrmApiResponse updateResponse = await CrmHelper.PostData($"/quotes({Reservation.quoteid})/Microsoft.Dynamics.CRM.bsd_Action_Clear_Installment", data);
                if (updateResponse.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CancelDeposit()
        {
            var data = new { };
            var apiResponse = await CrmHelper.PostData($"/quotes({Reservation.quoteid})//Microsoft.Dynamics.CRM.bsd_Action_Reservation_Cancel", data);

            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task LoadPromotionItem(string promotion_id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_promotion'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_values' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_promotionid' />
                                    <order attribute='bsd_name' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_promotionid' operator='eq' value='{promotion_id}' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PromotionModel>>("bsd_promotions", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            var data = result.value.SingleOrDefault();
            PromotionItem = data;
            if (data.bsd_startdate.HasValue)
                PromotionItem.bsd_startdate = data.bsd_startdate.Value.ToLocalTime();
            if (data.bsd_enddate.HasValue)
                PromotionItem.bsd_enddate = data.bsd_enddate.Value.ToLocalTime();
        }
        public async Task LoadHandoverConditionItem(Guid handovercondition_id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_packageselling'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_packagesellingid' />
                                    <attribute name='bsd_unittype' />
                                    <attribute name='bsd_type' />
                                    <attribute name='bsd_priceperm2' />
                                    <attribute name='bsd_percent' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_byunittype' />
                                    <attribute name='bsd_amount' />
                                    <order attribute='bsd_name' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_packagesellingid' operator='eq' value='{handovercondition_id}' />
                                    </filter>
                                    <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' link-type='outer' alias='aa'>
                                        <attribute name='bsd_name' alias='name_unit_type'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HandoverConditionModel>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            HandoverConditionItem = result.value.SingleOrDefault();
        }
        public async Task LoadDiscountSpecialItem(string discountspecialItem_id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discountspecial'>
                                    <attribute name='bsd_discountspecialid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_percentdiscount' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_totalamount' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountspecialid' operator='eq' value='{discountspecialItem_id}' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountSpecialModel>>("bsd_discountspecials", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            DiscountSpecialItem = result.value.SingleOrDefault();
        }
        public async Task LoadDiscountItem(Guid discount_id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_discounttype' />
                                    <attribute name='bsd_percentage' />
                                    <attribute name='bsd_discountnumber' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_discountid' />
                                    <attribute name='bsd_tovalue' />
                                    <attribute name='bsd_fromvalue' />
                                    <attribute name='bsd_special' />
                                    <attribute name='bsd_priority' />
                                    <attribute name='bsd_isconditionsapplied' />
                                    <attribute name='bsd_conditionsapply' />
                                    <order attribute='bsd_discountnumber' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_discountid' operator='eq' value='{discount_id}'/>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountModel>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            Discount = result.value.FirstOrDefault();
           // await LoadDiscountItems(Discount.bsd_discountid);
        }
        public async Task LoadDiscountItems(Guid discount_id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discountsitem'>
                                    <attribute name='bsd_discountsitemid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='1' />
                                    </filter>
                                    <link-entity name='bsd_discount' from='bsd_discountid' to='bsd_discounts' link-type='inner' alias='ad'>
                                      <filter type='and'>
                                        <condition attribute='bsd_discountid' operator='eq' value='{discount_id}' />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_discountsitems", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            Discount.distcount_list = new ObservableCollection<OptionSet>();
            foreach (var x in result.value)
            {
                Discount.distcount_list.Add(x);
            }
        }
        public async Task<Guid> FULTerminate()
        {
            var fulid = Guid.NewGuid();

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_followuplistid"] = fulid;
            if (Reservation.project_id != Guid.Empty)
            {
                data["bsd_project@odata.bind"] = "/bsd_projects(" + Reservation.project_id + ")";
            }

            data["bsd_group"] = 100000000;
            data["bsd_date"] = DateTime.Now.ToLocalTime();

            if (Reservation.statuscode == 3)
                data["bsd_type"] = 100000005;
            else if (Reservation.statuscode == 100000006 || Reservation.statuscode == 100000004)
                data["bsd_type"] = 100000001;
            else if (Reservation.bsd_reservationformstatus == 100000001)
                data["bsd_type"] = 100000000;
            else if (Reservation.statuscode == 100000000)
                data["bsd_type"] = 100000000;

            data["bsd_name"] = "Termination_"+ Reservation.quotenumber + "_CCR";
            data["bsd_reservation@odata.bind"] = "/quotes(" + Reservation.quoteid + ")";
            data["bsd_depositfee"] = Reservation.bsd_depositfee;
            if (Reservation.unit_id != Guid.Empty)
            {
                data["bsd_Units@odata.bind"] = "/products(" + Reservation.unit_id + ")";
            }
            if (UserLogged.Id != Guid.Empty && !UserLogged.IsLoginByUserCRM)
            {
                data["bsd_Employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty && !UserLogged.IsLoginByUserCRM)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            data["bsd_sellingprice"] = Reservation.totalamount;
            data["bsd_totalamount"] = Reservation.totalamount;
            data["bsd_totalamountpaid"] = Reservation.bsd_totalamountpaid;
            data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)"; // Don vi tien te mac dinh la "đ"

            string path = "/bsd_followuplists";
            CrmApiResponse result = await CrmHelper.PostData(path, data);
            if (result.IsSuccess)
            {
                return fulid;
            }
            else
            {
                return Guid.Empty;
            }
        }
        public async Task<bool> CheckFUL()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_followuplist'>
                                        <attribute name='bsd_followuplistid' />
                                        <order attribute='createdon' descending='true' />
                                        <filter type='and'>
                                            <condition attribute='statuscode' operator='eq' value='1' />
                                            <condition attribute='bsd_reservation' operator='eq' value='{Reservation.quoteid}' />
                                        </filter>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<FollowUpModel>>("bsd_followuplists", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                Reservation.bsd_followuplist_format = Language.khong;
                return true;// retrun true khi danh sách k có ful nào đang có hiệu lực
            }
            else
            {
                Reservation.bsd_followuplist_format = Language.co;
                return false;
            }
        }

        private async Task RefreshBTG()
        {
            LoadingHelper.Show();
            IsRefreshing = true;
            CoownerList.Clear();
            ListDiscountPaymentScheme.Clear();
            ListDiscount.Clear();
            ListPromotion.Clear();
            ListDiscountInternel.Clear();
            ListDiscountExchange.Clear();
            InstallmentList.Clear();
            ListSpecialDiscount.Clear();
            ButtonCommandList.Clear();
            ShowInstallmentList = false;
            NumberInstallment = 0;

            await Task.WhenAll(
                LoadCoOwners(this.ReservationId),
                LoadReservation(this.ReservationId),
                LoadPromotions(this.ReservationId),
                LoadSpecialDiscount(this.ReservationId),
                LoadInstallmentList(this.ReservationId)
                );
            await Task.WhenAll(
                LoadDiscounts(),
                LoadDiscountsPaymentScheme(),
                LoadDiscountsInternel(),
                LoadDiscountsExChange()
                );
            await LoadHandoverCondition(this.ReservationId);
            MessagingCenter.Send<BangTinhGiaDetailPageViewModel>(this, "IsRefresh");
            IsRefreshing = false;
            LoadingHelper.Hide();
        }
    }
}
