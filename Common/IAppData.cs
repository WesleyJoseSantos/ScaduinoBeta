using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IAppData
    {
        string DefaultFile { get; }
        void Save(string file);
        void SaveDefault();
        IAppData Load(string file);
        IAppData LoadDefault();
        IAppData New();
    }
}
