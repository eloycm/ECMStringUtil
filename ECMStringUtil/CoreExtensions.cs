using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net;
using System.Xml.Linq;
using ECMStringUtil.BO;

namespace ECMStringUtil.Extensions
{
    public static class CoreExtensions
    {
        /// <summary>
        /// This extension method will take a string with words seperated by spaces and convert them
        /// to a single word with the first letters of each word capatalized.  ex: 'Kevin was here' => 'KevinWasHere'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string input, bool ForceLowerFirst = false)
        {
            TextInfo info = Thread.CurrentThread.CurrentCulture.TextInfo;
            if (ForceLowerFirst)
                input = input.ToLower();
            input = info.ToTitleCase(input);
            string[] parts = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            string result = String.Join(String.Empty, parts);
            return result;
        } // ToPascalCase - Extension Method

        /// <summary>
        /// This extension method will take a string with words seperated by spaces and convert them
        /// to a single word with the first letters of each word capatalized except the first word.
        /// ex: 'Kevin was here' => 'kevinWasHere'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string input)
        {
            input = input.ToPascalCase();
            return input.Substring(0, 1).ToLower() + input.Substring(1);
        } // ToCamelCase - Extension Method

        /// <summary>
        /// This extension method will take a string with words seperated by spaces and convert them
        /// where only the first word is capatalized.  ex: 'Kevin Was Here' => 'Kevin was here'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToProperCase(this string input)
        {
            const string pattern = @"(?<=\w)(?=[A-Z])";
            string result = Regex.Replace(input, pattern, " ", RegexOptions.None);
            return result.Substring(0, 1).ToUpper() + result.Substring(1);
        } // ToProperCase - Extension Method

        public static string ToWordsFromPascalCase(this string input)
        {
            return Regex.Replace(input, "[a-z][A-Z]", m => String.Format("{0} {1}", m.Value[0], char.ToLower(m.Value[1])));
        }
        public static string Replace(this string Input, string oldValue, string newValue, StringComparison Comparison)
        {
            StringBuilder Result = new StringBuilder();
            int previousIndex = 0;
            int index = Input.IndexOf(oldValue, Comparison);

            while (index != -1)
            {
                Result.Append(Input.Substring(previousIndex, index - previousIndex));
                Result.Append(newValue);
                index += oldValue.Length;
                previousIndex = index;
                index = Input.IndexOf(oldValue, index, Comparison);
            } // while stepping backwards from the end

            Result.Append(Input.Substring(previousIndex));

            return Result.ToString();
        } // Replace - Extension Method - Overload
          
        /// <summary>
          /// replace the first appearence of search strings in text
          /// </summary>
          /// <param name="text"></param>
          /// <param name="replace"></param>
          /// <param name="search"></param>
          /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
                return text;

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        /// <summary>
        /// replace the first appearence of multiple strings in text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replace"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string ReplaceFirstMultiple(this string text, string replace, params string[] search)
        {

            foreach (var s in search)
                if (text.Contains(s))
                {
                    var rs = text.ReplaceFirst(s, replace);
                    return rs;
                }


            return text;
        }
        /// <summary>
        /// replaces a series of string in a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replacement"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string ReplaceMultiple(this string str, string replacement, params string[] search)
        {
            var sb = new StringBuilder(str);
            foreach(var s in search)
            {
                sb.Replace(s, replacement);
            }
            return sb.ToString();
        }

        /// <summary>
        /// True if the target string contains any string on the inpu list
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Possibilities"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string Input, List<string> Possibilities)
        {
            foreach (string s in Possibilities)
            {
                if (Input.Contains(s))
                    return true;
            } // foreach Possibiity

            return false;
        } // ContainsAny - Extension Method

