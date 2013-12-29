/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using System.Configuration;
using System.Drawing;

namespace Cubokta.Puyo
{
    /// <summary>
    /// 設定情報
    /// </summary>
    public class PuyofuConfiguration
    {
        /// <summary>デバッグ枠を表示するかどうか</summary>
        public bool DebugRectEnabled { get; set; }

        /// <summary>閾値</summary>
        public int SimilarityThreshold { get; set; }

        /// <summary>スクリーンキャプチャ範囲</summary>
        public Rectangle CaptureRect { get; set; }

        /// <summary>レコードID</summary>
        public int RecordId { get; set; }

        /// <summary>レコード日付</summary>
        public string RecordDate { get; set; }

        /// <summary>プレイヤ名１</summary>
        public string PlayerName1 { get; set; }

        /// <summary>プレイヤ名２</summary>
        public string PlayerName2 { get; set; }

        /// <summary>処理対象フィールド値</summary>
        public string TargetField { get; set; }

        /// <summary>キャプチャする手数</summary>
        public int CaptureStepNum { get; set; }

        /// <summary>ツモのみキャプチャするかどうか</summary>
        public bool CaptureOnlyTsumo { get; set; }

        /// <summary>レコード出力用テンプレート</summary>
        public string RecordTemplate { get; set; }

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Init()
        {
            DebugRectEnabled = ConfigurationManager.AppSettings["DebugRectEnabled"] == "1";
            SimilarityThreshold = int.Parse(ConfigurationManager.AppSettings["SimilarityThreshold"]);
            CaptureRect = StringToRectangle(ConfigurationManager.AppSettings["CaptureRect"]);
            RecordId = int.Parse(ConfigurationManager.AppSettings["RecordId"]);
            RecordDate = ConfigurationManager.AppSettings["RecordDate"];
            PlayerName1 = ConfigurationManager.AppSettings["PlayerName1"];
            PlayerName2 = ConfigurationManager.AppSettings["PlayerName2"];
            TargetField = ConfigurationManager.AppSettings["TargetField"];
            CaptureStepNum = int.Parse(ConfigurationManager.AppSettings["CaptureStepNum"]);
            CaptureOnlyTsumo = ConfigurationManager.AppSettings["CaptureOnlyTsumo"] == "1";
            RecordTemplate = ConfigurationManager.AppSettings["RecordTemplate"];
        }

        /// <summary>
        /// 保存する
        /// </summary>
        public void Save()
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appConfig.AppSettings.Settings["DebugRectEnabled"].Value = DebugRectEnabled ? "1" : "0";
            appConfig.AppSettings.Settings["SimilarityThreshold"].Value = SimilarityThreshold.ToString();
            appConfig.AppSettings.Settings["CaptureRect"].Value = RectToSaveString(CaptureRect);
            appConfig.AppSettings.Settings["RecordId"].Value = RecordId.ToString();
            appConfig.AppSettings.Settings["RecordDate"].Value = RecordDate;
            appConfig.AppSettings.Settings["PlayerName1"].Value = PlayerName1;
            appConfig.AppSettings.Settings["PlayerName2"].Value = PlayerName2;
            appConfig.AppSettings.Settings["TargetField"].Value = TargetField;
            appConfig.AppSettings.Settings["CaptureStepNum"].Value = CaptureStepNum.ToString();
            appConfig.AppSettings.Settings["CaptureOnlyTsumo"].Value = CaptureOnlyTsumo ? "1" : "0";
            appConfig.AppSettings.Settings["RecordTemplate"].Value = RecordTemplate;
            appConfig.Save();
        }

        /// <summary>
        /// 領域を文字列に変換する
        /// </summary>
        /// <param name="r">領域</param>
        /// <returns>変換した文字列</returns>
        private string RectToSaveString(Rectangle r)
        {
            return r.Left + "," + r.Top + "," + r.Width + "," + r.Height;
        }

        /// <summary>
        /// 文字列を領域に変換する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>変換した領域</returns>
        private Rectangle StringToRectangle(string str)
        {
            string[] values = str.Split(',');
            
            Rectangle r = new Rectangle() {
                X = int.Parse(values[0]),
                Y = int.Parse(values[1]),
                Width = int.Parse(values[2]),
                Height = int.Parse(values[3]),
            };

            return r;
        }
    }
}
