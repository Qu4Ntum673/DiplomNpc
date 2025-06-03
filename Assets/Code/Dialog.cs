using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour
{
    private CameraFollow cameraFollow;
    public string npcInitialResponse = "������, ������! � ������� ������ ��������. ������ ������ ��� ��� �����?";

    // �������� ������� ������
    public string[] playerResponses = { "��� ��� �� ��������?", "������ �� ��� �����?" };

    // ������ NPC �� ������ ������
    public string[][] npcResponses = {
        new string[] { "��� �������� ��������, ������� ��� ������ � ����. � ��� ����� ����." },
        new string[] { "�� ����� ����� ��� ����� �����, ��� ������ ����� �������. ��� ���� � �� ����� �������� ��� �����." }
    };

    public TextMeshProUGUI responseText;
    public TextMeshProUGUI[] playerButtonsText; // ������ ������� ��� ������
    public Button[] playerButtons; // ������ ������
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
                HideDialog(); // �������� � ���������� ������, ���� ����� ������
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

        // ������������� ����� ��� ������
        for (int i = 0; i < playerResponses.Length; i++)
        {
            playerButtonsText[i].text = playerResponses[i];
            int index = i; // ��������� ���������� ��� ���������
            playerButtons[index].onClick.RemoveAllListeners(); // ������� ������ ���������
            playerButtons[index].onClick.AddListener(() => PlayerRespond(index));
        }

        SetButtonsActive(true);
    }

    void HideDialog()
    {
        responseText.text = ""; // ������� �����
        SetButtonsActive(false); // �������� ������
        dialogActive = false; // ������������� ���� ������� � false
        StopAllCoroutines(); // ������������� ��� ��������, ��������� � ��������
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
        Debug.Log("�: " + playerResponses[index]);
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