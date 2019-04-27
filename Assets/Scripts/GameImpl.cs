using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameImpl : Game
{

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
