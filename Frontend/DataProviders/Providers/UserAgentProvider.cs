using DataProviders.Abstractions;
using System.Runtime.InteropServices;

namespace DataProviders.Providers;

public class UserAgentProvider : IUserAgentProvider
{
    public string GetUserAgent()
    {
        string os = RuntimeInformation.OSDescription;
        string machineName = Environment.MachineName;                  // 设备名
        string framework = RuntimeInformation.FrameworkDescription; // .NET 版本
        string arch = RuntimeInformation.OSArchitecture.ToString(); // x64/x86/Arm

        string userAgent = $"Elara/1.0 ({os}; {arch}; {framework}; {machineName})";
        return userAgent;
    }
}
