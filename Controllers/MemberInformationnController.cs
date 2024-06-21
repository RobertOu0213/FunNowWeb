using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;

namespace PrjFunNowWeb.Controllers
{
    [Route("[controller]")]
    public class MemberInformationnController : Controller
    {

        private readonly IWebHostEnvironment _env;
        private readonly FunNowContext _context;


        public MemberInformationnController(FunNowContext context, IWebHostEnvironment env)
        {

            _context = context;
            _env = env;

        }

        public IActionResult MemberInformationn()
        {
            return View();
        }

        //修改房客照片
        [HttpPut("PhotoEdit/{id}")]
        public async Task<IActionResult> PhotoEdit(int id, IFormFile imageFile)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return BadRequest("找不到該會員");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // 刪除舊圖片
                if (!string.IsNullOrEmpty(member.Image))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, member.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // 獲取檔案副檔名
                var extension = Path.GetExtension(imageFile.FileName);
                // 生成唯一的檔案名稱
                var fileName = $"{Guid.NewGuid()}{extension}";
                // 組合完整的檔案路徑
                var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
                // 確保 uploads 目錄存在
                Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "uploads"));
                // 將檔案保存到指定路徑
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                // 將新圖片路徑存儲在 Member 模型中
                member.Image = $"/uploads/{fileName}";

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(id))
                    {
                        return BadRequest("更新資料時找不到該會員");
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new { Message = "照片修改成功", ImageUrl = member.Image });
            }

            return BadRequest("未收到有效的圖片檔案");
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}

