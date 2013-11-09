using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    public class PuyofuConfiguration
    {
        public bool DebugRectEnabled { get; set; }
        public int SimilarityThreshold { get; set; }
        public Rectangle CaptureRect { get; set; }
        public Rectangle NextRect { get; set; }

        public void Init()
        {
            DebugRectEnabled = ConfigurationManager.AppSettings["DebugRectEnabled"] == "1";
            SimilarityThreshold = int.Parse(ConfigurationManager.AppSettings["SimilarityThreshold"]);
            CaptureRect = StringToRectangle(ConfigurationManager.AppSettings["CaptureRect"]);
            NextRect = StringToRectangle(ConfigurationManager.AppSettings["NextRect"]);
        }

        public void Save()
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appConfig.AppSettings.Settings["DebugRectEnabled"].Value = DebugRectEnabled ? "1" : "0";
            appConfig.AppSettings.Settings["SimilarityThreshold"].Value = SimilarityThreshold.ToString();
            appConfig.AppSettings.Settings["CaptureRect"].Value = RectToSaveString(CaptureRect);
            appConfig.AppSettings.Settings["NextRect"].Value = RectToSaveString(NextRect);
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
