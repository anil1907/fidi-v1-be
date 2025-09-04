namespace VsaSample.Application.Abstractions.FileTransfer;

public interface IFileTransferHelper
{
    Task UploadFileAsync(Stream content, string remotePath, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadFileAsync(string remotePath, CancellationToken cancellationToken = default);
}
