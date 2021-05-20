using BatchAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI.BatchData
{
    public class MockBatchData : IBatchData
    {
        List<Batch> batches = new List<Batch>();

        public List<Batch> GetBatch()
        {
            return batches;
        }

        public Batch AddBatch(Batch batch)
        {
            #region AB
            //Batch batch = new Batch();

            //ACL acl = new ACL();
            //acl.ReadUsers = new List<string>() { "ReadUser1" };
            //acl.ReadGroups = new List<string>() { "ReadGroup1" };

            //Attributes attributes = new Attributes();
            //attributes.Key = new List<string>() { "Key1" };
            //attributes.Value = new List<string>() { "Value1" };

            //batch.BatchId = Guid.NewGuid();
            //batch.BusinessUnit = "BU";
            //batch.ACL = acl;
            //batch.Attributes = attributes;
            //batch.ExpiryDate = DateTime.Now;
            #endregion

            batch.BatchId = Guid.NewGuid();
            batches.Add(batch);
            
            return batch;
        }

        public Batch GetBatch(Guid batchId)
        {
            return batches.SingleOrDefault(p => p.BatchId.Equals(batchId));
        }

        public BatchFile AddBatchFile(Guid batchId, string fileName, string mimeType, string contentSize)
        {
            throw new NotImplementedException();
        }
    }
}
