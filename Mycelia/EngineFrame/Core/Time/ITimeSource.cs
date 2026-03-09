//时间源接口-这是“现实时间”的抽象，而不是逻辑时间

namespace Mycelia;

public interface ITimeSource
{
    double Now { get; }
}