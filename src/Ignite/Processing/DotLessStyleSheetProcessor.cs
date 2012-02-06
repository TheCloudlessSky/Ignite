using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core;
using dotless.Core.Loggers;

namespace Ignite.Processing
{
    // TODO: Account for individual importing?
    public class DotLessStyleSheetProcessor : IStyleSheetProcessor
    {
        private class IgniteDotLessLogger : dotless.Core.Loggers.ILogger
        {
            public void Debug(string message)
            {

            }

            public void Error(string message)
            {
                // TODO: IgniteException
                throw new Exception("Failed to execute compressor: " + message);
            }

            public void Info(string message)
            {
                
            }

            public void Log(dotless.Core.Loggers.LogLevel level, string message)
            {
                if (level == dotless.Core.Loggers.LogLevel.Error)
                {
                    // TODO: IgniteException
                    throw new Exception("Failed to execute compressor: " + message);
                }
            }

            public void Warn(string message)
            {

            }
        }

        private readonly IDebugState debugState;

        public DotLessStyleSheetProcessor(IDebugState debugState)
        {
            this.debugState = debugState;
        }

        public string Execute(string data)
        {
            // TODO: Imports.
            // Decompile dotless and figure out default usage.
            // TODO: Need to pass path down to this.
            // new LessEngine(new dotless.Core.Parser.Parser(new dotless.Core.Stylizers.ConsoleStylizer(), new dotless.Core.Importers.Importer())

            return Less.Parse(data, new dotless.Core.configuration.DotlessConfiguration()
            {
                MinifyOutput = !debugState.IsDebugging(),
                Logger = typeof(IgniteDotLessLogger)
            });
        }
    }
}
