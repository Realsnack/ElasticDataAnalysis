# ElasticDataAnalysis

## About aplication
The application was built on .Net Core 3.1 for use with Elasticsearch NEST client. The application Queries data from one Elastic index and searches for relevant data from other Elastic index. The data that are queried and processed are availible from REST api for the Frontend.

FrontEnd use some basic views for transaction analysis and error analysis. 

## Used Software
At the time of developement was used:
- .Net Core - version 3.1.101
- NEST elasticsearch .Net client - version 7.6.1

## Services
### Backend
Backend uses NEST client to connect and query data from elasticsearch and then provides them through REST api.
Backend also implements Swagger and SwaggerUI for testing purpose. 

Available methods:
- GET /api/elastic/transaction
- GET /api/elastic/error

### Frontend
Frontend part was created as .Net core MVC web app. Application uses
