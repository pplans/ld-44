using UnityEngine;
using System.Collections;


using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

public abstract class Game : MonoBehaviour
{
	public static Game instance = null;
	public enum GameState
	{
		START,
		MENU,
		PLAYING,
		PLAYINGNOTIMER,
		END
	};

	private GameState m_state;
	public GameObject m_UIMain;
	public GameObject m_UIGame;
	public GameObject m_UIGameOver;
	public GameObject m_CameraGame;
	public GameObject m_CameraUI;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
		InitGame();
	}

	public void InitGame()
	{
		m_state = GameState.MENU;
		m_CameraUI.tag = "MainCamera";
		m_CameraGame.tag = "Untagged";
		m_CameraUI.SetActive(true);
		m_CameraGame.SetActive(false);

		m_UIGameOver.SetActive(false);
		m_UIGame.SetActive(false);
		m_UIMain.SetActive(true);
	}

	public void StartGame()
	{
		if(m_state == GameState.MENU)
		{
			m_CameraGame.tag = "MainCamera";
			m_CameraUI.tag = "Untagged";
			m_CameraGame.SetActive(true);
			m_CameraUI.SetActive(false);

			m_UIGame.SetActive(true);
			m_UIMain.SetActive(false);
			m_UIGameOver.SetActive(false);
			// start the game here
			Debug.Log("StartGame");
			m_state = GameState.PLAYING;
		}
	}

	public void StartGameNoTimer()
	{
		if (m_state == GameState.MENU)
		{
			m_CameraGame.tag = "MainCamera";
			m_CameraUI.tag = "Untagged";
			m_CameraGame.SetActive(true);
			m_CameraUI.SetActive(false);

			m_UIGame.SetActive(true);
			m_UIMain.SetActive(false);
			m_UIGameOver.SetActive(false);
			// start the game here
			Debug.Log("StartGame");
			m_state = GameState.PLAYINGNOTIMER;
		}
	}

	public void EndGame()
	{
		if (m_state == GameState.PLAYING)
		{
			m_state = GameState.END;
			m_CameraUI.tag = "MainCamera";
			m_CameraGame.tag = "Untagged";
			m_CameraUI.SetActive(true);
			m_CameraGame.SetActive(false);

			m_UIGame.SetActive(false);
			m_UIMain.SetActive(false);
			m_UIGameOver.SetActive(true);
		}
	}

	void Update()
	{
		switch(m_state)
		{
			case GameState.MENU:
			{
				break;
			}
			case GameState.PLAYINGNOTIMER:
			case GameState.PLAYING:
			{
				CaptureKeyboard();
				UpdateGame();
				break;
			}
		}
	}

    public GameState GetState()
    {
        return m_state;
    }

	public void SetState(GameState state)
	{
		m_state = state;
	}

	public abstract void UpdateGame();

	public abstract void CaptureKeyboard();
}