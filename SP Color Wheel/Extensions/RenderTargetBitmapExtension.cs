using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SP_Color_Wheel.Extensions
{
    public static class RenderTargetBitmapExtension
    {
        public static async Task CopyPixelsAsync(this RenderTargetBitmap bitmap,Int32Rect int32Rect,Array array,int stride,int offset)
        {
            await Task.Run(() =>
            {
                bitmap.CopyPixels(int32Rect, array, stride, offset);
            });
        }
    }
}
