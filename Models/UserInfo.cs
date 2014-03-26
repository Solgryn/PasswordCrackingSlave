using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWCrackService.Models
{
    [Serializable]
    public class UserInfo
    {
        public String Username { get; set; }
        public String EntryptedPasswordBase64 { get; set; }
        public byte[] EntryptedPassword { get; set; }

        public UserInfo() : this("", "")
        {
        }

        public UserInfo(String username, String entryptedPasswordBase64)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (entryptedPasswordBase64 == null)
            {
                throw new ArgumentNullException("entryptedPasswordBase64");
            }
            Username = username;
            EntryptedPasswordBase64 = entryptedPasswordBase64;
            EntryptedPassword = Convert.FromBase64String(entryptedPasswordBase64);
        }

        public override string ToString()
        {
            return Username + ":" + EntryptedPasswordBase64;
        }
    }
}