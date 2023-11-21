
# Maps da Vila Pelicanos 

**Número da Lista**: 10<br>
**Conteúdo da Disciplina**: Dividir e Conquistar<br>

## Alunos
|Matrícula | Aluno |
| -- | -- |
| 21/1043647  |  Giovanni Alvissus Camargo Giampauli |
| 21/1031092  |  Larissa Gomes Silva |

## Sobre 
Os habitantes da Vila Pelicanos precisam constantemente se locomover o mais rápido por ela para chegar nas lojas antes de fecharem, ou encontrar o melhor caminho para dar um presente a outro habitante. Pensando nisso, foi criado um Maps da Vila Pelicanos, para que consigam chegar o mais rápido possível pelo menor caminho ao seu destino. Os habitantes podem informar ao software quão distante querem ficar daqueles que não gostam muito, será informado a menor rota para que não se encontrem e acabe acontecendo uma briga. 

A lojinha do Pierre agora dispõe de uma nova forma de se fazer compras! O habitante informa quais itens tem interesse de comprar e o Pierre irá retornar a combinação dos melhores itens de acordo com quanto dinheiro você possui, infelizmente quanto melhor o produto mais caro.

Um grande problema identificado pela equipe do Maps da Vila Pelicanos é que existem muitas árvores na nova fazenda que atrapalham o caminho durante a locomoção pelo terreno, em conjunto a isso quem precisa as limpar comumente não possui muita energia então precisa ser o mais eficiente possível. Para ajudar a poupar energia, criamos uma funcionalidade que indica os pares de árvores mais próximas entre si da fazenda que podem atrapalhar o caminho. Cada árvore que voce poda, gasta 5 de energia.

Video explicando o projeto se encontra no arquivo. `Explicacao-do-projeto.mp4`.

## Screenshots
Tela inicial para seleção dos dados, em caso de dúvidas ver sessão de Uso
![imagem1](Assets/Images/Prints/1.png)
Tela com os dados selecionados 
![imagem1](Assets/Images/Prints/2.png)
Menu de configurações
![imagem1](Assets/Images/Prints/3.png)
Loja do Pierre
![imagem1](Assets/Images/Prints/4.png)
Menu de seleção dos itens
![imagem1](Assets/Images/Prints/5.png)
Produtos selecionados e dinheiro informado
![imagem1](Assets/Images/Prints/6.png)
Mochila com os produtos da combinação retornada 
![imagem1](Assets/Images/Prints/7.png)
Introdução a nova funcionalidade
![imagem1](Assets/Images/Prints/8.png)
Mapa padrão gerado
![imagem1](Assets/Images/Prints/9.png)
Targets de pontos mais proximos
![imagem1](Assets/Images/Prints/10.png)
Fazenda após algoritmo executado algumas vezes
![imagem1](Assets/Images/Prints/11.png)

## Instalação 

**Linguagem**: `C#`<br>
Pré-requisitos: 
- Primeiramente é necessário clonar ou baixar o projeto para a sua máquina
- Para rodar o projeto como exe, basta ter SO Windows ou possuir biblioteca para rodar exe no Linux ou MAC.
    - O arquivo exe está localizado na pasta `Vila Pelicano`, com o nome `Vila Pelicano.exe`

- Para rodar o projeto Unity no computador, é necessário abrir o projeto no aplicativo UnityHub
    - Quando o projeto estiver aberto na Unity, basta dar play para rodar
- O arquivo contendo o algoritmo principal utilizado está em `Assets/Scripts/GameTree.cs`

## Uso

### Maps

Após baixar e iniciar o arquivo .exe, irá aparecer um menu inicial. Ao iniciar o jogo, o usuário deve selecionar um personagem no painel esquerdo, após isso informar quais os nós de inicio e fim clicando no botão início e em seguida no nó escolhido no mapa, repetir o mesmo para fim. O mesmo processo deve ser feito para selecionar o segundo personagem e seus nós respectivos. Por fim, deve ser informado a distância mínima que devem permanecer um do outro no slider de Distância, em seguida apertar o botão de começar. 

Há um painel de controle para regular o volume da música de fundo, além da opacidade das arestas e vértices sobre o mapa da vila e a velocidade de locomoção do aldeão escolhido.

### Loja do Pierre

Para comprar na lojinha do Pierre, basta clicar no campo escrito Moedas e digitar quanto dinheiro você gostaria de gastar. Em seguida, ao clicar nos itens do menu principal, você estará desselecionando ele de forma que não será considerado para a compra, é possível scrollar para baixo para ver mais itens. Por fim, basta apertar em comprar que será adicionado todos os itens que foi possível comprar aos slots da mochila e o dinheiro será atualizado para o que sobrou, em caso de sucesso será 0 e em soluçã0 não possível qualquer resto.

### Fazenda

O objetivo do jogador dentro da fazenda é limpar seu terreno, mas para isso deve-se seguir uma lógica de limpar sempre as 2 arvores mais proximas entre si, pois elas acabam atrapalhando sua locomoção. Para realizar essa tarefa o jogador precisa ter 5 de energia para cada arvore que ele deseja podar. Também é possivel customizar a geração de arvores no terreno para fins de teste e diversão!
