using System;
using System.Windows.Forms;
using log4net;

namespace Cubokta.Puyo
{
    public partial class ConfigurationForm : Form
    {
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PuyofuConfiguration config;

        public ConfigurationForm(PuyofuConfiguration config)
        {
            this.config = config;

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            DebugRectEnabledChk.Checked = config.DebugRectEnabled;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            config.DebugRectEnabled = DebugRectEnabledChk.Checked;
            config.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ConfigurationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }
    }
}
