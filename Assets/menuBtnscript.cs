using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuBtnscript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenSubPanel(GameObject Panel)
    {
        Panel.SetActive(true);
        gameObject.SetActive(false);
            
    }
    public void forceBtnSelect(Button Boton)
    {
        Boton.Select();
    }

    public void DisablePanel(GameObject panelPadre)
    {
        panelPadre.SetActive(false);
    }

    public void getout()
    {
        Application.Quit();
    }



}
