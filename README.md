
# Meus Produtos

É um projeto de padrão [API Rest](https://www.redhat.com/pt-br/topics/api/what-is-a-rest-api) criado por [Erik Bergkamp](https://github.com/eriking50) e o projeto tem como base Asp Net 5. O banco de dados escolhido foi o [MariaDB/MySQL](https://mariadb.org). Para criptografia de banco de dados foi escolhido o algoritmo [SHA256](https://pt.wikipedia.org/wiki/SHA-2) e para autenticação do projeto foi escolhido o [JWT Bearer](https://www.devmedia.com.br/como-o-jwt-funciona/40265).

## Instruções

Antes de tudo instalar o banco de dados MariaDB em seu computador.

Após instalar o banco de dados alterar no arquivo **appsettings.json**, na linha 4:

	"MyProductsContext":  "Server=server;User Id=user;Password=pass;Database=database"

onde **server** é o local onde você irá rodar o servidor, como é um projeto para estudos localhost é uma boa opção, **user** é um nome de usuário do seu banco de dados que tem autorizações de leitura e escrita, **pass** a senha do usuário informado e **database** será o nome da tabela criada

Com os arquivos clonados em seu computador rodar as Migrations do Banco de Dados com o comandos:

### no Visual Studio 
	
	Update-Database

### no console (Dentro da pasta do projeto)  
	
	dotnet ef database update

Após rodar a migration compilar e rodar o servidor. 

As únicas rotas autorizadas para serem usadas sem login são: **Get**(ambas da rota Products), **Post**(da rota Users), **Get**(da rota Login). Caso queira fazer login de um usuário criado, utilizar a Login que irá te enviar como resposta um token que será usado para autorização. 

## Possiveis problemas

Caso esteja usando o Swagger para efetuar as chamadas você não terá problemas após fazer a autorização. Caso use algum outro programa ou faça requisição da api por meio de um app, você precisa incluir no Header da requisição:

	Authorization: Bearer /token

onde /token é o token gerado a partir do login.

## Customização

Uma possível alteração que pode ser feita no código é no arquivo **appsettings.json** na segunda linha:

	"JWTKey"  :  "key"

onde **key** é qualquer string que será usada para a geração dos tokens.

As Entidades também podem ser alteradas bem facilmente, mas lembre-se de alterar os [DTOs](https://docs.microsoft.com/pt-br/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5) para que eles reflitam as mudanças feitas.

## Considerações Finais

Foi um excelente desafio muitos dos conceitos eu já estava estudando anteriormente, confiram o meu [github](https://github.com/eriking50), eu já tinha uma Api Rest e um MVC de MySQL, juntar ambos foi uma tarefa interessante. A parte que eu mais precisei pesquisar foi em relação ao JWT e a criptografia no geral, acredito que esse foi um ponto que eu realmente empaquei em alguns momentos, ainda mais que alguns guias escritos que existem já estão desatualizados.