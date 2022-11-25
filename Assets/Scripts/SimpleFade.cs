using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SimpleFade : MonoBehaviour
{
   
    public LevelManager LevelManager;
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;

    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }
    

 
    void OnEnable()
    {
        StartCoroutine(Fade(FadeDirection.Out));
LevelManager = gameObject.GetComponent<LevelManager>();
       
    }
    

    
    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    

    
    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection,  bool win, bool exhausted)
    {
        yield return Fade(fadeDirection);
       // SceneManager.LoadScene(sceneToLoad);
        if(win == true && exhausted == false)
        {
            GameManager.instance.LoadResultsScene(true, LevelManager.GetLevel(), false);
        }
        if(win == false && exhausted == true)
        {
            GameManager.instance.LoadResultsScene(false, LevelManager.GetLevel(), true);
        }
        if(win == false && exhausted == false)
        {
            GameManager.instance.LoadResultsScene(false, LevelManager.GetLevel(), false);
        }
    }

    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }
    
}
