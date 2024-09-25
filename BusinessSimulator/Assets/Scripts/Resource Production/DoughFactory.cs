using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoughFactory : ResourceFactory
{
    private int neededFlour = 1;
    private int neededWater = 10;

    protected override void StartProduction()
    {
        if (GameManager.Instance.GetFlourAndWater(neededFlour, neededWater))
        {
            inProduction = true;

            productionProgressBar.gameObject.SetActive(true);
            productionProgressBar.value = 0f;

            GameManager.Instance.OnDoughProductionUpdated(true, productionTime);

            StartCoroutine(Produce());
        }
        else
        {
            return; // Production not possible
        }
    }

    protected override IEnumerator Produce()
    {
        productionTimer = 0f;
        while (productionTimer < productionTime)
        {
            // Update timer and progress bar
            productionTimer += Time.deltaTime;
            productionProgressBar.value = productionTimer / productionTime;

            yield return null;
        }

        // Finished
        finishedProduction = true;
        inProduction = false;

        GameManager.Instance.OnDoughProductionUpdated(false, productionTime);
    }

    protected override void Collect()
    {
        base.Collect();

        GameManager.Instance.CollectDough();
    }
}
