using Microsoft.Playwright.NUnit;

namespace Tests;

public class LoginTests : PageTest
{
    [Test]
    public async Task Login_Page_Should_Load()
    {
        await Page.GotoAsync("http://localhost:5041/login");

        await Expect(Page.Locator("input[name='username']")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[name='password']")).ToBeVisibleAsync();
    }
}
