namespace EnchCoreApi.Common.Logger {
    public class TextLog : GenericLog, IDisposable {
        protected StreamWriter _logWriter;
        public string FileName { get; set; }

        /// <summary>
        /// Creates the log file stream and sets the initial log level.
        /// </summary>
        /// <param name="filename">The output filename. This file will be overwritten if 'clear' is set.</param>
        /// <param name="clear">Whether or not to clear the log file on initialization.</param>
        public TextLog(string name, string filename, bool clear) : base(name) {
            FileName = filename;
            _logWriter = new StreamWriter(filename, !clear);
        }

        public void Dispose() {
            _logWriter.Dispose();
        }

        public override void Log(string log, LogFlag flag) {
            _logWriter.WriteLine("===============");
            _logWriter.WriteLine($"[{flag}][{DateTime.Now}]:");
            _logWriter.WriteLine($"{Name}:\n{log}");
        }
    }
}
