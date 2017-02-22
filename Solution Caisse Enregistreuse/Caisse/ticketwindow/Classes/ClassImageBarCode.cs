using System.Drawing;
using BarcodeLib;

namespace TicketWindow.Classes
{
    internal class ClassImageBarCode
    {
        private static TYPE SelectType(string barcode)
        {
            var caseSwitch = barcode.Replace(" ", "").Length;

            switch (caseSwitch)
            {
                case 11:
                    return TYPE.CODE11;
                case 8:
                    return TYPE.EAN8;
                case 12:
                    return TYPE.UCC12;
                case 18:
                    return TYPE.CODE128;
                default:
                    return TYPE.EAN13;
            }
        }

        public Image GetBc(string bc)
        {
            var b = new Barcode(bc, SelectType(bc));

            b.Encode(SelectType(bc), bc, Color.Black, Color.White, 200, 40);

            return b.EncodedImage;
        }
    }
}