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
            // Fetch the user from the database
            User user = await _dbContext.UserTable
                .Include(o => o.ClientObject)
                .ThenInclude(c => c.DocumentList)
                .FirstOrDefaultAsync(z => z.Id == id);

            // Check if user exists
            if (user == null)
            {
                return "User not found";
            }

            // Check if ClientObject is null, and initialize it if necessary
            if (user.ClientObject == null)
            {
                Console.WriteLine("Client Empty");
                user.ClientObject = new Client();  // Assuming ClientObject has a default constructor
            }

            // Check if DocumentList is null, and initialize it if necessary
            if (user.ClientObject.DocumentList == null)
            {
                Console.WriteLine("Document Empty");

                user.ClientObject.DocumentList = new List<Documents>();  // Initialize DocumentList if it's null
            }

            // Iterate over the files and validate/upload
            foreach (var file in fileList)
            {
                // Validate file extension
                List<string> validExtensions = new List<string>() { ".jpeg", ".jpg", ".png", ".gif" };
                string extension = Path.GetExtension(file.FileName);

                if (!validExtensions.Contains(extension))
                {
                    return "Extension is not valid";
                }

                // Validate file size
                long size = file.Length;
                if (size > (5 * 1024 * 1024))
                {
                    return "Max size can't exceed 5Mb";
                }

                // Generate a unique file name and save the file
                string fileName = Guid.NewGuid().ToString() + extension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                // Ensure the directory exists
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Add file details to DocumentList
                var fileDetails = new Documents()
                {
                    DocumentName = fileName,
                    DocumentType = extension,
                    DocumentUrl = Path.Combine(path, fileName),
                    UploadedOn = DateTime.Now,
                };

                user.ClientObject.DocumentList.Add(fileDetails);
                await _dbContext.SaveChangesAsync();
            }

            return "File(s) Uploaded Successfully";
        }

    }
}
