namespace SmartHomeMini.Abstracts;

/// <summary>
/// Abstraction cho các thiết bị có khả năng khóa và mở khóa (ví dụ: cửa).
/// </summary>
public interface ILockable
{
    /// <summary>
    /// Khóa thiết bị.
    /// </summary>
    void Lock();

    /// <summary>
    /// Mở khóa thiết bị.
    /// </summary>
    void Unlock();
}
