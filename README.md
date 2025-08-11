# Introspect1B
Contains the next stucture
1. Product Service Microservice
1. Order Service Microservice


## Product Overview
Product service microservice

### Containerization 

ProductService API Containerisation Steps
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