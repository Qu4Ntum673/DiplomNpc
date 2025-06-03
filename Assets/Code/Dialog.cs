using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour
{
    private CameraFollow cameraFollow;
    public string npcInitialResponse = "Привет, путник! Я потерял важный артефакт. Можешь помочь мне его найти?";

    // Варианты ответов игрока
    public string[] playerResponses = { "Что это за артефакт?", "Почему он так важен?" };

    // Ответы NPC на каждый вопрос
    public string[][] npcResponses = {
        new string[] { "Это семейный артефакт, который был утерян в лесу. Я его очень ценю." },
        new string[] { "Он очень важен для нашей семьи, это символ нашей истории. Без него я не смогу покинуть это место." }
    };

    public TextMeshProUGUI responseText;
    public TextMeshProUGUI[] playerButtonsText; // Массив текстов для кнопок
    public Button[] playerButtons; // Массив кнопок
    public float interactionDistance = 3.0f;
    public float letterDelay = 0.1f;

    private bool dialogActive = false;

    void Start()
    {
        SetButtonsActive(false);
        cameraFollow = Object.FindAnyObjectByType<CameraFollow>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= interactionDistance)
            {
                if (Input.GetKeyDown(KeyCode.E) && !dialogActive)
                {
                    ShowDialog();
                }
            }
            else if (dialogActive)
            {
                HideDialog(); // Скрываем и прекращаем диалог, если игрок отошел
            }
        }
    }

    void ShowDialog()
    {
        dialogActive = true;
        responseText.text = "";
        StartCoroutine(TypeText(npcInitialResponse));
        cameraFollow.StartDialog();

    }

    IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            responseText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        // Устанавливаем текст для кнопок
        for (int i = 0; i < playerResponses.Length; i++)
        {
            playerButtonsText[i].text = playerResponses[i];
            int index = i; // Локальная переменная для замыкания
            playerButtons[index].onClick.RemoveAllListeners(); // Удаляем старые слушатели
            playerButtons[index].onClick.AddListener(() => PlayerRespond(index));
        }

        SetButtonsActive(true);
    }

    void HideDialog()
    {
        responseText.text = ""; // Очищаем текст
        SetButtonsActive(false); // Скрываем кнопки
        dialogActive = false; // Устанавливаем флаг диалога в false
        StopAllCoroutines(); // Останавливаем все корутины, связанные с диалогом
        cameraFollow.EndDialog();

    }

    void SetButtonsActive(bool active)
    {
        foreach (Button button in playerButtons)
        {
            button.gameObject.SetActive(active);
        }
    }

    void PlayerRespond(int index)
    {
        Debug.Log("Я: " + playerResponses[index]);
        responseText.text = "";
        StartCoroutine(TypeResponses(npcResponses[index]));
        SetButtonsActive(false);
    }

    IEnumerator TypeResponses(string[] responses)
    {
        foreach (string response in responses)
        {
            yield return TypeText(response);
            yield return new WaitForSeconds(1f);
        }
        SetButtonsActive(true);
    }
}