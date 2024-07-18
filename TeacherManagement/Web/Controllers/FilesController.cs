using Microsoft.AspNetCore.Mvc;
using Web.Entities.Files;
using Web.Models.Files;

namespace Web.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Management()
        {
            var viewModel = new FileManagementModel()
            {
                Files = new List<MyFile>()
                {
                    new MyFile(){ Id = 1, Name = "file1.xlsx" },
                    new MyFile(){ Id = 2, Name = "file2.xlsx" },
                }
            };

            return View(viewModel);
        }
    }
}
