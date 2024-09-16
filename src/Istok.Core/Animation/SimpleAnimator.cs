using Istok.Core.PlayerLoopStages;

namespace Istok.Core.Animation;

public class SimpleAnimator : Component, IAnimatable
{
    public Clip? Animation { get; init; }
    public PlayMode Mode { get; init; }
    double? _previousTime;
    double _currentTime;
    bool _bound;
    float _duration;

    public enum PlayMode
    {
        HOLD,
        LOOP,
        PING_PONG,
    }

    protected override void OnConnect()
    {
        base.OnConnect();
        AnimationStage.Register(this);
        if (Animation != null)
            Bind();
    }

    protected override void OnDisconnect()
    {
        base.OnDisconnect();
        AnimationStage.Unregister(this);
    }


    public void Bind()
    {
        if (Animation == null)
            throw new Exception("Animation is null");

        if (SceneNode == null)
            throw new Exception("SceneNode is null");

        // _duration = Animation.Tracks.Max(track => track.Duration);
        _duration = 0;
        foreach (ITrack track in Animation.Tracks)
        {
            _duration = Math.Max(track.Duration, _duration);
            track.Bind(SceneNode);
        }

        _bound = true;
    }

    public void Animate(double time)
    {
        if (Animation == null || !_bound)
            return;

        _previousTime ??= time;

        _currentTime += time - _previousTime.Value;
        double t = Mode switch
        {
            PlayMode.LOOP => _currentTime % _duration,
            PlayMode.PING_PONG => Math.Abs((_currentTime + _duration) % (_duration * 2.0) - _duration),
            PlayMode.HOLD => Math.Clamp(_currentTime, 0, _duration),
            _ => Math.Clamp(_currentTime, 0, _duration),
        };
        _previousTime = time;

        Animation.Evaluate((float)t);
    }
}
