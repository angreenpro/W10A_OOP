using SmartHomeMini.Abstracts;

namespace SmartHomeMini.Devices;

public class SmartDoor : IDevice, ILockable
{
    public string Name { get; }
    public bool IsOn { get; private set; }
    public bool IsLocked { get; private set; }

    public SmartDoor(string name)
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

    /// <inheritdoc />
    public void Lock()
    {
        IsLocked = true;
        Console.WriteLine($"{Name}: LOCKED");
    }

    /// <inheritdoc />
    public void Unlock()
    {
        IsLocked = false;
        Console.WriteLine($"{Name}: UNLOCKED");
    }
}
