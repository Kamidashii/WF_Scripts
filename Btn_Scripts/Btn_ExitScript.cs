using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Btn_ExitScript : MonoBehaviour {

    public GameObject gameController;

    [SerializeField]
    private Text hintText;
    public string stopHint;

    public Button exitToMenuBtn;
    public Button stopBtn;

    public bool stop = false;
    
	void Start ()
    {
        exitToMenuBtn.onClick.AddListener(delegate { StartCoroutine(MenuLoader()); });
        stopBtn.onClick.AddListener(delegate {Stop();});
    }
	
    
	void Update ()
    {
		if(!stop)
        {
            if (string.Compare(hintText.text, stopHint, true) == 0)
            {
                hintText.text = "";
            }
        }
	}

    public IEnumerator MenuLoader()
    {

        yield return gameController.GetComponent<GameControllerScript>().SaveGame();

        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        yield return null;
    }

    public void Stop()
    {
        if(!stop)
        {
            Time.timeScale = 0;
            stop = true;
        }
        else
        {
            Time.timeScale = 1;
            stop = false;
        }
    }




}
