using SmartHomeMini.Abstracts;

namespace SmartHomeMini.Devices;

public class SmartFan : IDevice
{
    public string Name { get; }
    public bool IsOn { get; private set; }

    public SmartFan(string name)
    {
        Name = name;
    }

    /// <inheritdoc />
    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine($"{Name}: ON");
    }
    /// <inheritdoc />
    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine($"{Name}: OFF");
    }
}
