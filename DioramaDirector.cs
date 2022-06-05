using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public enum DioramaTimelineTypes{
    WalkNormal,
    WalkGetHit,
    GetConfused,
    LookBothWays,
    ShyFail,
    WaitForSignal,
    WalkGetHitAmbulance,
}

public enum ResultTexts{
    WinGeneric,
    FailGeneric,
    FailNoLookout,
    FailAmbulance,
    FailRedLight,
}

public class DioramaDirector : MonoBehaviour
{
    public List<string> _resultTexts = new List<string>{
        @"<color=#36B543>Победа!</color>",
        @"<color=#B71A1A>Проигрыш!</color>",
        @"<color=#B71A1A>Проигрыш!</color>
Не забывай смотреть по сторонам!",
        @"<color=#B71A1A>Проигрыш!</color>
Не забывай смотреть по сторонам даже если светофор горит зеленым светом!",
        @"<color=#B71A1A>Проигрыш!</color>
Никогда не переходи дорогу на красный свет!"
    };

    public UI_Manager _uiManager;
    public PlayableDirector _dioramaPlayer;

    [Tooltip("Walk Normal, WalkGetHit, Confused, LookBothWays, ShyFail, WaitForSignal, WalkGetHitAmbulance")]
    public List<TimelineAsset> _timeLines;


    void Start(){
        _uiManager._resultPanel.SetActive(false);
    }

    public IEnumerator PlayAnim(DioramaTimelineTypes timelineType, string resultText = ""){
        _uiManager._resultText.text = resultText;
        _dioramaPlayer.playableAsset = _timeLines[(int)timelineType];
        _dioramaPlayer.Play();
        yield return new WaitForSeconds((float) _dioramaPlayer.playableAsset.duration);
    }

    public IEnumerator PlayAnim(DioramaTimelineTypes timelineType, ResultTexts resultText){
        yield return PlayAnim(timelineType, _resultTexts[(int)resultText]);
    }
}
