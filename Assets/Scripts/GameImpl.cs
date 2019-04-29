using System.Collections;
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
	public LoadLevel loadLevel;

	public List<Person> m_targets;
	public List<Person> m_people;
    bool levelComplete = false;

    public Player m_player;
	public PlayerController m_playerController;

	const float m_playerMovingSpeed = 5f; // per second

    public float eatDepth;
    public const float eatConeAngle = 40;
    public const int eatRaysCount = 10;

    Person m_personBeingEaten;
	float m_timeSincePersonIsBeingEaten = 0f;

    Person m_personLocked;

	public override void UpdateGame()
	{
        m_player.isSuspicious = false;

        if (Input.Mapper.IsPressed(Input.Action.Action1) == Input.Type.Hold)
		{
			//Debug.Log("Action 1");
			PlayerEatPeople();
		}
		else
		{
            PlayerStopEatingPeople(); // bad vampire

            m_timeSincePersonIsBeingEaten = 0f;
			m_player.isEatingPeople = false;
		}
		if (Input.Mapper.IsPressed(Input.Action.Action2) == Input.Type.Down & !m_player.isEatingPeople)
		{
            m_player.PlayerBecomeInvisible();
		}
        if (Input.Mapper.IsPressed(Input.Action.Action2) == Input.Type.Up & !m_player.isEatingPeople)
        {
            m_player.PlayerBecomeVisible();
        }
        else if (Input.Mapper.IsPressed(Input.Action.Action3) == Input.Type.Up & !m_player.isEatingPeople)
		{
			Debug.Log("Action 3");
            PlayerKillFromDistance();
		}
        else if (Input.Mapper.IsPressed(Input.Action.Action3) == Input.Type.Hold & !m_player.isEatingPeople)
        {
            PlayerLockToKillFromDistance();
        }

        if (m_player.isEatingPeople)
        {
            m_playerController.canMove = false;
        }
        else
        {
            m_playerController.canMove = true;
        }

		// update dudes
		m_player.UpdatePlayer();

		if (m_player.isAlive == false)
		{
			loadLevel.LoadLevelX(0);
		}

        levelComplete = true;

        foreach (var p in m_targets)
        {
            if (p.GetIsAlive())
            {
                levelComplete = false; 
            }
        }

        if (levelComplete)
        {
            Debug.Log("Level Complete");
            loadLevel.LoadLevelX(1);
        }

		foreach (Person p in m_targets)
		{
			p.UpdatePerson();
		}
		foreach (Person p in m_people)
		{
			p.UpdatePerson();
		}
	}

    void PlayerStopEatingPeople()
    {
        if (m_personBeingEaten)
            m_personBeingEaten.StopBeingAttacked();
        m_player.m_animator.ResetTrigger("StartEating");
        m_player.m_animator.SetTrigger("StopEating");
    }

    void PlayerEatPeople()
	{
		Vector3 playerPosition = m_player.gameObject.transform.position;
        /*
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
        */

        if (m_timeSincePersonIsBeingEaten == 0f)
        {
            float angleStart = -eatConeAngle / 2;
            float angleStep = eatConeAngle / eatRaysCount;

            for (int r = 0; r <= eatRaysCount; ++r)
            {
                eatDepth = m_player.m_distanceToEatPeople;
                Ray ray = new Ray(m_player.transform.position + Vector3.up, Quaternion.AngleAxis(angleStart + r * angleStep, Vector3.up) * m_player.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, eatDepth, ~LayerMask.GetMask("Ignore Raycast")))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    Person p = hit.transform.gameObject.GetComponent<Person>();
                    if (p)
                    {
                        if (p.GetIsAlive())
                        {
                            if (m_personBeingEaten && m_personBeingEaten != p)
                                m_personBeingEaten.StopBeingAttacked();
                            p.StartBeingAttacked();

                            m_timeSincePersonIsBeingEaten += Time.deltaTime;
                            m_player.isEatingPeople = true;
                            m_personBeingEaten = p;
                            break;
                        }
                    }
                }
                else if (r == 0 || r == eatRaysCount)
                    Debug.DrawRay(ray.origin, ray.direction * eatDepth, Color.red);
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
                    m_player.isEatingPeople = false;
				}
			}
            else
            {
                if (m_personBeingEaten)
                    m_personBeingEaten.StopBeingAttacked();

                m_timeSincePersonIsBeingEaten = 0f;
                m_player.isEatingPeople = false;
            }
		}
		else
		{
            if (m_personBeingEaten)
                m_personBeingEaten.StopBeingAttacked();

            m_timeSincePersonIsBeingEaten = 0f;
			m_player.isEatingPeople = false;
		}

        if (m_player.isEatingPeople)
        {
            m_player.m_animator.ResetTrigger("StopEating");
            m_player.m_animator.SetTrigger("StartEating");
            m_player.isSuspicious = true;
        }
        else
        {
            m_player.m_animator.ResetTrigger("StartEating");
            m_player.m_animator.SetTrigger("StopEating");
        }
    }

    void PlayerKillFromDistance()
    {
        if (m_personLocked != null)
        {
            m_personLocked.UnLock();
            m_personLocked.Die();
            m_personLocked = null;
            m_player.Blood = m_player.Blood - m_player.m_bloodSpentToKillFromDistance;
            m_player.isSuspicious = true;
            m_player.m_animator.SetTrigger("StartRemoteAttack");
        }  
    }

    void PlayerLockToKillFromDistance()
    {
        float angleStart = -eatConeAngle / 2;
        float angleStep = eatConeAngle / eatRaysCount;

        bool thereIsSomeoneToLock = false;

        for (int r = 0; r <= eatRaysCount; ++r)
        {
            eatDepth = m_player.m_distanceToKillFromDistance;
            Ray ray = new Ray(m_player.transform.position + Vector3.up, Quaternion.AngleAxis(angleStart + r * angleStep, Vector3.up) * m_player.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, eatDepth, ~LayerMask.GetMask("Ignore Raycast")))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                Person p = hit.transform.gameObject.GetComponent<Person>();
                if (p)
                {
                    if (p.GetIsAlive())
                    {
                        if (m_personLocked != null)
                        {
                            if (p.GetInstanceID() != m_personLocked.GetInstanceID())
                            {
                                m_personLocked.UnLock();
                                p.Lock();
                                m_personLocked = p;
                                
                            }
                        }
                        else
                        {
                            p.Lock();
                            m_personLocked = p;
                        }
                        thereIsSomeoneToLock = true;
                        break;
                    }
                }
            }
            else if (r == 0 || r == eatRaysCount)
                Debug.DrawRay(ray.origin, ray.direction * eatDepth, Color.red);
        }

        if (thereIsSomeoneToLock == false)
        {
            if (m_personLocked != null)
            {
                m_personLocked.UnLock();
                m_personLocked = null;
            }
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
            else if (UnityEngine.Input.GetKey(input.Code))
            {
                input.Type |= Input.Type.Hold;
            }
            else if (UnityEngine.Input.GetKeyUp(input.Code))
			{
				input.Type |= Input.Type.Up;
			}
			Input.Mapper.Actions[index] = input;
		}
	}
}
