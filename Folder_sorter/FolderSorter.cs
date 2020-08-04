using Folder_sorter.DirUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace Folder_sorter
{
    public partial class FolderSorter : ServiceBase
    {
        List<Folder> activeFolders = new List<Folder>();

        public FolderSorter()
        {
            InitializeComponent();
            //Log setup
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("SorterSource"))
                EventLog.CreateEventSource(
                    "SorterSource", "SorterLog");
            eventLog1.Source = "SorterSource";
            eventLog1.Log = "SorterLog";
        }

        protected override void OnStart(string[] args)
        {
            activeFolders = Config.ReadConfig(args[0]);
            //TODO: Setup event for config change
        }

        protected override void OnStop()
        {
            EventLog.Dispose();
        }
    }
}
