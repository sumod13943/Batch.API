using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace BatchAzureBlob
{
    public class AzureBlob
    {
        //these variables are used throughout the class
        string ContainerName { get; set; }
        BlobContainerClient blobContainer { get; set; }

        //this is the only public constructor; can't use this class without this info
        public AzureBlob()
        {
         //   blobContainer = InitializeBlobContainer(storageAccountName, storageAccountKey, containerName);
        }

        public AzureBlob(string storageAccountName,string storageAccountKey, string containerName)
        {
               blobContainer = InitializeBlobContainer(storageAccountName, storageAccountKey, containerName);
        }

        /// <summary>
        /// set up references to the container, etc.
        /// </summary>
        public BlobContainerClient InitializeBlobContainer(string storageAccountName,
          string storageAccountKey, string containerName)
        {
            string connectionString = string.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
            storageAccountName, storageAccountKey);

            //get a reference to the container where you want to put the files
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower().Replace(" ", "-"));

            //just in case, check to see if the container exists,
            //  and create it if it doesn't
            blobContainerClient.CreateIfNotExists();

            //set access level to "blob", which means user can access the blob 
            //  but not look through the whole container
            //this means the user must have a URL to the blob to access it

            blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            return blobContainerClient;
        }

        public void SetUpContainer(string storageAccountName,
          string storageAccountKey, string containerName)
        {
            BlobContainerClient blobContainer =
                                    InitializeBlobContainer(storageAccountName, storageAccountKey, 
                                                                containerName.ToLower().Replace(" ", "-"));
            
            blobContainer.CreateIfNotExists();
            blobContainer.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }

        public void UploadFileinBlocks(string filePath, string fileName)
        {
            string _filename = DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss") + Path.GetFileName(filePath);
            BlockBlobClient blobClient = blobContainer.GetBlockBlobClient(fileName);// Path.GetFileName(filePath));

            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = "application/octet-stream";
            blobClient.SetHttpHeadersAsync(blobHttpHeaders);

            int blockSize = 256 * 1024; //256 kb

            using (FileStream fileStream =
              new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long fileSize = fileStream.Length;

                //block count is the number of blocks + 1 for the last one
                int blockCount = (int)((float)fileSize / (float)blockSize) + 1;

                //List of block ids; the blocks will be committed in the order of this list 
                List<string> blockIDs = new List<string>();

                //starting block number - 1
                int blockNumber = 0;

                try
                {
                    int bytesRead = 0; //number of bytes read so far
                    long bytesLeft = fileSize; //number of bytes left to read and upload

                    //do until all of the bytes are uploaded
                    while (bytesLeft > 0)
                    {
                        blockNumber++;
                        int bytesToRead;
                        if (bytesLeft >= blockSize)
                        {
                            //more than one block left, so put up another whole block
                            bytesToRead = blockSize;
                        }
                        else
                        {
                            //less than one block left, read the rest of it
                            bytesToRead = (int)bytesLeft;
                        }

                        //create a blockID from the block number, add it to the block ID list
                        //the block ID is a base64 string
                        string blockId =
                          Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("BlockId{0}",
                            blockNumber.ToString("0000000"))));
                        blockIDs.Add(blockId);
                        //set up new buffer with the right size, and read that many bytes into it 
                        byte[] bytes = new byte[bytesToRead];
                        fileStream.Read(bytes, 0, bytesToRead);

                        //calculate the MD5 hash of the byte array
                        byte [] blockHash = GetMD5HashByte(bytes);

                        //upload the block, provide the hash so Azure can verify it
                        blobClient.StageBlock(blockId, new MemoryStream(bytes), blockHash);

                        //increment/decrement counters
                        bytesRead += bytesToRead;
                        bytesLeft -= bytesToRead;
                    }

                    //commit the blocks
                    blobClient.CommitBlockList(blockIDs);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private byte[] GetMD5HashByte(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(bytes);

        
            return retVal;
        }

        private string GetMD5HashString(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
