using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public  class MD5
    {
       /// <summary>
       /// MD5
       /// </summary>
       /// <param name="input">加密字符串</param>
       /// <returns></returns>
       public static string ToMD5(string input)
       {
           //加盐
           string md5Key = "mv/?!%123";
           using (var md5Provider = new MD5CryptoServiceProvider())
           {
               var bytes = Encoding.UTF8.GetBytes(input+md5Key);
               var hash = md5Provider.ComputeHash(bytes);
               var count = hash.Length;
               hash[0] = (byte)(hash[3] + (hash[3] = hash[0]) * 0); //交换0,3的值
               hash[1] = (byte)(hash[2] + (hash[2] = hash[1]) * 0); //交换1,2的值
               hash[5] = (byte)(hash[4] + (hash[4] = hash[5]) * 0); //交换4,5的值
               hash[7] = (byte)(hash[6] + (hash[6] = hash[7]) * 0); //交换6,7的值
               return BitConverter.ToString(hash).Replace("-","");
           }
       }
    }
}
