using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTMusicUploader.Dialogues
{
    public partial class ConnectToYTMusic : OptimisedMetroForm
    {
        public ConnectToYTMusic() : base(formResizable: true,
                                          controlTagsAsTooltips: false)
        {
            InitializeComponent();
            SuspendDrawing(this);
        }

        private void ConnectToYTMusic_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);

       }
    }
}
