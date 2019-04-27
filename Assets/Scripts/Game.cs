﻿using UnityEngine;
using System.Collections;


using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

namespace Input
{
	enum Type
	{
		None = 0x0,
		Down = 0x1,
		Up = 0x2
	};

	enum Action
	{
		Action1 = 0,
		Action2 = 1
	}

	struct ActionEntry
	{
		private Action action;
		private KeyCode code;
		private string name;
		private Type type;
		public Action Action
		{
			get { return action; }
			set { }
		}
		public KeyCode Code
		{
			get { return code; } set { }
		}
		public string Name
		{
			get { return name; }
			set { }
		}
		public Type Type
		{
			get { return type; }
			set { type = value; }
		}
		public ActionEntry(Action a, KeyCode c, string s, Type t)
		{
			action = a; code = c; name = s; type = t;
		}
	};

	static class Mapper
	{
		public static ActionEntry[] Actions =
		{
			new ActionEntry(Action.Action1, KeyCode.X, "Action", Type.None),
			new ActionEntry(Action.Action2, KeyCode.C, "Action2", Type.None)
		};

		public static Type IsPressed(Action a)
		{
			return Actions[(int)a].Type;
		}
	}
}

public class Game : MonoBehaviour
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

	public virtual void UpdateGame()
	{
		switch (m_state)
		{
			case GameState.MENU:
				{
					if (UnityEngine.Input.anyKey) // special case, start
					{
						m_state = GameState.PLAYING;
					}
					break;
				}
			case GameState.PLAYINGNOTIMER:
			case GameState.PLAYING:
				{
					if ((Input.Mapper.IsPressed(Input.Action.Action1)&Input.Type.Up)==Input.Type.Up)
					{
					}
					break;
				}
			case GameState.END:
				{
					if(UnityEngine.Input.anyKey) // special case, restart
					{
						m_state = GameState.START;
					}
				}
			break;
		}
	}

	public virtual void CaptureKeyboard()
	{
		for (int index = 0; index < Input.Mapper.Actions.Length; index++)
		{
			Input.ActionEntry input = Input.Mapper.Actions[index];
			input.Type = Input.Type.None;
			if(UnityEngine.Input.GetKeyDown(input.Code))
			{
				input.Type |= Input.Type.Down;
			}
			if (UnityEngine.Input.GetKeyUp(input.Code))
			{
				input.Type |= Input.Type.Up;
			}
			Input.Mapper.Actions[index] = input;
		}
	}
}