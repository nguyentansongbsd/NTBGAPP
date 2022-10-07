using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class ContractDetailPageViewModel : BaseViewModel
    {
        private ContractModel _contract;
        public ContractModel Contract { get => _contract; set { _contract = value; OnPropertyChanged(nameof(Contract)); } }
        public ObservableCollection<ReservationCoownerModel> CoownerList { get; set; }
        public ObservableCollection<ReservationInstallmentDetailPageModel> InstallmentList { get; set; }

        private int _numberInstallment;
        public int NumberInstallment { get => _numberInstallment; set { _numberInstallment = value; OnPropertyChanged(nameof(NumberInstallment)); } }

        private bool _showInstallmentList;
        public bool ShowInstallmentList { get => _showInstallmentList; set { _showInstallmentList = value; OnPropertyChanged(nameof(ShowInstallmentList)); } }
        public ObservableCollection<DiscountModel> ListDiscount { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountSpecialModel> ListSpecialDiscount { get; set; } = new ObservableCollection<DiscountSpecialModel>();
        public ObservableCollection<OptionSet> ListPromotion { get; set; } = new ObservableCollection<OptionSet>();

        public ObservableCollection<DiscountModel> ListDiscountPaymentScheme { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountModel> ListDiscountInternel { get; set; } = new ObservableCollection<DiscountModel>();
        public ObservableCollection<DiscountModel> ListDiscountExchange { get; set; } = new ObservableCollection<DiscountModel>();

        private PromotionModel _promotionItem;
        public PromotionModel PromotionItem { get => _promotionItem; set { _promotionItem = value; OnPropertyChanged(nameof(PromotionItem)); } }

        private HandoverConditionModel _handoverConditionItem;
        public HandoverConditionModel HandoverConditionItem { get => _handoverConditionItem; set { _handoverConditionItem = value; OnPropertyChanged(nameof(HandoverConditionItem)); } }

        private DiscountSpecialModel _discountSpecialItem;
        public DiscountSpecialModel DiscountSpecialItem { get => _discountSpecialItem; set { _discountSpecialItem = value; OnPropertyChanged(nameof(DiscountSpecialItem)); } }

        private DiscountModel _discount;
        public DiscountModel Discount { get => _discount; set { _discount = value; OnPropertyChanged(nameof(Discount)); } }

        private InterestInstallmentModel _interest;
        public InterestInstallmentModel Interest { get => _interest; set { _interest = value; OnPropertyChanged(nameof(Interest)); } }

        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        private ConfirmDocumentForPinkBookDetailModel _confirmDocument;
        public ConfirmDocumentForPinkBookDetailModel ConfirmDocument { get => _confirmDocument; set { _confirmDocument = value;OnPropertyChanged(nameof(ConfirmDocument)); } }

        private ObservableCollection<UnitHandoversModel> _unitHandovers;
        public ObservableCollection<UnitHandoversModel> UnitHandovers { get => _unitHandovers; set { _unitHandovers = value; OnPropertyChanged(nameof(UnitHandovers)); } }

        public ObservableCollection<AcceptanceListModel> _acceptances;
        public ObservableCollection<AcceptanceListModel> Acceptances { get => _acceptances; set { _acceptances = value; OnPropertyChanged(nameof(Acceptances)); } }

        public ObservableCollection<PinkBookHandoversModel> PinkBooHandovers { get; set; } = new ObservableCollection<PinkBookHandoversModel>();

        private bool _showMoreAcceptances;
        public bool ShowMoreAcceptances { get => _showMoreAcceptances; set { _showMoreAcceptances = value; OnPropertyChanged(nameof(ShowMoreAcceptances)); } }

        private bool _showMorePinkBooHandover;
        public bool ShowMorePinkBooHandover { get => _showMorePinkBooHandover; set { _showMorePinkBooHandover = value; OnPropertyChanged(nameof(ShowMorePinkBooHandover)); } }

        private bool _showMoreUnitHandovers;
        public bool ShowMoreUnitHandovers { get => _showMoreUnitHandovers; set { _showMoreUnitHandovers = value; OnPropertyChanged(nameof(ShowMoreUnitHandovers)); } }

        public int PagePinkBooHandover { get; set; } = 1;
        public int PageAcceptance { get; set; } = 1;
        public int PageUnitHandover { get; set; } = 1;

        public bool IsLoaded { get; set; } = false;

        public ContractDetailPageViewModel()
        {
            Contract = new ContractModel();
            CoownerList = new ObservableCollection<ReservationCoownerModel>();
            InstallmentList = new ObservableCollection<ReservationInstallmentDetailPageModel>();
            UnitHandovers = new ObservableCollection<UnitHandoversModel>();
            Acceptances = new ObservableCollection<AcceptanceListModel>();
        }

        public async Task LoadContract(Guid ContractId)
        {
            // <attribute name='bsd_terminationletter' />
            // <attribute name='bsd_dadate' />
            //<attribute name='bsd_contracttypedescription' />
            //< attribute name = 'bsd_updatecontractdate' />
            // lỗi thiếu filed
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <attribute name='bsd_optionno' />
                                    <attribute name='customerid' />
                                    <attribute name='name' alias='salesorder_name'/>
                                    <attribute name='totalamount' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_referral' />
                                    <attribute name='bsd_queuingfee' />
                                    <attribute name='bsd_depositamount' />
                                    <attribute name='bsd_allowchangeunitsspec' />
                                    <attribute name='bsd_estimatehandoverdatecontract' />
                                    <attribute name='bsd_followuplist' />
                                    <attribute name='bsd_agreementdate' />
                                    <attribute name='bsd_signeddadate' />
                                    <attribute name='bsd_contractnumber' />
                                    <attribute name='bsd_contracttype' />
                                    <attribute name='bsd_contractdate' />
                                    <attribute name='bsd_contractprinteddate' />
                                    <attribute name='bsd_signingexpired' />
                                    <attribute name='bsd_signedcontractdate' />
                                    <attribute name='bsd_bsd_uploadeddate' />
                                    <attribute name='bsd_detailamount' />
                                    <attribute name='bsd_discount' />
                                    <attribute name='bsd_packagesellingamount' />
                                    <attribute name='bsd_totalamountlessfreight' />
                                    <attribute name='bsd_landvaluededuction' />
                                    <attribute name='totaltax' />
                                    <attribute name='bsd_freightamount' />
                                    <attribute name='totalamount' alias='bsd_totalamount'/>
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_managementfee' />
                                    <attribute name='bsd_waivermanafeemonth' />
                                    <attribute name='bsd_discounts' />
                                    <attribute name='bsd_interneldiscount' />
                                    <attribute name='bsd_exchangediscount' />
                                    <attribute name='bsd_totalamountpaidinstallment' />
                                    <attribute name='bsd_totalpercent' />
                                    <attribute name='bsd_selectedchietkhaupttt' />
                                    <order attribute='ordernumber' descending='false' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='aa'>
                                       <attribute name='bsd_projectid' alias='project_id'/>
                                       <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='quote' from='quoteid' to='quoteid' link-type='inner' alias='ak' >
                                        <attribute name='quoteid' alias='queue_id'/>
                                        <attribute name='name' alias='queue_name' />
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitnumber' link-type='outer' alias='ab'>
                                       <attribute name='productid' alias='unit_id'/>
                                       <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunch' link-type='outer' alias='ac'>
                                        <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id'/>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ad'>
                                       <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ae'>
                                      <attribute name='bsd_fullname' alias='contact_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' link-type='outer' alias='ag'>
                                      <attribute name='bsd_taxcodeid' alias='taxcode_id'/>
                                      <attribute name='bsd_name' alias='taxcode_name'/>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' link-type='outer' alias='aj'>
                                        <attribute name='pricelevelid'/>
                                        <attribute name='name' alias='pricelevel_name'/>
                                    </link-entity>

                                    <link-entity name='bsd_interneldiscount' from='bsd_interneldiscountid' to='bsd_interneldiscountlist' visible='false' link-type='outer' alias='a_c014fc37ba81e911a83b000d3a07be23'>
                                        <attribute name='bsd_name' alias='interneldiscount_name'/>
                                        <attribute name='bsd_interneldiscountid' alias='interneldiscount_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discountpromotion' from='bsd_discountpromotionid' to='bsd_exchangediscountlist' visible='false' link-type='outer' alias='a_2e80b433b075eb11a812000d3ac8b5f4'>
                                        <attribute name='bsd_name' alias='discountpromotion_name'/>
                                        <attribute name='bsd_discountpromotionid' alias='discountpromotion_id'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='salesorderid' operator='eq' value='" + ContractId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }

            Contract = result.value.SingleOrDefault();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='salesorderid' />
                                    <attribute name='bsd_handover' alias='unit_handoverid'/>
                                    <attribute name='bsd_acceptanceinformation' alias='acceptanceid'/>
                                    <attribute name='ordernumber' />
                                    <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='outer' alias='al'>
                                       <attribute name='bsd_name' alias='bsd_unitsspecification_name' />
                                      <attribute name='bsd_unitsspecificationid' alias='bsd_unitsspecification_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_exchangeratedetail' from='bsd_exchangeratedetailid' to='bsd_applyingexchangerate' link-type='outer' alias='ak'>
                                      <attribute name='bsd_name' alias='bsd_exchangeratedetail_name' />
                                      <attribute name='bsd_exchangeratedetailid'/>
                                    </link-entity>
                                    <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' link-type='outer' alias='ai'>
                                       <attribute name='bsd_name' alias='paymentscheme_name'/>
                                       <attribute name='bsd_paymentschemeid' alias='paymentscheme_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' link-type='outer' alias='ae'>
                                        <attribute name='bsd_discounttypeid' alias='discountlist_id' />
                                        <attribute name='bsd_name' alias='discountlist_name' />
                                    </link-entity>
                                    <link-entity name='bsd_pinkbookhandover' from='bsd_optionentry' to='salesorderid' link-type='inner' alias='ac' >
                                        <attribute name='bsd_pinkbookhandoverid' alias='pinkbook_handoverid'/>
                                    </link-entity >
                                    <filter type='and'>
                                        <condition attribute='salesorderid' operator='eq' value='" + ContractId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetch);
            if (result2 == null || result2.value.Count == 0)
            {
                return;
            }
            var data = result2.value.SingleOrDefault();
            Contract.bsd_unitsspecification_name = data.bsd_unitsspecification_name;
            Contract.bsd_unitsspecification_id = data.bsd_unitsspecification_id;
            Contract.bsd_exchangeratedetail_name = data.bsd_exchangeratedetail_name;
            Contract.bsd_exchangeratedetailid = data.bsd_exchangeratedetailid;
            Contract.paymentscheme_name = data.paymentscheme_name;
            Contract.paymentscheme_id = data.paymentscheme_id;
            Contract.discountlist_id = data.discountlist_id;
            Contract.discountlist_name = data.discountlist_name;

            Contract.unit_handoverid = data.unit_handoverid;
            Contract.acceptanceid = data.acceptanceid;
            Contract.pinkbook_handoverid = data.pinkbook_handoverid;
        }

        public async Task LoadCoOwners(Guid ContractId)
        {
            string xml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_coowner'>
                                <attribute name='bsd_coownerid' />
                                <attribute name='bsd_name' />
                                <attribute name='bsd_relationship' />
                                <order attribute='bsd_name' descending='true' />
                                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                                  <attribute name='bsd_name' alias='account_name' />
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                                  <attribute name='bsd_fullname' alias='contact_name' />
                                </link-entity>
                                 <filter type='and'>
                                      <condition attribute='bsd_optionentry' operator='eq' uitype='quote' value='{ContractId}' />
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

        public async Task LoadHandoverCondition(Guid ContractId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_packageselling'>
                                        <attribute name='bsd_name' alias='handovercondition_name' />
                                        <attribute name='bsd_packagesellingid' alias='handovercondition_id' />
                                        <order attribute='bsd_name' descending='true' />
                                        <link-entity name='bsd_salesorder_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                                            <link-entity name='salesorder' from='salesorderid' to='salesorderid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='salesorderid' operator='eq' uitype='quote' value='" + ContractId + @"' />
                                                </filter>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDetailPageModel>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            Contract.handovercondition_id = result.value.FirstOrDefault().handovercondition_id;
            Contract.handovercondition_name = result.value.FirstOrDefault().handovercondition_name;
        }

        public async Task LoadSpecialDiscount(Guid ContractId)
        {
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
                                            <condition attribute='bsd_optionentry' operator='eq' uitype='salesorder' value='" + ContractId + @"' />
                                        </filter>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountSpecialModel>>("bsd_discountspecials", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            foreach (var item in result.value)
            {
                this.ListSpecialDiscount.Add(item);
            }
        }

        public async Task LoadPromotions(Guid ContractId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_promotion'>
                                        <attribute name='bsd_name' alias='Label' />
                                        <attribute name='bsd_promotionid' alias='Val' />
                                        <order attribute='createdon' descending='true' />
                                        <link-entity name='bsd_salesorder_bsd_promotion' from='bsd_promotionid' to='bsd_promotionid' visible='false' intersect='true'>
                                            <link-entity name='salesorder' from='salesorderid' to='salesorderid' alias='ab'>
                                                <filter type='and'>
                                                    <condition attribute='salesorderid' operator='eq' uitype='quote' value='" + ContractId + @"' />
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
            if (string.IsNullOrWhiteSpace(this.Contract.bsd_discounts)) return;
            string[] arrDiscounts = new string[] { };
            string conditionValue = string.Empty;
            arrDiscounts = this.Contract.bsd_discounts.Split(',');
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

        public async Task LoadInstallmentList(Guid ContractId)
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
                <attribute name='bsd_interestchargeamount' />
                <order attribute='bsd_ordernumber' descending='false' />
                <filter type='and'>
                    <condition attribute='statecode' operator='eq' value='0' />
                    <condition attribute='bsd_optionentry' operator='eq' value='{ContractId}'/>
                </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationInstallmentDetailPageModel>>("bsd_paymentschemedetails", xml);
            if (result == null || result.value.Count == 0)
                return;

            foreach (var x in result.value)
            {
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
            if (data.bsd_startdate.HasValue)
            {
                data.bsd_startdate = data.bsd_startdate.Value.ToLocalTime();
            }
            if (data.bsd_enddate.HasValue)
            {
                data.bsd_enddate = data.bsd_enddate.Value.ToLocalTime();
            }
            PromotionItem = data;
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
            var data = result.value.SingleOrDefault();
            data.bsd_startdate = data.bsd_startdate.ToLocalTime();
            data.bsd_enddate = data.bsd_enddate.ToLocalTime();
            Discount = data;
            //await LoadDiscountItems(Discount.bsd_discountid);
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

            var data = result.value.SingleOrDefault();
            if (data.bsd_startdate.HasValue)
            {
                data.bsd_startdate = data.bsd_startdate.Value.ToLocalTime();
            }
            if (data.bsd_enddate.HasValue)
            {
                data.bsd_enddate = data.bsd_enddate.Value.ToLocalTime();
            }
            HandoverConditionItem = data;
        }

        public async Task LoadDiscountsInternel()
        {
            if (string.IsNullOrWhiteSpace(this.Contract.bsd_interneldiscount)) return;
            string[] arrDiscountsInternel = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsInternel = this.Contract.bsd_interneldiscount.Split(',');
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
            if (string.IsNullOrWhiteSpace(this.Contract.bsd_exchangediscount)) return;
            string[] arrDiscountsExchange = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsExchange = this.Contract.bsd_exchangediscount.Split(',');
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

        public async Task LoadDiscountsPaymentScheme()
        {
            if (string.IsNullOrWhiteSpace(this.Contract.bsd_selectedchietkhaupttt)) return;
            string[] arrDiscountsPaymentscheme = new string[] { };
            string conditionValue = string.Empty;
            arrDiscountsPaymentscheme = this.Contract.bsd_selectedchietkhaupttt.Split(',');
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

        public async Task LoadInstallmentById(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentschemedetail'>
                                    <attribute name='bsd_paymentschemedetailid' />
                                    <attribute name='bsd_actualgracedays' />
                                    <attribute name='bsd_interestwaspaid' />
                                    <attribute name='bsd_interestchargestatus' />
                                    <attribute name='bsd_interestchargeremaining' />
                                    <attribute name='bsd_interestchargeamount' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_paymentschemedetailid' operator='eq' value='{id}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<InterestInstallmentModel>>("bsd_paymentschemedetails", fetchXml);
            if (result == null && result.value.Count == 0) return;
            this.Interest = result.value.SingleOrDefault();
        }

        public async Task LoadConfirmDocumentForPinkBookDetail(Guid id)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_confirmdocumentforpinbookdetail'>
                                    <attribute name='bsd_confirmdocumentforpinbookdetailid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_hasvatinvoice' />
                                    <attribute name='bsd_hasidcardpassport' />
                                    <attribute name='bsd_hascontract' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_pptionentry' operator='eq' value='{id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ConfirmDocumentForPinkBookDetailModel>>("bsd_confirmdocumentforpinbookdetails", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.ConfirmDocument = result.value.SingleOrDefault();
        }

        public async Task LoadAcceptances(Guid contractid)
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageAcceptance}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_acceptance'>
                                    <attribute name='bsd_acceptanceid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_contract' operator='eq' value='{contractid}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='project'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' link-type='outer' alias='product'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_contract' link-type='outer' alias='contract'>
                                        <attribute name='name' alias='contract_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AcceptanceListModel>>("bsd_acceptances", fetch);
            if (result == null || result.value.Count == 0) return;

            IsLoaded = true;
            var data = result.value;
            ShowMoreAcceptances = data.Count < 5 ? false : true;

            foreach (var item in data)
            {
                Acceptances.Add(item);
            }
        }

        public async Task LoadUnitHandovers(Guid contractid)
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageUnitHandover}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_handover'>
                                    <attribute name='bsd_handoverid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_optionentry' operator='eq' value='{contractid}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a_3743f43dba81e911a83b000d3a07be23'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_96e2d45bba81e911a83b000d3a07be23'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitHandoversModel>>("bsd_handovers", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreUnitHandovers = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.UnitHandovers.Add(item);
            }
        }

        public async Task LoadPinkBooHandovers(Guid contractid)
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PagePinkBooHandover}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_pinkbookhandover'>
                                    <attribute name='bsd_name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_pinkbookhandoverid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statuscode' operator='ne' value='2' />
                                        <condition attribute='bsd_optionentry' operator='eq' value='{contractid}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_26f8767ec690ec11b400000d3aa1f0ac'>
                                      <attribute name='bsd_contactfullname' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_91124d44c790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_fd36f62dc790ec11b400000d3aa1f0ac'>
                                      <attribute name='name' alias='optionentry_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PinkBookHandoversModel>>("bsd_pinkbookhandovers", fetch);
            if (result == null) return;
            IsLoaded = true;
            var data = result.value;
            ShowMorePinkBooHandover = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                PinkBooHandovers.Add(x);
            }
        }
    }
}
