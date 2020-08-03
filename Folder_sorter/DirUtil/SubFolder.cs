using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_sorter.DirUtil
{
    public class SubFolder : FolderBase
    {

        public List<string> NameMatches;
        public List<string> AllowedExtensions;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the folder</param>
        public SubFolder(string path) 
            : base(path)
        {
            NameMatches = new List<string>();
            AllowedExtensions = new List<string>();
        }

        #endregion

    }
}
