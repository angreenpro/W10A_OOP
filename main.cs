using System.Text;
using SmartHomeMini.Abstracts;
using SmartHomeMini.Configuration;
using SmartHomeMini.Devices;
using SmartHomeMini.Facade;
using SmartHomeMini.Factories;

namespace SmartHomeMini;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        RunDemo();
        Console.WriteLine();
        RunVerificationTests();
    }

    private static void RunDemo()
    {
        Console.WriteLine("=== Smart Home Mini Demo ===");

        // Singleton: kiểm tra cấu hình chung chỉ có một thể hiện duy nhất
        HomeConfiguration config1 = HomeConfiguration.Instance;
        HomeConfiguration config2 = HomeConfiguration.Instance;
        if (!ReferenceEquals(config1, config2))
        {
            throw new InvalidOperationException("Singleton implementation is incorrect.");
        }

        Console.WriteLine($"Home: {config1.HomeName}");
        Console.WriteLine($"Singleton ReferenceEquals: {ReferenceEquals(config1, config2)}");

        // Factory: tạo thiết bị qua abstraction IDevice
        IDevice light = DeviceFactory.Create("light", "Living room light");
        IDevice fan = DeviceFactory.Create("fan", "Living room fan");
        IDevice door = DeviceFactory.Create("door", "Main door");

        if (door is not ILockable lockableDoor)
        {
            throw new InvalidOperationException("Door device must implement ILockable.");
        }

        // Điều khiển trực tiếp từng thiết bị qua abstraction khi cần
        Console.WriteLine("\n-- Direct device control --");
        light.TurnOn();
        fan.TurnOn();
        lockableDoor.Unlock();

        // Facade: điều khiển đồng loạt theo ngữ cảnh
        var facade = new SmartHomeFacade(light, fan, lockableDoor);

        Console.WriteLine("\n-- Facade: GoToWork() --");
        facade.GoToWork();

        Console.WriteLine("\n-- Facade: GoToSleep() --");
        facade.GoToSleep();
    }

    private static void RunVerificationTests()
    {
        Console.WriteLine("=== Verification Tests (T1 - T5) ===");

        VerifyT1FactoryCreatesExpectedTypes();
        VerifyT2FactoryThrowsOnUnsupportedType();
        VerifyT3SingletonReturnsSameInstance();
        VerifyT4FacadeGoToWork();
        VerifyT5FacadeGoToSleep();

        Console.WriteLine("All tests T1 - T5 PASSED.");
    }

    private static void VerifyT1FactoryCreatesExpectedTypes()
    {
        IDevice light = DeviceFactory.Create("Light", "Test light");
        IDevice fan = DeviceFactory.Create("  FAN  ", "Test fan");
        IDevice door = DeviceFactory.Create("door", "Test door");

        Assert(light is SmartLight, "T1 failed: expected SmartLight.");
        Assert(fan is SmartFan, "T1 failed: expected SmartFan.");
        Assert(door is SmartDoor, "T1 failed: expected SmartDoor.");

        Console.WriteLine("[PASS] T1: DeviceFactory creates SmartLight, SmartFan, SmartDoor (case-insensitive).");
    }

    private static void VerifyT2FactoryThrowsOnUnsupportedType()
    {
        bool threwArgumentException = false;
        try
        {
            DeviceFactory.Create("camera", "Front camera");
        }
        catch (ArgumentException)
        {
            threwArgumentException = true;
        }

        Assert(threwArgumentException, "T2 failed: expected ArgumentException for unsupported type 'camera'.");
        Console.WriteLine("[PASS] T2: DeviceFactory throws ArgumentException for unsupported type 'camera'.");
    }

    private static void VerifyT3SingletonReturnsSameInstance()
    {
        var c1 = HomeConfiguration.Instance;
        var c2 = HomeConfiguration.Instance;

        Assert(ReferenceEquals(c1, c2), "T3 failed: HomeConfiguration instances are not the same reference.");
        Console.WriteLine("[PASS] T3: HomeConfiguration.Instance returns a single shared instance.");
    }

    private static void VerifyT4FacadeGoToWork()
    {
        var light = (SmartLight)DeviceFactory.Create("light", "Living room light");
        var fan = (SmartFan)DeviceFactory.Create("fan", "Living room fan");
        var door = (SmartDoor)DeviceFactory.Create("door", "Main door");

        string output = CaptureConsoleOutput(() =>
        {
            light.TurnOn();
            fan.TurnOn();
            door.Unlock();

            var facade = new SmartHomeFacade(light, fan, door);
            facade.GoToWork();
        });

        Assert(!light.IsOn, "T4 failed: light should be OFF.");
        Assert(!fan.IsOn, "T4 failed: fan should be OFF.");
        Assert(door.IsLocked, "T4 failed: door should be LOCKED.");
        Assert(output.Contains("Living room light: OFF"), "T4 failed: missing light OFF log.");
        Assert(output.Contains("Living room fan: OFF"), "T4 failed: missing fan OFF log.");
        Assert(output.Contains("Main door: LOCKED"), "T4 failed: missing door LOCKED log.");

        Console.WriteLine("[PASS] T4: GoToWork() turns off light and fan, and locks door.");
    }

    private static void VerifyT5FacadeGoToSleep()
    {
        var light = (SmartLight)DeviceFactory.Create("light", "Night light");
        var fan = (SmartFan)DeviceFactory.Create("fan", "Bedroom fan");
        var door = (SmartDoor)DeviceFactory.Create("door", "Main door");

        string output = CaptureConsoleOutput(() =>
        {
            fan.TurnOn();
            door.Unlock();
            light.TurnOff();

            var facade = new SmartHomeFacade(light, fan, door);
            facade.GoToSleep();
        });

        int expectedLevel = HomeConfiguration.Instance.NightLightLevel;
        Assert(!fan.IsOn, "T5 failed: fan should be OFF.");
        Assert(door.IsLocked, "T5 failed: door should be LOCKED.");
        Assert(light.IsOn, "T5 failed: light should be ON.");
        Assert(output.Contains($"Night light level: {expectedLevel}%"),
            $"T5 failed: missing night light level {expectedLevel}% log.");

        Console.WriteLine($"[PASS] T5: GoToSleep() turns off fan, locks door, turns on light, and applies {expectedLevel}% brightness.");
    }

    private static string CaptureConsoleOutput(Action action)
    {
        TextWriter originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
