using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using PhuLongCRM.Config;
using PhuLongCRM.Helper;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Settings;
using Newtonsoft.Json;

namespace PhuLongCRM.ViewModels
{
    public class ReservationFormViewModel : BaseViewModel
    {
        public Guid QuoteId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ProductId { get; set; }
        public string KeywordHandoverCondition { get; set; }
        public string KeywordPromotion { get; set; }
        public List<string> SelectedPromotionIds { get; set; }
        public Guid quotedetailid { get; set; }

        private QuoteModel _quote;
        public QuoteModel Quote { get => _quote; set { _quote = value; OnPropertyChanged(nameof(Quote)); } }

        private List<string> ckChungIds { get; set; }
        private List<string> ckPTTTIds { get; set; }
        private List<string> ckNoiBoIds { get; set; }
        private List<string> ckQuyDoiIds { get; set; }

        private string _titleQuote;
        public string TitleQuote { get => _titleQuote; set { _titleQuote = value; OnPropertyChanged(nameof(TitleQuote)); } }
        private string _staffAgentQuote;
        public string StaffAgentQuote { get => _staffAgentQuote; set { _staffAgentQuote = value; OnPropertyChanged(nameof(StaffAgentQuote)); } }
        private string _descriptionQuote;
        public string DescriptionQuote { get => _descriptionQuote; set { _descriptionQuote = value; OnPropertyChanged(nameof(DescriptionQuote)); } }
        private string _waiverManaFee;
        public string WaiverManaFee { get => _waiverManaFee; set { _waiverManaFee = value; OnPropertyChanged(nameof(WaiverManaFee)); } }

        public ObservableCollection<DiscountChildOptionSet> DiscountChilds { get; set; } = new ObservableCollection<DiscountChildOptionSet>();
        public ObservableCollection<DiscountChildOptionSet> DiscountChildsInternel { get; set; } = new ObservableCollection<DiscountChildOptionSet>();
        public ObservableCollection<DiscountChildOptionSet> DiscountChildsPaymentSchemes { get; set; } = new ObservableCollection<DiscountChildOptionSet>();
        public ObservableCollection<DiscountChildOptionSet> DiscountChildsExchanges { get; set; } = new ObservableCollection<DiscountChildOptionSet>();
        public ObservableCollection<OptionSet> PromotionsSelected { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> Promotions { get; set; } = new ObservableCollection<OptionSet>();

        private List<PaymentSchemeModel> _paymentSchemes;
        public List<PaymentSchemeModel> PaymentSchemes { get => _paymentSchemes; set { _paymentSchemes = value; OnPropertyChanged(nameof(PaymentSchemes)); } }
        private List<OptionSet> _paymentSchemeTypes;
        public List<OptionSet> PaymentSchemeTypes { get => _paymentSchemeTypes; set { _paymentSchemeTypes = value; OnPropertyChanged(nameof(PaymentSchemeTypes)); } }
        private List<OptionSet> _discountLists;
        public List<OptionSet> DiscountLists { get => _discountLists; set { _discountLists = value; OnPropertyChanged(nameof(DiscountLists)); } }
        private List<OptionSet> _discountInternelLists;
        public List<OptionSet> DiscountInternelLists { get => _discountInternelLists; set { _discountInternelLists = value; OnPropertyChanged(nameof(DiscountInternelLists)); } }
        private List<OptionSet> _discountExchangeLists;
        public List<OptionSet> DiscountExchangeLists { get => _discountExchangeLists; set { _discountExchangeLists = value; OnPropertyChanged(nameof(DiscountExchangeLists)); } }
        private List<HandoverConditionModel> _handoverConditions;
        public List<HandoverConditionModel> HandoverConditions { get => _handoverConditions; set { _handoverConditions = value; OnPropertyChanged(nameof(HandoverConditions)); } }

        private List<LookUp> _listCollaborator;
        public List<LookUp> ListCollaborator { get => _listCollaborator; set { _listCollaborator = value; OnPropertyChanged(nameof(ListCollaborator)); } }
        private List<LookUp> _listCustomerReferral;
        public List<LookUp> ListCustomerReferral { get => _listCustomerReferral; set { _listCustomerReferral = value; OnPropertyChanged(nameof(ListCustomerReferral)); } }

        private LookUp _collaborator;
        public LookUp Collaborator { get => _collaborator; set { _collaborator = value; OnPropertyChanged(nameof(Collaborator)); } }
        private LookUp _customerReferral;
        public LookUp CustomerReferral { get => _customerReferral; set { _customerReferral = value; OnPropertyChanged(nameof(CustomerReferral)); } }

        public PaymentSchemeModel paymentSheme_Temp { get; set; }
        private PaymentSchemeModel _paymentScheme;
        public PaymentSchemeModel PaymentScheme { get => _paymentScheme; set { _paymentScheme = value; OnPropertyChanged(nameof(PaymentScheme)); } }
        private OptionSet _paymentSchemeType;
        public OptionSet PaymentSchemeType { get => _paymentSchemeType; set { _paymentSchemeType = value; OnPropertyChanged(nameof(PaymentSchemeType)); } }
        private OptionSet _discountList;
        public OptionSet DiscountList { get => _discountList; set { _discountList = value; OnPropertyChanged(nameof(DiscountList)); } }
        private OptionSet _discountInternelList;
        public OptionSet DiscountInternelList { get => _discountInternelList; set { _discountInternelList = value; OnPropertyChanged(nameof(DiscountInternelList)); } }
        private OptionSet _discountExchangeList;
        public OptionSet DiscountExchangeList { get => _discountExchangeList; set { _discountExchangeList = value; OnPropertyChanged(nameof(DiscountExchangeList)); } }
        private HandoverConditionModel _handoverCondition;
        public HandoverConditionModel HandoverCondition { get => _handoverCondition; set { _handoverCondition = value; OnPropertyChanged(nameof(HandoverCondition)); } }
        public HandoverConditionModel HandoverCondition_Update { get; set; }

        private QuoteUnitInforModel _unitInfor;
        public QuoteUnitInforModel UnitInfor { get => _unitInfor; set { _unitInfor = value; OnPropertyChanged(nameof(UnitInfor)); } }

        private StatusCodeModel _statusUnit;
        public StatusCodeModel StatusUnit { get => _statusUnit; set { _statusUnit = value; OnPropertyChanged(nameof(StatusUnit)); } }

        private OptionSet _priceListApply;
        public OptionSet PriceListApply { get => _priceListApply; set { _priceListApply = value; OnPropertyChanged(nameof(PriceListApply)); } }

        #region CoOwner
        public ObservableCollection<CoOwnerFormModel> CoOwnerList { get; set; } = new ObservableCollection<CoOwnerFormModel>();

        private CoOwnerFormModel _coOwner;
        public CoOwnerFormModel CoOwner { get => _coOwner; set { _coOwner = value; OnPropertyChanged(nameof(CoOwner)); } }

        private string _titleCoOwner;
        public string TitleCoOwner { get => _titleCoOwner; set { _titleCoOwner = value; OnPropertyChanged(nameof(TitleCoOwner)); } }

        private List<OptionSet> _relationships;
        public List<OptionSet> Relationships { get => _relationships; set { _relationships = value; OnPropertyChanged(nameof(Relationships)); } }

        private OptionSet _relationship;
        public OptionSet Relationship { get => _relationship; set { _relationship = value; OnPropertyChanged(nameof(Relationship)); } }

        private OptionSet _customerCoOwner;
        public OptionSet CustomerCoOwner { get => _customerCoOwner; set { _customerCoOwner = value; OnPropertyChanged(nameof(CustomerCoOwner)); } }
        #endregion

        #region Thong tin ban hang
        private List<OptionSet> _queues;
        public List<OptionSet> Queues { get => _queues; set { _queues = value; OnPropertyChanged(nameof(Queues)); } }

        private OptionSet _queue;
        public OptionSet Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }

        private OptionSet _buyer;
        public OptionSet Buyer { get => _buyer; set { _buyer = value; OnPropertyChanged(nameof(Buyer)); } }
        #endregion

        #region Chi tiet
        private List<OptionSet> _contractTypes;
        public List<OptionSet> ContractTypes { get => _contractTypes; set { _contractTypes = value; OnPropertyChanged(nameof(ContractTypes)); } }

        private OptionSet _contractType;
        public OptionSet ContractType { get => _contractType; set { _contractType = value; OnPropertyChanged(nameof(ContractType)); } }

