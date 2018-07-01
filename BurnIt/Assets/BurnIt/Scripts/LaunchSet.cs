using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchSet : MonoBehaviour {

    // Use this for initialization
   // public Text progressText;
	void Start () {
        transform.position = Vector3.zero;
        StartCoroutine(Load());

    }
    public Image Bar;
    IEnumerator Load()
    {
      
        var scao = SceneManager.LoadSceneAsync(1);
        scao.allowSceneActivation = false;
        float s = 0f;
        while (scao.progress < 0.8f)
        {
            s += 0.05f;
            s = Mathf.Min(0.9f, s);
            
            UpdateProgress(s);
            yield return null;
        }

        while (s < 0.9f)
        {
            s += 0.1f;
            s = Mathf.Min(1f, s);

            UpdateProgress(s);
            yield return null;
        }
        scao.allowSceneActivation = true;
        UpdateProgress(s);

    }

    void UpdateProgress(float t)
    {
        Bar.fillAmount = t;
        
        //progressText.text = "Loading..." + (t * 100f).ToString("0") + "%";
    }
	

}
