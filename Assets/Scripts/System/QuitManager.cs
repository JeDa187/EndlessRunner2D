using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuitManager : MonoBehaviour
{
    public static QuitManager Instance { get; private set; }
    public GameObject quitPanelPrefab;
    private GameObject activeQuitPanel;
    private TMP_Text quitMessage;
    private bool isSecondConfirmation = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleQuitPanel();
        }
    }

    public void ToggleQuitPanel()
    {
        if (activeQuitPanel == null)
        {
            activeQuitPanel = Instantiate(quitPanelPrefab, transform.position, Quaternion.identity);
            quitMessage = activeQuitPanel.GetComponentInChildren<TMP_Text>();
            Button[] buttons = activeQuitPanel.GetComponentsInChildren<Button>();

            // Oletetaan, että ensimmäinen painike on "Yes" ja toinen on "No"
            buttons[0].onClick.AddListener(HandleYesButton);
            buttons[1].onClick.AddListener(ToggleQuitPanel);
        }
        else
        {
            Destroy(activeQuitPanel);
            activeQuitPanel = null;
            isSecondConfirmation = false; // Resetoi toisen tason varmistus, kun paneeli suljetaan.
        }
    }

    private void HandleYesButton()
    {
        if (!isSecondConfirmation)
        {
            isSecondConfirmation = true;
            if (quitMessage)
                quitMessage.text = "Are you sure?";
        }
        else
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        // Tähän kohtaan voi lisätä toimintoja, kuten tallennuksen, ennen pelin sulkemista.
        Application.Quit();
    }
}
