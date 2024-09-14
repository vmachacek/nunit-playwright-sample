using System.Dynamic;
using Microsoft.Playwright;

namespace NunitPwSample;

public static class BrowserFactory
{
    public static async Task<IBrowser> OpenBrowser()
    {
        var browserParams = new[] { "--start-maximized", "--auto-open-devtools-for-tabs" };

        var playwright = await Playwright.CreateAsync();

        return await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions()
            {
                Channel = "chrome",
                Args = browserParams,
                Headless = false,
            });
    }

    public static async Task<IPage> OpenNewPage(IBrowser browser)
    {
        var contextOpts = new BrowserNewContextOptions()
        {
            ViewportSize = ViewportSize.NoViewport,
            Permissions = new[] { "clipboard-read", "clipboard-write" },
        };
        
        var page = await (await browser.NewContextAsync(contextOpts)).NewPageAsync();

        await page.ExposeFunctionAsync("reportErrorToTestRunner", (object error, string url) =>
        {
            // here I want to fail the test
            return "";
        });
        
        return page;
    }
}