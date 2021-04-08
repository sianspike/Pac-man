using System.Collections;
using System.Collections.Generic;
using ScriptResources;
using UnityEngine;

public class GhostMode : MonoBehaviour
{
    [SerializeField] public float frightenedSpeed = 2.9f;
    [SerializeField] public float consumedSpeed = 15f;
    
    public Mode currentMode = Mode.Scatter;
    public float frightenedModeTimer;
    public float previousSpeed;
    public float normalSpeed = 5.9f;
    public int modeChangeIteration = 1;
    public float modeChangeTimer;
    
    private const int FrightenedModeDuration = 10;
    private const int ScatterModeTimer1 = 7;
    private const int ChaseModeTimer1 = 20;
    private const int ScatterModeTimer2 = 7;
    private const int ChaseModeTimer2 = 20;
    private const int ScatterModeTimer3 = 5;
    private const int ChaseModeTimer3 = 20;
    private const int ScatterModeTimer4 = 5;
    private Mode _previousMode;
    private GhostMove _ghostMovement;
    private GameObject _game;
    private AudioSource _backgroundAudio;
    private Audio _audio;

    private void Start()
    {
        _ghostMovement = GetComponent<GhostMove>();
        _game = GameObject.Find("game");
        _backgroundAudio = _game.transform.GetComponent<AudioSource>();
        _audio = _game.transform.GetComponent<Audio>();
    }
    
    private void ChangeMode(Mode mode)
    {
        if (currentMode == Mode.Frightened)
        {
            _ghostMovement.speed = previousSpeed;
        }

        if (mode == Mode.Frightened)
        {
            previousSpeed = _ghostMovement.speed;
            _ghostMovement.speed = frightenedSpeed;
        }

        if (currentMode != mode)
        {
            _previousMode = currentMode;
            currentMode = mode;
        }
    }
    
    public void ModeUpdate()
    {
        if (currentMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;

            switch (modeChangeIteration)
            {
                case 1:
                {
                    if (currentMode == Mode.Scatter && modeChangeTimer > ScatterModeTimer1)
                    {
                        ChangeMode(Mode.Chase);
                        modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(modeChangeTimer > ChaseModeTimer1)) return;

                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                    break;
                }
                case 2:
                {
                    if (currentMode == Mode.Scatter && modeChangeTimer > ScatterModeTimer2)
                    {
                        ChangeMode(Mode.Chase);
                        modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(modeChangeTimer > ChaseModeTimer2)) return;
                
                    modeChangeIteration = 3;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                    break;
                }
                case 3:
                {
                    if (currentMode == Mode.Scatter && modeChangeTimer > ScatterModeTimer3)
                    {
                        ChangeMode(Mode.Chase);
                        modeChangeTimer = 0;
                    }

                    if (currentMode != Mode.Chase || !(modeChangeTimer > ChaseModeTimer3)) return;
                
                    modeChangeIteration = 4;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                    break;
                }
                case 4 when currentMode != Mode.Scatter || !(modeChangeTimer > ScatterModeTimer4):
                    return;
                
                case 4:
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                    break;
            }
        }
        else if (currentMode == Mode.Frightened)
        {
            frightenedModeTimer += Time.deltaTime;

            if (frightenedModeTimer >= FrightenedModeDuration)
            {
                _backgroundAudio.clip = _audio.normalBackgroundAudio;
                _backgroundAudio.Play();
                frightenedModeTimer = 0;
                ChangeMode(_previousMode);
            }
        }
    }

    public void StartFrightenedMode()
    {
        _backgroundAudio.clip = _audio.frightenedBackgroundAudio;
        _backgroundAudio.Play();
        frightenedModeTimer = 0;
        
        ChangeMode(Mode.Frightened);
    }
}