        public static string FormatTime(this string Input)
        {
            if (string.IsNullOrWhiteSpace(Input))
            {
                return "";
            }
            else
            {
                DateTime output;
                if (DateTime.TryParseExact(Input, "HHmm", new CultureInfo("en-US"), DateTimeStyles.None, out output) == true)
                {
                    return String.Format("{0:h:mm tt}", output);
                }
                else
                {
                    return "";
                }
            }

        }

        public static string FormatPhoneNumber(this string Input)
        {

            string Number = Input.Substring(0, 10);
            long lNumber;
            string Extension = Input.Substring(10);

            if (long.TryParse(Number, out lNumber) == true)
            {
                return string.Format("{0:000-000-0000}{1}{2}", lNumber, !string.IsNullOrWhiteSpace(Extension) ? " ext. " : "", !string.IsNullOrWhiteSpace(Extension) ? Extension : "");
            }
            else
            {
                return string.Format("{0:000-000-0000}{1}{2}", Number, !string.IsNullOrWhiteSpace(Extension) ? " ext. " : "", !string.IsNullOrWhiteSpace(Extension) ? Extension : "");
            }
        }

        public static string FormatZipCode(this string Input)
        {
            if (Input.Length == 9)
            {
                return string.Format("{0}-{1}", Input.Substring(0, 5), Input.Substring(5, 4));
            }
            else
            {
                return Input;
            }
        }
        /// <summary>
        /// cuts a string without cutting in the middle of a word
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cutOffLength"></param>
        /// <param name="separators">we can pass more than one valid char as a separator</param>
        /// <returns></returns>
        public static string WordCut(this string text, int cutOffLength, char[] separators)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            cutOffLength = cutOffLength > text.Length ? text.Length : cutOffLength;
            int separatorIndex = text.Substring(0, cutOffLength).LastIndexOfAny(separators);
            if (separatorIndex > 0)
                return text.Substring(0, separatorIndex);
            return text.Substring(0, cutOffLength);
        }
        public static string WordCut(this string text, int cutOffLength)
        {
            return WordCut(text, cutOffLength, new char[] { ' ' });
        }

        /// <summary>
        /// searches for email address in a string and decorates them with an html mailto tag
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string IncludeMailTo(this string s)
        {
            Regex email_regex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string result = email_regex.Replace(s, "<a href=mailto:$1>$1</a>");

            return result;
        }
        /// <summary>
        /// Sanitizes the scaty bookmark created sometimes by ckh editor.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string SanitizeScatyBookmark(this string s)
        {
            const string search = "id=\"scayt_bookmark\"";
            return s.Replace(search, string.Empty);
        }
        /// <summary>
        /// Sanitizes the menu link string. spaces and & are converted to hyphen
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string SanitizeMenuLinkString(this string s)
        {
            s = s.Replace(" & ", "-");
            s = s.Replace(' ', '-');
            // s=s.Replace("/",string.Empty);

            return s;

        }
        /// <summary>
        /// Removes the special characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string input)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }
        /// <summary>
        /// Remove brackets {} for example in a guid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveBrackets(this string input)
        {
            string rs = input.Replace("{", string.Empty);
            rs = rs.Replace("}", string.Empty);
            return rs;
        }

