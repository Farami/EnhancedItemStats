namespace EnhancedItemStats.Helper {
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class ClipboardNotification {
        public static event EventHandler ClipboardUpdate;

        private static NotificationForm form = new NotificationForm();

        private static void OnClipboardUpdate(EventArgs e) {
            var handler = ClipboardUpdate;
            handler?.Invoke(null, e);
        }

        private class NotificationForm : Form {
            public NotificationForm() {
                NativeMethods.SetParent(this.Handle, NativeMethods.HWND_MESSAGE);
                NativeMethods.AddClipboardFormatListener(this.Handle);
            }

            protected override void WndProc(ref Message m) {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE) {
                    OnClipboardUpdate(null);
                }
                base.WndProc(ref m);
            }
        }
    }

    internal static class NativeMethods {
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}
