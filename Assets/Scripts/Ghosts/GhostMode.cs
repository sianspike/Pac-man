using ScriptResources;
using UnityEngine;

namespace Ghosts
{
    public class GhostMode : MonoBehaviour
    {
        public static float frightenedSpeed = 2.9f;
        public static float consumedSpeed = 15f;
    
        public Mode currentMode = Mode.Scatter;
        public float frightenedModeTimer;
        public float previousSpeed;
        public static float normalSpeed = 5.9f;
        public int modeChangeIteration = 1;
        public float modeChangeTimer;
        public static int scatterModeTimer1 = 7;
        public static int chaseModeTimer1 = 20;
        public static int scatterModeTimer2 = 7;
        public static int chaseModeTimer2 = 20;
        public static int scatterModeTimer3 = 5;
        public static int chaseModeTimer3 = 20;
        public static int scatterModeTimer4 = 5;
        public static int frightenedModeDuration = 10;
    
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
                        if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer1)
                        {
                            ChangeMode(Mode.Chase);
                            modeChangeTimer = 0;
                        }

                        if (currentMode != Mode.Chase || !(modeChangeTimer > chaseModeTimer1)) return;

                        modeChangeIteration = 2;
                        ChangeMode(Mode.Scatter);
                        modeChangeTimer = 0;
                        break;
                    }
                    case 2:
                    {
                        if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer2)
                        {
                            ChangeMode(Mode.Chase);
                            modeChangeTimer = 0;
                        }

                        if (currentMode != Mode.Chase || !(modeChangeTimer > chaseModeTimer2)) return;
                
                        modeChangeIteration = 3;
                        ChangeMode(Mode.Scatter);
                        modeChangeTimer = 0;
                        break;
                    }
                    case 3:
                    {
                        if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer3)
                        {
                            ChangeMode(Mode.Chase);
                            modeChangeTimer = 0;
                        }

                        if (currentMode != Mode.Chase || !(modeChangeTimer > chaseModeTimer3)) return;
                
                        modeChangeIteration = 4;
                        ChangeMode(Mode.Scatter);
                        modeChangeTimer = 0;
                        break;
                    }
                    case 4 when currentMode != Mode.Scatter || !(modeChangeTimer > scatterModeTimer4):
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

                if (frightenedModeTimer >= frightenedModeDuration)
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
}
