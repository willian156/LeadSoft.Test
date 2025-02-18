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