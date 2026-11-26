# parcial 2 integrador - desarrollo de aplicaciones moviles II

este proyecto es una continuacion del que realice para el primer parcial de la materia.

la aplicacion permite consultar marcas de autos y sus modelos mediante una API publica. para este segundo parcial agregue la posibilidad de guardar marcas favoritas y utilizar distintas funciones del dispositivo.

tambien hice algunos cambios en la organizacion del codigo e incorpore pruebas y herramientas que vimos durante la cursada.

## cosas que tiene la app

- consumo de una API REST
- carga y busqueda de marcas de autos
- listado con CollectionView
- actualizacion de la lista deslizando hacia abajo
- navegacion entre pantallas con Shell
- carga de modelos segun la marca elegida
- guardado y eliminacion de favoritos
- almacenamiento local con SQLite
- captura de fotos desde la camara
- guardado de la ubicacion
- actualizacion de marcas al agitar el celular
- vibracion para confirmar algunas acciones
- manejo de errores y permisos
- data binding y commands
- dependency injection

## api

sigo utilizando la API publica vPIC de NHTSA para obtener las marcas y los modelos de los autos.

las consultas se realizan con HttpClient y las respuestas en formato JSON se convierten a objetos de C#.

## organizacion

para este parcial dividi la solucion en tres proyectos:

- Parcial1: contiene las pantallas, los ViewModels, el consumo de la API y las funciones del dispositivo.
- Parcial1.Core: contiene los modelos de favoritos, servicios, repositorios y validaciones.
- Parcial1.Tests: contiene las pruebas unitarias y de integracion.

mantengo el patron MVVM del primer parcial y agregue Repository Pattern para separar el acceso a la base de datos de la logica del programa.

## funcionamiento

al entrar a la aplicacion se pueden cargar las marcas, buscarlas y seleccionar alguna para ver sus modelos.

desde el detalle de una marca se puede guardarla como favorita. en la nueva pantalla de favoritos se pueden consultar las marcas guardadas, eliminarlas, tomar una foto o guardar la ubicacion actual.

tambien se puede actualizar el listado de marcas agitando el celular.

los favoritos se guardan en SQLite por lo que no se pierden al cerrar la aplicacion.

## cambios respecto al primer parcial

- separe parte de la logica en un proyecto independiente
- centralice el manejo de errores de la API
- agregue validaciones en una clase aparte
- reemplace Frame por Border
- mejore la organizacion del codigo y de las dependencias

## testing

realice 50 casos de prueba utilizando xUnit, Moq y SQLite In-Memory.

con estas pruebas pude comprobar las validaciones, el manejo de errores y las distintas operaciones sobre los favoritos sin depender de la interfaz de la app.

## github actions

configure un workflow que se ejecuta al subir cambios y realizar pull requests hacia el integrador.

este se encarga de restaurar las dependencias, compilar y ejecutar los tests en Windows y Ubuntu. tambien utiliza cache de paquetes NuGet y genera los resultados de las pruebas y el APK de Android.

## tecnologias

- .NET MAUI
- .NET 9
- C#
- XAML
- MVVM
- HttpClient
- System.Text.Json
- SQLite
- xUnit
- Moq
- Git y GitHub
- GitHub Actions

## como ejecutar

clonar el repositorio, seleccionar la rama parcial2-integrador y abrir Parcial1.slnx desde Visual Studio con .NET MAUI instalado.

la aplicacion se puede ejecutar en Windows o Android. las funciones de hardware necesitan un dispositivo compatible.

para ejecutar los tests:

dotnet test Parcial1.Tests/Parcial1.Tests.csproj

## integrante

- Juan Bautista Mareco - 152413
