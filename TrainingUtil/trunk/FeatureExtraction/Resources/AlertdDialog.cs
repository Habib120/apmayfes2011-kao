using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureExtraction
{
    public class AlertdDialog
    {
        public static DialogResult Error(string message)
        {
            return MessageBox.Show(message, "Feature Extraction", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Error(string message, Exception ex)
        {
#if DEBUG
            return Error(message + Environment.NewLine
                + ex.Message + Environment.NewLine
                + ex.StackTrace);
#else
            return Error(message + Environment.NewLine
                + ex.Message);
#endif
        }

        public static DialogResult Error(string message, IEnumerable<string> messages)
        {
            return Error(message + Environment.NewLine + String.Join("", messages.ToArray()));
        }

        public static DialogResult Confirm(string message)
        {
            return MessageBox.Show(message, "Feature Extraction ", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Question(string message)
        {
            return MessageBox.Show(message, "Feature Extraction", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult Notify(string message)
        {
            return MessageBox.Show(message, "Feature Extraction", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
