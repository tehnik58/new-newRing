using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField] private GameObject guidePrefab;
    [SerializeField] private Transform guideTargetPlayer;
    [SerializeField] private Route route;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        var guide = Instantiate(guidePrefab, spawnPoint.position, Quaternion.identity);

        if (guide.TryGetComponent(out GuideController guideController) && guideTargetPlayer)
        {
            guideController.Init(route, guideTargetPlayer);
            guideController.StartRoute();
        }
        else
        {
            Debug.LogWarning("GuideController not found or guideTargetPlayer is null");
        }
    }
}