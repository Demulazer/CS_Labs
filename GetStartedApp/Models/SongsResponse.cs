using System.Collections.Generic;

namespace GetStartedApp.Models;

public class SongsResponse
{
    public List<ApiSong> Songs { get; set; } = new();
}
