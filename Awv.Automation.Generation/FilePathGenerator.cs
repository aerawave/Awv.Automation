using System;
using System.Collections.Generic;
using System.IO;

namespace Awv.Automation.Generation
{
    public class FilePathGenerator : QueriedGenerator<string, string>
    {
        public static readonly Func<string, IEnumerable<string>> GET_FILES = directory => Directory.GetFiles(directory);

        public FilePathGenerator(string directory) : base (directory, GET_FILES)
        {
        }

        public FilePathGenerator(IEnumerable<string> files)
            : base(files)
        {
        }
    }
}
