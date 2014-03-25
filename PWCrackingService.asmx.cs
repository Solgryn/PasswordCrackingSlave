using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PWCrackService.Models;

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
        public string Crack(List<string> words, List<UserInfo> userInfos)
        {
            return "Hello World";
        }
    }
}
