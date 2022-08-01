using System;
using System.Collections.Generic;

namespace Novit.Academia.ChallengePlaylist.Models
{
    public partial class Song
    {
        public Song()
        {
            IdPlaylists = new HashSet<Playlist>();
        }

        public int IdSong { get; set; }
        public string Name { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Album { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual ICollection<Playlist> IdPlaylists { get; set; }
    }
}
