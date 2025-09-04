using System.Net;
using FluentFTP;

namespace VsaSample.Infrastructure.FileTransfer;

public sealed class FtpHelper : IFileTransferHelper, IAsyncDisposable
{
    private readonly AsyncFtpClient _client;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public FtpHelper(IOptions<FtpOptions> options)
    {
        var opt = options.Value;
        _client = new AsyncFtpClient(opt.Host, new NetworkCredential(opt.UserName, opt.Password), opt.Port);
    }

    public async Task UploadFileAsync(Stream content, string remotePath, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!_client.IsConnected)
            {
                await _client.Connect(cancellationToken);
            }
            await _client.UploadStream(content, remotePath, token: cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Stream?> DownloadFileAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!_client.IsConnected)
            {
                await _client.Connect(cancellationToken);
            }
            var ms = new MemoryStream();
            var ok = await _client.DownloadStream(ms, remotePath, token: cancellationToken);
            if (!ok)
            {
                await ms.DisposeAsync();
                return null;
            }
            ms.Position = 0;
            return ms;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        _semaphore.Dispose();
        await _client.DisposeAsync();
    }
}
