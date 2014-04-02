using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PWCrackService.util;

namespace PWCrackService
{
    public class Cracking
    {
        /// <summary>
        /// The algorithm used for encryption.
        /// Must be exactly the same algorithm that was used to encrypt the passwords in the password file
        /// </summary>
        private readonly HashAlgorithm _messageDigest;

        public Cracking()
        {
            _messageDigest = new SHA1CryptoServiceProvider();
        }

        /// <summary>
        /// Runs the password cracking algorithm
        /// </summary>
        public List<UserInfoClearText> RunCracking(List<string> words, List<UserInfo> userInfos)
        {
            var result = new List<UserInfoClearText>();
            var list = new List<List<string>>();
            var size = words.Count/Environment.ProcessorCount;

            for (var i = 0; i < words.Count; i += size)
            {
                list.Add(words.GetRange(i, Math.Min(size, words.Count - i)));
            }

            Parallel.ForEach(list, currentList =>
            {
                foreach (var word in currentList)
                {
                    var partialResult = CheckWordWithVariations(word, userInfos);
                    result.AddRange(partialResult);
                }
            });
            return result;
        }

        /// <summary>
        /// Generates a lot of variations, encrypts each of the and compares it to all entries in the password file
        /// </summary>
        /// <param name="dictionaryEntry">A single word from the dictionary</param>
        /// <param name="userInfos">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>
        private IEnumerable<UserInfoClearText> CheckWordWithVariations(String dictionaryEntry, List<UserInfo> userInfos)
        {
            var result = new List<UserInfoClearText>();

            var possiblePassword = dictionaryEntry;
            var partialResult = CheckSingleWord(userInfos, possiblePassword);
            result.AddRange(partialResult);

            var possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            var partialResultUpperCase = CheckSingleWord(userInfos, possiblePasswordUpperCase);
            result.AddRange(partialResultUpperCase);

            var possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            var partialResultCapitalized = CheckSingleWord(userInfos, possiblePasswordCapitalized);
            result.AddRange(partialResultCapitalized);

            var possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            var partialResultReverse = CheckSingleWord(userInfos, possiblePasswordReverse);
            result.AddRange(partialResultReverse);

            for (var i = 0; i < 100; i++)
            {
                var possiblePasswordEndDigit = dictionaryEntry + i;
                var partialResultEndDigit = CheckSingleWord(userInfos, possiblePasswordEndDigit);
                result.AddRange(partialResultEndDigit);
            }

            for (var i = 0; i < 100; i++)
            {
                var possiblePasswordStartDigit = i + dictionaryEntry;
                var partialResultStartDigit = CheckSingleWord(userInfos, possiblePasswordStartDigit);
                result.AddRange(partialResultStartDigit);
            }

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    var possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    var partialResultStartEndDigit = CheckSingleWord(userInfos, possiblePasswordStartEndDigit);
                    result.AddRange(partialResultStartEndDigit);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks a single word (or rather a variation of a word): Encrypts and compares to all entries in the password file
        /// </summary>
        /// <param name="userInfos"></param>
        /// <param name="possiblePassword">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>
        private IEnumerable<UserInfoClearText> CheckSingleWord(IEnumerable<UserInfo> userInfos, String possiblePassword)
        {
            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());
            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);

            var results = new List<UserInfoClearText>();
            foreach (var userInfo in userInfos)
            {
                if (CompareBytes(userInfo.EntryptedPassword, encryptedPassword))
                {
                    results.Add(new UserInfoClearText(userInfo.Username, possiblePassword));
                }
            }
            return results;
        }

        /// <summary>
        /// Compares to byte arrays. Encrypted words are byte arrays
        /// </summary>
        /// <param name="firstArray"></param>
        /// <param name="secondArray"></param>
        /// <returns></returns>
        private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray)
        {
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("firstArray");
            //}
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("secondArray");
            //}
            if (firstArray.Count != secondArray.Count)
            {
                return false;
            }
            for (var i = 0; i < firstArray.Count; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }
            return true;
        }

    }
}