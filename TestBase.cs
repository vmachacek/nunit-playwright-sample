using Microsoft.Playwright;

namespace NunitPwSample;

public class TestBase
{
#pragma warning disable NUnit1032
    protected IPage page;
#pragma warning restore NUnit1032

    /// <summary>
    /// OneTimeSetUp
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // this is to simulate externally running server
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://localhost:5000");
        var app = builder.Build();
        app.UseStaticFiles();
        app.MapGet("/", async context => { await context.Response.SendFileAsync("wwwroot/index.html"); });
        
        _ = app.StartAsync();

        var browser = BrowserFactory.OpenBrowser().GetAwaiter().GetResult();

        page = await BrowserFactory.OpenNewPage(browser);
    }

    /// <summary>
    /// OneTimeTearDown
    /// </summary>
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        page?.CloseAsync().GetAwaiter().GetResult();
    }
}