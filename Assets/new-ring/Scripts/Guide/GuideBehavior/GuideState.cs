public abstract class GuideState
{
    protected GuideController Controller;
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}