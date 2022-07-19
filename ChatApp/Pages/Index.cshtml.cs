using ChatApp.Data.Repository;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMessageRepository _repo;

    public IndexModel(ILogger<IndexModel> logger, IMessageRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public IList<MessageModel> Chat { get; set; }

    public async Task OnGetAsync()
    {
        var messages = await _repo.GetAllAsync(50);
        Chat = messages
            .Select(x => new MessageModel {
                Id = x.Id,
                Content = x.Content,
                TimeStamp = x.TimeStamp,
                Owner = x.Owner.UserName
            })
            .ToList();
    }
}

public class MessageModel
{
    public object Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public string Owner { get; set; }
}

