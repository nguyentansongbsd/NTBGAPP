using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhuLongCRM.ViewModels
{
    public class DanhBaViewModel : BaseViewModel
    {
        public ObservableCollection<DanhBaItemModel> Contacts { get; set; }

        public ObservableCollection<LeadListModel> LeadConvert { get; set; } = new ObservableCollection<LeadListModel>();

        private bool _isCheckedAll;
        public bool isCheckedAll { get => _isCheckedAll; set { _isCheckedAll = value; OnPropertyChanged(nameof(isCheckedAll)); } }

        private int _numberChecked;
        public int numberChecked { get => _numberChecked; set { _numberChecked = value; totalChecked = value.ToString() + "/" + total.ToString(); OnPropertyChanged(nameof(numberChecked)); } }

        private int _total;
        public int total { get => _total; set { _total = value; totalChecked = numberChecked.ToString() + "/" + value.ToString(); OnPropertyChanged(nameof(total)); } }

        private string _totalChecked;
        public string totalChecked { get => _totalChecked; set { _totalChecked = Language.da_chon + " " + value; OnPropertyChanged(nameof(totalChecked)); } }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private bool _isLoadingMore = false;
        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged(nameof(IsLoadingMore));
            }
        }
        public int totalConactActive { get; set; }
        public DanhBaViewModel()
        {
            Contacts = new ObservableCollection<DanhBaItemModel>();
            isCheckedAll = false;
            numberChecked = 0;
            total = 0;
        }

        public ICommand RefreshCommand
        {
            get => new Command(async () =>
            {
                IsRefreshing = true;
                await LoadOnRefreshCommandAsync();
                IsRefreshing = false;
            });
        }

        public void reset()
        {
            Contacts.Clear();
            isCheckedAll = false;
            numberChecked = 0;
            total = 0;
            totalConactActive = 0;
        }

        public async Task LoadLeadConvert()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='lead'>
                        <attribute name='lastname' />
                        <attribute name='subject' />
                        <attribute name='mobilephone'/>
                        <attribute name='emailaddress1' />
                        <attribute name='createdon' />
                        <attribute name='leadid' />
                        <attribute name='leadqualitycode' />
                        <order attribute='createdon' descending='true' />                      
                      </entity>
                    </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadListModel>>("leads", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                LeadConvert.Add(item);
            }
        }
        public virtual async Task LoadOnRefreshCommandAsync()
        {
            if (Contacts != null && Contacts.Count > 0)
            {
                Contacts.Clear();
                await LoadContacts();
            }
        }
        public async Task LoadContacts()
        {
            PermissionStatus RequestContactsRead = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (!Plugin.ContactService.CrossContactService.IsSupported)
            {
                ToastMessageHelper.ShortMessage(":( Permission not granted to contact.");
                return;
            }
            if (RequestContactsRead != PermissionStatus.Granted)
            {
                RequestContactsRead = await Permissions.RequestAsync<Permissions.ContactsRead>();
            }
            if (RequestContactsRead == PermissionStatus.Granted)
            {
                await LoadLeadConvert();
                var contacts = (await Plugin.ContactService.CrossContactService.Current.GetContactListAsync()).Where(x => x.Name != null);
                reset();
                foreach (var tmp in contacts.OrderBy(x => x.Name))
                {
                    var numbers = tmp.Numbers;
                    foreach (var n in numbers)
                    {
                        var sdt = n.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

                        var item = new Models.DanhBaItemModel
                        {
                            Name = tmp.Name,
                            numberFormated = sdt,
                            IsSelected = false
                        };
                        if (LeadConvert.Where(x => x.mobilephone.Contains(sdt) == true).ToList().Count <= 0)
                        {
                            item.IsConvertToLead = false;
                            totalConactActive++;
                        }
                        else
                        {
                            item.IsConvertToLead = true;
                        }
                        Contacts.Add(item);
                    }
                }
                total = Contacts.Count();
            }
        }
    }
}
