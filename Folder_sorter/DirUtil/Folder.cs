using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private void Sort(object source, FileSystemEventArgs e)
        {
            //Sort the files
            foreach (var sub in SubFolders)
            {
                var files = new List<string>();
                //Extension select
                foreach (var ext in sub.AllowedExtensions)
                    //Get files for extension
                    files.AddRange(Directory.GetFiles(Path, $"*{ext}"));

                //Optional name select
                if (sub.NameMatches.Count != 0)
                {
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
                else
                {
                    if (files.Count == 0) continue;
                    foreach (var file in files)
                        File.Move
                        (file,
                            MergeSubAndFile(file, sub.Path));
                }
            }
        }

        private string MergeSubAndFile(string file, string sub)
        {
            var sb = new StringBuilder();
            string[] parts = file.Split('\\');
            sb.Append(sub);
            sb.Append($@"\\{parts[parts.Length - 1]}");

            return sb.ToString();
        }

        private string CreateQuery(SubFolder sub)
        {
            var sb = new StringBuilder();
            sb.Append("(");
            foreach (string name in sub.NameMatches)
                sb.Append($@"\w*{name}\w*|");
            sb.Replace('|', ')', sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}