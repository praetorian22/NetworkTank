using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] InputField inputNameField;
    [SerializeField] GameObject clientServerPanel;
    [SerializeField] List<GameObject> specialButtonsGO = new List<GameObject>();

    private Coroutine checkNameCoro;
    private void Start()
    {
        if (checkNameCoro != null) StopCoroutine(checkNameCoro);
        checkNameCoro = StartCoroutine(CheckNameFieldCoro());
        GameManager.singleton.FirstEnterPlayer();
        foreach (GameObject go in specialButtonsGO)
        {
            go.GetComponent<ButtonSpecial>().special = new Special(positionSpecial.free, typeSpecial.none);
        }
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
    public void SetNewSpecial(Special special)
    {
        foreach (GameObject go in specialButtonsGO)
        {
            if (go.GetComponent<ButtonSpecial>().special == null) continue;
            if (special.PositionSpecial == go.GetComponent<ButtonSpecial>().special.PositionSpecial)
            {
                go.GetComponent<ButtonSpecial>().special = new Special(special);
                return;
            }
        }
    }
}
