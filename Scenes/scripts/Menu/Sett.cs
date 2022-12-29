using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System;
using UnityNpgsql;

public class Sett : MonoBehaviour
{
    [SerializeField] private Slider slidermusic;
    [SerializeField] private Slider slidersound;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject sound;

    void Start()
    {

        string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();
        NpgsqlCommand dbcom = conn.CreateCommand();
        string command = $"SELECT * FROM settings";
        dbcom.CommandText = command;
        NpgsqlDataReader reader = dbcom.ExecuteReader();
        reader.Read();
        slidersound.value=reader.GetInt32(0);
        slidermusic.value=reader.GetInt32(1);
        fullscreen.isOn=reader.GetBoolean(2);
        SliderChanged(slidermusic);
        MusicSlider();
        SoundSlider();
        reader.Close();
        dbcom.Dispose();
        conn.Close();
    }


    void Update()
    {
        
    }

    public void SliderChanged(Slider slider)
    {        
        slider.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text =Math.Round(slider.value,0).ToString();
    }

    public void MusicSlider()
    {  
       //float help=slidermusic.value==0?0.0f:slidermusic.value/1000;
        music.volume=slidermusic.value/500;
    }

    public void SoundSlider()
    {  
        //float help=slidersound.value==0?0.0f:slidersound.value/100;
        for(int i =0;i<sound.transform.childCount;i++)
            sound.transform.GetChild(i).GetComponent<AudioSource>().volume=slidersound.value/100;
        
    }

    public void OnButtonDownBack()
    {  
        string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();
        NpgsqlCommand dbcom = conn.CreateCommand();
        string command=$"UPDATE settings SET sound={Math.Round(slidersound.value,0)}, music={Math.Round( slidermusic.value,0)}, fullscreen={fullscreen.isOn}";
        dbcom.CommandText = command;
        dbcom.ExecuteNonQuery();
        dbcom.Dispose();
        conn.Close();
        
    }
}
