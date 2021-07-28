using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using State = IState<AIManager>;

namespace AIStates.Manager
{
    public enum ZoneStateIndex
    {
        inside,
        outside
    }

    public class InsideZone : State
    {
        void State.Enter(AIManager manager)
        {
            manager.playerInZone = true;

            foreach (var poolAgent in manager.cultistPool)
            {
                // Check if the agent should be active in it's zone
                if (poolAgent.isActive)
                {
                    poolAgent.gameObject.SetActive(true);
                }
            }
        }

        void State.Update(AIManager manager)
        {

        }

        void State.Exit(AIManager manager)
        {

        }
    }

    public class OutsideZone : State
    {
        void State.Enter(AIManager manager)
        {
            manager.playerInZone = false;

            foreach (var poolAgent in manager.cultistPool)
            {
                // Check if the agent should be active in it's zone
                if (poolAgent.isActive)
                {
                    poolAgent.gameObject.SetActive(false);
                }
            }
        }

        void State.Update(AIManager manager)
        {

        }

        void State.Exit(AIManager manager)
        {

        }
    }
}
