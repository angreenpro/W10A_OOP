using SmartHomeMini.Abstracts;
using SmartHomeMini.Devices;

namespace SmartHomeMini.Factories;

public static class DeviceFactory
{
   
    public static IDevice Create(string type, string name)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException($"Unsupported device type: {type}");
        }

        return type.Trim().ToLowerInvariant() switch
        {
            "light" => new SmartLight(name),
            "fan"   => new SmartFan(name),
            "door"  => new SmartDoor(name),
            _ => throw new ArgumentException(
                $"Unsupported device type: {type}")
        };
    }
}
