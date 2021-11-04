using AIStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SilhouetteGenerator : MonoBehaviour
{
    private AIManager pastAiManager;
    private AIManager futureAiManager;

    private List<AIAgent> aiAgents = new List<AIAgent>();

    public GameObject cultistAttackSilhouette;
    public GameObject cultistRunSilhouette;
    public GameObject belcherAttackSilhouette;
    public GameObject belcherRunSilhouette;

    private List<GameObject> silhouettes = new List<GameObject>();

    private float offsetAmount;

    private CustomTimer timer = new CustomTimer();

    private void Start()
    {
        GameManager gm = gameObject.GetComponent<GameManager>();
        pastAiManager = gm.pastAIManager;
        futureAiManager = gm.futureAIManager;
        offsetAmount = GetComponent<Timewarp>().offsetAmount;
        timer.Start();
    }

    private void Update()
    {
        timer.DoTick();
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
        timer.Reset();
        GetActiveAIAgents();
        if (Globals.isInPast)
        {
            foreach (AIAgent agent in aiAgents)
            {
                if (agent.isAlive)
                {
                    Vector3 newPos = agent.transform.position;
                    newPos.y += offsetAmount;
                    if (agent.currentState == StateIndex.attackPlayer)
                    {
                        silhouettes.Add(Instantiate(cultistAttackSilhouette, newPos, agent.transform.rotation));
                    }
                    else
                    {
                        silhouettes.Add(Instantiate(cultistRunSilhouette, newPos, agent.transform.rotation));
                    }
                }
            }
        }
        else
        {
            foreach (AIAgent agent in aiAgents)
            {
                if (agent.isAlive)
                {
                    Vector3 newPos = agent.transform.position;
                    newPos.y -= offsetAmount;
                    if (agent.currentState == StateIndex.attackPlayer)
                    {
                        silhouettes.Add(Instantiate(belcherAttackSilhouette, newPos, agent.transform.rotation));
                    }
                    else
                    {
                        silhouettes.Add(Instantiate(belcherAttackSilhouette, newPos, agent.transform.rotation));
                    }
                }
            }
        }
        StartCoroutine(FadeSilhouettes());
    }

    public void ClearSilhouettes()
    {
        for (int i = 0; i < silhouettes.Count; i++)
        {
            Destroy(silhouettes[i]);
        }
        silhouettes.Clear();
    }

    private IEnumerator FadeSilhouettes(int totalFadeFrames = 1, float fadeIntervals = 0.2f)
    {
        if (totalFadeFrames <= 0)
        {
            totalFadeFrames = 1;
        }

        int currentFade = 0;

        float currentParticleAmount = silhouettes[0].GetComponent<VisualEffect>().GetFloat("Particle Spawn Rate");

        while (totalFadeFrames < currentFade)
        {
            currentFade++;
            currentParticleAmount *= currentFade / totalFadeFrames;

            foreach (GameObject silhouette in silhouettes)
            {
                silhouette.GetComponent<VisualEffect>().SetFloat("Particle Spawn Rate", currentParticleAmount);
            }

            yield return new WaitForSeconds(fadeIntervals);
        }

        ClearSilhouettes();
    }
}