namespace TagzApp.Providers.Instagram.Models;

public class InstagramSearchResult
{
	public InstagramSearchResultData Graphql { get; set; }

	public IEnumerable<InstagramMedia> MediaList { get => Graphql.Hashtag.MediaList; }

}

public class InstagramSearchResultData
{
	public InstagramHashtag Hashtag { get; set; }
}

public class InstagramHashtag
{
	public InstagramMediaConnection EdgeHashtagToMedia { get; set; }

	public InstagramMediaConnection EdgeHashtagToTopPosts { get; set; }

	public IEnumerable<InstagramMedia> MediaList { get => EdgeHashtagToMedia.Edges.Select(x => x.Node); }
}

public class InstagramMediaConnection
{
	public IEnumerable<InstagramMediaEdge> Edges { get; set; }
}

public class InstagramMediaEdge
{
	public InstagramMedia Node { get; set; }
}

public class InstagramMedia
{
	public string Id { get; set; }

	public string Shortcode { get; set; }

	public long TakenAtTimestamp { get; set; }

	public InstagramCaption Caption { get; set; }

	public string ThumbnailUrl { get; set; }

	public IEnumerable<InstagramImage> ThumbnailResources { get; set; }

	public InstagramUser Owner { get; set; }
}

public class InstagramCaption
{
	public IEnumerable<InstagramCaptionEdge> Edges { get; set; }

	public string Text { get => Edges.First().Node.Text; }
}

public class InstagramCaptionEdge
{
	public InstagramCaptionNode Node { get; set; }
}

public class InstagramCaptionNode
{
	public string Text { get; set; }
}

public class InstagramUser
{
	public string Id { get; set; }

	public string Username { get; set; }

	public string ProfileImageUrl { get; set; }

	public Uri ProfileImageUri { get => new Uri(ProfileImageUrl); }
}

public class InstagramImage
{
	public int Width { get; set; }

	public int Height { get; set; }

	public string Url { get; set; }
}

