using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vuforia;

public enum card_type{
    LOOKOUT,
    WAIT,
    WALKACROSS,
}

public class Card_Capturer : MonoBehaviour
{
    public Game_Manager _gameManager;

    public Camera _arCamera;

    public UI_Manager uiManager;
    List<card_type> _tempSequence = new List<card_type>(3);
    
    [HideInInspector]
    public List<card_type> _cardsSequence = new List<card_type>(12);
    
    [HideInInspector]
    private int _sequenceCapacity = 12;
    
    List<GameObject> _unsortedCardGameObjects;
    List<GameObject> _sortedCardGameObjects;

    public GameObject _arTargetsParent;

    [HideInInspector]
    public bool _capturingCards;


    public void StartCapturing(){
        _capturingCards = true;
        _arCamera.GetComponent<VuforiaBehaviour>().enabled = true;
        
    }
    
    public void StopCapturingCards(){
        _capturingCards = false;
        _arCamera.GetComponent<VuforiaBehaviour>().enabled = false;
    }

    public void ResetSequence(){
        _cardsSequence = new List<card_type>(12);
        uiManager.ResetSequence();
    }

    public void ResetTempSequence(){
        _tempSequence = new List<card_type>(3);
        uiManager.ResetTempSequence();
    }

    public void AddCardToSequence(card_type card){
        if (_cardsSequence.Count >= _sequenceCapacity) return;
        _cardsSequence.Add(card);
        uiManager.AddCardToSequence(card);
    }

    // for debugging purposes
    public void AddCardByNum(int num){
        AddCardToSequence((card_type)num);
    }

    public void AddCardToTempSequence(card_type card){
        if (_cardsSequence.Count + _tempSequence.Count >= _sequenceCapacity) return;
        _tempSequence.Add(card);
        uiManager.AddCardToTempSequence(card);
    }

    public void AddTempCardsToSequence(){
        foreach (card_type card in _tempSequence) AddCardToSequence(card);
        ResetTempSequence();
    }

    void CaptureCards(){
        ResetTempSequence();
        _unsortedCardGameObjects = new List<GameObject>(3);
        _sortedCardGameObjects = new List<GameObject>(3);

        // adding card_types to temp sequence based on their position in AR camera space (left to right)
        foreach (Transform arTargetTransform in _arTargetsParent.transform)
        {
            bool _targetActive = arTargetTransform.gameObject.activeInHierarchy;
            bool _targetMeshEnabled = arTargetTransform.GetChild(0).GetComponent<MeshRenderer>().enabled;
            if (_targetActive && _targetMeshEnabled)
                _unsortedCardGameObjects.Add(arTargetTransform.gameObject);
            
        }
        _sortedCardGameObjects = _unsortedCardGameObjects.OrderBy(o=>_arCamera.WorldToScreenPoint(o.transform.position).x).ToList();
        foreach (GameObject target in _sortedCardGameObjects)
        {
            if      (target.CompareTag("card_lookout"))  AddCardToTempSequence(card_type.LOOKOUT);
            else if (target.CompareTag("card_wait"))     AddCardToTempSequence(card_type.WAIT);
            else if (target.CompareTag("card_walk"))     AddCardToTempSequence(card_type.WALKACROSS);
        }
    }

    
    void Start(){
        // if Sequence Panel has placeholder prefab images remove them
        ResetSequence();

        // Start capturing cards to temp sequence
        StartCapturing();
    }
    
    void Update(){
        if (_capturingCards){
            CaptureCards();
        }
        uiManager.AddButtonInteractable(_tempSequence.Any());
        uiManager.ConfirmButtonInteractable(_cardsSequence.Any());
    }
}
