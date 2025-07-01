using System.Text;

namespace OEngineResourceReader.Utils
{
    public class ResourceNameDecryptor
    {
        // this keys from Ravenswatch.v1.02.02.00
        private static readonly byte[] UppercaseMap = "HBYVJSLMKUGREDOQTWCPNFAIZX"u8.ToArray();
        private static readonly byte[] LowercaseMap = "bcjizqaftpvlkhwxeoydrsumgn"u8.ToArray();

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            StringBuilder decrypted = new StringBuilder(encryptedText.Length);
            foreach (char c in encryptedText)
            {
                if (c >= 'a' && c <= 'z')
                {
                    int index = c - 'a';
                    if (index < LowercaseMap.Length)
                    {
                        decrypted.Append((char)LowercaseMap[index]);
                    }
                    else
                    {
                        decrypted.Append(c);
                    }
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    int index = c - 'A';
                    if (index < UppercaseMap.Length)
                    {
                        decrypted.Append((char)UppercaseMap[index]);
                    }
                    else
                    {
                        decrypted.Append(c);
                    }
                }
                else
                {
                    // special symbols not need decryption
                    decrypted.Append(c);
                }
            }
            return decrypted.ToString();
        }
    }
}
