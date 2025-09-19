using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions;

        public FileUploadController(ILogger<FileUploadController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _uploadPath = _configuration.GetValue<string>("FileUpload:UploadPath") ?? "uploads";
            _maxFileSize = _configuration.GetValue<long>("FileUpload:MaxFileSizeMB") * 1024 * 1024; // Convert MB to bytes
            _allowedExtensions = _configuration.GetSection("FileUpload:AllowedExtensions").Get<string[]>()
                ?? new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx" };

            // Ensure upload directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        /// <summary>
        /// Upload single file
        /// </summary>
        [HttpPost("single")]
        [RequestSizeLimit(100 * 1024 * 1024)] // 100MB limit
        public async Task<IActionResult> UploadSingle([FromForm] FileUploadRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest(new { error = "Aucun fichier sélectionné" });
                }

                var validationResult = ValidateFile(request.File);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { error = validationResult.ErrorMessage });
                }

                var uploadResult = await SaveFileAsync(request.File, request.SubDirectory);

                _logger.LogInformation("Fichier uploadé avec succès: {FileName}", uploadResult.FileName);

                return Ok(new
                {
                    success = true,
                    file = uploadResult
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'upload du fichier");
                return StatusCode(500, new { error = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Upload multiple files
        /// </summary>
        [HttpPost("multiple")]
        [RequestSizeLimit(500 * 1024 * 1024)] // 500MB total limit
        public async Task<IActionResult> UploadMultiple([FromForm] MultipleFileUploadRequest request)
        {
            try
            {
                if (request.Files == null || !request.Files.Any())
                {
                    return BadRequest(new { error = "Aucun fichier sélectionné" });
                }

                if (request.Files.Count() > 10)
                {
                    return BadRequest(new { error = "Maximum 10 fichiers autorisés" });
                }

                var uploadResults = new List<FileUploadResult>();
                var errors = new List<string>();

                foreach (var file in request.Files)
                {
                    var validationResult = ValidateFile(file);
                    if (!validationResult.IsValid)
                    {
                        errors.Add($"{file.FileName}: {validationResult.ErrorMessage}");
                        continue;
                    }

                    try
                    {
                        var uploadResult = await SaveFileAsync(file, request.SubDirectory);
                        uploadResults.Add(uploadResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erreur lors de l'upload du fichier {FileName}", file.FileName);
                        errors.Add($"{file.FileName}: Erreur lors de la sauvegarde");
                    }
                }

                return Ok(new
                {
                    success = uploadResults.Any(),
                    uploadedFiles = uploadResults,
                    errors = errors.Any() ? errors : null,
                    summary = new
                    {
                        totalFiles = request.Files.Count(),
                        successfulUploads = uploadResults.Count,
                        failedUploads = errors.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'upload multiple");
                return StatusCode(500, new { error = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Get file info
        /// </summary>
        [HttpGet("info/{fileName}")]
        public IActionResult GetFileInfo(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_uploadPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { error = "Fichier non trouvé" });
                }

                var fileInfo = new FileInfo(filePath);

                return Ok(new
                {
                    fileName = fileInfo.Name,
                    size = fileInfo.Length,
                    sizeFormatted = FormatFileSize(fileInfo.Length),
                    createdDate = fileInfo.CreationTime,
                    lastModified = fileInfo.LastWriteTime,
                    extension = fileInfo.Extension
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des infos du fichier");
                return StatusCode(500, new { error = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Download file
        /// </summary>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_uploadPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { error = "Fichier non trouvé" });
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                var contentType = GetContentType(filePath);

                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du téléchargement du fichier");
                return StatusCode(500, new { error = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        [HttpDelete("{fileName}")]
        [Authorize] // Require authentication for deletion
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_uploadPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { error = "Fichier non trouvé" });
                }

                System.IO.File.Delete(filePath);
                _logger.LogInformation("Fichier supprimé: {FileName}", fileName);

                return Ok(new { success = true, message = "Fichier supprimé avec succès" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du fichier");
                return StatusCode(500, new { error = "Erreur interne du serveur" });
            }
        }

        #region Private Methods

        private FileValidationResult ValidateFile(IFormFile file)
        {
            // Check file size
            if (file.Length > _maxFileSize)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"La taille du fichier dépasse la limite autorisée ({FormatFileSize(_maxFileSize)})"
                };
            }

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Extension de fichier non autorisée. Extensions autorisées: {string.Join(", ", _allowedExtensions)}"
                };
            }

            // Check for potentially dangerous files
            var fileName = Path.GetFileName(file.FileName);
            if (string.IsNullOrEmpty(fileName) || fileName.Contains("..") || Path.IsPathRooted(fileName))
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Nom de fichier invalide"
                };
            }

            return new FileValidationResult { IsValid = true };
        }

        private async Task<FileUploadResult> SaveFileAsync(IFormFile file, string subDirectory = null)
        {
            var originalFileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(originalFileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var targetDirectory = _uploadPath;
            if (!string.IsNullOrEmpty(subDirectory))
            {
                targetDirectory = Path.Combine(_uploadPath, subDirectory);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
            }

            var filePath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new FileUploadResult
            {
                OriginalFileName = originalFileName,
                FileName = uniqueFileName,
                FilePath = filePath,
                Size = file.Length,
                SizeFormatted = FormatFileSize(file.Length),
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".txt", "text/plain" }
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.TryGetValue(ext, out string contentType) ? contentType : "application/octet-stream";
        }

        private static string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }

        #endregion
    }

    #region DTOs and Models

    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }

        public string SubDirectory { get; set; }
    }

    public class MultipleFileUploadRequest
    {
        [Required]
        public IEnumerable<IFormFile> Files { get; set; }

        public string SubDirectory { get; set; }
    }

    public class FileUploadResult
    {
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public string SizeFormatted { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class FileValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion
}