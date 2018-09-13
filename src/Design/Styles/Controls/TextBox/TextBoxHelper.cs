using System.Windows.Controls;
using System.Windows;

namespace Design.Styles.Controls
{
    public partial class TextBoxHelper
    {
        private void TextClear(object sender, RoutedEventArgs e)
        {            
            if (sender is Button)
            {
                Button button = sender as Button;
                button.Tag = "";
            }
        }
    }
}