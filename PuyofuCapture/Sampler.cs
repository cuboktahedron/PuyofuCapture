using Cubokta.Puyo.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Cubokta.Puyo
{
    /// <summary>
    /// ぷよ画像サンプラ取得クラス
    /// </summary>
    class Sampler
    {
        /// <summary>サンプリング処理中かどうか</summary>
        public bool IsSampling { get; set; }

        /// <summary>現在サンプリング処理中のぷよ種別に対応する数値</summary>
        private int samplingTargetIndex;

        /// <summary>
        /// サンプリングを開始する
        /// </summary>
        public void Begin()
        {
            IsSampling = true;
            samplingTargetIndex = (int)PuyoType.NONE;
        }

        /// <summary>
        /// 説明文を取得する
        /// </summary>
        /// <returns>説明文</returns>
        public string GetText()
        {
            if (IsSampling)
            {
                string typeName = PuyoTypeUtil.GetTypeName((PuyoType)samplingTargetIndex);
                return "「" + typeName + "」のサンプルを選択してください。右クリックでスキップします。";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// サンプリング処理を進める
        /// </summary>
        public void Proceed()
        {
            samplingTargetIndex++;
            if (samplingTargetIndex > (int)PuyoType.MURASAKI)
            {
                IsSampling = false;
            }
        }

        /// <summary>
        /// サンプリング処理中のぷよ種別を取得する
        /// </summary>
        /// <returns></returns>
        public PuyoType GetSamplingType()
        {
            return (PuyoType)samplingTargetIndex;
        }

        /// <summary>
        /// サンプル画像を保存する
        /// </summary>
        /// <param name="cellBmp">保存するサンプル画像</param>
        public void SaveSample(Bitmap cellBmp)
        {
            Directory.CreateDirectory("img");
            cellBmp.Save("img/" + (PuyoType)samplingTargetIndex + ".bmp", ImageFormat.Bmp);
        }
    }
}
