# UniversityAdmissions

<img width="1833" height="853" alt="image" src="https://github.com/user-attachments/assets/714677d2-4a8c-40e7-bc93-a445cdb81074" />
<img width="1228" height="901" alt="image" src="https://github.com/user-attachments/assets/071b45f9-235c-4d9b-80c1-e892f2fe325e" />
<img width="1228" height="853" alt="image" src="https://github.com/user-attachments/assets/90e6e87d-c294-4491-8d7e-febfeb673a3b" />

**Sistema de Gestión de Admisiones Universitarias** desarrollado con **ASP.NET Core 8, Blazor Server, SQL Server y Entity Framework Core**, siguiendo una arquitectura en capas inspirada en Clean Architecture.

La aplicación centraliza la recepción de solicitudes de aspirantes, el almacenamiento de documentos, la revisión administrativa y la comunicación del estado de cada solicitud. Incluye una interfaz pública para estudiantes y un área administrativa protegida por autenticación y autorización basada en roles.

> **Alcance de esta documentación:** se basa en el código fuente compartido y en los módulos posteriores de administración y consulta pública desarrollados para el proyecto. Algunas funciones descritas requieren haber integrado los paquetes de actualización correspondientes. Los procedimientos de instalación y despliegue son orientativos y deben adaptarse al entorno.

## Contenido

