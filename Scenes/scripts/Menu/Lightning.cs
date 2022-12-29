using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Lightning : MonoBehaviour
{
    [SerializeField]private GameObject effect;
    [SerializeField]private float timeligth;
    [SerializeField]private Light loop;
    [SerializeField]private AudioSource sound;
    private float time=1.0f;
    private bool lightning;

    void Start()
    {
        
    }

   private void FixedUpdate()
   {
        if(timeligth<0)
        {
            if(lightning){lightning=false;}else{lightning=true;
            sound.Play();}
            timeligth=20;
            
        }
        else   {
            timeligth-=Time.deltaTime;
            loop.intensity=1;}
        if(lightning)
        {
            if(time>0){
                time-=Time.deltaTime;
                loop.intensity=1;
            }
            if(time<=0)
            {
                effect.gameObject.GetComponent<ParticleSystem>().Play();
                loop.intensity=UnityEngine.Random.Range(500,1000);
                time=UnityEngine.Random.Range(-1f,1f);
                timeligth-=3;
            }  
        }
   }

   public void Play()
   {
       timeligth=0;
   }
}
