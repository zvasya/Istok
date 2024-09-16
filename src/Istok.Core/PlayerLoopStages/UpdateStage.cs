using Istok.Core.PlayerLoop;

namespace Istok.Core.PlayerLoopStages;

public interface IUpdateble
{
    void Update(double time);
}
public class UpdateStage : PlayerLoopStage<IUpdateble>
{
    readonly DateTime _dateTime = DateTime.UtcNow;

    double _time;
    protected override void Invoke(IUpdateble subscriber)
    {
        subscriber.Update(_time);
    }

    public override void Execute()
    {
        _time = (DateTime.UtcNow - _dateTime).TotalMilliseconds;
        base.Execute();
    }
}