        private TaxCodeModel _taxCode;
        public TaxCodeModel TaxCode { get => _taxCode; set { _taxCode = value; OnPropertyChanged(nameof(TaxCode)); } }
        #endregion

        #region Thong tin bao gia
        private List<OptionSet> _salesAgents;
        public List<OptionSet> SalesAgents { get => _salesAgents; set { _salesAgents = value; OnPropertyChanged(nameof(SalesAgents)); } }

        private OptionSet _salesAgent;
        public OptionSet SalesAgent { get => _salesAgent; set { _salesAgent = value; OnPropertyChanged(nameof(SalesAgent)); } }
        #endregion

        private TotalReservationModel _totalReservation;
        public TotalReservationModel TotalReservation { get => _totalReservation; set { _totalReservation = value; OnPropertyChanged(nameof(TotalReservation)); } }

        private PhasesLanchModel _phasesLanchModel;
        public PhasesLanchModel PhasesLanchModel { get => _phasesLanchModel; set { _phasesLanchModel = value; OnPropertyChanged(nameof(PhasesLanchModel)); } }

        public OptionSet QuoteDetail { get; set; }
        private Guid PhasesLaunchId { get; set; }
        public Guid UnitType { get; set; }

        public bool IsHadLichThanhToan { get; set; }

        public bool IsLocked { get; set; }
        public bool IsExpiredDiscount { get; set; }
        public bool IsExpiredInternel { get; set; }
        public bool IsExpiredExchange { get; set; }

        public ReservationFormViewModel()
        {
            SelectedPromotionIds = new List<string>();
            this.Quote = new QuoteModel();

            ListCollaborator = new List<LookUp>();
            ListCustomerReferral = new List<LookUp>();
            SalesAgents = new List<OptionSet>();
            PaymentSchemes = new List<PaymentSchemeModel>();
            HandoverConditions = new List<HandoverConditionModel>();
        }

        public async Task CheckTaoLichThanhToan()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentschemedetail'>
                                    <attribute name='bsd_paymentschemedetailid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_reservation' operator='in'>
                                        <value uitype='quote'>{this.QuoteId}</value>\
                                      </condition>
                                      <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_paymentschemedetails", fetchXml);
            if (result == null) return;
            this.IsHadLichThanhToan = result.value.Count != 0 ? true : false;
        }

        //Load thong tin Quote
        public async Task LoadQuote()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' />
                                    <attribute name='bsd_depositfee' />
                                    <attribute name='bsd_bookingfee' />
                                    <attribute name='bsd_nameofstaffagent' />
                                    <attribute name='bsd_referral' />
                                    <attribute name='bsd_detailamount' />
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_managementfee' />
                                    <attribute name='bsd_discount' />
                                    <attribute name='bsd_packagesellingamount' />
                                    <attribute name='bsd_totalamountlessfreight' />
                                    <attribute name='bsd_landvaluededuction' />
                                    <attribute name='totaltax' />
                                    <attribute name='bsd_freightamount' />
                                    <attribute name='bsd_netsellingpriceaftervat' />
                                    <attribute name='totalamount' />
                                    <attribute name='quoteid' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_actualarea' />
                                    <attribute name='bsd_netusablearea' />
                                    <attribute name='bsd_unitstatus' />
                                    <attribute name='bsd_paymentschemestype' />
                                    <attribute name='bsd_startingdatecalculateofps' />
                                    <attribute name='bsd_discounts' />
                                    <attribute name='bsd_interneldiscount' />
                                    <attribute name='bsd_selectedchietkhaupttt' />
                                    <attribute name='bsd_exchangediscount' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='quoteid' operator='eq' uitype='quote' value='{this.QuoteId}' />
                                    </filter>
                                    <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' visible='false' link-type='outer' alias='a_134b4ac419dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='paymentscheme_name'/>
                                        <attribute name='bsd_paymentschemeid' alias='paymentscheme_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' visible='false' link-type='outer' alias='a_de5241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='discountlist_name'/>
                                        <attribute name='bsd_discounttypeid' alias='discountlist_id'/>
                                    </link-entity>
                                    <link-entity name='opportunity' from='opportunityid' to='opportunityid' visible='false' link-type='outer' alias='a_9de364c890d1eb11bacc000d3a80021e'>
                                        <attribute name='name' alias='queue_name'/>
                                        <attribute name='opportunityid' alias='queue_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_534f5ec290d1eb11bacc000d3a80021e'>
                                        <attribute name='bsd_name' alias='account_name'/>
                                        <attribute name='accountid' alias='account_id'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_984f5ec290d1eb11bacc000d3a80021e'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id' />
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='a_d52436e819dbeb11bacb002248168cad'>
                                        <attribute name='productid' alias='unit_id'/>
                                        <attribute name='name' alias='unit_name'/>
                                        <attribute name='price' alias='unit_price'/>
                                        <attribute name='bsd_landvalueofunit' />
                                        <attribute name='bsd_maintenancefeespercent' alias='maintenancefreespercent'/>
                                        <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                        <attribute name='bsd_projectcode' alias='_bsd_projectcode_value'/>
                                        <attribute name='bsd_phaseslaunchid' alias='_bsd_phaseslaunchid_value'/>
                                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_9a5e44d019dbeb11bacb002248168cad'>
                                          <attribute name='bsd_name' alias='project_name'/>
                                          <attribute name='bsd_projectid' alias='project_id'/>
                                        </link-entity>
                                        <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_645347ca19dbeb11bacb002248168cad'>
                                            <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' visible='false' link-type='outer' alias='a_cd5064ce90d1eb11bacc000d3a80021e'>
                                        <attribute name='name' alias='pricelist_apply_name'/>
                                        <attribute name='pricelevelid' alias='pricelist_apply_id'/>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistphaselaunch' visible='false' link-type='outer' alias='a_377c3be219dbeb11bacb002248168cad'>
                                        <attribute name='name' alias='pricelist_phaselaunch_name'/>
                                        <attribute name='pricelevelid' alias='pricelist_phaselaunch_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.Quote = result.value.SingleOrDefault();

            string fetchXml2 = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='quoteid' operator='eq' uitype='quote' value='{this.QuoteId}' />
                                    </filter>
                                    <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' visible='false' link-type='outer' alias='a_225f44d019dbeb11bacb002248168cad'>
                                        <attribute name='bsd_value' alias='tax_value'/>
                                        <attribute name='bsd_taxcodeid' alias='tax_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_salessgentcompany' visible='false' link-type='outer' alias='a_c4034cb219dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='saleagentcompany_name'/>
                                        <attribute name='accountid' alias='saleagentcompany_id'/>
                                    </link-entity>
                                    <link-entity name='quotedetail' from='quoteid' to='quoteid' link-type='outer' alias='af' >
                                        <attribute name='quotedetailid' alias='quotedetail_id' />
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
                                    <link-entity name='bsd_interneldiscount' from='bsd_interneldiscountid' to='bsd_interneldiscountlist' visible='false' link-type='outer' alias='a_c014fc37ba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_name' alias='interneldiscount_name'/>
                                        <attribute name='bsd_interneldiscountid' alias='interneldiscount_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discountpromotion' from='bsd_discountpromotionid' to='bsd_exchangediscountlist' visible='false' link-type='outer' alias='a_2e80b433b075eb11a812000d3ac8b5f4'>
                                        <attribute name='bsd_name' alias='discountpromotion_name'/>
                                        <attribute name='bsd_discountpromotionid' alias='discountpromotion_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteModel>>("quotes", fetchXml2);
            if (result2 == null || result2.value.Any() == false) return;

