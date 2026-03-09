namespace Mycelia;

public interface IConfigService
{
    UnitConfig GetUnit(int id);
    CardConfig GetCard(int id);
}