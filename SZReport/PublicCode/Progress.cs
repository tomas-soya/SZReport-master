using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SZReport.PublicCode
{
    public static class Progress
    {
        /// <summary>
        /// 向Http响应中实时写入信息（只有前端监听了onprogress事件此方法才会生效）
        /// </summary>
        /// <param name="message">消息文本</param>
        /// <param name="status">消息状态（参见ProgressStatus）</param>
        public static void BuildMessage(string message, string status = ProgressStatus.Default)
        {
            const string model = "<p style=\"color: {0}\">{1}：{2}</p>";
            string result = string.Format(model, status, DateTime.Now, message);

            //HttpContext.Current.Response.Write(result);
            //HttpContext.Current.Response.Flush();
        }
    }

    public static class ProgressStatus
    {
        public const string Default = "black";

        public const string Success = "green";

        public const string Waring = "orange";

        public const string Error = "red";
    }
}
