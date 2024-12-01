# Tech Challenge 04

## Grupo - 17

- Mateus Oliveira - RM355320
- Renan Ferreira - RM353185
- Thiago Matos - RM355947

## Vídeo de apresentação (entrega)

- [Clique aqui]()

## Requisitos Funcionais

- **Kubernets:** implementar Kubernetes para gerenciar implantação, escalabilidade e gerenciamento de estado dos microsserviços. Considerar na implementação a escalabilidade, resiliência e gerenciamente de configuração de dados.


## Requisitos Técnicos

- **Kubernets** Criar Pods, Services, ConfigMaps, ReplicaSets, Deployments e armazenamento de dados em volumes.
- **CI Pipeline:** Build, Testes unitários e Testes de integração.
- **Prometheus:** Coletar métricas: Latência das requisições, uso de CPU e Memória. Configurar endpoints de métricas.
- **Grafana:** Criar painéis para exibir as métricas: Latência, contagem de requisições por status de resposta, uso de recursos do sistema (memória e CPU).


# Docker Builds
### APIs

```shell
    docker build --no-cache -t contactcreateapiv1 -f APIs/CreateAPI/Dockerfile .

    docker build --no-cache -t contactreadapiv1 -f APIs/ReadAPI/Dockerfile .
  
    docker build --no-cache -t contactupdateapiv1 -f APIs/UpdateAPI/Dockerfile .
    
    docker build --no-cache -t contactdeleteapiv1 -f APIs/DeleteAPI/Dockerfile .

```

### Workers

```shell
    docker build --no-cache -t createworkerv1 -f Workers/CreateWorker/Dockerfile .
    
    docker build --no-cache -t updateworkerv1 -f Workers/UpdateWorker/Dockerfile .
    
    docker build --no-cache -t deleteworkerv1 -f Workers/DeleteWorker/Dockerfile .
```

# Kubernets
- Acessar a pasta Pipelines e executar o comando:

```shell
  kubectl apply -R -f .
```

Este comando vai executar todos os pods, services e deployments.

### RabbitMQ

- Instalando o Helm: https://helm.sh/pt/docs/intro/install/

- Execute os comandos:
```shell
    helm repo add bitnami https://charts.bitnami.com/bitnami
    helm repo update 
    helm upgrade --install rabbitmq --set auth.username=guest --set auth.password=guest bitnami/rabbitmq
```

- Para acessar o RabbitMQ
```shell
    kubectl port-forward --namespace default svc/rabbitmq 5672:5672
    kubectl port-forward --namespace default svc/rabbitmq 15672:15672
```

# Acessar APIs

- Create: http://localhost:30000/swagger/index.html
- Read: http://localhost:30002/swagger/index.html
- Update: http://localhost:30003/swagger/index.html
- Delete: http://localhost:30005/swagger/index.html

# Prometheus 

- Instalando o Prometheus: https://artifacthub.io/packages/helm/prometheus-community/prometheus

- Execute os comandos:
```shell
    helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
    helm repo update
    helm install prometheus prometheus-community/prometheus
```
- Para acessar o Prometheus
```shell
    kubectl --namespace default port-forward $POD_NAME 9090
```

# Grafana

- Instalando o Grafana: https://artifacthub.io/packages/helm/grafana/grafana
```shell
    helm repo add grafana https://grafana.github.io/helm-charts
    helm repo update
    helm install grafana grafana/Grafana
```
- Para acessar o Grafana
```shell
    kubectl --namespace default port-forward $POD_NAME 3000
```
- Para recuperar a senha do Grafana
```Linux
    kubectl get secret --namespace default grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo
```
- Dashboards Grafana
```Importação arquivo json: TechChallengeFiapFase4\Grafana_Dashboards
   Importação por ID: 
    18283 (Kubernetes Dashboard)
    19924 (ASP.NET Core)
    19925 (ASP.NET Core Endpoint)
```