1. [Características](#características)
2. [Tecnologías](#tecnologías)
3. [Arquitectura y estructura](#arquitectura-y-estructura)
4. [Flujo de trabajo](#flujo-de-trabajo)
5. [Estados de las solicitudes](#estados-de-las-solicitudes)
6. [Roles y permisos](#roles-y-permisos)
7. [Requisitos previos](#requisitos-previos)
8. [Instalación y configuración](#instalación-y-configuración)
9. [Base de datos y migraciones](#base-de-datos-y-migraciones)
10. [Ejecución local](#ejecución-local)
11. [Rutas principales](#rutas-principales)
12. [Documentos adjuntos](#documentos-adjuntos)
13. [Seguridad](#seguridad)
14. [Despliegue](#despliegue)
15. [Solución de problemas](#solución-de-problemas)
16. [Evolución recomendada](#evolución-recomendada)

## Características

### Portal público del aspirante

- Página de inicio con acceso al formulario de admisión y a la consulta de estado
- Formulario de solicitud con nombres, apellidos, fecha de nacimiento, género, tipo y número de identificación y teléfono
- Cálculo de edad a partir de la fecha de nacimiento
- Validaciones de campos obligatorios y formatos de identificación y teléfono
- Consentimiento de términos y condiciones
- Carga obligatoria de fotografía 2×2 y documento de identidad
- Confirmación con número de solicitud tras completar el registro
- Consulta pública del estado mediante número de solicitud y número de identificación, cuando está integrado el módulo de seguimiento

### Administración

- Inicio y cierre de sesión con ASP.NET Core Identity
- Listado de solicitudes con búsqueda, filtros por estado y paginación
- Consulta del detalle del aspirante y documentos adjuntos
- Descarga de documentos con autorización
- Transiciones de estado sujetas a permisos: pasar a revisión, aprobar y rechazar
- Observaciones obligatorias al rechazar
- Registro de decisiones en la entidad `AdmissionDecision`
- Confirmación de acciones mediante JavaScript/SweetAlert2

El alcance actual **no incluye un dashboard ni un módulo de gestión de usuarios en la interfaz**, aunque existe una política `Users.Manage` y contratos relacionados con usuarios en la capa Application.

## Tecnologías

| Área | Tecnología | Uso |
|---|---|---|
| Backend y UI | .NET 8 / ASP.NET Core | Aplicación web y servicios |
| Interfaz | Blazor Server / Razor Components | Pantallas públicas y administrativas |
| Lenguaje | C# | Lógica de aplicación |
| Persistencia | SQL Server | Base de datos relacional |
| ORM | Entity Framework Core 8 | Consultas, relaciones y migraciones |
| Identidad | ASP.NET Core Identity | Usuarios, roles y autenticación por cookies |
| Estilos | Tailwind CSS 3.4.17 | Diseño responsivo |
| Frontend complementario | JavaScript | Máscaras, alertas y descarga de documentos |
| Gestión de dependencias | NuGet y npm | Paquetes .NET y herramientas de estilos |

El archivo `package.json` de la solución original declara `tailwindcss` versión `3.4.17` como dependencia de desarrollo. No declara scripts npm para compilar los estilos.

## Arquitectura y estructura

La solución separa entidades de negocio, casos de uso, infraestructura y presentación:

```text
UniversityAdmissions/
├── UniversityAdmissions.sln
├── UniversityAdmissions.Domain/
│   ├── Entities/
│   │   ├── Admission.cs
│   │   ├── AdmissionDecision.cs
│   │   ├── AdmissionDocument.cs
│   │   ├── AdmissionStatus.cs
│   │   ├── Applicant.cs
│   │   ├── DocumentType.cs
│   │   ├── Gender.cs
│   │   └── IdentificationType.cs
│   └── Exceptions/
├── UniversityAdmissions.Application/
│   ├── DTOs/
│   │   ├── Admissions/
│   │   ├── Applicants/
│   │   ├── Catalogs/
│   │   ├── Documents/
│   │   └── Security/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── Storage/
│   ├── Responses/
│   ├── Security/
│   └── Services/
├── UniversityAdmissions.Infrastructure/
│   ├── Identity/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/
│   └── Storage/
├── UniversityAdmissions/
│   ├── Components/
│   │   ├── Layout/
│   │   ├── Pages/
│   │   │   ├── Account/
│   │   │   ├── Administration/
│   │   │   └── Admissions/
│   │   └── Routes.razor
│   ├── Endpoints/
│   ├── Shared/
│   ├── Styles/
│   ├── wwwroot/
│   │   ├── css/
│   │   └── js/
│   ├── Program.cs
│   └── appsettings.json
├── package.json
└── package-lock.json
```

**Domain:** contiene las entidades y reglas de dominio, sin dependencia de la interfaz de usuario.

**Application:** define DTO, contratos de repositorios y servicios, respuestas y operaciones como registrar, consultar y cambiar el estado de solicitudes.

**Infrastructure:** implementa acceso a SQL Server con EF Core, repositorios, almacenamiento privado de archivos y ASP.NET Core Identity.

**UniversityAdmissions (web):** contiene páginas Blazor, layouts, endpoints de autenticación, recursos estáticos y configuración de la aplicación.

### Entidades principales

| Entidad | Propósito |
|---|---|
| `Applicant` | Datos personales y de contacto del aspirante |
| `Admission` | Solicitud y referencia a su estado actual |
| `AdmissionStatus` | Catálogo de estados |
| `AdmissionDocument` | Metadatos de documentos vinculados a la solicitud |
| `DocumentType` | Clasificación de documentos |
| `AdmissionDecision` | Historial de cambios o decisiones administrativas |
| `Gender` | Catálogo de géneros |
| `IdentificationType` | Catálogo de tipos de identificación |
| `ApplicationUser` / `ApplicationRole` | Usuarios y roles de Identity |

## Flujo de trabajo

### Registro de una solicitud

1. El aspirante accede a `/admissions/create` desde la página de inicio
2. Completa su información personal, identificación y teléfono
3. Adjunta la fotografía y el documento de identidad
4. Acepta los términos del proceso
5. El sistema valida los datos y registra al aspirante
6. Se crea la solicitud con estado inicial **Pendiente**
7. Los documentos se almacenan y se asocian con la solicitud
8. Se muestra el número de solicitud

El formulario implementa un mecanismo de reintento para documentos pendientes si la solicitud ya fue creada; así evita registrar otra solicitud durante ese reintento dentro del mismo flujo de pantalla.

### Revisión administrativa

1. Un usuario autorizado inicia sesión en `/admin/login`
2. Accede a `/admin/admissions`
3. Busca la solicitud y abre sus detalles
4. Examina la información y descarga documentos cuando tiene permiso
5. Pasa una solicitud pendiente a **En revisión**
6. Un usuario con permiso de decisión puede **Aprobar** o **Rechazar**
7. Se registra la decisión y se actualiza el estado

### Seguimiento del estudiante

Cuando está integrado el módulo público de consulta:

1. El estudiante abre `/admissions/status`
2. Introduce su número de solicitud y la identificación registrada
3. El sistema verifica la correspondencia de ambos datos
4. Muestra el número, la fecha, el estado actual y un mensaje de seguimiento

La consulta pública no debe exponer documentos, datos personales completos ni operaciones administrativas.

## Estados de las solicitudes

El catálogo inicial está definido mediante datos semilla en `ApplicationDbContext`:

| ID | Estado | Significado |
|---|---|---|
| 1 | Pendiente | Solicitud recibida, pendiente de revisión |
| 2 | En revisión | El personal está evaluando la solicitud |
| 3 | Aprobada | Decisión favorable |
| 4 | Rechazada | Decisión desfavorable |

**Transiciones contempladas:**

```text
Pendiente ──> En revisión ──> Aprobada
                          └─> Rechazada
```

No se contempla en el flujo mostrado la edición libre de cualquier estado ni el regreso automático de una decisión final a un estado anterior.

## Roles y permisos

El proyecto define los roles `Administrador`, `OficialAdmisiones` y `Aprobador`. Las políticas configuradas en `Program.cs` son:

| Política | Administrador | OficialAdmisiones | Aprobador |
|---|:---:|:---:|:---:|
| `Admissions.View` | Sí | Sí | Sí |
| `Admissions.Review` | Sí | Sí | No |
| `Admissions.Approve` | Sí | No | Sí |
| `Admissions.Reject` | Sí | No | Sí |
| `Documents.View` | Sí | Sí | Sí |
| `Users.Manage` | Sí | No | No |

La interfaz administrativa usa políticas de autorización para controlar la visibilidad y ejecución de acciones. **La autorización debe verificarse también en el límite de escritura del servidor** para impedir cambios no autorizados por vías distintas a la interfaz.

## Requisitos previos

- Windows, Linux o macOS con **SDK de .NET 8** instalado
- **SQL Server** accesible desde el entorno de ejecución
- **Node.js y npm** para recompilar Tailwind CSS
- Visual Studio 2022 compatible con .NET 8, Visual Studio Code o una terminal
- Credenciales con permisos para crear o actualizar la base de datos
- Una carpeta de almacenamiento con permisos de lectura y escritura para la cuenta que ejecuta la aplicación

Comprobar las herramientas:

```bash
dotnet --version
node --version
npm --version
```

## Instalación y configuración

### 1. Obtener el proyecto

```bash
git clone <URL_DEL_REPOSITORIO>
cd UniversityAdmissions
```

Si ya tienes el repositorio en tu equipo, omite este paso.

### 2. Restaurar dependencias .NET

```bash
dotnet restore UniversityAdmissions.sln
```

### 3. Instalar las dependencias de frontend

```bash
npm ci
```

Si Windows PowerShell bloquea `npm.ps1`, puedes ejecutar `npm.cmd ci` o usar CMD, sin modificar la política de seguridad del equipo.

### 4. Configurar la aplicación

La aplicación utiliza estas claves de configuración:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=UniversityAdmissionsDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "FileStorage": {
    "RootPath": "C:\\UniversityAdmissions\\PrivateFiles"
  },
  "AdminUser": {
    "Email": "admin@example.com",
    "Password": "CONFIGURAR_MEDIANTE_USER_SECRETS",
    "FirstName": "Administrador",
    "LastName": "Sistema"
  },
  "Admissions": {
    "DocumentTypes": {
      "Photo": 1,
      "Identity": 2
    }
  }
}
```

**Este JSON es un ejemplo de referencia, no un archivo listo para producción.** Cambia el servidor, el nombre de la base de datos, la ruta de archivos y el correo según tu entorno. No utilices literalmente el valor de ejemplo de la contraseña.

En desarrollo, configura la contraseña del administrador mediante **User Secrets** desde la carpeta del proyecto web:

```bash
cd UniversityAdmissions
dotnet user-secrets init
dotnet user-secrets set "AdminUser:Password" "<CONTRASEÑA_SEGURA>"
cd ..
```

También puedes configurar `ConnectionStrings:DefaultConnection` con User Secrets si contiene credenciales. **Nunca publiques contraseñas reales en `appsettings.json` ni en Git.** Si una contraseña ya fue compartida o versionada, cámbiala y revisa el historial del repositorio.

El proyecto incluye un inicializador `IdentitySeeder` que crea los roles y, si están configurados `AdminUser:Email` y `AdminUser:Password`, crea el administrador inicial o le asigna el rol correspondiente. La política de contraseña requiere al menos **10 caracteres**, mayúscula, minúscula, dígito y carácter especial.

### 5. Configurar el almacenamiento privado

La implementación `LocalFileStorage` utiliza `FileStorage:RootPath`. En Windows puedes crear la carpeta de ejemplo:

```cmd
mkdir C:\UniversityAdmissions\PrivateFiles
```

La cuenta del proceso de ASP.NET Core o del Application Pool de IIS debe tener permisos apropiados. Es recomendable mantener esta carpeta **fuera de `wwwroot`** para que los documentos no sean accesibles directamente por URL.

## Base de datos y migraciones

La persistencia se implementa con `ApplicationDbContext`, EF Core y SQL Server. El proyecto incluye migraciones para la creación inicial y la incorporación de Identity.

Desde la raíz de la solución, después de configurar la conexión, aplica las migraciones con:

```bash
dotnet ef database update --project UniversityAdmissions.Infrastructure --startup-project UniversityAdmissions
```

Si no tienes instalada la herramienta `dotnet-ef`, instálala con una versión compatible con EF Core 8:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

Si ya está instalada, revisa su versión antes de actualizarla. El comando de migración debe ejecutarse desde la raíz de la solución y requiere acceso al servidor SQL configurado.

### Catálogos iniciales

`ApplicationDbContext` incluye datos semilla para:

- Géneros: Masculino y Femenino
- Tipos de identificación: Cédula y Pasaporte
- Estados de admisión: Pendiente, En revisión, Aprobada y Rechazada
- Tipos de documentos: Fotografía, Identificación, Acta de nacimiento, Certificado de estudios y Otro documento

Los tipos obligatorios del formulario están configurados mediante los IDs `1` (Fotografía) y `2` (Identificación).

**No modifiques manualmente los IDs de estos catálogos sin revisar las referencias del código y la configuración.**

## Ejecución local

### Compilar los estilos

El proyecto original utiliza Tailwind CSS v3 con las directivas de `Styles/input.css`:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

El archivo compilado de estilos se encuentra en `UniversityAdmissions/wwwroot/css/tailwind.css`. Si necesitas regenerarlo, puedes utilizar un comando equivalente a:

```bash
npx tailwindcss -i ./UniversityAdmissions/Styles/input.css -o ./UniversityAdmissions/wwwroot/css/tailwind.css --content "./UniversityAdmissions/**/*.{razor,html,cshtml}" --minify
```

El comando es una opción de compilación manual; el `package.json` original no define un script `build:css`. Ajusta las rutas si modificas la estructura.

### Iniciar la aplicación

```bash
dotnet run --project UniversityAdmissions
```

Abre la URL HTTPS indicada en la consola. En desarrollo, Visual Studio puede usar un puerto local específico; **no asumas que será siempre el mismo**.

Para iniciar desde Visual Studio:

1. Abre `UniversityAdmissions.sln`
2. Selecciona el proyecto web `UniversityAdmissions` como proyecto de inicio
3. Verifica la cadena de conexión y la carpeta de documentos
4. Ejecuta con HTTPS

## Rutas principales

| Ruta | Acceso | Descripción |
|---|---|---|
| `/` | Público | Página principal |
| `/admissions/create` | Público | Formulario de admisión |
| `/admissions/status` | Público | Consulta de estado; requiere integrar el módulo de seguimiento |
| `/admin/login` | Público | Inicio de sesión administrativo |
| `/admin/admissions` | Autenticado y autorizado | Listado de solicitudes |
| `/admin/admissions/{id}` | Autenticado y autorizado | Detalle y gestión de una solicitud |
| `POST /admin/auth/login` | Público | Procesamiento del inicio de sesión |
| `POST /admin/auth/logout` | Autenticado | Cierre de sesión |

Las rutas de login/logout se implementan en `AdminAuthEndpoints`. El formulario del layout administrativo debe enviar el cierre de sesión a **`/admin/auth/logout`**, no a `/account/logout`, salvo que se configure expresamente ese endpoint alternativo.

## Documentos adjuntos

El formulario requiere:

| Documento | Formatos | Límite en el formulario |
|---|---|---|
| Fotografía 2×2 | JPG, JPEG, PNG | 5 MB |
| Documento de identidad | PDF, JPG, JPEG, PNG | 5 MB |

El servicio `DocumentService` trabaja con `IFileStorage`. La implementación local guarda los archivos con nombres internos generados mediante GUID y conserva los metadatos necesarios para asociarlos a una solicitud.

La validación de extensiones y tipos MIME en el formulario **no sustituye** una validación profunda del contenido de los archivos en el servidor. Para producción, considera antivirus, inspección de firmas, límites en servidor, cuotas de almacenamiento y políticas de retención.

## Seguridad

### Controles existentes en el código

- ASP.NET Core Identity con contraseñas robustas
- Bloqueo de cuenta tras **5 intentos fallidos** durante **15 minutos**
- Cookies `HttpOnly`, `Secure` y `SameSite=Lax`
- Políticas de autorización basadas en roles
- Protección antifalsificación (`AntiforgeryToken`) en formularios de autenticación
- Almacenamiento privado configurable fuera de los archivos públicos
- Validaciones de datos de entrada y restricciones de carga

### Medidas que deben revisarse antes de producción

1. **Autorización del cambio de estado:** validar permisos en el servidor, no solo en el componente Blazor
2. **Consulta pública:** aplicar limitación de solicitudes, monitoreo de abuso y protección contra enumeración de números de solicitud
3. **Credenciales:** utilizar variables de entorno, un almacén de secretos o un gestor de credenciales; rotar credenciales expuestas
4. **Archivos:** comprobar contenido real, tipos admitidos, tamaño y seguridad de descarga
5. **Auditoría:** conservar el historial de decisiones y evitar modificaciones no autorizadas
6. **Errores:** no revelar detalles internos, rutas físicas ni cadenas de conexión a los usuarios
7. **Infraestructura:** HTTPS, copias de seguridad, permisos mínimos y actualizaciones de seguridad
8. **Privacidad:** establecer retención, eliminación y control de acceso a documentos personales

La consulta mediante número de solicitud e identificación es una verificación básica de correspondencia; **no constituye autenticación fuerte**.

## Despliegue

### Publicación de la aplicación

```bash
dotnet publish UniversityAdmissions/UniversityAdmissions.csproj -c Release -o ./publish
```

Para desplegar en IIS sobre Windows:

1. Instala el **ASP.NET Core Hosting Bundle** compatible con .NET 8 en el servidor
2. Crea un sitio o aplicación en IIS apuntando a la carpeta publicada
3. Configura el Application Pool con **No Managed Code**
4. Configura las variables de entorno y la conexión a SQL Server
5. Otorga permisos a la carpeta privada de documentos
6. Aplica las migraciones mediante un procedimiento controlado antes de habilitar tráfico
7. Configura HTTPS y revisa los registros de aplicación e IIS
8. Verifica inicio de sesión, cierre de sesión, registro, carga, descarga y cambios de estado

**Importante:** la publicación no reemplaza la configuración de seguridad ni el aprovisionamiento de la base de datos. No coloques archivos privados en `wwwroot` ni incluyas secretos en el paquete publicado.

## Solución de problemas

### `git add .` falla por archivos `.vs`

La carpeta `.vs` contiene archivos temporales de Visual Studio. Asegúrate de tener un archivo llamado **`.gitignore`** (con punto inicial) en la raíz del repositorio:

```gitignore
.vs/
**/bin/
**/obj/
**/node_modules/
.env
.env.*
!.env.example
```

Si ya se habían agregado archivos al índice:

```bash
git rm -r --cached --ignore-unmatch .vs
git add .gitignore
git add .
```

### Error 404 al cerrar sesión

Verifica que el formulario administrativo envíe `POST` a `/admin/auth/logout` y que `app.MapAdminAuthEndpoints()` esté registrado en `Program.cs`.

### El encabezado fijo cubre contenido

Asegúrate de reservar espacio superior en el contenedor `<main>` del `AdminLayout`. El espacio debe corresponder a la altura efectiva del encabezado.

### Los botones aparecen sin colores

Comprueba que Tailwind haya compilado las clases utilizadas por los componentes Razor y que el archivo `tailwind.css` generado esté cargado por la aplicación. Revisa también estilos globales que puedan sobrescribir los botones.

### La base de datos no conecta

Comprueba `ConnectionStrings:DefaultConnection`, disponibilidad de SQL Server, credenciales, permisos y certificados. `TrustServerCertificate=True` puede servir para desarrollo local, pero no debe adoptarse sin evaluar los certificados del entorno de producción.

### No se guardan documentos

Verifica `FileStorage:RootPath`, permisos de escritura, espacio en disco y tamaño permitido. La carpeta se crea desde `LocalFileStorage` cuando la aplicación puede acceder a su ruta padre.

## Evolución recomendada

Las siguientes mejoras **no deben interpretarse como funcionalidades ya implementadas**:

- Notificaciones por correo sobre cambios de estado
- Recuperación de solicitudes incompletas entre sesiones
- Consulta pública con código de verificación temporal
- Historial de decisiones visible para administradores
- Paginación y filtros del lado del servidor para volúmenes grandes
- Auditoría centralizada, métricas y monitoreo
- Pruebas unitarias, de integración y de autorización
- Automatización CI/CD
- Almacenamiento de documentos en un servicio administrado y estrategia de copias de seguridad
- Manuales separados para aspirantes y personal de admisiones

## Contribución y mantenimiento

Para colaborar, crea una rama de trabajo, realiza cambios pequeños, comprueba la compilación y documenta cualquier modificación de base de datos mediante migraciones. No incluyas archivos `bin`, `obj`, `.vs`, `node_modules`, credenciales ni documentos personales en commits.

Este repositorio contiene información de una aplicación de admisiones; cualquier dato real de aspirantes utilizado para pruebas debe anonimizarse.

## Licencia

**No se ha especificado una licencia para este proyecto.** Antes de distribuirlo públicamente o aceptar contribuciones, define los términos de uso y agrega un archivo `LICENSE` si corresponde.

---

**UniversityAdmissions** — Sistema de Gestión de Admisiones Universitarias.
