using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Replay
{
    public class ReplayManager: MonoBehaviour
    {
        public static List<KeyCode> inputs = new List<KeyCode>();
        public static List<float> timeBetweenInputs = new List<float>();
        
        //private static Transform[] _transforms;
        private static MemoryStream _memoryStream;
        private static BinaryWriter _binaryWriter;
        private static BinaryReader _binaryReader;
        private bool _recording;
        private bool _replaying;
        private bool _recordingInitialized;
        private int _currentRecordingFrames;
        private int _maxRecordingFrames = 7200;
        private int _replayFrameLength = 2;
        private int _replayFrameTimer;

        private void FixedUpdate()
        {
            if (_recording)
            {
                UpdateRecording();
            }
            else if (_replaying)
            {
                UpdateReplaying();
            }
        }

        private void InitializeRecording()
        {
            _memoryStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_memoryStream);
            _binaryReader = new BinaryReader(_memoryStream);
            _recordingInitialized = true;
        }
        
        public void StartRecording()
        {
            if (!_recordingInitialized)
            {
                InitializeRecording();
            }
            else
            {
                _memoryStream.SetLength(0);
            }
            
            ResetReplayFrame();
            StartReplayFrameTimer();
            
            _recording = true;
        }
        
        private void UpdateRecording()
        {
            if (_currentRecordingFrames > _maxRecordingFrames)
            {
                StopRecording();
                _currentRecordingFrames = 0;
                return;
            }

            if (_replayFrameTimer == 0)
            {
                //SaveTransforms(inputs);
                ResetReplayFrameTimer();
            }
            --_replayFrameTimer;
            ++_currentRecordingFrames;
        }
        
        public void StopRecording()
        {
            _recording = false;

            Debug.Log("Size of inputs: " + inputs.Count);
            Debug.Log("Size of timeBetweenInputs: " + timeBetweenInputs.Count);
        }
        
        private void ResetReplayFrame()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            _binaryWriter.Seek(0, SeekOrigin.Begin);
        }

        public void StartReplaying()
        {
            ResetReplayFrame();
            StartReplayFrameTimer();
        
            _replaying = true;
        }

        private void UpdateReplaying()
        {
            if (_memoryStream.Position >= _memoryStream.Length)
            {
                StopReplaying();
                return;
            }

            if (_replayFrameTimer == 0)
            {
                //LoadTransforms(inputs);
                ResetReplayFrameTimer();
            }
            --_replayFrameTimer;
        }

        private void StopReplaying()
        {
            _replaying = false;
        }

        private void ResetReplayFrameTimer()
        {
            _replayFrameTimer = _replayFrameLength;
        }

        private void StartReplayFrameTimer()
        {
            _replayFrameTimer = 0;
        }

        private void SaveTransforms(IEnumerable<KeyCode> keyCodes)
        {
            foreach (var keyCode in keyCodes)
            {
                SaveTransform(keyCode);
            }
        }

        private void SaveTransform(KeyCode keyCode)
        {
            // var transformLocalPosition = transformToSave.localPosition;
            // var transformLocalScale = transformToSave.localScale;
            //
            // _binaryWriter.Write(transformLocalPosition.x);
            // _binaryWriter.Write(transformLocalPosition.y);
            // _binaryWriter.Write(transformLocalPosition.z);
            // _binaryWriter.Write(transformLocalScale.x);
            // _binaryWriter.Write(transformLocalScale.y);
            // _binaryWriter.Write(transformLocalScale.z);
            
            //_binaryWriter.Write(keyCode);
        }

        private void LoadTransforms(IEnumerable<KeyCode> keyCodes)
        {
            foreach (var keyCode in keyCodes)
            {
                LoadTransform(keyCode);
            }
        }

        private void LoadTransform(KeyCode keyCode)
        {
            // var x = _binaryReader.ReadSingle();
            // var y = _binaryReader.ReadSingle();
            // var z = _binaryReader.ReadSingle();
            // transformToLoad.localPosition = new Vector3(x, y, z);
            // x = _binaryReader.ReadSingle();
            // y = _binaryReader.ReadSingle();
            // z = _binaryReader.ReadSingle();
            // transformToLoad.localScale = new Vector3(x, y, z);

            var keyHash = _binaryReader.ReadSingle();
        }
    }
}