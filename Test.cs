namespace NunitPwSample;

[TestFixture]
public class Test : TestBase
{
    [Test]
    public async Task SampleTest()
    {
        await page.GotoAsync("http://localhost:5000");
    }
}