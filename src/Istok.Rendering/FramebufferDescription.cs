namespace Istok.Rendering;

public readonly record struct FramebufferDescription(ImageView? DepthTarget, params ImageView[] ColorTargets);
