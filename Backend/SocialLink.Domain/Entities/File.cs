using DomainCommons;
using Strongly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                  StronglyConverter.SwaggerSchemaFilter |
                  StronglyConverter.SystemTextJson |
                  StronglyConverter.TypeConverter)]
    public partial struct FileId;
    public class File : Entity<FileId>
    {
        public override FileId Id => throw new NotImplementedException();

        //public string Id { get; set; }
        //public string Filename { get; set; }
        //public string FileType { get; set; } // 文件类型 (如图片、视频、文档)
        //public string Url { get; set; }      // 文件存储路径
        //public string SenderId { get; set; } // 文件发送者
        //public string ConversationId { get; set; } // 所属对话
        //public DateTime UploadedAt { get; set; }
    }
}
