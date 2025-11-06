using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ec.edu.monster.vista.ui
{
    public class PlaceholderTextBox : TextBox
    {
        [Browsable(true)]
        [DefaultValue("")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value ?? string.Empty; UpdateCue(); }
        }
        private string _placeholder = string.Empty;

        // Mensaje Win32 para cue banner
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public PlaceholderTextBox()
        {
            // Estilos suaves
            BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            UpdateCue();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // El cue banner lo dibuja Windows, no hace falta pintar manualmente
        }

        private void UpdateCue()
        {
            if (!IsHandleCreated) return;
            SendMessage(Handle, EM_SETCUEBANNER, IntPtr.Zero, _placeholder);
        }
    }
}
