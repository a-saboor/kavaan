using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace MyProject.Web.Helpers
{
	public class Uploader
	{
		public static string UploadImages(HttpFileCollectionBase files, string absolutePath, string relativePath, string prefix, ref List<string> pictures, ref string ErrorMessage, string fileName = "Images", bool compress = false)
		{
			//int i = 0;
			ErrorMessage = string.Empty;
			for (int index = 0; index < files.Count; index++)
			{
				try
				{
					var file = files[index];
					if (file != null)
					{
						string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
						String fileExtension = System.IO.Path.GetExtension(file.FileName);
						string FilePath;
						string MainDirectory = string.Empty;
						if (file.ContentType.Contains("image"))
						{
							if (SupportedImageFormat.Contains(fileExtension.ToLower()))
							{
								FilePath = string.Format("{0}{1}{2}{3}", relativePath, prefix, Guid.NewGuid().ToString()+"_", fileExtension);
								Directory.CreateDirectory(absolutePath + relativePath);
								//absolutePath += FilePath;
								if (compress)
								{
									Image img = Image.FromStream(file.InputStream);
									ImageCompressor.SaveJpeg(absolutePath + FilePath, img, 30);
								}
								else
								{
									file.SaveAs(absolutePath + FilePath);
								}
								pictures.Add(FilePath);
							}
							else
							{
								ErrorMessage += string.Format("Image {0} format not supported !<br>", index + 1);
							}
						}
						else
						{
							ErrorMessage += string.Format("Wrong format for image {0} !<br>", index + 1);
						}
					}
				}
				catch (Exception ex)
				{ }
			}
			return string.Empty;
		}
		public static string UploadImage(HttpPostedFileBase files, string absolutePath, string relativePath, string prefix, ref string ErrorMessage, string fileName = "Image", bool compress = false)
		{
			ErrorMessage = string.Empty;
			var file = files;
			if (file != null)
			{
				string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
				String fileExtension = System.IO.Path.GetExtension(file.FileName);
				string FilePath;
				string MainDirectory = string.Empty;
				if (file.ContentType.Contains("image"))
				{
					if (SupportedImageFormat.Contains(fileExtension.ToLower()))
					{
						Regex pattern = new Regex("[~`!@#$%^&*()+<>?:,.]");
						string relPath = pattern.Replace(relativePath, "_");
						FilePath = string.Format("{0}{1}{2}{3}", relPath, prefix, Guid.NewGuid().ToString(), fileExtension);
						Directory.CreateDirectory(absolutePath + relPath);
						absolutePath += FilePath;

						if (compress)
						{
							Image img = Image.FromStream(file.InputStream);
							ImageCompressor.SaveJpeg(absolutePath, img, 30);
						}
						else
						{
							file.SaveAs(absolutePath);
						}

						return FilePath;
					}
					else
					{
						ErrorMessage = "Image Format Not supported !";
					}
				}
				else
				{
					ErrorMessage = "Wrong format for image !";
				}
			}
			else
			{
				ErrorMessage = "Please Select an image first !";
			}
			return string.Empty;
		}


		public static string UploadImage(HttpFileCollectionBase files, string absolutePath, string relativePath, string prefix, ref string ErrorMessage, string fileName = "Image", bool compress = false)
		{
			ErrorMessage = string.Empty;
			if (files.Count > 0)
			{
				var file = files[fileName];
				if (file != null)
				{
					string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
					String fileExtension = System.IO.Path.GetExtension(file.FileName);
					string FilePath;
					string MainDirectory = string.Empty;
					if (file.ContentType.Contains("image"))
					{
						if (SupportedImageFormat.Contains(fileExtension.ToLower()))
						{
							Regex pattern = new Regex("[~`!@#$%^&*()+<>?:,.]");
							string relPath = pattern.Replace(relativePath, "_");
							FilePath = string.Format("{0}{1}{2}{3}", relPath, prefix, Guid.NewGuid().ToString(), fileExtension);
							Directory.CreateDirectory(absolutePath + relPath);
							absolutePath += FilePath;

							if (compress)
							{
								Image img = Image.FromStream(file.InputStream);
								ImageCompressor.SaveJpeg(absolutePath, img, 30);
							}
							else
							{
								file.SaveAs(absolutePath);
							}

							return FilePath;
						}
						else
						{
							ErrorMessage = "Image Format Not supported !";
						}
					}
					else
					{
						ErrorMessage = "Wrong format for image !";
					}
				}
				else
				{
					ErrorMessage = "Please Select an image first !";
				}
			}
			else
			{
				return "~/Assets/AppFiles/Images/default.png";
			}
			return string.Empty;
		}

		public static string UploadVideo(HttpFileCollectionBase files, string absolutePath, string relativePath, string prefix, ref string ErrorMessage, string fileName = "Video", bool compress = false)
		{
			ErrorMessage = string.Empty;
			if (files.Count > 0)
			{
				var file = files[fileName];
				if (file != null)
				{
					string[] SupportedImageFormat = { ".mp4", ".MKV", ".FLV", ".MOV" };
					String fileExtension = System.IO.Path.GetExtension(file.FileName);
					string FilePath;
					string MainDirectory = string.Empty;
					if (file.ContentType.Contains("video"))
					{
						if (SupportedImageFormat.Contains(fileExtension.ToLower()))
						{
							FilePath = string.Format("{0}{1}{2}{3}", relativePath, prefix, Guid.NewGuid().ToString(), fileExtension);
							Directory.CreateDirectory(absolutePath + relativePath);
							absolutePath += FilePath;

							if (compress)
							{
								Image img = Image.FromStream(file.InputStream);
								ImageCompressor.SaveJpeg(absolutePath, img, 30);
							}
							else
							{
								file.SaveAs(absolutePath);
							}

							return FilePath;
						}
						else
						{
							ErrorMessage = "Image Format Not supported !";
						}
					}
					else
					{
						ErrorMessage = "Wrong format for image !";
					}
				}
				else
				{
					ErrorMessage = "Please Select an image first !";
				}
			}
			else
			{
				return "~/Assets/AppFiles/Images/default.png";
			}
			return string.Empty;
		}

		public static string UploadDocs(HttpFileCollectionBase files, string absolutePath, string relativePath, string prefix, ref string ErrorMessage, string fileName = "Image", bool compress = false)
		{
			ErrorMessage = string.Empty;
			if (files.Count > 0)
			{
				var file = files[fileName];
				if (file != null)
				{
					string[] SupportedImageFormat = { ".docx", ".pdf"};
					String fileExtension = System.IO.Path.GetExtension(file.FileName);
					string FilePath;
					string MainDirectory = string.Empty;
					if (file.ContentType.Contains("image"))
					{
						if (SupportedImageFormat.Contains(fileExtension.ToLower()))
						{
							FilePath = string.Format("{0}{1}{2}{3}", relativePath, prefix, Guid.NewGuid().ToString(), fileExtension);
							Directory.CreateDirectory(absolutePath + relativePath);
							absolutePath += FilePath;

							if (compress)
							{
								Image img = Image.FromStream(file.InputStream);
								ImageCompressor.SaveJpeg(absolutePath, img, 30);
							}
							else
							{
								file.SaveAs(absolutePath);
							}

							return FilePath;
						}
						else
						{
							ErrorMessage = "Document Format Not supported !";
							//ErrorMessage = "";//"";//CVFormatNotsupported;
						}
					}
					else if (file.ContentType.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document") || file.ContentType.Contains("application/pdf"))
					{
						if (SupportedImageFormat.Contains(fileExtension.ToLower()))
						{
							FilePath = string.Format("{0}{1}{2}{3}", relativePath, prefix, Guid.NewGuid().ToString(), fileExtension);
							Directory.CreateDirectory(absolutePath + relativePath);
							absolutePath += FilePath;

							file.SaveAs(absolutePath);


							return FilePath;
						}
						else
						{
							ErrorMessage = "Document Format Not supported !";
							//ErrorMessage = "";//"";//CVFormatNotsupported;
						}
					}
					else
					{
						ErrorMessage = "Wrong format for Document !";
						//ErrorMessage = "";//"";//WrongFormatforCV;
					}
				}
				else
				{

					ErrorMessage = "Please choose correct file !";
					//ErrorMessage = "";//"";//PleaseChooseCorrectFile; ;
				}
			}
			else
			{
				return "~/Assets/AppFiles/Images/default.png";
			}
			return string.Empty;
		}

		public static string SaveImage(string sourceImageUrl, string absolutePath, string relativePath, string prefix, ImageFormat format)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				// Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
				string FilePath;
				string MainDirectory = string.Empty;
				FilePath = string.Format("{0}{1}{2}.{3}", relativePath, prefix, Guid.NewGuid().ToString(), format.ToString());
				Directory.CreateDirectory(absolutePath + relativePath);
				absolutePath += FilePath;

				WebClient client = new WebClient();
				Stream stream = client.OpenRead(sourceImageUrl);
				Bitmap bitmap; bitmap = new Bitmap(stream);

				if (bitmap != null)
				{
					bitmap.Save(absolutePath);
				}

				stream.Flush();
				stream.Close();
				client.Dispose();

				return FilePath;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		public static bool SaveImages(string[] sourceImageUrls, string absolutePath, string relativePath, string prefix, ImageFormat format, ref List<string> pictures)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				// Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

				foreach (var sourceImageUrl in sourceImageUrls)
				{
					string FilePath;
					string MainDirectory = string.Empty;
					FilePath = string.Format("{0}{1}{2}.{3}", relativePath, prefix, Guid.NewGuid().ToString(), format.ToString());
					Directory.CreateDirectory(absolutePath + relativePath);
					//absolutePath += FilePath;

					WebClient client = new WebClient();
					Stream stream = client.OpenRead(sourceImageUrl);
					Bitmap bitmap; bitmap = new Bitmap(stream);

					if (bitmap != null)
					{
						bitmap.Save(absolutePath + FilePath, format);
						pictures.Add(FilePath);
					}

					stream.Flush();
					stream.Close();
					client.Dispose();
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}