        /// <summary>
        /// eliminates any html carriages return while preseerving other tags
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string ToSingleLineHtml(this string s)
        {
            if (s == null)
                return s;
            s = s.ReplaceMultiple(string.Empty, "<br/>", "<br />", "<br>", "<p>", "</p>", "<p/>");

            return s; 
        }
        public static bool IsHtmlValid(this string s)
        {
            try
            {
                XmlDocument xm = new XmlDocument();
                xm.LoadXml(string.Format("<root>{0}</root>", s));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// determines if a string is a guid
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsGuid(this string s)
        {
            Regex isGuid = new Regex(@"^\{?[a-fA-F\d]{8}-([a-fA-F\d]{4}-){3}[a-fA-F\d]{12}\}?$", RegexOptions.Compiled);
            bool result = isGuid.IsMatch(s);
            return result;
        }
        public static string MatchGuid(this string s)
        {
            string guidRegex = @"[({]?(0x)?[0-9a-fA-F]{8}([-,]?(0x)?[0-9a-fA-F]{4}){2}((-?[0-9a-fA-F]{4}-?[0-9a-fA-F]{12})|(,\{0x[0-9a-fA-F]{2}(,0x[0-9a-fA-F]{2}){7}\}))[)}]?";
            Regex isGuid = new Regex(guidRegex, RegexOptions.Compiled);
            Match match = isGuid.Match(s);
            if (match.Success)
                return match.Groups[0].Value;

            return string.Empty;
        }
        public static string CleanStringBefore(this string s, string tk)
        {
            if (s.IndexOf(tk) < 0)
                return s;
            s = s.Substring(s.IndexOf(tk));
            return s;

        }
        public static string CleanupAbsoluteUrl(this string url)
        {
            Uri result;
            bool r = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out result);

            if (!result.IsAbsoluteUri)
                return url;



            string rs = result.AbsolutePath.Replace("%20", " ");

            return rs;
        }

        /// <summary>
        /// Determines whether the specified string contains a GUID.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <c>true</c> if the specified s contains GUID; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsGuid(this string s)
        {
            int iniPos = s.IndexOf("{");
            if (iniPos < 0)
                return false;

            if ((iniPos + 32) > s.Length)
                return false;

            s = s.Substring(iniPos + 1, 36);

            bool result = IsGuid(s);
            return result;
        }

        /// <summary>
        /// Remove HTML tags from an arbitrary string using char the high performace
        /// char array algorithm.
        /// </summary>
        public static string StripTagsCharArray(this string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {

                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        /// <summary>
        /// makes sure that a string starts with the prefix
        /// </summary>
        /// <param name="source"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string EnsurePrefix(this string source, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return source;

            if (string.IsNullOrWhiteSpace(source))
                return source;

            if (source.StartsWith(prefix))
                return source;

            return string.Format("{0}{1}", prefix, source);
        }

        /// <summary>
        /// if a string starts with http.. or wwww is external
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsExternalUrl(this string url)
        {
            if (url.StartsWith("http://") || url.StartsWith("www."))
                return true;

            return false;
        }
        public static string EnsureAbsoluteUrl(this string url)
        {

            string rs = url.LastIndexOf("www") > 10 ? url.Substring(url.LastIndexOf("www")) : url;

            rs = rs.StartsWith("http://") ? rs : string.Format("http://{0}", url);
            return rs;

        }
        /// <summary>
        /// Converts a vertical response form in an asp.net friendly html
        /// The asp.net friendly html will be converted again to the form by jquery
        /// working around the asp.net only one form restriction
        /// </summary>
        /// <param name="sToclean">The s toclean.</param>
        /// <returns></returns>
        public static string VerticalResponseformCleanUp(this string sToclean)
        {
            string rs = sToclean.Replace("</form>", "</div>");
            rs = rs.Replace("<form", "<div id='divform'");
            rs = rs.Replace("method=", "data-method=");
            rs = rs.Replace("action=", "data-action=");
            rs = rs.Replace("target=", "data-target=");
            rs = rs.Replace("onsubmit=", "data-onsubmit=");
            return rs;
        }

        /// <summary>
        /// Encrypts the string using a RijndaelManaged algorithm .
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public static string EncryptString(this string plainText)
        {
            // Instantiate a new RijndaelManaged object to perform string symmetric encryption
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            // Set key and IV
            rijndaelCipher.Key = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");
            rijndaelCipher.IV = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our RijndaelManaged object
            ICryptoTransform rijndaelEncryptor = rijndaelCipher.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
        }

        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }


        /// <summary>
        /// Decrypts the string using RijndaelManaged algorithm.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public static string DecryptString(this string cipherText)
        {

            if (!cipherText.IsBase64String())
                return string.Empty;
            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = null;

            try
            {
                // Instantiate a new RijndaelManaged object to perform string symmetric encryption
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                // Set key and IV
                rijndaelCipher.Key = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");
                rijndaelCipher.IV = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");

                // Instantiate a new encryptor from our RijndaelManaged object
                ICryptoTransform rijndaelDecryptor = rijndaelCipher.CreateDecryptor();

                // Instantiate a new CryptoStream object to process the data and write it to the 
                // memory stream
                cryptoStream = new CryptoStream(memoryStream, rijndaelDecryptor, CryptoStreamMode.Write);

                // Will contain decrypted plaintext
                string plainText = String.Empty;


                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the encrypted byte array to a base64 encoded string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                return plainText;

            }

            catch (Exception)
            {
                // I should be able to not break up if the user pass me a garbage string
                return string.Empty;
            }

            finally
            {
                // Close both the MemoryStream and the CryptoStream
                if (memoryStream != null)
                    memoryStream.Close();

                if (cryptoStream != null)
                    cryptoStream.Close();
            }



        }
        
        public static string BytesToString(this string byteCount)
        {
            long bt;
            long.TryParse(byteCount, out bt);
            if (bt == 0)
                return string.Empty;

            return bt.BytesToString();
        }
        public static string BytesToString(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
        /// <summary>
        /// In sitecore 6.6 media urls come with a repeated http root breaking the link, this method
        /// cleans up the first ocurrence of the duplicated, leaving alone urls well formed
        /// </summary>
        /// <param name="mediaUrl">url to be cleaned</param>
        /// <returns>cleaned url</returns>
        public static string CleanUpMediaUrl(this string mediaUrl, bool isSecureConnection = false)
        {
            string shttp = isSecureConnection ? "https://" : "http://";

            string sb = mediaUrl.TrimBefore(shttp);

            if ((!sb.IsInitialSubstringRepeated(shttp)))
                return sb;

            sb = mediaUrl.Substring(7);
            sb = sb.Substring(sb.IndexOf(shttp));
            sb = sb.CleanPortFromUrl();

            return sb;
        }

        public static string CleanPortFromUrl(this string url)
        {
            var uri = new Uri(url);
            var clean = uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port,
                               UriFormat.UriEscaped);

            return clean;
        }
        /// <summary>
        /// Finds out if a substring is repeated inside a string
        /// this should be unbreakeble
        /// </summary>
        /// <param name="source">string to be tested</param>
        /// <param name="subString">string to be searched</param>
        /// <returns>true or false depending on whether substring is found mor than one time or not</returns>
        public static bool IsInitialSubstringRepeated(this string source, string subString)
        {
            if (source == null || subString == null)
                return false;

            if (source.Length < 2)
                return false;

            if (source.IndexOf(subString) < 0)
                return false;

            if (subString.Length * 2 > source.Length)
                return false;

            string s = source.Substring(1);
            bool r = s.IndexOf(subString) >= 0;

            return r;

        }
        /// <summary>
        /// Trims all the characters before the searchstring
        /// </summary>
        /// <param name="source"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static string TrimBefore(this string source, string searchString)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (source.IndexOf(searchString) <= 0)
                return source;

            string rs = source.Substring((source.IndexOf(searchString)));

            return rs;

        }
        /// <summary>
        /// Trims all the characters before the searchstring
        /// </summary>
        /// <param name="source"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static string TrimAfter(this string source, string searchString)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (source.IndexOf(searchString) <= 0)
                return source;

