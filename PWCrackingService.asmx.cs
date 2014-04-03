using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PWCrackService
{
    /// <summary>
    /// Summary description for PWCrackingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PWCrackingService : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] Crack(string[] words)
        {
            var wordsList = words.ToList();
            var userInfos = (List<UserInfo>)Application["UserInfos"];

            var cracker = new Cracking();
            var result = cracker.RunCracking(wordsList, userInfos);

            var resultArray = new string[result.Count];
            for (var i = 0; i < result.Count; i++)
            {
                resultArray[i] = result[i].UserName + ": " + result[i].Password;
            }
            return resultArray;
        }

        [WebMethod]
        public void GiveUserInfo(string[] userInfos)
        {
            var userInfoList = new List<UserInfo>();
            foreach (var line in userInfos)
            {
                if (line != "")
                {
                    var parts = line.Split(':');
                    var userInfo = new UserInfo(parts[0], parts[1]);
                    userInfoList.Add(userInfo);
                }
            }
            Application["UserInfos"] = userInfoList;
        }
    }
}
