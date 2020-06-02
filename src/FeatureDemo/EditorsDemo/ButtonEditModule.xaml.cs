using FeatureDemo.Common;
using Microsoft.UI.Xaml.Controls;

namespace EditorsDemo {
    public sealed partial class ButtonEditModule : UserControl {
        public ButtonEditModule() {
            this.InitializeComponent();
        }
        int value = 0;
        private void Increase(object sender, System.EventArgs e) {
            RepeatTextEdit.Text = (++value).ToString();
        }

        private void Decrease(object sender, System.EventArgs e) {
            RepeatTextEdit.Text = (--value).ToString();
        }

        private void ButtonInfo_Click(object sender, System.EventArgs e) {

        }
    }
}
