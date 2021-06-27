using DotNetCom.General.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCom.General
{
    public interface IComModule : ITagWriterConteiner
    {
        string Name { get; set; }
    }
}
