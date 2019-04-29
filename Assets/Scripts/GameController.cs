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
    public AudioClip sfxJump, sfxAttack;
    public AudioClip[] sfxStep;


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
}
