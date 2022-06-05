using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Tooltip("LOOKOUT, WAIT, WALKACROSS")]
    public List<GameObject> _cardImagesPrefabs = new List<GameObject>(3);

    [HideInInspector]
    public List<GameObject> _cardsSequenceUI = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> _tempSequenceUI = new List<GameObject>(3);

    public GameObject _sequencePanel;
    public GameObject _arPanel;
    public GameObject _confirmButton;
    public GameObject _addButton;
    public GameObject _hideButton;
    public GameObject _hintButton;
    public GameObject _restartButton;
    public GameObject _resultPanel;
    public TMP_Text _resultText;

    public GameObject _hintPanel;
    public TMP_Text _hintText;

    public Card_Capturer _cardCapturer;

    [HideInInspector]
    public bool _uiVisible = true;

    public void ShowHintPanel(){
        _hintPanel.SetActive(true);
    }

    public void HideHintPanel(){
        _hintPanel.SetActive(false);
    }

    internal void AddCardToSequence(card_type card)
    {
        GameObject _cardGameObj = Instantiate(_cardImagesPrefabs[(int) card], _sequencePanel.transform);
        _cardsSequenceUI.Add(_cardGameObj);
    }

    internal void ResetSequence()
    {
        foreach (Transform child in _sequencePanel.transform) GameObject.Destroy(child.gameObject);
        _cardsSequenceUI = new List<GameObject>();
    }

    internal void ResetTempSequence()
    {
        foreach (GameObject _tempcard in _tempSequenceUI) GameObject.Destroy(_tempcard);
        _tempSequenceUI = new List<GameObject>(3);
    }

    internal void AddCardToTempSequence(card_type card)
    {
        GameObject _cardGameObj = Instantiate(_cardImagesPrefabs[(int) card], _sequencePanel.transform);
        _cardGameObj.GetComponent<Image>().color = new Color(0.6f,0.6f,0.6f);
        _tempSequenceUI.Add(_cardGameObj);
    }

    internal void AddButtonInteractable(bool interactable){
        _addButton.GetComponent<Button>().interactable = interactable;
    }

    internal void ConfirmButtonInteractable(bool interactable){
        _confirmButton.GetComponent<Button>().interactable = interactable;
    }
    
    internal void SetUIVisibility(bool Visible){
        if (!_cardCapturer._capturingCards) { return; }
        _uiVisible = Visible;
        _arPanel.SetActive(Visible);
        _hintButton.SetActive(Visible);
        _restartButton.SetActive(Visible);
        _hideButton.transform.GetChild(0).gameObject.SetActive(Visible);
        _hideButton.transform.GetChild(1).gameObject.SetActive(! Visible);
    }

    public void ToggleUI(){
        SetUIVisibility(_uiVisible);
        _uiVisible = ! _uiVisible;
    }

}
