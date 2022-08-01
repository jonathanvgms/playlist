using System.ComponentModel.DataAnnotations;

namespace Novit.Academia.ChallengePlaylist.ViewModel;

public class SongViewModel
{
    [Required]
    public string Name { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public DateTime Date { get; set; }
}