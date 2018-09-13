using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Design.Styles.Controls
{
    public partial class TextBoxInPlace
    {
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox EditableBox = sender as TextBox;
                EditableBox.IsReadOnly = false;
                EditableBox.Focus();
            }
        }
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)(sender as TextBox).IsReadOnly = true;
        }
        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && e.Key == Key.Enter && sender is TextBox)
                (sender as TextBox).IsReadOnly = true;
        }
    }
}