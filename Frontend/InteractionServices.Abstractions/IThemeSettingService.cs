namespace InteractionServices.Abstractions;

public interface IThemeSettingService
{
    bool IsDarkTheme { get; }
    void ToggleThemeLightDark();
    void ApplyThemeLightDark(bool isDarkTheme);
}
