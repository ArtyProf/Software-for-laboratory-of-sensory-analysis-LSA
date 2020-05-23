using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Accord.IO;
using Accord.Statistics.Analysis;
using LSA.Data;
using LSA.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LSA.Controllers
{
    public class DataAnalysisController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public DataAnalysisController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View(_context.FilesInformation.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // path to folder files
                string path = "/files/" + uploadedFile.FileName;
                // save file in folder Files in folder wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileInformation file = new FileInformation { Name = uploadedFile.FileName, Path = path };
                _context.FilesInformation.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: DataAnalysis/PCA_Analysis/5
        public async Task<IActionResult> PCA_Analysis(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataAnalysis = await _context.FilesInformation.FindAsync(id);
            if (dataAnalysis == null)
            {
                return NotFound();
            }

            string path = await _context.FilesInformation.Where(m => m.Id == id).Select(d => d.Path).FirstOrDefaultAsync();

            CsvReader reader = new CsvReader(_appEnvironment.WebRootPath + path, hasHeaders: true);
            double[][] actual = reader.ToJagged();

            var pcaTool = new KernelPrincipalComponentAnalysis();
            pcaTool.Learn(actual);
            pcaTool.NumberOfOutputs = 3;
            var outputMatrix = pcaTool.Transform(actual);

            ViewBag.data = outputMatrix;
            ViewBag.columns = pcaTool.NumberOfOutputs;
            ViewBag.rows = outputMatrix.Length;

            List<double> PC1 = new List<double>();
            List<double> PC2 = new List<double>();
            List<double> PC3 = new List<double>();

            for (int i = 0; i < outputMatrix.Length; i++)
            {
                for (int j = 0; j < pcaTool.NumberOfOutputs; j+=3)
                {
                    PC1.Add(ViewBag.data[i][j]);
                    PC2.Add(ViewBag.data[i][j + 1]);
                    PC3.Add(ViewBag.data[i][j + 2]);
                }  
            }

            ViewBag.PC1 = PC1;
            ViewBag.PC2 = PC2;
            ViewBag.PC3 = PC3;

            return View();
        }

        // GET: DataAnalysis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataAnalysis = await _context.FilesInformation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataAnalysis == null)
            {
                return NotFound();
            }

            return View(dataAnalysis);
        }

        // POST: DataAnalysis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataAnalysis = await _context.FilesInformation.FindAsync(id);
            _context.FilesInformation.Remove(dataAnalysis);

            string path = await _context.FilesInformation.Where(m => m.Id == id).Select(d => d.Path).FirstOrDefaultAsync();

            if (System.IO.File.Exists(_appEnvironment.WebRootPath + path))
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + path);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}