using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Module.Core.Common
{
    /// <summary>
    /// 消息内容组装成Html
    /// </summary>
    public class MessageHtml
    {
        /// <summary>
        /// 获取消息通知内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="messageType">0:系统通知,10:帖子回复消息,11:帖子评论消息,12:帖子点赞消息,13:回复点赞消息,14:评论点赞消息,20:文档主题点赞消息,21:文档点赞消息</param>
        /// <returns></returns>
        public static string GetMessageContent(string nickName,int objectId,string title,int messageType,int id=0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<a href=\"javascript:void(0)\">{0}</a>&nbsp;", nickName);
            switch (messageType)
            {
                case 10:
                    stringBuilder.Append("回复了你的帖子&nbsp;");
                    break;
                case 11:
                    stringBuilder.Append("评论了你的帖子回复&nbsp;");
                    break;
                case 12:
                    stringBuilder.Append("点赞了你的帖子&nbsp;");
                    break;
                case 13:
                    stringBuilder.Append("点赞了你的帖子回复&nbsp;");
                    break;
                case 14:
                    stringBuilder.Append("点赞了你的帖子评论&nbsp;");
                    break;
                case 20:
                    stringBuilder.Append("点赞了你的文档主题&nbsp;");
                    break;
                case 21:
                    stringBuilder.Append("点赞了你的文档&nbsp;");
                    break;
            }
            if (messageType >= 10 && messageType < 20)
            {
                stringBuilder.AppendFormat("<a href=\"/cms/read/{0}\" target=\"_blank\">{1}</a>&nbsp;", objectId, title);
            }
            else if (messageType >= 20 && messageType < 30)
            {
                stringBuilder.AppendFormat("<a href=\"/docs/read/{0}/{1}\" target=\"_blank\">{2}</a>&nbsp;", id, objectId, title);
            }
            return stringBuilder.ToString();
        }
    }
}
