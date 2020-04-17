# PROOF OF CONCEPT IDENTITY SERVER 4 .NET CORE
## Token + Refresh Token + Expire Time Span Cookie 

### 1. OBJETIVO
<p>Demonstrar em uma Proof of Concept (PoC) em .net core a utilização do Identity Server 4 e suas configurações para os ajustes de tempo de expiração do Token, Refresh Token e Cookie em uma app MVC consumindo uma API (ambos protegidos pelo Identity).</p>

### 2. ARQUITETURA
<ul>
    <li>2.1 - IDENTITY SERVER<br>
    <li>2.2 - API (protegida pelo Identity)<br>
    <li>2.3 - MVC (protegida pelo Identity)<br>
</ul>

### 3. INSTALAÇÃO
3.1 - Clone o repositório em um diretório de sua    preferência.
    
3.2 - Execute os 3 projetos e terminais distintos:

    \>Docway.IDSERVER
    \>Docway.API
    \>Docway.MVC

```sh
dotnet run
```

 ### 4. URLs de execução em self-host:
    IDSERVER: http://localhost:5000
    MVC:      http://localhost:5001
    API:      http://localhost:5002

### 5. SUGESTÃO DE CASOS DE TESTES
5.1 - USUÁRIO NÃO AUTENTICADO<br>
<li><b><font style=color:green>Usuário:</font></b> 	acessa a app em http://localhost:5000<br>
<li><b><font style=color:green>Usuário:</font></b> 	clica em "Listar Médicos API".<br>
<li><b><font style=color:purple>App:</font></b>	 	exibe a mensagem: "401 - Não autorizado! Acesso Expirado."
<br><br><br>
5.2 - USUÁRIO AUTENTICADO E ATIVO<br>
<li><b><font style=color:green>Usuário:</font></b> 	acessa a app em http://localhost:5000<br>
<li><b><font style=color:green>Usuário:</font></b> 	clica em "Mostrar Token"<br>
<li><b><font style=color:purple>App:</font></b>	 	redireciona para a tela de login do Identity.<br>
<li><b><font style=color:green>Usuário:</font></b> 	digita o usuário: docway e senha: docway<br>
<li><b><font style=color:purple>App:</font></b>	 	redireciona para o MVC e exibe o Token e Refresh Token.<br>
<li><b><font style=color:green>Usuário:</font></b> 	clica em "Listar Médicos API".<br>
<li><b><font style=color:purple>App:</font></b>	 	retorna a lista de médicos obtidas da API1.
<br><br><br>
5.3 - USUÁRIO AUTENTICADO E INATIVO POR 1 (UM) MINUTO<br>
<li><b><font style=color:green>Usuário:</font></b> 	clica em "Mostrar Médicos API".<br>
<li><b><font style=color:purple>App:</font></b>	 	retorna a lista de médicos obtidas da API1.
<li><b><font style=color:green>Usuário:</font></b> 	permanece ocioso por 1 minuto.<br>
<li><b><font style=color:green>Usuário:</font></b> 	após 1 minuto de inatividade clica em "Mostrar Médicos API"<br>
<li><b><font style=color:purple>App:</font></b>	 	exibe a mensagem: "401 - Não autorizado! Acesso Expirado."
<br><br><br>
5.4 - USUÁRIO AUTENTICADO COM COOKIE EXPIRADO (Refresh Token)
<li><b><font style=color:green>Usuário:</font></b> 	clica em "Mostrar Token".<br>
<li><b><font style=color:purple>App:</font></b>	 	identifica que o cookie expirou, chama o Identity Server, revalida o token e retorna para a tela do MVC que exibindo o novo Token e Refresh Token.

### 6. CONCLUSÃO
Nesta PoC verificamos que apesar do Token estar dentro da validade padrão de 1 hora ele expira por causa do cookie estar ajustado para expirar com 1 (um) minuto de inatividade (veja a linha 33 da Startup.cs do projeto Docway.MVC).	Portanto, é recomendável que o tempo de expiração do cookie seja ajustado para um tempo maior do que o tempo de expiração do Token, para evitar que a app MVC não consiga pegar o token no cookie para realizar chamadas nas APIs usando o token válido, como acontece na linha 59 da HomeController.cs do projeto MVC.

### 7. Nota do Desenvolvedor
Esta é apenas uma aplicação conceito para demonstrar as possíveis formas de implementações do Identity Server em aplicações distribuídas e pode ser utilizada como exemplo de uso para o desenvolvimento de aplicações em diversas arquiteturas.

### 8. REFERENCIAS
https://identityserver4.readthedocs.io/en/latest/
https://github.com/IdentityServer/IdentityServer4.Templates



