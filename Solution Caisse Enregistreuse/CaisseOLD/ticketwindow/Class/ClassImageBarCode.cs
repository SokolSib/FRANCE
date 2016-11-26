using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;

namespace ticketwindow.Class
{
    class ClassImageBarCode
    {
        private BarcodeLib.TYPE selectTYPE(string barcode)
        {

            int caseSwitch = barcode.Replace(" ", "").Length;

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
        public Image get_bc(string bc)
        {
            BarcodeLib.Barcode b = new Barcode(bc, selectTYPE(bc));

            b.Encode(selectTYPE(bc), bc, Color.Black, Color.White, 200, 40);

            return b.EncodedImage;
        }

    }
}
