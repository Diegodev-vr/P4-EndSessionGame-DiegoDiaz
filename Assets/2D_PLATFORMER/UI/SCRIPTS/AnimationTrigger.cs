using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    ///// this script is meant to be called from an animation event in the time line,
    ///// to trigger a one-time action at the end of the sprite animation
    [SerializeField] private GameObject targetObject;

    public void ActivateObject()
    {
        ///// this will activate the main menu buttons at the end of the opening animation
        targetObject.SetActive(true);
    }
}