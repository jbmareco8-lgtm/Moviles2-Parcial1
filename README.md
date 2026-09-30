# parcial 1 - desarrollo de aplicaciones moviles II

este proyecto fue realizado para el primer parcial de la materia desarrollo de aplicaciones moviles II.

la aplicacion fue desarrollada con .NET MAUI y tiene como objetivo aplicar los temas vistos en clase, principalmente MVVM, consumo de APIs REST, data binding, comandos, manejo de errores y navegacion entre pantallas.

## de que trata el proyecto

para el proyecto elegimos trabajar con informacion de marcas y modelos de autos.

la aplicacion consume la API publica vPIC de la NHTSA. desde la pantalla principal se pueden cargar distintas marcas de vehiculos y, al seleccionar una, se navega hacia una segunda pantalla donde se muestra informacion de la marca y sus modelos disponibles.

la idea fue hacer una aplicacion simple, pero que nos permita aplicar de forma practica los distintos temas vistos durante la materia.

## tecnologias que usamos

- .NET 9
- .NET MAUI
- C#
- XAML
- MVVM
- HttpClient
- System.Text.Json
- API REST
- Git
- GitHub

## api utilizada

para obtener los datos usamos la API publica vPIC de la NHTSA.

la aplicacion realiza principalmente dos consultas:

- obtener marcas de autos
- obtener los modelos correspondientes a una marca seleccionada

los datos recibidos por la API vienen en formato JSON y despues se deserializan a objetos de C# para poder usarlos dentro de la aplicacion.

## como organizamos el proyecto

el proyecto esta organizado siguiendo el patron MVVM.

### models

los models representan los datos que recibimos desde la API.

creamos las clases:

- `CarMake`
- `CarMakeResponse`
- `CarModel`
- `CarModelResponse`

estas clases nos permiten transformar la informacion del JSON en objetos que podemos manejar desde C#.

### services

en esta parte dejamos la logica relacionada con el consumo de la API.

`IApiService` define las operaciones que puede realizar el servicio y `ApiService` contiene la implementacion usando `HttpClient`.

de esta manera evitamos tener la logica de conexion con la API directamente dentro de las pantallas.

### viewmodels

los viewmodels contienen la logica relacionada con cada pantalla.

`MainViewModel` se encarga de cargar y almacenar las marcas.

`ModelsViewModel` recibe los datos de la marca seleccionada y consulta sus modelos.

tambien usamos un `BaseViewModel` que implementa `INotifyPropertyChanged`, lo que permite actualizar la interfaz cuando cambia alguna propiedad.

### views

la interfaz esta hecha con XAML.

la pantalla principal tiene el boton para cargar marcas y un `CollectionView` para mostrar los resultados.

la segunda pantalla muestra los datos de la marca seleccionada y los modelos relacionados con esa marca.

las views se conectan con los viewmodels mediante data binding.

## funciones principales

la aplicacion permite:

- cargar marcas desde una API publica
- mostrar los resultados en un `CollectionView`
- mostrar mensajes sobre el estado de la consulta
- seleccionar una marca
- navegar hacia otra pantalla usando Shell
- pasar datos entre pantallas
- mostrar el nombre, tipo de vehiculo e ID de la marca
- consultar los modelos correspondientes a esa marca
- manejar distintos tipos de errores

## manejo de errores

agregamos manejo de errores para que la aplicacion no falle directamente si ocurre algun problema al consultar la API.

diferenciamos:

- problemas de conexion o falta de internet
- errores HTTP
- tiempo de espera excedido
- problemas al interpretar el JSON
- errores inesperados

en cada caso se muestra un mensaje al usuario desde el viewmodel.

## navegacion

para la navegacion usamos Shell.

cuando se selecciona una marca desde la pantalla principal se ejecuta un comando en `MainViewModel`.

desde ahi usamos `GoToAsync` para navegar hacia `ModelsPage`.

durante la navegacion enviamos:

- ID de la marca
- nombre de la marca
- tipo de vehiculo

estos datos se reciben en `ModelsViewModel` mediante `IQueryAttributable`.

despues de recibirlos se hace una nueva consulta a la API para obtener los modelos de esa marca.

## dependency injection

las dependencias principales estan registradas en `MauiProgram.cs`.

los viewmodels reciben `IApiService` mediante el constructor y MAUI se encarga de proporcionar la implementacion correspondiente.

tambien configuramos `HttpClient` usando `AddHttpClient`.

## como fuimos armando el proyecto

primero creamos el proyecto base en .NET MAUI y configuramos Git y GitHub.

despues creamos los models necesarios para representar la informacion de la API y armamos el servicio encargado de hacer las consultas.

cuando eso ya funcionaba agregamos los viewmodels, los comandos, el data binding y el listado de marcas.

despues sumamos la navegacion hacia una segunda pantalla, el paso de parametros y una nueva consulta para obtener los modelos de la marca seleccionada.

por ultimo mejoramos el manejo de errores y la informacion que se muestra en la pantalla de detalle.

para ir organizando los cambios usamos distintas ramas de Git y despues las integramos a `main` mediante pull requests.

## como ejecutar el proyecto

para ejecutar el proyecto hay que tener Visual Studio con .NET MAUI y .NET 9.

despues:

1. clonar el repositorio
2. abrir `Parcial1.slnx`
3. esperar a que Visual Studio restaure las dependencias
4. seleccionar `Windows Machine`
5. ejecutar el proyecto

## integrantes

- Juan Bautista Mareco - 152413
- Francisco Pollet - 151464
- Matias Nicolas Bozzone - 151843