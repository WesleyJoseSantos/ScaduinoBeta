using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScaduinoDevices
{
    public interface IDevice
    {
        string Name { get; set; }

        void Start();

        void Stop();
    }
}
