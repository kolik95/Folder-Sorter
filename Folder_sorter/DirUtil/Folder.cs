using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace Folder_sorter.DirUtil
{
    public class Folder : FolderBase
    {
        public List<SubFolder> SubFolders;

        private FileSystemWatcher _watcher;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the folder</param>
        public Folder(string path)
            : base(path)
        {
            this.SubFolders = new List<SubFolder>();
            this._watcher = new FileSystemWatcher();
            _watcher.Path = this.Path;
            _watcher.NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;
            _watcher.Filter = "";
            _watcher.Changed += Sort;
            _watcher.Renamed += Sort;
            _watcher.Created += Sort;
            _watcher.EnableRaisingEvents = true;
        }

        #endregion

        public void StopWatcher() => _watcher.EnableRaisingEvents = false;

        private void Sort(object source, FileSystemEventArgs e)
        {
            //Sort the files
            foreach (var sub in SubFolders)
            {
                var files = Directory.GetFiles(this.Path);
                var reg = new Regex(CreateQuery(sub));
                var query = from file in files
                    where reg.IsMatch(file)
                    select file;
                if (query.ToArray().Length == 0) continue;
                foreach (var file in query)
                    File.Move
                    (file,
                        MergeSubAndFile(file, sub.Path));
            }
        }

        /// <summary>
        /// Merges file path with a subfolder path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        private string MergeSubAndFile(string file, string sub)
        {
            var sb = new StringBuilder();
            string[] parts = file.Split('\\');
            sb.Append(sub);
            sb.Append($@"\\{parts[parts.Length - 1]}");

            return sb.ToString();
        }

        /// <summary>
        /// Creates a regex matching the subfolder requirements
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        private string CreateQuery(SubFolder sub)
        {
            var sb = new StringBuilder();

            //Empty list check
            if(sub.NameMatches.Count == 0)
                sub.NameMatches.Add("");
            if (sub.AllowedExtensions.Count == 0)
                sub.AllowedExtensions.Add(".*");

            //Name match
            sb.Append("(");
            foreach (string name in sub.NameMatches)
                sb.Append($@".*{name}.*|");
            sb.Replace('|', ')', sb.Length - 1, 1);

            //Extension match
            sb.Append(@"+(");
            foreach (string ext in sub.AllowedExtensions)
                sb.Append($@"{ext}|");
            sb.Replace('|', ')', sb.Length - 1, 1);
            sb.Append("$");

            return sb.ToString();
        }
    }
}