            var data = result2.value.SingleOrDefault();
            this.quotedetailid = data.quotedetail_id;
            this.Quote.tax_id = data.tax_id;
            this.Quote.tax_value = data.tax_value;
            this.Quote.saleagentcompany_id = data.saleagentcompany_id;
            this.Quote.saleagentcompany_name = data.saleagentcompany_name;
            this.Quote.collaborator_id = data.collaborator_id;
            this.Quote.collaborator_name = data.collaborator_name;
            this.Quote.customerreferral_contact_id = data.customerreferral_contact_id;
            this.Quote.customerreferral_contact_name = data.customerreferral_contact_name;
            this.Quote.customerreferral_account_id = data.customerreferral_account_id;
            this.Quote.customerreferral_account_name = data.customerreferral_account_name;
            this.Quote.interneldiscount_id = data.interneldiscount_id;
            this.Quote.interneldiscount_name = data.interneldiscount_name;
            this.Quote.discountpromotion_id = data.discountpromotion_id;
            this.Quote.discountpromotion_name = data.discountpromotion_name;
            this.Quote.bsd_constructionarea_format = StringFormatHelper.FormatPercent(Quote.bsd_constructionarea);
            this.Quote.bsd_netusablearea_format = StringFormatHelper.FormatPercent(Quote.bsd_netusablearea);
            this.Quote.bsd_actualarea_format = StringFormatHelper.FormatPercent(Quote.bsd_actualarea);
            this.Quote.bsd_bookingfee_format = StringFormatHelper.FormatCurrency(Quote.bsd_bookingfee);
            this.Quote.bsd_depositfee_format = StringFormatHelper.FormatCurrency(Quote.bsd_depositfee);
            this.Quote.bsd_managementfee_format = StringFormatHelper.FormatCurrency(Quote.bsd_managementfee);

            this.Buyer = this.Quote.contact_id != Guid.Empty ? new OptionSet(this.Quote.contact_id.ToString(), this.Quote.contact_name) { Title = "2" } : new OptionSet(this.Quote.account_id.ToString(), this.Quote.account_name) { Title = "3" };
            this.Queue = this.Quote.queue_id != Guid.Empty ? new OptionSet(this.Quote.queue_id.ToString(), this.Quote.queue_name) : null;
            this.ContractType = ContractTypeData.GetContractTypeById(this.Quote.bsd_contracttypedescripton);
            this.StatusUnit = StatusCodeUnit.GetStatusCodeById(this.Quote.bsd_unitstatus);
            this.PriceListApply = new OptionSet(this.Quote.pricelist_apply_id.ToString(), this.Quote.pricelist_apply_name);
            this.TaxCode = new TaxCodeModel() { bsd_taxcodeid = this.Quote.tax_id, bsd_value = this.Quote.tax_value };

            if (this.Quote.saleagentcompany_id != Guid.Empty)
            {
                this.SalesAgent = new OptionSet(this.Quote.saleagentcompany_id.ToString(), this.Quote.saleagentcompany_name);
            }
            if (!string.IsNullOrWhiteSpace(this.Quote.collaborator_id))
            {
                this.Collaborator = new LookUp() { Id = Guid.Parse(this.Quote.collaborator_id), Name = this.Quote.collaborator_name };
            }
            if (!string.IsNullOrWhiteSpace(this.Quote.customerreferral_account_id))
            {
                this.CustomerReferral = new LookUp() { Id = Guid.Parse(this.Quote.customerreferral_account_id), Name = this.Quote.customerreferral_account_name };
            }
            else if (!string.IsNullOrWhiteSpace(this.Quote.customerreferral_contact_id))
            {
                this.CustomerReferral = new LookUp() { Id = Guid.Parse(this.Quote.customerreferral_contact_id), Name = this.Quote.customerreferral_contact_name };
            }
            if (!string.IsNullOrWhiteSpace(Quote.interneldiscount_id))
            {
                this.DiscountInternelList = new OptionSet(this.Quote.interneldiscount_id, this.Quote.interneldiscount_name);
            }
            if (!string.IsNullOrWhiteSpace(Quote.discountpromotion_id))
            {
                this.DiscountExchangeList = new OptionSet(this.Quote.discountpromotion_id, this.Quote.discountpromotion_name);
            }

            this.paymentSheme_Temp = this.PaymentScheme = new PaymentSchemeModel() { bsd_paymentschemeid = this.Quote.paymentscheme_id, bsd_name = this.Quote.paymentscheme_name };
            this.DiscountList = this.Quote.discountlist_id != Guid.Empty ? new OptionSet(this.Quote.discountlist_id.ToString(), this.Quote.discountlist_name) : null;
            this.PhasesLaunchId = this.Quote._bsd_phaseslaunchid_value;
            this.UnitType = this.Quote._bsd_unittype_value;

