# LeadSoft.Test

# Guia de Configuração do Projeto

## Ferramentas Necessárias

Para executar o código, certifique-se de ter as seguintes ferramentas instaladas:

- **Docker**
- **.NET 8**
- **Node.js** (com npm)
- **PgAdmin 4**
- **Postman** (opcional, caso seja necessário testar a API)

## Como Iniciar o Projeto

1. Na pasta `LeadSoft.Test`, crie um diretório chamado `Archive`.
2. Mova o arquivo CSV `final_animedataset.csv` para dentro da pasta `Archive`.
3. Abra um terminal **PowerShell** dentro da pasta `LeadSoft.Test`.
4. Execute o seguinte comando para construir e iniciar os containers Docker:

   ```sh
   docker compose up --build
5. Verifique o IP e porta do banco de dados (o padrão é localhost:5432)
6. Verifique se a connectionString da API está apontando para o db corretamente.
7. Abra um terminal **PowerShell** dentro da pasta `LeadSoft.Test`.
8. Execute o seguinte comando para construir e iniciar os containers Docker:

   ```sh
   dotnet-ef database upgrade
9. Você estará apto para utilizar o programa!
