using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoggingBencher
{
    class Generator
    {
        Random random;
        long Id = 0;

        public Generator()
        {
            random = new Random(DateTime.Now.Millisecond);
        }
        public string ID()
        {
            Id++;
            return String.Format("id:{0}", Id);
        }

        public string Time()
        {
            DateTime now = DateTime.Now;
            string result = now.ToString("yyyy-MM-dd HH:mm:ss");
            return String.Format("time:{0}", result);
        }

        public string Level()
        {
            string[] level = { "DEBUG", "INFO", "WARN", "ERROR" };
            string[] result = level.OrderBy(i => Guid.NewGuid()).ToArray();
            return String.Format("level:{0}", result.First());
        }

        public string Method()
        {
            string[] level = { "GET", "POST", "PUT", "DELETE", "PATCH" };
            string[] result = level.OrderBy(i => Guid.NewGuid()).ToArray();
            return String.Format("method:{0}", result.First());
        }
        public string Uri()
        {
            string[] level = { "/api/v1/user", "/api/v1/login", "/api/v1/logout", "/api/v1/profile", "/api/v2/settings" };
            string[] result = level.OrderBy(i => Guid.NewGuid()).ToArray();
            return String.Format("uri:{0}", result.First());
        }

        public string RequestTime()
        {
            return String.Format("reqtime:{0}", random.NextDouble() * 5); // [0.0, 5.0]
        }

        public string Parameter(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return String.Format("param:{0}", result);
        }

        public string Run() {

            return String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", ID(), Time(), Level(), Method(), Uri(), RequestTime(), Parameter(8));
        }
    }
}
