// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.CloudStorage.Extension.Exceptions
{
    public class ContentStorageException : Exception
    {
        public ContentStorageException(string message)
            : base(message)
        {
        }

        public ContentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
