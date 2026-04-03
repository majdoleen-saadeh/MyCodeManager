using System.Text;
using System.Text.Json;

namespace MyCodeManager.Services
{
    public class GeminiAIService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiAIService(IConfiguration configuration)
        {
            _apiKey = configuration["GeminiApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<string> AskAIAsync(string prompt)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                return "❌ خطأ: المفتاح السري (API Key) غير موجود!";
            }

            try
            {
                // 1. التحديث الأهم: استخدام موديل 2.5 الجديد بدلاً من 1.5 الملغى
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = prompt } } }
                    }
                };

                var jsonBody = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseString);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text").GetString();

                    return text;
                }

                return $"❌ عذراً، جوجل رفضت الطلب: {responseString}";
            }
            catch (Exception ex)
            {
                return $"❌ حدث خطأ في النظام: {ex.Message}";
            }
        }
    }
}