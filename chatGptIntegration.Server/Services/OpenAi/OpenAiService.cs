using chatGptIntegration.Server.Configurations;
using chatGptIntegration.Server.Models;
using Microsoft.Extensions.Options;
using OpenAI_API.Chat;
using OpenAI_API.Completions;

namespace chatGptIntegration.Server.Services.ChatGpt
{
    public class ChatService : IOpenAiService
    {
        private readonly OpenAiConfig _openAiConfig;
        public ChatService(IOptionsMonitor<OpenAiConfig> optionsMonitor)
        {
            _openAiConfig = optionsMonitor.CurrentValue;
        }
        public async Task<string> AskChatGptAsync(string question)
        {
            string result = "";

            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);

            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = question;
            completionRequest.Model = OpenAI_API.Models.Model.DefaultModel;
            completionRequest.MaxTokens = 1024;

            var completions = await api.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Completions)
            {
                result += completion.Text;
            }
            return result;
        }

        public async Task<string> GetChatCompletionAsync(List<Message> messages)
        {
            var openAIMessages = new List<ChatMessage>();
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
            foreach (var message in messages)
            {
                var role = ChatMessageRole.FromString(message.role);
                openAIMessages.Add(new ChatMessage(role, message.content));
            }
            var chatCompletion = await api.Chat.CreateChatCompletionAsync(new ChatRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = openAIMessages
            });
            return chatCompletion.Choices[0].Message.Content;
        }
    }
}
