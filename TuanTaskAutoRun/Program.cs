using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace TaskAutoRun
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new TaskRunService(),  
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
