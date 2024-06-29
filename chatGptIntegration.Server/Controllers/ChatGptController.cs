using chatGptIntegration.Server.Models;
using chatGptIntegration.Server.Services.ChatGpt;
using Microsoft.AspNetCore.Mvc;

namespace chatGptIntegration.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGptController : ControllerBase
    {
        private readonly IOpenAiService _chatService;

        public ChatGptController(IOpenAiService chatService)
        {
            _chatService = chatService;
        }
        [HttpGet("ask")]
        public async Task<IActionResult> Get(string Question)
        {
            var response = await _chatService.AskChatGptAsync(Question);

            return Ok(new
            {
                text = response,
                sender = "gpt",
            });
        }

        [HttpPost("chat")]
        public async Task<IActionResult> GetChatCompletionAsync([FromBody] ChatRequestDto chatRequest)
        {
            var response = await _chatService.GetChatCompletionAsync(chatRequest.Messages);
            return Ok(new { text = response, sender = "gpt" });
        }

    }
}
