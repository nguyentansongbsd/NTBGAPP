using System;
using System.Collections.ObjectModel;
using System.Linq;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class PhongThuyModel : BaseViewModel
    {
        private ObservableCollection<string> lst_menh;
        private ObservableCollection<ObservableCollection<int>> lst_nhagiap_menh;
        private ObservableCollection<ObservableCollection<string>> lst_nguhanhnapam;
        private ObservableCollection<ObservableCollection<int>> lst_quemenh_gioitinh;
        private ObservableCollection<ObservableCollection<int>> lst_huong_theo_quemenh;

        private ObservableCollection<string> lst_quemenh;
        private ObservableCollection<string> lst_can;
        private ObservableCollection<string> lst_chi;
        private ObservableCollection<string> lst_tumenh;
        private ObservableCollection<string> lst_huong;
        private ObservableCollection<string> lst_ketqua_huong;
        private ObservableCollection<string> lst_image;

        private int can_value;
        private int chi_value;
        private int nha_giap_value;
        private string sinh_khi;
        private string thien_y;
        private string dien_nien;
        private string phuc_vi;
        private string tuyet_menh;
        private string ngu_quy;
        private string luc_sat;
        private string hoa_hai;

        private int _gioi_tinh;
        public int gioi_tinh { get => _gioi_tinh; set { _gioi_tinh = value;  OnPropertyChanged(nameof(gioi_tinh)); } }

        private int _ngay_sinh;
        public int ngay_sinh { get => _ngay_sinh; set { _ngay_sinh = value; OnPropertyChanged(nameof(ngay_sinh)); } }

        private int _thang_sinh;
        public int thang_sinh { get => _thang_sinh; set { _thang_sinh = value; OnPropertyChanged(nameof(thang_sinh)); } }
        
        private int _nam_sinh;
        public int nam_sinh { get => _nam_sinh; set { _nam_sinh = value; OnPropertyChanged(nameof(nam_sinh)); 
               if(value != 0)
                {
                    #region Ten nam sinh am lich
                    can_value = value % 10;
                    //var chi_value = (value % 100)% 12;
                    var chi_value = value % 12;
                    string can_label = lst_can[can_value];
                    string chi_label = lst_chi[chi_value];
                    ten_nam_sinh_am_lich = can_label + " " + chi_label;
                    #endregion

                    #region Ngu hanh nap am
                    var balance = (can_value < 4) ? (can_value + 10) - 4 : can_value - 4;
                    var chi_giap = (nam_sinh - balance) % 12;
                    int index_menh = balance % 2 == 0 ? balance / 2 : (balance - 1) / 2;
                    if (chi_giap == 4 || chi_giap == 10)
                    {
                        menhnapam_label = chi_giap == 4 ? lst_nguhanhnapam[0][index_menh] : lst_nguhanhnapam[1][index_menh];
                        menh_value = lst_nhagiap_menh[0][index_menh];
                    }
                    if (chi_giap == 8 || chi_giap == 2)
                    {
                        menhnapam_label = chi_giap == 8 ? lst_nguhanhnapam[2][index_menh] : lst_nguhanhnapam[3][index_menh];
                        menh_value = lst_nhagiap_menh[1][index_menh];
                    }
                    if (chi_giap == 6 || chi_giap == 0)
                    {
                        menhnapam_label = chi_giap == 6 ? lst_nguhanhnapam[4][index_menh] : lst_nguhanhnapam[5][index_menh];
                        menh_value = lst_nhagiap_menh[2][index_menh];
                    }
                    menh_label = lst_menh[menh_value] + ", " + menhnapam_label;
                    #endregion

                    if(gioi_tinh == 1 || gioi_tinh == 2)
                    {
                        #region Que menh va Huong tot - Huong xau
                        int hang_donvi = nam_sinh % 10;
                        int hang_chuc = (nam_sinh / 10) % 10;
                        int hang_tram = (nam_sinh / 100) % 10;
                        int hang_nghin = (nam_sinh / 1000) % 10;
                        var tmp_quemenh = (hang_donvi + hang_chuc + hang_tram + hang_nghin) % 9;

                        if (gioi_tinh == 1)
                        {
                            quemenh_value = lst_quemenh_gioitinh[0][tmp_quemenh];
                        }
                        else if (gioi_tinh == 2)
                        {
                            quemenh_value = lst_quemenh_gioitinh[1][tmp_quemenh];
                        }

                        image = lst_image[quemenh_value]; 

                        var lst_huong_tmp = lst_huong_theo_quemenh[quemenh_value];
                        var huong1 = lst_huong_tmp.IndexOf(0);
                        var huong2 = lst_huong_tmp.IndexOf(1);
                        var huong3 = lst_huong_tmp.IndexOf(2);
                        var huong4 = lst_huong_tmp.IndexOf(6);
                        var huong5 = lst_huong_tmp.IndexOf(3);
                        var huong6 = lst_huong_tmp.IndexOf(5);
                        var huong7 = lst_huong_tmp.IndexOf(7);
                        var huong8 = lst_huong_tmp.IndexOf(4);

                        if (quemenh_value == 0 || quemenh_value == 1 || quemenh_value == 2 || quemenh_value == 3)
                        {
                            quemenh_label = lst_quemenh[quemenh_value] + " (" + lst_tumenh[1] + ")";

                            huong_tot = lst_huong[3] + " - " + lst_ketqua_huong[huong5] + "\n"
                                + lst_huong[5] + " - " + lst_ketqua_huong[huong6] + "\n"
                                + lst_huong[7] + " - " + lst_ketqua_huong[huong7] + "\n"
                                + lst_huong[4] + " - " + lst_ketqua_huong[huong8];

                            huong_xau = lst_huong[0] + " - " + lst_ketqua_huong[huong1] + "\n"
                                + lst_huong[1] + " - " + lst_ketqua_huong[huong2] + "\n"
                                + lst_huong[2] + " - " + lst_ketqua_huong[huong3] + "\n"
                                + lst_huong[6] + " - " + lst_ketqua_huong[huong4];

                        }
                        else if (quemenh_value == 4 || quemenh_value == 5 || quemenh_value == 6 || quemenh_value == 7)
                        {
                            quemenh_label = lst_quemenh[quemenh_value] + " (" + lst_tumenh[0] + ")";

                            huong_xau = lst_huong[3] + " - " + lst_ketqua_huong[huong5] + "\n"
                                + lst_huong[5] + " - " + lst_ketqua_huong[huong6] + "\n"
                                + lst_huong[7] + " - " + lst_ketqua_huong[huong7] + "\n"
                                + lst_huong[4] + " - " + lst_ketqua_huong[huong8];

                            huong_tot = lst_huong[0] + " - " + lst_ketqua_huong[huong1] + "\n"
                                + lst_huong[1] + " - " + lst_ketqua_huong[huong2] + "\n"
                                + lst_huong[2] + " - " + lst_ketqua_huong[huong3] + "\n"
                                + lst_huong[6] + " - " + lst_ketqua_huong[huong4];
                        }
                        #endregion
                    }
                    else
                    {
                        quemenh_label = null;
                        huong_tot = null;
                        huong_xau = null;
                        image = null;
                    }
                }
            } }

        private string _ten_nam_sinh_am_lich;
        public string ten_nam_sinh_am_lich { get => _ten_nam_sinh_am_lich; set { _ten_nam_sinh_am_lich = value;OnPropertyChanged(nameof(ten_nam_sinh_am_lich)); } }

        private int _menh_value;
        public int menh_value { get => _menh_value; set { _menh_value = value; OnPropertyChanged(nameof(menh_value)); } }

        private string _menh_label;
        public string menh_label { get => _menh_label;set { _menh_label = value;OnPropertyChanged(nameof(menh_label)); } }

        private int _menhnapam_value;
        public int menhnapam_value { get => _menhnapam_value;set { _menhnapam_value = value; OnPropertyChanged(nameof(menhnapam_value)); } }

        private string _menhnapam_labe;
        public string menhnapam_label { get => _menhnapam_labe;set { _menhnapam_labe = value; OnPropertyChanged(nameof(menhnapam_label)); } }
        
        private int _quemenh_value;
        public int quemenh_value { get => _quemenh_value; set { _quemenh_value = value; OnPropertyChanged(nameof(quemenh_value)); } }

        private string _quemenh_label;
        public string quemenh_label { get => _quemenh_label; set { _quemenh_label = value; OnPropertyChanged(nameof(quemenh_label)); } }
        
        private string _huong_tot;
        public string huong_tot { get => _huong_tot; set { _huong_tot = value; OnPropertyChanged(nameof(huong_tot)); } }

        private string _huong_xau;
        public string huong_xau { get => _huong_xau; set { _huong_xau = value; OnPropertyChanged(nameof(huong_xau)); } }

        private string _image;
        public string image { get => _image; set { _image = value; OnPropertyChanged(nameof(image)); } }

        public PhongThuyModel()
        {
            //Danh sach chi
          //  lst_chi = new ObservableCollection<string>() { "Tý","Sửu","Dần","Mão","Thìn","Tỵ","Ngọ","Mùi","Thân","Dậu","Tuất","Hợi"};
            lst_chi = new ObservableCollection<string>() { "Thân", "Dậu", "Tuất", "Hợi", "Tý", "Sửu", "Dần", "Mão", "Thìn", "Tỵ", "Ngọ", "Mùi" };

            //Danh sach can
            lst_can = new ObservableCollection<string>() { "Canh","Tân","Nhâm","Quý","Giáp","Ất","Bính","Đinh","Mậu","Kỷ"};

            ///Danh sach cac menh
            lst_menh = new ObservableCollection<string>() {"Kim","Mộc","Thuỷ","Hoả","Thổ"};

            //Danh sach huong
            lst_huong = new ObservableCollection<string>() { "Bắc","Nam","Đông","Tây","Đông Bắc","Tây Bắc","Đông Nam","Tây Nam"};

            ///Danh sách thứ tự mệnh của từng nhà giáp
            /// 3 nhà giáp (Tý-Ngọ,Thìn-Tuất,Dần-Thân)
            /// Số mệnh trong mỗi gìa giáp tương ứng với thứ tự mệnh trong danh sách các mệnh
            lst_nhagiap_menh = new ObservableCollection<ObservableCollection<int>>();
            lst_nhagiap_menh.Add(new ObservableCollection<int>() { 0, 3, 1, 4, 0 }); // Nhà giáp Tý-Ngọ
            lst_nhagiap_menh.Add(new ObservableCollection<int>() { 3, 2, 4, 0, 1 }); // Nhà giáp Thìn-Tuất
            lst_nhagiap_menh.Add(new ObservableCollection<int>() { 2, 4, 3, 1, 2 }); // Nhà giáp Dần-Thân

            ///Danh sach ngu hanh nap am
            lst_nguhanhnapam = new ObservableCollection<ObservableCollection<string>>();

            ///Nap am nha giap Ty
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Hải Trung Kim - Vàng trong biển", 
                "Lô Trung Hỏa - Lửa trong lò", 
                "Đại Lâm Mộc - Gỗ trong rừng", 
                "Lộ Bàng Thổ - Đất ven đường", 
                "Kiếm Phong Kim - Sắt đầu kiếm" 
            });

            ///Nap am nha giap Ngo
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Sa Trung Kim - Vàng trong cát", 
                "Sơn Họa Hỏa - Lửa chân núi",
                "Bình Địa Mộc - Cây đất bằng",
                "Bích Thượng Thổ - Đất trên vách",
                "Kim Bạch Kim - Vàng pha bạc"
            });

            ///Nap am nha giap Thin
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Phúc Đăng Hỏa - Lửa đèn lớn",
                "Thiên Hà Thủy - Nước sông trời",
                "Đại Dịch Thổ - Đất rộng lớn",
                "Thoa Xuyến Kim - Vàng trang sức",
                "Tang Đố Mộc - Gỗ cây dâu"
            });

            ///Nap am nha giap Tuat
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Sơn Đầu Hỏa - Lửa đỉnh núi", 
                "Giản Hạ Thủy - Nước dưới suối",
                "Thành Đầu Thổ - Đất đầu thành",
                "Bạch Lạp Kim - Vàng trong nến",
                "Dương Liễu Mộc - Gỗ dương liễu"
            });

            ///Nap am nha giap Dan
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Đại Khuê Thủy - Nước khe lớn",
                "Sa Trung Thổ - Đất trong cát",
                "Thiên Thượng Hỏa - Lửa trên trời",
                "Thạch Lựu Mộc - Gỗ thạch lựu",
                "Đại Hải Thủy - Nước biển lớn"
            });

            ///Nap am nha giap Than
            lst_nguhanhnapam.Add(new ObservableCollection<string>()
            {
                "Tinh Tuyền Thủy - Nước trong giếng",
                "Ốc Thượng Thổ - Đất mái nhà",
                "Tích Lịch Hỏa - Lửa sấm sét",
                "Tùng Bách Mộc - Gỗ tùng bách",
                "Trường Lưu Thủy - Nước suối lớn"
            });

            ///Danh sach que menh
            lst_quemenh = new ObservableCollection<string>(){ "Càn", "Đoài", "Cấn", "Khôn", "Ly", "Khảm", "Tốn", "Chấn" };
            lst_image = new ObservableCollection<string>() { "bsd_can.png","bsd_doai.png","bsd_caan.png","bsd_khon.png","bsd_ly.png","bsd_kham.png","bsd_ton.png","bsd_chan.png"};
            ///Danh sach huong theo que menh (thu tu huong ung voi sinh_khi, thien_y,dien_nien.phuc_vi,tuyet_menh,ngu_quy,luc_sat,hoa_hai)
            lst_huong_theo_quemenh = new ObservableCollection<ObservableCollection<int>>();
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 3,4,7,5,1,2,0,6});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 5,7,4,3,2,1,6,0});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 7,5,3,4,6,0,2,1});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 4,3,5,7,0,6,1,2}); 
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 2,6,0,1,5,3,7,4});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 6,2,1,0,7,4,5,3});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 0,1,2,6,4,7,3,5});
            lst_huong_theo_quemenh.Add(new ObservableCollection<int>() { 1,0,6,2,3,5,4,7});

            lst_quemenh_gioitinh = new ObservableCollection<ObservableCollection<int>>();
            ////Que menh nam
            lst_quemenh_gioitinh.Add(new ObservableCollection<int>() { 3, 5, 4, 2, 1, 0, 3, 6, 7 });
            ////Que menh nu
            lst_quemenh_gioitinh.Add(new ObservableCollection<int>() { 6, 2, 0, 1, 2, 4, 5, 3, 7 });

            ///Danh sách tứ mệnh
            lst_tumenh = new ObservableCollection<string>() { "Đông Tứ Mệnh","Tây Tứ Mệnh"};

            lst_ketqua_huong = new ObservableCollection<string>();
            sinh_khi = "Sinh Khí: Phúc lộc vẹn toàn, đường con cái thuận lợi, nhà quay về hướng sinh khí của chủ nhà là ngôi nhà ấm áp, là tổ ấm của mọi thành viên trong gia đình.";
            thien_y = "Thiên Y: Được thiên thời che chở, giải bệnh dễ dàng, nhanh chóng tai qua nạn khỏi.";
            dien_nien = "Diên Niên: Mọi sự ổn định, gặp nhiều may mắn,dễ gặp quý nhân phù trợ, có phúc có đức có hậu vận tốt, con cái thành đạt.";
            phuc_vi = "Phục Vị: Nhàn hạ, ít lao động chân tay, công danh sự nghiệp ổn định.";
            tuyet_menh = "Tuyệt Mệnh: Gia đình dễ chia ly về tình cảm, đổ vỡ, người trong nhà phải tự bươn chải, ít có sự giúp đõ từ mọi phía.";
            ngu_quy = "Ngũ Quỷ: Dễ gặp tai họa, kẻ xấu quấy phá, bị cản trở đường sự nghiệp, nhà có hướng về hướng này dễ bực bội khó chịu, mệt mõi về sức khỏe, bất hòa trong gia đình, không may mắn trong sự nghiệp.";
            luc_sat = "Lục Sát: Nhà có sát khí, có sự thiệt hại về người và của, sinh nở có thể xảy thai hoặc trẻ sơ sinh khó nuôi, nhà có thể có người chết trẻ.";
            hoa_hai = "Họa Hại: Hay gặp tai nạn bất ngờ, trong nhà thường có người có bệnh tật bẩm sinh, mãn tính, hoặc nan y.";

            lst_ketqua_huong.Add(sinh_khi);
            lst_ketqua_huong.Add(thien_y);
            lst_ketqua_huong.Add(dien_nien);
            lst_ketqua_huong.Add(phuc_vi);
            lst_ketqua_huong.Add(tuyet_menh);
            lst_ketqua_huong.Add(ngu_quy);
            lst_ketqua_huong.Add(luc_sat);
            lst_ketqua_huong.Add(hoa_hai);
        }
    }
}
