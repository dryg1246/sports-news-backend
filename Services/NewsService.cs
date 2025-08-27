using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Dtos;

namespace SportsNewsAPI.Services;

public class NewsService
{
    private readonly HttpClient _httpClient;
    private readonly SportsNewsContext _context;


    public NewsService(HttpClient httpClient, SportsNewsContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    public async Task LoadTodayNews()
    {
        var utcToday = DateTime.UtcNow.Date;
        var utcYesterday = utcToday.AddDays(-1);

        string from = utcYesterday.ToString("yyyy-MM-ddTHH:mm:ss");
        string to = utcToday.AddDays(1).AddTicks(-1).ToString("yyyy-MM-ddTHH:mm:ss");
        
        var url =
            $"https://newsapi.org/v2/everything?q=sports&from={from}&to={to}&sortBy=popularity&language=en&apiKey=778121830edd480eba72a03f23ab9ec0";

        Console.WriteLine($"Loading url: {url}");

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "MyNewsApp/1.0");

            var responce = await client.GetAsync(url);
            var json = await responce.Content.ReadAsStringAsync();
            Console.WriteLine(json);
        }

        var response = await _httpClient.GetAsync(url);
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("News/1.0");
        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var todayNews = new News
        {
            Status = result.Status,
            TotalResults = result.TotalResults
        };

        _context.TodayNews.Add(todayNews);
        await _context.SaveChangesAsync();
        foreach (var articleDto in result.Articles)
        {

            var source = await _context.Sources
                .FirstOrDefaultAsync(s => s.Name == articleDto.Source.Name);

            if (source == null)
            {
                source = new Source { Name = articleDto.Source.Name, ApiId = articleDto.Source.ApiId ?? string.Empty };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }

            var existing = await _context.Articles
                .FirstOrDefaultAsync(a => a.Url == articleDto.Url);

            if (existing == null)
            {
                var newsArticle = new Article
                {
                    Title = articleDto.Title ?? string.Empty,
                    Url = articleDto.Url ?? string.Empty,
                    Description = articleDto.Description ?? string.Empty,
                    Author = articleDto.Author ?? string.Empty,
                    Content = articleDto.Content ?? string.Empty,
                    PublishedAt = articleDto.PublishedAt.ToUniversalTime(),
                    UrlToImage = articleDto.UrlToImage ?? string.Empty,
                    NewsId = todayNews.Id,
                    SourceId = source.Id,
                };

                _context.Articles.Add(newsArticle);
            }
            else
            {
                existing.Title = articleDto.Title ?? string.Empty;
                existing.Url = articleDto.Url ?? string.Empty;
                existing.Description = articleDto.Description ?? string.Empty;
                existing.Author = articleDto.Author ?? string.Empty;
                existing.Content = articleDto.Content ?? string.Empty;
                existing.PublishedAt = articleDto.PublishedAt.ToUniversalTime();
                existing.UrlToImage = articleDto.UrlToImage ?? string.Empty;
                existing.NewsId = todayNews.Id;
                existing.SourceId = source.Id;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteOldTodayNews()
    {
        var utcNow = DateTime.UtcNow;
        var utcYesterday = utcNow.AddDays(-1);
        var utcTomorrow = utcNow.AddDays(1);

        string from = utcYesterday.ToString("yyyy-MM-dd");
        string to = utcTomorrow.AddDays(1).AddTicks(-1).ToString("yyyy-MM-dd");

        var oldArticles = await _context.Articles
            .Where(a => a.PublishedAt < utcYesterday || a.PublishedAt >= utcTomorrow)
            .ToListAsync();

        _context.Articles.RemoveRange(oldArticles);
        await _context.SaveChangesAsync();
    }

    // public class NewsUpdaterService : BackgroundService
    // {
    //     private readonly IServiceProvider _services;
    //
    //     public NewsUpdaterService(IServiceProvider services)
    //     {
    //         _services = services;
    //     }
    //
    //     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //     {
    //         while (!stoppingToken.IsCancellationRequested)
    //         {
    //             using (var scope = _services.CreateScope())
    //             {
    //                 var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //
    //                 // вызов твоего метода очистки + загрузки статей
    //                 await DeleteOldTodayNews(context);
    //                 await LoadTodayNews(context);
    //             }
    //
    //             // ждем 24 часа
    //             await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
    //         }
    //     }


public async Task LoadFootballNews()
    {
        var utcNow = DateTime.UtcNow;
        var utcYesterday = utcNow.AddDays(-1);
        var utcTomorrow = utcNow.AddDays(1);
        
        string from = utcYesterday.ToString("yyyy-MM-dd");
        string to   = utcTomorrow.AddDays(1).AddTicks(-1).ToString("yyyy-MM-dd");
        
        var oldArticles = await _context.Articles
            .Where(a => a.PublishedAt < utcYesterday || a.PublishedAt >= utcTomorrow)
            .ToListAsync();

        _context.Articles.RemoveRange(oldArticles);
        await _context.SaveChangesAsync();
        var url =
            $"https://newsapi.org/v2/everything?q=soccer%20OR%20football%20-\\%22American%20Football\\%22&from={from}&to={to}&language=en&sortBy=publishedAt&apiKey=778121830edd480eba72a03f23ab9ec0";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyFootballApp/1.0");
        
        var response = await _httpClient.SendAsync(request);

        var result = await response.Content.ReadFromJsonAsync<NewsDto>();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);

        var footballNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        
        _context.FootballNews.Add(footballNews);
        await _context.SaveChangesAsync();

        foreach (var footballNewsDto in result.Articles)
        {
            
            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == footballNewsDto.Source.Name);

            if (source == null)
            {
                source = new Source { Name = footballNewsDto.Source.Name, ApiId = footballNewsDto.Source.ApiId ?? string.Empty };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }
            
            var existing = await _context.Articles.FirstOrDefaultAsync(a => a.Url == footballNewsDto.Url);

            if (existing == null)
            {
                var footballNewsArticle = new Article()
                {
                    Title = footballNewsDto.Title ?? string.Empty,
                    Url = footballNewsDto.Url ?? string.Empty,
                    Description = footballNewsDto.Description ?? string.Empty,
                    Author = footballNewsDto.Author ?? string.Empty,
                    Content = footballNewsDto.Content ?? string.Empty,
                    PublishedAt = footballNewsDto.PublishedAt.ToUniversalTime(),
                    UrlToImage = footballNewsDto.UrlToImage ?? string.Empty,
                    Category = "Football",
                    SourceId = source.Id,
                    NewsId = footballNews.Id,
                };

                _context.Articles.Add(footballNewsArticle);
            }
            else
            {
                existing.Title = footballNewsDto.Title ?? string.Empty;
                existing.Url = footballNewsDto.Url ?? string.Empty;
                existing.Description = footballNewsDto.Description ?? string.Empty;
                existing.Author = footballNewsDto.Author ?? string.Empty;
                existing.Content = footballNewsDto.Content ?? string.Empty;
                existing.PublishedAt = footballNewsDto.PublishedAt.ToUniversalTime();
                existing.UrlToImage = footballNewsDto.UrlToImage ?? string.Empty;
                existing.NewsId = footballNews.Id;
            }
            await _context.SaveChangesAsync();
           
        }
    }

}