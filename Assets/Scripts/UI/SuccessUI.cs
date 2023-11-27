using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SuccessUI : MonoBehaviour
{
    public GameObject successPanel;
    public Button retryButton;

    void Start()
    {
        // Disable the success panel at the beginning
        successPanel.SetActive(false);

        retryButton.gameObject.SetActive(false);

        // Add an onClick listener to the retry button
        retryButton.onClick.AddListener(RetryButtonClicked);
    }

    // Method to show the success panel
    public void ShowSuccessPanel()
    {
        successPanel.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    // Method to handle the retry button click
    private void RetryButtonClicked()
    {
        // Reload the current scene (you may need to import UnityEngine.SceneManagement)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


}
