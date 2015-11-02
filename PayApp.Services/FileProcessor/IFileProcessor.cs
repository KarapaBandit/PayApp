namespace PayApp.Services.FileProcessor
{
    public interface IFileProcessor
    {
        void Run(string path);
        void ProcessFile(string path);
        void ProcessLine(string line);
    }
}
