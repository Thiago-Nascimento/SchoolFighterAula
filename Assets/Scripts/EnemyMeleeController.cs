using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMeleeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Variavel que indica se o inimigo está vivo
    public bool isDead;    
    
    // Variaveis para controlar o lado que o inimigo está virado
    public bool facingRight;
    public bool previousDirectionRight;

    // Variavel para armazenar posição do Player
    private Transform target;

    // Variaveis para movimentação do inimigo
    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private bool isWalking;

    private float horizontalForce;
    private float verticalForce;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posição
        target = FindAnyObjectByType<PlayerController>().transform;

        // Incializar a velocidade do inimigo
        currentSpeed = enemySpeed;
    }

    void Update()
    {
        // Verificar se o Player está para a Direita ou para a Esquerda
        // E com isso determinar o lado que o Inimigo ficará virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se facingRight for TRUE, vamos virar o inimigo em 180º no eixo Y,
        // Senão vamos virar o inimigo para a esquerda

        // Se o Player à direita e a direção anterior NÃO era direita (inimigo olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o Player NÃO está à direita e a direção anterior ERA direita (inimigo olhando para direita)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Gerenciar a animação do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // MOVIMENTAÇÃO

        // Variavel para armazenar a distancia entre o Inimigo e o Player
        Vector3 targetDistance = target.position - this.transform.position;

        // Determina se a força horizontal deve ser negativa ou positiva
        // 5 / 5     =   1
        // -5 / 5    =   -1
        horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

        // Caso esteja perto do Player, parar a movimentação
        if (Mathf.Abs(targetDistance.x) < 0.2f)
        {
            horizontalForce = 0;
        }

        // Aplica velocidade no inimigo fazendo o movimentar
        rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, 0);
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }
}
