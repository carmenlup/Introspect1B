# Introspect1B
Introspect1B solution includes 2 microservices that are separated projects.
Solution strucure contains the following projects:
1. ProductService project
1. OrderService project

# ProductService Microservice Documentation
ProductService microservice is a RESTful API that provides product-related functionalities. It allows users to manage products, including creating, updating, retrieving, and deleting product information.
It provides a Swagger UI for easy API exploration and testing.

Also, it is containerized using Docker for easy deployment and scalability. Please reffer to the [Containerization](ProductService/Dockerfile) file code that contains documented step by step configuration to containerize the Product service.

## Containerization 
This chapter outlines the steps to containerize the ProductService API using Docker. The process includes building the Docker image, creating a self-signed certificate for HTTPS, and running the container with the necessary environment variables.

##### 1. Build Immage
```
docker build -t productservice .
```

##### 2. Create  Self-Signed Certificate
```
dotnet dev-certs https -t -ep "%USERPROFILE%\.aspnet\https\productservice.pfx" -p runapifromdocker
```
##### 3. Trust the certificate on your local machine
```
dotnet dev-certs https --trust
```

##### 4. Run the Container

```
docker run -it --rm -p 8081:8081 `
  -e "ASPNETCORE_URLS=https://+:8081;http://+:8080" `
  -e "ASPNETCORE_HTTPS_PORTS=8081" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/productservice.pfx" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Password=RunProductServiceFromDocker" `
  -v "${HOME}\.aspnet\https\productservice.pfx:/https/productservice.pfx" `
  productservice
```

### Accessing the Product Service
You can access the Product Service API at the following URL:
```
https://localhost:8081/swagger/index.html
http://localhost:8080/swagger/index.html
```