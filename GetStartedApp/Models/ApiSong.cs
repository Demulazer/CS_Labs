namespace GetStartedApp.Models;

public record ApiSong(int Id, ApiSongName SongName, ApiSongAuthor SongAuthor);
public record ApiSongName(string Name);
public record ApiSongAuthor(string Author);