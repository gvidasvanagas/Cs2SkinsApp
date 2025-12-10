using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Cs2SkinsApp.E2E;

[Parallelizable(ParallelScope.None)]
public class Cs2SkinsE2ETests : PageTest
{
    private const string BaseUrl = "http://localhost:5197";

    [Test]
    public async Task ShowsHeadingAndSkins()
    {
        await Page.GotoAsync(BaseUrl);

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "CS2 Skins Explorer" }))
            .ToBeVisibleAsync();

        var firstCard = Page.Locator(".card-skin").First;
        await Expect(firstCard).ToBeVisibleAsync();

        await Expect(firstCard.Locator("h5.card-title")).ToBeVisibleAsync();
        await Expect(firstCard.GetByRole(AriaRole.Button, new() { Name = "☆ Add to favourites" }))
            .ToBeVisibleAsync();
    }


    [Test]
    public async Task Search_FiltersSkinsByName()
    {
        await Page.GotoAsync(BaseUrl);

        var cardTitles = Page.Locator(".card-skin h5.card-title");
        await Expect(cardTitles.First).ToBeVisibleAsync();

        var firstTitle = await cardTitles.First.InnerTextAsync();
        var searchFragment = firstTitle.Length > 3 ? firstTitle[..3] : firstTitle;

        var searchInput = Page.GetByPlaceholder("e.g. AK-47, Dragon Lore, AWP...");
        await searchInput.FillAsync(searchFragment);

        var visibleTitles = Page.Locator(".card-skin h5.card-title");
        var count = await visibleTitles.CountAsync();
        Assert.That(count, Is.GreaterThan(0), "Expected at least one filtered result");

        for (int i = 0; i < count; i++)
        {
            var text = await visibleTitles.Nth(i).InnerTextAsync();
            Assert.That(text, Does.Contain(searchFragment), "Filtered title did not contain search term fragment");
        }
    }

    [Test]
    public async Task AddSkinToFavourites_AndSeeItOnFavouritesPage()
    {
        await Page.GotoAsync(BaseUrl);

        var firstCard = Page.Locator(".card-skin").First;
        await Expect(firstCard).ToBeVisibleAsync();

        var title = await firstCard.Locator("h5.card-title").InnerTextAsync();

        var favButton = firstCard.GetByRole(AriaRole.Button, new() { Name = "☆ Add to favourites" });
        await favButton.ClickAsync();

        var favouritesLink = Page.GetByRole(AriaRole.Link, new() { Name = "Favourites" }).First;
        await favouritesLink.ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Favourite Skins" }))
            .ToBeVisibleAsync();

        await Expect(Page.GetByText(title)).ToBeVisibleAsync();
    }

    [Test]
    public async Task RandomSkin_AndCanToggleFavourite()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Random skin" }).ClickAsync();

        var randomCard = Page.Locator(".card-random");
        await Expect(randomCard).ToBeVisibleAsync();

        var randomTitle = randomCard.Locator("h4.card-title");
        await Expect(randomTitle).ToContainTextAsync("Random skin:");

        var randomFavButton = randomCard.GetByRole(AriaRole.Button, new() { Name = "☆ Add to favourites" });

        if (!await randomFavButton.IsVisibleAsync())
        {
            randomFavButton = randomCard.GetByRole(AriaRole.Button, new() { Name = "★ Favourited" });
        }

        await Expect(randomFavButton).ToBeVisibleAsync();
    }
}
