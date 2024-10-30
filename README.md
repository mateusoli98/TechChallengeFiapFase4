# TechChallenge_03

## Grupo - 17

- Mateus Oliveira - RM355320
- Renan Ferreira - RM353185
- Thiago Matos - RM355947

## Vídeo de apresentação (entrega)

- [Clique aqui](https://www.youtube.com/watch?v=vH4TJV-whWU)

## Requisitos Funcionais

- **Microserviços:** dividir a aplicação em microsserviços, onde cada microsserviço realiza uma operação do padrão REST.
- **Testes de integração:** assegurar que os componentes do sistema funcionem corretamente quando integrados.
- **Integração contínua (CI) com Github Actions:** automatizar testes unitários, testes de integração e build.
- **Monitoramento com Prometheus e Grafana:** implementar métricas para monitorar a saúde e o desempenho da aplicação.

## Requisitos Técnicos

- **CI Pipeline:** Build, Testes unitários e Testes de integração.
- **Prometheus:** Coletar métricas: Latência das requisições, uso de CPU e Memória. Configurar endpoints de métricas.
- **Grafana:** Criar painéis para exibir as métricas: Latência, contagem de requisições por status de resposta, uso de recursos do sistema (memória e CPU).

# Prometheus e Grafana

## Gerando a imagem CreateAPI

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t contactcreateapiv1:latest -f ./Contact/APIs/CreateAPI/Dockerfile .

## Gerando a imagem DeleteAPI

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t contactdeleteapiv1:latest -f ./Contact/APIs/DeleteAPI/Dockerfile .

## Gerando a imagem ReadAPI

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t contactreadapiv1:latest -f ./Contact/APIs/ReadAPI/Dockerfile .

## Gerando a imagem UpdateAPI

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t contactupdateapiv1:latest -f ./Contact/APIs/UpdateAPI/Dockerfile .

## Gerando a imagem CreateWorker

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t createworker-tech3:latest -f ./Contact/Workers/CreateWorker/Dockerfile .

## Gerando a imagem DeleteWorker

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t deleteworker-tech3:latest -f ./Contact/Workers/DeleteWorker/Dockerfile .

## Gerando a imagem UpdateWorker

- Dentro do diretório TechChallengeFiapFase3 abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker build -t updateworker-tech3:latest -f ./Contact/Workers/UpdateWorker/Dockerfile .

## Executando o Docker Compose

- Dentro do diretório TechChallengeFiapFase3/ServicesCompose abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > docker compose --project-name techchallenge-3 up -d

## Aplicando as Migrations

- Alterar o Server da Connection String para “Locahost” e fazer o rebuild do projeto.
- Dentro do diretório TechChallengeFiapFase3/Contact/Infra abrir o terminal powershell ou cmd e digitar o comando abaixo:
  > dotnet ef database update

## Acessando as aplicações

- contactcreateapiv1-tech3: http://localhost:8000/swagger/index.html
- contactdeleteapiv1-tech3: http://localhost:8002/swagger/index.html
- contactreadapiv1-tech3: http://localhost:8003/swagger/index.html
- contactupdateapiv1-tech3: http://localhost:8004/swagger/index.html
- prometheus-tech2-2: http://localhost:9090
- grafana-tech2-2: http://localhost:3000
- rabbitmq-tech3: http://localhost:15672 (user: guest, password: guest)

## Coletando Metricas da API

- http://localhost:8000/metrics

## Consultando reposta da API no Prometheus

- Ir na opção status e depois targets para verificar se a API está UP.

# Acessando o Grafana

- Usuário: admin
- Senha: admin

## Adicionando o DataSource Prometheus

- Add new connection Prometheus, definir a conexão e salvar.

## Adicionando Dashboards ao Grafana

- Clicar em New e depois em import passando o id. Ids utilizados 10915 e 10427.

# Métricas para monitorar requisições por status e uso de cpu

- sum by(code) (max_over_time(http_request_duration_seconds_count[1m]))

## Métrica utilizada para total de requisições por status code:

- sum by(code) (max_over_time(http_request_duration_seconds_count[1m]))

## Métricas utilizadas para latência de endpoint:

- histogram_quantile(0.5, rate(http_request_duration_seconds_bucket{job="contactapi", endpoint="contact"}[5m]))
- histogram_quantile(0.5, rate(http_request_duration_seconds_bucket{job="contactapi", endpoint="contact/search"}[5m]))
- histogram_quantile(0.5, rate(http_request_duration_seconds_bucket{job="contactapi", endpoint="contact/{id}"}[5m]))
- histogram_quantile(0.5, rate(http_request_duration_seconds_bucket{job="contactapi", endpoint="contact/{id}/permanently"}[5m]))

## Métrica utilizada para uso de cpu:

- system_runtime_cpu_usage

## Métrica utilizada para utilização de memória

- dotnet_total_memory_bytes /1024/1024
