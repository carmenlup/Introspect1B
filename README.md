# Introspect1B
Introspect1B solution includes 2 microservices that are separated projects.
Solution strucure contains the following projects:
1. ProductService project
1. OrderService project

# ProductService Microservice Documentation
ProductService microservice is a RESTful API that provides product related functionalities. It allows users to manage products, including creating, updating, retrieving, and deleting product information.
It provides a Swagger UI for easy API exploration and testing.
ProductService Create endpoint use Dapr to publish events to a message broker when a product is created.
This allows other services, such as OrderService, to subscribe to these events and perform actions based on product availability.

Also, it is containerized using Docker for easy deployment and scalability. 
Please reffer to the [Dockerfile](ProductService/Dockerfile) file code that contains documented step by step configuration to containerize the Product service.

For detaild documeentation for ProductService implementation please refer to the [StepByStepImplementation.md](ProductService/Documentation/StepByStepImplementation.md) file.

# OrderService Microservice Documentation
OrderService microservice is a RESTful API that subscribes to ProductService events to demonstrate communication between microservices using Dapr. 
It provides a Swagger UI for easy API exploration and testing.

OrderService is also containerized using Docker for easy deployment and scalability.
Please refer to the [Dockerfile](OrderService/Dockerfile) file code that contains documented step by step configuration to containerize the Order service.

For detailed documentation for OrderService implementation please refer to the [StepByStepImplementation.md](OrderService/Documentation/StepByStepImplementation.md) file.


## Deployment 
##### 1. Login to azure
```
az login --tenant YOUR_TENANT_ID_
```
##### 2 Create the resource group
```
az group create --name introspect-1-b --location westeurope
```
##### 3. Create the ACR registry
```
az acr create --resource-group introspect-1-b --name introspect1bacr --sku Basic
```

##### 5 Get the ACR login server name
```
az acr show --name introspect1bacr --query loginServer --output table
```

##### 5. Login to the ACR registry
```
az acr login --name introspect1bacr
```
##### 6. Tag the Docker image
```
docker tag productservice introspect1bacr.azurecr.io/productservice:latest
```
##### 7. Push the Docker image to ACR
```
docker push introspect1bacr.azurecr.io/productservice:latest
```

## Dapr Setup
1. Ensure you have Dapr installed and initialized on your machine. You can follow the [Dapr installation guide](https://docs.dapr.io/getting-started/) for instructions.
1. Open a terminal or command prompt
1. Install Dapr CLI if you haven't already. You can download it from the [Dapr CLI installation page](https://docs.dapr.io/getting-started/install-dapr-cli/).
1. Initialize Dapr in your local environment by running the following command:
   ```
   dapr init
   ```
This command sets up the necessary components for Dapr to run locally, including a Redis state store and Pub/Sub component. 
Check your docker containers to ensure that Dapr components are running correctly.
  ![Blob Accessibility](Documentation/Images/DockerDapperContainers.jpg "Docker containers for dapr")

# Order Microservice Documentation
OrderService microservice is a RESTful API that provides order-related functionalities.
Order service Subscribes to ProductService events to manage orders based on product availability.

Also, it is containerized using Docker for easy deployment and scalability. Please reffer to the [Dockerfile](OrderService/Dockerfile) file code that contains documented step by step configuration to containerize the Order service.

## Containerization 
This chapter outlines the steps to containerize the OrderService API using Docker. The process includes building the Docker image, creating a self-signed certificate for HTTPS, and running the container with the necessary environment variables.
Open a terminal under solution folder and navigate to the OrderService project directory. The following steps will guide you through the containerization process:
##### 1. Build Immage
```
docker build -t orderservice:latest .
```

##### 2. Run the Container using the same certificate created for ProductService

```
docker run -it --rm -p 8021:8021 -p 8020:8020 `
  -e "ASPNETCORE_URLS=https://+:8021;http://+:8020" `
  -e "ASPNETCORE_HTTPS_PORTS=8021" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/productservice.pfx" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Password=runapifromdocker" `
  -v "${HOME}\.aspnet\https\productservice.pfx:/https/productservice.pfx" `
  orderservice
```

### Accessing the Product Service
You can access the Product Service API at the following URL:
```
https://localhost:8021/swagger/index.html
http://localhost:8020/swagger/index.html
```

## Deployment
##### 1. Login to azure
```
az login --tenant YOUR_TENANT_ID_
```
##### 2 Create the resource group
```
az group create --name introspect-1-b --location westeurope
```
##### 3. Create the ACR registry
```
az acr create --resource-group introspect-1-b --name introspect1bacr --sku Basic
```

##### 5 Get the ACR login server name
```
az acr show --name introspect1bacr --query loginServer --output table
```

##### 5. Login to the ACR registry
```
az acr login --name introspect1bacr
```
##### 6. Tag the Docker image
```
docker tag productservice introspect1bacr.azurecr.io/productservice:latest
```
##### 7. Push the Docker image to ACR
```
docker push introspect1bacr.azurecr.io/productservice:latest
```