using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporte : MonoBehaviour
{
    private GameController _gameController;

    public Transform pontoSaida, posCamera, limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBai;
    public MusicaFase novaMusica;

    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _gameController.TrocarMusica(MusicaFase.CAVERNA);

            col.transform.position = pontoSaida.position;
            Camera.main.transform.position = posCamera.position;
            _gameController.limiteCamEsq = limiteCamEsq;
            _gameController.limiteCamDir = limiteCamDir;
            _gameController.limiteCamSup = limiteCamSup;
            _gameController.limiteCamBai = limiteCamBai;
        }
    }
}
