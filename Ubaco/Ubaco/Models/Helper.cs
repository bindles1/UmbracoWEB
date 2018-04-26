using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;

namespace Ubaco.Helper
{
    public static class FileUpload
    {
        public static bool CreateImageDirectory(string dir)
        {
            string path = HttpContext.Current.Server.MapPath(dir);
            if (!File.Exists(path))
            {
                File.Create(path);
                return true;
            }
            return false;
        }

        public static void SingleUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("/Content/Images"), Guid.NewGuid() + "_" + file.FileName));
            }
        }

        public static void SingleUpload(HttpPostedFileBase file, string path)
        {
            if (file != null && file.ContentLength > 0)
            {
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(path), Guid.NewGuid() + "_" + file.FileName));
            }
        }

        public static void MultiUpload(List<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("/Content/Images"), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                }
            }
        }
    }

    public class Paging

    {

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage { get; set; }

        public int NextPage { get; set; }

        public double TotalPages { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }



        public static Paging GetPages(int itemCount, int itemsPerPage)

        {

            int page;

            int.TryParse(HttpContext.Current.Request.QueryString["page"], out page);

            if (page == 0) page = 1;



            var pages = new Paging { ItemsPerPage = itemsPerPage, CurrentPage = page, PreviousPage = page - 1, NextPage = page + 1, TotalPages = Math.Ceiling(itemCount / (Double)itemsPerPage), Skip = (page * itemsPerPage) - itemsPerPage, Take = itemsPerPage };


            return pages;

        }

    }


    public class ValueCheck
    {
        public static bool ContainsList(List<string> list, string value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Benchmark
    {
        const int _max = 100000000;
        public static string BenchmarkList(List<string> list, string value)
        {
            var s2 = Stopwatch.StartNew();
            for (int i = 0; i < _max; i++)
            {
                bool f = list.Contains(value);
            }
            s2.Stop();

            return ((s2.Elapsed.TotalMilliseconds * 1000000) / _max).ToString("0.00 ns");
        }
    }

    public class Notification
    {
        public Notification()
        {

        }
        public NotifyStatus Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public enum NotifyStatus
        {
            Success,
            Warning,
            Danger,
            Info,
            Default
        }
    }

    public class TextFormat
    {
        #region CropText
        public static string CropText(string text, int maxLength, bool doDots)
        {
            return (text.Length <= maxLength ? text : text.Substring(0, maxLength) + (doDots ? "..." : ""));
        }
        #endregion
    }

    public class Mail
    {
        #region MailSender
        public static bool MailSender(string from, string to, string subject, string body)
        {
            bool status = false;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 25;
                client.Credentials = new System.Net.NetworkCredential("krisbjj@gmail.com", "ihasnoidea55");

                if (IsValidEmail(to))
                {
                    mail.From = new MailAddress(from);
                    mail.To.Add(new MailAddress(to));
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    string htmlbody = body.ToString();
                    mail.Body = htmlbody;
                    client.Send(mail);
                    return (status = true);
                }
                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }
        #endregion

        #region Validate Email
        public static bool IsValidEmail(string mailAddress)
        {
            Regex reg = new Regex("^((([a-z]|\\d|[!#\\$%&amp;'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&amp;'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            if (!string.IsNullOrEmpty(mailAddress) && reg.IsMatch(mailAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    public class Log
    {
        #region Log
        /// <summary>
        /// LogStatus represents a status of a log
        /// </summary>
        public enum LogStatus
        {
            OK,
            Error,
            Send,
            NotSend,
            Exception
        }
        /// <summary>
        /// Log sets a new entry in log.txt file
        /// </summary>
        /// <param name="logMessage">the message to parse to the log</param>
        /// <param name="status">the status from enum LogStatus</param>
        public static void LogEntry(string logMessage, LogStatus status)
        {
            string path = HttpContext.Current.Server.MapPath("/App_Data/Logs/myLog.txt");
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            using (StreamWriter w = File.AppendText(path))
            {
                TextWriter writer = w;
                w.Write("\r\nLog Entry : {0} {1} : Status {2} ---- {3}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), status, logMessage);
            }
        }
        #endregion

        #region DumpLog
        /// <summary>
        /// DumpLog helps output a list of log messages
        /// </summary>
        /// <returns></returns>
        public static List<string> DumpLog()
        {
            string path = HttpContext.Current.Server.MapPath("/App_Data/Logs/myLog.txt");
            List<string> logs = new List<string>();
            string line;
            using (StreamReader r = File.OpenText(path))
            {
                while ((line = r.ReadLine()) != null)
                {
                    logs.Add(line);
                }
            }
            return logs;
        }
        #endregion
    }

    public class Numbers
    {
        #region Shuffle int
        public static void Shuffle(int[] arr)
        {
            Random r = new Random();
            for (int n = arr.Length - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                int temp = arr[n];
                arr[n] = arr[k];
                arr[k] = temp;
            }
        }
        #endregion

        #region Shuffle String
        public static void Shuffle(string[] arr)
        {
            Random r = new Random();
            for (int n = arr.Length - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                string temp = arr[n];
                arr[n] = arr[k];
                arr[k] = temp;
            }
        }
        #endregion
    }

    public class PagingExtensions
    {
        #region Paging
        public class Paging
        {
            public int ItemsPerPage { get; set; }
            public int CurrentPage { get; set; }
            public int PreviousPage { get; set; }
            public int NextPage { get; set; }
            public double TotalPages { get; set; }
            public int Skip { get; set; }
            public int Take { get; set; }
        }
        #endregion

        #region Paging GetPages
        public static Paging GetPages(int itemCount, int itemsPerPage)
        {
            int page;
            int.TryParse(HttpContext.Current.Request.QueryString["page"], out page);
            if (page == 0) { page = 1; }
            var pages = new Paging
            {
                ItemsPerPage = itemsPerPage,
                CurrentPage = page,
                PreviousPage = page - 1,
                NextPage = page + 1,
                TotalPages = Math.Ceiling(itemCount / (double)itemsPerPage),
                Skip = (page * itemsPerPage) - itemsPerPage,
                Take = itemsPerPage
            };
            return pages;
        }
        #endregion
    }

    public class Encryption
    {
        #region MD5 Hashing
        public static string MD5Hash(string data)
        {
            // namespace System.Security.Cryptography
            MD5 md5 = MD5.Create();

            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            // returner Hexadecimal streng
            return returnValue.ToString();

        }
        #endregion

        #region Validate MD5
        public static bool ValidateMD5Hash(string inputData, string hashData)
        {
            //MD5 hash ny string
            string getHashInputData = MD5Hash(inputData);
            // sammenlign de to strenge
            if (string.Compare(getHashInputData, hashData) == 0)
                return true;
            else
                return false;
        }
        #endregion

        #region SHA1 Hashing
        public static string SHA1Hash(string data)
        {
            // namespace System.Security.Cryptography
            SHA1 sha1 = SHA1.Create();

            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            // returnere hexadecimal streng
            return returnValue.ToString();
        }
        #endregion

        #region Validate SHA1
        public static bool ValidateSHA1Hash(string inputData, string hashData)
        {
            //SHA1 hash ny string
            string getHashInputData = SHA1Hash(inputData);
            // sammenlign de to strenge
            if (string.Compare(getHashInputData, hashData) == 0)
                return true;
            else
                return false;
        }
        #endregion
    }

    public class HtmlRemoval
    {
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
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
    }

    //public static string Cut(this object text, int cut = 50)
    //{
    //    if (text.ToString().Length <= cut)
    //        return text.ToString();
    //    else
    //        return text.ToString().Remove(cut - 3) + "...";
    //}

    //public static string CapFirst(this string text)
    //{
    //    // Check for empty string.
    //    if (string.IsNullOrEmpty(text))
    //    {
    //        return string.Empty;
    //    }
    //    // Return char and concat substring.
    //    return char.ToUpper(text[0]) + text.Substring(1);
    //}

    //public static decimal ToTax(this decimal price, int addFee = 0)
    //{
    //    decimal NewPrice = price + addFee;
    //    return Math.Round(Decimal.Multiply(new decimal(0.2), NewPrice), 2);
    //}

    //public static string ToPrice(this object price)
    //{
    //    return string.Format("{0},-", price.ToString().Replace(",00", ""));
    //}

    //public static int ToInt(this object data)
    //{
    //    int value = 0;
    //    int.TryParse(data.ToString(), out value);

    //    return value;
    //}
}
