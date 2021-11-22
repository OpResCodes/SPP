using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShortestPaths.Yen;
using YenWeb.Logic;
using YenWeb.Models;

namespace YenWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("LineGen");
        }

        [HttpGet]
        public IActionResult LineGen()
        {
            YenInputViewmodel vm = new YenInputViewmodel();
            return View(vm);
        }
         
        [HttpPost]
        public IActionResult LineGen(YenInputViewmodel model)
        {
            if(ModelState.IsValid)
            {
                StreamWriter w = null;
                MemoryStream stream = null;
                try
                {
                    GraphConvert r = new GraphConvert();
                    LineGeneration lg = new LineGeneration();
                    var graph = r.ReadGraphFromString(model.CsvString, model.MirrorArcs);
                    var paths = lg.GenerateLines(model.MinimumLength, model.MaximumLength, graph);
                    string result = r.WriteText(paths);

                    YenInputViewmodel vmResult = new YenInputViewmodel()
                    {
                        MinimumLength = model.MinimumLength,
                        MaximumLength = model.MaximumLength,
                        CsvString = result
                    };

                    stream = new MemoryStream();                    
                    w = new StreamWriter(stream);

                    w.Write(vmResult.CsvString);
                    w.Flush();
                    stream.Position = 0;
                    return File(stream, "text/comma-separated-values", "Lineplanning.csv");
                }
                catch (Exception ex)
                {
                    ViewBag.ExceptionMessage = ex.Message;
                    return View("Failed");
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                    if (w != null)
                        w.Dispose();
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Test(YenInputViewmodel model)
        {
            var stream = new MemoryStream();
            StreamWriter w = new StreamWriter(stream);
            w.Write(model.CsvString);
            w.Flush();
            return File(stream, "application/octet-stream", "Lineplanning.csv");
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
