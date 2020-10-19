using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }
    //This function will predict location and seek when necesary
    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget >90) && relativeHeading <20 || target.GetComponent<PlayerController_HockeyRB>().moveSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent < PlayerController_HockeyRB > ().moveSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<PlayerController_HockeyRB>().moveSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander() // as long as there is ground for the radius the npc will move
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 1;


        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f));

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if(Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if(raycastInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        Wander();
    }

}
