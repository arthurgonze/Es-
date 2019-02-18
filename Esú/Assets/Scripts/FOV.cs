using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        ROTATIONAL
    }

    //raio de visao
    public float viewRadius = 5;
    //angulo de visao
    public float viewAngle = 115;
    public float radiusOffsetDistance = 2;

    //variaveis para identificarmos o que eh obtaculo e o que deve ser detectado respectivamente
    public LayerMask obstacleMask, detectionMask;

    //vetor de possiveis alvos em nosso raio de visao
    public Collider2D[] targetsInRadius;

    //lista de posicoes de possiveis alvos visiveis
    public List<Transform> visibleTargets = new List<Transform>();

    private Direction direction;
    Vector2 dir;
    [SerializeField] float cos = 1, sen = 1, x = 1, y = 1, velocity = 1;

    private Game_Controller controller;

    private void Start()
    {
        //Initial direction state
        direction = Direction.RIGHT;

        controller = FindObjectOfType<Game_Controller>();
    }

    private void Update()
    {
        FindVisibleTargets();
        if (direction == Direction.ROTATIONAL)
        {
            GetComponentInChildren<FOVMesh>().E += velocity * Time.deltaTime;
        }
    }

    public void SetDirection(Vector2 direcao)
    {
        if (direcao.x > 0.1)
        {
            direction = Direction.RIGHT;
            dir = transform.right;
        }
        else if (direcao.x < -0.1)
        {
            direction = Direction.LEFT;
            dir = -transform.right;
        }
        else if (direcao.y > 0.1)
        {
            direction = Direction.UP;
            dir = transform.up;
        }
        else if (direcao.y < -0.1)
        {
            direction = Direction.DOWN;
            dir = -transform.up;
        }
    }

    public void SetRotational(bool toggle, float velocity)
    {
        if (toggle)
        {
            direction = Direction.ROTATIONAL;
            this.velocity = velocity;
            GetComponentInChildren<FOVMesh>().setRotational(true);
        }
    }

    void FindVisibleTargets()
    {
        //cria um circulo centrado no detentor desse script, com raio definido pelo desenvolvedor e que ira retornar
        //quantos colisores que entrarem nesse circulo que estejam na camada de deteccao e ira colocar esses colisores
        // no vetor passado como terceiro parametro
        targetsInRadius = Physics2D.OverlapBoxAll(transform.position,
            new Vector2(viewRadius + radiusOffsetDistance, viewRadius + radiusOffsetDistance),
            viewAngle, detectionMask);

        //limpa o vetor de alvos visiveis
        visibleTargets.Clear();

        //Debug.Log(targetsInRadius.Length);
        //percorre todo o vetor de alvos no raio de visao
        for (int i = 0; i < targetsInRadius.Length; i++)
        {
            //aloca a transformacao do objeto no vetor para uma variavel para facilitar a manipulacao/comparacao
            Transform target = targetsInRadius[i].transform;

            //calculo para descobrir o vetor direcao a partir do detentor desse script para o alvo
            Vector2 dirTarget = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);

            //tranform.right sempre aponta para o eixo x positivo
            //se o angulo entre o vetor de direçao enre o jogador e o alvo, e o vetor positivo X for menor que o 
            //angulo de visao dividido por 2
            Vector2 dir = new Vector2();

            if (Vector2.Angle(dirTarget, dir) < viewAngle / 2)
            {
                //armazena a distancia entre o detentor desse script e o alvo
                float distanceTarget = Vector2.Distance(transform.position, target.position);

                //Se ao lancarmos um raio na direcao do alvo e na distancia do alvo e nao colidir com nenhum objeto,
                //adicionamos esse alvo no vetor de alvos visiveis
                if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget + radiusOffsetDistance, obstacleMask))
                {
                    visibleTargets.Add(target);
                    if(target.gameObject.tag == "Player")
                    {
                        controller.GetComponent<Game_Controller>().changeGameStatus();
                    }
                }
            }
        }
    }

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        // se os angulos não forem globais, transforma eles em globais
        if (!global)
        {
            angleDeg += transform.eulerAngles.z;
        }

        switch (direction)
        {
            case Direction.RIGHT:
                //GetComponentInChildren<Transform>().Rotate(new Vector3(1, 1, 0));
                GetComponentInChildren<FOVMesh>().E = 1;
                GetComponentInChildren<FOVMesh>().F = 1;
                GetComponentInChildren<FOVMesh>().G = 1;

                return new Vector2(cos * Mathf.Cos(x * angleDeg * Mathf.Deg2Rad), sen * Mathf.Sin(y * angleDeg * Mathf.Deg2Rad));
            case Direction.LEFT:
                //GetComponentInChildren<Transform>().Rotate(new Vector3(-180,-1,0));
                GetComponentInChildren<FOVMesh>().E = 1;
                GetComponentInChildren<FOVMesh>().F = -1;
                GetComponentInChildren<FOVMesh>().G = -1;

                return new Vector2(-1 * cos * Mathf.Cos(angleDeg * Mathf.Deg2Rad), sen * Mathf.Sin(angleDeg * Mathf.Deg2Rad));
            case Direction.UP:
                //GetComponentInChildren<Transform>().Rotate(new Vector3(-180, -1, 0));
                GetComponentInChildren<FOVMesh>().E = 3;
                GetComponentInChildren<FOVMesh>().F = -3.66f;
                GetComponentInChildren<FOVMesh>().G = -1;

                return new Vector2(-1 * cos * Mathf.Cos(angleDeg * Mathf.Deg2Rad), sen * Mathf.Sin(angleDeg * Mathf.Deg2Rad));
            case Direction.DOWN:
                //GetComponentInChildren<Transform>().Rotate(new Vector3(1, 1, 0));
                GetComponentInChildren<FOVMesh>().E = 1;
                GetComponentInChildren<FOVMesh>().F = 1;
                GetComponentInChildren<FOVMesh>().G = 1;

                return new Vector2(cos * Mathf.Sin(angleDeg * Mathf.Deg2Rad), -1 * sen * Mathf.Cos(angleDeg * Mathf.Deg2Rad));
        }


        return new Vector2(cos * Mathf.Cos(x * angleDeg * Mathf.Deg2Rad), sen * Mathf.Sin(y * angleDeg * Mathf.Deg2Rad));
    }
}
