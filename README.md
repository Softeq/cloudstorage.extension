![Azure DevOps builds](https://dev.azure.com/eugenypetlakh/cloudstorage.extension/_apis/build/status/Softeq.cloudstorage.extension?branchName=master)
![NuGet](https://img.shields.io/nuget/v/Softeq.CloudStorage.Extension.svg)

# Cloud Storage Extensions

Cloud Storage Extensions provide high level abstraction of Azure Cloud storage. 

## Get started
1. Register package
``` csharp
            builder.Register(x =>
                {
                    var context = x.Resolve<IComponentContext>();
                    var config = context.Resolve<IConfiguration>();
                    return new AzureCloudStorage(config[ConnectionString]);
                })
                .As<IContentStorage>();
```
2. Configure storage
```csharp
            builder.Register(x =>
            {
                var context = x.Resolve<IComponentContext>();
                var config = context.Resolve<IConfiguration>();
                var cfg = new AzureStorageConfiguration(
                    config[ContentStorageHost],
                    config[MessageAttachmentsContainer],
                    config[MemberAvatarsContainer],
                    config[ChannelImagesContainer],
                    config[TempContainerName],
                    Convert.ToInt32(config[MessagePhotoSize]));
                return cfg;

            }).AsSelf();
```
3.  **appsettings.json** section
```json
  "AzureStorage": {
    "ConnectionString": "[connection string to azure storage]",
    "ContentStorageHost": "[content storage host]",
    "MessageAttachmentsContainer": "message",
    "MemberAvatarsContainer": "avatar",
    "ChannelImagesContainer": "channel",
    "TempContainerName": "temp",
    "MessagePhotoSize": 300
  }
```

## About

This project is maintained by [Softeq Development Corp.](https://www.softeq.com/)
We specialize in .NET core applications.

 - [Facebook](https://web.facebook.com/Softeq.by/)
 - [Instagram](https://www.instagram.com/softeq/)
 - [Twitter](https://twitter.com/Softeq)
 - [Vk](https://vk.com/club21079655).

## Contributing

We welcome any contributions.

## License

Cloud Storage Extensions project is available for free use, as described by the [LICENSE](/LICENSE) (MIT).