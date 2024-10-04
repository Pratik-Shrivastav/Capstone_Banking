﻿    using Capstone_Banking.Data;
    using Capstone_Banking.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

    namespace Capstone_Banking.CommonFunction
    {
        public class UploadHandler
        {
            private BankingDbContext _dbContext;
            private Cloudinary _cloudinary;
        public UploadHandler(BankingDbContext dbContext, IOptions<CloudinarySettings> config)
            {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _dbContext = dbContext;
            _cloudinary = new Cloudinary(acc);
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
            foreach (var file in fileList) 
            {
                var uploadResult = new ImageUploadResult();

                if (file.Length > 0)

                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
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

        public async Task<FileResult> DownloadFile(string fileName)
        {
            // Specify the path where files are stored
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                return null; // or handle it as needed (e.g., return a NotFound result)
            }

            // Read the file content
            var fileContent = await System.IO.File.ReadAllBytesAsync(path);

            // Get the file extension to determine the content type
            string extension = Path.GetExtension(fileName);
            string contentType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream" // default
            };

            return new FileContentResult(fileContent, contentType)
            {
                FileDownloadName = fileName // Set the file name for the download
            };
        }

    }
}
