using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //Track the animation component
    // track the animation clips for fade in/out
    //function that receive animation events
    //functions to play fade in/out animations


    [SerializeField]  private Animation _mainMenuAnimator;
    [SerializeField]  private AnimationClip _fadeOutAnimation;
    [SerializeField]  private AnimationClip _fadeInAnimation;


    public void OnFadeOutComplete()
    {
        Debug.LogWarning("Fade Out Complete");
    }

    public void OnFadeInComplete()
    {
        Debug.LogWarning("Fade In Complete");
        UIManager.Instance.SetDummyCameraActive(true); 
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();

    }

    public void FadeOut()
    {

        UIManager.Instance.SetDummyCameraActive(false);
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}
