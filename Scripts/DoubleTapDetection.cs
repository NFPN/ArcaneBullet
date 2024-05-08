using Godot;

public partial class DoubleTapDetection : Node
{
    private const float DoubleTapInterval = 0.5f;
    private bool isWaitingSecondTap = false;
    private float lastTapTime = 0;

    public bool HasDoubleTapped(bool tapped)
    {
        if (tapped)
        {
            float currentTime = Time.GetTicksMsec() / 1000.0f;

            if (isWaitingSecondTap && currentTime - lastTapTime <= DoubleTapInterval)
            {
                isWaitingSecondTap = false;
                return true;
            }
            else
                isWaitingSecondTap = true;

            lastTapTime = currentTime;
        }
        return false;
    }
}
