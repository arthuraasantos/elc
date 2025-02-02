﻿using Microsoft.AspNetCore.Mvc;
using Web.Models.Files;
using Core.Entities.Files;
using Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Infra.Contexts;
using Web.Configurations;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Core.Services;
using Core.Entities.Files.FileDataExtractTypes;
using Infra;
using ClosedXML.Excel;
using IronXL;
using GemBox.Spreadsheet;
using Web.Adapters;

namespace Web.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly UserManager<AppUser> _identityUserManager;
        private readonly IMyFileRepository _fileRepository;
        private readonly IFileService _fileService;
        private readonly IUnitofWork _unitOfWork;

        public FilesController(UserManager<AppUser> identityUserManager, IMyFileRepository fileRepository, IFileService fileService, IUnitofWork unitOfWork)
        {
            _identityUserManager = identityUserManager;
            _fileRepository = fileRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Management()
        {
            var currentUser = await _identityUserManager.GetUserAsync(User);
            var userFiles = await _fileRepository.GetUserFiles(currentUser!.Id);
            
            var viewModel = FileAdapter.ToManagementModel(userFiles);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileManagementModel fileManagementModel)
        {
            const long UPLOAD_LIMIT_MB = 5;
            if (fileManagementModel.NewFiles != null && fileManagementModel.NewFiles.Length > 0)
            {
                var uploadedFileKBLength = fileManagementModel.NewFiles.Length / 1024;
                var uploadedFileMBLength = uploadedFileKBLength / 1024;
                
                if (UPLOAD_LIMIT_MB < uploadedFileMBLength)
                {
                    TempData["Message"] = "O limte do arquivo é 5MB.";

                    return RedirectToAction("Management");
                }

                var user = await _identityUserManager.GetUserAsync(User);

                if (user == null)
                {
                    TempData["Message"] = "Erro ao fazer upload";

                    return RedirectToAction("Management");
                }
                string userUploadFolderPath = GetUserFilesPath(user.Id);

                if (!Directory.Exists(userUploadFolderPath))
                    Directory.CreateDirectory(userUploadFolderPath);

                var fileExtension = _fileService.GetFileExtension(fileManagementModel.NewFiles.FileName);

                var file = new MyFile()
                {
                    Id = Guid.NewGuid(),
                    OriginalName = fileManagementModel.NewFiles.FileName,
                    Size = uploadedFileKBLength,
                    Extension = fileExtension,
                    OwnerId = user.Id,
                };

                var fileName = $"{file.Id}.{file.Extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), AppConstants.UPLOAD_FILES_FODLER, user.Id.ToString(), fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileManagementModel.NewFiles.CopyToAsync(stream);
                }

                await _fileRepository.CreateAsync(file);
                
                await _unitOfWork.CommitAsync();

                var result = await _fileService.ReadFile(ParseFileTypes.GradeStudents, filePath, true);
                file.Data = new MyFileData()
                {
                    FileId = file.Id,
                    ExtractedData = result.ExtractedData,
                    Thumbnail = result.Thumbnail
                };

                await _fileRepository.UpdateAsync(file);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                TempData["Message"] = "Erro inesperado ao fazer o upload.";
            }

            return RedirectToAction("Management");
        }

        private static string GetUserFilesPath(Guid userId)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), AppConstants.UPLOAD_FILES_FODLER, userId.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> EditFile(EditFileModel editFileModel)
        {
            var file = await _fileRepository.GetByIdAsync(editFileModel.FileId);

            if (file == null) return View("Management");

            file.Description = editFileModel.FileDescription;

            await _unitOfWork.CommitAsync();

            return RedirectToAction("Management");
        }

        public async Task<IActionResult> DownloadFile(string fileId)
        {
            if(!Guid.TryParse(fileId, out Guid fileIdConverted))
                return NotFound();

            var user = await _identityUserManager.GetUserAsync(User);
            var file = await _fileRepository.GetByIdAsync(fileIdConverted);

            if (file == null || user is null) return NotFound();

            string userUploadFolderPath = GetUserFilesPath(user.Id);
            string fileName = $"{file.Id}.{file.Extension}";
            string fileNameToDownload = $"{file.OriginalName}";
            var fileToDownload = Path.Combine(userUploadFolderPath, fileName);

            if (!System.IO.File.Exists(fileToDownload)) return NotFound();

            var contentType = _fileService.GetFileContentType(fileName);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(fileToDownload);

            file.Downloads++;
            await _unitOfWork.CommitAsync();

            return File(fileBytes, contentType, fileNameToDownload);
        }

        public async Task<IActionResult> DownloadPDFFile(string fileId)
        {
            if (!Guid.TryParse(fileId, out Guid fileIdConverted))
                return NotFound();

            var user = await _identityUserManager.GetUserAsync(User);
            var file = await _fileRepository.GetByIdAsync(fileIdConverted);

            if (file == null || user is null) return NotFound();

            string userUploadFolderPath = GetUserFilesPath(user.Id);
            string fileName = $"{file.Id}.{file.Extension}";
            var excelToDownload = Path.Combine(userUploadFolderPath, fileName);

            if (!System.IO.File.Exists(excelToDownload)) return NotFound();

            var contentType = _fileService.GetFileContentType(fileName);
            string pdfFileName = _fileService.GetPdfFromXLS(file, userUploadFolderPath, excelToDownload);

            file.Downloads++;
            await _unitOfWork.CommitAsync();

            string extension = _fileService.GetFileExtension(file.OriginalName);
            string fileNameToDownload = $"{file.OriginalName.Replace(extension, string.Empty)}.pdf";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfFileName);

            return File(fileBytes, contentType, fileNameToDownload);
        }

        

        public async Task<IActionResult> RemoveFile(string fileId)
        {
            if (!Guid.TryParse(fileId, out Guid fileIdConverted))
                return NotFound();

            var file = await _fileRepository.GetByIdAsync(fileIdConverted);
            if (file == null) return NotFound();

            await _fileRepository.RemoveAsync(file);
            await _unitOfWork.CommitAsync();

            return RedirectToAction("Management");
        }

        public async Task<IActionResult> GetClassFileData(string fileId)
        {
            if (!Guid.TryParse(fileId, out Guid fileIdConverted))
                return NotFound();

            var file = await _fileRepository.GetByIdAsync(fileIdConverted);

            if (file == null) return Json(null);

            var classData = new ClassEvaluationFileData();
            
            if (file.Data != null)
                classData = file.Data.GetFormattedData<ClassEvaluationFileData>();

            var result = new
            {
                fileId = file.Id,
                teacher = classData.Teacher,
                subject = classData.Subject,
                approvalPercentage = classData.ApprovalPercentage > 0 ? float.Round(classData.ApprovalPercentage.Value, 2) : 0,
                average = classData.Average > 0 ? float.Round(classData.Average.Value, 2) : 0,
                students = classData.Students,
                bestStudentId = classData.BestStudentId
            };
            return Json(result);
        }
    }
}
