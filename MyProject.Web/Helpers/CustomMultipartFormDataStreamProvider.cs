using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyProject.Web.Helpers
{
	public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public string filePath { set; get; }
		public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

		public override string GetLocalFileName(HttpContentHeaders headers)
		{
            //if (headers.ContentDisposition.FileName.Contains("."))
            //{
				var fileName = string.Format("/{0}-{1}", Guid.NewGuid().ToString(), headers.ContentDisposition.FileName.Replace("\"", string.Empty));
				filePath = fileName;
				return fileName;
			//}
			//filePath = string.Empty;
			//return string.Empty;
			
		}
	}
}