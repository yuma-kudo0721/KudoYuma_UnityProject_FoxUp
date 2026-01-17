using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    TextMeshProUGUI txt;
    [SerializeField]
    TextMeshProUGUI txtTitle;
    [SerializeField]
    MapScroll map;
    [SerializeField]
    GameObject water;
    [SerializeField]
    AudioClip se_start, se_over, se_clear;

    AudioSource snd;



    enum Mode{
        Title, Game, Over, Clear
    };

    Mode mode = Mode.Title;
    // Start is called before the first frame update
    void Start()
    {
        snd = gameObject.AddComponent<AudioSource>();
        player.enabled = false;
        txt.enabled = false;
        mode = Mode.Title;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(mode){
            case Mode.Title:
                Title();
                break;
            case Mode.Over:
            case Mode.Clear:
            CheckKeyNextTitle();
            break;
        }
        
    }

    void Title(){
        if(!Input.GetKeyDown(KeyCode.Z)){return;}

        snd.PlayOneShot(se_start);
        water.transform.position = new Vector3(0,0,0);
        map.isStop = false;
        txt.enabled = true;
        txtTitle.enabled = false;
        player.enabled  = true;
        player.Reset();
        mode =Mode.Game;

    }

    public void StartGameover(){
        snd.PlayOneShot(se_over);
        map.isStop = true;
        txt.enabled   = false;
        txtTitle.text = "GAME OVER";
        txtTitle.enabled = true;
        player.enabled = false;
        mode = Mode.Over;
    }

    public void StartGameclear(){
        snd.PlayOneShot(se_clear);
        map.isStop = true;
        txt.enabled   = false;
        txtTitle.text = "GAME CLEAR";
        txtTitle.enabled = true;
        player.enabled = false;
        

        mode = Mode.Clear;

    }

    void CheckKeyNextTitle(){
        //Zきーが押されていない場合は戻る
        if(!Input.GetKeyDown(KeyCode.Z)){return;}
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        snd.PlayOneShot(se_start);
        txtTitle.text = "FOX UP";
        txtTitle.enabled = true;
        mode = Mode.Title;



    }
}
