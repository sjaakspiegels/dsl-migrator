# About


Lokad Contracts DSL is an optional console utility that you can run in the background. It 
tracks changes to files with special compact syntax and updates CS file with message 
contract definitions. 

Changes are immediate upon saving file (and ReSharper immediately picks them). This is an improved 
version of Lokad Code DSL, it supports identities and can auto-generate interfaces for 
aggregates and aggregate state classes.

**Was**:

```csharp
AddSecurityPassword?(SecurityId id, string displayName, string login, string password)
```    
**Becomes**:
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

Lokad Code DSL is used by [Lokad.CQRS](http://lokad.github.com/lokad-cqrs/) (was originally part of it) 
and is explained in greater detail in [BeingTheWorst Podcast](http://beingtheworst.com/) - Episode 12.

You can try this out by starting `Sample` project and then changing `Sample\Contracts.ddd` 
(view [Contracts.ddd source] (http://github.com/Lokad/lokad-codedsl/blob/master/Sample/Contracts.ddd)). 
Code DSL tool will be regenerating corresponding contracts file as you change and 
save (view [Contracts.cs source](http://github.com/Lokad/lokad-codedsl/blob/master/Sample/Contracts.cs)).

Current DSL code generates contracts classes that are compatible with DataContracts, 
ServiceStack.JSON and ProtoBuf.

You can get download binary from [github downloads](https://github.com/Lokad/lokad-codedsl/downloads). Sometimes
even later stable version would be available by getting latest source code and building.


**Lokad Code DSL** ([homepage](http://lokad.github.com/lokad-codedsl/)) is shared as open 
source project by [Lokad](http://www.lokad.com) in hopes that it would benefit the community. 


Syntax Definitions
-----------------
### Namespaces

Add namespace for our messages  

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

### Data contract namespace

```csharp
extern "Lokad"
```

**Result:**

```csharp
[DataContract(Namespace = "Lokad")]
```

### Simple Contract definitions

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

In order to use interface in contract classes, need to create interface shortcut first, definition
of interface IIdentity must be contained in C# file
    
```csharp
if ! = IIdentity
```
For the next step define simple class with one property

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

Method arguments constants allow us to define constant to replace method argument definition. For 
example, now we can use term `dateUtc` instead full definition with argument type and name.

```csharp
const dateUtc = DateTime dateUtc
```

###

Application service & state
---------------------------
Definition of application service must begining with interface key.

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

Syntax Highlights
-----------------

Syntax used in the DSL tool is derived from keywords in C++ and C#. This means, that
any text editor could provide nice highlighting, if you use language color settings
from C++ or C#.

Here's how DSL source code might look like:

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

**Visual Studio 2010 settings**

Here are the settings to set up in Visual Studio:

![Visual Studio settings] (https://github.com/Lokad/lokad-codedsl/raw/master/Docs/vs2010_settings.PNG)


Related articles
-----------
* **Tutorial**: [Extending Lokad DSL Tool](http://zbz5.net/extending-lokad-dsl-tool) by [Vidar Løvbrekke Sømme](https://twitter.com/vidarls)
* [Improved DSL Syntax for DDD and Event Sourcing] (http://abdullin.com/journal/2012/7/25/improved-dsl-syntax-for-ddd-and-event-sourcing.html)

Feedback
--------

Please, feel free to drop feedback in the [Lokad Community](https://groups.google.com/forum/#!forum/lokad).
