using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace APBD_Tutorial_4.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //log errors to file

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string query = httpContext.Request?.QueryString.ToString();
                string method = httpContext.Request?.Method;
                string bodyParameters = "";

                using (StreamReader streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true , 1024, true))
                {
                    bodyParameters = await streamReader.ReadToEndAsync();
                }
                
                if (File.Exists(Constants.LOG_FILE))
                {
                    File.Delete(Constants.LOG_FILE);
                }
                FileStream writer = new FileStream(Constants.LOG_FILE, FileMode.Create);
                writer.Close();

                await using TextWriter textWriter = new StreamWriter(Constants.LOG_FILE, true);
                textWriter.WriteLine("Path: " + path + 
                                     "; Query: " + query + "; " +
                                     "Method: " + method + 
                                     ",Body Parameters: " + bodyParameters);
            }
            
            await _next(httpContext);
        }
            
    }
}