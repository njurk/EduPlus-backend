using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class LogsController : ControllerBase
    {
        private readonly string _logsDirectory;

        public LogsController()
        {
            _logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        }

        [HttpGet("files")]
        public IActionResult GetLogFiles()
        {
            if (!Directory.Exists(_logsDirectory))
            {
                return Ok(new List<object>());
            }

            var files = Directory.GetFiles(_logsDirectory, "*.log")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new
                {
                    name = f.Name,
                    size = f.Length,
                    lastModified = f.LastWriteTime
                })
                .ToList();

            return Ok(files);
        }

        [HttpGet("content/{fileName}")]
        public IActionResult GetLogContent(string fileName, [FromQuery] int lines = 100)
        {
            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
            {
                return BadRequest("Nieprawidłowa nazwa pliku");
            }

            var filePath = Path.Combine(_logsDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Plik nie istnieje");
            }

            var allLines = System.IO.File.ReadAllLines(filePath);
            var lastLines = allLines.TakeLast(lines).ToArray();

            return Ok(new
            {
                fileName,
                totalLines = allLines.Length,
                lines = lastLines
            });
        }
    }
}
