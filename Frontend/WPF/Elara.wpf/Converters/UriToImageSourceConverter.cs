using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Elara.wpf.Converters;

public class UriToImageSourceConverter : IValueConverter
{
    public static readonly UriToImageSourceConverter Instance = new();

    private static readonly BitmapImage DefaultImage = new(new Uri("Assets/NoImage.png", UriKind.Relative));

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Uri uri)
            return DefaultImage;

        try
        {
            // 网络图片
            if (uri.IsAbsoluteUri &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return new BitmapImage(uri);
            }

            // 本地绝对路径（file://）
            if (uri.IsAbsoluteUri && uri.Scheme == Uri.UriSchemeFile)
            {
                if (File.Exists(uri.LocalPath))
                    return new BitmapImage(uri);
            }

            // Pack URI 格式
            if (uri.IsAbsoluteUri && uri.Scheme == "pack")
            {
                return new BitmapImage(uri);
            }

            // 相对路径处理（作为 pack:// 或转为绝对路径）
            if (!uri.IsAbsoluteUri)
            {
                string fullPath = Path.GetFullPath(uri.OriginalString);
                if (File.Exists(fullPath))
                    return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
        }
        catch
        {
            // 可选：记录异常日志
        }

        return DefaultImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }

}
