using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCircularPlayer : MonoBehaviour
{
    [Header("Orbita Data")]
    public Transform centro;
    public int orbitaActual = 2;
    public float radio;
    [Range(-Mathf.PI, Mathf.PI)]
    public float rotacionInicial;

    [Header("Movimiento")]
    public float speed;
    public float speedDash;
    [Range(0, 0.5f)]
    public float duracionDash;
    public string estado;


    [Header("Acelerar")]
    public float aceleracion;
    public float desaceleracion;
    public float velocidadActual;
    public float velocidadMaxima;
    public float duracionFreno;


    [Header("Cambio de Orbitas")]
    public Transform[] posicionOrbita = new Transform[3];

    Rigidbody2D rBody;

    //Movimento General
    float direccion;
    [HideInInspector]
    public float rotacionSegunTiempo;

    //Movimiento con inputs y dash
    float ultimaDireccion;
    float inicioDelDash;




    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        radio = CalcularRadio(centro, posicionOrbita[orbitaActual]);
        rotacionSegunTiempo = rotacionInicial;
        //estado = "idle";
        estado = "acelerar";
        direccion = 1;
    }

    private void FixedUpdate()
    {
        //Maquina de estado 1
        if (estado == "idle")
        {
            Mirar(centro);
            CambiarDeOrbita();
            if (Input.GetKeyDown("space")) estado = "dashing";
            if (Input.GetButton("Horizontal")) estado = "moving";
        }

        if (estado == "moving")
        {
            OscilarConInputs();
            Mirar(centro);
            CambiarDeOrbita();

            if (Input.GetKeyDown("space")) estado = "dashing";
            if (Input.GetButton("Horizontal") == false) estado = "idle";
        }

        if (estado == "dashing")
        {
            Mirar(centro);
            DashConAngulos();
        }


        //Maquina de estado 2
        if (estado == "acelerar")
        {
            AcelerarSinInputs();
            CambiarDeOrbita();
            Mirar(centro);

            if (Input.GetKeyDown("space")) estado = "frenar";
        }

        if (estado == "frenar")
        {
            FrenarSinInputs();
            CambiarDeOrbita();
            Mirar(centro);

            if (DebeCambiarDireccion())
            {
                direccion = direccion * -1;
                estado = "acelerar";
            }
        }
    }




    #region Base del movimiento
    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    void Mover(float _velocidad)
    {
        #region Explicacion
        /* Como el "sin" y "cos" son operaciones ciclicas, no importa el valor
         * que tome la variable de rotacion. Tambien, al multiplicarse con la 
         * velocidad, si no se mueve, el valor es cero y suma a la rotacion. */
        #endregion
        rotacionSegunTiempo += Time.fixedDeltaTime * _velocidad;
        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY, 0);
        rBody.MovePosition(newPosition);
    }
    #endregion

    #region Movimiento con Inputs
    void OscilarConInputs()
    {
        #region Explicacion
        /* Como el "sin" y "cos" son operaciones ciclicas, no importa el valor
         * que tome la variable de rotacion. Tambien, al multiplicarse con la 
         * velocidad, si no se mueve, el valor es cero y suma a la rotacion. */
        #endregion
        direccion = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Horizontal")) ultimaDireccion = direccion;

        float speedX = direccion * speed;
        Mover(speedX);
    }

    void DashConAngulos()
    {
        if (inicioDelDash == 0)
        {
            inicioDelDash = Time.time;
        }

        if (Input.GetButton("Horizontal"))
        {
            direccion = Input.GetAxisRaw("Horizontal");
            ultimaDireccion = direccion;
        }
        else direccion = ultimaDireccion;

        float speedX = direccion * speedDash;
        rotacionSegunTiempo += Time.fixedDeltaTime * speedX;
        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY, 0);
        rBody.MovePosition(newPosition);

        if (Time.time > inicioDelDash + duracionDash)
        {
            estado = "idle";
            inicioDelDash = 0;
        }
    }
    #endregion

    #region Movimiento sin Inputs
    void AcelerarSinInputs()
    {
        velocidadActual += (Time.fixedDeltaTime * aceleracion) * direccion;
        if (direccion == 1) if (velocidadActual >= velocidadMaxima) velocidadActual = velocidadMaxima;
        if (direccion == -1) if (velocidadActual <= -velocidadMaxima) velocidadActual = -velocidadMaxima;
        Mover(velocidadActual);

        duracionFreno = Mathf.Abs(velocidadActual) / desaceleracion;
    }

    void FrenarSinInputs()
    {
        velocidadActual += (Time.fixedDeltaTime * -desaceleracion) * direccion;
        Mover(velocidadActual);
    }

    bool DebeCambiarDireccion()
    {
        if (direccion == 1)
        {
            if (velocidadActual <= 0) return true;
        }
        if (direccion == -1)
        {
            if (velocidadActual >= 0) return true;
        }
        return false;
    }
    #endregion









    float CalcularRadio(Transform _centro, Transform _satelite)
    {
        float distanciaX = _satelite.position.x - _centro.position.x;
        float distanciaY = _satelite.position.y - _centro.position.y;
        float _radio = new Vector2(distanciaX, distanciaY).magnitude;
        return _radio;
    }

    void Mirar(Transform target)
    {
        float distanciaX = transform.position.x - target.position.x;
        float distanciaY = transform.position.y - target.position.y;
        float angulo = Mathf.Atan2(distanciaY, distanciaX) * Mathf.Rad2Deg;
        rBody.MoveRotation(angulo - 270);
    }

    void CambiarDeOrbita()
    {
        if (Input.GetKeyDown("up") && orbitaActual > 0)
        {
            orbitaActual--;
            Transform nuevaOrbita = posicionOrbita[orbitaActual];
            radio = CalcularRadio(centro, nuevaOrbita);
            rBody.MovePosition(nuevaOrbita.position);
        }
        if (Input.GetKeyDown("down") && orbitaActual < 2)
        {
            orbitaActual++;
            Transform nuevaOrbita = posicionOrbita[orbitaActual];
            radio = CalcularRadio(centro, nuevaOrbita);
            rBody.MovePosition(nuevaOrbita.position);
        }
    }












    #region Funciones desechadas
    //Codigo sacado de: https://gamedev.stackexchange.com/questions/128141/how-to-orbit-an-object-around-another-object-in-an-oval-path-in-unity
    void OcilarOvalo()
    {
        #region Explicacion
        /* Como el "sin" y "cos" son operaciones ciclicas, no importa el valor
         * que tome la variable de rotacion. Tambien, al multiplicarse con la 
         * velocidad, si no se mueve, el valor es cero y suma a la rotacion. 
         * 
         * Formulas del movimiento ovalado:
         * x = centerX + (cos(angulo) * radioMaximoX);
         * y = centerY + (sin(angulo) * radioMaximoY);
         */
        #endregion
        float speedX = Input.GetAxisRaw("Horizontal") * speed;
        float anchoDeOrbita = radio * 2;
        float altoDeOrbita = radio;

        rotacionSegunTiempo += Time.fixedDeltaTime * speedX;
        float posX = centro.position.x + (Mathf.Cos(rotacionSegunTiempo) * anchoDeOrbita);
        float posY = centro.position.y + (Mathf.Sin(rotacionSegunTiempo) * altoDeOrbita);

        Vector3 newPosition = new Vector3(posX, posY, 0);
        rBody.MovePosition(newPosition);
    }


    //Codigo sacado del segundo ejemplo de: https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
    /*
     void DashConLerp()
    {
        journeyLength = Vector3.Distance(startMarker, endMarker);
        float distCovered = (Time.time - inicioDelDash) * speedDash;
        float fractionOfJourney = distCovered / journeyLength;

        Vector3 newPosition = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
        rBody.MovePosition(newPosition);

        if (transform.position == endMarker) isDashing = false;
    }
    */
    /*
    Vector3 CalcularEndDashPosition(float _direccion)
    {
        if (_direccion > 0)
        {
            rotacionSegunTiempo += dashDintanceAngle;
        }
        if (_direccion < 0)
        {
            rotacionSegunTiempo -= dashDintanceAngle;
        }
        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;
        return new Vector3(posX, posY, 0);
    }
    */
    #endregion

}
