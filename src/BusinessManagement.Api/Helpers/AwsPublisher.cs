using Amazon.S3;
using Amazon.S3.Model;
using BusinessManagementApi.Dto;
using Newtonsoft.Json;

namespace BusinessManagement.Helpers;

public interface IAwsPublisher
{
    public Task Publish(string key, MemoryStream doc);
}

public class AwsPublisher: IAwsPublisher
{
    private readonly IAmazonS3 _s3Client;

    public AwsPublisher(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task Publish(string key, MemoryStream doc)
    {
        var putRequest = new PutObjectRequest()
        {
            BucketName = "valmiki-invoices",
            Key = $"{key}.pdf",
            InputStream = doc,
            ContentType = "application/pdf"
        };
        await _s3Client.PutObjectAsync(putRequest);
    }
}