using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace FileService.Domain.Entities;

public record UploadedItem : Entity<UploadedItemId>, ICreationAuditable
{
    private UploadedItem()
    {
    }

    public UploadedItem(long fileSizeInBytes, string filename, string fileType, string fileSHA256Hash, Uri backupUrl, Uri remoteUrl)
    {
        Id = UploadedItemId.New();
        CreatedAt = DateTime.Now;
        Filename = filename;
        FileType = fileType;
        FileSHA256Hash = fileSHA256Hash;
        FileSizeInBytes = fileSizeInBytes;
        BackupUrl = backupUrl;
        RemoteUrl = remoteUrl;
    }
    public override UploadedItemId Id { get; protected set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public long FileSizeInBytes { get; private set; }

    public string Filename { get; private set; } = null!;

    public string FileType { get; private set; } = null!;// 文件类型 (如图片、视频、文档)

    /// <summary>
    /// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
    /// SHA256的碰撞的概率比MD5低很多。
    /// </summary>
    public string FileSHA256Hash { get; private set; } = null!;
    /// <summary>
    /// 上传的文件的供外部访问者访问的路径
    /// </summary>
    public Uri RemoteUrl { get; private set; } = null!;
    /// <summary>
    /// 备份文件路径，因为可能会更换文件存储系统或者云存储供应商，因此系统会保存一份自有的路径。
    /// 备份文件一般放到内网的高速、稳定设备上，比如NAS等。
    /// </summary>
    public Uri BackupUrl { get; private set; } = null!;



}
