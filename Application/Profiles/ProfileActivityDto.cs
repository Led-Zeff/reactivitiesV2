using System;
using System.Text.Json.Serialization;

namespace Application.Profiles
{
    public class ProfileActivityDto
    {
        public Guid ActivityId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        [JsonIgnore]
        public string HostUsername { get; set; }
    }
}