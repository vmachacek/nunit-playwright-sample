using System.Dynamic;
using Microsoft.Playwright;

namespace NunitPwSample;

public static class BrowserFactory
{
    public static async Task<IBrowser> OpenBrowser()
    {
        Environment.SetEnvironmentVariable("PW_EXPERIMENTAL_SERVICE_WORKER_NETWORK_EVENTS", "1");


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

    public static async Task<IPage> OpenNewPage(IBrowser browser, bool isMobile = false)
    {
        var contextOpts = new BrowserNewContextOptions()
        {
            ViewportSize = ViewportSize.NoViewport,
            Permissions = new[] { "clipboard-read", "clipboard-write" },
        };
        IBrowserContext context;
        if (isMobile)
        {
            var playwright = await Playwright.CreateAsync();
            var iphone13 = playwright.Devices["iPhone 13"];
            context = await browser.NewContextAsync(iphone13);
        }
        else
        {
            context = await browser.NewContextAsync(contextOpts);
        }


        var page = await context.NewPageAsync();

        // this will get executed by each page load - when browser executes
        await page.AddInitScriptAsync("""
                                      function addNoAnimationsClass() { document.querySelector('html').classList.add('no-animations');}
                                      const times = [1, 10, 50, 100,200,300,400,500,600,700];
                                      for (let i = 0; i < times.length; i++) { setTimeout(addNoAnimationsClass, times[i]); }
                                      """);

        await page.ExposeFunctionAsync("reportErrorToTestRunner", (object error, string url) =>
        {
            // here I want to fail the test
            return "";
        });


        return page;
    }
}