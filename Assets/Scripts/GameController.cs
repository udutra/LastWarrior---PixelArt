using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform playerTransformer;
    private Transform cam;

    public Transform limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBai;
    public float speedCam;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip sfxJump, sfxAttack, sfxCoin, sfxEnemyDeath, sfxDamage, musicFloresta, musicCaverna;
    public AudioClip[] sfxStep;
    public MusicaFase musicaAtual;

    public GameObject[] fase;


    
    private void Start()
    {
        cam = Camera.main.transform;
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
}
