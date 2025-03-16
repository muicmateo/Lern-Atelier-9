using System.Threading.Tasks;

namespace VideoGameTracker.Services
{
    public interface IGithubService
    {
        Task<string> GetLatestVersionAsync();
        Task<bool> CheckForUpdatesAsync();
        Task<string> GetReleaseNotesAsync(string version);
        Task<bool> ReportIssueAsync(string title, string description);
    }
}