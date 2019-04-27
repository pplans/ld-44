using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			get { return code; }
			set { }
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

public class GameImpl : Game
{

	public List<Person> m_targets;
	public List<Person> m_people;
	public Player		m_player;

	public override void UpdateGame()
	{
		switch (GetState())
		{
			case GameState.MENU:
				{
					if (UnityEngine.Input.anyKey) // special case, start
					{
						SetState(GameState.PLAYING);
					}
					break;
				}
			case GameState.PLAYINGNOTIMER:
			case GameState.PLAYING:
				{
					if ((Input.Mapper.IsPressed(Input.Action.Action1) & Input.Type.Up) == Input.Type.Up)
					{
					}
					// update dudes
					m_player.Update();
					foreach (Person p in m_targets)
					{
						p.Update();
					}
					foreach (Person p in m_people)
					{
						p.Update();
					}
					break;
				}
			case GameState.END:
				{
					if (UnityEngine.Input.anyKey) // special case, restart
					{
						SetState(GameState.START);
					}
				}
				break;
		}
	}

	public override void CaptureKeyboard()
	{
		for (int index = 0; index < Input.Mapper.Actions.Length; index++)
		{
			Input.ActionEntry input = Input.Mapper.Actions[index];
			input.Type = Input.Type.None;
			if (UnityEngine.Input.GetKeyDown(input.Code))
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