            string rs = source.Substring(0, (source.IndexOf(searchString)));

            return rs;

        }

        public static bool IsPoBox(this string addr)
        {
            if (string.IsNullOrEmpty(addr))
                return false;

            const string pat = @"\b[P|p]*(OST|ost)*\.*\s*[O|o|0]*(ffice|FFICE)*\.*\s*[B|b][O|o|0][X|x]\b";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            var rs = r.IsMatch(addr);

            return rs;
        }
        /// <summary>
        /// check if a string is a representation of a google array
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsGoogleArray(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            s = s.Replace("\r\n", string.Empty);
            if (s.Substring(0, 1) != "[")
                return false;

            if (s.Substring(s.Length - 1, 1) != "]")
                return false;


            return true;
        }
        /// <summary>
        /// checks if a string contains tab delimited data
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsTabDelimited(this string s)
        {

            if (s.IndexOf("\t\t") < 1)
                return false;
            if (s.IndexOf("\n") < 1)
                return false;

            return true;

        }
        /// <summary>
        /// converts a tag delimited data into a google array
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string toGogleArray(this string s)
        {
            s = s.Replace("\t\n", "|");
            s = s.Replace("\n", "|");

            s = s.Replace(Environment.NewLine, "|");
            var rows = s.Split('|');

            string rs = "[";
            for (int j = 0; j < rows.Length - 1; j++)
            {
                rs = string.Format("{0}{1}", rs, "[");

                var cc = rows[j].Replace("\t\t", "|");
                cc = cc.Replace("\t", "|");

                var mm = cc.Split('|');
                for (int i = 0; i < mm.Length; i++)
                    mm[i] = mm[i].formattedGoogleArrayElement();

                cc = string.Join(",", mm);
                rs = string.Format("{0}{1}{2}", rs, cc, "],");
            }
            rs = string.Format("{0}{1}", rs, "]");
            rs = rs.Replace("],]", "]]");

            return rs;


        }
        private static string formattedGoogleArrayElement(this string s)
        {
            if (!s.IsNumeric())
                return string.Format("{0}{1}{2}", "'", s.Replace("'", @"\'"), "'");

            return s.sanitizeGoogleNumeric();
        }
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s.sanitizeGoogleNumeric(), out output);
        }
        public static string sanitizeGoogleNumeric(this string n)
        {
            n = n.Replace("%", string.Empty);

            return n;
        }

        /// <summary>
        /// split a string into a list of string based on separator
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> ToStringList(this string s, string separator= "\r\n")
        {
            s = s.SanitizeCarriageReturn();
            var rs = s.Split(new string[] { separator }, StringSplitOptions.None).ToList();
            return rs;
        }
        public static string GetParishString(this List<string> s, string parishID)
        {
            string template = string.Format("{{ \"type\": \"Feature\", \"properties\": {{ \"ID\": \"{0}\"", parishID);

            string rs = s.Find(w => w.StartsWith(template));

            return rs;
        }
       
        public static string SanitizeSingleQuotedString(this string s)
        {
            string rs = s.Replace("'", "&quot;");
            return rs;
        }
        /// <summary>
        /// handles the cases where there is only a \n as a carriage return
        /// </summary>
        /// <param name="s"></param>
        /// <returns>sanitized string with \r\n instead of only \n</returns>
        public static string SanitizeCarriageReturn(this string s)
        {
            string rs = Regex.Replace(s, "(?<!\r)\n", "\r\n");
            return rs;
        }
        public static string CleanCarriageReturn(this string s, string replacement = " ")
        {
            string result = Regex.Replace(s, @"\r\n?|\n", replacement);
            return result;
        }

        /// <summary>
        /// takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        public static string InBetween(this string @this, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? @this.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { return null; }

            var endIndex = !string.IsNullOrEmpty(until)
            ? @this.IndexOf(until, startIndex, comparison)
            : @this.Length;

            if (endIndex < 0) { return null; }

            var subString = @this.Substring(startIndex, endIndex - startIndex);
            return subString;
        }
        public static string SanitizeSitecoreName(this string stringToFormat)
        {
            //Remove invalid characters for name
            string returnValue = Regex.Replace(stringToFormat, @"[\/:?'<>|;\-\,\*\&\.!$\+\~\#\[\]\(\)\%""]", string.Empty);

            // Return cleaned string
            return returnValue.Trim();
        }
        /// <summary>
        /// Filter and accept some html on strings based on a coding convention
        /// </summary>
        /// <param name="toRecode">string containing the encoded html tagas</param>
        /// <param name="tags">tags to be accepted as valid (other tags will be ignored)</param>
        /// <param name="lTag">left tag alternate symbol</param>
        /// <param name="rTag">right tag alternate symbol</param>
        /// <returns>string containg html.</returns>
        public static string RecodeHtml(this string toRecode, string[] tags, string lTag = "[", string rTag = "]")
        {
            foreach (var s in tags)
            {
                string srch = string.Format("{0}{1}{2}", lTag, s, rTag);
                string rcd = string.Format("<{0}>", s);
                toRecode = toRecode.Replace(srch, rcd);

                srch = string.Format("{0}/{1}{2}", lTag, s, rTag);
                rcd = string.Format("</{0}>", s);
                toRecode = toRecode.Replace(srch, rcd);

                srch = string.Format("{0}{1}/{2}", lTag, s, rTag);
                rcd = string.Format("<{0} />", s);
                toRecode = toRecode.Replace(srch, rcd);

                srch = string.Format("{0}{1} /{2}", lTag, s, rTag);
                rcd = string.Format("<{0} />", s);
                toRecode = toRecode.Replace(srch, rcd);
            }

            return toRecode;
        }

        /// <summary>
        /// Eliminates html tables on text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SanitizeHtmlTables(this string html)
        {
            html = html.Replace("<table>", "<p>");
            html = html.Replace("<td>", "");
            html = html.Replace("</td>", " ");
            html = html.Replace("<tr>", "");

            html = html.Replace("</tr>", "<br/>");
            html = html.Replace("</table>", "</p>");

            return html;
        }
        /// <summary>
        /// converts to datetime, useful for events that contain a separate string with the time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string date, string time)
        {

            var d = string.Format("{0} {1}", GetDatePartFromSitecore(date), time);
            var rs = DateTime.Parse(d);
            return rs;
        }
        public static string GetDatePartFromSitecore(string date)
        {
            var mat = date.Split('T');
            return mat[0];
        }
        /// <summary>
        /// Converts a time string from 20:20 to 8PM
        /// </summary>
        /// <param name="timeToTest"></param>
        /// <returns></returns>
        public static string ToStandardTime(this string timeToTest)
        {
            if (string.IsNullOrEmpty(timeToTest))
                return string.Empty;
            string rs = string.Empty;

            try
            {
                if (timeToTest.IsAMPMTime())
                    return timeToTest;

                rs = DateTime.ParseExact(timeToTest.FixMilitaryTime(), "HH:mm", CultureInfo.CurrentCulture).ToString("hh:mm tt");
            }
            catch (Exception)
            {

                return string.Empty;
            }
            rs = rs.TrimLeadingZero();
            return rs;
        }
        public static bool IsAMPMTime(this string time)
        {
            var r = new Regex(@"^([0]\d|[1][0-2]):([0-5]\d)\s?(?:AM|PM)", RegexOptions.IgnoreCase);
            if (!r.IsMatch(time))
                return r.IsMatch(string.Format("0{0}", time));

            return true;
        }
        /// <summary>
        /// uses regular expressions to determine if the military time is in the correct format
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsMilitaryTime(this string time)
        {
            var r = new Regex("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
            return r.IsMatch(time);
        }
        /// <summary>
        /// checks if the time is military time, if not, attemp some basic fixes
        /// if no fixes work, returns 00:00Is
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FixMilitaryTime(this string time)
        {
            if (time.IsMilitaryTime())
                return time;
            var t2 = string.Format("0{0}", time);
            if (t2.IsMilitaryTime())
                return t2;

            return "00:00";

        }
        public static string TrimLeadingZero(this string input)
        {
            var rs = input.TrimStart('0', ' ');
            return rs;
        }
        /// <summary>
        /// Search an address inside the string, assuming the address always starts by a number
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string AddressOnString(this string address)
        {
            int pos = address.IndexOfAny("0123456789".ToCharArray());
            if (pos <= 0)
                return address;

            string rs = address.Substring(pos);
            return rs;
        }

        /// <summary>
        /// gets a set of google coordinates for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string[] GetCoordinates(this string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address.AddressOnString()));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");
            if (result == null)
                return new string[] { string.Empty, string.Empty };  //google didn't find the address

            var locationElement = result.Element("geometry").Element("location");
            var lat = locationElement.Element("lat");
            var lng = locationElement.Element("lng");

            string[] rs = new string[] { lat.Value.ToString(), lng.Value.ToString() };
            return rs;

        }
        public static string GetImgUrlFromHtml(this string html)
        {
            string matchString = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
            matchString = matchString.Replace(" ", "%20");
            return matchString;
        }
        /// <summary>
        /// get last word from a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LastWord(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            string[] parts = str.Split(' ');
            string lastWord = parts[parts.Length - 1];
            return lastWord;
        }
        /// <summary>
        /// eliminates or replace last word from string if parameter are specified
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stripString"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string StripLastWord(this string str, string stripString = "", string replacement = "")
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            string[] parts = str.Split(' ');


            if (stripString != string.Empty && stripString != parts[parts.Length - 1])
                return str;
            if (replacement == string.Empty)
                return String.Join(" ", parts, 0, parts.Length - 1);

            string rs = string.Format("{0} {1}", String.Join(" ", parts, 0, parts.Length - 1), replacement);
            return rs;
        }
        /// <summary>
        /// delete the specified first char on a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toReplace"></param>
        /// <returns></returns>
        public static string DeleteLeadingCharIf(this string str, string toReplace)
        {

            if (string.IsNullOrEmpty(str))
                return str;

            if (str == toReplace)
                return string.Empty;

            if (str.Substring(0, 1) != toReplace)
                return str;

            var rs = str.Substring(1, str.Length - 1);

            return rs;
        }
        /// <summary>
        /// decodes a basic authentication header,
        /// </summary>
        /// <param name="s"></param>
        /// <returns>username and password</returns>
        public static string DecodeUserPWdBasic(this string s)
        {
            s = s.Replace("Basic ", "");
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(s));
            return usernamePassword;
        }

        public static StructText InTag(this string s,string opening,string closing)
        {
            var rs = new StructText();
            int nopen = s.IndexOf(opening);

            if (nopen<0)
            {
                rs.text = s;
                return rs;
            }

            return rs;
        }
        public static bool ContainsFrom(this string s,int index, string search)
        {
            if (index + search.Length > s.Length)
                return false;

            var substr = s.Substring(index, search.Length);

            return substr == search;


        }
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static IList<T> TakeRandom<T>(this IList<T> list, int lenght)
        {
            var rs = new List<T>();
            list.Shuffle();
            for (int i = 0; i < list.Count && i < lenght; i++)
            {
                rs.Add(list[i]);

            }
            return rs;
        }
        public static string ExtractStringFromGuid(this string guid)
        {
            var rs = guid.Replace("-", string.Empty);
            rs = rs.Replace("{", string.Empty).Replace("}", string.Empty);
            return rs;
        }
        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        public static int ToInt(this string s, int defaultvalue = 0)
        {
            int rs;
            var i = int.TryParse(s, out rs);
            if (!i)
                return defaultvalue;

            return rs;
        }
        /// <summary>
        /// strips sitecore domain from users (for instance)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string StripDomain(this string username)
        {
            int pos = username.IndexOf('\\');
            return pos != -1 ? username.Substring(pos + 1) : username;
        }
        public static string ToLowerUnbrackedGuid(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var rs = s.ToLower().Replace("{", "").Replace("}", "");
            return rs;
        }


    }
}
