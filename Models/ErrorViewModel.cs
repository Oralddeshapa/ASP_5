using System;

namespace TeaChair.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string SratusCode { get; set; }

        public string Time { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
