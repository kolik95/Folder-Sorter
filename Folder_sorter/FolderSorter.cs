using Folder_sorter.DirUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;

namespace Folder_sorter
{
    public partial class FolderSorter : ServiceBase
    {
        List<Folder> activeFolders = new List<Folder>();

        public FolderSorter()
        {
            InitializeComponent();
            //Log setup
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("SorterSource"))
                System.Diagnostics.EventLog.CreateEventSource(
                    "SorterSource", "SorterLog");
            eventLog1.Source = "SorterSource";
            eventLog1.Log = "SorterLog";
        }

        protected override void OnStart(string[] args)
        {
            //installutil.exe
            eventLog1.WriteEntry("Reading configuration file", EventLogEntryType.Information);
            activeFolders = Config.ReadConfig(args[0]);
            //TODO: Setup event for config change
        }

        protected override void OnStop()
        {
        }
    }
}
