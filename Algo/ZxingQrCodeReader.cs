namespace DevWeek.Algo
{
    using System;
    using System.DrawingCore;
    using System.IO;
    using ZXing;
    using ZXing.Common;
    using ZXing.QrCode;
    using ZXing.ZKWeb;

    /// <summary>
    /// ZXing.QrCode.QRCodeReader wrapper
    /// </summary>
    /// <remarks>
    /// https://social.technet.microsoft.com/wiki/contents/articles/37921.qr-code-generator-in-asp-net-core-using-zxing-net.aspx
    /// </remarks>
    public class ZxingQrCodeReader : IReadQrCode
    {
        public string DecodePngFile(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);

            // create an in memory bitmap
            var bitmap = (Bitmap)Bitmap.FromFile(fileName);

            return GetEncodedBitmapString(bitmap);
        }

        public string DecodePngStream(Stream pngStream)
        {
            try
            {
                // create an in memory bitmap
                var bitmap = (Bitmap)Bitmap.FromStream(pngStream);

                return GetEncodedBitmapString(bitmap);
            }
            catch
            {
                // A null reference or invalid value was found [GDI+ status: InvalidParameter]
                return null;
            }
        }

        private static string GetEncodedBitmapString(Bitmap bitmap)
        {
            // create a barcode reader instance
            Reader qrCodeReader = new QRCodeReader();

            // class which represents the luminance values for a bitmap object
            LuminanceSource source = new BitmapLuminanceSource(bitmap);

            // thresholding algo 4 high frequency images of barcodes with black data on white backgrounds
            var binarizer = new HybridBinarizer(source);

            // core bitmap class used by ZXing
            var binaryBitmap = new BinaryBitmap(binarizer);

            // decode
            Result pngResult = qrCodeReader.decode(binaryBitmap);

            return pngResult?.Text;
        }
    }
}
