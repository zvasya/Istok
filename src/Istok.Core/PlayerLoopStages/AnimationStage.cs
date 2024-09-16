using System.Diagnostics;
using Istok.Core.PlayerLoop;

namespace Istok.Core.PlayerLoopStages;

public interface IAnimatable
{
    void Animate(double time);
}
public class AnimationStage : PlayerLoopStage<IAnimatable>
{
    readonly Stopwatch _timer = Stopwatch.StartNew();
    double _time;
    protected override void Invoke(IAnimatable subscriber)
    {
        subscriber.Animate(_time);
    }

    public override void Execute()
    {
        _time = _timer.ElapsedTicks / (double)Stopwatch.Frequency;

        base.Execute();
    }
}
