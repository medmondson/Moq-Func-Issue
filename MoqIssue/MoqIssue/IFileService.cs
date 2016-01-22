using System;
using System.Collections.Generic;
using System.IO;

namespace MoqIssue
{
    public interface IFileService
    {
        void IterateFiles<TFileFunctionParameter, TFileFunctionOutput>(IEnumerable<string> filePaths,
            Func<string, TFileFunctionParameter, TFileFunctionOutput> fileFunction,
            TFileFunctionParameter fileFunctionParameter,
            FileIterationErrorAction errorAction,
            out List<TFileFunctionOutput> outputs);

        string[] GetDirectoryFiles(string path);
        string[] GetDirectoryFiles(string path, string searchPattern);
        string GetFileName(string path);
        StreamWriter CreateWriter(string path, bool append);
    }

    public enum FileIterationErrorAction
    {
        Continue,
        ContinueThenThrow,
        Throw
    }
}