using InteractionServices.Abstractions;
using MaterialDesignThemes.Wpf;

namespace Elara.wpf.Services;

internal class ThemeSettingService : IThemeSettingService
{
    public void ChangeThemeLightDark()
    {
        var paletteHelper = new PaletteHelper();
        Theme theme = paletteHelper.GetTheme();
        var currentBaseTheme = theme.GetBaseTheme();
        if (currentBaseTheme != BaseTheme.Dark)
        {
            theme.SetDarkTheme();
        }
        else
        {
            theme.SetLightTheme();
        }
        paletteHelper.SetTheme(theme);

    }
}
