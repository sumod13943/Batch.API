using BatchAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI.BatchData
{
    public class SQLBatchData : IBatchData
    {
        private BatchContext _batchContext;

        public SQLBatchData(BatchContext batchContext)
        {
            _batchContext = batchContext;
        }
        public Batch AddBatch(Batch batch)
        {
            batch.BatchId = Guid.NewGuid();
            _batchContext.Batches.Add(batch);
            _batchContext.SaveChanges();
            
            return batch;
        }

        public List<Batch> GetBatch()
        {
            return _batchContext.Batches.ToList();
        }

        public Batch GetBatch(Guid batchId)
        {
            Batch batch = new Batch();

            batch= _batchContext.Batches
                    .Include("ACL")
                    .Include("Attributes")
                    .SingleOrDefault(p => p.BatchId.Equals(batchId));

            return batch;
        }
    }
}
