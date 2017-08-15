using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.StreamExt;

namespace Dot.Net.DevFast.Sample
{
    public static class TransformSample
    {
        public static void Run()
        {
            Console.Out.WriteLine("---------------------------------------------------");
            Console.Out.WriteLine("------------- Running TransformSample -------------");
            Console.Out.WriteLine("---------------------------------------------------");

            const string original = "Here is some d@t@ to encrypt!";
            using (var myRijndael = new RijndaelManaged())
            {
                myRijndael.GenerateKey();
                myRijndael.GenerateIV();

                //We try to do Round trip Rijndael encryption on "original" string
                var roundtrip = RijndaelRoundTrip(original, myRijndael).Result;
                Console.WriteLine("Original:   {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);//prints same string
                Console.WriteLine(roundtrip.Equals(original));//prints true
            }
        }

        private static async Task<string> RijndaelRoundTrip(string plainText, SymmetricAlgorithm cryptoAlgo)
        {
            using (var msEncrypt = new MemoryStream())
            {
                await plainText.TransformAsync(cryptoAlgo.CreateEncryptor(), msEncrypt,
                    CancellationToken.None).ConfigureAwait(false);

                msEncrypt.Seek(0, SeekOrigin.Begin);
                var sb = new StringBuilder();

                await msEncrypt.TransformAsync(cryptoAlgo.CreateDecryptor(), sb,
                    CancellationToken.None).ConfigureAwait(false);
                return sb.ToString();
            }
        }
    }
}