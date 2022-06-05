using UnityEngine;

public enum TrafficSignalState {Walk, DontWalk}

public class TrafficSignalsHandler : MonoBehaviour
{

    public TrafficSignalState _signalState;

    public Material _walkMat;
    [ColorUsageAttribute(false, true)]
    public Color _walkEmission = new Color(0,0.217f,0.035f,1);

    public Material _dontWalkMat;
    [ColorUsageAttribute(false, true)]
    public Color _dontwalkEmission = new Color(0,0.217f,0.035f,1);
    
    void Start()
    {
        ChangeState(TrafficSignalState.DontWalk);
    }

    public void ChangeState(TrafficSignalState _newState){
        _signalState = _newState;
        if (_signalState == TrafficSignalState.Walk){            
            _walkMat.SetColor("_BaseColor", new Color(1,1,1,1));
            _walkMat.SetColor("_EmissionColor", _walkEmission);
            
            _dontWalkMat.SetColor("_BaseColor", new Color(0,0,0,0));
            _dontWalkMat.SetColor("_EmissionColor", new Color(0,0,0,0));
        }
        else if (_signalState == TrafficSignalState.DontWalk){

            _dontWalkMat.SetColor("_BaseColor", new Color(1,1,1,1));
            _dontWalkMat.SetColor("_EmissionColor", _dontwalkEmission);
            
            _walkMat.SetColor("_BaseColor", new Color(0,0,0,0));
            _walkMat.SetColor("_EmissionColor", new Color(0,0,0,0));
        }
    }

    public void GreenLight(){
        ChangeState(TrafficSignalState.Walk);
    }
}
