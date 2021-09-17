using System.Collections.Generic;
using UnityEngine;

public class SilhouetteGenerator : MonoBehaviour
{
    private AIManager pastAiManager;
    private AIManager futureAiManager;

    private List<AIAgent> aiAgents = new List<AIAgent>();

    public GameObject defaultSilhouette;

    private List<GameObject> silhouettes = new List<GameObject>();

    private float offsetAmount;

    private void Start()
    {
        GameManager gm = gameObject.GetComponent<GameManager>();
        pastAiManager = gm.pastAIManager;
        futureAiManager = gm.futureAIManager;
        offsetAmount = GetComponent<Timewarp>().offsetAmount;
    }

    private void GetActiveAIAgents()
    {
        if (aiAgents.Count > 0)
        {
            aiAgents.Clear();
        }
        if (Globals.isInPast)
        {
            aiAgents = futureAiManager.activeAgents;
        }
        else
        {
            aiAgents = pastAiManager.activeAgents;
        }
    }

    public void CreateSilhouettes()
    {
        ClearSilhouettes();
        GetActiveAIAgents();
        if (Globals.isInPast)
        {
            foreach (AIAgent agent in aiAgents)
            {
                Vector3 newPos = agent.transform.position;
                newPos.y += offsetAmount;
                silhouettes.Add(Instantiate(defaultSilhouette, newPos, agent.transform.rotation));
            }
        }
        else
        {
            foreach (AIAgent agent in aiAgents)
            {
                Vector3 newPos = agent.transform.position;
                newPos.y -= offsetAmount;
                silhouettes.Add(Instantiate(defaultSilhouette, newPos, agent.transform.rotation));
            }
        }
    }

    public void ClearSilhouettes()
    {
        for (int i = 0; i < silhouettes.Count; i++)
        {
            Destroy(silhouettes[i]);
        }
        silhouettes.Clear();
    }
}