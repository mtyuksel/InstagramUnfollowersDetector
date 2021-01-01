
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramUnfollowers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            InstagramManager.GetUser();

            var api = await InstagramManager.Login();
            if (api != null)
            {
                await InstagramManager.GetUnfollowers(api);
            }
            else
            {
                Console.WriteLine("Bir hata oluştu.");
            }

            Console.Read();
        }
    }
}
