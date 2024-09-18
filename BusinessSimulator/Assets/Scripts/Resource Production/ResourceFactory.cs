using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ResourceFactory : MonoBehaviour
{
    public float productionTime;
    protected float productionTimer = 0f;

    protected bool inProduction = false;
    protected bool finishedProduction = false;

    public Slider productionProgressBar;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startProductionClip;
    public AudioClip collectClip;

    private void Start()
    {
        productionProgressBar.gameObject.SetActive(false);
    }

    // Player Input
    private void OnMouseDown()
    {
        if (!inProduction && !finishedProduction)
        {
            StartProduction(); // Produce
        }
        else if (finishedProduction)
        {
            Collect(); // Collect
        }
    }

    // OVERRIDE FOR NEEDED FUNCTIONALITY
    protected virtual void StartProduction()
    {
        return;
    }

    protected virtual IEnumerator Produce()
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

        // INSERT CORRECT PRODUCTION UPDATE (GM)
    }

    protected virtual void Collect()
    {
        // Reset
        finishedProduction = false;
        productionProgressBar.gameObject.SetActive(false);
        productionProgressBar.value = 0f;

        // Audio
        audioSource.PlayOneShot(collectClip);

        // INSERT CORRECT COLLECT FUNCTION (GM)
    }


}
