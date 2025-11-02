using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Dev
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(optional: true)
                .Build();
            // non-secret default; override via env var if desired
            var modelId = config["OPENAI_DEPLOYMENT"] ?? "gpt-4.1-mini";

            // required secrets/config — read from environment (or secret store)
            var endpoint = config["OPENAI_ENDPOINT"]
                          ?? throw new InvalidOperationException("OPENAI_ENDPOINT environment variable is not set.");
            endpoint = endpoint.TrimEnd('/'); // normalize

            var apiKey = config["OPENAI_API_KEY"]
                         ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: modelId,
                endpoint: endpoint,
                apiKey: apiKey);

            Kernel kernel = builder.Build();

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            if (chat == null)
            {
                Console.WriteLine("IChatCompletion service is not registered. Verify your builder configuration and package versions.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\nEnter your prompt (or 'exit' to quit):");
                var prompt = Console.ReadLine();
                if (string.IsNullOrEmpty(prompt) || prompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;


                var response = await chat.GetChatMessageContentsAsync(prompt);

                if (response == null || response.Count == 0)
                {
                    Console.WriteLine("\nResponse: [no messages returned]");
                }
                else
                {
                    // Simplified: the SDK returns a collection of ChatMessageContent objects that expose `Content`.
                    // Read the `Content` property directly and fallback to ToString() when necessary.
                    for (int i = 0; i < response.Count; i++)
                    {
                        var msg = response[i];
                        var text = msg?.Content ?? msg?.ToString() ?? "[no text found]";
                        Console.WriteLine($"\nResponse #{i + 1}: {text}");
                    }
                }
            }
        }
    }
}