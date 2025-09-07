using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.IO;
using System.Linq;

namespace API.EndToEnd.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("Web/API");
    }
}
public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder UseSolutionRelativeContentRoot(
        this IWebHostBuilder webHostBuilder,
        string solutionRelativePath,
        string solutionName = "*.sln")
    {
        var solutionDir = GetSolutionDirectory(solutionName);
        var contentRoot = Path.Combine(solutionDir, solutionRelativePath);
        return webHostBuilder.UseContentRoot(contentRoot);
    }

    private static string GetSolutionDirectory(string solutionName)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles(solutionName).Any())
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new System.Exception("Solution directory not found.");
    }
}