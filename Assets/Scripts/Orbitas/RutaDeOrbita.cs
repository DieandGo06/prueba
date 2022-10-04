using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RutaDeOrbita : MonoBehaviour
{
    [SerializeField] LineRenderer render;
    [SerializeField] Transform centro;
    [SerializeField] Transform satelite;
    public float radio;
    public float altoExtra;
    public float anchoExtra;


    private void Start()
    {
        radio = CalcularRadio();
        DibujarCirculo(100, radio);
    }

    //Codigo sacado de: https://www.youtube.com/watch?v=DdAfwHYNFOE
    public void DibujarCirculo(int vertices, float _radio)
    {
        render.positionCount = vertices;

        for (int verticeAcual = 0; verticeAcual < vertices; verticeAcual++)
        {
            float progresoDelCirculo = (float)verticeAcual / vertices;
            float radianActual = progresoDelCirculo * (2 * Mathf.PI); //Se multiplica por 2PI porque es igual a 360 grados

            float escalarX =  (Mathf.Cos(radianActual) * anchoExtra);
            float escalarY = (Mathf.Sin(radianActual) * altoExtra);
            float verticeSizeX = centro.position.x + (escalarX * _radio);
            float verticeSizeY = centro.position.y + (escalarY * _radio);

            Vector3 verticePosicion = new Vector3(verticeSizeX, verticeSizeY, 0);
            render.SetPosition(verticeAcual, verticePosicion);
        }
    }

    public float CalcularRadio()
    {
        #region nota
        /* Esta es una version "vieja" de la funcion. La "nueva" usa parametros.
         * Pero como debe funcionar en el script que edita el inspector del componente,
         * prefiero dejarla asi y acortar las lineas de dicho script
         */
        #endregion
        Vector2 _centro = new Vector2(centro.position.x, centro.position.y);
        float distaciaX = satelite.position.x - _centro.x;
        float distanciaY = satelite.position.y - _centro.y;
        float _radio = new Vector2(distaciaX, distanciaY).magnitude;
        return _radio;
    }
}
