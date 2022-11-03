using Asp.Net_PartialViews.Services.Interfaces;
using System.IO;

namespace Asp.Net_PartialViews.Services
{
    public class FileService : IFileService
    {
        public string ReadFile(string path, string template)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                template = reader.ReadToEnd();
            }
            return(template);
        }
    }
}
