using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager: MonoBehaviour
{
    [SerializeField] public Animator transition;

    private const float TransitionTime = 1.5f;
    private static readonly int Start = Animator.StringToHash("start");

    public IEnumerator PlayTransition()
    {
        transition.SetTrigger(Start);

        yield return new WaitForSeconds(TransitionTime);
    }
}
