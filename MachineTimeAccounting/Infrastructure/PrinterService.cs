using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;


namespace MachineTimeAccounting
{
    public static class PrinterService
    {
        public static void Print(string text)
        {
            var document = new PrintDocument();
            document.PrintPage += (s, e) =>
            {
                e.Graphics.DrawString(text, new Font("Arial", 16),
                    new SolidBrush(Color.Black),
                    new RectangleF(0, 0,
                    document.DefaultPageSettings.PrintableArea.Width, document.DefaultPageSettings.PrintableArea.Height));
            };

            var dialog = new PrintDialog
            {
                AllowSomePages = true,
                Document = document
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                document.Print();
        }
    }
}
