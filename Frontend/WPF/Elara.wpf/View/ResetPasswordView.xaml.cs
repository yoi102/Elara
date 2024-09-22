using Resources.Strings;
using System.Windows.Controls;

namespace Elara.wpf.View;

/// <summary>
/// ResetPasswordView.xaml 的交互逻辑
/// </summary>
public partial class ResetPasswordView : UserControl
{
    public ResetPasswordView()
    {
        InitializeComponent();
    }

    private CancellationTokenSource? countdownCts;

    private async void Countdown_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (sender is not Border border)
            return;

        if (!border.IsVisible)
        {
            countdownCts?.Cancel();
            return;
        }
        if (border.Child is not TextBlock textBlock)
            return;
        countdownCts?.Cancel();
        countdownCts = new CancellationTokenSource();
        var token = countdownCts.Token;

        var message = Strings.ResendInSeconds;

        try
        {
            for (int i = 60; i >= 0; i--)
            {
                textBlock.Text = $"{message}({i})";
                await Task.Delay(1000, token); // 支持取消
            }

            textBlock.Text = "Resend";
            border.Visibility = System.Windows.Visibility.Collapsed;
        }
        catch (TaskCanceledException)
        {
            // 被取消时不做处理即可
        }
    }
}
