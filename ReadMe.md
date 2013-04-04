# About


Lokad Contracts DSL is an optional console utility that you can run in the background. It 
tracks changes to files with a special compact syntax and a .ddd file extension.  When the .ddd file changes, it updates the corresponding C# .cs file with message contract definitions. 

Changes are immediate upon saving the .ddd file (and ReSharper immediately picks them up). This is an improved 
version of the Lokad Code DSL, it supports identities and can auto-generate interfaces for 
aggregates and aggregate state classes.

**DSL syntax entered as**:

```csharp
AddSecurityPassword?(SecurityId id, string displayName, string login, string password)
```    
**Becomes this C# code**:
```csharp
[DataContract(Namespace = "Sample")]
public partial class AddSecurityPassword : ICommand<SecurityId>
{
    [DataMember(Order = 1)] public SecurityId Id { get; private set; }
    [DataMember(Order = 2)] public string DisplayName { get; private set; }
    [DataMember(Order = 3)] public string Login { get; private set; }
    [DataMember(Order = 4)] public string Password { get; private set; }
 
    AddSecurityPassword () {}
    public AddSecurityPassword (SecurityId id, string displayName, string login, string password)
    {
        Id = id;
        DisplayName = displayName;
        Login = login;
        Password = password;
    }
}
```    

