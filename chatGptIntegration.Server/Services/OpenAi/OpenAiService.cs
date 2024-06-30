using chatGptIntegration.Server.Models;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chatGptIntegration.Server.Services.ChatGpt
{
    public class ChatService : IOpenAiService
    {
        private readonly string _openAiApiKey;
        public ChatService()
        {
            _openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        }

        public async Task<string> AskChatGptAsync(string question)
        {
            string result = "";

            var api = new OpenAIAPI(_openAiApiKey);

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
            var api = new OpenAIAPI(_openAiApiKey);
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
