# parcial 1 - desarrollo de aplicaciones moviles II

este proyecto fue realizado para el primer parcial de la materia.

la aplicacion consume una API publica de vehiculos y permite cargar distintas marcas de autos. al seleccionar una marca se puede entrar a otra pantalla donde se muestran algunos datos de la marca y sus modelos disponibles.

usamos MVVM para separar la interfaz de la logica del programa.

## cosas que tiene la app

- consumo de una API REST
- carga de marcas de autos
- listado con CollectionView
- busqueda de marcas
- actualizacion de la lista deslizando hacia abajo
- navegacion entre pantallas con Shell
- paso de datos entre pantallas
- carga de modelos segun la marca elegida
- mensajes de carga y de error
- manejo de errores de conexion y errores HTTP
- data binding y commands
- dependency injection

## api

para obtener los datos usamos la API publica vPIC de NHTSA.

con una consulta obtenemos las marcas de autos y con otra obtenemos los modelos de la marca seleccionada.

las respuestas de la API llegan en formato JSON y se convierten a objetos de C# usando System.Text.Json.

## organizacion

el proyecto esta separado principalmente en:

- models
- services
- viewmodels
- views

tambien use una interfaz `IApiService` para manejar las consultas a la API.

la mayor parte de la logica se encuentra en los ViewModels para evitar poner codigo importante dentro de las paginas.

## funcionamiento

al iniciar la aplicacion se puede tocar el boton para cargar las marcas.

despues se pueden buscar marcas desde el buscador o actualizar la lista deslizando hacia abajo.

al seleccionar una marca se abre una pantalla de detalle donde se muestra el nombre, el tipo de vehiculo, el id y una lista de modelos.

## tecnologias

- .NET MAUI
- .NET 9
- C#
- XAML
- MVVM
- HttpClient
- System.Text.Json
- Git y GitHub

## integrante

- Juan Bautista Mareco
