using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class BeerReviewDbContextDesignFactory : IDesignTimeDbContextFactory<BeerReviewDbContext>
    {
        private const int MaxHeight = 3;

        public BeerReviewDbContext CreateDbContext(string[] args)
        {
            // Get the environment
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var sourceFolder =
                FindTargetDirectoryPath(Directory.GetParent(Directory.GetCurrentDirectory()), "Presentation");

            var basePath = FindAppsettingsJsonPath(sourceFolder);

            Console.WriteLine($"Found settings files in directory {basePath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(nameof(ConnectionSettings.DefaultConnection));

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must be set!");
            }

            var optionsBuilder = new DbContextOptionsBuilder<BeerReviewDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BeerReviewDbContext(optionsBuilder.Options);
        }

        private static string FindTargetDirectoryPath(DirectoryInfo directory, string targetFolder)
        {
            var height = 0;

            while (true)
            {
                if (height > MaxHeight)
                    throw new ArgumentException(
                        $"Could not find the target directory path {targetFolder} within the {nameof(MaxHeight)} constraint of {MaxHeight}");

                if (directory.Name == targetFolder) return directory.FullName;

                var subDirectories = directory.GetDirectories();
                foreach (var subDirectory in subDirectories)
                {
                    if (subDirectory.Name == targetFolder) return subDirectory.FullName;
                }

                directory = directory.Parent ?? throw new ArgumentNullException(nameof(directory.Parent));
                height += 1;
            }
        }

        private static string FindAppsettingsJsonPath(string rootDirectory)
        {
            var queue = new Queue<string>(Directory.GetDirectories(rootDirectory));

            while (queue.TryDequeue(out var currentDirectory))
            {
                var files = Directory.GetFiles(currentDirectory);
                if (files.Any(x => x.EndsWith("appsettings.json")))
                {
                    return currentDirectory;
                }

                Directory.GetDirectories(currentDirectory)
                    .Where(x => !x.EndsWith("bin") && !x.EndsWith("obj"))
                    .ToList()
                    .ForEach(x => queue.Enqueue(x));
            }

            throw new ArgumentNullException(nameof(rootDirectory), "Could not find appsettings.json file");
        }
    }
}