// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.CloudStorage.Extension
{
    public class BlobNotFoundException : ContentStorageException
    {
        public BlobNotFoundException(string message)
            : base(message)
        {
        }

        public BlobNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}