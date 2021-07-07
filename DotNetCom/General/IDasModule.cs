using DotNetCom.General.NamedObject;
using DotNetCom.General.Tags;
using System.ComponentModel;
using System.Drawing.Design;

namespace DotNetCom.General
{
    public interface IDasModule
    {
        bool Enabled { get; set; }

        TagCollection TagCollection { get; set; }

        string Name { get; set; }

        void Start();

        void Stop();
    }
}
