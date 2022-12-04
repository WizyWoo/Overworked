using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phone_display : MonoBehaviour
{

    public bool is_paused;
    public KeyCode pause_menu_key;
    
    public GameObject home, settings, work;
    
    public Dictionary<applications, GameObject> active_app = new Dictionary<applications, GameObject>();
    

    public Animator animator;


    public void Start()
    {
        active_app.Add(applications.home    , home);
        active_app.Add(applications.settings, settings);
        active_app.Add(applications.work    , work);
    }

    //private void OnEnable() =>  open_phone();
    

    void Update()
    {
        if (Input.GetKeyDown(pause_menu_key))
        {
            is_paused = !is_paused;
        }
        if (is_paused == true)
        {
            Time.timeScale = 0f;
            animator.Play("display");
        }
        if (is_paused == false)
        {
            Time.timeScale = 1;
            animator.Play("close_phone");
        }

        float i;

        i = Time.unscaledDeltaTime;
        animator.Update(i);
    }

   



    public void settings_menu() =>   open_application(applications.settings);
    public void Work_menu()     =>   open_application(applications.work);
    public void Home_menu()     =>   open_application(applications.home);
       
    

    public void open_application(applications app)
    {
        foreach(var App in active_app)
            App.Value.SetActive(false);
        
        active_app[app].SetActive(true);
    }

    public void QuitGame()      => Application.Quit();
    public void ResumeGame()    => is_paused = false;

    public void RestartScene()  => SceneManager.LoadScene(Current_scene());
    public int Current_scene()  => SceneManager.GetActiveScene().buildIndex;

    public void Go_home()       => SceneManager.LoadScene("MainMenu");

}
public enum applications
{
    home,
    settings,
    work
}