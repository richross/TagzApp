using System.Net.Mime;
using System.Net;
using System.Text.Json;
using System.Web;
using TagzApp.Common;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using TagzApp.Providers.Instagram.Models;

namespace TagzApp.Providers.Instagram;

public class InstagramProvider : ISocialMediaProvider
{
	public string Id => "INSTAGRAM";
	public string DisplayName => "Instagram";
	public TimeSpan NewContentRetrievalFrequency => TimeSpan.FromMinutes(1);

	private readonly HttpClient _HttpClient;
	private readonly ILogger<InstagramProvider> _Logger;

	public InstagramProvider(IHttpClientFactory httpClientFactory, ILogger<InstagramProvider> logger)
	{
		_HttpClient = httpClientFactory.CreateClient("InstagramClient");
		_Logger = logger;
	}

	public async Task<IEnumerable<Content>> GetContentForHashtag(Hashtag tag, DateTimeOffset since)
	{
		var response = await _HttpClient.GetAsync($"https://www.instagram.com/explore/tags/{tag.Text}/?__a=1");
		if (response.StatusCode == HttpStatusCode.NotFound)
		{
			return Enumerable.Empty<Content>();
		}

		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		var result = JsonSerializer.Deserialize<InstagramSearchResult>(json, options);
		return result.MediaList.Select(m => new Content
		{
			Provider = this.Id,
			ProviderId = m.Id,
			SourceUri = new Uri($"https://www.instagram.com/p/{m.Shortcode}"),
			Author = new Creator
			{
				ProfileUri = new Uri($"https://www.instagram.com/{m.Owner.Username}/"),
				ProfileImageUri = new Uri(m.Owner.ProfileImageUrl),
				DisplayName = m.Owner.Username
			},
			Text = HttpUtility.HtmlEncode(m.Caption),
			Type = Common.ContentType.Image, //TODO: change if the type of content is not a photo
			Timestamp = DateTimeOffset.FromUnixTimeSeconds(m.TakenAtTimestamp)
		});
	}
}

