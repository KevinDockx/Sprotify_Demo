using System;

namespace Sprotify.Web.Models.Api
{
    public class Playlist
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid OwnerId { get; set; }
    }
}
