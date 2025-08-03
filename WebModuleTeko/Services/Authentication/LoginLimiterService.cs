using System.Collections.Concurrent;

namespace WebModuleTeko.Services.Authentication;

public class LoginLimiterService
{
    private readonly int MaxAttempts = 5;

    private readonly ConcurrentDictionary<string, LoginAttemptEntry> _loginAttempts = new();

    public void CleanupCachedLoginAttempts()
    {
        var now = DateTimeOffset.UtcNow;

        foreach(var (key, entry) in _loginAttempts.ToDictionary())
        {
            if((now - entry.LastAttempt) > TimeSpan.FromMinutes(5))
            {
                _loginAttempts.TryRemove(key, out _);
            }
        }
    }

    public void RecordLoginFailure(string username)
    {
        CleanupCachedLoginAttempts();

        if (!_loginAttempts.TryGetValue(username, out LoginAttemptEntry? entry))
        {
            entry = new LoginAttemptEntry
            {
                Tries = 0
            };
            _loginAttempts.TryAdd(username, entry);
        }

        entry.Tries++;
        entry.LastAttempt = DateTimeOffset.UtcNow;
    }

    public bool ShouldLoginBeRestricted(string username)
    {
        CleanupCachedLoginAttempts();

        if (!_loginAttempts.TryGetValue(username, out LoginAttemptEntry? entry))
        {
            return false;
        }

        return entry.Tries > MaxAttempts;
    }

    private class LoginAttemptEntry
    {
        public int Tries { get; set; }
        public DateTimeOffset LastAttempt { get; set; }
    }

}
