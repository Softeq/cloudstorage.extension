// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.CloudStorage.Extension
{
    public interface IContentStorage
    {
        /// <summary>
        /// Saves content to blob asynchronously
        /// </summary>
        /// <param name="fileName">Source file name</param>
        /// <param name="content">Content stream</param>
        /// <param name="containerName">Destination container name</param>
        /// <param name="contentType">Stream content type</param>
        /// <returns>
        /// Blob local path
        /// </returns>
        Task<string> SaveContentAsync(string fileName, Stream content, string containerName, string contentType = null);

        /// <summary>
        /// Deletes blob from container asynchronously
        /// </summary>
        /// <param name="fileName">File name to delete</param>
        /// <param name="containerName">File container name</param>
        /// <returns>
        /// Throws ContentStorageException
        /// </returns>
        Task DeleteContentAsync(string fileName, string containerName);

        /// <summary>
        /// Gets blob content from container asynchronously
        /// </summary>
        /// <param name="fileName">Source file name</param>
        /// <param name="containerName">File container name</param>
        /// <returns>
        /// Blob bytes array
        /// </returns>
        Task<byte[]> GetContetAsync(string fileName, string containerName);

        /// <summary>
        /// Checks if blob exists asynchronously
        /// </summary>
        /// <param name="fileName">Destination file name</param>
        /// <param name="containerName">Destination file container name</param>
        /// <returns>
        /// Throws ContentStorageException
        /// </returns>
        Task<bool> BlobExistsAsync(string fileName, string containerName);

        /// <summary>
        /// Сopies blob from one container to another asynchronously
        /// </summary>
        /// <param name="fileName">Source file name</param>
        /// <param name="sourceContainerName">Source file container name</param>
        /// <param name="targetContainerName">Destination file container name</param>
        /// <returns>
        /// Target blob original Uri
        /// </returns>
        Task<string> CopyBlobAsync(string fileName, string sourceContainerName, string targetContainerName);

        /// <summary>
        /// Gets container Sas token asynchronously
        /// </summary>
        /// <param name="containerName">Target container name</param>
        /// <param name="expiryTimeInMinutes">Token expiration time in minutes</param>
        /// <returns>
        /// Container Sas token
        /// </returns>
        Task<string> GetContainerSasTokenAsync(string containerName, int expiryTimeInMinutes);

        /// <summary>
        /// Gets blob Sas token asynchronously
        /// </summary>
        /// <param name="containerName">Target file container name</param>
        /// <param name="fileName">Target file name</param>
        /// <param name="expiryTimeInMinutes">Token expiration time in minutes</param>
        /// <returns>
        /// Blob Sas token
        /// </returns>
        Task<string> GetBlobSasUriAsync(string containerName, string fileName, int expiryTimeInMinutes);
    }
}