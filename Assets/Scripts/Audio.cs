using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] public AudioClip chomp1;
    [SerializeField] public AudioClip chomp2;
    
    private bool _playedChomp1;
    private AudioSource _audio;
    
    // Start is called before the first frame update
    private void Start()
    {
        _audio = transform.GetComponent<AudioSource>();
    }

    public void PlayChompSound()
    {
        if (_playedChomp1)
        {
            _audio.PlayOneShot(chomp2);
            _playedChomp1 = false;
        }
        else
        {
            _audio.PlayOneShot(chomp1);
            _playedChomp1 = true;
        }
    }
}