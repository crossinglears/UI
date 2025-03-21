using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace CrossingLears
{
    
public class UI_Collapsible : MonoBehaviour
{
    [Tooltip("Higher value means faster animation")]
    [Min(0)]
    public float AnimationSpeed = 1;

    public RectTransform TargetRectTransform;
    public CollapsePosition collapsePosition = CollapsePosition.Top;

    public UnityEvent OnFullyCollapse;
    public UnityEvent OnFullyOpen;

    private Coroutine animationCoroutine;
    private bool willOpen;

    public enum CollapsePosition
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public void Trigger(bool willOpen)
    {
        if (TargetRectTransform == null) return;
        this.willOpen = willOpen;

        Vector2 targetPivot = TargetRectTransform.pivot;

        switch (collapsePosition)
        {
            case CollapsePosition.Left:
                targetPivot.x = willOpen ? 0 : 1;
                break;
            case CollapsePosition.Right:
                targetPivot.x = willOpen ? 1 : 0;
                break;
            case CollapsePosition.Top:
                targetPivot.y = willOpen ? 1 : 0;
                break;
            case CollapsePosition.Bottom:
                targetPivot.y = willOpen ? 0 : 1;
                break;
        }

        if (AnimationSpeed <= 0)
        {
            TargetRectTransform.pivot = targetPivot;
        }
        else
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(AnimatePivot(TargetRectTransform.pivot, targetPivot));
        }
    }

    private IEnumerator AnimatePivot(Vector2 startPivot, Vector2 targetPivot)
    {
        float distance = Vector2.Distance(startPivot, targetPivot);
        float duration = distance / AnimationSpeed;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            TargetRectTransform.pivot = Vector2.Lerp(startPivot, targetPivot, elapsedTime / duration);
            yield return null;
        }

        TargetRectTransform.pivot = targetPivot;

        (willOpen ? OnFullyOpen : OnFullyCollapse).Invoke();
    }
}

}