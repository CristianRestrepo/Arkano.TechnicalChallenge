<b>Proyecto Arkano.challenge</b>

<b>Prerequisitos:</b>
- Docker
- Net 8.0
- Visual Studio 2022 o Visual Studio Code

<b>Pasos para levantar el ambiente:</b>
- Clonar repositorio.
- Desde un terminal acceder a la ruta donde se haya clonado el proyecto.
- Ejecutar el siguiente comando para levantar entorno de kafka y base de datos postgres:
  - <b>docker-compose up -d</b>
- Ejecutar los siguientes comandos para crear los topicos:
  - <b>docker-compose exec kafka kafka-topics --bootstrap-server kafka:9092 --create --topic Transactions</b>
  - <b>docker-compose exec kafka kafka-topics --bootstrap-server kafka:9092 --create --topic Processed-Transactions</b>
- Ejecutar el siguiente comando para ejecutar migraciones y crear base de datos:
  - <b>dotnet ef database update --project Arkano.Transaction.Infrastructure --startup-project Arkano.Transaction.Api</b>
 
- Abrir una segunda terminal y acceder a la ruta del proyecto:
  - En una terminal ejecutar el comando: <b>dotnet run --project Arkano.Transaction.Api</b>
  - En la otra terminal ejecutar el comando: <b>dotnet run --project Arkano.Antifraud.Worker</b>

- Si se desea acceder al swagger del proyecto ingresar al navegador a la URL: <b>http://localhost:5158/swagger/index.html</b>
  
<b>Arquitectura del proyecto</b>

<img>![Arquitectura Basica drawio](https://github.com/user-attachments/assets/17b7f192-35cd-46c1-bcbd-a612d4d22104)</img>


<b>Detalles del proyecto</b>
<div>
  <p>El proyecto <b>Arkano.Transaction.Api</b> es un Web API implementado con Clean Arquitecture y el patrón CQRS. Cuenta con un backgroundService en la capa de infraestructura que sirve como consumidor del bus.</p>
</div>
<div>
  <p>El proyecto <b>Arkano.Antifraud.Worker</b> es un worker que implementa la interfaz IHostedService, tambien implementado con Clean Arquitecture y el patrón CQRS.</p> 
</div>

