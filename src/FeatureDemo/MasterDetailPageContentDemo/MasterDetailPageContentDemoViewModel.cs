using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.Data;
using System;
using System.Collections.Generic;
using Color = Windows.UI.Color;

namespace MasterDetailPageContentDemo {
    public class MasterDetailPageContentDemoViewModel : ViewModelBase {
        public MasterDetailPageContentSplitterMode SplitterMode {
            get { return GetProperty<MasterDetailPageContentSplitterMode>(); }
            set { SetProperty(value); }
        }
        public bool EnableSplitter {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public List<MessageViewModel> Messages { get; } = new List<MessageViewModel>();
        public MessageViewModel SelectedMessage {
            get { return GetProperty<MessageViewModel>(); }
            set { SetProperty(value); }
        }
        public MasterDetailPageContentDemoViewModel() {
            SplitterMode = MasterDetailPageContentSplitterMode.OverlayVisible;
            EnableSplitter = true;
            Dictionary<string, Color> colors = new Dictionary<string, Color>();
            Random rnd = new Random();
            foreach(Message message in DataStorage.Messages) {
                Color color;
                if(colors.ContainsKey(message.From)) {
                    color = colors[message.From];
                } else {
                    byte[] rgb = new byte[3];
                    rnd.NextBytes(rgb);
                    color = Color.FromArgb(0xFF, rgb[0], rgb[1], rgb[2]);
                    colors.Add(message.From, color);
                }
                Messages.Add(new MessageViewModel(message, color));
            }
            SelectedMessage = Messages[0];
        } 
    }
    public class MessageViewModel  {
        public Color Color { get; set; }
        public string Initials { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public MessageViewModel(Message source, Color color) {
            Date = source.Date.ToString("d");
            From = source.From;
            Subject = source.Subject;
            string[] nameParts = From.Split(' ');
            if(nameParts.Length == 1 || nameParts.Length > 1 && !char.IsLetter(nameParts[1][0]))
                Initials = nameParts[0][0].ToString();
            else
                Initials = nameParts[0][0].ToString() + nameParts[1][0];
            Color = color;
            Text = ParseToRtf(source.Text, source.Date.ToString());
        }
        string ParseToRtf(string text, string contentDate) {
            string prefix = "<html><body style='font-family:Segoe UI'><table><tr><td rowspan = '2'><svg xmlns='http://www.w3.org/2000/svg' width='40' height='40'><g><circle  x='0' y='0' cx='20' cy='20' r='20' fill='{0}' /><text x='19' y='26' text-anchor='middle' font-family='Segoe UI' font-size='18' fill='white'>{1}</text></g></svg></td><td style='font-family:Segoe UI;font-size:18px;text-indent:10px'>{2}</td></tr><tr><td style='font-family:Segoe UI;font-size:14px;text-indent:10px;color:#737373'>{3}</td></tr></table><p><b>{4}</b></p>";
            return string.Format(prefix, "#" + Color.ToString().Substring(3), Initials, From, contentDate, Subject) + text + "</body></html>";
        }
    }
}