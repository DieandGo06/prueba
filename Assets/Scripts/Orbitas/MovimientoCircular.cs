using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCircular : MonoBehaviour
{
    public Transform centro;
    public Transform satelite;
    public float speed;
    public float radio;

    [Range(-1, 1)]
    public int direccion;
    [Range(-Mathf.PI, Mathf.PI)]
    public float rotacionInicial;

    Rigidbody2D rBody;
    [HideInInspector] public float rotacionSegunTiempo;



    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        radio = CalcularRadio(centro, satelite);
        rotacionSegunTiempo = rotacionInicial;
    }

    private void FixedUpdate()
    {
        Mover();
    }



    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    public void Mover()
    {
        float speedX = direccion * speed;
        rotacionSegunTiempo += Time.fixedDeltaTime * speedX;

        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;
        rBody.MovePosition(new Vector3(posX, posY, 0));
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
        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;
        transform.position = new Vector3(posX, posY, -1);
    }
}
