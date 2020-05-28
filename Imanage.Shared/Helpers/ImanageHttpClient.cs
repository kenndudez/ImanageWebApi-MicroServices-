using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Imanage.Shared.Helpers
{
    public class ImanageHttpClient
    {
        public HttpClient Client { get; }
        public ImanageHttpClient(HttpClient client)
        {
            Client = client;
        }

        public static string VerifyPefUserURL(string env) => 
            env == "Development" ? $"/api/UserMgmt/VerifyPEFUser" 
                : $"/dafmis-auth/api/UserMgmt/VerifyPEFUser";

        public static string VerifyMarketerURL (string env) =>
            env == "Development" ? $"/api/Marketer/getmarketer/"
                : $"/dafmis-marketers/api/Marketer/getmarketer/";

        public static string CreateMarketerUserURL(string env) =>
            env == "Development" ? $"/api/UserMgmt/CreateMarketerUser"
                : $"/dafmis-auth/api/UserMgmt/CreateMarketerUser";
    }
}
