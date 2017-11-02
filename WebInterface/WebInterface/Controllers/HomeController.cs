﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Classes;
using WebInterface.Models;
using WebInterface.Services;

namespace WebInterface.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult DownloadDockerfiles(UserConfiguration config)
        {
            var hs = new HomeControllerService();
            hs.CreateGamsDockerfile(config.LicencePath);
            hs.CreateModelDockerfile(config);

            var dlFile = hs.CreateDockerZipFile();

            return hs.DownloadFile(dlFile, "dockerfile.zip");
        }

        [HttpPost]
        public IActionResult RunScript(UserConfiguration config)
        {
            // https://stackoverflow.com/questions/43387693/build-docker-in-asp-net-core-no-such-file-or-directory-error

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var hs = new HomeControllerService();
            hs.CreateGamsDockerfile(config.LicencePath);
            hs.CreateModelDockerfile(config);

            // docker compose yml

            // build docker image of program from dockerfile
            var programDockerfile = "./Output/gams-dockerfile";

            var fullpath = Path.GetFullPath(programDockerfile);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = $@"docker build -f {fullpath} .";
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start(); // no such file or directory

            Debug.WriteLine(process.StandardOutput.ReadToEnd());

            // build docker image of model


            return View("Index");
        }
    }
}
