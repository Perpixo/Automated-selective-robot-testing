using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capiturado : MonoBehaviour
{

    Ai_Controller ai_Controller;
    [SerializeField] GameObject Player;

    Coin_Rotation coin_Rotation;
    [SerializeField] GameObject Coin;

    [SerializeField] public Transform objetoASeguir;

    public float speed = 0.1f;

    //Rotação do coin
    private Quaternion rotacaoInicial;
    public float velocidadeRetorno = 1f;

    private float startTime;

    public bool DroneGotMe = false;

    //[SerializeField] GameObject Basefinal;
    [SerializeField] GameObject Deposito;
    public bool Depositando = false;
    public bool Depositado = false;

    public float velocidadeDeposito = 1f;

    //Posição relativa ao Drone
    public Vector3 posicaoRelativa;

    public Vector3 posicaoRelativaAoDeposito;

    // Start is called before the first frame update
    void Awake() 
        {
        ai_Controller = Player.GetComponent<Ai_Controller>();
        coin_Rotation = Coin.GetComponent<Coin_Rotation>();

        rotacaoInicial = transform.rotation;
        }

    // Update is called once per frame
    void Update()
    {
        //player ref de chegada final
        bool ChegouFinalBaseRef = ai_Controller.ChegouFinalBase;

        // Calcula a distância entre os dois primeiros objetos encontrados com a tag
        float distancia = Vector3.Distance(Player.transform.position, Coin.transform.position);
        
        //iniciar coleta
        if (distancia < 0.45f)
            {

            bool isOverTargetRef = ai_Controller.isOverTarget;
        
            if (isOverTargetRef)
                {
                    coin_Rotation.rotationSpeed = 0;

                    transform.rotation = Quaternion.Lerp(transform.rotation, rotacaoInicial, Time.deltaTime * velocidadeRetorno);

                    if (startTime == 0f)
                    {
                        startTime = Time.time;
                    }
                    
                    float elapsedTime = Time.time - startTime;

                    if (!Depositando)
                    {
                        
                            if (elapsedTime > 3.0f)
                            {
                                if (!ChegouFinalBaseRef)
                                    {
                                        DroneGotMe = true;
                                    }

                                if (DroneGotMe)
                                {
                                    //seguir player
                                    Vector3 posicaoSeguir = objetoASeguir.TransformPoint(posicaoRelativa);
                                    transform.position = posicaoSeguir;
                                    
                                }
                                

                            }
                        
                    }
                    
                }
            }
        

        //finalizar coleta + deposito
        if (ChegouFinalBaseRef)
                {
                    // Calcula a direção e a distância para o objeto + posição relativa
                    //Vector3 direction = Basefinal.transform.position - transform.position + posicaoRelativaAoDeposito;
                    //float distance = direction.magnitude;

                    // Calcula a direção e a distância para o Deposito
                    Vector3 directionDeposito = Deposito.transform.position - transform.position;
                    float distanceDepostio = directionDeposito.magnitude;

                    if (distanceDepostio < 1f)
                        {   
                            Depositando = true;
                            DroneGotMe = false;
                        }

                        if (Depositando)
                        {
                            // Normaliza a direção e multiplica pela velocidade
                            Vector3 velocity = directionDeposito.normalized * velocidadeDeposito;

                            // Move o cubo na direção e velocidade calculadas
                            transform.Translate(velocity * Time.deltaTime);

                        }

                        if (directionDeposito.magnitude < 0.1f)
                        {
                            Depositado = true;
                            Depositando = false;

                            startTime = 0f;
                        }


                }
                
            
    }
            

}