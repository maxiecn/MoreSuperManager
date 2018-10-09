using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBTopicModel : IChannelModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public int TopicType { get; set; }
        /// <summary>
        /// 投放位置列表
        /// </summary>
        public string PositionTypeList { get; set; }
        /// <summary>
        /// 主题标题
        /// </summary>
        public string TopicTitle { get; set; }
        /// <summary>
        /// 主题标签列表
        /// </summary>
        public string TopicTags { get; set; }
        /// <summary>
        /// 主题封面
        /// </summary>
        public string TopicCoverImageUrl { get; set; }
        /// <summary>
        /// 主题简介
        /// </summary>
        public string TopicSummary { get; set; }
        /// <summary>
        /// 主题内容
        /// </summary>
        public string TopicContent { get; set; }
        /// <summary>
        /// 主题来源网站
        /// </summary>
        public string TopicOriginalWebsite { get; set; }
        /// <summary>
        /// 主题原始 URL
        /// </summary>
        public string TopicOriginalUrl { get; set; }
        /// <summary>
        /// 发布者
        /// </summary>
        public string TopicUserCode { get; set; }
        /// <summary>
        /// 主题状态，默认，审核，已删除
        /// </summary>
        public int TopicStatus { get; set; }
        /// <summary>
        /// 访问次数
        /// </summary>
        public int TopicVisitNum { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime TopicDateTime { get; set; }
    }
}
