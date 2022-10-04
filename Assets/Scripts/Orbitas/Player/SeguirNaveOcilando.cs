using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirNaveOcilando : MonoBehaviour
{
    [Header("Posicionamiento Inicial")]
    public Transform centro;
    public Transform satelite;
    public float radio;
    public bool ocultar;

    //Son publicas porque se usan en el script editor
    [HideInInspector] public float rotacion;
    [HideInInspector] public MovimientoCircularPlayer player;


    [Header("Distancia al frenar nave")]
    public float distancia;
    public float distanciaX;
    public float distanciaY;
    public float tiempo;



    private void Awake()
    {
        radio = CalcularRadio(centro, satelite);
        player = FindObjectOfType<MovimientoCircularPlayer>();
        rotacion = player.rotacionSegunTiempo;

        if (ocultar) GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        SeguirNave();

        /* Es un itnento de calcular el punto donde la velocidad llegue a 0 al frenar
        if (player.estado == "acelerar")
        {
            //CalcularDistanciaAlFrenar();
        }

        if (player.estado == "frenar")
        {

        }
        */
    }



    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    void SeguirNave()
    {
        rotacion = player.rotacionSegunTiempo;
        float posX = Mathf.Cos(rotacion) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacion) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY, 0);
        transform.position = newPosition;
    }

    public float CalcularRadio(Transform _centro, Transform _satelite)
    {
        float distanciaX = _satelite.position.x - _centro.position.x;
        float distanciaY = _satelite.position.y - _centro.position.y;
        float _radio = new Vector2(distanciaX, distanciaY).magnitude;
        return _radio;
    }


    //Esta funcion es usada en el script editor
    public void MoverAPosicionInicial()
    {
        float posX = Mathf.Cos(rotacion) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacion) * radio + centro.position.y;
        transform.position = new Vector3(posX, posY, -1);
    }


    //La idea era calcular la posicion en donde la nave frenaba por completo: Posiblemente se borrara esta funcion
    void CalcularDistanciaAlFrenar()
    {
        float delayFrenado = Mathf.Abs(player.velocidadActual) / player.desaceleracion;
        tiempo = delayFrenado;
        //Debo tener el frame rate que tiene el juego en todo momento para predecir cuantos frames tardara
        //en hacer el frenado. "delatFrenado" me esta dando el tiempo pero en segundos.

        // distancia = VelInicial * tiempo + (aceleracion *  tiempo)/2
        float distanciaRadianes = player.velocidadActual * delayFrenado + ((player.desaceleracion * delayFrenado) / 2);

        float distX = Mathf.Cos(distanciaRadianes) * radio + centro.position.x;
        float distY = Mathf.Sin(distanciaRadianes) * radio + centro.position.y;

        distancia = distanciaRadianes;
        distanciaX = distX;
        distanciaY = distY;



        rotacion = player.rotacionSegunTiempo;
        float posX = Mathf.Cos(rotacion + distanciaRadianes) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacion + distanciaRadianes) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY , 0);
        transform.position = newPosition;
    }
}
