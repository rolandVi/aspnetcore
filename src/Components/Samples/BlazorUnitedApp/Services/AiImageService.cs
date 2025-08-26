// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorUnitedApp.Services;

public class AiImageService
{
    private readonly HttpClient _http;
    private readonly ILogger<AiImageService> _logger;
    private readonly IConfiguration _config;

    public AiImageService(HttpClient http, ILogger<AiImageService> logger, IConfiguration config)
    {
        _http = http;
        _logger = logger;
        _config = config;
    }

    private record OpenAiImageRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("prompt")] string Prompt
    );

    private sealed class OpenAiImageResponse
    {
        [JsonPropertyName("data")] public List<OpenAiImageData> Data { get; set; } = new();
    }

    private sealed class OpenAiImageData
    {
        [JsonPropertyName("b64_json")] public string? Base64Json { get; set; }
    }

    public async Task<byte[]?> GenerateImageAsync(string prompt, CancellationToken cancellationToken)
    {
        var apiKey = _config["OPENAI_API_KEY"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning("OPENAI_API_KEY not configured; returning placeholder");
            return null;
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/images");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        var payload = new OpenAiImageRequest("gpt-image-1", prompt);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

        using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        resp.EnsureSuccessStatusCode();
        await using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
        var data = await JsonSerializer.DeserializeAsync<OpenAiImageResponse>(stream, cancellationToken: cancellationToken);
        var b64 = data?.Data.FirstOrDefault()?.Base64Json;
        if (string.IsNullOrEmpty(b64))
        {
            return null;
        }
        return Convert.FromBase64String(b64);
    }
}
