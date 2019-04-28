﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    enum Type
    {
        None = 0x0,
        Down = 0x1,
        Up = 0x2,
        Hold = 0x3
    };

    enum Action
    {
        Action1 = 0,
        Action2 = 1,
        Action3 = 2,
        Up = 3,
        Down = 4,
        Left = 5,
        Right = 6
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
            new ActionEntry(Action.Action2, KeyCode.C, "Action2", Type.None),
            new ActionEntry(Action.Action3, KeyCode.V, "Action3", Type.None),
            new ActionEntry(Action.Up, KeyCode.UpArrow, "Up", Type.None),
            new ActionEntry(Action.Down, KeyCode.DownArrow, "Down", Type.None),
            new ActionEntry(Action.Left, KeyCode.LeftArrow, "Left", Type.None),
            new ActionEntry(Action.Right, KeyCode.RightArrow, "Right", Type.None)
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
    public Player m_player;
    public PlayerController m_playerController;

    const float m_playerMovingSpeed = 5f; // per second

    Person m_personBeingEaten;
    float m_timeSincePersonIsBeingEaten = 0f;

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
                    if (Input.Mapper.IsPressed(Input.Action.Action1) == Input.Type.Hold)
                    {
                        //Debug.Log("Action 1");
                        PlayerEatPeople();
                    }
                    else
                    {
                        m_timeSincePersonIsBeingEaten = 0f;
                        m_player.isEatingPeople = false;
                    }
                    if (Input.Mapper.IsPressed(Input.Action.Action2) == Input.Type.Up & !m_player.isEatingPeople)
                    {
                        Debug.Log("Action 2");
                    }
                    else if (Input.Mapper.IsPressed(Input.Action.Action3) == Input.Type.Up & !m_player.isEatingPeople)
                    {
                        Debug.Log("Action 3");
                    }
                    else
                    {
                        m_playerController.Move();
                    }
                    /*
                    else if ((Input.Mapper.IsPressed(Input.Action.Down) == Input.Type.Hold) & (Input.Mapper.IsPressed(Input.Action.Left) == Input.Type.Hold) & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(-0.71f * m_playerMovingSpeed * Time.deltaTime, 0, -0.71f * m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Down-Left");
                    }
                    else if ((Input.Mapper.IsPressed(Input.Action.Down) == Input.Type.Hold) & (Input.Mapper.IsPressed(Input.Action.Right) == Input.Type.Hold) & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(0.71f * m_playerMovingSpeed * Time.deltaTime, 0, -0.71f * m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Down-Right");
                    }
                    else if ((Input.Mapper.IsPressed(Input.Action.Up) == Input.Type.Hold) & (Input.Mapper.IsPressed(Input.Action.Left) == Input.Type.Hold) & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(-0.71f * m_playerMovingSpeed * Time.deltaTime, 0, 0.71f * m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Up-Left");
                    }
                    else if ((Input.Mapper.IsPressed(Input.Action.Up) == Input.Type.Hold) & (Input.Mapper.IsPressed(Input.Action.Right) == Input.Type.Hold) & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(0.71f * m_playerMovingSpeed * Time.deltaTime, 0, 0.71f * m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Up-Right");
                    }
                    else if (Input.Mapper.IsPressed(Input.Action.Down) == Input.Type.Hold & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(0, 0, -m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Down");
                    }
                    else if (Input.Mapper.IsPressed(Input.Action.Up) == Input.Type.Hold & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(0, 0, m_playerMovingSpeed * Time.deltaTime);
                        //Debug.Log("Up");
                    }
                    else if (Input.Mapper.IsPressed(Input.Action.Left) == Input.Type.Hold & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(-m_playerMovingSpeed * Time.deltaTime, 0, 0);
                        //Debug.Log("Left");
                    }
                    else if (Input.Mapper.IsPressed(Input.Action.Right) == Input.Type.Hold & !m_player.isEatingPeople)
                    {
                        m_player.gameObject.transform.Translate(m_playerMovingSpeed * Time.deltaTime, 0, 0);
                        //Debug.Log("Right");
                    }
                    */
                    // update dudes
                    m_player.UpdatePlayer();

                    if (m_player.isAlive == false)
                    {
                        SetState(GameState.END);
                    }

                    foreach (Person p in m_targets)
                    {
                        p.UpdatePerson();
                    }
                    foreach (Person p in m_people)
                    {
                        p.UpdatePerson();
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

    void PlayerEatPeople()
    {
        Vector3 playerPosition = m_player.gameObject.transform.position;
        if (m_timeSincePersonIsBeingEaten == 0f)
        {
            foreach (var p in m_targets)
            {
                float dist = Vector3.Distance(playerPosition, p.transform.position);
                if (p.GetIsAlive() & dist <= m_player.m_distanceToEatPeople)
                {
                    m_timeSincePersonIsBeingEaten += Time.deltaTime;
                    m_player.isEatingPeople = true;
                    m_personBeingEaten = p;
                    break;
                }
            }
        }
        if (m_timeSincePersonIsBeingEaten == 0f)
        {
            foreach (var p in m_people)
            {
                float dist = Vector3.Distance(playerPosition, p.transform.position);
                if (p.GetIsAlive() & dist <= m_player.m_distanceToEatPeople)
                {
                    m_timeSincePersonIsBeingEaten += Time.deltaTime;
                    m_player.isEatingPeople = true;
                    m_personBeingEaten = p;
                    break;
                }
            }
        }

        if (m_timeSincePersonIsBeingEaten > 0f)
        {
            if (m_personBeingEaten.GetIsAlive()
            & Vector3.Distance(m_personBeingEaten.transform.position, playerPosition) <= m_player.m_distanceToEatPeople)
            {
                m_timeSincePersonIsBeingEaten += Time.deltaTime;
                if (m_timeSincePersonIsBeingEaten >= m_player.m_timeToEatOnePeople)
                {
                    m_player.Blood += m_personBeingEaten.m_blood;
                    m_personBeingEaten.Die();
                    m_timeSincePersonIsBeingEaten = 0f;
                }
            }
        }
        else
        {
            m_timeSincePersonIsBeingEaten = 0f;
            m_player.isEatingPeople = false;
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
            if (UnityEngine.Input.GetKey(input.Code))
            {
                input.Type |= Input.Type.Hold;
            }
            Input.Mapper.Actions[index] = input;
        }
    }
}
