using Amazon.S3.Model;

namespace s3Test.Helper;

public interface IFileHandler
{
    Task<IFileUploadResult?> UploadAsync(Stream? file, string displayName, string mimeType, string extension, CancellationToken cancellationToken = default);

    Task<IFileUploadResult> UploadFileAsync(IFormFile? file, CancellationToken cancellationToken = default);

    Task<IFileUploadResult?> UploadAsync(Stream? file, string displayName, string mimeType, string extension, string bucketName, CancellationToken cancellationToken = default);

    Task<IFileUploadResult> UploadFileAsync(IFormFile? file, string bucketName, CancellationToken cancellationToken = default);

    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default);

    Task<GetObjectResponse> DownloadAsync(string path, string bucketName, CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, string bucketName, CancellationToken cancellationToken = default);


}
