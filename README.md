# Planificador de Horarios

API REST para planificación de horarios universitarios. Permite subir imágenes de mallas curriculares, extraer datos mediante OCR, generar todas las combinaciones válidas de horarios sin conflictos de horario, y guardar/cargar horarios personalizados.

## Características

- **OCR inteligente**: Extracción automática de materias, aulas y horarios desde imágenes usando Google Cloud Document AI.
- **Generación de horarios**: Algoritmo de backtracking que genera todas las combinaciones posibles de horarios sin conflictos de tiempo.
- **Autenticación JWT**: Registro y login de usuarios con tokens JWT para operaciones protegidas.
- **Persistencia**: Guardado y carga de horarios personalizados por usuario en PostgreSQL.
- **Docker**: Soporte completo para despliegue con Docker.

## Arquitectura

El proyecto sigue un patrón de **Arquitectura Hexagonal (Puertos y Adaptadores)** organizado en Vertical Slices:

```
PlanificadorDeHorarios/
├── Program.cs                          # Punto de entrada y composición de DI
├── PlanificadorDeHorarios.Api.csproj   # Definición del proyecto
├── appsettings.json                    # Configuración
├── Dockerfile                          # Build multi-etapa
│
├── Common/                             # Utilidades compartidas
│   ├── JwtOptions.cs                   # Modelo de configuración JWT
│   └── Result.cs                       # Monad Result<T> para manejo de errores
│
├── Domain/                             # Modelo de dominio y lógica de negocio
│   ├── Aula.cs                         # Aula (nombre + bloques de tiempo)
│   ├── BloqueHorario.cs               # Bloque horario (día, inicio, fin) + verificación de interferencia
│   ├── GeneradorDeHorarios.cs          # Generador de horarios (algoritmo de backtracking)
│   ├── Horario.cs                      # Horario completo (mapa materia → aula)
│   ├── Materia.cs                      # Materia (nombre + opciones de aula)
│   └── Usuario.cs                      # Entidad de usuario
│
├── Ports/                              # Interfaces (contratos)
│   ├── IEndpoint.cs                    # Contrato para auto-registro de endpoints
│   ├── IHandler.cs                     # Interfaz marcador para handlers
│   ├── IPasswordHelper.cs             # Helper de contraseñas
│   ├── ITokenGenerator.cs             # Generador de tokens JWT
│   ├── IOcrApiAdapter.cs              # Adaptador OCR
│   ├── IUsuarioRepositorio.cs          # Repositorio de usuarios
│   └── IHorarioRepositorio.cs          # Repositorio de horarios
│
├── Features/                           # Funcionalidades (Vertical Slices)
│   ├── HolaApi.cs                      # Health check
│   ├── Login.cs                        # Autenticación
│   ├── RegistrarUsuario.cs            # Registro de usuario
│   ├── CargarImagen.cs                # Subir imagen OCR
│   ├── GenerarHorarios.cs             # Generar combinaciones de horarios
│   ├── GuardarHorario.cs              # Guardar horario
│   └── ObtenerHorariosGuardados.cs    # Obtener horarios guardados
│
├── Infraestructure/                    # Implementaciones concretas
│   ├── PasswordHelper.cs              # Hashing de contraseñas (ASP.NET Identity)
│   ├── TokenGenerator.cs              # Generación de tokens JWT (HMAC-SHA256)
│   ├── UsuarioRepositorio.cs           # Repositorio de usuarios (PostgreSQL + Npgsql)
│   ├── HorarioRepositorio.cs           # Repositorio de horarios (PostgreSQL + JSON)
│   ├── DocumentAiApiAdapter.cs         # Integración con Google Cloud Document AI
│   └── ResponseMapper.cs              # Mapeo de respuesta OCR → modelos de dominio
│
├── Extensions/
│   └── Extensions.cs                   # Métodos de extensión para auto-registro
│
└── database/
    └── init.sql                        # Script de creación de tablas
```

### Flujo de una petición

```
HTTP Request → IEndpoint (mapeo de ruta) → IHandler (lógica de aplicación) → Ports (interfaces) → Infraestructure (implementación concreta)
```

## Tecnologías

| Tecnología | Versión | Uso |
|---|---|---|
| .NET | 9.0 | Runtime y framework |
| ASP.NET Core | 9.0 | API REST (Minimal APIs) |
| C# | 13 | Lenguaje de programación |
| PostgreSQL | — | Base de datos |
| Npgsql | 9.0.1 | Driver PostgreSQL para .NET |
| Google Cloud Document AI | 3.23.0 | OCR de imágenes |
| JWT Bearer | 9.0.0 | Autenticación por tokens |
| OpenAPI/Swagger | 9.0.10 | Documentación de la API |
| Docker | — | Contenedorización |

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) instalado y ejecutándose
- Cuenta de [Google Cloud](https://cloud.google.com/) con Document AI habilitado (opcional, solo para OCR)
- [Docker](https://docs.docker.com/get-docker/) (opcional, para despliegue en contenedor)

## Instalación y configuración

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd PlanificadorDeHorarios
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Crear la base de datos

Ejecutar el script SQL para crear las tablas necesarias:

```bash
psql -U usuario -d planificador -f database/init.sql
```

### 4. Configurar la cadena de conexión a PostgreSQL

Usando .NET User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=planificador;Username=usuario;Password=contraseña"
```

### 5. Configurar credenciales de Google Cloud (opcional)

Si deseas usar la funcionalidad de OCR:

```bash
dotnet user-secrets set "GoogleCloud:ProjectId" "tu-project-id"
dotnet user-secrets set "GoogleCloud:ProcessorId" "tu-processor-id"
dotnet user-secrets set "GoogleCloud:Location" "us"
```

También debes configurar la variable de entorno `GOOGLE_APPLICATION_CREDENTIALS` apuntando a tu archivo de credenciales JSON:

```bash
export GOOGLE_APPLICATION_CREDENTIALS="/ruta/a/tu/archivo-credenciales.json"
```

### 6. Configurar el secreto JWT

```bash
dotnet user-secrets set "Jwt:Secreto" "tu-secreto-super-seguro-aqui"
```

### 7. Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en `http://localhost:5046` (HTTP) o `https://localhost:7261` (HTTPS).

## Endpoints

| Método | Ruta | Autenticación | Descripción |
|---|---|---|---|
| `GET` | `/` | No | Health check |
| `POST` | `/register` | No | Registrar nuevo usuario |
| `POST` | `/login` | No | Autenticar usuario y obtener token JWT |
| `POST` | `/imagen/subir` | No | Subir imagen para extracción OCR |
| `POST` | `/horarios` | No | Generar combinaciones de horarios |
| `POST` | `/horarios/guardar` | Sí (Bearer) | Guardar un horario |
| `GET` | `/horarios` | Sí (Bearer) | Obtener horarios guardados del usuario |

## Docker

### Construir la imagen

```bash
docker build -t planificador-de-horarios .
```

### Ejecutar el contenedor

```bash
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__Default="Host=host.docker.internal;Port=5432;Database=planificador;Username=usuario;Password=contraseña" \
  -e Jwt__Secreto="tu-secreto-super-seguro" \
  planificador-de-horarios
```

La API estará disponible en `http://localhost:8080`.
