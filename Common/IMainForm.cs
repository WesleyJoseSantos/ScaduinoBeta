using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IMainForm
    {
        IAppData AppData { get; set; }

        AppDataFileDialog FileDialog { get; set; }
    }
}
