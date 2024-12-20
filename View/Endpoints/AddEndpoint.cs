using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Model;
using Presenter;

namespace TestLib;

public class AddEndpoint
{
    static SongPresenter _songPresenterLink = new SongPresenter();

    public static void Map(WebApplication app) => app
        .MapPut("/add", Handle);
    
    public record Response(string Message);

    public static async Task<Results<Ok<Response>, BadRequest<string>>> Handle(string songName, string songAuthor)
    {
        if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(songAuthor))
            return TypedResults.BadRequest("Invalid song data.");
        await _songPresenterLink.AddSongPresenter(songName,songAuthor);
        return TypedResults.Ok(new Response("Song added successfully."));
    }
}