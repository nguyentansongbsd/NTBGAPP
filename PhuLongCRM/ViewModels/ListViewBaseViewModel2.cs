using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace PhuLongCRM.ViewModels
{
    public class ListViewBaseViewModel2<TEntity> : BaseViewModel where TEntity : class
    {
        public bool IsAuthenticate { get; set; } = false;
        public string Url { get; set; } // bao gom {0}
        public string EntityName { get; set; }
        public string FetchXml { get; set; }
        public int? RecordPerPage { get; set; } // de kiem tra du lieu tra ve co = recordperpage tren api khong, neu nho hon thi set out of data
        public InfiniteScrollCollection<TEntity> Data { get; set; }

        public Action ConnectFail;

        public bool UseConnectFailtNotificationDefault { get; set; }

        public string ApiUrl { get; set; }
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

        public bool OutOfData { get; set; } = false;

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

        private int _page;
        public int Page
        {
            get => _page;
            set
            {
                if (_page != value)
                {
                    _page = value;
                    OnPropertyChanged(nameof(Page));
                }
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmptyList
        {
            get => Data.Count == 0;
        }

        public ICommand PreLoadData { get; set; }

        public Command<TEntity> OnMapItem { get; set; }

        public ICommand RefreshCommand
        {
            get => new Command(async () =>
            {
                IsRefreshing = true;
                await LoadOnRefreshCommandAsync();
                IsRefreshing = false;
            });
        }

        public ListViewBaseViewModel2()
        {
            Data = new InfiniteScrollCollection<TEntity>
            {
                OnLoadMore = async () =>
                {
                    _page += 1;
                    return await this.LoadItems();
                },
                OnCanLoadMore = () => OutOfData == false
            };
            _page = 1;
        }

        public async Task LoadData()
        {
            if (this._page == 1)
            {
                this.Data.Clear();
            }
            var items = await LoadItems();
            this.Data.AddRange(items);
            OnPropertyChanged(nameof(IsEmptyList));
        }

        public virtual async Task<List<TEntity>> LoadItems()
        {
            PreLoadData.Execute(null);
            var items = new List<TEntity>();

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TEntity>>(EntityName, FetchXml);

            if (result != null)
            {
                var list = (List<TEntity>)result.value;
                var count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        if (OnMapItem != null)
                        {
                            OnMapItem.Execute(item);
                        }
                        items.Add(item);
                    }

                    if (RecordPerPage.HasValue)
                    {
                        if (count < RecordPerPage)
                        {
                            OutOfData = true;
                        }
                    }
                }
                else
                {
                    OutOfData = true;
                }
            }
            else
            {
                //OutOfData = false;
                //Data.Clear();
                //_page = 1;

                //if (UseConnectFailtNotificationDefault == true)
                //{
                //    ConnectFail = async () => Helpers.ToastMessageHelper.ShortMessage("mat ket noi");
                //    ConnectFail();
                //}
                //else
                //{
                //    ConnectFail?.Invoke();
                //}
            }
            return items;
        }

        public virtual async Task LoadOnRefreshCommandAsync()
        {
            _page = 1;
            OutOfData = false;
            await LoadData();
        }
    }
}
