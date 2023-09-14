using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LoadingPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Image playFabLogo;

    private const string FULL_LOADING_TEXT = "LOADING...";
    private bool isWaiting = false;

    private void Awake()
    {
        // Aseta logon alfa-arvo aluksi 0:ksi (täysin läpinäkyvä)
        Color logoColor = playFabLogo.color;
        logoColor.a = 0f;
        playFabLogo.color = logoColor;
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "";
        StartCoroutine(LoadingAnimation());
    }

    public void HideLoadingPanel()
    {
        if (!isWaiting)
        {
            loadingPanel.SetActive(false);
        }
    }

    private IEnumerator LoadingAnimation()
    {
        isWaiting = true;

        // Logo's Fade-in
        float fadeInDuration = 1.0f;
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / fadeInDuration);
            Color logoColor = playFabLogo.color;
            logoColor.a = normalizedTime;
            playFabLogo.color = logoColor;
            yield return null;
        }

        // Text loading with individual letter fade-in
        float delay = 1f / FULL_LOADING_TEXT.Length;
        for (int i = 0; i < FULL_LOADING_TEXT.Length; i++)
        {
            loadingText.text += FULL_LOADING_TEXT[i];
            yield return StartCoroutine(FadeInLetter(i, delay));
        }

        // Wait for x seconds after all letters are displayed
        yield return new WaitForSeconds(0.2f);

        isWaiting = false;
        HideLoadingPanel();
    }

    private IEnumerator FadeInLetter(int charIndex, float duration)
    {
        loadingText.ForceMeshUpdate(); // Pakota teksti päivittämään sen verteksitieto
        TMP_CharacterInfo charInfo = loadingText.textInfo.characterInfo[charIndex];
        Color32[] newVertexColors = loadingText.textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 255, counter / duration);

            int vertexIndex = charInfo.vertexIndex;
            newVertexColors[vertexIndex + 0].a = (byte)alpha;
            newVertexColors[vertexIndex + 1].a = (byte)alpha;
            newVertexColors[vertexIndex + 2].a = (byte)alpha;
            newVertexColors[vertexIndex + 3].a = (byte)alpha;

            loadingText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return null;
        }
    }

}
