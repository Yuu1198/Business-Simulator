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

    protected bool shouldProduce = true;
    public Material activeMaterial;
    public Material inactiveMaterial;
    private Renderer factoryRenderer;

    private void Start()
    {
        factoryRenderer = GetComponent<Renderer>();
        activeMaterial = factoryRenderer.material;

        productionProgressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (shouldProduce)
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
    }

    private void OnMouseDown()
    {
        if (shouldProduce)
        {
            shouldProduce = false;

            factoryRenderer.material = inactiveMaterial;
            productionProgressBar.gameObject.SetActive(false);
        }
        else
        {
            shouldProduce = true;

            factoryRenderer.material = activeMaterial;

            if (inProduction)
            {
                productionProgressBar.gameObject.SetActive(true);
            }
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
            if (!shouldProduce)
            {
                yield return null;
            }

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

        // INSERT CORRECT COLLECT FUNCTION (GM)
    }
}
