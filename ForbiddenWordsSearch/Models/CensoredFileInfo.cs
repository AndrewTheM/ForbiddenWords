using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForbiddenWordsSearch
{
    public class CensoredFileInfo
    {
        public string Name { get; set; }

        public string OriginalPath { get; set; }

        public string CopyPath { get; set; }

        public long Size { get; set; }

        public int CensorCount { get; set; }
    }
}
