using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Model;
using Presenter;

namespace TestLib;

public class SearchEndpoint
{
    static SongPresenter _songPresenterLink = new SongPresenter();

    public static void Map(WebApplication app) => app
        .MapGet("/search", Handle);
    
    public record Response(List<Song> Songs);

    public static async Task<Results<Ok<Response>, NotFound, BadRequest>> Handle(string find)
    {
        if (string.IsNullOrEmpty(find))
            return TypedResults.BadRequest();
        var found = await _songPresenterLink.SongSearchPresenter(find);
        var response = new Response(found);
        return response.Songs.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(response);
    }
}
