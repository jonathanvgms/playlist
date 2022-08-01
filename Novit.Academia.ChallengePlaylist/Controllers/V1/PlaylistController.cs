using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novit.Academia.ChallengePlaylist.Models;
using Novit.Academia.ChallengePlaylist.ViewModel;

namespace Novit.Academia.ChallengePlaylist.Controllers.V1;

[ApiController]
[ApiVersion("1.0")] 
[Route("api/v{version:apiVersion}/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly PlaylistContexto _playlistContext;

    public PlaylistController(PlaylistContexto playlistContext)
    {
        _playlistContext = playlistContext;
    }

    [HttpGet]
    public ActionResult<List<PlaylistViewModel>> Get() => Ok(_playlistContext.Playlists.ToList().Adapt<List<PlaylistViewModel>>());


    [HttpGet("{name}")]
    public ActionResult Get(string name)
    {
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {name}");
        return Ok(playlist);
    }

    [HttpPost]
    public ActionResult Post(PlaylistViewModel playlist)
    {
        if (!ModelState.IsValid) return BadRequest("Nombre playlist incorrecto");

        _playlistContext.Add(playlist.Adapt<Playlist>());
        _playlistContext.SaveChanges();
        var playlistCreated = _playlistContext.Playlists.FirstOrDefault(x => x.Name == playlist.Name);
        return StatusCode(StatusCodes.Status201Created, playlistCreated);
    }

    [HttpPut]
    public ActionResult Put(string name, PlaylistViewModel playlist)
    {
        if (!ModelState.IsValid) return BadRequest("Nombre playlist incorrecto");
        var playlistModified = _playlistContext.Playlists.FirstOrDefault(x => x.Name == name);
        if (playlistModified == null) return NotFound($"No se encontro playlist con el nombre: {name}");
        if (playlist.Name.ToUpper() != name.ToUpper()) return StatusCode(StatusCodes.Status409Conflict, $"No es posible modificar el nombre de la playlist");
        playlistModified.Description = playlist.Description;
        _playlistContext.Entry(playlistModified).State = EntityState.Modified;
        _playlistContext.SaveChanges();
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpDelete]
    public ActionResult Delete(string name)
    {
        var playlistToRemove = _playlistContext.Playlists.FirstOrDefault(x => x.Name == name);
        if (playlistToRemove == null) return NotFound($"No se encontro playlist con el nombre: {name}");
        _playlistContext.Remove(playlistToRemove);
        _playlistContext.SaveChanges();
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpGet("{playlistName}/Songs")]
    public ActionResult GetSongs(string playlistName)
    {
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name.ToUpper() == playlistName.ToUpper());
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {playlistName}");
        return Ok(playlist.IdSongs.Adapt<List<SongViewModel>>());
    }
    
    [HttpGet("{playlistName}/Songs/{songName}")]
    public ActionResult GetSong(string playlistName, string songName)
    {
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name.ToUpper() == playlistName.ToUpper());
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {playlistName}");
        var song = playlist.IdSongs.FirstOrDefault(x => x.Name.ToUpper() == songName.ToUpper());
        return Ok(song);
    }
    
    [HttpPost("{playlistName}/Songs")]
    public ActionResult PostSong(string playlistName, SongViewModel song)
    {
        if (!ModelState.IsValid) return BadRequest("Nombre de cancion incorrecto");
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name == playlistName);
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {playlistName}");
        playlist.IdSongs.Add(song.Adapt<Song>());
        _playlistContext.SaveChanges();
        playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name == playlist.Name);
        return Ok(playlist);
    }
    
    [HttpPut("{playlistName}/Songs/{songTitle}")]
    public ActionResult PutSong(string playlistName, string songTitle, SongViewModel song)
    {
        if (!ModelState.IsValid) return BadRequest("Nombre playlist incorrecto");
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name == playlistName);
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {playlistName}");
        var songToModified = playlist.IdSongs.FirstOrDefault(x => x.Name == songTitle);
        if (songToModified == null) return NotFound($"No se encontro la cancion con el titulo: {songTitle}");
        songToModified.Artist = song.Artist;
        songToModified.Date = song.Date;
        songToModified.Album = song.Album;
        _playlistContext.Entry(playlist).State = EntityState.Modified;
        _playlistContext.SaveChanges();
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpDelete("{playlistName}/Songs/{songTitle}")]
    public ActionResult PutSong(string playlistName, string songTitle)
    {
        var playlist = _playlistContext.Playlists.FirstOrDefault(x => x.Name == playlistName);
        if (playlist == null) return NotFound($"No se encontro playlist con el nombre: {playlistName}");
        var songToRemove = playlist.IdSongs.FirstOrDefault(x => x.Name == songTitle);
        if (songToRemove == null) return NotFound($"No se encontro la cancion con el titulo: {songTitle}");
        playlist.IdSongs.Remove(songToRemove);
        _playlistContext.SaveChanges();
        return StatusCode(StatusCodes.Status204NoContent);
    }
}