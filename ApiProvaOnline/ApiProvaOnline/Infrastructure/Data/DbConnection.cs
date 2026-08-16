// This file is kept for reference only.
// The application now uses Entity Framework Core via ProvaOnDbContext.
// See Infrastructure/Data/ProvaOnDbContext.cs and Program.cs for the new configuration.

namespace ApiProvaOnline
{
    [Obsolete("Use ProvaOnDbContext via dependency injection instead.")]
    public static class DbConnection
    {
        public static string ConnectionString => throw new InvalidOperationException(
            "DbConnection is obsolete. Use ProvaOnDbContext via dependency injection.");
    }
}
