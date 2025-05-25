using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace IC.Shared.Helpers
{
    public static class StringHelper
    {
        public static T Deserialize<T>(string json)
        {
            try
            {
                JsonSerializer s = new JsonSerializer();
                return s.Deserialize<T>(new JsonTextReader(new StringReader(json)));
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static string SetDefault(this string str, string strDefault)
        {
            return string.IsNullOrWhiteSpace(str) ? strDefault : str;
        }
        //----------------------------------------------------------------------------------------------
        public static string GetDataJsonFromUrl(string url)
        {
            string response = "";
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                response = httpClient.GetStringAsync(new Uri(url)).Result;
            }
            return response;
        }
        //----------------------------------------------------------------------------------------------
        public static string GetQuote(string odString)
        {
            string[] codes = odString.Split(',');
            string result = "";
            for (int i = 0; i < codes.Length; i++)
            {
                result = result + "'" + codes[i] + "',";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        //----------------------------------------------------------------------------------------------
        public static string CombineUrl(params string[] inputs)
        {
            string result = "";
            foreach (var item in inputs)
            {
                if (result != "") result += "/";
                result += item;
            }
            result = result.Replace("://", ":::");
            return Regex.Replace(result, @"/+", "/").Replace(":::", "://");
        }
        //----------------------------------------------------------------------------------------------
        public static string ExactValue(string str, string objName, char delimiter)
        {
            string[] codes = str.Split(delimiter);
            string result = "";
            int i = 0;
            while (i < codes.Length)
            {
                if (codes[i].Contains(objName))
                {
                    result = codes[i].Replace(objName, "");
                    result = result.Replace("=", "").Trim();
                    i = codes.Length;
                }
                i++;
            }
            return result;
        }
        public static bool IsImageFile(this string FileName)
        {
            bool RetVal = false;
            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.gif;.png;.bmp;.jpeg";
            if (imageFile.IndexOf(fileExt) >= 0) RetVal = true;
            return RetVal;
        }
        public static string GetRouterTypeByFileType(this string fileName)
        {
            string result = "Others";
            string value = Path.GetExtension(fileName).ToLower();
            string text = ".jpg;.jpeg;.gif;.png;.bmp";
            if (text.IndexOf(value) >= 0)
            {
                result = "images/original";
            }

            return result;
        }
        public static bool IsValidUploadFile(this string fileExt)
        {
            string[] ExtWhiteList = { ".mp3", ".webm", ".mp4", ".wave", ".png", ".jpg", ".jpeg", ".svg", ".bmp", ".gif", ".tiff", ".docx", ".xls", ".xlsx", ".pdf", ".zip", ".ico" };
            bool isValid = false;
            string checkExt = "";
            if (fileExt.Contains("."))
            {
                checkExt = fileExt.Substring(fileExt.LastIndexOf("."));

            }
            else
            {
                checkExt = "." + fileExt;
            }
            checkExt = checkExt.ToLower();
            if (ExtWhiteList.Contains(checkExt))
            {
                isValid = true;
            }
            return isValid;
        }
        //----------------------------------------------------------------------------------------------
        public static bool isNumeric(string s)
        {
            try
            {
                Convert.ToInt32(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------
        private static readonly string[] VietnameseSigns = new string[]
        {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
        };
        //----------------------------------------------------------------------------------------------
        public static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;

        }
        //----------------------------------------------------------------------------------------------
        public static string RemoveSpecialCharInURL(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = RemoveSignature(input);

            Regex reg = new Regex("[^a-zA-Z0-9_ ]");
            input = reg.Replace(input, " ");

            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }
            input = input.Trim().Replace(" ", "-");
            return input;
        }
        public static string RemoveSignatureForURL(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = RemoveSignature(input);
            input = input.Replace("?", "").Replace("&", "").Replace("\"", "").Replace("'", "");
            input = input.Replace(".", "").Replace(",", "").Replace(";", "");
            input = input.Replace("@", "").Replace("$", "").Replace("%", "").Replace("*", "");
            input = input.Replace("~", "").Replace("\\", "").Replace("/", "").Replace("!", "");
            input = input.Replace(":", "").Replace(")", "").Replace("(", "").Replace("+", "");
            input = input.Replace("`", "").Replace("|", "");
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }
            input = input.Replace(" ", "-");
            return input;
        }
        public static string RemoveSignature(string input)
        {
            if (input == null)
            {
                return null;
            }
            input = input.Replace("-", " ");
            Regex rga = new Regex("[àÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬ]");
            Regex rgd = new Regex("[đĐ]");
            Regex rge = new Regex("[èÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆ]");
            Regex rgi = new Regex("[ìÌỉỈĩĨíÍịỊ]");
            Regex rgo = new Regex("[òÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢ]");
            Regex rgu = new Regex("[ùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰ]");
            Regex rgy = new Regex("[ỳỲỷỶỹỸýÝỵỴ]");
            input = rga.Replace(input, "a");
            input = rgd.Replace(input, "d");
            input = rge.Replace(input, "e");
            input = rgi.Replace(input, "i");
            input = rgo.Replace(input, "o");
            input = rgu.Replace(input, "u");
            input = rgy.Replace(input, "y");
            return input;
        }
        public static string GetLead(string inputString, int length)
        {
            string retVal = "";
            try
            {
                if (string.IsNullOrEmpty(inputString))
                {
                    return retVal;
                }
                if (inputString.Length <= length)
                {
                    retVal = inputString;
                }
                else
                {
                    retVal = inputString.Substring(0, length);
                    if (retVal.Contains(" "))
                    {
                        retVal = retVal.Substring(0, retVal.LastIndexOf(" "));
                    }
                    retVal += "...";
                }
            }
            catch
            {
                retVal = inputString;
            }
            return retVal;
        }
        public static string InjectionString(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string tmp;
                    tmp = killChar(str).Replace("'", "''");
                    return str;
                }
            }
            catch
            {
            }
            return "";
        }
        public static DateTime ConvertToDateTime(string strDateTime)
        {
            string name = "vi-VN";
            try
            {
                return DateTime.Parse(strDateTime, CultureInfo.CreateSpecificCulture(name));
            }
            catch
            {
                return DateTime.Now;
            }
        }
        //-----------------------------------------------------------------------
        private static string killChar(string strInput)
        {
            try
            {
                string newChars;
                string[] badChars = new string[] { "select", "drop", ";", "--", "insert", "delete", "xp_" };
                newChars = strInput.Trim();
                for (int i = 0; i < badChars.Length; i++)
                {
                    newChars = newChars.Replace(badChars[i], "");
                }
                return newChars;
            }
            catch
            {
                return "";
            }
        }
        //-----------------------------------------------------------------------
        public static bool CheckPasswordStrength(string password)
        {
            int score = 1;
            if (password.Length >= 8)
                score++;
            if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
                Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript) == true)
                score++;
            if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                score++;

            return (score >= 3 ? true : false);
        }
        //-----------------------------------------------------------------------
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string NumberFormat(this int number, string format = "#,###")
        {
            return number > 0 ? number.ToString(format) : "0";
        }
        public static string NumberFormat(this int? number, string format = "#,###")
        {
            if (number.HasValue)
                return number.Value.NumberFormat();
            return "";
        }
        public static string NumberFormats(this int number, string format = "#,###")
        {
            return number.ToString(format);
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return
                enumValue.GetAttribute<DisplayAttribute>()?.Name;
        }

        public static string GetDescription(this Enum enumValue)
        {
            return
                enumValue.GetAttribute<DescriptionAttribute>()?.Description;
        }

        public static string GetEnumMember(this Enum enumValue)
        {
            return
                enumValue.GetAttribute<EnumMemberAttribute>()?.Value;
        }

        public static string GetPreviewContent(this string content)
        {
            if (content == null)
            {
                return string.Empty;
            }
            return cutString(content, Math.Min(100, content.Length), "...");
        }
        public static string cutString(string str, int maxLength, string replaceString)
        {
            try
            {
                if (str.Length > maxLength)
                {
                    str = str.Substring(0, maxLength);
                    str = str.Substring(0, str.LastIndexOf(" ")) + replaceString;
                }
                return str;
            }
            catch
            {
                return str;
            }
        }
        public static string GetLastString(string str, int length)
        {
            try
            {
                if (str.Length < length)
                {
                    length = str.Length;

                }
                str = str.Substring(str.Length - length);
            }
            catch { }
            return str;
        }

        public static string FileSizeFormat(this int number)
        {
            var sizeInKB = number / 1024.0;
            if (sizeInKB < 1)
            {
                return number + " Bytes";
            }

            var sizeInMB = sizeInKB / 1024.0;
            if (sizeInMB < 1)
            {
                return sizeInKB.ToString("0.00") + " KB";
            }

            var sizeInGB = sizeInMB / 1024.0;
            if (sizeInGB < 1)
            {
                return sizeInMB.ToString("0.00") + " MB";
            }

            return sizeInGB.ToString("0.00") + " GB";
        }

        public static string FileSizeFormat(this int? number)
        {
            if (number.HasValue)
                return number.Value.FileSizeFormat();
            return "";
        }

        public static string DurationSecondFormat(this int? currentDuration)
        {
            if (!currentDuration.HasValue || currentDuration.Value <= 0)
            {
                return string.Empty;
            }

            var duration = Math.Floor((decimal)currentDuration);

            var hours = Math.Floor(duration / 3600);

            var minutes = Math.Floor((duration - hours * 3600) / 60);

            var seconds = duration % 60;

            return $"{(hours < 10 ? $"0{hours}" : hours)}:{(minutes < 10 ? $"0{minutes}" : minutes)}:{(seconds < 10 ? $"0{seconds}" : seconds)}";
        }

        public static string DurationSecondFormat(this int currentDuration)
        {
            if (currentDuration <= 0)
            {
                return string.Empty;
            }

            var duration = Math.Floor((decimal)currentDuration);

            var hours = Math.Floor(duration / 3600);

            var minutes = Math.Floor((duration - hours * 3600) / 60);

            var seconds = duration % 60;

            return $"{(hours < 10 ? $"0{hours}" : hours)}:{(minutes < 10 ? $"0{minutes}" : minutes)}:{(seconds < 10 ? $"0{seconds}" : seconds)}";
        }

        public static string GenerateUniqId(string diff = null)
        {
            string text = DateTime.Now.Ticks.ToString();
            if (!string.IsNullOrEmpty(diff))
                text = $"{text}{diff}";

            return ConvertToUniqIdString(text);
        }

        private static string ConvertToUniqIdString(string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            var uniqId = Convert.ToBase64String(plainTextBytes, 0, 16);
            return uniqId.Substring(0, 16);
        }

        public static string GetFullnamePrefix(string fullname)
        {
            string retVal = "N";
            if (string.IsNullOrEmpty(fullname)) return retVal;

            fullname = fullname.Trim();
            while (fullname.Contains("  "))
            {
                fullname = fullname.Replace("  ", " ");
            }

            if (fullname.Contains(" "))
            {
                var splits = fullname.Split(' ');
                retVal = splits[0].Substring(0, 1) + splits[splits.Length - 1].Substring(0, 1);
            }
            else
            {
                if (fullname.Length > 2)
                {
                    retVal = fullname.Substring(0, 2);
                }
                else
                {
                    retVal = fullname;
                }
            }
            return retVal.ToUpper();
        }
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);

            if (pos != 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string GetDataUrl(this string path, string domain = "/")
        {
            string resultVar = path ?? string.Empty;

            if (!string.IsNullOrEmpty(resultVar))
            {
                if (resultVar.StartsWith("http:"))
                {
                    resultVar = resultVar.Replace("http:", "https:");
                }

                if (!resultVar.Contains("://"))
                {
                    while (resultVar.StartsWith("/"))
                    {
                        resultVar = resultVar[1..];
                    }

                    if (!string.IsNullOrWhiteSpace(domain) && !domain.EndsWith("/"))
                    {
                        domain += "/";
                    }

                    resultVar = string.Concat(domain, resultVar);
                }
            }

            return resultVar;
        }

        public static string GetBlockUrl(this string path, int blockId, int tabId, string domain = "/")
        {
            string resultVar = path;

            if (!string.IsNullOrEmpty(resultVar))
            {
                resultVar = resultVar.Replace(".html", "");

                if (resultVar.StartsWith("http:"))
                {
                    resultVar = resultVar.Replace("http:", "https:");
                }

                if (!resultVar.Contains("://"))
                {
                    while (resultVar.StartsWith("/"))
                    {
                        resultVar = resultVar[1..];
                    }

                    if (!string.IsNullOrWhiteSpace(domain) && !domain.EndsWith("/"))
                    {
                        domain += "/";
                    }

                    resultVar = string.Concat(domain, resultVar);
                }
            }

            if (tabId > 0)
            {
                return $"{resultVar}-b{blockId}-t{tabId}.html";
            }

            return $"{resultVar}-b{blockId}.html";
        }

        public static string GetThumbImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                if (imagePath.StartsWith("/uploads/"))
                {
                    imagePath = imagePath.GetDataUrl("https://data.voh.com.vn");
                }
                else if (imagePath.StartsWith("/clouds/"))
                {
                    imagePath = imagePath.Replace("/clouds/", "https://cdnx.voh.com.vn/voh/");
                }

                return imagePath;
            }

            return string.Empty;
        }
        public static string RemoveQueryParam(string url, string keyToRemove)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(keyToRemove))
            {
                return url;
            }
            var urlParts = url.Split('?');

            if (urlParts.Length < 2)
            {
                return url;
            }

            var baseUrl = urlParts[0];
            var queryParams = urlParts[1].Split('&');
            var updatedParams = new List<string>();

            foreach (var param in queryParams)
            {
                var keyValue = param.Split('=');
                var paramName = keyValue[0];
                if (paramName != keyToRemove)
                {
                    updatedParams.Add(param);
                }
            }

            var updatedUrl = baseUrl + (updatedParams.Count > 0 ? "?" + string.Join("&", updatedParams) : "");

            return updatedUrl;
        }

        public static long CreateId(string context, bool sensitive, Encoding encoding)
        {
            if (string.IsNullOrEmpty(context)) return 0;
            if (!sensitive) context = context.ToUpper();
            using var hasher = MD5.Create();
            var bytes = encoding.GetBytes(context.ToUpper());
            var hBytes = hasher.ComputeHash(bytes);

            return hBytes.Select((q, i) => Convert.ToInt64(q * Math.Pow(10, i + 1))).Sum();
        }

        public static string DefaultIfEmpty(this string str, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(str) ? defaultValue : str;
        }

        public static string DocGetViewUrl(int docId, int docFileId, string docUrl)
        {
            string[] array = docUrl.Split('/');
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(".html"))
                {
                    text = array[i];
                    break;
                }
            }

            return $"/xem-file-{text.Replace(".html", "")}-{docId}-{docFileId}.html";
        }

        public static string DocGetDownloadUrl(int docId, int docFileId, string docUrl)
        {
            string[] l_Url = docUrl.Split('/');
            string docUrlNotPath = string.Empty;
            for (int index = 0; index < l_Url.Length; index++)
            {
                if (l_Url[index].Contains(".html"))
                {
                    docUrlNotPath = l_Url[index];
                    break;
                }
            }

            return $"/tai-file-{docUrlNotPath.Replace(".html", string.Empty)}-{docId}-{docFileId}.html";
        }

        public static string CreateQueryString(this Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(kvp =>
                $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
        }

        private static string RemoveAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);

            return Regex.Replace(normalized, @"\p{Mn}", string.Empty);
        }

        public static string GetSlug(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string str = input.ToLower();

                str = RemoveAccents(str);

                str = str.Replace("đ", "d").Replace("Đ", "d");

                str = Regex.Replace(str, @"[^0-9a-z\s-]", "");

                str = Regex.Replace(str, @"\s+", "-");

                str = Regex.Replace(str, "-+", "-");

                str = str.Trim('-');

                return str;
            }

            return string.Empty;
        }

        public static string GetTag(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string str = RemoveAccents(input);

                str = str.Replace("đ", "d").Replace("Đ", "D");

                str = Regex.Replace(str, @"[^0-9a-zA-Z\s-]", "");

                str = Regex.Replace(str, @"\s+", "-");

                str = Regex.Replace(str, "-+", "-");

                str = str.Trim('-');

                return str;
            }

            return string.Empty;
        }

        public static string HighlightText(this string text, string keywords, string cssClass = "")
        {
            if (text == string.Empty || keywords == string.Empty || cssClass == string.Empty)
                return text;

            string resultVar = text;
            string[] words = keywords.Split(new[] { '/', ' ', ':', '-' }, StringSplitOptions.RemoveEmptyEntries),
                words2 = text.Split(new[] { '/', ' ', ':', '-' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Select(word => words2.FirstOrDefault(x => x.ConvertToUnSign() == word.ConvertToUnSign())).Where(wordReplace => !string.IsNullOrEmpty(wordReplace)).Aggregate(resultVar, (current, wordReplace) => current.Replace(wordReplace, $"<span class=\"{cssClass}\">{wordReplace}</span>"));
        }

        public static string ConvertToUnSign(this string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (var item in stFormD)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(item);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(item);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return sb.ToString().Normalize(NormalizationForm.FormD).ToLower();
        }

        public static string RemoveHtmlTags(this string input)
        {
            var regex = new Regex("<.*?>", RegexOptions.Compiled);
            return regex.Replace(input, string.Empty);
        }
        public static string StripTags(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char[] arr = new char[value.Length];
                int arrIndex = 0;
                bool check = false;
                for (int i = 0; i < value.Length; i++)
                {
                    char item = value[i];
                    if (item == '<')
                    {
                        check = true;
                        continue;
                    }
                    if (item == '>')
                    {
                        check = false;
                        continue;
                    }
                    if (!check)
                    {
                        arr[arrIndex] = item;
                        arrIndex++;
                    }
                }
                return new string(arr, 0, arrIndex);
            }
            else return value;
        }

        public static string Sanitize(this string stringValue)
        {
            if (null == stringValue)
                return null;
            return stringValue
                .RegexReplace("-{2,}", "-")                 // transforms multiple --- in - use to comment in sql scripts
                .RegexReplace(@"[*/]+", string.Empty)      // removes / and * used also to comment in sql scripts
                .RegexReplace(@"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string SanitizeWithoutSplash(this string stringValue)
        {
            if (null == stringValue)
                return null;
            return stringValue
                .RegexReplace("-{2,}", "-")                 // transforms multiple --- in - use to comment in sql scripts
                .RegexReplace(@"[*]+", string.Empty)      // removes / and * used also to comment in sql scripts
                .RegexReplace(@"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith);
        }

        public static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
        }
        public static string ConvertToSlug(this string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return "";

            stringValue = RemoveSignature(stringValue);
            stringValue = RemoveSpecialCharInURL(stringValue);
            return stringValue.ToLower();
        }
        public static string FirstCharLower(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            return $"{input[0].ToString().ToLower()}{input.Substring(1)}";
        }
        public static string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
    }
}
