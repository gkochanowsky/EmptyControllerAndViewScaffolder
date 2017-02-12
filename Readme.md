# Custom code generator that works with dotnet aspnet-codegenerator

This code generator generates an empty controller and an empty view in a single operation.

### Nuget URL
[EmptyControllerAndViewScaffolder](https://www.nuget.org/packages/EmptyControllerAndViewScaffolder/)

### Steps to build. 
1. Run `dotnet restore` in the `src\EmptyControllerAndViewScaffolder\` directory.
2. Run `dotnet pack -o ..\artifacts` in the `src\EmptyControllerAndViewScaffolder\` directory.
3. Run `dotnet restore` in the `SampleTestApp` directory.
4. Run `dotnet aspnet-codegenerator -p . controllerandview --model SampleModel` in the `SampleTestApp` directory.


### Usage

Install the package `EmptyControllerAndViewScaffolder` to the project.
```xml
<PackageReference Include="EmptyControllerAndViewScaffolder" Version="1.0.0-*" />
```

```
dotnet aspnet-codegenerator -p SampleTestApp.csproj controllerandview -h

Finding the generator 'controllerandview'...
Running the generator 'controllerandview'...


Usage:  controllerandview [options]

Options:
  --help|-h|-?                       Show help information
  --controllerName|-cName            Name for the controller
  --controllerNamespace|-cNamespace  Namespace for the controller
  --controllerRelativePath|-cPath    Relative path for the controller
  --viewName|-vName                  Name for the View
  --viewRelativePath|-vPath          Relative Path for the view.
  --layout|-l                        Custom layout page to use.
  --partialView|-partial             Specifies if the view should be created as a partial view.
  --UseDefaultLayout|-udl            Specifies whether to use Default layout page.
  --force|-f                         Specifies whether to force overwrite existing files.
```

```
dotnet aspnet-codegenerator -p SampleTestApp.csproj controllerandview -cName EmptyController -cNamespace SampleTestApp.Controllers -cPath Controllers -vName Index.cshtml -vPath Views\Empty\ -udl -partial                                                                                           

Finding the generator 'controllerandview'...                                                                                                        
Running the generator 'controllerandview'...                                                                                                        
Added Controller : \Controllers\EmptyController.cs                                                                                                  
Added View : \Views\Empty\Index.cshtml.cshtml                                                                                                       
```