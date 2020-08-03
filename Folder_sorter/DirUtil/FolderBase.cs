using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_sorter.DirUtil
{
    public abstract class FolderBase
    {
        public string Path;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the folder</param>
        public FolderBase(string path)
        {
            this.Path = path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #endregion
    }
}
