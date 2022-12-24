using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using s3Test.Options;

namespace s3Test.Helper;

public class FileHandler : IFileHandler
{
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<s3Configuration> _options;

    public FileHandler(IOptions<s3Configuration> options)
    {
        _options = options;

        //this is just for local stack
        _s3Client = new AmazonS3Client(new AmazonS3Config
        {
            ServiceURL = _options.Value.ProxyUrl,
            ForcePathStyle = true,
            UseHttp = true,
            SignatureVersion = "2"
        });
    }


    public async Task<IFileUploadResult?> UploadAsync(Stream? file, string displayName, string mimeType,
        string extension, CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return null;
        }

        await CheckBucketAsync(cancellationToken);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var request = new PutObjectRequest
        {
            BucketName = _options.Value.BucketName,
            Key = fileName,
            InputStream = file,
            ContentType = mimeType,
        };
        

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return new FileUploadResult
        {
            Path = fileName,
            DisplayName = displayName
        };
    }

    public async Task<IFileUploadResult> UploadFileAsync(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return new FileUploadResult();
        }

        await CheckBucketAsync(cancellationToken);

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine("uploads", fileName);

        var request = new PutObjectRequest
        {
            BucketName = _options.Value.BucketName,
            Key = filePath,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType,

        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return new FileUploadResult
        {
            Path = filePath,
            DisplayName = fileName
        };
    }

    public async Task<IFileUploadResult?> UploadAsync(Stream? file, string displayName, string mimeType,
        string extension, string bucketName, CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return null;
        }

        await CheckBucketAsync(cancellationToken);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = file,
            ContentType = mimeType
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return new FileUploadResult
        {
            Path = fileName,
            DisplayName = displayName
        };
    }

    public async Task<IFileUploadResult> UploadFileAsync(IFormFile? file, string bucketName,
        CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return new FileUploadResult();
        }

        await CheckBucketAsync(cancellationToken);

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine("uploads", fileName);

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = filePath,
            InputStream = file.OpenReadStream()
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return new FileUploadResult
        {
            Path = filePath,
            DisplayName = fileName
        };
    }

    public async Task<GetObjectResponse> DownloadAsync(string path, CancellationToken cancellationToken)
    {

        return await _s3Client.GetObjectAsync(_options.Value.BucketName, path, cancellationToken);
    }

    public async Task<GetObjectResponse> DownloadAsync(string path, string bucketName,
        CancellationToken cancellationToken)
    {


        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = path
        };

        return await _s3Client.GetObjectAsync(request, cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        
        var request = new DeleteObjectRequest
        {
            BucketName = _options.Value.BucketName,
            Key = path
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    public async Task DeleteAsync(string path, string bucketName, CancellationToken cancellationToken)
    {

        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = path
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    private async Task CheckBucketAsync(CancellationToken cancellationToken)
    {

        var bucketExists = await _s3Client.DoesS3BucketExistAsync(_options.Value.BucketName);

        if (!bucketExists)
            await _s3Client.PutBucketAsync(_options.Value.BucketName, cancellationToken);

    }

}