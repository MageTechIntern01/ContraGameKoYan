using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorFunctions : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem particleSystem1;
    [SerializeField] private int emitAmount1;
    [SerializeField] private ParticleSystem particleSystem2;
    [SerializeField] private int emitAmount2;
    [SerializeField] private ParticleSystem particleSystem3;
    [SerializeField] private int emitAmount3;
    [SerializeField] private ParticleSystem particleSystem4;
    [SerializeField] private int emitAmount4;

    [Header("Sound Bank")]
    [SerializeField] private AudioClip[] sound1;
    [SerializeField] private float sound1Volume = 1;
    [SerializeField] private AudioClip[] sound2;
    [SerializeField] private float sound2Volume = 1;
    [SerializeField] private AudioClip[] sound3;
    [SerializeField] private float sound3Volume = 1;
    [SerializeField] private AudioClip[] sound4;
    [SerializeField] private float sound4Volume = 1;
    [SerializeField] private AudioClip[] sound5;
    [SerializeField] private float sound5Volume = 1;
    [SerializeField] private AudioClip[] sound6;
    [SerializeField] private float sound6Volume = 1;
    [SerializeField] private AudioClip[] sound7;
    [SerializeField] private float sound7Volume = 1;
    [SerializeField] private AudioClip[] sound8;
    [SerializeField] private float sound8Volume = 1;
    [SerializeField] private AudioClip[] sound9;
    [SerializeField] private float sound9Volume = 1;
    [SerializeField] private AudioClip[] sound10;
    [SerializeField] private float sound10Volume = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //Play a sound through the specified audioSource
    

    public void EmitParticles()
    {
        particleSystem1.Emit(5);
    }

    public void EmitParticles1()
    {
        particleSystem1.Emit(emitAmount1);
    }

    public void EmitParticles2()
    {
        particleSystem2.Emit(emitAmount2);
    }

    public void EmitParticles3()
    {
        particleSystem3.Emit(emitAmount3);
    }

    public void EmitParticles4()
    {
        particleSystem4.Emit(emitAmount4);
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}