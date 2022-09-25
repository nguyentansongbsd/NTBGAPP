using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class MandatorySecondaryFormViewModel : BaseViewModel
    {
        private MandatorySecondaryModel _mandatorySecondary;
        public MandatorySecondaryModel mandatorySecondary { get => _mandatorySecondary; set { _mandatorySecondary = value; OnPropertyChanged(nameof(mandatorySecondary)); } }
        public ObservableCollection<LookUp> list_contact_lookup { get; set; }

        private LookUp _contact;
        public LookUp Contact { get => _contact; set { _contact = value; OnPropertyChanged(nameof(Contact)); } }

        public MandatorySecondaryFormViewModel()
        {
            mandatorySecondary = new MandatorySecondaryModel();
            list_contact_lookup = new ObservableCollection<LookUp>();
        }
        //public async Task GetOneAccountById( string accountid)
        //{
        //    string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
        //                      <entity name='account'>
        //                        <attribute name='bsd_name' />                                
        //                        <attribute name='accountid' />
        //                        <attribute name='bsd_businesstypesys' alias='bsd_businesstype' />
        //                        <order attribute='createdon' descending='true' />
        //                            <filter type='and'>
        //                              <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
        //                            </filter>                                   
        //                      </entity>
        //                    </fetch>";
        //    var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
        //    if (result == null)
        //        return;
        //    var tmp = result.value.FirstOrDefault();
        //    if (tmp == null)
        //    {
        //        return;
        //    }
        //    mandatorySecondary.bsd_developeraccount = tmp.bsd_name;
        //    mandatorySecondary._bsd_developeraccount_value = tmp.accountid;
        //}

        public async Task LoadContactsLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='bsd_fullname' alias='Name'/>
                    <attribute name='contactid' alias='Id' />
                    <order attribute='fullname' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                list_contact_lookup.Add(x);
            }
        }

        public async Task<bool> Save()
        {
            if(mandatorySecondary == null)
            {
                return false;
            }
            string path = "/bsd_mandatorysecondaries";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }    

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_name"] = mandatorySecondary.bsd_name;
            data["statuscode"] = mandatorySecondary.statuscode = 1;
            data["bsd_descriptionsvn"] = mandatorySecondary.bsd_descriptionsvn;
            data["bsd_descriptionsen"] = mandatorySecondary.bsd_descriptionsen;
            data["bsd_jobtitlevn"] = mandatorySecondary.bsd_jobtitlevn;
            data["bsd_jobtitleen"] = mandatorySecondary.bsd_jobtitleen;
            data["bsd_effectivedateto"] = mandatorySecondary.bsd_effectivedateto.HasValue ? (DateTime.Parse(mandatorySecondary.bsd_effectivedateto.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
            data["bsd_effectivedatefrom"] = mandatorySecondary.bsd_effectivedatefrom.HasValue ? (DateTime.Parse(mandatorySecondary.bsd_effectivedatefrom.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
            if (mandatorySecondary.bsd_contactid == null)
            {
                await DeletLookup("bsd_contact", mandatorySecondary.bsd_mandatorysecondaryid);
            }
            else
            {
                data["bsd_contact@odata.bind"] = "/contacts(" + mandatorySecondary.bsd_contactid + ")";
            }

            if (mandatorySecondary._bsd_developeraccount_value == null)
            {
                await DeletLookup("bsd_developeraccount", mandatorySecondary.bsd_mandatorysecondaryid);
            }
            else
            {
                data["bsd_developeraccount@odata.bind"] = "/accounts(" + mandatorySecondary._bsd_developeraccount_value + ")";
            }

            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            return data;
        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid Id)
        {
            var result = await CrmHelper.SetNullLookupField("bsd_mandatorysecondaries", Id , fieldName);
            return result.IsSuccess;
        }
    }
}
