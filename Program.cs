//using System.Reflection;
using System.Threading;
namespace AsyncFileLogger
{
    internal class Program
    {
        //declare global variables
        private static readonly object fileLock = new object(); // Lock object for file access
        private static string filePath = "";
        private static string fileName = "";
        private static int lineCount = 0;
        static void Main(string[] args)
        {
            ///check for user argument for filepath
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Please provide a file path as an argument.");
            //    return;
            //}
            ///assign file path
            //filePath = args[0];
            //filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());

            //filePath = $"log";
            ///create file path/overwrite if it exists
            filePath = "/app/log";
            if (!Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            Console.WriteLine($"File created successfully: {filePath}");
            fileName = $"{filePath}/out.txt";
            //fileName = $"/data/out.txt";
          
            //initialize the file.  Overwrite existing file.
            using (StreamWriter sw = File.CreateText(fileName))
            {
                string currentTimeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
                sw.WriteLine($"Line Number:0,Thread ID:0,Time:{currentTimeStamp}");                
            }

            ///create 10 threads
            Thread[] threads = new Thread[10];
            
            try
            {
                ///start 10 threads
                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(() => AppendLine(i));
                    threads[i].Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());                
            }
            // Wait for all threads to finish
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Finished writing to file.");
        }
        private static void AppendLine(int threadId)
        {   
            ///For every thread, append 10 lines to the file
            for (int i = 0; i < 10; i++)
            {
                string currentTimeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
                //string line = $"line {i + 1} from thread {threadId},currentTimeStamp";
                string line = $"Thread ID:{threadId},Time:{currentTimeStamp}";
                AppendLineToFile(line);
            }
                      
        }
        private static void AppendLineToFile(string line)
        {
            ///lock before writing
            lock (fileLock) 
            {
                lineCount++;///generate the serial number
                using (StreamWriter writer = File.AppendText(fileName))
                {
                    writer.WriteLine($"Line Number:{lineCount},{line}");
                }
            }
        }
    }
}
