using DotNetCom.General.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCom.General
{
    public interface IComModule : ITagWriterConteiner
    {
        bool Enabled {get; set;}

        string Name { get; set; }

        void Start();

        void Stop();
    }
}
