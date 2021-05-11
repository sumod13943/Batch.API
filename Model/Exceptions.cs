using System;
using System.Collections.Generic;

namespace BatchAPI.Model
{
    public class Exceptions
    {
        public Guid CorrelationId { get; set; }

        public IList<Errors> Errors { get; set; }
    }

    public class Errors

    {
        public string Source { get; set; }

        public string Description { get; set; }
    }
}
