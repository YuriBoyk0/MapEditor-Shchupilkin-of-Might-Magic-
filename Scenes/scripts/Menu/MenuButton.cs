using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using UnityNpgsql;
using TMPro;


public class MenuButton : MonoBehaviour
{   

    private float speed = 1.0f;
    [SerializeField]private Camera cam;
    [SerializeField]private  GameObject camstart;
    [SerializeField]private GameObject camend;
    [SerializeField]private float maxSpeed; 
    [SerializeField]private GameObject[] panels;
    [SerializeField]private Button prefab; 
    [SerializeField]private GameObject content; 
    [SerializeField]private GameObject Area; 
    [SerializeField]private Color color; 

    private Button[] button;
    private int selected;
    private float lastClick = 0f;
    private float interval = 0.4f;
   
    private void MoveCamera(){
        if (panels[1].activeSelf&&cam.transform.position!=camend.transform.position)
        {

            var change = maxSpeed * Time.deltaTime;
            cam.transform.position =Vector3.MoveTowards(cam.transform.position, camend.transform.position, change);

            Vector3 targetDirection = cam.transform.position - camend.transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(cam.transform.forward , targetDirection, singleStep, 0.0f);
            Debug.DrawRay(camend.transform.position, newDirection, Color.red);
            cam.transform.rotation = Quaternion.LookRotation(newDirection);
        }
        if (panels[0].activeSelf&&cam.transform.position!=camstart.transform.position)
        {
        
            
            Vector3 targetDirection = cam.transform.position - camstart.transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(cam.transform.forward , targetDirection, singleStep, 0.0f);
            Debug.DrawRay(camstart.transform.position, newDirection, Color.red);
            cam.transform.rotation = Quaternion.LookRotation(newDirection);
            
            var change = maxSpeed * Time.deltaTime;
            cam.transform.position =Vector3.MoveTowards(cam.transform.position, camstart.transform.position, change);
        }          
    }                                                      
    
    public void OnButtonDownNewGame(){  
        SetDisable(false);
        panels[1].SetActive(true);
    }

    public void OnButtonDownLoad(){
        SetDisable(false);
        ReloadButton();
        panels[3].SetActive(true);
    }

    private void SetDisable(bool set){
        for(int i =0;i<panels.Length;i++)
            panels[i].SetActive(set);
        
    }

    public void OnButtonDownSetings(){
        SetDisable(false);
        panels[2].SetActive(true);
    }

    public void OnButtonDownBack(){
        SetDisable(false);
        panels[0].SetActive(true);
        
        
    }

    public void OnButtonDownExit(){     
        Application.Quit();
    }

    public void OnButtonDownDeleteSaves(){
        string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();
        NpgsqlCommand dbcom = conn.CreateCommand();
        string command = $"DELETE FROM saves WHERE id={selected+1}";
        dbcom.CommandText = command;
        dbcom.ExecuteNonQuery();
        
        for(int i=selected;i<button.Length;i++){
        command=$"UPDATE saves SET id = {i+1} WHERE id = {i+2}";
        dbcom.CommandText = command;
        dbcom.ExecuteNonQuery();
        }
        ReloadButton();
        dbcom.Dispose();
        conn.Close();
        
    }

    private void ReloadButton(){
        for(int i =0;i<button.Length;i++){
        button[i].gameObject.SetActive(false);   
        }
        
        LoadSaves();
        
        for(int i =0;i<button.Length;i++){
        button[i].gameObject.SetActive(true);   
        }
    }

    private NpgsqlDataReader Request(string command,ref NpgsqlConnection conn ){
            NpgsqlCommand dbcom = conn.CreateCommand();
            dbcom.CommandText = command;
            NpgsqlDataReader reader = dbcom.ExecuteReader();
            dbcom.Dispose();
            return reader; 
    }

    private void LoadSaves(){
       
        
        string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();
        NpgsqlDataReader reader = Request("SELECT count(*) FROM saves",ref conn);



        while(reader.Read())  
            button=new Button[reader.GetInt64(0)];     

        reader=Request("SELECT * FROM saves",ref conn);
        content.GetComponent<VerticalLayoutGroup>().padding.bottom = (int)(Area.GetComponent<RectTransform>().rect.height - (button.Length+1)*prefab.GetComponent<RectTransform>().rect.height);
        if(content.GetComponent<VerticalLayoutGroup>().padding.bottom<0)
            content.GetComponent<VerticalLayoutGroup>().padding.bottom=0;

        for(int i =0;i<button.Length;i++){
        reader.Read();
        button[i] = Instantiate( prefab );   
        button[i].transform.position = content.transform.position;
        button[i].GetComponent<RectTransform>().SetParent(content.transform);
        button[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,0,prefab.GetComponent<RectTransform>().rect.width);
        button[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text=$"\tНазвание: {reader.GetString(0)}\t|\tДата: {reader.GetDateTime(1).ToShortDateString()} {reader.GetDateTime(5).ToLongTimeString()}\t|\tУровень: {reader.GetInt32(2)}\t|\tПерсонаж: {reader.GetString(4)}";
        }
        
        
        reader.Close();
        conn.Close();
        prefab.gameObject.SetActive(false);
       
    }

    public void OnButtonDownOrigin(Button downbutton){     
        prefab.GetComponent<Image>().color=color;
        for(int i=0;i<button.Length;i++){
            if(downbutton==button[i]){
                selected=i;
            }
           button[i].GetComponent<Image>().color=color;
        }
        downbutton.GetComponent<Image>().color=Color.green;
    }
    
    public void InteractableOff(Button downbutton){           
        downbutton.interactable=false;
    }

    public void InteractableOn(Button downbutton){      
        downbutton.interactable=true;
    }

    public void SetActiveOf(GameObject downbutton){     
        downbutton.SetActive(false);
    }

    public void SetActiveOn(GameObject downbutton){       
        downbutton.SetActive(true);
    }

    public void OnButtonDownLoadGame(){     
            Debug.LogError($"Load Game With {selected+1} saves");
    }

    public void Question(GameObject panel){
        panel.SetActive(true);     
        for(int i=0;i<button.Length;i++){
            button[i].interactable=false;
        }  
    }

    public void QuestionButton(){     
        for(int i=0;i<button.Length;i++){
            button[i].interactable=true;
        }  
    }

    void Start(){  
        LoadSaves();
        SetDisable(true);
        SetDisable(false);
        panels[0].SetActive(true);    
    }  

     void Update()
    {
        MoveCamera();
        
    }
}
