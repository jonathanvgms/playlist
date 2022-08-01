using System;
using System.Collections.Generic;

namespace Novit.Academia.ChallengePlaylist.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            IdSongs = new HashSet<Song>();
        }

        public int IdPlaylist { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Song> IdSongs { get; set; }
    }
}
