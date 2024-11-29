using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] InputField inputNameField;
    [SerializeField] GameObject clientServerPanel;

    private Coroutine checkNameCoro;
    private void Start()
    {
        if (checkNameCoro != null) StopCoroutine(checkNameCoro);
        checkNameCoro = StartCoroutine(CheckNameFieldCoro());
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            SaveData saveData = new SaveData();
            saveData = SaveLoad.Instance.LoadData();
            if (saveData.namePlayer != "")
            {
                DataPlayer.Instance.playerName = saveData.namePlayer;
                inputNameField.text = saveData.namePlayer;
            }
        }
        else
        {
            SaveData saveData = new SaveData();
            saveData.namePlayer = "";
            SaveLoad.Instance.SaveData(saveData);            
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
        SaveData saveData = new SaveData();
        saveData.namePlayer = inputNameField.text;
        SaveLoad.Instance.SaveData(saveData);
        clientServerPanel.SetActive(true);
    }
}
