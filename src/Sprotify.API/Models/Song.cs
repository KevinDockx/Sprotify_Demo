using System;

namespace Sprotify.API.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Band { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
