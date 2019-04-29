using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    private Transform cam;

    public GameState currentState;
    public GameObject painelTitulo, painelGameOver, painelEnd;
    public GameObject[] fase;
    public Transform playerTransformer, limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBai;
    public float speedCam;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip sfxJump, sfxAttack, sfxCoin, sfxEnemyDeath, sfxDamage, musicFloresta, musicCaverna, musicGameOver, musicFim;
    public AudioClip[] sfxStep;
    public MusicaFase musicaAtual;

    public int moedasColetadas, vida;
    public Text moedasTxt;
    public Image[] coracoes;
    
    private void Start()
    {
        cam = Camera.main.transform;
        HeartController();
    }

    private void Update()
    {
        if(currentState == GameState.TITULO && Input.GetKeyDown(KeyCode.Space))
        {
            currentState = GameState.GAMEPLAY;
            painelTitulo.SetActive(false);
        }

        else if ((currentState == GameState.GAMEOVER || currentState == GameState.END) && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LateUpdate()
    {
        CamController();
    }

    private void CamController()
    {
        float posCamX = playerTransformer.position.x;
        float posCamY = playerTransformer.position.y;

        if (cam.position.x < limiteCamEsq.position.x && playerTransformer.position.x < limiteCamEsq.position.x)
        {
            posCamX = limiteCamEsq.position.x;
        }
        else if (cam.position.x > limiteCamDir.position.x && playerTransformer.position.x > limiteCamDir.position.x)
        {
            posCamX = limiteCamDir.position.x;
        }

        if (cam.position.y < limiteCamBai.position.y && playerTransformer.position.y < limiteCamBai.position.y)
        {
            posCamY = limiteCamBai.position.y;
        }
        else if (cam.position.y > limiteCamSup.position.y && playerTransformer.position.y > limiteCamSup.position.y)
        {
            posCamY = limiteCamSup.position.y;
        }


        Vector3 posCam = new Vector3(posCamX, posCamY, cam.position.z);
        cam.position = Vector3.Lerp(cam.position, posCam, speedCam * Time.deltaTime);
    }

    public void PlaySFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    public void TrocarMusica(MusicaFase novaMusica)
    {
        AudioClip clip = null;
        switch (novaMusica)
        {
            case MusicaFase.FLORESTA:
                {
                    clip = musicFloresta;
                    break;
                }
            case MusicaFase.CAVERNA:
                {
                    clip = musicCaverna;
                    break;
                }
            case MusicaFase.GAMEOVER:
                {
                    clip = musicGameOver;
                    break;
                }
            case MusicaFase.THEEND:
                {
                    clip = musicFim;
                    break;
                }
        }
        StartCoroutine("ControleMusica", clip);
    }

    private IEnumerator ControleMusica(AudioClip musica)
    {
        float volumeMaximo = musicSource.volume;

        for (float volume = volumeMaximo; volume > 0; volume -= 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = musica;
        musicSource.Play();

        for (float volume = 0; volume < volumeMaximo; volume += 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }

    public void GetHit()
    {
        vida -= 1;
        HeartController();
        if(vida <= 0)
        {
            playerTransformer.transform.gameObject.SetActive(false);
            painelGameOver.SetActive(true);
            currentState = GameState.GAMEOVER;
            TrocarMusica(MusicaFase.GAMEOVER);
        }
    }

    public void GetCoin()
    {
        moedasColetadas += 1;
        moedasTxt.text = moedasColetadas.ToString();
    }

    public void HeartController()
    {
        foreach (Image h in coracoes)
        {
            h.enabled = false;
        }
        for (int v = 0; v < vida; v++)
        {
            coracoes[v].enabled = true;
        }
    }

    public void GameOver()
    {
        PlaySFX(sfxDamage, 0.5f);
        vida = 0;
        HeartController();
        painelGameOver.SetActive(true);
        currentState = GameState.GAMEOVER;
        TrocarMusica(MusicaFase.GAMEOVER);
    }

    public void TheEnd()
    {
        currentState = GameState.END;
        painelEnd.SetActive(true);
        TrocarMusica(MusicaFase.THEEND);
    }
}