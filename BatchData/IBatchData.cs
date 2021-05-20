using BatchAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchAPI.BatchData
{
    public interface IBatchData
    {
        List<Batch> GetBatch();

        Batch AddBatch(Batch batch);

        Batch GetBatch(Guid batchId);

        BatchFile AddBatchFile(Guid batchId, string fileName, string mimeType, string contentSize);
    }
}
