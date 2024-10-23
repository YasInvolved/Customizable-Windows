using Customizable_Windows.CLI;
using Customizable_Windows.UUP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Customizable_Windows
{
    public class Constants
    {
        public static readonly string UUPDUMP_JSONAPI = "https://api.uupdump.net"; // uupdump json api
        public static readonly List<UUP.Version> VERSIONS_11 = new List<UUP.Version>
        {
            new UUP.Version("23H2", "e93eb708-98ec-494c-b4fc-025c6b012173"),
            new UUP.Version("22H2", "e7036acd-c5d4-46e5-bce0-ccb55d8dbf56"),
            new UUP.Version("21H2", "35630b7b-4509-45b6-83a1-d4c75d5aa9b6")

        };

        public static readonly List<UUP.Version> VERSIONS_10 = new List<UUP.Version>
        {
            new UUP.Version("22H2", "60d84f07-7bce-4652-a0cd-24608a0cd0fc"),
            new UUP.Version("21H2", "f548dcf7-683b-42f6-828a-9f9d2ff146af"),
            new UUP.Version("1809", "41a574f2-b782-4e2b-92a4-f79c120fb017")
        };
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TextUtils.Format("Welcome to Windows Customizer v1.0a", TextColor.WHITE, TextColor.BLACK, TextFormat.BOLD, TextFormat.UNDERLINE));
            Thread.Sleep(3000);
            Console.Clear();

            Form<UUP.Version> versionForm = new Form<UUP.Version>("Please choose desired version (it's recommended to pick the latest one)", Constants.VERSIONS_11);
            UUP.Version selectedVersion = versionForm.Show();

            var languages = Language.GetLanguages(selectedVersion.UUID);
            languages.Wait();

            foreach (var language in languages.Result)
            {
                Console.WriteLine($"{language}");
            }
        }
    }
}
