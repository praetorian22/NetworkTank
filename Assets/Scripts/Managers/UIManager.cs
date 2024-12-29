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
    }
    public void InitSpecialButton()
    {
        specialButtonsGO = new List<GameObject>();
        GameObject[] specButtons = GameObject.FindGameObjectsWithTag("specialButtons");
        for (int i = 0; i < specButtons.Length; i++)
        {
            specialButtonsGO.Add(specButtons[i]);
        }
        for (int i = 0; i < specialButtonsGO.Count; i++)
        {
            specialButtonsGO[i].GetComponent<ButtonSpecial>().special = new Special((positionSpecial)i, typeSpecial.none, null);
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
                go.GetComponent<Button>().onClick.RemoveAllListeners();
                go.GetComponent<Button>().onClick.AddListener(() => go.GetComponent<ButtonSpecial>().special.action.Invoke(GameManager.singleton.player));
                return;
            }
        }
    }
}
