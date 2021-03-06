using BatchAPI.BatchData;
using BatchAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using static BatchAPI.Model.Batch;

namespace BatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BatchController : Controller
    {
        private readonly IBatchData _batchData;
        private readonly ILogger<BatchController> _logger;
        //private readonly string _logSource = "Batch API";

        public BatchController(IBatchData batchData, ILogger<BatchController> logger)
        {
            _batchData = batchData;
            _logger = logger;

            //if (!EventLog.SourceExists(_logSource))
            //    EventLog.CreateEventSource(_logSource, "Logger");
        }

        //[HttpGet]
        //public IActionResult Batch()
        //{
        //    return Ok(_batchData.GetBatch());
        //}

        /// <summary>
        /// Create a new batch to upload files into.
        /// </summary>
        /// <param name="batch"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request - there are one or more errors in the specified patameters</response>
        /// <response code="401">Unauthorised - either you have not provided any credentials, or your credentials are not recognised.</response>
        /// <response code="403">Forbidden - you have been authorised, but you are not allowed to access this resource.</response>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Batch(Batch batch)
        {
            _logger.Log(LogLevel.Information, "Adding a new batch");

            try
            {
                if (ModelState.IsValid)
                {
                    _batchData.AddBatch(batch);
                    CreatedAtActionResult result = new CreatedAtActionResult("Batch", "Batch", "", new { batchId = batch.BatchId });

                    _logger.Log(LogLevel.Information, "New batch added");

                    return result;
                }
                else
                {
                    _logger.Log(LogLevel.Warning, $"Bad Request, Error(s):-", ModelState.ErrorCount);
                    return BadRequest("Bad Request");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Get details of the batch including links to all the files in the batch.
        /// </summary>
        /// <param name="batchId"> A batch ID</param>
        /// <remarks>This get will include full details of the batch, for example it's status, the files in the batch.</remarks>
        /// <response code="200">OK - Return details about the batch.</response>
        /// <response code="400">Bad request - could be an invalid batch ID format. Batch IDs should be a GUID. A valid GUID that doesn't match a batch ID will return a 404.</response>
        /// <response code="401">Unauthorised - either you have not provided any credentials, ot your credentials are not recognised.</response>
        /// <response code="403">Forbidden - you have been authorised, but you are not allowed to access this resource.</response>
        /// <response code="404">Not Found - Could be that the batch ID doesn't exist.</response>
        /// <response code="410">Gone - the batch has been expired and is no longer available.</response>
        /// <returns></returns>

        [HttpGet]
        [Route("{batchId}")]
        public IActionResult Batch(Guid batchId)
        {
            if (batchId == Guid.Empty)
            {
                _logger.Log(LogLevel.Error, $"Bad Request - could be an invalid batch ID format. Batch IDs should be a GUID. A valid GUID that doesn't match a batch ID will return a 404");
                return BadRequest("Bad Request - could be an invalid batch ID format. Batch IDs should be a GUID. A valid GUID that doesn't match a batch ID will return a 404");
            }

            _logger.Log(LogLevel.Information, "Getting batch with id={ID}", batchId);

            try
            {
                Batch batch = _batchData.GetBatch(batchId);

                if (batch == null)
                {
                    _logger.Log(LogLevel.Warning, $"Batch with given id = {batchId} does not exists");
                    return NotFound("Not- Found - Could that be the batch ID does not exists");
                }
                else
                {
                    if (batch.ExpiryDate < DateTime.Now) //expiry date check
                    {
                        _logger.Log(LogLevel.Warning, "Gone - the batch has been expired and is no longer available");
                        return StatusCode(410, "Gone - the batch has been expired and is no longer available");
                    }
                }

                _logger.Log(LogLevel.Information, "Batch details retrieved");
                return Ok(batch);

            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Add a file to the batch
        /// </summary>
        /// <param name="batchId">A Batch ID</param>
        /// <param name="fileName">Filename for the new file. Must be unique in the batch (but can be the same as another file in another batch). Filenames don't include a path</param>
        /// <param name="mimeType">Optional. The MIME content type of the file. The default type is application/octet-stream'</param>
        /// <param name="contentSize">The final size of the file in bytes.</param>
        /// <remarks>Creates a file in the batch. To upload the content of the file,one or more uploadBlockofFile requests will need to be made followed by a 'putBlocksinFile' request to complete the file. </remarks>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request - Could be a bad batch ID; a batch ID that doesn't exist; a bad filename</response>
        /// <response code="401">Unauthorised - either you have not provided any credentials, or your credentials are not recognised.</response>
        /// <response code="403">Forbidden - you have been authorised, but you are not allowed to access this resource.</response>
        /// <returns></returns>

        [HttpPost]
        [Route("{batchId}/{fileName}")]
        public IActionResult Batch(Guid batchId, string fileName,
                                [FromHeader(Name = "X-Content-Size")] string contentSize,
                                [FromHeader(Name = "X-MIME-Type")] string mimeType = "application/octet-stream")
        {
            ModelState.Clear();
            _logger.Log(LogLevel.Information, "Adding a new batch file");

            BatchFileValidator validator = new BatchFileValidator();

            try
            {
                BatchFile batchFile = _batchData.AddBatchFile(batchId, fileName, mimeType, contentSize);
                var result = validator.Validate(batchFile);
                if (result.IsValid)
                {
                    _logger.Log(LogLevel.Information, "New batch file added");
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("Batch", "Could be a bad batch ID; a batch ID that doesn't exist; a bad filename");
                    _logger.Log(LogLevel.Warning, $"Bad Request, Error(s):- Batch Id doesn't exists.");
                    return BadRequest("Bad Request - Could be a bad batch ID; a batch ID that doesn't exist; a bad filename");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest();
            }
        }
    }
}
