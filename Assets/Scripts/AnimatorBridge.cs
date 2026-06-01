using UnityEngine;
public class AnimatorBridge : MonoBehaviour
{
    public Animator source;  // PlayerArmature's Animator
    public Animator target;  // moth's Animator

    void Update()
    {
        target.SetFloat("Speed", source.GetFloat("Speed"));
    }
}
