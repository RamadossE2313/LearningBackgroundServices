namespace LearningBackgroundServices.UserProfiles.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse> GetUserProfile(int pageNumber);
    }
}
