using StackSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PostController : Controller
{
    private readonly StackoverflowContext _context;

    public PostController(StackoverflowContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, int skip = 0)
    {
        if (_context.Posts == null)
        {
            return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        if(string.IsNullOrEmpty(searchString)){
            return View(new PostsListViewModel {Posts = [], HasMore = false, hasSearched = false});
        }
        // var query = _context.Posts.Take(100).Where(string.IsNullOrEmpty(searchString) ? p => true : p => (p.Body.Contains(searchString) || p.Title!.Contains(searchString) || p.Tags!.Contains(searchString)));
        // var query = _context.Posts.Select(p => new Post
        // {
        //     Id = p.Id,
        //     Title = p.Title,
        //     Body = p.Body,
        //     Tags = p.Tags,
        //     // Set other properties
        //     AnswerCount = p.AnswerCount,
        //     VoteCount = _context.Votes.Where(v => v.PostId == p.Id).Sum(v => v.VoteTypeId),
        //     UserReputation = p.OwnerUserId.HasValue ? _context.Users.Find(p.OwnerUserId.Value)!.Reputation : 0,
        //     UserName = p.OwnerUserId.HasValue ? _context.Users.Find(p.OwnerUserId.Value)!.DisplayName : null,
        //     UserBadgeCount = p.OwnerUserId.HasValue ? _context.Badges.Where(b => b.UserId == p.OwnerUserId.Value).Count() : 0,
        //     // VoteType = _context.Votes.Where(v => v.PostId == p.Id).FirstOrDefault()?.VoteTypeId == 2 ? "UpVote" : null,
        // });
        var query = from p in _context.Posts
                    select new Post
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            SummarisedBody = p.Body!.Substring(0, Math.Min(p.Body.Length, 140)),
            Tags = p.Tags,
            // Set other properties
            AnswerCount = p.AnswerCount,
            VoteCount = _context.Votes.Where(v => v.PostId == p.Id).Sum(v => v.VoteTypeId),
            UserReputation = p.OwnerUserId.HasValue ? _context.Users.Where(u => u.Id == p.OwnerUserId).Select(u => u.Reputation).FirstOrDefault() : 0,
            UserName = p.OwnerUserId.HasValue ? _context.Users.Where(u => u.Id == p.OwnerUserId)
                        .Select(u => u.DisplayName)
                        .FirstOrDefault() : null,
            UserBadgeCount = p.OwnerUserId.HasValue ? _context.Badges.Where(b => b.UserId == p.OwnerUserId).Count() : 0,
            // VoteType = _context.Votes.Where(v => v.PostId == p.Id).FirstOrDefault()?.VoteTypeId == 2 ? "UpVote" : null,
        };
         if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(p => EF.Functions.Like(p.Title, $"%{searchString}%")
            // p.Title!.Contains(searchString) || p.Tags!.Contains(searchString)
            // || p.Tags!.Contains(searchString)
            );
        }
        var posts = await query.OrderByDescending(p => p.VoteCount).Skip(skip).Take(10).ToListAsync();
        var hasMore = _context.Posts.Count() > skip + 10;

        return View(new PostsListViewModel {Posts = posts, HasMore = hasMore, hasSearched = true});
    }

    public class PostsListViewModel
    {
        public IEnumerable<Post> Posts { get; set; } = null!;
        public bool HasMore { get; set; }

        public bool hasSearched {get; set;}
    }

}