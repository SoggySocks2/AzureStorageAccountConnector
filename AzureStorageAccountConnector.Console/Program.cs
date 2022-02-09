using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

/* Initialise configuration */
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();

/* Get Azure storage account connection string */
/*
 NOTE: Use the below command in Azure CLI to get your storage account connection string:
 Remember to replace <name> with the name fo your storgae account and update thr resource group
az storage account show-connection-string \
  --resource-group learn-5e6d0aba-e82d-49cc-97f2-4272626d41ae \
  --query connectionString \
  --name <name>
 */
var connectionString = configuration.GetConnectionString("StorageAccount");
string containerName = "photos";

/* Create the photos container if it doesn't already exist */
var container = new BlobContainerClient(connectionString, containerName);
container.CreateIfNotExists();

/* Upload a file */
string blobName = "docs-and-friends-selfie-stick";
string fileName = "docs-and-friends-selfie-stick.png";
BlobClient blobClient = container.GetBlobClient(blobName);
blobClient.Upload(fileName, true);

/* Get and show a list of blobs in the photos container */
var blobs = container.GetBlobs();
foreach (var blob in blobs)
{
    Console.WriteLine($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:yyyy-MM-dd HH:mm:ss}  Size: {blob.Properties.ContentLength}");
}