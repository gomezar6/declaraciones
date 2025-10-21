# DEVOPS
## Repositorio webapp
Es de suma importancia mantener el c�digo de DevOps actualizado, por lo cu�l se define la siguiente estrategia:
1. Rama __main__, se utiliza para el c�digo final. Esta rama se utilizar� para el c�digo probado y listo para producci�n.
1. Rama __dev__, se utiliza para el c�digo de desarrollo. Esta rama se utilizar� para pruebas.
1. Rama __feature/id de tarea-descripci�n separada por gui�n__ (ej. feature/100-listado-de-declaraciones)

El flujo de trabajo consiste en crear una rama __feature__ basada en la rama __dev__, una vez completada la tarea se debe iniciar un *pull request* hacia la rama __dev__. Una vez aprobado el c�digo de la rama __dev__ se procede a realizar un *pull request* hacia la rama __main__, esta rama recibir� una etiqueta con n�mero de versi�n y se generar�n los elementos para producci�n.

# Datos
## Patr�n de desarrollo
Se utilizar� desarrollo basado en Patr�n de Repositorio (Repository Pattern) para referencia [Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application (9 of 10)](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application).

# Estructura
La soluci�n se divide en cuantro proyectos:
## EIGO.PDLA.Common
Este proyecto debe contener todos los elementos comunes, tipos de datos, extensiones, utilidades, etc.
## EIGO.PDLA.WebAPI
Este proyecto contiene los elementos de la **API** que permite el acceso a los datos.
## EIGO.PDLA.WebAPP
Este proyecto contiene la aplicaci�n Web **MVC**.
## EIGO.PDLA.Tests
Este proyecto contiene las pruebas. Es necesario contar con al menos 70% de cobertura de c�digo en las pruebas.

# Control de versiones
## 1.0 Primera publicación
## 2.0 Segunda publicación
### Crear un tipo de Declaración Estándar
### Fecha de Finalizalizmación de un proceso que sea Opcional/Editable
### Reabrir un proceso que fue cerrado (Estatus/Fecha)
### Cambiar la fecha de cierre del actual "Proceso de Declaraciones 2024"
## 2.1 Tercera publicación
### Cambiar “Nombre Completo del Funcionario o Familiar” por “Nombre Completo de la Persona Funcionaria o Familiar”
### Cambiar “Nombre de la Empresa” por “Nombre de la Entidad” 
### Dividir la segunda tabla de Participaciones en 2 tablas
### Se añade sección con “Usted identifica algun conflicto de interes?” y justificación 
### El campo cargo tiene una modificación para poder llenarlo por un listado