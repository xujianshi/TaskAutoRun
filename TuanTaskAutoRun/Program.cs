using System.ServiceProcess;

namespace TaskAutoRun
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
            {
                new TaskRunService(),
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}