// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.CloudStorage.Extension
{
    public interface IContentStorage
    {
        Task<string> SaveContentAsync(string fileName, Stream content, string container, string contentType = null);
        /// <summary>
        /// Delete content
        /// </summary>
        /// <returns>
        /// Throws ContentStorageException
        /// </returns>
        Task DeleteContentAsync(string fileName, string container);

        /// <summary>
        /// Get content
        /// </summary>
        /// <returns>
        /// Throws ContentStorageException
        /// </returns>
        Task<byte[]> GetContetAsync(string fileName, string container);

        /// <summary>
        /// Check contents exists
        /// </summary>
        /// <returns>
        /// Throws ContentStorageException
        /// </returns>
        Task<bool> ContentExistsAsync(string fileName, string container);

        /// <summary>
        /// Copy blob from one source container to target container
        /// </summary>
        /// <param name="fileName">blob name</param>
        /// <param name="sourceContainer">source container name</param>
        /// <param name="targetContainer">target container name</param>
        /// <returns></returns>
        Task<string> CopyBlobAsync(string fileName, string sourceContainer, string targetContainer);

        /// <summary>
        /// Get Sas token for whole container with expiration time
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expiryTimeInMinutes"></param>
        /// <returns></returns>
        Task<string> GetContainerSasTokenAsync(string container, int expiryTimeInMinutes);
        
        /// <summary>
        /// Get Sas token for specific blob from the container with expiration time
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fileName"></param>
        /// <param name="expiryTimeInMinutes"></param>
        /// <returns></returns>
        Task<string> GetBlobSasUriAsync(string container, string fileName, int expiryTimeInMinutes);
    }
}
