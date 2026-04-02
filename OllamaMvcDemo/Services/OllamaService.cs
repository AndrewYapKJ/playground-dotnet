using System.Net.Http.Json;

namespace OllamaMvcDemo.Services;

public class OllamaService
{
    private readonly HttpClient _http;

    public OllamaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new
        {
            model = "llama3:8b",
            prompt = prompt,
            stream = false
        };

        var response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/generate",
            request
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

        return result?.response ?? "";
    }
}

public class OllamaResponse
{
    public string response { get; set; } = "";
}