            this.TotalReservation = new TotalReservationModel();
            this.TotalReservation.ListedPrice = this.Quote.bsd_detailamount;
            this.TotalReservation.Discount = this.Quote.bsd_discount;
            this.TotalReservation.HandoverAmount = this.Quote.bsd_packagesellingamount;
            this.TotalReservation.NetSellingPrice = this.Quote.bsd_totalamountlessfreight;
            this.TotalReservation.LandValue = this.Quote.bsd_landvaluededuction;
            this.TotalReservation.TotalTax = this.Quote.totaltax;
            this.TotalReservation.MaintenanceFee = this.Quote.bsd_freightamount;
            this.TotalReservation.NetSellingPriceAfterVAT = this.Quote.bsd_netsellingpriceaftervat;
            this.TotalReservation.TotalAmount = this.Quote.totalamount;
            this.TotalReservation.ListedPrice_format = StringFormatHelper.FormatCurrency(TotalReservation.ListedPrice);
            this.TotalReservation.Discount_format = StringFormatHelper.FormatCurrency(TotalReservation.Discount);
            this.TotalReservation.HandoverAmount_format = StringFormatHelper.FormatCurrency(TotalReservation.HandoverAmount);
            this.TotalReservation.NetSellingPrice_format = StringFormatHelper.FormatCurrency(TotalReservation.NetSellingPrice);
            this.TotalReservation.LandValue_format = StringFormatHelper.FormatCurrency(TotalReservation.LandValue);
            this.TotalReservation.TotalTax_format = StringFormatHelper.FormatCurrency(TotalReservation.TotalTax);
            this.TotalReservation.MaintenanceFee_format = StringFormatHelper.FormatCurrency(TotalReservation.MaintenanceFee);
            this.TotalReservation.NetSellingPriceAfterVAT_format = StringFormatHelper.FormatCurrency(TotalReservation.NetSellingPriceAfterVAT);
            this.TotalReservation.TotalAmount_format = StringFormatHelper.FormatCurrency(TotalReservation.TotalAmount);
        }

        // Tinh tien
        public async Task<CrmApiResponse> GetTotal(string quoteId)
        {
            CalualteReservationModel model = new CalualteReservationModel();
            model.DKBG = this.HandoverCondition.Val;

            ckChungIds = new List<string>();
            ckPTTTIds = new List<string>();
            ckNoiBoIds = new List<string>();
            ckQuyDoiIds = new List<string>();

            foreach (var item in DiscountChilds)
            {
                if (item.Selected == true)
                {
                    ckChungIds.Add(item.Val);
                }
            }
            foreach (var item in DiscountChildsPaymentSchemes)
            {
                if (item.Selected == true)
                {
                    ckPTTTIds.Add(item.Val);
                }
            }
            foreach (var item in DiscountChildsInternel)
            {
                if (item.Selected == true)
                {
                    ckNoiBoIds.Add(item.Val);
                }
            }
            foreach (var item in DiscountChildsExchanges)
            {
                if (item.Selected == true)
                {
                    ckQuyDoiIds.Add(item.Val);
                }
            }
            model.CKChung = ckChungIds.Count > 0 ? string.Join(",", ckChungIds) : null;
            model.CKPTTT = ckPTTTIds.Count > 0 ? string.Join(",", ckPTTTIds) : null;
            model.CKNoiBo = ckNoiBoIds.Count > 0 ? string.Join(",", ckNoiBoIds) : null;
            model.CKQuyDoi = ckQuyDoiIds.Count > 0 ? string.Join(",", ckQuyDoiIds) : null;

            string path = $"/quotes({quoteId})//Microsoft.Dynamics.CRM.bsd_Action_CalculateReservation_ForApp";

            string json = JsonConvert.SerializeObject(model);
            var input = new
            {
                input = json
            };
            string body = JsonConvert.SerializeObject(input);
            CrmApiResponse result = await CrmHelper.PostData(path, body);
            if (result.IsSuccess == true)
            {
                string content = result.Content;
                ResponseAction responseActions = JsonConvert.DeserializeObject<ResponseAction>(content);
                if (responseActions.output != null)
                {
                    result.Content = responseActions.output;
                }

            }
            return result;
        }

        // Load thong tin san pham
        public async Task LoadUnitInfor()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_netsaleablearea' />
                                    <attribute name='bsd_actualarea' />
                                    <attribute name='bsd_projectcode' alias='_bsd_projectcode_value'/>
                                    <attribute name='bsd_phaseslaunchid' alias='_bsd_phaseslaunchid_value'/>
                                    <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                    <attribute name='bsd_taxpercent' />
                                    <attribute name='bsd_queuingfee' />
                                    <attribute name='bsd_depositamount' />
                                    <attribute name='price' />
                                    <attribute name='bsd_landvalueofunit' />
                                    <attribute name='bsd_maintenancefeespercent' />
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_managementamountmonth' />
                                    <attribute name='productid' />
                                    <attribute name='defaultuomid' alias='_defaultuomid_value'/>
                                    <order attribute='bsd_constructionarea' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='productid' operator='eq' uitype='product' value='{ProductId}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_9a5e44d019dbeb11bacb002248168cad'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                      <attribute name='bsd_projectid' alias='project_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_645347ca19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                        <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistid' visible='false' link-type='outer' alias='a_f07b3be219dbeb11bacb002248168cad'>
                                          <attribute name='name' alias='pricelist_name_phaseslaunch'/>
                                          <attribute name='pricelevelid' alias='pricelist_id_phaseslaunch'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' visible='false' link-type='outer' alias='a_af3c62ff7dd1eb11bacc000d3a80021e'>
                                      <attribute name='name' alias='pricelist_name_unit'/>
                                      <attribute name='pricelevelid' alias='pricelist_id_unit'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteUnitInforModel>>("products", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.UnitInfor = result.value.FirstOrDefault();
            this.StatusUnit = StatusCodeUnit.GetStatusCodeById(UnitInfor.statuscode);

            this.PriceListApply = new OptionSet(UnitInfor.pricelist_id_unit.ToString(), UnitInfor.pricelist_name_unit);

            this.Quote.unit_id = UnitInfor.productid;
            this.Quote.name = this.Quote.unit_name = UnitInfor.name;
            this.Quote.bsd_constructionarea = UnitInfor.bsd_constructionarea;
            this.Quote.bsd_constructionarea_format = StringFormatHelper.FormatPercent(UnitInfor.bsd_constructionarea);
            this.Quote.bsd_netusablearea = UnitInfor.bsd_netsaleablearea;
            this.Quote.bsd_netusablearea_format = StringFormatHelper.FormatPercent(UnitInfor.bsd_netsaleablearea);
            this.Quote.bsd_actualarea = UnitInfor.bsd_actualarea;
            this.Quote.bsd_actualarea_format = StringFormatHelper.FormatPercent(UnitInfor.bsd_actualarea);
            this.Quote._bsd_projectcode_value = UnitInfor._bsd_projectcode_value;
            this.Quote.project_name = UnitInfor.project_name;
            this.Quote.project_id = UnitInfor.project_id;
            this.Quote.bsd_bookingfee = UnitInfor.bsd_queuingfee;
            this.Quote.bsd_bookingfee_format = StringFormatHelper.FormatCurrency(UnitInfor.bsd_queuingfee);
            this.Quote.bsd_depositfee = UnitInfor.bsd_depositamount;
            this.Quote.bsd_depositfee_format = StringFormatHelper.FormatCurrency(UnitInfor.bsd_depositamount);
            this.Quote._bsd_phaseslaunchid_value = UnitInfor._bsd_phaseslaunchid_value;
            this.Quote.phaseslaunch_name = UnitInfor.phaseslaunch_name;
            this.Quote.bsd_detailamount = UnitInfor.price;
            this.Quote.bsd_numberofmonthspaidmf = UnitInfor.bsd_numberofmonthspaidmf;
            this.Quote.bsd_unitstatus = UnitInfor.statuscode;
            this.Quote.pricelist_apply_id = Guid.Parse(PriceListApply.Val);
            this.Quote.bsd_managementfee = this.UnitInfor.bsd_managementamountmonth * this.UnitInfor.bsd_netsaleablearea * this.UnitInfor.bsd_numberofmonthspaidmf * (decimal)1.1;
            this.Quote.bsd_managementfee_format = StringFormatHelper.FormatCurrency(Quote.bsd_managementfee);
            this.UnitType = UnitInfor._bsd_unittype_value;
            this.PhasesLaunchId = this.UnitInfor._bsd_phaseslaunchid_value;
        }

        public async Task LoadPhasesLaunch()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_phaseslaunchid' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaseslaunchid' operator='eq' value='{this.Quote._bsd_phaseslaunchid_value}' />
                                    </filter>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' visible='false' link-type='outer' alias='a_182aff31ba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='discount_name'/>
                                       <attribute name='bsd_discounttypeid' alias='discount_id'/>
                                      <attribute name='bsd_startdate' alias='startdate_discountlist'/>
                                      <attribute name='bsd_enddate' alias='enddate_discountlist'/>
                                    </link-entity>
                                    <link-entity name='bsd_interneldiscount' from='bsd_interneldiscountid' to='bsd_internaldiscountlist' visible='false' link-type='outer' alias='a_7514fc37ba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='internel_name'/>
                                      <attribute name='bsd_interneldiscountid' alias='internel_id'/>
                                      <attribute name='bsd_startdate' alias='startdate_internellist'/>
                                      <attribute name='bsd_enddate' alias='enddate_internellist'/>
                                    </link-entity>
                                    <link-entity name='bsd_discountonpaymentscheme' from='bsd_discountonpaymentschemeid' to='bsd_discountonpaymentscheme' visible='false' link-type='outer' alias='a_e829ff31ba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='paymentscheme_name'/>
                                      <attribute name='bsd_discountonpaymentschemeid' alias='paymentscheme_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discountpromotion' from='bsd_discountpromotionid' to='bsd_promotiondiscountlist' visible='false' link-type='outer' alias='a_feadc62e2b23eb11a813000d3a07be14'>
                                      <attribute name='bsd_name' alias='promotion_name'/>
                                      <attribute name='bsd_discountpromotionid' alias='promotion_id' />
                                      <attribute name='bsd_startdate' alias='startdate_exchangelist'/>
                                      <attribute name='bsd_enddate' alias='enddate_exchangelist'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLanchModel>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            try
            {
                this.PhasesLanchModel = result.value.SingleOrDefault();
                if (PhasesLanchModel.discount_id != Guid.Empty)
                {
                    if (PhasesLanchModel.startdate_discountlist.Date.ToLocalTime() <= DateTime.Now.Date && PhasesLanchModel.enddate_discountlist.Date.ToLocalTime() >= DateTime.Now.Date)
                    {
                        this.DiscountList = new OptionSet() { Val = PhasesLanchModel.discount_id.ToString(), Label = PhasesLanchModel.discount_name };
                        this.IsExpiredDiscount = false;
                    }
                    else this.IsExpiredDiscount = true;
                    await LoadDiscountChilds();
                }
                if (PhasesLanchModel.internel_id != Guid.Empty)
                {
                    if (PhasesLanchModel.startdate_internellist.Date.ToLocalTime() <= DateTime.Now.Date && PhasesLanchModel.enddate_internellist.Date.ToLocalTime() >= DateTime.Now.Date)
                    {
                        this.DiscountInternelList = new OptionSet() { Val = PhasesLanchModel.internel_id.ToString(), Label = PhasesLanchModel.internel_name };
                        this.IsExpiredInternel = false;
                    }
                    else this.IsExpiredInternel = true;
                    await LoadDiscountChildsInternel();
                }
                if (PhasesLanchModel.promotion_id != Guid.Empty)
                {
                    if (PhasesLanchModel.startdate_exchangelist.Date.ToLocalTime() <= DateTime.Now.Date && PhasesLanchModel.enddate_exchangelist.Date.ToLocalTime() >= DateTime.Now.Date)
                    {
                        this.DiscountExchangeList = new OptionSet() { Val = PhasesLanchModel.promotion_id.ToString(), Label = PhasesLanchModel.promotion_name };
                        this.IsExpiredExchange = false;
                    }
                    else this.IsExpiredExchange = true;

                    await LoadDiscountChildsExchange();
                }
            }
            catch(Exception ex)
            {

            }
            

        }

        // Load tax code
        public async Task LoadTaxCode()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_taxcode'>
                                    <attribute name='bsd_taxcodeid'/>
                                    <attribute name='bsd_value'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_default' operator='eq' value='1' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaxCodeModel>>("bsd_taxcodes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.TaxCode = result.value.SingleOrDefault();
        }

        // load phuong thuc thanh toan vs status code = confirm va theo du an
        public async Task LoadPaymentSchemes()
        {
            Guid unitId = this.QuoteId != Guid.Empty ? this.Quote._bsd_projectcode_value : this.UnitInfor._bsd_projectcode_value;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentscheme'>
                                    <attribute name='bsd_name'/>
                                    <attribute name='bsd_paymentschemeid'/>
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                      <condition attribute='bsd_phaseslaunch' operator='eq' uitype='bsd_phaseslaunch' value='{this.PhasesLaunchId}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PaymentSchemeModel>>("bsd_paymentschemes", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                if (item.bsd_startdate.Date.ToLocalTime() <= DateTime.Now.Date && item.bsd_enddate.Date.ToLocalTime() >= DateTime.Now.Date)
                {
                    this.PaymentSchemes.Add(item);
                }
            }
        }

        // Load Dieu kien ban giao
        public async Task LoadHandoverConditions()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_packageselling'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                    <attribute name='bsd_byunittype' />
                                    <attribute name='bsd_packagesellingid' alias='Val'/>
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_priceperm2' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percent' />
                                    <attribute name='bsd_startdate' alias='startdate' />
                                    <attribute name='bsd_enddate' alias='enddate'/>
                                    <order attribute='bsd_name' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='bsd_enddate' operator='not-null' />
                                        <condition attribute='bsd_startdate' operator='not-null' />
                                        <condition attribute='statuscode' operator='eq' value='100000000' />
                                        <condition attribute='bsd_phaselaunch' operator='eq' uitype='bsd_phaseslaunch' value='{PhasesLaunchId}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HandoverConditionModel>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                if (item.startdate.Date.ToLocalTime() <= DateTime.Now.Date && item.enddate.Date.ToLocalTime() >= DateTime.Now.ToLocalTime())
                {
                    this.HandoverConditions.Add(item);
                }
            }
        }

        // Load handover condition khi cap nha
        public async Task LoadHandoverCondition()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_packageselling'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                    <attribute name='bsd_byunittype' />
                                    <attribute name='bsd_packagesellingid' alias='Val'/>
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_priceperm2' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percent' />
                                    <order attribute='bsd_name' descending='true' />
                                    <link-entity name='bsd_quote_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                                            <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + this.QuoteId + @"' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HandoverConditionModel>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.HandoverCondition = this.HandoverCondition_Update = result.value.SingleOrDefault();
        }

        // Load Chieu khau
        public async Task LoadDiscountList()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_phaseslaunchid' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaseslaunchid' operator='eq' value='{PhasesLaunchId}'/>
                                    </filter>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' visible='false' link-type='outer' alias='a_d55241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='Label'/>
                                        <attribute name='bsd_discounttypeid' alias='Val'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.DiscountLists = result.value;
        }

        // Load Chieu khau quy doi
        public async Task LoadDiscountExchangeList()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discountpromotion'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_discountpromotionid'  alias='Val'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaselaunch' operator='eq' value='{PhasesLaunchId}'/>
                                      <condition attribute='statuscode' operator='eq' value='100000001' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_discountpromotions", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.DiscountExchangeLists = result.value;
        }

        public async Task LoadDiscountChildsExchange()
        {
            // new_type -> loai cua discounts (precent:100000000 or amount:100000001)
            List<DiscountChildOptionSet> data = new List<DiscountChildOptionSet>();
            var result = new RetrieveMultipleApiResponse<DiscountChildOptionSet>();
            if (this.IsExpiredExchange == false && this.DiscountExchangeList != null)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='bsd_bsd_discount_bsd_discountpromotion' from='bsd_discountid' to='bsd_discountid' intersect='true'>
                                      <filter>
                                        <condition attribute='bsd_discountpromotionid' operator='eq' value='{this.DiscountExchangeList.Val}' uitype='bsd_bsd_discount_bsd_discountpromotion' />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
                result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
                if (result != null && result.value.Any() == true)
                {
                    data.AddRange(result.value);
                }
            }

            string fetchXml2 = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_special' operator='eq' value='1'/>
                                      <condition attribute='bsd_discounttype' operator='eq' value='100000006'/>
                                      <condition attribute='statuscode' operator='eq' value='100000000'/>
                                      <condition attribute='bsd_phaseslaunch' operator='eq' value='{this.PhasesLaunchId}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml2);
            if (result2 != null && result2.value.Any() == true)
            {
                data.AddRange(result2.value);
            }
            if (data.Count() < 0) return;

            foreach (var item in data)
            {
                item.IsEnableChecked = (this.IsHadLichThanhToan == true || item.IsExpired == true || item.IsNotApplied == true) ? false : true;
                this.DiscountChildsExchanges.Add(item);
            }
        }

        // Load Chieu khau noi bo
        public async Task LoadDiscountInternelList()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_interneldiscount'>
                                    <attribute name='bsd_name' alias='Label' />
                                    <attribute name='bsd_interneldiscountid' alias='Val'/>
                                    <filter type='and'>
                                      <condition attribute='bsd_phaselaunch' operator='eq'  uitype='bsd_phaseslaunch' value='{PhasesLaunchId}' />
                                      <condition attribute='statuscode' operator='eq' value='100000001' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_interneldiscounts", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.DiscountInternelLists = result.value;
        }

        // Load Chieu khau con
        public async Task LoadDiscountChilds()
        {
            // new_type -> loai cua discounts (precent:100000000 or amount:100000001)
            List<DiscountChildOptionSet> data = new List<DiscountChildOptionSet>();
            var result = new RetrieveMultipleApiResponse<DiscountChildOptionSet>();
            if (this.IsExpiredDiscount == false && this.DiscountList != null)
            {
                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='bsd_bsd_discounttype_bsd_discount' from='bsd_discountid' to='bsd_discountid' visible='false' intersect='true'>
                                      <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discounttypeid' alias='ab'>
                                        <filter type='and'>
                                          <condition attribute='bsd_discounttypeid' operator='eq' uitype='bsd_discounttype' value='" + this.DiscountList.Val + @"' />
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
                result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
                if (result != null && result.value.Any() == true)
                {
                    data.AddRange(result.value);
                }
            }
            
            string fetchXml2 = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_special' operator='eq' value='1'/>
                                      <condition attribute='bsd_discounttype' operator='eq' value='100000000'/>
                                      <condition attribute='statuscode' operator='eq' value='100000000'/>
                                      <condition attribute='bsd_phaseslaunch' operator='eq' value='{this.PhasesLaunchId}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml2);
            if (result2 != null || result2.value.Any() == true)
            {
                data.AddRange(result2.value);
            }
            if (data.Count() < 0) return;

            foreach (var item in data)
            {
                item.IsEnableChecked = (this.IsHadLichThanhToan == true || item.IsExpired == true || item.IsNotApplied == true) ? false : true;
                this.DiscountChilds.Add(item);
            }
        }

        // Load Chieu khau con
        public async Task LoadDiscountChildsInternel()
        {
            // new_type -> loai cua discounts (precent:100000000 or amount:100000001)
            List<DiscountChildOptionSet> data = new List<DiscountChildOptionSet>();
            var result = new RetrieveMultipleApiResponse<DiscountChildOptionSet>();
            if (this.IsExpiredInternel == false && this.DiscountInternelList != null)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='bsd_bsd_interneldiscount_bsd_discount' from='bsd_discountid' to='bsd_discountid' intersect='true'>
                                      <filter>
                                        <condition attribute='bsd_interneldiscountid' operator='eq' value='{this.DiscountInternelList.Val}' uitype='bsd_bsd_interneldiscount_bsd_discount' />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
                result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
                if (result != null && result.value.Any() == true)
                {
                    data.AddRange(result.value);
                }
            }
            
            string fetchXml2 = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_special' operator='eq' value='1'/>
                                      <condition attribute='bsd_discounttype' operator='eq' value='100000004'/>
                                      <condition attribute='statuscode' operator='eq' value='100000000'/>
                                      <condition attribute='bsd_phaseslaunch' operator='eq' value='{this.PhasesLaunchId}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml2);
            if (result2 != null && result2.value.Any() == true)
            {
                data.AddRange(result2.value);
            }
            if (data.Count() < 0) return;
            
            foreach (var item in data)
            {
                item.IsEnableChecked = (this.IsHadLichThanhToan == true || item.IsExpired == true || item.IsNotApplied == true) ? false : true;
                this.DiscountChildsInternel.Add(item);
            }
        }

        // Get Id discount payment scheme list
        public async Task<Guid> GetDiscountPamentSchemeListId(string paymentSchemeId)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                  <entity name='bsd_discountonpaymentscheme'>
                                    <attribute name='bsd_discountonpaymentschemeid' alias='Val'/>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_discountonpaymentscheme' to='bsd_discountonpaymentschemeid' link-type='inner' alias='ad'>
                                      <link-entity name='bsd_paymentscheme' from='bsd_phaseslaunch' to='bsd_phaseslaunchid' link-type='inner' alias='ae'>
                                        <filter type='and'>
                                          <condition attribute='bsd_paymentschemeid' operator='eq'  value='{paymentSchemeId}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_discountonpaymentschemes", fetchXml);
            if (result == null || result.value.Any() == false) return Guid.Empty;

            return Guid.Parse(result.value.SingleOrDefault().Val);
        }

        // Load CK PTTT
        public async Task LoadDiscountChildsPaymentSchemes(string Id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='bsd_bsd_discountonpaymentscheme_bsd_discoun' from='bsd_discountid' to='bsd_discountid' intersect='true'>
                                      <filter>
                                        <condition attribute='bsd_discountonpaymentschemeid' operator='eq' value='{Id}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            string fetchXml2 = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_special' operator='eq' value='1'/>
                                      <condition attribute='bsd_discounttype' operator='eq' value='100000002'/>
                                      <condition attribute='statuscode' operator='eq' value='100000000'/>
                                      <condition attribute='bsd_phaseslaunch' operator='eq' value='{this.PhasesLaunchId}'/>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml2);
            if ((result == null || result.value.Any() == false)&&(result2 == null || result2.value.Any() == false)) return;
            List<DiscountChildOptionSet> data = new List<DiscountChildOptionSet>();
            data.AddRange(result.value);
            data.AddRange(result2.value);

            foreach (var item in data)
            {
                item.IsEnableChecked = (this.IsHadLichThanhToan == true || item.IsExpired == true || item.IsNotApplied == true) ? false : true;
                this.DiscountChildsPaymentSchemes.Add(item);
            }
        }

        public async Task LoadDiscountSpecialPaymentSchemes() // hien tai khong dung
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <attribute name='bsd_method'/>
                                    <attribute name='bsd_startdate'/>
                                    <attribute name='bsd_enddate'/>
                                    <attribute name='createdon'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_special' operator='eq' value='1' />
                                    </filter>
                                    <link-entity name='bsd_bsd_discountonpaymentscheme_bsd_discoun' from='bsd_discountid' to='bsd_discountid' intersect='true'>
                                      <filter type='and'>
                                          <condition attribute='bsd_discounttype' operator='eq' value='100000000'/>
                                          <condition attribute='statuscode' operator='eq' value='100000000'/>
                                          <condition attribute='bsd_phaseslaunch' operator='eq' value='{PhasesLaunchId}'/>
                                        </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            foreach (var item in result.value)
            {
                this.DiscountChildsPaymentSchemes.Add(item);
            }
        }

        // Load Khuyen mai
        public async Task LoadPromotions()
        {
            // load KM theo dk: Type = No codition , Status = Approved , theo Dot mo ban va con thoi gian hieu luc
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_promotion'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_promotionid' alias='Val' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='bsd_type' operator='eq' value='100000000' />
                                        <condition attribute='statuscode' operator='eq' value='100000001' />
                                        <condition attribute='bsd_phaselaunch' operator='eq' uitype='bsd_phaseslaunch' value='{this.PhasesLaunchId}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PromotionModel>>("bsd_promotions", fetchXml);
            if (result == null || result.value.Any() == false) return;

            foreach (var item in result.value)
            {
                if (item.bsd_startdate.HasValue && item.bsd_enddate.HasValue && item.bsd_startdate.Value < DateTime.Now.Date && item.bsd_enddate.Value > DateTime.Now.Date)
                {
                    this.Promotions.Add(item);
                }
            }
        }

        public async Task LoadPromotionsSelected()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_promotion'>
                                        <attribute name='bsd_name' alias='Label' />
                                        <attribute name='bsd_promotionid' alias='Val' />
                                        <order attribute='createdon' descending='true' />
                                        <link-entity name='bsd_quote_bsd_promotion' from='bsd_promotionid' to='bsd_promotionid' visible='false' intersect='true'>
                                            <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + this.QuoteId + @"' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_promotions", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                this.PromotionsSelected.Add(item);
                this.SelectedPromotionIds.Add(item.Val);
            }
        }

        // Load Giu cho
        public async Task LoadQueues()
        {
            string fectchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='opportunityid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("opportunities", fectchXml);
            if (result == null || result.value.Any() == false) return;
            this.Queues = result.value;
        }

        // Load danh sach dai ly/ san giao dich
        public async Task LoadSalesAgents()
        {
            string fetchphaseslaunch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_phaseslaunch'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_locked' />
                                <attribute name='bsd_salesagentcompany' />
                                <attribute name='bsd_phaseslaunchid' />
                                <order attribute='bsd_name' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_phaseslaunchid' operator='eq' value='{PhasesLaunchId}' />
                                </filter>
                                <link-entity name='account' from='accountid' to='bsd_salesagentcompany' link-type='outer' alias='aw'>
                                    <attribute name='name' alias='salesagentcompany_name' />
                                </link-entity>
                              </entity>
                            </fetch>";
            var result_phasesLaunch = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLaunch>>("bsd_phaseslaunchs", fetchphaseslaunch);

            string develop = $@"<link-entity name='bsd_project' from='bsd_investor' to='accountid' link-type='inner'>
                                                <filter type='and'>
                                                    <condition attribute='bsd_projectid' operator='eq' value='{Quote.project_id}' />
                                                </filter>
                                            </link-entity>";
            string all = $@"<link-entity name='bsd_projectshare' from='bsd_salesagent' to='accountid' link-type='inner'>
                                                <filter type='and'>
                                                    <condition attribute='statuscode' operator='eq' value='1' />
                                                    <condition attribute='bsd_project' operator='eq' value='{Quote.project_id}' />
                                                </filter>
                                            </link-entity>";
            string sale_phasesLaunch = $@"<link-entity name='bsd_phaseslaunch' from='bsd_salesagentcompany' to='accountid' link-type='inner'>
                                                        <filter type='and'>
                                                            <condition attribute='bsd_phaseslaunchid' operator='eq' value='{PhasesLaunchId}' />
                                                         </filter>
                                                    </link-entity>";
            string isproject = $@"<filter type='and'>
                                       <condition attribute='bsd_businesstypesys' operator='contain-values'>
                                         <value>100000002</value>
                                       </condition>                                
                                    </filter>";

            if (result_phasesLaunch != null && result_phasesLaunch.value.Count > 0)
            {
                var phasesLaunch = result_phasesLaunch.value.FirstOrDefault();
                if (phasesLaunch.bsd_locked == false)
                {
                    if (string.IsNullOrWhiteSpace(phasesLaunch.salesagentcompany_name))
                    {
                        if (SalesAgents != null)
                        {
                            SalesAgents.AddRange(await LoadAccuntSales(all));
                            //SalesAgents.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                    else
                    {
                        if (SalesAgents != null)
                        {
                            SalesAgents.AddRange(await LoadAccuntSales(sale_phasesLaunch));
                            SalesAgents.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                }
                else if (phasesLaunch.bsd_locked == true)
                {
                    if (string.IsNullOrWhiteSpace(phasesLaunch.salesagentcompany_name))
                    {
                        if (SalesAgents != null)
                        {
                            SalesAgents.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                    else
                    {
                        if (SalesAgents != null)
                        {
                            SalesAgents.AddRange(await LoadAccuntSales(sale_phasesLaunch));
                        }
                    }
                }

            }
            else
            {
                if (SalesAgents != null)
                {
                    SalesAgents.AddRange(await LoadAccuntSales(all));
                    SalesAgents.AddRange(await LoadAccuntSales(develop));
                }
            }
            //Load account co field bsd_businesstypesys la sales agent(100000002)
            //string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                      <entity name='account'>
            //                        <attribute name='name' alias='Label'/>
            //                        <attribute name='accountid' alias='Val' />
            //                        <order attribute='createdon' descending='true' />
            //                        <filter type='and'>
            //                          <condition attribute='bsd_businesstype' operator='contain-values'>
            //                            <value>100000002</value>
            //                          </condition>
            //                        </filter>
            //                      </entity>
            //                    </fetch>";
            //var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetchXml);
            //if (result == null || result.value.Any() == false) return;
            //this.SalesAgents = result.value;
        }

        public async Task<List<OptionSet>> LoadAccuntSales(string filter)
        {
            List<OptionSet> list = new List<OptionSet>();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' alias='Label' />
                                    <attribute name='accountid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    " + filter + @"
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetch);
            if (result != null && result.value.Count != 0)
            {
                var data = result.value;
                foreach (var item in data)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        // Load cong tac vien
        public async Task LoadCollaboratorLookUp()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <order attribute='createdon' descending='true' />                   
                    <filter type='and'>
                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='" + UserLogged.Id + @"' />
                        <condition attribute='bsd_type' operator='eq' value='100000001' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
            if (result == null || result.value.Count == 0)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ListCollaborator.Add(item);
            }
        }

        // Load khach hang gioi thieu
        public async Task LoadCustomerReferralLookUp()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <order attribute='createdon' descending='true' />                   
                    <filter type='and'>
                        <condition attribute='{UserLogged.UserAttribute}' operator='eq' value='" + UserLogged.Id + @"' />
                        <condition attribute='bsd_type' operator='eq' value='100000000' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
            if (result == null || result.value.Count == 0)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ListCustomerReferral.Add(item);
            }
        }

        public async Task LoadCoOwners()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_coowner'>
                                    <attribute name='bsd_coownerid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='bsd_relationship' alias='bsd_relationshipId' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{this.QuoteId}' />
                                        <condition attribute='statuscode' operator='eq' value='1' />
                                    </filter>
                                    <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_c5024cb219dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='account_name'/>
                                        <attribute name='accountid' alias='account_id'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_b1053fd619dbeb11bacb002248168cad'>
                                        <attribute name='bsd_fullname' alias='contact_name' />
                                        <attribute name='contactid' alias='contact_id' />
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CoOwnerFormModel>>("bsd_coowners", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.bsd_relationship = RelationshipCoOwnerData.GetRelationshipById(item.bsd_relationshipId).Label;
                this.CoOwnerList.Add(item);
            }
        }

        public async Task<CrmApiResponse> DeletePromotion(string promotionId)
        {
            string path = $"/quotes({this.Quote.quoteid})//Microsoft.Dynamics.CRM.bsd_Action_Quote_Check_Promotion";
            List<string> CKTPTTTIds = new List<string>();
            foreach (var item in this.DiscountChildsPaymentSchemes)
            {
                if (item.Selected == true)
                {
                    CKTPTTTIds.Add(item.Val);
                }
            }
            string CKIds = CKTPTTTIds.Count > 0 ? string.Join(",", CKTPTTTIds) : null;

            var json = new
            {
                input = promotionId,
                input2 = CKIds
            };
            var data = JsonConvert.SerializeObject(json);
            CrmApiResponse apiResponse = await CrmHelper.PostData(path, data);
            return apiResponse;
        }

        public async Task<bool> AddPromotion(List<string> Ids)
        {
            if (Ids.Count == 0) return false;
            string path = $"/quotes({this.Quote.quoteid})/bsd_quote_bsd_promotion/$ref";
            IDictionary<string, string> data = new Dictionary<string, string>();
            CrmApiResponse apiResponse = new CrmApiResponse();
            foreach (var item in Ids)
            {
                data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_promotions({item})";
                apiResponse = await CrmHelper.PostData(path, data);
            }
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddHandoverCondition()
        {
            string path = $"/quotes({this.Quote.quoteid})/bsd_quote_bsd_packageselling/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_packagesellings({this.HandoverCondition.Val})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddCoOwer()
        {
            if (this.CoOwnerList == null || this.CoOwnerList.Count == 0) return false;
            string path = "/bsd_coowners";
            CrmApiResponse apiResponse = new CrmApiResponse();
            if (this.QuoteId != Guid.Empty)
            {
                var content = await GetContentCoOwer(this.CoOwner);
                apiResponse = await CrmHelper.PostData(path, content);
            }
            else
            {
                foreach (var item in this.CoOwnerList)
                {
                    var content = await GetContentCoOwer(item);
                    apiResponse = await CrmHelper.PostData(path, content);
                }
            }

            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CrmApiResponse> UpdateCoOwner()
        {
            CrmApiResponse apiResponse = new CrmApiResponse();
            if (this.CoOwnerList == null || this.CoOwnerList.Count == 0)
            {
                apiResponse.IsSuccess = false;
                return apiResponse;
            }
            string path = $"/bsd_coowners({this.CoOwnerList[0].bsd_coownerid})";
            var content = await GetContentCoOwer(this.CoOwner);
            apiResponse = await CrmHelper.PatchData(path, content);
            return apiResponse;
        }

        private async Task<object> GetContentCoOwer(CoOwnerFormModel coOwner)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_coownerid"] = coOwner.bsd_coownerid;
            data["bsd_name"] = coOwner.bsd_name;
            data["bsd_relationship"] = coOwner.bsd_relationshipId;
            data["bsd_reservation@odata.bind"] = $"quotes({this.Quote.quoteid})";

            data["bsd_project@odata.bind"] = $"/bsd_projects({this.Quote.project_id})";
            data["bsd_units@odata.bind"] = $"/products({this.Quote.unit_id})";

            if (this.CustomerCoOwner.Title == "2")
            {
                data["bsd_customer_contact@odata.bind"] = $"/contacts({coOwner.contact_id})";
                await CrmHelper.SetNullLookupField("bsd_coowners", coOwner.bsd_coownerid, "bsd_customer_contact");
            }
            else
            {
                data["bsd_customer_account@odata.bind"] = $"/accounts({coOwner.account_id})";
                await CrmHelper.SetNullLookupField("bsd_coowners", coOwner.bsd_coownerid, "bsd_customer_account");
            }
            return data;
        }

        public async Task<CrmApiResponse> CreateQuote()
        {
            string path = "/quotes";
            var content = await GetContentCreateQuote();
            CrmApiResponse response = await CrmHelper.PostData(path, content);
            return response;
        }

        public async Task<CrmApiResponse> UpdateQuote()
        {
            string path = $"/quotes({this.Quote.quoteid})";
            var content = await GetContentUpdateQuote();
            CrmApiResponse response = await CrmHelper.PatchData(path, content);
            return response;
        }

        public async Task<CrmApiResponse> UpdateQuote_HasLichThanhToan()
        {
            string path = $"/quotes({this.Quote.quoteid})";
            var content = await GetContentUpdateQuote_HasLichThanhToan();
            CrmApiResponse response = await CrmHelper.PatchData(path, content);
            return response;
        }

        public async Task<CrmApiResponse> UpdatePaymentShemes()
        {
            string path = $"/quotes({this.Quote.quoteid})";
            var content = await GetContentPaymentShemes();
            CrmApiResponse response = await CrmHelper.PatchData(path, content);
            return response;
        }

        public async Task<CrmApiResponse> UpdateDiscountChildsPaymentShemes()
        {
            string path = $"/quotes({this.Quote.quoteid})";
            var content = await GetContentDiscountChildsPaymentShemes();
            CrmApiResponse response = await CrmHelper.PatchData(path, content);
            return response;
        }

        private async Task<object> GetContentDiscountChildsPaymentShemes()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            ckPTTTIds = new List<string>();
            foreach (var item in DiscountChildsPaymentSchemes)
            {
                if (item.Selected == true)
                {
                    ckPTTTIds.Add(item.Val);
                }
            }
            data["bsd_selectedchietkhaupttt"] = ckPTTTIds.Count > 0 ? string.Join(",", ckPTTTIds) : null; ;
            return data;
        }

        private async Task<object> GetContentPaymentShemes()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (this.PaymentScheme != null)
            {
                data["bsd_paymentscheme@odata.bind"] = $"/bsd_paymentschemes({this.PaymentScheme.bsd_paymentschemeid})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_paymentscheme");
            }
            return data;
        }

        public async Task<object> GetContentCreateQuote()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            Quote.quoteid = Guid.NewGuid();
            data["quoteid"] = this.Quote.quoteid;
            data["name"] = this.Quote.name;

            data["bsd_depositfee"] = this.Quote.bsd_depositfee;
            data["bsd_unitstatus"] = this.Quote.bsd_unitstatus;
            data["bsd_constructionarea"] = this.Quote.bsd_constructionarea;
            data["bsd_netusablearea"] = this.Quote.bsd_netusablearea;
            data["bsd_actualarea"] = this.Quote.bsd_actualarea;

            data["bsd_projectid@odata.bind"] = $"/bsd_projects({this.Quote._bsd_projectcode_value})";
            data["bsd_taxcode@odata.bind"] = $"/bsd_taxcodes({this.TaxCode.bsd_taxcodeid})";
            data["bsd_unitno@odata.bind"] = $"/products({this.Quote.unit_id})";

            data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)";

            if (this.Quote._bsd_phaseslaunchid_value != Guid.Empty)//this.UnitInfor._bsd_phaseslaunchid_value != Guid.Empty
            {
                data["bsd_phaseslaunchid@odata.bind"] = $"/bsd_phaseslaunchs({this.Quote._bsd_phaseslaunchid_value})";
            }

            if (this.Quote.bsd_startingdatecalculateofps.HasValue)
            {
                data["bsd_startingdatecalculateofps"] = this.Quote.bsd_startingdatecalculateofps.Value.Date;
            }

            if (this.PaymentScheme != null)
            {
                data["bsd_paymentscheme@odata.bind"] = $"/bsd_paymentschemes({this.PaymentScheme.bsd_paymentschemeid})";
                this.Quote.paymentscheme_id = this.PaymentScheme.bsd_paymentschemeid;
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_paymentscheme");
            }
            if (this.Buyer.Title == "2")
            {
                data["customerid_contact@odata.bind"] = $"/contacts({this.Buyer.Val})";
                //await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_account");
            }
            else if (this.Buyer.Title == "3")
            {
                data["customerid_account@odata.bind"] = $"/accounts({this.Buyer.Val})";
                //await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_contact");
            }

            if (UserLogged.IsLoginByUserCRM == false && UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.IsLoginByUserCRM == false && UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        public async Task<object> GetContentUpdateQuote()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data["name"] = this.Quote.name;
            data["bsd_discounts"] = string.Join(",", ckChungIds);
            data["bsd_interneldiscount"] = string.Join(",", ckNoiBoIds);
            data["bsd_selectedchietkhaupttt"] = string.Join(",", ckPTTTIds);
            data["bsd_exchangediscount"] = string.Join(",", ckQuyDoiIds);

            data["bsd_paymentschemestype"] = this.PaymentSchemeType?.Val;
            data["bsd_bookingfee"] = this.Quote.bsd_bookingfee;
            data["bsd_depositfee"] = this.Quote.bsd_depositfee;
            data["bsd_nameofstaffagent"] = this.Quote.bsd_nameofstaffagent;
            data["bsd_numberofmonthspaidmf"] = this.Quote.bsd_numberofmonthspaidmf;
            data["bsd_managementfee"] = this.Quote.bsd_managementfee;

            data["bsd_detailamount"] = decimal.Round(this.TotalReservation.ListedPrice, 0);
            data["bsd_discount"] = this.TotalReservation.Discount;
            data["bsd_packagesellingamount"] = this.TotalReservation.HandoverAmount;
            data["bsd_totalamountlessfreight"] = this.TotalReservation.NetSellingPrice;
            data["bsd_landvaluededuction"] = this.TotalReservation.LandValue;
            data["bsd_freightamount"] = this.TotalReservation.MaintenanceFee;
            data["totaltax"] = this.TotalReservation.TotalTax;
            data["bsd_netsellingpriceaftervat"] = this.TotalReservation.NetSellingPriceAfterVAT;
            data["totalamount"] = this.TotalReservation.TotalAmount;

            data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)"; // Don vi tien te mac dinh la "đ"

            if (this.Quote.bsd_startingdatecalculateofps.HasValue)
            {
                data["bsd_startingdatecalculateofps"] = this.Quote.bsd_startingdatecalculateofps.Value.Date;
            }
            if (this.Quote.pricelist_apply_id != Guid.Empty)
            {
                data["pricelevelid@odata.bind"] = $"/pricelevels({this.Quote.pricelist_apply_id})";
            }

            if (this.DiscountList != null)
            {
                data["bsd_discountlist@odata.bind"] = $"/bsd_discounttypes({this.DiscountList.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_discountlist");
            }
            if (this.DiscountInternelList != null)
            {
                data["bsd_interneldiscountlist@odata.bind"] = $"/bsd_interneldiscounts({this.DiscountInternelList.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_interneldiscountlist");
            }
            if (this.DiscountExchangeList != null)
            {
                data["bsd_exchangediscountlist@odata.bind"] = $"/bsd_discountpromotions({this.DiscountExchangeList.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_exchangediscountlist");
            }
            if (this.Queue != null)
            {
                data["opportunityid@odata.bind"] = $"/opportunities({this.Queue.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "opportunityid");
            }
            if (this.Buyer.Title == "2")
            {
                data["customerid_contact@odata.bind"] = $"/contacts({this.Buyer.Val})";
                //await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_account");
            }
            else if (this.Buyer.Title == "3")
            {
                data["customerid_account@odata.bind"] = $"/accounts({this.Buyer.Val})";
                //await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_contact");
            }
            if (this.SalesAgent != null && Guid.Parse(this.SalesAgent?.Val) != Guid.Empty)
            {
                data["bsd_salessgentcompany@odata.bind"] = $"/accounts({this.SalesAgent.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_salessgentcompany");
            }
            if (this.Collaborator != null)
            {
                data["bsd_collaborator@odata.bind"] = $"/contacts({this.Collaborator.Id})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_collaborator");
            }
            if (this.CustomerReferral != null)
            {
                data["bsd_customerreferral_contact@odata.bind"] = $"/contacts({this.CustomerReferral.Id})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_customerreferral_contact");
            }

            return data;
        }

        public async Task<object> GetContentUpdateQuote_HasLichThanhToan()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data["name"] = this.Quote.name;
            //data["bsd_exchangediscount"] = string.Join(",", ckQuyDoiIds);
            data["bsd_paymentschemestype"] = this.PaymentSchemeType?.Val;
            if (this.Quote.bsd_startingdatecalculateofps.HasValue)
            {
                data["bsd_startingdatecalculateofps"] = this.Quote.bsd_startingdatecalculateofps.Value.Date;
            }

            if (this.DiscountExchangeList != null)
            {
                data["bsd_exchangediscountlist@odata.bind"] = $"/bsd_discountpromotions({this.DiscountExchangeList.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_exchangediscountlist");
            }
            return data;
        }

        public async Task<CrmApiResponse> CreateQuoteProduct()
        {
            string path = "/quotedetails";
            var content = await GetContentQuoteProduct();
            CrmApiResponse apiResponse = await CrmHelper.PostData(path, content);
            return apiResponse;
        }

        public async Task<CrmApiResponse> UpdateQuoteProduct()
        {
            string path = $"/quotedetails({quotedetailid})";
            var content = await GetContentQuoteProduct();
            CrmApiResponse response = await CrmHelper.PatchData(path, content);
            return response;
        }

        public async Task<object> GetContentQuoteProduct()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (quotedetailid != Guid.Empty)
            {
                data["baseamount"] = this.TotalReservation.ListedPrice;
                data["volumediscountamount"] = this.TotalReservation.ListedPrice;
                data["tax"] = this.TotalReservation.TotalTax;
                data["manualdiscountamount"] = this.TotalReservation.Discount;
                data["extendedamount"] = this.TotalReservation.ListedPrice + TotalReservation.TotalTax;
            }
            else
            {
                quotedetailid = Guid.NewGuid();
                data["quotedetailid"] = quotedetailid;
                data["isproductoverridden"] = false;
                data["ispriceoverridden"] = false;
                data["priceperunit"] = this.UnitInfor.price;
                data["quantity"] = 1;
                data["quotedetailname"] = this.Quote.name;

                data["quoteid@odata.bind"] = $"/quotes({this.Quote.quoteid})";
                data["uomid@odata.bind"] = $"/products({this.UnitInfor._defaultuomid_value})";
                data["productid@odata.bind"] = $"/products({this.UnitInfor.productid})";
                data["createdby@odata.bind"] = $"/systemusers({UserLogged.ManagerId})";
                data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)";
                //transactioncurrencyid
            }

            return data;
        }

        public async Task<CrmApiResponse> DeleteQuote()
        {
            var result = await CrmHelper.DeleteRecord($"/quotes({this.QuoteId})");
            return result;
        }

    }
}
