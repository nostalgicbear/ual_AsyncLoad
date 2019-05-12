using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script controls the loading of a level asyncronously. When you load a scene asynchronously, it allows you to load the new scene "in the background" while still continuing your current scene. 
/// </summary>
public class LoadManager : MonoBehaviour
{

    public Image image; //This is the circle image I use
    public Text percentageComplete; //How much the load is complete 
    AsyncOperation operation; //This is object that contains information about the scene we want to load (Eg is it finished loading, or what % of the load has been completed)
    public bool loadAutomaticallyWhenReady; //If this is true, then the level will load automatically when it is ready. 
    public GameObject loadButton;


    public enum SCENE { //Will let us select a scene from a dropdown
        Level2,
        Level3
    };

    public SCENE sceneToLoad; //The scene we selected from the dropdown. The scene we will load. 

    [SerializeField]
    bool lowerFrameRate = false; //Just enable this if you want to purposely make the game run really slow so you can see the value increase 

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Load());
    }

    private void Update()
    {
        //This will lower the frame rate, causing the game to run slow, so you can see the circle increase more clearly
        if (lowerFrameRate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 1;
        }
    }

    IEnumerator Load()
    {
        
       operation = SceneManager.LoadSceneAsync(sceneToLoad.ToString());

        if(!loadAutomaticallyWhenReady) //if we do not want to load the level automatically when it is ready....
        {
            operation.allowSceneActivation = false; //When this is false, the new scene will not load automatically when its ready. If it is true, then the second the new scene is fully ready, it will open. 
        }
       

        //The image fill amount is set to 0 starting off. This loop will run each frame until the fill amount is full. It 
        while(image.fillAmount <= 0.9f) //if you dont want to use the image to display progress, change this to 'while(operation.progress <=0.9f)'
        {
            float progress =  Mathf.Clamp01(operation.progress / 0.9f); //Will explain this change further
            percentageComplete.text = (progress * 100).ToString(); //Displays the load progress (* by 100 as the value is 0-1)
            image.fillAmount = progress; //Set the image fillAmount to the progress amount 
            Debug.Log("Progress is : " + operation.progress);
            yield return null;
        }


        if(!loadAutomaticallyWhenReady) //If we dont load the new level automatically when ready, we display a button that the user can then press if they want to progress
        {
            loadButton.SetActive(true);
        }
        

        yield return null;
    }

    /// <summary>
    /// Sets allowSceneActivation to true, which will trigger the level to load at that moment
    /// </summary>
    public void LoadLevel()
    {
        operation.allowSceneActivation = true;
    }
}
