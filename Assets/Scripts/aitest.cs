using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aitest : MonoBehaviour
{
    public GameObject player;
    public playerscript ps;
    public GameObject sprite;
    public NavMeshAgent agent;
    public float MaxLosingTime;
    public float LosingTime;
    public bool FOUND;

    void Update()
    {
        LayerMask pl = LayerMask.GetMask("Player");
        Ray ray = new Ray(sprite.transform.position, sprite.transform.forward);
        if(Physics.Raycast(ray,out RaycastHit hit,500))
        {
            if(hit.transform.gameObject.CompareTag("Player"))
            {
                FOUND = true;
                print("found you :33");
                LosingTime = MaxLosingTime;
            }
            else
            {
                LosingTime--;
            }
        }

        if(LosingTime <= 0)
        {
            LosingTime = MaxLosingTime;
            FOUND = false;
        }

        if(FOUND)
        {
            print("i'm gonna get ya");
            agent.SetDestination(player.transform.position);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(sprite.transform.position, sprite.transform.forward));
    }
}
