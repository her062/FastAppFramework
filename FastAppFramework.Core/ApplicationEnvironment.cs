using System.IO;
using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FastAppFramework.Core
{
    public class ApplicationEnvironment : IApplicationEnvironment
    {
#region Properties
        public static string AssemblyName
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly == null)
                    throw new ApplicationException("Failed to get entry assembly");

                var name = assembly.GetName().Name;
                if (string.IsNullOrEmpty(name))
                    throw new ApplicationException("The entry assembly name is null or empty");

                return name;
            }
        }
        public static string AssemblyFile
        {
            get
            {
                string? path = Assembly.GetEntryAssembly()?.Location;
                if (string.IsNullOrEmpty(path))
                {
                    FastApplication.Current.Logger.LogWarning("Cannot determine the executable file path from EntryAssembly");
                    using (var module = Process.GetCurrentProcess().MainModule)
                        path = module?.FileName;
                }
                if (string.IsNullOrEmpty(path))
                    throw new ApplicationException("Cannot determine the executable file path");

                return path;
            }
        }
        public static string AssemblyFolder
        {
            get
            {
                var folder = System.IO.Path.GetDirectoryName(AssemblyFile);
                if (string.IsNullOrEmpty(folder))
                    throw new ApplicationException($"The folder for {AssemblyFile} is null or empty");

                return folder;
            }
        }

        public string ApplicationDataFolder => this._dataFolder;
        public string TempFolder => this._tempFolder;
        public string LogFolder => System.IO.Path.Combine(this.TempFolder, "logs");
#endregion

#region Fields
        private string _dataFolder;
        private string _tempFolder;
#endregion

#region Constructor/Destructor
        public ApplicationEnvironment()
        {
            // Setup Fields.
            {
                this._dataFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyName);
                this._tempFolder = System.IO.Path.Combine(Path.GetTempPath(), AssemblyName);
            }
        }
#endregion
    }
}