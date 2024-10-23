using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{

    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct ReactionId;
    public class Reaction : Entity<ReactionId>
    {
        private Reaction()
        {
        }
        public Reaction(string emoji, MessageId messageId, UserId userId)
        {
            Id = ReactionId.New();
            this.Emoji = emoji;
            this.MessageId = messageId;
            this.UserId = userId;
        }

        public override ReactionId Id { get; }
        public string Emoji { get; private set; }      // 表情符号
        public MessageId MessageId { get; private set; }  // 所属的消息ID
        public UserId UserId { get; private set; }     // 给出反应的用户ID
    }
}
