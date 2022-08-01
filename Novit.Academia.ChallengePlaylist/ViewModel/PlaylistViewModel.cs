using System.ComponentModel.DataAnnotations;

namespace Novit.Academia.ChallengePlaylist.ViewModel;

public class PlaylistViewModel
{
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}