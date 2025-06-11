using System.Windows.Controls;

namespace Elara.wpf.View.Dialogs;

/// <summary>
/// MessageDialog.xaml 的交互逻辑
/// </summary>
public partial class MessageDialog : UserControl
{
    public MessageDialog(string header,string message)
    {
        InitializeComponent();
        HeaderTextBlock.Text = header;
        MessageTextBlock.Text = message;
    }
}
