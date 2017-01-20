using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	public GameObject pausable;
	public Canvas pauseCanvas;
    public Canvas deadCanvas;
    public Character chara;

    private bool isPaused = false;
	private Animator pauseAnim;
    private Animator deadAnim;
    private Component[] pausableInterfaces;
	private Component[] quittableInterfaces;

	void Start() 
	{
		// PauseManager requires the EventSystem - make sure there is one
		if (FindObjectOfType<EventSystem>() == null)
		{
			var es = new GameObject("EventSystem", typeof(EventSystem));
			es.AddComponent<StandaloneInputModule>();
		}

		pausableInterfaces = pausable.GetComponents (typeof(IPausable));
		quittableInterfaces = pausable.GetComponents (typeof(IQuittable));
		pauseAnim = pauseCanvas.GetComponent<Animator> ();
        deadAnim = deadCanvas.GetComponent<Animator>();


        pauseCanvas.enabled = false;
        deadCanvas.enabled = false;
    }
	
	void Update () {
        if (chara.currentHealth <= 0)
        {
            Dead();
            deadCanvas.enabled = true;
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if( isPaused ) {
				OnUnPause();
			} else {
				OnPause();
			}
		}

		pauseCanvas.enabled = isPaused;
		pauseAnim.SetBool( "IsPaused", isPaused );
	}
		
	public void OnQuit() {
		Debug.Log ("PauseManager.OnQuit");

		foreach (var quittableComponent in quittableInterfaces) {		
			IQuittable quittableInterface = (IQuittable)quittableComponent;
			if( quittableInterface != null )
				quittableInterface.OnQuit ();
		}		
	}
	
	public void OnUnPause() {
		isPaused = false;

		foreach (var pausableComponent in pausableInterfaces) {		
			IPausable pausableInterface = (IPausable)pausableComponent;
			if( pausableInterface != null )
				pausableInterface.OnUnPause ();
		}
	}

	public void OnPause() {
		isPaused = true;
        foreach (var pausableComponent in pausableInterfaces) {		
			IPausable pausableInterface = (IPausable)pausableComponent;
			if( pausableInterface != null )
				pausableInterface.OnPause ();
		}
	}

    public void Dead()
    {
        chara.gameObject.SetActive(false);
    }

    public void onRestart()
    {
        chara.gameObject.SetActive(true);
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }
}
