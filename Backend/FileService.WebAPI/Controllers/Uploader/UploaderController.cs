using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using FileService.Domain;
using FileService.Infrastructure;
using FileService.WebAPI.Controllers.Uploader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers.Uploader;

[Route("api/uploader")]
[ApiController]
[Authorize]
[UnitOfWork(typeof(FileServiceDbContext))]
public class UploaderController : ControllerBase
{
    private readonly FileServiceDbContext dbContext;
    private readonly FileServiceDomainService domainService;
    private readonly IFileServiceRepository repository;

    public UploaderController(FileServiceDomainService domainService, FileServiceDbContext dbContext, IFileServiceRepository repository)
    {
        this.domainService = domainService;
        this.dbContext = dbContext;
        this.repository = repository;
    }

    /// <summary>
    /// 检查是否有和指定的大小和SHA256完全一样的文件
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
    {
        var item = await repository.FindFileAsync(fileSize, sha256Hash);
        return item == null ? new FileExistsResponse(IsExists: false, null) : new FileExistsResponse(true, item.RemoteUrl);
    }

    [HttpPost]
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult<UploadedItemResponse>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
    {
        var file = request.File;
        string fileName = file.FileName;
        using Stream stream = file.OpenReadStream();
        var (isOldItem, Item) = await domainService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);
        if (!isOldItem)
        {
            dbContext.Add(Item);
        }
        var uploadItemResult = new UploadedItemResponse(Item.Id, Item.FileSizeInBytes, Item.Filename, Item.FileType, Item.FileSHA256Hash, Item.RemoteUrl, Item.CreatedAt);
        return Ok(uploadItemResult);
    }

    [HttpPost, Route("files")]
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult<IList<UploadedItemResponse>>> PostFiles([FromForm] IEnumerable<IFormFile> files, CancellationToken cancellationToken = default)
    {
        List<UploadedItemResponse> uploadItemResults = new();
        foreach (var file in files)
        {
            string fileName = file.FileName;
            using Stream stream = file.OpenReadStream();
            var (isOldItem, Item) = await domainService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);
            if (!isOldItem)
            {
                dbContext.Add(Item);
            }
            var uploadItemResult = new UploadedItemResponse(Item.Id, Item.FileSizeInBytes, Item.Filename, Item.FileType, Item.FileSHA256Hash, Item.RemoteUrl, Item.CreatedAt);
            uploadItemResults.Add(uploadItemResult);
        }
        return Ok(uploadItemResults);
    }

    [HttpGet("{fileId}")]
    public async Task<IActionResult> Get(UploadedItemId fileId)
    {
        var file = await repository.GetFileByIdAsync(fileId);
        if (file == null)
            return NotFound();

        var uploadItemResult = new UploadedItemResponse(file.Id, file.FileSizeInBytes, file.Filename, file.FileType, file.FileSHA256Hash, file.RemoteUrl, file.CreatedAt);

        return Ok(uploadItemResult);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetBatch([FromQuery] UploadedItemId[] Ids)
    {
        var file = await repository.GetFilesByIdsAsync(Ids);
        var uploadItemResults = file.Select(x => new UploadedItemResponse(x.Id, x.FileSizeInBytes, x.Filename, x.FileType, x.FileSHA256Hash, x.RemoteUrl, x.CreatedAt)).ToArray();
        return Ok(uploadItemResults);
    }
}
