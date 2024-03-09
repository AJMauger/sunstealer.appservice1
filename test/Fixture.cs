using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public enum Browser
        {
            Chromium,
            Firefox,
            Webkit
        }

        public Microsoft.Playwright.IPlaywright? Playwright { get; private set; }
        public Lazy<Task<Microsoft.Playwright.IBrowser>>? Chromium { get; private set; }

        public async Task InitializeAsync()
        {
            var i = Microsoft.Playwright.Program.Main(new[] { "install-deps" });
            if (i != 0)
            {
                throw new Exception($"Microsoft.Playwright.Program.Main(new[] install-deps) error {i}");
            }

            i = Microsoft.Playwright.Program.Main(new[] { "install" });
            if (i != 0)
            {
                throw new Exception($"Microsoft.Playwright.Program.Main(new[] install) error {i}");
            }

            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Chromium = new Lazy<Task<Microsoft.Playwright.IBrowser>>(Playwright.Chromium.LaunchAsync( new Microsoft.Playwright.BrowserTypeLaunchOptions() { Headless=false }));
        }

        // => x.unit
        public const string PlaywrightCollection = nameof(PlaywrightCollection);
        [CollectionDefinition(PlaywrightCollection)]
        public class PlaywrightCollectionDefinition
          : ICollectionFixture<PlaywrightFixture>
        {
        }

        public async Task GotoPageAsync(string url, System.Func<Microsoft.Playwright.IPage, Task> testHandler)
        {
            if (Chromium==null)
            {
                throw new Exception($"Chromium==null");
            }

            var browser = await Chromium.Value;

            await using var context = await browser.NewContextAsync(new Microsoft.Playwright.BrowserNewContextOptions{ IgnoreHTTPSErrors = true });

            var page = await context.NewPageAsync();
            Assert.NotNull(page);
            try
            {
                var gotoResult = await page.GotoAsync(url, new Microsoft.Playwright.PageGotoOptions { WaitUntil = Microsoft.Playwright.WaitUntilState.NetworkIdle });
                Assert.NotNull(gotoResult);
                await gotoResult.FinishedAsync();
                Assert.True(gotoResult.Ok);
                await testHandler(page);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        public async Task DisposeAsync()
        {
            if (Playwright != null)
            {
                if (Chromium != null && Chromium.IsValueCreated)
                {
                    var browser = await Chromium.Value;
                    await browser.DisposeAsync();
                }
                Playwright.Dispose();
                Playwright = null;
            }
        }
    }
}



