using System;

namespace openai_api.DTO
{
    public class csvDTO
    {
        public string? userEmail { get; set; }
        public string? clientIP { get; set; }
        public string? question { get; set; }
        public string? answer { get; set; }
        public bool dlpAns { get; set; }
        public DateTime date { get; set; }
        public string? type { get; set; }
    }
}
