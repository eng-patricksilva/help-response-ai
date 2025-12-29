namespace HelpResponse.AI.Domain.Settings
{
    public class ApiConfiguration
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DefaultRequestTimeout { get; set; }
        public string Version { get; set; }
        public string PathBase { get; set; } = "/";

        public int GetMajorVersion()
        {
            if (string.IsNullOrWhiteSpace(Version))
                return 1;

            var versionParts = Version.Split('.');
            if (versionParts.Length >= 1 && int.TryParse(versionParts[0], out int major))
                return major;

            return 1;
        }

        public int GetMinorVersion()
        {
            if (string.IsNullOrWhiteSpace(Version))
                return 0;

            var versionParts = Version.Split('.');
            if (versionParts.Length >= 2 && int.TryParse(versionParts[1], out int minor))
                return minor;

            return 0;
        }

        public string GetPathBase()
        {
            if (!string.IsNullOrEmpty(PathBase))
            {
                if (!PathBase.StartsWith('/')) PathBase = "/" + PathBase;
                if (PathBase.EndsWith('/')) PathBase = PathBase.TrimEnd('/');
            }

            return PathBase;
        }
    }
}