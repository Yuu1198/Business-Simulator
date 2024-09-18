using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlourFactory : ResourceFactory
{
    private int neededWheat = 10;

    protected override void StartProduction()
    {
        if (GameManager.Instance.GetWheat(neededWheat))
        {
            // Audio
            audioSource.PlayOneShot(startProductionClip);

            inProduction = true;

            productionProgressBar.gameObject.SetActive(true);
            productionProgressBar.value = 0f;

            GameManager.Instance.OnFlourProductionUpdated(true, productionTime);

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

        GameManager.Instance.OnFlourProductionUpdated(false, productionTime);
    }

    protected override void Collect()
    {
        base.Collect();

        GameManager.Instance.CollectFlour();
    }

}
