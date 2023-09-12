using UnityEngine;

public class LoadingPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel; 

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}
