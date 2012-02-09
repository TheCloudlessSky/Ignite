using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core;
using dotless.Core.Loggers;
using dotless.Core.Stylizers;
using dotless.Core.Importers;
using System.IO;

namespace Ignite.Processing
{
    public class DotLessStyleSheetProcessor : IStyleSheetProcessor
    {
        private readonly string appPath;

        public DotLessStyleSheetProcessor(string appPath)
        {
            this.appPath = appPath;
        }

        private class IgniteDotLessLogger : dotless.Core.Loggers.ILogger
        {
            public void Debug(string message)
            {

            }

            public void Error(string message)
            {
                throw new ProcessingException("Failed to execute DotLess processor: " + message);
            }

            public void Info(string message)
            {
                
            }

            public void Log(dotless.Core.Loggers.LogLevel level, string message)
            {
                if (level == dotless.Core.Loggers.LogLevel.Error)
                {
                    throw new ProcessingException("Failed to execute DotLess processor: " + message);
                }
            }

            public void Warn(string message)
            {
                throw new ProcessingException("Failed to execute DotLess processor: " + message);
            }
        }

        public string Preprocess(string data, string fileName)
        {
            // Ensure that the current directory for the @import statements
            // is the directory of the file.
            var fullFilePath = Path.Combine(this.appPath, fileName.Replace('/', '\\'));
            Directory.SetCurrentDirectory(Path.GetDirectoryName(fullFilePath));

            // Execute LESS engine but do not compress or optimize. This allows resolving 
            // @import statements during debugging as well as a pretty-print version of the CSS.
            var e = new LessEngine(new dotless.Core.Parser.Parser(optimization: 0), new IgniteDotLessLogger(), compress: false);
            return e.TransformToCss(data, fileName);
        }

        public string Process(string data)
        {
            return Less.Parse(data, new dotless.Core.configuration.DotlessConfiguration()
            {
                MinifyOutput = true,
                Logger = typeof(IgniteDotLessLogger)
            });
        }
    }
}