Lokad Code DSL is used by [Lokad.CQRS](http://lokad.github.com/lokad-cqrs/) (it was originally part of it) 
and is explained in greater detail on the [Being The Worst Podcast](http://beingtheworst.com/) - [Episode 12 - Now Serving DSL.](http://beingtheworst.com/2012/episode-12-now-serving-dsl)

You can try this out by starting the `Lokad.Dsl.Sample` project, and then change the `Sample\Contracts.ddd` file and save it.
(view [Contracts.ddd source] (http://github.com/Lokad/lokad-codedsl/blob/master/Sample/Contracts.ddd)). 
The Code DSL tool will run in a background console/tray icon and will regenerate the corresponding C# contracts file as you change and save the .ddd file that contains the DSL (view [Contracts.cs source](http://github.com/Lokad/lokad-codedsl/blob/master/Sample/Contracts.cs)).

The current DSL code generates contract classes that are compatible with DataContract, 
ServiceStack.JSON and ProtoBuf.

You can download the binary from [github downloads](https://github.com/Lokad/lokad-codedsl/downloads). On occasion, you can get newer stable versions of this tool by downloading the latest source code from GitHub and building it.


**Lokad Code DSL** ([homepage](http://lokad.github.com/lokad-codedsl/)) is shared as an open 
source project by [Lokad](http://www.lokad.com) with hopes that it will benefit the community. 


Syntax Definitions
-----------------
### Namespaces

Define the C# namespace that our messages will be in  

```csharp
namespace NameSpace
```

**Result:**

```csharp
namespace NameSpace  
{  
...  
}
```

### Data Contract Namespace

Define the namespace that the Data Contract will use for the message

```csharp
extern "Lokad"
```

**Result:**

```csharp
[DataContract(Namespace = "Lokad")]
```

### Simple Contract Definitions

```csharp
Universe(UniverseId Id, string name)
```

**Result:**

```csharp
[DataContract(Namespace = "Lokad")]
public partial class Universe
{
    [DataMember(Order = 1)] public UniverseId Id { get; private set; }
    [DataMember(Order = 2)] public string Name { get; private set; }

    Universe () {}
    public Universe (UniverseId id, string name)
    {
        Id = id;
        Name = name;
    }
}
```

### Interface Shortcuts

To generate a contract class that implements an interface, you must define the name of the interface with the ! = shortcut first.  The definition of the interface that uses the interface identifier that you specify after the = must already exist and be contained in a C# file.  For example, the IIdentity interface is defined in Interfaces.cs in the sample project. 
    
```csharp
if ! = IIdentity
```
Once you have associated ! with an interface, define a class that implements it like this:

```csharp
UniverseId!(long id)
```

**Result:**

```csharp
[DataContract(Namespace = "Lokad")]
public partial class UniverseId : IIdentity
{
    [DataMember(Order = 1)] public long Id { get; private set; }
    
    UniverseId () {}
    public UniverseId (long id)
    {
        Id = id;
    }
}
```

### Method Argument Constants

Method argument constants allow us to define a constant to replace a method argument definition. For 
example, now we can use the constant term `dateUtc` instead of the full definition with the argument type and name.

```csharp
const dateUtc = DateTime dateUtc
```

###

Application service & state
---------------------------
The definition of an application service must begin with the "interface" keyword.

The ? shortcut is used with command messages to define the interface that the command message implements.

The ! shortcut is used with event messages to define the interface that the event message implements. 

```csharp
interface Universe(UniverseId Id)
{
    // define shortcut for commands
    if ? = IUniverseCommand
    // define shortcut for events
    if ! = IUniverseEvent<UniverseId>

    CreateUniverse?(name)
        // override ToString() for command
        explicit "Create universe - {name}"
        UniverseCreated!(name)
        // override ToString() for event
        explicit "Universe {name} created"
}
```

**Result:**

```csharp
public interface IUniverseApplicationService
{
    void When(CreateUniverse c);
}

public interface IUniverseState
{
    void When(UniverseCreated e);
}
```

Command and corresponding event

```csharp
[DataContract(Namespace = "Lokad")]
public partial class CreateUniverse : IUniverseCommand
{
    [DataMember(Order = 1)] public UniverseId Id { get; private set; }
    [DataMember(Order = 2)] public string Name { get; private set; }
    
    CreateUniverse () {}
    public CreateUniverse (UniverseId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public override string ToString()
    {
        return string.Format(@"Create universe - {0}", Name);
    }
}

[DataContract(Namespace = "Lokad")]
public partial class UniverseCreated : IUniverseEvent<UniverseId>
{
    [DataMember(Order = 1)] public UniverseId Id { get; private set; }
    [DataMember(Order = 2)] public string Name { get; private set; }
    
    UniverseCreated () {}
    public UniverseCreated (UniverseId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public override string ToString()
    {
        return string.Format(@"Universe {0} created", Name);
    }
}
```

Syntax Highlighting
-------------------

The syntax used in the DSL tool is derived from keywords in the C++ and C# programming languages. This means that
any text editor that understands these widely used programming languages can provide nice syntax highlighting (if you use the language-specific color settings provided for C++ or C#).

Here's how the DSL source code might look with syntax highlighting supported by the editor:

<table>
<thead>
<tr>
<th>Sublime Text 2</th>
<th>Visual Studio 2010</th>
</tr>
</thead>
<tbody>
<tr>
<td><img src="https://github.com/Lokad/lokad-codedsl/raw/master/Docs/sublimeText2.PNG" />
<td><img src="https://github.com/Lokad/lokad-codedsl/raw/master/Docs/vs2010_csharp.PNG" />
</tr>
</tbody>
</table>  

**Visual Studio 2010/2012 DSL Syntax Highlighting Settings**

In Visual Studio, under the Tools-->Options menu:

1. - add ddd as the file Extension
1. - Select Microsoft Visual C# as the Editor
1. - Click Add and OK

![Visual Studio settings] (https://github.com/Lokad/lokad-codedsl/raw/master/Docs/vs2010_settings.PNG)


Related articles
-----------
* **Tutorial**: [Extending Lokad DSL Tool](http://zbz5.net/extending-lokad-dsl-tool) by [Vidar Løvbrekke Sømme](https://twitter.com/vidarls)
* [Improved DSL Syntax for DDD and Event Sourcing] (http://abdullin.com/journal/2012/7/25/improved-dsl-syntax-for-ddd-and-event-sourcing.html)

Feedback
--------

Please, feel free to drop feedback and question into the [Lokad Community Google group](https://groups.google.com/forum/#!forum/lokad).
