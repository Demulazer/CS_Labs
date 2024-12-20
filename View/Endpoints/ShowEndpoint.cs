using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Model;
using Presenter;

namespace TestLib;

public class ShowEndpoint
{
    static SongPresenter _songPresenterLink = new SongPresenter();

    public static void Map(WebApplication app) => app
        .MapGet("/show", Handle);
    
    public record Response(List<Song> Songs);

    public static async Task<Results<Ok<Response>, NotFound, BadRequest>> Handle()
    {
        var found = await _songPresenterLink.ShowSongPresenter();
        var response = new Response(found);
        return response.Songs.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(response);
    }
}