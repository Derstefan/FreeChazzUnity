public abstract class Animation
{
    public float duration;
    public float progressTime = 0f;

    public abstract void animate();

    public abstract void finish();
}
