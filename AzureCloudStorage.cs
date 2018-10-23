// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using Softeq.CloudStorage.Extension.Exceptions;

namespace Softeq.CloudStorage.Extension
{
    public class AzureCloudStorage : IContentStorage
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;

        public AzureCloudStorage() { }

        public AzureCloudStorage(string azureConStringName)
        {
            _storageAccount = CloudStorageAccount.Parse(azureConStringName);
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> SaveContentAsync(string fileName, Stream content, string containerName, string contentType = null)
        {
            return await SaveContentAsync(content, fileName, containerName, contentType);
        }

        public async Task DeleteContentAsync(string fileName, string containerName)
        {
            var container = await GetOrCreateContainerAsync(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            if (!await blockBlob.ExistsAsync())
            {
                throw new BlobNotFoundException($"Blob doesn't exist. FileName={fileName}, Container={containerName}");
            }
            await blockBlob.DeleteAsync();
        }

        public async Task<byte[]> GetContetAsync(string fileName, string containerName)
        {
            var container = await GetOrCreateContainerAsync(containerName);
            var blob = await container.GetBlobReferenceFromServerAsync(fileName);

            using (var memoryStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public async Task<bool> BlobExistsAsync(string fileName, string containerName)
        {
            var container = await GetOrCreateContainerAsync(containerName);
            return await container.GetBlockBlobReference(fileName).ExistsAsync();
        }

        public async Task<string> CopyBlobAsync(string fileName, string sourceContainerName, string targetContainerName)
        {
            var sourceContainer = await GetOrCreateContainerAsync(sourceContainerName);
            var targetContainer = await GetOrCreateContainerAsync(targetContainerName);
            var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
            var targetBlob = targetContainer.GetBlockBlobReference(fileName);
            var isSourceBlobExists = await sourceBlob.ExistsAsync();

            if (!isSourceBlobExists)
            {
                throw new BlobNotFoundException($"Source blob doesn't found. FileName={fileName}, Container={sourceContainerName}");
            }

            var context = new SingleTransferContext
            {
                ShouldOverwriteCallbackAsync = (source, destination) => Task.FromResult(true)
            };

            var cancellationSource = new CancellationTokenSource();
            await TransferManager.CopyAsync(sourceBlob, targetBlob, true, null, context, cancellationSource.Token);

            return targetBlob.Uri.OriginalString;
        }

        public async Task<string> SaveContentAsync(Stream content, string fileName, string containerName, string contentType)
        {
            var container = await GetOrCreateContainerAsync(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);

            if (!string.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }

            // Create or overwrite the "myblob" blob with contents from a local file.
            await blockBlob.UploadFromStreamAsync(content);
            return blockBlob.Uri.LocalPath;
        }

        public async Task<string> GetContainerSasTokenAsync(string containerName, int expiryTimeInMinutes)
        {
            var container = await GetOrCreateContainerAsync(containerName);

            var sharedPolicy = GetSharedAccessBlobPolicy(DateTime.UtcNow.AddHours(expiryTimeInMinutes), SharedAccessBlobPermissions.Write);
            var sasContainerToken = container.GetSharedAccessSignature(sharedPolicy);

            return sasContainerToken;
        }

        public async Task<string> GetBlobSasUriAsync(string containerName, string fileName, int expiryTimeInMinutes)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(containerName))
            {
                throw new CloudStorageValidationException("Invalid incoming parameters");
            }

            var container = await GetOrCreateContainerAsync(containerName);
            var blob = container.GetBlockBlobReference(fileName);

            var sharedPolicy = GetSharedAccessBlobPolicy(DateTime.UtcNow.AddMinutes(expiryTimeInMinutes), SharedAccessBlobPermissions.Read);
            var sasBlobToken = blob.GetSharedAccessSignature(sharedPolicy);

            return $"{blob.Uri}{sasBlobToken}";
        }

        private async Task<CloudBlobContainer> GetOrCreateContainerAsync(string containerName)
        {
            const string replacementPattern = "[^0-9a-z]+";
            containerName = Regex.Replace(containerName, replacementPattern, string.Empty);
            var container = _blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            };
            await container.SetPermissionsAsync(permissions);

            return container;
        }

        private SharedAccessBlobPolicy GetSharedAccessBlobPolicy(DateTimeOffset expirationTime, SharedAccessBlobPermissions blobPermissions)
        {
            // Create a new shared access policy and define its constraints.
            // The access policy provides create, write, read, list, and delete permissions.
            var sharedPolicy = new SharedAccessBlobPolicy
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = expirationTime,
                Permissions = blobPermissions
            };

            return sharedPolicy;
        }
    }
}