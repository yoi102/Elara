﻿using ASPNETCore;
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
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
    {
        var file = request.File;
        string fileName = file.FileName;
        using Stream stream = file.OpenReadStream();
        var result = await domainService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);
        if (!result.isOldUploadedItem)
        {
            dbContext.Add(result.UploadedItem);
        }
        return result.UploadedItem.RemoteUrl;
    }

    [HttpPost, Route("files")]
    [RequestSizeLimit(60_000_000)]
    public async Task<ActionResult<IList<Uri>>> PostFiles([FromForm] IEnumerable<IFormFile> files, CancellationToken cancellationToken = default)
    {
        List<Uri> uris = new();
        foreach (var file in files)
        {
            string fileName = file.FileName;
            using Stream stream = file.OpenReadStream();
            var result = await domainService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);
            if (!result.isOldUploadedItem)
            {
                dbContext.Add(result.UploadedItem);
            }
            uris.Add(result.UploadedItem.RemoteUrl);
        }
        return uris;
    }

    [HttpGet("{fileId}")]
    public async Task<IActionResult> Get(UploadedItemId fileId)
    {
        var file = await repository.GetFileByIdAsync(fileId);
        if (file == null)
            return NotFound();
        return Ok(file);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetBatch([FromQuery] UploadedItemId[] Ids)
    {
        var file = await repository.GetFilesByIdsAsync(Ids);

        return Ok(file);
    }
}
