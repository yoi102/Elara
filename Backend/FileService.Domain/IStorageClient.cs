﻿namespace FileService.Domain;

public interface IStorageClient
{
    StorageType StorageType { get; }


    Task<Uri> SaveAsync(string partialPath, Stream content, CancellationToken cancellationToken = default);
}