using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PathGenerator.Web.Model;
using ShortestPaths.Algorithms.Yen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PathGenerator.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly LineGeneration _lineGeneration;

        public IndexModel(ILogger<IndexModel> logger, LineGeneration lineGeneration)
        {
            _logger = logger;
            _lineGeneration = lineGeneration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            if(!ModelState.IsValid)
            {
                return Page();
            }

            StreamWriter? w = null;
            MemoryStream? stream = null;
            try
            {
                LogCsv(Viewmodel.CsvString);

                GraphConvert r = new GraphConvert();
                
                var graph = r.ReadGraphFromString(Viewmodel.CsvString, Viewmodel.MirrorArcs);
                var paths = await Task.Run(() =>  _lineGeneration.GenerateLines(
                    Viewmodel.MinimumLength, Viewmodel.MaximumLength, graph) );
                string result = r.WriteText(paths);

                stream = new MemoryStream();
                w = new StreamWriter(stream);

                w.Write(result);
                w.Flush();
                stream.Position = 0;
                _logger.LogInformation("Paths generated successfully.");
                return File(stream, "text/comma-separated-values", "LineGeneration.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewData["Error"] = ex.Message;
                return Page();
            }
        }

        private void LogCsv(string csvString)
        {
            int n = csvString.Length;
            _logger.LogInformation("Generating Paths. CSV-Length: {0}", n);
            n = Math.Min(120, n);
            _logger.LogInformation("Csv string head:\n\n '{0}' \n [...]", csvString.Substring(0,n));
        }

        [BindProperty]
        public YenInputViewmodel Viewmodel { get; set; }

    }
}
