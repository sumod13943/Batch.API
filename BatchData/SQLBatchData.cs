using BatchAPI.Model;
using BatchAzureBlob;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BatchAPI.BatchData
{
    public class SQLBatchData : IBatchData
    {
        private BatchContext _batchContext;
        private readonly string _storageAccountKey = "batchfilestorage";
        private readonly string _storageAccountName = "Su+z78wpiVFuI4FSnOhPYx1BTHtF1xqU81NcuoE6+0gkP9KvVwNQhT86Ad7WlpO7ZmXjpR8a21aG05IGwW0sYw==";
        private readonly string _containerName = "BatchContainer";


        public SQLBatchData(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }
        public Batch AddBatch(Batch batch)
        {
            batch.BatchId = Guid.NewGuid();
            _batchContext.Batches.Add(batch);
            _batchContext.SaveChanges();

            AzureBlob azureBlob = new AzureBlob();
            azureBlob.InitializeBlobContainer(_storageAccountKey, _storageAccountName, _containerName);

            return batch;
        }

        public BatchFile AddBatchFile(Guid batchId, string fileName, string mimeType, string contentSize)
        {
            Batch batch = _batchContext.Batches
                            .FirstOrDefault(p => p.BatchId.Equals(batchId));

            BatchFile batchFile = new BatchFile();

            if (batch != null)
            {
                bool fileExists = _batchContext.BatchFiles
                                  .Any(p => p.Batch.BatchId.Equals(batchId) && p.FileName.Equals(fileName));
                if (!fileExists)
                {
                    batchFile.Batch = batch;
                    batchFile.FileName = fileName;
                    batchFile.FileType = mimeType;
                    batchFile.FileSize = contentSize;

                    _batchContext.BatchFiles.Add(batchFile);
                    _batchContext.SaveChanges();

                    AzureBlob azureBlob = new AzureBlob(_storageAccountKey, _storageAccountName, _containerName);

                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    //azureBlob.UploadFileinBlocks(@"D:\Sums\Practice\UKHG\Batch.API\Uploads\sample.txt", fileName);
                    azureBlob.UploadFileinBlocks($"{path}/Uploads/sample.txt", fileName);

                }
            }
            return batchFile;
        }

        public List<Batch> GetBatch()
        {
            return _batchContext.Batches.ToList();
        }

        public Batch GetBatch(Guid batchId)
        {
            Batch batch = new Batch();

            batch = _batchContext.Batches
                    .Include("ACL")
                    .Include("Attributes")
                    .SingleOrDefault(p => p.BatchId.Equals(batchId));

            return batch;
        }
    }
}
