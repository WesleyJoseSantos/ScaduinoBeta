using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IAppData
    {
        string File { get; set; }
        bool IsDefaultFile { get; }
        string Directory { get; set; }
        void SaveAs(string file);
        void Save();
        IAppData Load(string file);
        IAppData Load();
        IAppData New();
    }
}
