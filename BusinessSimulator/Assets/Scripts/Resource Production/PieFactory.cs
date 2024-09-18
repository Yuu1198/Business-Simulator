using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieFactory : ResourceFactory
{
    private int neededDough = 1;
    private int neededBerries = 20;

    protected override void StartProduction()
    {
        if (GameManager.Instance.GetDoughAndBerries(neededDough, neededBerries))
        {
            // Audio
            audioSource.PlayOneShot(startProductionClip);

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

        GameManager.Instance.OnPieProductionUpdated(false, productionTime);
    }

    protected override void Collect()
    {
        base.Collect();

        GameManager.Instance.CollectPie();
    }
}
