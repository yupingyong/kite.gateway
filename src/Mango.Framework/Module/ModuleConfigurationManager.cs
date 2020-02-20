using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
namespace Mango.Framework.Module
{
    public class ModuleConfigurationManager
    {
        public static readonly string ModulesFilename = "module.json";

        public List<ModuleInfo> GetModules()
        {
            List<ModuleInfo> modulesResult = new List<ModuleInfo>();
            try
            {
                var modulesFolderPath = Path.Combine(GlobalConfiguration.ContentRootPath, "Modules");

                var modulesFolder = new DirectoryInfo(modulesFolderPath);
                if (Directory.Exists(modulesFolderPath))
                {
                    var files = modulesFolder.GetFileSystemInfos(ModulesFilename, SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        using (var reader = new StreamReader(file.FullName))
                        {
                            string content = reader.ReadToEnd();
                            var moduleInfo = JsonConvert.DeserializeObject<ModuleInfo>(content);
                            modulesResult.Add(moduleInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return modulesResult;
        }
    }
}
