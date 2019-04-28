using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform playerTransformer;
    private Camera cam;

    public Transform limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBai;
    public float speedCam;
    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        CamController();
    }

    private void CamController()
    {
        float posCamX = playerTransformer.position.x;
        float posCamY = playerTransformer.position.y;

        if (cam.transform.position.x < limiteCamEsq.position.x && playerTransformer.position.x < limiteCamEsq.position.x)
        {
            posCamX = limiteCamEsq.position.x;
        }
        else if (cam.transform.position.x > limiteCamDir.position.x && playerTransformer.position.x > limiteCamDir.position.x)
        {
            posCamX = limiteCamDir.position.x;
        }

        if (cam.transform.position.y < limiteCamBai.position.y && playerTransformer.position.y < limiteCamBai.position.y)
        {
            posCamY = limiteCamBai.position.y;
        }
        else if (cam.transform.position.y > limiteCamSup.position.y && playerTransformer.position.y > limiteCamSup.position.y)
        {
            posCamY = limiteCamSup.position.y;
        }


        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }
}
