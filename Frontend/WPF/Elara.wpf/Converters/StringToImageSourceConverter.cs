using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Elara.wpf.Converters;

public class StringToImageSourceConverter : IValueConverter
{
    public static readonly StringToImageSourceConverter Instance = new();

    private static readonly BitmapImage DefaultImage = new(new Uri("Assets/NoImage.png", UriKind.Relative));

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string path || string.IsNullOrWhiteSpace(path))
            return DefaultImage;

        try
        {
            // 网络图片
            if (Uri.TryCreate(path, UriKind.Absolute, out Uri? uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return new BitmapImage(uri);
            }

            // 本地绝对路径
            if (Path.IsPathRooted(path) && File.Exists(path))
            {
                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }

            // 尝试加载相对路径（相对当前目录或支持 pack URI）
            string fullPath = Path.GetFullPath(path);
            if (File.Exists(fullPath))
            {
                return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }

            // 支持 Pack URI 格式
            if (path.StartsWith("pack://", StringComparison.OrdinalIgnoreCase))
            {
                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }
        }
        catch
        {
            // 可以记录日志
        }

        return DefaultImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
