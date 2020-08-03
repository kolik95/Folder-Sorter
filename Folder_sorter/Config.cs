using System.Collections.Generic;
using System.IO;
using Folder_sorter.DirUtil;

namespace Folder_sorter
{
    public static class Config
    {

        public static List<Folder> ReadConfig(string config_path)
        {
            var folders = new List<Folder>();
            string[] lines = File.ReadAllLines(config_path);
            string marker = "";
            foreach (string line in lines)
            {
                if (line[0] == '#')
                    marker = line.Remove(0, 1);
                else
                    switch (marker)
                    {
                        case "Start":
                            folders.Add(new Folder(line));
                            break;
                        case "folder":
                            folders[folders.Count - 1]
                                .SubFolders.Add
                                    (new SubFolder
                                    ($@"{folders[folders.Count - 1].Path}\{line}"));
                            break;
                        case "names":
                            //Parent folder
                            folders[folders.Count - 1]
                                //Sub folder
                                .SubFolders[folders[folders.Count - 1]
                                                .SubFolders.Count-1]
                                .NameMatches.Add(line);
                            break;
                        case "ext":
                            //Parent folder
                            folders[folders.Count - 1]
                                //Sub folder
                                .SubFolders[folders[folders.Count - 1]
                                    .SubFolders.Count-1]
                                .AllowedExtensions.Add(line);
                            break;
                    }
            }

            return folders;
        }
    }
}
