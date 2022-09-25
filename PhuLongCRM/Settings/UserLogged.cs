using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Essentials;

namespace PhuLongCRM.Settings
{
    public class UserLogged
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static Guid Id
        {
            get => AppSettings.GetValueOrDefault(nameof(Id), Guid.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Id), value);
        }

        public static string Language
        {
            get => AppSettings.GetValueOrDefault(nameof(Language), "vi"); //en
            set => AppSettings.AddOrUpdateValue(nameof(Language), value);
        }

        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(AccessToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AccessToken), value);
        }

        public static string RefreshToken
        {
            get => AppSettings.GetValueOrDefault(nameof(RefreshToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(RefreshToken), value);
        }

        public static string AccessTokenSharePoint
        {
            get => AppSettings.GetValueOrDefault(nameof(AccessTokenSharePoint), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AccessTokenSharePoint), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static string Avartar
        {
            get => AppSettings.GetValueOrDefault(nameof(Avartar), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Avartar), value);
        }
        public static string User
        {
            get => AppSettings.GetValueOrDefault(nameof(User), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(User), value);
        }
        public static bool IsLogged
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLogged), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLogged), value);
        }

        public static Guid ContactId
        {
            get => AppSettings.GetValueOrDefault(nameof(ContactId), Guid.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ContactId), value);
        }

        public static string ContactName
        {
            get => AppSettings.GetValueOrDefault(nameof(ContactName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ContactName), value);
        }

        public static Guid ManagerId
        {
            get => AppSettings.GetValueOrDefault(nameof(ManagerId), Guid.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ManagerId), value);
        }

        public static string ManagerName
        {
            get => AppSettings.GetValueOrDefault(nameof(ManagerName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ManagerName), value);
        }

        public static bool IsSaveInforUser
        {
            get => AppSettings.GetValueOrDefault(nameof(IsSaveInforUser), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsSaveInforUser), value);
        }
        public static bool IsLoginByUserCRM
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLoginByUserCRM), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLoginByUserCRM), value);
        }
        public static string UserAttribute
        {
            get => AppSettings.GetValueOrDefault(nameof(UserAttribute), null);
            set => AppSettings.AddOrUpdateValue(nameof(UserAttribute), value);
        }
        public static string UserCRM
        {
            get => AppSettings.GetValueOrDefault(nameof(UserCRM), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserCRM), value);
        }
    }
}
