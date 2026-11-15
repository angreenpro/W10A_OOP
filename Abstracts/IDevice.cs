namespace SmartHomeMini.Abstracts;

/// <summary>
/// Abstraction chung cho mọi thiết bị thông minh có thể bật và tắt.
/// </summary>
public interface IDevice
{
    /// <summary>
    /// Bật thiết bị.
    /// </summary>
    void TurnOn();

    /// <summary>
    /// Tắt thiết bị.
    /// </summary>
    void TurnOff();
}
