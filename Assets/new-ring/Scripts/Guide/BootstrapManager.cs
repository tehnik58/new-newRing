using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField] private GameObject guidePrefab;
    [SerializeField] private Route route;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        var guide = Instantiate(guidePrefab, spawnPoint.position, Quaternion.identity);
        if (guide.TryGetComponent(out GuideController guideController))
            guideController.Init(route);
        else
            Debug.LogWarning("GuideController not found");
        
        guideController.StartRoute();
    }
}