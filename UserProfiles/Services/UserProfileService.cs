using System.Text.Json;

namespace LearningBackgroundServices.UserProfiles.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly HttpClient _httpClient;

        public UserProfileService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient();
        }

        public async Task<UserProfileResponse> GetUserProfile(int pageNumber)
        {
            string requestUri = $"https://reqres.in/api/users?page={pageNumber}";

            var response = await _httpClient.GetAsync(requestUri);
           
            response.EnsureSuccessStatusCode();

            dynamic content = await response.Content.ReadAsStringAsync();

            UserProfileResponse userProfileResponse = JsonSerializer.Deserialize<UserProfileResponse>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return userProfileResponse;
        }
    }
}
