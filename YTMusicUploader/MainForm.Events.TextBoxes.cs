using System;
using System.Windows.Forms;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void TbThrottleSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            RemoveDoublePoints();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);           
            ThrottleTextChangedTimer.Stop();
            ThrottleTextChangedTimer.Start();
        }

        private void TbThrottleSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            RemoveDoublePoints();
            e.Handled = !char.IsDigit((char)e.KeyValue) && !char.IsControl((char)e.KeyValue);            
            ThrottleTextChangedTimer.Stop();
            ThrottleTextChangedTimer.Start();
        }

        private void TbThrottleSpeed_TextChanged(object sender, EventArgs e)
        {
            RemoveDoublePoints();
            if (tbThrottleSpeed.Text != "∞")
            {
                tbThrottleSpeed.Text = tbThrottleSpeed.Text.RemoveLetters().Trim();                
                ThrottleTextChangedTimer.Stop();
                ThrottleTextChangedTimer.Start();
            }
        }

        private void ThrottleTextChangedTimer_Elapsed(object sender, EventArgs e)
        {
            RemoveDoublePoints();
            ValidateThrottleTextBox();
            ThrottleTextChangedTimer.Stop();
            if (Settings != null)
            {
                if (string.IsNullOrWhiteSpace(tbThrottleSpeed.Text) ||
                    tbThrottleSpeed.Text.Trim() == "0")
                    tbThrottleSpeed.Text = "∞";

                if (tbThrottleSpeed.Text != "∞")
                {
                    try
                    {
                        if (Convert.ToDouble(tbThrottleSpeed.Text) < 0.1)
                            tbThrottleSpeed.Text = "0.1";

                        Settings.ThrottleSpeed = Convert.ToInt32(Convert.ToDouble(tbThrottleSpeed.Text) * 1000000);

                    }
                    catch
                    {
                        tbThrottleSpeed.Text = "∞";
                        Settings.ThrottleSpeed = -1;
                    }
                }
                else
                    Settings.ThrottleSpeed = -1;

                Settings.Save();
            }
        }

        private void RemoveDoublePoints()
        {
            tbThrottleSpeed.Text = tbThrottleSpeed.Text.Replace("..", ".").Replace("..", ".");

            while (tbThrottleSpeed.Text.CountSubstringOccurrences(".") > 1)
            {
                int lastPointCharIndex = tbThrottleSpeed.Text.LastIndexOf(".");
                tbThrottleSpeed.Text = tbThrottleSpeed.Text.Remove(lastPointCharIndex, 1);
            }           
        }

        private void ValidateThrottleTextBox()
        {
            if (tbThrottleSpeed.Text.StartsWith("."))
                tbThrottleSpeed.Text = "0" + tbThrottleSpeed.Text;

            if (tbThrottleSpeed.Text.EndsWith("."))
                tbThrottleSpeed.Text = tbThrottleSpeed.Text.Substring(0, tbThrottleSpeed.Text.Length - 1);
        }
    }
}
