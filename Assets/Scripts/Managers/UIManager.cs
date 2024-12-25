using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] InputField inputNameField;
    [SerializeField] GameObject clientServerPanel;

    private Coroutine checkNameCoro;
    private void Start()
    {
        if (checkNameCoro != null) StopCoroutine(checkNameCoro);
        checkNameCoro = StartCoroutine(CheckNameFieldCoro());
        GameManager.singleton.FirstEnterPlayer();
    }
    private IEnumerator CheckNameFieldCoro()
    {
        startButton.interactable = false;
        while (inputNameField.text == "")
        {
            yield return null;
        }
        startButton.interactable = true;
    }

    public void PressButtonFirstDispleyStart()
    {
        GameManager.singleton.changePlayerNameEvent?.Invoke(inputNameField.text);
        clientServerPanel.SetActive(true);
    }
    public void SetPlayerName(string name)
    {
        inputNameField.text = name;
    }
}
