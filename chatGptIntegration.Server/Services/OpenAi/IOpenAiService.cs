using chatGptIntegration.Server.Models;
using OpenAI_API.Chat;

namespace chatGptIntegration.Server.Services.ChatGpt
{
    public interface IOpenAiService
    {
        Task<string> AskChatGptAsync(string question);
        Task<string> GetChatCompletionAsync(List<Message> messages);
    }
}
