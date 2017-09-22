using System;

namespace Sprotify.Web.Models.Music
{
    public class PlaylistInYourMusic
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
    }
}
