// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.CloudStorage.Extension.Exceptions
{
    public class CloudStorageValidationException : Exception
    {
        public CloudStorageValidationException(string message) 
            : base(message)
        {
        }

        public CloudStorageValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}