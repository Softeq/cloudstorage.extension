using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Softeq.CloudStorage.Extension.Test
{
    public class AzureCloudStorageIntegrationTest
    {
        private const string _cloudStorageConnectionString = "PUT_YOUR_CLOUD_CONNECTION_STRING_HERE";
        private const string _containerName = "test";
        private const string _fileName = "test.jpg";
        private readonly IContentStorage _contentStorage; 
        
        public AzureCloudStorageIntegrationTest()
        {
            _contentStorage = new AzureCloudStorage(_cloudStorageConnectionString);
        }
        
        [Fact]
        public async Task ShouldGetBlobUrlAsync()
        {
            Uri savedUrl = await SavedContentAsync(_fileName);
            var url = await _contentStorage.GetBlobUrlAsync(_fileName, _containerName);
            
            url.Should().BeEquivalentTo(savedUrl);
        }
        
        [Fact]
        public async Task ShouldSaveContentAsync()
        {
            Uri savedUrl = await SavedContentAsync(_fileName);
            var url = await _contentStorage.GetBlobUrlAsync(_fileName, _containerName);
            url.Should().Be(savedUrl);
            
            //Clear
            await _contentStorage.DeleteContentAsync(_fileName, _containerName);
        }

        private async Task<Uri> SavedContentAsync(string fileName)
        {
            Uri savedUrl;
            string binaryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            string filePath = Path.Combine(binaryPath, "Resources", fileName);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                savedUrl = await _contentStorage.SaveContentAsync(fileName, stream, _containerName);
            }

            return savedUrl;
        }
    }
}