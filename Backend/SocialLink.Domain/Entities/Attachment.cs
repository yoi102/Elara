using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
              StronglyConverter.SwaggerSchemaFilter |
              StronglyConverter.SystemTextJson |
              StronglyConverter.TypeConverter)]
    public partial struct AttachmentId;

    public class Attachment : Entity<AttachmentId>
    {
        public Attachment(string name, string fileType, Uri url)
        {
            Id = AttachmentId.New();
            Name = name;
            FileType = fileType;
            Url = url;
        }

        private Attachment()
        {
        }

        public string FileType { get; set; } = null!;
        public override AttachmentId Id { get; protected set; }
        public string Name { get; set; } = null!;
        public Uri Url { get; set; } = null!;
    }
}