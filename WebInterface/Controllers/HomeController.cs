﻿using System.Runtime.InteropServices.WindowsRuntime;

namespace WebInterface.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WebInterface.Models;
    using WebInterface.Services;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HomeController : Controller
    {
        public List<GithubRepository> GithubRepositories { get; set; }

        public async Task<IActionResult> Index()
        {
            var userConfig = await this.GetUserConfig(new UserConfiguration());

            return this.View(userConfig);
        }

        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult DownloadDockerfiles(UserConfiguration config)
        {
            var hs = new HomeControllerService();
            hs.CreateGamsDockerfile(config.SelectedProgramVersion, config.ProgramArchitecture, config.LicencePath);
            hs.CreateModelDockerfile(config);

            var dlFile = hs.CreateDockerZipFile();

            return hs.DownloadFile(dlFile, "dockerfile.zip");
        }

        [HttpPost]
        public IActionResult DownDownloadGeonodeFile(string id)
        {
            var hs = new HomeControllerService();
            return hs.DownloadFile("http://localhost:8011/documents/34/download", "dockerfile.zip");
        }

        [HttpPost]
        public IActionResult RunScript(UserConfiguration config)
        {
            // https://stackoverflow.com/questions/43387693/build-docker-in-asp-net-core-no-such-file-or-directory-error
            // https://stackoverflow.com/questions/2849341/there-is-no-viewdata-item-of-type-ienumerableselectlistitem-that-has-the-key

            if (!this.ModelState.IsValid)
            {
                return this.View("Index", config);
            }

            var hs = new HomeControllerService();
            hs.CreateGamsDockerfile(config.SelectedProgramVersion, config.ProgramArchitecture, config.LicencePath);
            hs.CreateModelDockerfile(config);

            // docker compose yml

            // build docker image of program from dockerfile
            const string programDockerfile = "./Output/gams-dockerfile";

            var fullpath = Path.GetFullPath(programDockerfile);

            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "/bin/bash",
                Arguments = $@"docker build -f {fullpath} .",
                RedirectStandardOutput = true
            };

            process.StartInfo = startInfo;
            process.Start(); // no such file or directory

            Debug.WriteLine(process.StandardOutput.ReadToEnd());

            // build docker image of model

            return this.View("Index", config);
        }

        public async Task<UserConfiguration> GetUserConfig(UserConfiguration userConfig,
            string programRepo = "gams-docker")
        {
            if (userConfig == null)
            {
                userConfig = new UserConfiguration();
            }

            var homeControllerService = new HomeControllerService();

            if (userConfig.GeoNodeDocuments.Count == 0)
            {
                var geoNodeDocuments = await homeControllerService.GetGeonodeData();
                if (geoNodeDocuments != null)
                {
                    userConfig.GeoNodeDocuments = geoNodeDocuments.Documents.Select(document => new SelectListItem
                    {
                        Value = document.Id.ToString(),
                        Text = document.Title
                    })
                        .ToList();
                }
            }

            // TODO: Get GeoNode Document Tags
            //var geoNodeDocumentTags = 
            //var geonodeDocumentTagList = geoNodeDocumentTags.Select(document => new SelectListItem
            //    {
            //        Value = document.Title,
            //        Text = document.Title
            //    })
            //    .ToList();

            // https://stackoverflow.com/questions/28781345/listing-all-repositories-using-github-c-sharp
            if (userConfig.GithubRepositories.Count == 0)
            {
                var repositories = await homeControllerService.GetGithubRepositories(userConfig.GitHubUser);
                if (repositories != null)
                {
                    userConfig.GithubRepositories = repositories.Select(repository => new SelectListItem
                    {
                        Value = repository.Name,
                        Text = repository.Name
                    })
                        .ToList();

                    if (string.IsNullOrEmpty(userConfig.SelectedGithubRepository))
                    {
                        userConfig.SelectedGithubRepository = repositories.First().Name;
                    }

                    var repositoryVersions =
                        await homeControllerService.GetGithubRepoVersions(userConfig.GitHubUser,
                            userConfig.SelectedGithubRepository);
                    if (repositoryVersions != null)
                    {
                        userConfig.GithubRepositoryVersions = repositoryVersions.Select(version => new SelectListItem
                        {
                            Value = version.Url.Substring(version.Url.LastIndexOf('/') + 1),
                            Text = version.Url.Substring(version.Url.LastIndexOf('/') + 1)
                        })
                            .ToList();
                    }
                }
            }

            if (userConfig.ProgramVersions.Count != 0)
            {
                return userConfig;
            }

            var programVersions = await homeControllerService.GetGithubRepoContents(userConfig.GitHubUser, programRepo);

            if (programVersions == null)
            {
                return userConfig;
            }

            programVersions = programVersions.Where(x => x.Type == "dir").ToList();
            userConfig.ProgramVersions = programVersions.Select(version => new SelectListItem
            {
                Value = version.Name,
                Text = version.Name
            })
                .ToList();

            return userConfig;
        }
    }
}
