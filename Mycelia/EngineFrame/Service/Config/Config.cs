using System;

namespace Mycelia;

internal static class Config
{
    private static IConfigService? _config;
    
    internal static void InitTables(IConfigService configService)
    {
        _config = configService ?? throw new ArgumentNullException(nameof(configService));
    }
    
    internal static UnitConfig GetUnitConfig(int id)
    {
        return _config?.GetUnit(id) ?? throw new Exception("在使用配置文件之前，必须先调用 MC.InitTables 方法.");
    }
    
    internal static CardConfig GetCardConfig(int id)
    {
        return _config?.GetCard(id) ?? throw new Exception("在使用配置文件之前，必须先调用 MC.InitTables 方法.");
    }
}