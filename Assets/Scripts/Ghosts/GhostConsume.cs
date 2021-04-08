using Ghosts;
using ScriptResources;
using UnityEngine;

public class GhostConsume: MonoBehaviour
{
    private GhostMode _mode;
    private GhostMove _ghostMovement;

    private void Start()
    {
        _mode = GetComponent<GhostMode>();
        _ghostMovement = GetComponent<GhostMove>();
    }
    
    public void Consumed()
    {
        _mode.currentMode = Mode.Consumed;
        _mode.previousSpeed = _ghostMovement.speed;
        _ghostMovement.speed = _mode.consumedSpeed;
    }
}
