﻿using System.Configuration;
using System.Drawing;

namespace Cubokta.Puyo
{
    public class PuyofuConfiguration
    {
        public bool DebugRectEnabled { get; set; }
        public int SimilarityThreshold { get; set; }
        public Rectangle CaptureRect { get; set; }
        public int RecordId { get; set; }
        public string RecordDate { get; set; }
        public string PlayerName1 { get; set; }
        public string PlayerName2 { get; set; }
        public string TargetField { get; set; }

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
        }

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
            appConfig.Save();
        }

        private string RectToSaveString(Rectangle r)
        {
            return r.Left + "," + r.Top + "," + r.Width + "," + r.Height;
        }

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
