using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityNpgsql;
using UnityEngine.SceneManagement;


public class TriggerHeroes : MonoBehaviour
{
    private RaycastHit theHit ;
    [SerializeField] private GameObject[] hero;
    [SerializeField] private GameObject panelhero;
    [SerializeField] private InputField name;
    private MenuButton button=new MenuButton();
    private List<GameObject> objects;
    
    private void OnAllRed(){
        for(int i =0;i<hero.Length;i++)
             hero[i].GetComponent<Outline>().OutlineColor= Color.red;   
    }

    public void AllSetActive(bool ch){
        for(int i =0;i<hero.Length;i++)
             hero[i].SetActive(ch);
    }
    
    private (double damage,double armor, double evasions) CalculateDamage(int agility,int strength,int intelligence){
        return (strength*0.5*agility+1,strength*0.5*intelligence+1,agility*0.5*intelligence+1);   
    }

    public void OnButtonDownBack(){
        AllSetActive(false);
        OnAllRed();
        panelhero.SetActive(false);
        objects[1].GetComponent<Button>().interactable=false;
    }

    private void FillingPanelHero(int count){
            string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand dbcom = conn.CreateCommand();
            string command =
            "SELECT * " +
            "FROM hero";
            dbcom.CommandText = command;
            NpgsqlDataReader reader = dbcom.ExecuteReader();
            for(int i=0; i<count+1;i++)
                reader.Read();
            GameObject.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text=$"{reader.GetString(0)}";     
            GameObject.Find("Charact").GetComponent<TMPro.TextMeshProUGUI>().text=$@"Ловкость: {reader.GetInt32(1)} Сила: {reader.GetInt32(2)} Интелект: {reader.GetInt32(3)}
Урон: {CalculateDamage(reader.GetInt32(1),reader.GetInt32(2),reader.GetInt32(3)).damage} Броня: {CalculateDamage(reader.GetInt32(1),reader.GetInt32(2),reader.GetInt32(3)).armor} Уклонения: {CalculateDamage(reader.GetInt32(1),reader.GetInt32(2),reader.GetInt32(3)).evasions}";

            reader.Close();
            dbcom.Dispose();
            conn.Close();
        }

    private void CheckClick(){
        for(int i =0;i<hero.Length;i++)
                    if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ) , out theHit , Mathf.Infinity )&&theHit.collider.transform.position == hero[i].transform.position ) //Прверяем попадение в любой колайдер,совпадение позицию этого колайдера с массивом
                        if(Input.GetMouseButtonDown(0)&&hero[i].GetComponent<Outline>().OutlineColor!=Color.green)
                        {
                            panelhero.SetActive(true);
                            FillingPanelHero(i);
                            OnAllRed();
                            hero[i].GetComponent<Outline>().OutlineColor= Color.green;
                        }
                        else    
                            hero[i].GetComponent<Outline>().OutlineWidth=5;
                    else
                        if(hero[i].GetComponent<Outline>().OutlineColor!=Color.green)
                            hero[i].GetComponent<Outline>().OutlineWidth=0;
                        else 
                            objects[1].GetComponent<Button>().interactable=true;
                            
    }
    public void clear(InputField inputfield){
        inputfield.Select();
        inputfield.text = "";
    }
    public void OnButtonDownSelect(Button downbutton){
        objects[0].SetActive(true);
        downbutton.interactable=false;
    }

    public void OnButtonDownClose(Button downbutton){
        objects[0].SetActive(false);    
        downbutton.interactable=true;
        clear(name);    
    }

    private void FillingList(){
        objects=new List<GameObject>();
        objects.Add(GameObject.Find("InterName"));
        objects.Add(GameObject.Find("Select"));
        objects[1].GetComponent<Button>().interactable=false;
    }

    public void OnButtonDownStartGame(GameObject input){
    //    string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
    //     NpgsqlConnection conn = new NpgsqlConnection(connectionString);
    //     conn.Open();
    //     NpgsqlCommand dbcom = conn.CreateCommand();

    //     string command = $"SELECT count(*) FROM saves";
    //     dbcom.CommandText = command;
    //     NpgsqlDataReader reader = dbcom.ExecuteReader();
    //     reader.Read();
    //     name.text=name.text==""?"Unknown":name.text;
    //     command = $"INSERT INTO saves (savetitle,date,lvl,id,personname,time) VALUES ('{name.text}', '{DateTime.Now}', '{1}', '{reader.GetInt64(0)+1}', '{GameObject.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text}', '{DateTime.Now}')";
    //     reader.Close();
    //     dbcom.CommandText = command;
    //     dbcom.ExecuteNonQuery();
    //     reader.Close();
    //     dbcom.Dispose();
    //     conn.Close();
    //     clear(name);  
        
    //     GameObject.Find("BackTo").GetComponent<Button>().interactable=true;
        SceneManager.LoadScene("Scene");
    }

    private void Start() {
        AllSetActive(false);
        FillingList();
        panelhero.SetActive(false);
        OnAllRed();
    }

   private void Update()
    {
        if(!objects[0].activeSelf)
            CheckClick();
    }
}
