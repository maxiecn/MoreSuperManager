using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class DALFactory
    {
        public static MenuDAL Menu { get { return new MenuDAL(); } }
        public static RoleDAL Role { get { return new RoleDAL(); } }
        public static TopicTypeDAL TopicType { get { return new TopicTypeDAL(); } }
        public static TopicPositionTypeDAL TopicPositionType { get { return new TopicPositionTypeDAL(); } }
        public static TopicDAL Topic { get { return new TopicDAL(); } }
        public static FlowDAL Flow { get { return new FlowDAL(); } }
        public static FlowStepDAL FlowStep { get { return new FlowStepDAL(); } }
        public static FlowTypeDAL FlowType { get { return new FlowTypeDAL(); } }
        public static ProjectTypeDAL ProjectType { get { return new ProjectTypeDAL(); } }
        public static ProjectDAL Project { get { return new ProjectDAL(); } }
        public static VoteTypeDAL VoteType { get { return new VoteTypeDAL(); } }
        public static VoteDAL Vote { get { return new VoteDAL(); } }
        public static VoteItemDAL VoteItem { get { return new VoteItemDAL(); } }
        public static UserDAL User { get { return new UserDAL(); } }
        public static FlowSymbolTypeDAL FlowSymbolType { get { return new FlowSymbolTypeDAL(); } }
        public static ActionTypeDAL ActionType { get { return new ActionTypeDAL(); } }
        public static ModuleDAL Module { get { return new ModuleDAL(); } }
        public static UserLogDAL UserLog { get { return new UserLogDAL(); } }
        public static MessageDAL Message { get { return new MessageDAL(); } }
        public static MessageReplyDAL MessageReply { get { return new MessageReplyDAL(); } }
        public static AttachmentDAL Attachment { get { return new AttachmentDAL(); } }
        public static LinkFriendTypeDAL LinkFriendType { get { return new LinkFriendTypeDAL(); } }
        public static LinkFriendDAL LinkFriend { get { return new LinkFriendDAL(); } }
        public static IndexMapperDAL IndexMapper { get { return new IndexMapperDAL(); } }
        public static NoticeTypeDAL NoticeType { get { return new NoticeTypeDAL(); } }
        public static NoticeDAL Notice { get { return new NoticeDAL(); } }
        public static InitDAL Init { get { return new InitDAL(); } }
        public static ApplicationDAL Application { get { return new ApplicationDAL(); } }
        public static ChannelDAL Channel { get { return new ChannelDAL(); } }
    }
}
