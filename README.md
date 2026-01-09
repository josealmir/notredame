
## Notredame API

### Instrodução
#### Build image docker
```bash  
  ➜ notredame: docker build -t notredame-api . 
```
#### Run image
 ``` bash 
  ➜ docker run --env-file .env -p 80:80 notredame-api
 ```
 #### Access Api
  - locahost:
  http://localhost:5010/scalar
  - web
  https://notredame-api.onrender.com/scalar/

#### Used in App 


| Used in development                | Description             |
| -----------------------------------| ------------------------|
| .NET 10                            | DOTNET                  |
| Scalar Doc Api                     | Documentation API       |
| OpenTelemetry                      | Metrics / Trace / Logs  |
| OpenTelemetry Collector            | Manager Metrics         |
| Version Api                        | Asp.Versioning          |
| Clean Architecture                 | Architecture            |
| Manage Package Versions Centrally  | Manager Metrics         |
| Test Unit and Integration          | XUnit                   |
| ProblemDetail Patterne             | RFC                     |
