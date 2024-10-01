using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.CommonFunction
{
    public class UploadHandler
    {
        private BankingDbContext _dbContext;
        public UploadHandler(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> Upload(int id, ICollection<IFormFile> fileList)
        {
            User user =await _dbContext.UserTable.Include(o=>o.ClientObject).ThenInclude(c=>c.DocumentList).FirstOrDefaultAsync(z=>z.Id==id);

            foreach(var file in fileList )
            {
                //extension
                List<string> validExtensions = new List<string>()
                    { ".jpeg",".jpg",".png",".gif"};
                string extension = Path.GetExtension(file.FileName);

                if (!validExtensions.Contains(extension))
                {
                    return "Extension is not valid";
                }
                //fileSize

                long size = file.Length;
                if (size > (5 * 1024 * 1024))
                {

                    return "Max size can't exeeds 5Mb";
                }

                //Name Changing
                string fileName = Guid.NewGuid().ToString() + extension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                FileStream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                file.CopyTo(fileStream);

                //for database storage............
                var fileDetails = new Documents()
                {
                    DocumentName = fileName,
                    DocumentType = extension,
                    DocumentUrl = path,
                    UploadedOn = DateTime.Now,
                };
                user.ClientObject.DocumentList.Add(fileDetails);
                _dbContext.SaveChanges();
                //................................
                fileStream.Dispose();
                fileStream.Close();
                return fileName;
            }
            return "File Uploaded Success";
            
            
        }
    }
}
