using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqIssue
{
    class CsvParam : ICsvConversionProcessParameter
    {
        public string OutputFolderPath { get; set; }
        public string InputFolderPath { get; set; }

    }
}
