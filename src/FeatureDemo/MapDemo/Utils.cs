using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace MapDemo {
    public class DemoValuesProvider {
        const string updatedKey = "UtQsCivjZeuC7gjahROM~OH43ARXPug81UzThcSHI7g~ArG9ZFpBe1iZTXbdE3kyq6IZNyHEyFOG7otcBlMNxAInlhopSGvgy_nI1-2SPZX4";
        public string DevexpressBingKey { get { return updatedKey; } }
    }
    public static class DataLoader {
        public static XDocument LoadFromXmlResource(string path) {
            path = path.Replace('\\', '/');
            path = path.TrimStart('/');
            string uriPath = "ms-appx:///" + path;
            Task<StorageFile> operation = StorageFile.GetFileFromApplicationUriAsync(new Uri(uriPath)).AsTask<StorageFile>();
            StorageFile file = operation.Result;
            Task<string> fileReadingTask = FileIO.ReadTextAsync(file).AsTask<string>();
            string fileContent = fileReadingTask.Result;
            XDocument result = XDocument.Parse(fileContent, LoadOptions.None);
            return result;
        }
    }
}
