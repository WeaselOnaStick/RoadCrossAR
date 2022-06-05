using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public DioramaDirector _dioramaDirector;
    public Card_Capturer _cardCapturer;
    public UI_Manager _uiManager;
    
    public LevelsListScriptableObject _levels;
    [HideInInspector]
    public LevelScriptableObject _currentLevel;
    public int _currentLevelIndex;

    public obstacle_type _levelObstacle;


    void Start(){
        
        _currentLevel = _levels.levels_list[_currentLevelIndex];

        _levelObstacle = _currentLevel.obstacle;

        // uiManager show hint prompt
        if (_currentLevel.intro_hint != ""){
            _uiManager._hintText.text = _currentLevel.intro_hint;
            _uiManager.ShowHintPanel();
        }      
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(LevelScriptableObject level){
        SceneManager.LoadScene(level.scene_name);
    }

    public void NextLevel(){
        if (_currentLevelIndex >= _levels.levels_list.Count-1) _currentLevelIndex = -1; // loop back to first level
        LoadLevel(_levels.levels_list[_currentLevelIndex+1]);
    }

    public void StartSim(){
        _uiManager.SetUIVisibility(false);
        _cardCapturer.StopCapturingCards();
        StartCoroutine(Simulation());
    }

    public IEnumerator Simulation(){
        bool _crossedTheStreet = false;
        bool _lookedBothWays = false;
        bool _waitedForSignal = false;
        foreach (card_type card in _cardCapturer._cardsSequence)
        {
            //Оглядываемся по сторонам
            if (card == card_type.LOOKOUT) 
            {
                //Может добавить проверку на зеленый свет?
                yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.LookBothWays));
                _lookedBothWays = true;
            }

            //Ждем зеленый свет, если светофора нет
            if (card == card_type.WAIT && _levelObstacle == obstacle_type.CROSS_SIMPLE) yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.GetConfused));

            //Ждем зеленый свет, если светофор есть
            if (card == card_type.WAIT && _levelObstacle == obstacle_type.CROSS_WITH_TRAFFIC_LIGHT){
                if      (!_waitedForSignal)
                {
                yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WaitForSignal));
                _waitedForSignal = true;
                }
                //Уже горит зеленый, нет смысла ждать
                else if (_waitedForSignal) yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.GetConfused));
                _lookedBothWays = false;
            }

            if (card == card_type.WALKACROSS){

                //Переход без светофора
                if (_levelObstacle == obstacle_type.CROSS_SIMPLE){
                    //Посмотрел по сторонам
                    if (_lookedBothWays){
                        yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WalkNormal, ResultTexts.WinGeneric));
                        _crossedTheStreet = true;
                        yield break;
                    }
                    //Не посмотрел по сторонам
                    else if (!_lookedBothWays){
                        yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WalkGetHit, ResultTexts.FailNoLookout));
                        yield break;
                    }
                }
                //Переход со светофором
                else if (_levelObstacle == obstacle_type.CROSS_WITH_TRAFFIC_LIGHT){
                    //Дождался зеленого света и посмотрел по сторонам
                    if (_waitedForSignal && _lookedBothWays){
                        yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WalkNormal, ResultTexts.WinGeneric));
                        _crossedTheStreet = true;
                        yield break;
                    }
                    //Дождался зеленого света но не посмотрел по сторонам
                    else if (_waitedForSignal && ! _lookedBothWays){
                        yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WalkGetHitAmbulance, ResultTexts.FailAmbulance));
                        yield break;
                    }
                    //Не дождался зеленого света
                    else if (! _waitedForSignal){
                        yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.WalkGetHit, ResultTexts.FailRedLight));
                        yield break;
                    }
                }
            }
        }

        if (! _crossedTheStreet){
            yield return StartCoroutine(_dioramaDirector.PlayAnim(DioramaTimelineTypes.ShyFail, ResultTexts.FailGeneric));
            yield break;
        }
        
    }
}
