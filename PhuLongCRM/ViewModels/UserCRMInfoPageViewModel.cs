using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhuLongCRM.ViewModels
{
    public class UserCRMInfoPageViewModel : BaseViewModel
    {
        private string _avatar;
        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }

        private UserModel _userCRM;
        public UserModel UserCRM { get => _userCRM; set { _userCRM = value; OnPropertyChanged(nameof(UserCRM)); } }

        public UserCRMInfoPageViewModel()
        {
            this.Avatar = $"https://ui-avatars.com/api/?background=2196F3&rounded=false&color=ffffff&size=150&length=2&name={UserLogged.UserCRM}";
        }
        public async Task LoadUserCRM()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='systemuser'>
                                    <attribute name='fullname' />
                                    <attribute name='businessunitid' />
                                    <attribute name='systemuserid' />
                                    <attribute name='internalemailaddress' />
                                    <attribute name='mobilephone' />
                                    <attribute name='domainname' />
                                    <order attribute='fullname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='systemuserid' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UserModel>>("systemusers", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.UserCRM = result.value.SingleOrDefault();
        }
    }
}
