using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                  StronglyConverter.SwaggerSchemaFilter |
                  StronglyConverter.SystemTextJson |
                  StronglyConverter.TypeConverter)]
    public partial struct EmojiId;

    public class Emoji : Entity<EmojiId>, IHasCreationTime
    {
        public Emoji(string name)
        {
            Id = EmojiId.New();
            Name = name;
            CreationTime = DateTime.Now;
        }

        private Emoji()
        {
        }

        public DateTimeOffset CreationTime { get; private set; }
        public override EmojiId Id { get; protected set; }
        public string Name { get; private set; } = null!;
    }
}