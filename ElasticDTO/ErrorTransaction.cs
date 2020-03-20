using System;

namespace ElasticDTO
{
    public class ErrorTransaction
    {
        public string JmsId { get; set; }
        public string DionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public ErrorMessage Error { get; set; }
    }
}