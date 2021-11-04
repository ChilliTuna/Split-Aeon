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

    public float fadeDuration = 2;

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
        foreach (AIAgent agent in aiAgents)
        {
            if (agent.isAlive)
            {
                Vector3 newPos = agent.transform.position;
                if (Globals.isInPast)
                {
                    newPos.y += offsetAmount;
                }
                else
                {
                    newPos.y -= offsetAmount;
                }
                if (agent.settings.enemyType == EnemyType.belcher)
                {
                    if (agent.currentState == StateIndex.attackPlayer)
                    {
                        silhouettes.Add(Instantiate(belcherAttackSilhouette, newPos, agent.transform.rotation));
                    }
                    else
                    {
                        silhouettes.Add(Instantiate(belcherRunSilhouette, newPos, agent.transform.rotation));
                    }
                }
                else
                {
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
        StartCoroutine(FadeSilhouettes(fadeDuration));
    }

    public void ClearSilhouettes()
    {
        StopCoroutine(FadeSilhouettes());
        for (int i = 0; i < silhouettes.Count; i++)
        {
            Destroy(silhouettes[i]);
        }
        silhouettes.Clear();
    }

    private IEnumerator FadeSilhouettes(float totalFadeTime = 2, float fadeIntervals = 0.2f)
    {
        if (silhouettes.Count > 0)
        {
            if (totalFadeTime <= 0)
            {
                totalFadeTime = 1;
            }

            float currentFade = 0;

            float currentParticleAmount = cultistAttackSilhouette.GetComponent<VisualEffect>().GetFloat("Particle Spawn Rate");

            while (totalFadeTime > currentFade)
            {
                currentFade += fadeIntervals + Time.deltaTime;
                currentParticleAmount -= currentParticleAmount * (currentFade / totalFadeTime);

                if (silhouettes.Count == 0)
                {
                    StopCoroutine(FadeSilhouettes());
                }

                foreach (GameObject silhouette in silhouettes)
                {
                    silhouette.GetComponent<VisualEffect>().SetFloat("Particle Spawn Rate", currentParticleAmount);
                }

                yield return new WaitForSeconds(fadeIntervals);
            }

            ClearSilhouettes();
        }
    }
}