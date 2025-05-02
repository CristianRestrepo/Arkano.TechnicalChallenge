<b>Proyecto Arkano.challenge</b>

<b>Detalles del proyecto</b>
<div>
 <p>El proyecto <b>Arkano.Transaction.Api</b> es un Web API implementado con clean architecture y el patrón CQRS. Se encarga de creación de las transacciones en base de datos en un estado <b>Pending</b> que posteriormente son enviadas al topico <b>Transactions</b> del bus. Cuenta con un <b>BackgroudService</b> implementado en la capa de infraestructura que se encarga de consumir y procesar los mensajes entregados al topico <b>Processed-Transactions</b> desde el componente <b>Arkano.Antifraud.Worker</b> y actualizar la transacción en base de datos
</div>
<div>
  <p>El proyecto <b>Arkano.Antifraud.Worker</b> es un worker que implementa la interfaz IHostedService, también implementado con clean architecture y el patrón CQRS. Este proyecto contiene un backgroundService que consume el topico <b>Transactions</b> del bus y procesa la lógica de validación sobre las transacciones creadas en el proyecto <b>Arkano.Transaction.Api</b> y determina si se rechazan o se aprueban, el resultado de esta validación es enviado al topico <b>Processed-Transactions</b> para que el proyecto <b>Arkano.Transaction.Api</b> actualice las transacciones en base de datos.</p>
</div>

<b>Arquitectura del proyecto</b>

<img>![Arquitectura drawio](https://github.com/user-attachments/assets/ff3b0ece-2542-4c2d-80f5-792326ef71f0)</img>


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
  






