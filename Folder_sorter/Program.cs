using System.Diagnostics;
using System.ServiceProcess;

namespace Folder_sorter
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FolderSorter()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
