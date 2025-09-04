using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace TeaDiary.Api.Tests
{
    public class TeaApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public TeaApiTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task GetTeas_ReturnsSuccessAndContent()
        {
            // Выполняем GET запрос
            var response = await _client.GetAsync("/api/tea");

            // Проверяем, что статус 200 OK
            response.EnsureSuccessStatusCode();

            // Получаем содержимое (тело) ответа
            var content = await response.Content.ReadAsStringAsync();

            // Проверяем, что содержимое не пустое
            Assert.False(string.IsNullOrEmpty(content));
        }

        [Fact]
        public async Task GetTea_ById_ReturnsSuccess()
        {
            var teaId = "6ced17d9-72a9-4d13-85dd-03a43340a263";

            // Выполняем GET запрос по адресу с Id
            var response = await _client.GetAsync($"/api/tea/{teaId}");

            // Проверяем успешность запроса
            response.EnsureSuccessStatusCode();

            // Получаем содержимое (тело) ответа
            var content = await response.Content.ReadAsStringAsync();

            // Проверяем, что содержимое не пустое
            Assert.False(string.IsNullOrEmpty(content));
        }

        [Fact]
        public async Task PostTea_CreatesNewTea_ReturnsCreatedTea()
        {
            var newTea = new
            {
                Name = "Test Tea " + Guid.NewGuid(),
                TeaTypeId = (Guid?)null,
                YearCollection = "2025",
                Quantity = 10,
                Price = 100,
                LinkPurchase = "https://example.com",
                WouldBuyAgain = true,
                Description = "Test description",
                UserId = "3e110463-5d1a-405e-80fa-926157ec01ef",
                LinkToPhoto = "https://example.com/photo.jpg"
            };

            var response = await _client.PostAsJsonAsync("/api/tea", newTea);

            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine("Response body: " + responseContent);

            Assert.True(response.IsSuccessStatusCode, "Response status code indicates failure.");
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTea_ById_ReturnsNoContent()
        {
            // Сначала создаём чай, чтобы гарантировать его наличие для удаления
            var newTea = new
            {
                Name = "Test Tea " + Guid.NewGuid(),
                TeaTypeId = (Guid?)null,
                YearCollection = "2025",
                Quantity = 10,
                Price = 100,
                LinkPurchase = "https://example.com",
                WouldBuyAgain = true,
                Description = "Test description",
                UserId = "3e110463-5d1a-405e-80fa-926157ec01ef",
                LinkToPhoto = "https://example.com/photo.jpg"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/tea", newTea);
            createResponse.EnsureSuccessStatusCode();

            var createdContent = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            string createdId = createdContent.GetProperty("id").GetString();

            // Далее используем createdId для удаления
            var deleteResponse = await _client.DeleteAsync($"/api/tea/{createdId}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Проверяем, что сущность больше недоступна
            var getResponse = await _client.GetAsync($"/api/tea/{createdId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

    }
}
