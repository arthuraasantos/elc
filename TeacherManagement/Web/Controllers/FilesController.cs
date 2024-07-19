using Microsoft.AspNetCore.Mvc;
using Web.Models.Files;
using Core.Entities.Files;
using Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Infra.Contexts;

namespace Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly UserManager<AppUser> _identityUserManager;
        private readonly IMyFileRepository _fileRepository;
        private readonly TMContext _tmContext;

        public FilesController(UserManager<AppUser> identityUserManager, IMyFileRepository fileRepository, TMContext tmContext)
        {
            _identityUserManager = identityUserManager;
            _fileRepository = fileRepository;
            _tmContext = tmContext;
        }

        public async Task<IActionResult> Management()
        {
            var currentUser = await _identityUserManager.GetUserAsync(User);
            var userFiles = await _fileRepository.GetUserFiles(currentUser!.Id);

            var viewModel = new FileManagementModel()
            {
                Files = new List<MyFile>()
                {
                    new MyFile(){ Id = Guid.NewGuid(), OriginalName = "file1.xlsx" },
                    new MyFile(){ Id = Guid.NewGuid(), OriginalName = "file2.xlsx" },
                }
            };

            viewModel.Files.AddRange(userFiles);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileManagementModel fileManagementModel)
        {
            if (fileManagementModel.NewFiles != null && fileManagementModel.NewFiles.Length > 0)
            {
                var user = await _identityUserManager.GetUserAsync(User);
                
                if(user == null)
                {
                    ViewBag.Message = "File upload failed!";
                    
                    return RedirectToAction("Management");
                }

                var userUploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", user.Id.ToString());

                if (!Directory.Exists(userUploadFolderPath))
                    Directory.CreateDirectory(userUploadFolderPath);

                var file = new MyFile()
                {
                    Id = Guid.NewGuid(),
                    OriginalName = fileManagementModel.NewFiles.FileName,
                    Size = fileManagementModel.NewFiles.Length / 1024,
                    OwnerId = user.Id,
                };

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", user.Id.ToString(), file.Id.ToString());

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileManagementModel.NewFiles.CopyToAsync(stream);
                }

                await _fileRepository.CreateAsync(file);
                _tmContext.SaveChanges();

                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "File upload failed!";
            }

            return RedirectToAction("Management");
        }
    }
}
