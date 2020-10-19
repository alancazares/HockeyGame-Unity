using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {//IDLE:DO NOTHING, DEFFENSE: PROTECT GOAL LINE, RECOVER: GO AFTER PUCK, PURUSUE: GO AFTER PLAYER, ATTACK; ATTACK PLAYER, OFFENSE: GO SCORE.
        IDLE, DEFFENSE, RECOVER, PURSUE, ATTACK, OFFENSE
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    //name is based on the ENUM
    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    //this is not the ENUM
    protected State nextState;
    protected NavMeshAgent agent;

    float visDistance = 10.0f;
    float visAngle = 30.0f;
    float hitDistance = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    //skeleton code for each phases
    //This shuold run first and sets the stage to whatever it was going to be next
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    //this is the method run from outside to progress the stage through
    public State Process()
    {
        //once inside it will change state to UPDATE
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        //if we're not returning anything then we just return this state
        return this;
    }

    //helper method, player must be within a distance and infront
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < visDistance && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if(direction.magnitude < hitDistance)
        {
            return true;
        }
        return false;
    }

}

/*We'll create the states that inherit from the original State Class inside this same file but for practice only, typically they'll be on a separate file*/
    public class Idle :State
{
    //we use base to store it for us
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                :base (_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsIdle");
        //this sets the stage to update
        base.Enter();
    }
    public override void Update()
    {
        //transition to our nextState, someway it has to break itself out of there and this is how it will do it for now. UPDATE to EXIT
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;

        }//to make player stay longer in idle we increase the range EX.(0,5000)
        else if(Random.Range(0,100) < 10)
        {
            nextState = new Deffense(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        //base.Update();
    }

    public override void Exit()
    {
        //Use Reset as a trick with Anim, since this will erase any events in the system sitting there.
        anim.ResetTrigger("IsIdle");
        base.Exit();
    }
}

public class Deffense: State
{
    //We'll use this to follow the waypoints
    int currentIndex = -1;

    public Deffense(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.DEFFENSE;
        //speed is only necesary when agent has a path, otherwise it will not move
        agent.speed = 10;
        //stop the agent to move on a particular path if you change this, you stop him to give another destination.
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //validate and store the last closest distance for all waypoints to transition to on deffense
        float lastDistance = Mathf.Infinity;
        for(int i = 0; i<Manager_Environment.Singleton.Waypoints.Count; i++)
        {
            GameObject thisWP = Manager_Environment.Singleton.Waypoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if(distance < lastDistance)
            {
                currentIndex = i - 1;
                lastDistance = distance;
            }
        }
        anim.SetTrigger("IsDeffense");
        base.Enter();
    }
    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            //we'll move to the next waypoint to complete
            if (currentIndex >= Manager_Environment.Singleton.Waypoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.SetDestination(Manager_Environment.Singleton.Waypoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;

        }
        //base.Update();
    }
    public override void Exit()
    {
        anim.ResetTrigger("IsDeffense");
        base.Exit();
    }
}

public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsPursue");
        base.Enter();
    }
    public override void Update()
    {
        //follow player
        agent.SetDestination(player.position);
        //this means it's following the player, We validate before executing next steps, since it needs to set and it might take some time (it's just how navmesh works)
        //something else we can do is just set it on the Enter
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Deffense(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }

        }
    }
    public override void Exit()
    {
        anim.ResetTrigger("IsPursue");
        base.Exit();
    }
}

public class Attack: State
{
    //npc needs to lookat player
    float rotationSpeed = 2.0f;
    AudioSource hit; //we'll use getcomponent, it might be better to pass it through instead of getting it every time.

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        hit = _npc.GetComponent<AudioSource>();

    }

    public override void Enter()
    {
        
        anim.SetTrigger("IsAttack");
        //this case is set to true since we want to stop the agent out of the pursue, prevent from following
        agent.isStopped = true;
        hit.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        //Rotate npc as it is hitting
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsAttack");
        hit.Stop();
        base.Exit();
    }
}