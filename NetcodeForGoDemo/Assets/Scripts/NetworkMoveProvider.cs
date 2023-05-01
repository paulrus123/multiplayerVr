using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using Vector2 = UnityEngine.Vector2;

public class NetworkMoveProvider : DynamicMoveProvider
{
    public bool enableInputActions;

    protected override Vector2 ReadInput()
    {
        if (!enableInputActions) return Vector2.zero;

        return base.ReadInput();
    }
}
