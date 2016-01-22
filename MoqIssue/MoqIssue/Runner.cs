using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqIssue
{
    public class Runner
    {
        public Runner(IFileService service)
        {
            string[] paths = new[] { "path1" };

            List<ProcessOutput> output = new List<ProcessOutput>();

            service.IterateFiles(paths, ProcessFile, new CsvParam(), FileIterationErrorAction.ContinueThenThrow, out output);
        }

        public ProcessOutput ProcessFile(string file, ICsvConversionProcessParameter parameters)
        {
            return new ProcessOutput();
        }
    }
}
