# Instalación de un paquete de plantillas

Use el comando [dotnet new --install](https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-new-install) para instalar un paquete de plantillas.

## Instalar un paquete de plantillas desde un directorio del sistema de archivos

Las plantillas se pueden instalar desde una carpeta de plantillas. Especifique la ruta de acceso de la carpeta .template.config. La ruta de acceso al directorio de plantillas no es necesario que sea absoluto.

```dotnetcli
dotnet new --install <FILE_SYSTEM_DIRECTORY>
```

## Obtención de una lista de los paquetes de plantillas instalados

El comando de desinstalación, sin ningún otro parámetro, enumerará todos los paquetes de plantillas instalados y las plantillas incluidas.

```dotnetcli
dotnet new --uninstall
```

Ese comando devuelve algo similar a la salida siguiente:

```console
Template Instantiation Commands for .NET CLI

Currently installed items:
  Microsoft.DotNet.Common.ItemTemplates
    Templates:
      global.json file (globaljson)
      NuGet Config (nugetconfig)
      Solution File (sln)
      Dotnet local tool manifest file (tool-manifest)
      Web Config (webconfig)
  Microsoft.DotNet.Common.ProjectTemplates.3.0
    Templates:
      Class library (classlib) C#
      Class library (classlib) F#
      Class library (classlib) VB
      Console Application (console) C#
      Console Application (console) F#
      Console Application (console) VB
...
```
El primer nivel de los elementos situados después de `Currently installed items:` son los identificadores que se usan en la desinstalación de un paquete de plantillas. Y en el ejemplo anterior, se enumeran `Microsoft.DotNet.Common.ItemTemplates` y `Microsoft.DotNet.Common.ProjectTemplates.3.0`. Si el paquete de plantillas se instaló mediante una ruta de acceso del sistema de archivos, este identificador será la ruta de acceso de la carpeta *.template.config*

## Desinstalación de un paquete de plantillas

Use el comando [dotnet new -u|--uninstall](https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-new-uninstall) para desinstalar un paquete de plantillas.

Si el paquete se instaló mediante la especificación de una ruta de acceso a la carpeta *.template.config*, use esa ruta de acceso para desinstalarlo. Puede ver la ruta de acceso absoluta del paquete de plantillas en la salida proporcionada por el comando `dotnet new --uninstall` . Para obtener más información, consulte la sección [Obtención de una lista de los paquetes de plantillas instalados](#obtención-de-una-lista-de-los-paquetes-de-plantillas-instalados) anterior.

```dotnetcli
dotnet new --uninstall <FILE_SYSTEM_DIRECTORY>
```

## Crear un proyecto con una plantilla personalizada

Después de que se instale una plantilla, use la plantilla ejecutando el comando `dotnet new <TEMPLATE>` como lo haría con cualquier otra plantilla preinstalada. También puede especificar [options](https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-new#options)  en el comando `dotnet new`, incluidas las opciones específicas de plantilla que ha definido en la configuración de la plantilla. Proporcione el nombre breve de la plantilla directamente al comando:

```dotnetcli
dotnet new <TEMPLATE>
```