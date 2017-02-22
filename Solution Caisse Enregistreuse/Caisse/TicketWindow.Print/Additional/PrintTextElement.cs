using System.Drawing;

namespace TicketWindow.Print.Additional
{
    public sealed class PrintTextElement
    {
        public PrintTextElement(string text, int x, int y, int w, int h, StringFormat stringFormat = null, Font font = null, Brush brush = null)
        {
            Rectangle = new Rectangle(x, y, w, h);
            StringFormat = stringFormat ?? new StringFormat();
            Font = font ?? new Font("Arial", 9);
            Brush = brush ?? new SolidBrush(Color.Black);
            Text = text;
        }

        public Brush Brush { get; set; }
        public Font Font { get; set; }
        public StringFormat StringFormat { get; set; }
        public string Text { get; set; }
        public Rectangle Rectangle { get; set; }
    }
}