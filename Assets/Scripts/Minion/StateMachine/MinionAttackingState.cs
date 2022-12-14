using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAttackingState : MinionBaseState
{
    private Tower LockedTower;
    public override void EnterState(MinionStateManager minion)
    {
        Debug.Log("I'm in AttackState");
        Vector3 destination = minion.LockedEnemy.transform.position;
        LockedTower = minion.LockedEnemy.GetComponent<Tower>();
        minion.GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    public override void OnCollisionEnter(MinionStateManager minion, Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(MinionStateManager minion)
    {
        HandleAttack(minion);
    }

    private void HandleAttack(MinionStateManager minion)
    {
        if(minion.LockedEnemy.gameObject != null)
        {
            float distance = Vector3.Distance(minion.transform.position,
                        minion.LockedEnemy.transform.position);

            minion.transform.LookAt(LockedTower.gameObject.transform);

            if (distance <= minion.GetComponent<Minion>().Range)
            {
                minion.GetComponent<MinionAttack>().AttackTower(LockedTower);

                // if enemy tower is destroyed - switch to the FollowLaneState and follow the path
                if (LockedTower.HealthPoints <= 0)
                {
                    minion.SwitchState(minion.FollowLaneState);
                }
            }
        }
        else
        {
            minion.SwitchState(minion.FollowLaneState);
        }
        
    }
}
