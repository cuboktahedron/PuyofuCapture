using System.Drawing;
using System.Drawing.Imaging;

namespace Cubokta.Common
{
    /// <summary>
    /// ビットマップアクセスを高速化するためのアクセサ
    /// </summary>
    /// <remarks>
    /// このクラスではunsafeコードを使用しているため、ビルドの設定で
    /// unsafeコードを許可の設定を行う必要があります。
    /// </remarks>
    public class RapidBitmapAccessor
    {
        /// <summary>オリジナルのBitmapオブジェクト</summary>
        private Bitmap _bmp = null;

        /// <summary>Bitmapに直接アクセスするためのオブジェクト</summary>
        private BitmapData _img = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="original">オリジナルのBitmapオブジェクト</param>
        public RapidBitmapAccessor(Bitmap original)
        {
            _bmp = original;
        }

        /// <summary>
        /// Bitmap処理の高速化開始
        /// </summary>
        public void BeginAccess()
        {
            // Bitmapに直接アクセスするためのオブジェクト取得
            _img = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Bitmap処理の高速化終了
        /// </summary>
        public void EndAccess()
        {
            if (_img != null)
            {
                // Bitmapに直接アクセスするためのオブジェクト開放
                _bmp.UnlockBits(_img);
                _img = null;
            }
        }

        /// <summary>
        /// 指定ピクセルの色情報を取得する
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>色情報</returns>
        public Color GetPixel(int x, int y)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のGetPixelを使用
                return _bmp.GetPixel(x, y);
            }
            unsafe
            {
                // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセスする
                byte* adr = (byte*)_img.Scan0;
                int pos = x * 3 + _img.Stride * y;
                byte b = adr[pos + 0];
                byte g = adr[pos + 1];
                byte r = adr[pos + 2];
                return Color.FromArgb(r, g, b);
            }
        }

        /// <summary>
        /// 指定ピクセルの色を設定する
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <param name="col">色</param>
        public void SetPixel(int x, int y, Color col)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のSetPixelを使用
                _bmp.SetPixel(x, y, col);
                return;
            }
            unsafe
            {
                // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセスする
                byte* adr = (byte*)_img.Scan0;
                int pos = x * 3 + _img.Stride * y;
                adr[pos + 0] = col.B;
                adr[pos + 1] = col.G;
                adr[pos + 2] = col.R;
            }
        }
    }
}
