namespace Istok.Core.PlayerLoop;

public class PlayerLoop
{
    readonly List<IPlayerLoopStage> _list = new();

    public void Run()
    {
        foreach (var stage in _list)
        {
            stage.Execute();
        }
    }

    public void Add(IPlayerLoopStage stage)
    {
        _list.Add(stage);
    }

    public void Remove(IPlayerLoopStage stage)
    {
        _list.Remove(stage);
    }
}
