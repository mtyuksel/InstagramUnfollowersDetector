using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;

namespace InstagramUnfollowers
{
    class InstagramManager
    {
        private static UserSessionData user;
        public static UserSessionData GetUser()
        {

            Console.WriteLine("Kullanıcı Adı: ");
            string _username = Console.ReadLine();
            Console.WriteLine("Şifre: ");
            string _password = Console.ReadLine();

            user = new UserSessionData();
            user.UserName = _username;
            user.Password = _password;

            return user;
        }

        public static async Task<IInstaApi> Login()
        {
            IInstaApi api = InstaApiBuilder.CreateBuilder()
                            .SetUser(user)
                            .UseLogger(new DebugLogger(LogLevel.Exceptions))
                            .SetRequestDelay(RequestDelay.FromSeconds(8, 8))
                            .Build();

            var loginRequest = await api.LoginAsync();
            if (loginRequest.Succeeded)
            {
                Console.WriteLine("Logged başarılı!");
                Console.WriteLine("Yükleniyor, lütfen bekleyiniz.");
                return api;
            }
            else
            {
                Console.WriteLine("Error: " + loginRequest.Info.Message);
                return null;
            }
        }

        public static async Task GetUnfollowers(IInstaApi api)
        {
            var followersList = await GetFollowersList(api);
            var followingList = await GetFollowingList(api);

            HashSet<string> followerUsernameList = new HashSet<string>(followersList.Select(s => s.UserName));

            var results = followingList.Where(m => !followerUsernameList.Contains(m.UserName));

            foreach (var user in results)
            {
                Console.WriteLine(user.UserName);
            }
        }
        public static async Task<InstaUserShortList> GetFollowingList(IInstaApi api)
        {
            var following = await api.GetUserFollowingAsync(user.UserName, PaginationParameters.Empty);
            var followingList = following.Value;
            return followingList;
        }

        public static async Task<InstaUserShortList> GetFollowersList(IInstaApi api)
        {
            var followers = await api.GetUserFollowersAsync(user.UserName, PaginationParameters.Empty);
            var followersList = followers.Value;
            return followersList;
        }
    }
}
