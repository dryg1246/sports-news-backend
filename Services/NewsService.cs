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
            $"https://newsapi.org/v2/everything?q=soccer%20OR%20football%20-\\%22American%20Football\\%22&source=bleacher-report,espn&from={from}&to={to}&language=en&sortBy=publishedAt&apiKey=778121830edd480eba72a03f23ab9ec0";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyFootballApp/1.0");
        
        var response = await _httpClient.SendAsync(request);

        var result = await response.Content.ReadFromJsonAsync<NewsDto>(); ;

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

    public async Task LoadRecentNews()
    {
        // var utcNow = DateTime.UtcNow;
        // var from = utcNow.AddDays(-10);
        // var to = utcNow.AddDays(1);
        //
        // var oldRecentNews = await _context.Articles.Where(a => a.PublishedAt < utcNow || a.PublishedAt > from).ToListAsync();
        // _context.Articles.RemoveRange(oldRecentNews);
        // await _context.SaveChangesAsync();

        var url = "https://newsapi.org/v2/top-headlines?category=sports&language=en&apiKey=778121830edd480eba72a03f23ab9ec0";
        var request = new HttpRequestMessage(HttpMethod.Get, url); 
        request.Headers.UserAgent.ParseAdd("MyNewsApp/1.0");
        
        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var recentNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        
         _context.RecentNews.Add(recentNews);
        await _context.SaveChangesAsync();

        foreach (var recentNewsDto in result.Articles)
        {

            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == recentNewsDto.Source.Name);

            if (source == null)
            {
                source = new Source()
                {
                    Name = recentNewsDto.Source.Name,
                    ApiId = recentNewsDto.Source.ApiId ?? string.Empty,
                };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }
            var existing = await _context.Articles.FirstOrDefaultAsync(a => a.Url == recentNewsDto.Url);

            if (existing == null)
            {
                var recentNewsArticle = new Article()
                {
                    Title = recentNewsDto.Title ?? string.Empty,
                    Url = recentNewsDto.Url ?? string.Empty,
                    Description = recentNewsDto.Description ?? string.Empty,
                    Author = recentNewsDto.Author ?? string.Empty,
                    Content = recentNewsDto.Content ?? string.Empty,
                    PublishedAt = recentNewsDto.PublishedAt.ToUniversalTime(),
                    UrlToImage = recentNewsDto.UrlToImage ?? string.Empty,
                    NewsId = recentNews.Id,
                    Category = "Recent",
                    SourceId = recentNews.Id,
                };
                
                _context.Articles.Add(recentNewsArticle);
            }
            else
            {
                existing.Title = recentNewsDto.Title ?? string.Empty;
                existing.Url = recentNewsDto.Url ?? string.Empty;
                existing.Description = recentNewsDto.Description ?? string.Empty;
                existing.Author = recentNewsDto.Author ?? string.Empty;
                existing.Content = recentNewsDto.Content ?? string.Empty;
                existing.PublishedAt = recentNewsDto.PublishedAt.ToUniversalTime();
                existing.UrlToImage = recentNewsDto.UrlToImage ?? string.Empty;
                existing.NewsId = recentNews.Id;
            }
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task LoadHockeyNews()
    {
        var url = "https://newsapi.org/v2/everything?q=hockey&sources=espn,nhl&sortBy=relevancy&language=en&apiKey=778121830edd480eba72a03f23ab9ec0";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyNewsApp/1.0");
        
        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var hockeyNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        _context.HockeyNews.Add(hockeyNews);
        await _context.SaveChangesAsync();

        foreach (var hockeyNewsDto in result.Articles)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == hockeyNewsDto.Source.Name);

            if (source == null)
            {
                source = new Source()
                {
                    Name = hockeyNewsDto.Source.Name,
                    ApiId = hockeyNewsDto.Source.ApiId ?? string.Empty,
                };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }
            
            var existing = await _context.Articles.FirstOrDefaultAsync(a => a.Url == hockeyNewsDto.Url);

            if (existing == null)
            {
                var hockeyNewsArticle = new Article()
                {
                    Title = hockeyNewsDto.Title ?? string.Empty,
                    Url = hockeyNewsDto.Url ?? string.Empty,
                    Description = hockeyNewsDto.Description ?? string.Empty,
                    Author = hockeyNewsDto.Author ?? string.Empty,
                    Content = hockeyNewsDto.Content ?? string.Empty,
                    PublishedAt = hockeyNewsDto.PublishedAt.ToUniversalTime(),
                    UrlToImage = hockeyNewsDto.UrlToImage ?? string.Empty,
                    NewsId = hockeyNews.Id,
                    Category = "Hockey",
                    SourceId = source.Id,
                };
                
                _context.Articles.Add(hockeyNewsArticle);
            }
            else
            {
                existing.Title = hockeyNewsDto.Title ?? string.Empty;
                existing.Url = hockeyNewsDto.Url ?? string.Empty;
                existing.Description = hockeyNewsDto.Description ?? string.Empty;
                existing.Author = hockeyNewsDto.Author ?? string.Empty;
                existing.Content = hockeyNewsDto.Content ?? string.Empty;
                existing.PublishedAt = hockeyNewsDto.PublishedAt.ToUniversalTime();
                existing.UrlToImage = hockeyNewsDto.UrlToImage ?? string.Empty;
                existing.NewsId = hockeyNews.Id;
                existing.SourceId = source.Id;
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task LoadBasketballNews()
    {
        var url = "https://newsapi.org/v2/everything?q=basketball&sources=bleacher-report,espn&sortBy=relevancy&language=en&apiKey=778121830edd480eba72a03f23ab9ec0";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyNewsApp/1.0");

        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var basketballNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        _context.BasketballNews.Add(basketballNews);
        await _context.SaveChangesAsync();


        foreach (var basketballNewsDTO in result.Articles)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == basketballNewsDTO.Source.Name);

            if (source == null)
            {
                source = new Source()
                {
                    Name = basketballNewsDTO.Source.Name,
                    ApiId = basketballNewsDTO.Source.ApiId ?? string.Empty,
                };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }
            
            var existing = await _context.Articles.FirstOrDefaultAsync(a => a.Url == basketballNewsDTO.Url);

            if (existing == null)
            {
                var basketballNewsArticle = new Article()
                {
                    Title = basketballNewsDTO.Title ?? string.Empty,
                    Url = basketballNewsDTO.Url ?? string.Empty,
                    Description = basketballNewsDTO.Description ?? string.Empty,
                    Author = basketballNewsDTO.Author ?? string.Empty,
                    Content = basketballNewsDTO.Content ?? string.Empty,
                    PublishedAt = basketballNewsDTO.PublishedAt.ToUniversalTime(),
                    UrlToImage = basketballNewsDTO.UrlToImage ?? string.Empty,
                    NewsId = basketballNews.Id,
                    Category = "Basketball",
                    SourceId = source.Id,
                };
                _context.Articles.Add(basketballNewsArticle);
            }
            else
            {
                existing.Title = basketballNewsDTO.Title ?? string.Empty;
                existing.Url = basketballNewsDTO.Url ?? string.Empty;
                existing.Description = basketballNewsDTO.Description ?? string.Empty;
                existing.Author = basketballNewsDTO.Author ?? string.Empty;
                existing.Content = basketballNewsDTO.Content ?? string.Empty;
                existing.PublishedAt = basketballNewsDTO.PublishedAt.ToUniversalTime();
                existing.UrlToImage = basketballNewsDTO.UrlToImage ?? string.Empty;
                existing.NewsId = basketballNews.Id;
                existing.SourceId = source.Id;
            }

            await _context.SaveChangesAsync();
        }
        
    }

    public async Task LoadBadmintonNews()
    {
        var url =
            "https://newsapi.org/v2/everything?q=badminton&language=en&sortBy=publishedAt&apiKey=778121830edd480eba72a03f23ab9ec0";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyNewsApp/1.0");

        var response = await _httpClient.SendAsync(request);

        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var badmintonNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        _context.BadmintonNews.Add(badmintonNews);
        await _context.SaveChangesAsync();

        foreach (var badmintonNewsDTO in result.Articles)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == badmintonNewsDTO.Source.Name);
            if (source == null)
            {
                source = new Source()
                {
                    Name = badmintonNewsDTO.Source.Name,
                    ApiId = badmintonNewsDTO.Source.ApiId ?? string.Empty,
                };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }

            var existing = _context.Articles.FirstOrDefault(a => a.Url == badmintonNewsDTO.Url);

            if (existing == null)
            {
                var badmintonArticles = new Article()
                {
                    Title = badmintonNewsDTO.Title ?? String.Empty,
                    Author = badmintonNewsDTO.Author ?? String.Empty,
                    Url = badmintonNewsDTO.Url ?? String.Empty,
                    UrlToImage = badmintonNewsDTO.UrlToImage ?? String.Empty,
                    Category = "Badminton",
                    Content = badmintonNewsDTO.Content ?? String.Empty,
                    Description = badmintonNewsDTO.Description ?? String.Empty,
                    NewsId = badmintonNews.Id,
                    PublishedAt = badmintonNewsDTO.PublishedAt,
                    SourceId = source.Id,
                };
                _context.Articles.Add(badmintonArticles);
            }
            else
            {
                existing.Title = badmintonNewsDTO.Title ?? String.Empty;
                existing.Author = badmintonNewsDTO.Author ?? String.Empty;
                existing.Url = badmintonNewsDTO.Url ?? String.Empty;
                existing.UrlToImage = badmintonNewsDTO.UrlToImage ?? String.Empty;
                existing.Content = badmintonNewsDTO.Content ?? String.Empty;
                existing.Description = badmintonNewsDTO.Description ?? String.Empty;
                existing.PublishedAt = badmintonNewsDTO.PublishedAt;
                existing.NewsId = badmintonNews.Id;
                existing.SourceId = source.Id;
            }

            await _context.SaveChangesAsync();
        }

    }

    public async Task LoadTrendingNews()
    {
         var url =
            "https://newsapi.org/v2/top-headlines?language=en&apiKey=778121830edd480eba72a03f23ab9ec0";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd("MyNewsApp/1.0");

        var response = await _httpClient.SendAsync(request);

        var result = await response.Content.ReadFromJsonAsync<NewsDto>();

        var trendingNews = new News()
        {
            Status = result.Status,
            TotalResults = result.TotalResults,
        };
        _context.BadmintonNews.Add(trendingNews);
        await _context.SaveChangesAsync();

        foreach (var trendingNewsDTO in result.Articles)
        {
            var source = await _context.Sources.FirstOrDefaultAsync(a => a.Name == trendingNewsDTO.Source.Name);
            if (source == null)
            {
                source = new Source()
                {
                    Name = trendingNewsDTO.Source.Name,
                    ApiId = trendingNewsDTO.Source.ApiId ?? string.Empty,
                };
                _context.Sources.Add(source);
                await _context.SaveChangesAsync();
            }

            var existing = _context.Articles.FirstOrDefault(a => a.Url == trendingNewsDTO.Url);

            if (existing == null)
            {
                var badmintonArticles = new Article()
                {
                    Title = trendingNewsDTO.Title ?? String.Empty,
                    Author = trendingNewsDTO.Author ?? String.Empty,
                    Url = trendingNewsDTO.Url ?? String.Empty,
                    UrlToImage = trendingNewsDTO.UrlToImage ?? String.Empty,
                    Category = "Trending",
                    Content = trendingNewsDTO.Content ?? String.Empty,
                    Description = trendingNewsDTO.Description ?? String.Empty,
                    NewsId = trendingNews.Id,
                    PublishedAt = trendingNewsDTO.PublishedAt,
                    SourceId = source.Id,
                };
                _context.Articles.Add(badmintonArticles);
            }
            else
            {
                existing.Title = trendingNewsDTO.Title ?? String.Empty;
                existing.Author = trendingNewsDTO.Author ?? String.Empty;
                existing.Url = trendingNewsDTO.Url ?? String.Empty;
                existing.UrlToImage = trendingNewsDTO.UrlToImage ?? String.Empty;
                existing.Content = trendingNewsDTO.Content ?? String.Empty;
                existing.Description = trendingNewsDTO.Description ?? String.Empty;
                existing.PublishedAt = trendingNewsDTO.PublishedAt;
                existing.NewsId = trendingNews.Id;
                existing.SourceId = source.Id;
            }

            await _context.SaveChangesAsync();
        }
    }

}