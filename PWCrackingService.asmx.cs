using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Services;
using log4net;
using log4net.Config;

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
        private readonly ILog log;

        public PWCrackingService()
        {
            XmlConfigurator.Configure(new FileInfo(HostingEnvironment.ApplicationPhysicalPath + @"logconfig.xml"));
            log = LogManager.GetLogger(typeof(PWCrackingService));
            log.Debug("Service started.");
        }

        [WebMethod]
        public string[] Crack(string[] words)
        {
            log.Debug("Crack");
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
            log.Debug("GiveUserInfo");
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
