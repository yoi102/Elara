namespace InteractionServices.Abstractions;

public interface ICultureSettingService
{
    void ChangeCulture(string language);

    void ChangeCulture(int lcid);
}
