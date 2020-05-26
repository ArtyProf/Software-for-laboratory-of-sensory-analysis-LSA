using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Accord.IO;
using Accord.MachineLearning;
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

        // GET: DataAnalysis/PCA_Analysis/id
        public async Task<IActionResult> PcaAnalysis(int? id)
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

            ViewBag.columns = pcaTool.NumberOfOutputs;
            ViewBag.rows = outputMatrix.Length;

            List<double> PC1 = new List<double>();
            List<double> PC2 = new List<double>();
            List<double> PC3 = new List<double>();

            for (int i = 0; i < outputMatrix.Length; i++)
            {
                for (int j = 0; j < pcaTool.NumberOfOutputs; j+=3)
                {
                    outputMatrix[i][j] = Math.Round(outputMatrix[i][j], 3);
                    outputMatrix[i][j + 1] = Math.Round(outputMatrix[i][j + 1], 3);
                    outputMatrix[i][j + 2] = Math.Round(outputMatrix[i][j + 2], 3);
                    PC1.Add(outputMatrix[i][j]);
                    PC2.Add(outputMatrix[i][j + 1]);
                    PC3.Add(outputMatrix[i][j + 2]);
                }  
            }

            ViewBag.data = outputMatrix;

            ViewBag.PC1 = PC1;
            ViewBag.PC2 = PC2;
            ViewBag.PC3 = PC3;

            return View();
        }

        // GET: DataAnalysis/KmeansAnalysis/id
        public async Task<IActionResult> KmeansAnalysis(int? id, int? numOfClusters)
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
            DataTable dataTable = reader.ToTable();
            var headers = reader.GetFieldHeaders();

            double[][] arrayOfData = dataTable.AsEnumerable().Select(x => new[] { Convert.ToDouble(x[headers[0]]), Convert.ToDouble(x[headers[1]]), Convert.ToDouble(x[headers[2]]) }).ToArray();

            int numOfClustersOnPost = numOfClusters ?? default(int);

            if (numOfClusters == null)
            {
                numOfClustersOnPost = 3;
            }

            // Create a new K-Means algorithm
            KMeans kmeans = new KMeans(k: numOfClustersOnPost);

            // Compute and retrieve the data centroids
            var clusters = kmeans.Learn(arrayOfData);

            // Use the centroids to parition all the data
            int[] clusterId = clusters.Decide(arrayOfData);

            ViewBag.data = arrayOfData;
            ViewBag.columns = 3;
            ViewBag.rows = arrayOfData.Length;

            List<double> field1 = new List<double>();
            List<double> field2 = new List<double>();
            List<double> field3 = new List<double>();

            for (int i = 0; i < ViewBag.data.Length; i++)
            {
                for (int j = 0; j < ViewBag.columns; j += 3)
                {
                    field1.Add(ViewBag.data[i][j]);
                    field2.Add(ViewBag.data[i][j + 1]);
                    field3.Add(ViewBag.data[i][j + 2]);
                }
            }

            ViewBag.field1 = field1;
            ViewBag.field2 = field2;
            ViewBag.field3 = field3;
            ViewBag.nameField1 = headers[0];
            ViewBag.nameField2 = headers[1];
            ViewBag.nameField3 = headers[2];

            ViewBag.clusters = clusterId;

            return View();
        }

        // POST: DataAnalysis/KmeansAnalysis/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SelectNumOfClustersForKmeansAnalysis(int? id, int? numOfClusters)
        {
            return KmeansAnalysis(id, numOfClusters);
        }

        // GET: DataAnalysis/CombinedAnalysis/id
        public async Task<IActionResult> CombinedAnalysis(int? id, int? numOfClusters)
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

            int numOfClustersOnPost = numOfClusters ?? default(int);

            if (numOfClusters == null)
            {
                numOfClustersOnPost = 3;
            }

            KMeans kmeans = new KMeans(k: numOfClustersOnPost);
            var clusters = kmeans.Learn(outputMatrix);
            int[] clusterId = clusters.Decide(outputMatrix);

            ViewBag.columns = pcaTool.NumberOfOutputs;
            ViewBag.rows = outputMatrix.Length;

            List<double> PC1 = new List<double>();
            List<double> PC2 = new List<double>();
            List<double> PC3 = new List<double>();

            for (int i = 0; i < outputMatrix.Length; i++)
            {
                for (int j = 0; j < ViewBag.columns; j += 3)
                {
                    outputMatrix[i][j] = Math.Round(outputMatrix[i][j], 3);
                    outputMatrix[i][j + 1] = Math.Round(outputMatrix[i][j + 1], 3);
                    outputMatrix[i][j + 2] = Math.Round(outputMatrix[i][j + 2], 3);
                    PC1.Add(outputMatrix[i][j]);
                    PC2.Add(outputMatrix[i][j + 1]);
                    PC3.Add(outputMatrix[i][j + 2]);
                }
            }
            ViewBag.data = outputMatrix;

            ViewBag.pc1 = PC1;
            ViewBag.pc2 = PC2;
            ViewBag.pc3 = PC3;

            ViewBag.clusters = clusterId;

            return View();
        }

        // POST: DataAnalysis/CombinedAnalysis/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SelectNumOfClustersForCombinedAnalysis(int? id, int? numOfClusters)
        {
            return CombinedAnalysis(id, numOfClusters);
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