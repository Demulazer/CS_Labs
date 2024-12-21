using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using Presenter;


namespace TestLib;

public class SongView : ISongView
{

    
    public void InitializeApi()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<ISongPresenter, SongPresenter>();
        var app = builder.Build();
        AddEndpoint.Map(app);
        SearchEndpoint.Map(app);
        DeleteEndpoint.MapId(app);
        DeleteEndpoint.MapName(app);
        ShowEndpoint.Map(app);
        app.Run();
    }
}