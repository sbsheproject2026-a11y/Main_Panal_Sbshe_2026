//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Formats.Jpeg;

//// iText7 ke liye using
//using iText.Kernel.Pdf;

//// Conflict hatane ke liye
//using Image = SixLabors.ImageSharp.Image;

//namespace Application
//{
//    public static class FileHelper
//    {
//        public static bool IsFileSizeValid(IFormFile file, long maxSizeInMB = 2)
//        {
//            if (file == null || file.Length == 0) return false;
//            long maxBytes = maxSizeInMB * 1024 * 1024;
//            return file.Length <= maxBytes;
//        }

//        // ⭐ MASTER METHOD (Jo khud check karega)
//        public static async Task<string> ProcessAndSaveFileAsync(IFormFile file, string folderPath)
//        {
//            if (file == null || file.Length == 0) return null;
//            string extension = Path.GetExtension(file.FileName).ToLower();

//            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
//            {
//                return await SaveCompressedImageAsync(file, folderPath, 60);
//            }
//            else if (extension == ".pdf")
//            {
//                return await CompressPdfAsync(file, folderPath); // iText7 wala method
//            }
//            else
//            {
//                return await SaveFileAsync(file, folderPath);
//            }
//        }

//        public static async Task<string> SaveFileAsync(IFormFile file, string folderPath)
//        {
//            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
//            string uniqueFileName = $"{Guid.NewGuid():N}".Substring(0, 8) + "_" + Path.GetFileName(file.FileName);
//            string filePath = Path.Combine(folderPath, uniqueFileName);
//            using var stream = new FileStream(filePath, FileMode.Create);
//            await file.CopyToAsync(stream);
//            return uniqueFileName;
//        }

//        public static async Task<string> SaveCompressedImageAsync(IFormFile file, string folderPath, int quality = 60)
//        {
//            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
//            string uniqueFileName = $"{Guid.NewGuid():N}".Substring(0, 8) + "_" + Path.GetFileNameWithoutExtension(file.FileName) + ".jpg";
//            string filePath = Path.Combine(folderPath, uniqueFileName);

//            using var image = await Image.LoadAsync(file.OpenReadStream());
//            if (image.Width > 1920 || image.Height > 1080)
//            {
//                image.Mutate(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(1920, 1080) }));
//            }
//            await image.SaveAsJpegAsync(filePath, new JpegEncoder { Quality = quality });
//            return uniqueFileName;
//        }

//        // ✅ NAYA: iText7 se PDF Compress (Bina Ghostscript ke)
//        public static async Task<string> CompressPdfAsync(IFormFile file, string folderPath)
//        {
//            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

//            string uniqueFileName = $"{Guid.NewGuid():N}".Substring(0, 8) + "_" + Path.GetFileName(file.FileName);
//            string inputFilePath = Path.Combine(folderPath, uniqueFileName);

//            // Pehle original file save karein
//            using (var stream = new FileStream(inputFilePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            string outputFileName = "compressed_" + uniqueFileName;
//            string outputFilePath = Path.Combine(folderPath, outputFileName);

//            try
//            {
//                // iText7 ka use karke compress karein
//                using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(inputFilePath), new PdfWriter(outputFilePath, new WriterProperties().SetFullCompressionMode(true))))
//                {
//                    // Khali chhod dein, ye automatically compress karke save kar dega
//                }

//                // Agar compress hone ke baad file ban gayi, toh original delete karein
//                if (File.Exists(outputFilePath))
//                {
//                    long originalSize = new FileInfo(inputFilePath).Length;
//                    long compressedSize = new FileInfo(outputFilePath).Length;

//                    // Agar compress hone ke baad size kam hua hai, toh nayi file return karein
//                    if (compressedSize < originalSize)
//                    {
//                        File.Delete(inputFilePath);
//                        return outputFileName;
//                    }
//                    else
//                    {
//                        // Agar size kam nahi hua, toh original hi return karein
//                        File.Delete(outputFilePath);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Agar koi error aaye (jaise password protected PDF), toh original file hi rahne dein
//                Console.WriteLine("PDF Compression Error: " + ex.Message);
//            }

//            return uniqueFileName; // Fallback: Original file
//        }
//    }
//}