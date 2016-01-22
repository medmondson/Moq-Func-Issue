using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MoqIssue;
using NUnit.Framework;

namespace MoqIssueTest
{
    public class MoqIssueTest
    {
        [Test]
        public void IterateFiles_Called()
        {
            Mock<IFileService> mock = new Mock<IFileService>();
            var flex = new Runner(mock.Object);

            List<ProcessOutput> outputs;
            mock.Verify(x => x.IterateFiles(It.IsAny<IEnumerable<string>>(),
                           It.IsAny<Func<string, ICsvConversionProcessParameter, ProcessOutput>>(),
                           It.IsAny<ICsvConversionProcessParameter>(),
                           It.IsAny<FileIterationErrorAction>(),
                           out outputs), Times.Once);

            //Also tried
            //List<ProcessOutput> outputs;
            //mock.Verify(x => x.IterateFiles(It.IsAny<string[]>(),
            //        flex.ProcessFile,
            //        It.IsAny<ICsvConversionProcessParameter>(),
            //        It.IsAny<FileIterationErrorAction>(),
            //        out outputs), Times.Once);

        }
    }
}
