using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Model;
using Presenter;

namespace TestLib;

public class DeleteEndpoint
{

    public static void MapId(WebApplication app) => app
        .MapDelete("/delete-by-id", HandleId);
    public static void MapName(WebApplication app) => app
        .MapDelete("/delete-by-name-and-author", HandleNameAndAuthor);
    
    public record Response(string Message);

    public static async Task<Results<Ok<Response>, BadRequest<string>>> HandleId(int id, ISongPresenter presenter)
    {
        if (id < 0)
            return TypedResults.BadRequest("Invalid song name or author.");
        await presenter.RemoveSongPresenter(id);
        return TypedResults.Ok(new Response("Song deleted by id successfully."));
    }
    public static async Task<Results<Ok<Response>, BadRequest<string>>> HandleNameAndAuthor(string name, string author, ISongPresenter presenter)
    {            
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
            return TypedResults.BadRequest("Invalid song name or author.");
        await presenter.RemoveSongPresenter(name, author);
        return TypedResults.Ok(new Response("Song deleted by name and author successfully."));}
}