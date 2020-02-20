using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Mango.Framework.Module
{
    public class ModuleAssemblyLoadContext : AssemblyLoadContext
    {
        public ModuleAssemblyLoadContext() : base(true)
        {
        }
    }
}
