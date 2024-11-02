using DomainCommons;
using SocialLink.Domain.Enums;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                  StronglyConverter.SystemTextJson |
                  StronglyConverter.TypeConverter)]
    public partial struct UserContactId;

    public class UserContact : Entity<UserContactId>, IHasCreationTime
    {
        public UserContact(string remark, ContactStatus contactStatus)
        {
            Id = UserContactId.New();
            Remark = remark;
            ContactStatus = contactStatus;
        }

        private UserContact()
        {
        }

        public ContactStatus ContactStatus { get; }
        public DateTimeOffset CreationTime { get; private set; }
        public override UserContactId Id { get; protected set; }
        public string? Remark { get; private set; }
        public ContactStatus Status { get; private set; }
        public UserId UserId { get; private set; }
    }
}