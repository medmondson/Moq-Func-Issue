using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoqIssue
{
    public class FileService : IFileService
    {
        private enum SubFolders
        {
            Processing,
            Error,
            Complete
        }

        public string[] GetDirectoryFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string[] GetDirectoryFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public StreamWriter CreateWriter(string path, bool append)
        {
            return new StreamWriter(path, append);
        }

        private string ChangePathToSubFolder(string filePath, SubFolders subfolder, string dateStamp)
        {
            var directory = Path.GetDirectoryName(filePath);
            var filename = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    dateStamp,
                    Path.GetExtension(filePath));

            string subfilePath = string.Format(@"{0}\{1}\{2}",
                directory,
                subfolder,
                filename
                );

            //check subdirectory exists
            string subD = Path.GetDirectoryName(subfilePath);
            if (subD != null && !Directory.Exists(subD))
            {
                Directory.CreateDirectory(subD);
            }
            return subfilePath;
        }

        public void IterateFiles<TFileFunctionParameter, TFileFunctionOutput>(IEnumerable<string> filePaths,
            Func<string, TFileFunctionParameter, TFileFunctionOutput> fileFunction,
            TFileFunctionParameter parameter,
            FileIterationErrorAction errorAction,
            out List<TFileFunctionOutput> outputs)
        {
            var exceptions = new List<Exception>();
            outputs = new List<TFileFunctionOutput>();

            foreach (var filePath in filePaths)
            {
                var processingTimestap = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileToProcess = ChangePathToSubFolder(filePath, SubFolders.Processing, processingTimestap);
                Exception exception = null;

                try
                {
                    //move to processing folder
                    File.Move(filePath, fileToProcess);
                }
                catch (Exception ex)
                {
                    exception = new ApplicationException(
                        string.Format("Unable to move file {0} in to processing directory {1}.", filePath, fileToProcess),
                        ex);
                }

                //no error moving file
                if (exception == null)
                {
                    try
                    {
                        outputs.Add(fileFunction.Invoke(fileToProcess, parameter));
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }

                if (exception != null)
                {
                    //move to error folder
                    exceptions.Add(exception);
                    File.Move(fileToProcess, ChangePathToSubFolder(filePath, SubFolders.Error, processingTimestap));
                    if (errorAction == FileIterationErrorAction.Throw)
                    {
                        throw exception;
                    }
                }
                else
                {
                    //move to processed folder
                    File.Move(fileToProcess, ChangePathToSubFolder(filePath, SubFolders.Complete, processingTimestap));
                }
            }

            if (exceptions.Count > 0 && errorAction == FileIterationErrorAction.ContinueThenThrow)
            {
                var root = exceptions.First();

                if (exceptions.Count > 1)
                {
                    root = new Exception(exceptions.Select(e => e.Message).Aggregate((a, b) => a + ("\r\n" + b)),
                        root);
                }

                throw root;
            }
        }
    }
}
