using Minio.Exceptions;
using Minio;

namespace IC.Shared.Helpers
{
	public class MinioHelper
	{
		public class MinioConfig
		{
			public string AwsEndPoint { get; set; }
			public string AwsKey { get; set; }
			public string AwsSecret { get; set; }
			public string AwsRegion { get; set; }
			public string AwsBucket { get; set; }
		}

		public static async Task<string> UploadToS3(MinioConfig config, string filePath, string objectName, string contentType = "")
		{
			string result;
			try
			{
				var minio = new MinioClient()
									.WithEndpoint(config.AwsEndPoint)
									.WithCredentials(config.AwsKey, config.AwsSecret)
									//.WithSSL()
									.Build();
				result = await RunUpload(minio, config.AwsBucket, filePath, objectName, contentType);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

        public static async Task<string> UploadToS3(MinioConfig config, Stream fileStream, string objectName, string contentType = "")
        {
            string result;
            try
            {
                var minio = new MinioClient()
                                    .WithEndpoint(config.AwsEndPoint)
                                    .WithCredentials(config.AwsKey, config.AwsSecret)
                                    .Build();

                result = await RunUpload(minio, config.AwsBucket, fileStream, objectName, contentType);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }


        private async static Task<string> RunUpload(MinioClient minio, string bucket, string filePath, string objectName, string contentType = "")
		{
			//var contentType = "application/zip";
			string result = "OK";  
			try
			{
				// Make a bucket on the server, if not already present.
				var beArgs = new BucketExistsArgs()
					.WithBucket(bucket);
				bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);

				if (!found)
				{
					var mbArgs = new MakeBucketArgs()
						.WithBucket(bucket);

					await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
				}

                // Upload a file to bucket.
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    PutObjectArgs putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithObjectSize(fileStream.Length)
                        .WithStreamData(fileStream);                    
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        putObjectArgs.WithContentType(contentType);
                    }
                    await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                    minio.Dispose();
                }
                
			}
			catch (MinioException e)
			{
				result = e.Message;
			}

			return result;
		}

        private async static Task<string> RunUpload(MinioClient minio, string bucket, Stream fileStream, string objectName, string contentType = "")
        {
            string result = "OK";
            try
            {
                // Check if the bucket exists, and create it if it does not.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucket);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);

                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucket);

                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }

                // Reset the stream position to the beginning.
                fileStream.Seek(0, SeekOrigin.Begin);

                // Upload the file stream to the bucket.
                var putObjectArgs = new PutObjectArgs()
					.WithBucket(bucket)
					.WithObject(objectName)
					.WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length);

                if (!string.IsNullOrEmpty(contentType))
                {
                    putObjectArgs.WithContentType(contentType);
                }

                await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

                minio.Dispose();
            }
            catch (MinioException e)
            {
                result = e.Message;
            }

            return result;
        }

    }
}
