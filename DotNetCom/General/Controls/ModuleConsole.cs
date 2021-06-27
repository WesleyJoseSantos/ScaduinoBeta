using System.Windows.Forms;

namespace DotNetCom.General.Controls
{
    public class ModuleConsole
    {
        public TextBoxBase Log { get; set; }
        public TextBoxBase Errors { get; set; }
        public ModuleStatus Status { get; set; }

        public ModuleConsole() { }

        public ModuleConsole(TextBoxBase log, TextBoxBase errors)
        {
            Log = log;
            Errors = errors;
        }

        public ModuleConsole(TextBoxBase log, TextBoxBase errors, ModuleStatus moduleStatus)
        {
            Log = log;
            Errors = errors;
            Status = moduleStatus;
        }

        public ModuleConsole(TextBoxBase log, TextBoxBase errors,
                             ToolStripStatusLabel speedLabel, ToolStripStatusLabel connectionStatusLabel)
        {
            Log = log;
            Errors = errors;

            Status = new ModuleStatus(speedLabel, connectionStatusLabel);
        }
    }
}
