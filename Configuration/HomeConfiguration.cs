namespace SmartHomeMini.Configuration;

public sealed class HomeConfiguration
{
    private static readonly Lazy<HomeConfiguration> lazy =
        new(() => new HomeConfiguration());

    public static HomeConfiguration Instance => lazy.Value;

    public string HomeName { get; set; } = "My Home";

    public int NightLightLevel { get; set; } = 30;

    // Ngăn khởi tạo trực tiếp từ bên ngoài để đảm bảo chỉ tồn tại một thể hiện.
    private HomeConfiguration()
    {
    }
}
