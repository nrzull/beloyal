using Amazon.Runtime;
using Amazon.S3;

namespace Beloyal.API;

public static class AWSExtensions
{
  public static void SetupS3(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<AmazonS3Client>(sp =>
    {
      var config = new AmazonS3Config()
      {
        ForcePathStyle = true,
        UseHttp = true,
        ServiceURL = Environment.GetEnvironmentVariable("S3_PUBLIC_URL"),
      };

      var credentials = new BasicAWSCredentials(
        Environment.GetEnvironmentVariable("S3_ACCESS_KEY"),
        Environment.GetEnvironmentVariable("S3_SECRET_KEY")
      );

      var client = new AmazonS3Client(clientConfig: config, credentials: credentials);

      return client;
    });
  }
}
