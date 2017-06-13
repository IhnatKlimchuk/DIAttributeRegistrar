# DIAttributeRegistrar

Provides an easy way of attribute based dependency injection registration in asp.net core applications.

## Why do you need this?

ASP.NET Core includes a simple built-in container (represented by the IServiceProvider interface). You can configure the built-in container's services in the ConfigureServices method in your application's Startup class. Like this:

```cs
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddTransient<IEmailSender, EmailSender>();
    services.AddTransient<ISmsSender, SmsSender>();
    //...
}
```

When your project is big enough you can face with spaghetti code during services configuration. Problem even get worse when you need to mock dependencies per certain environment. Here is a problem example:

```cs
public void ConfigureServices(IServiceCollection services)
{
    //...
    if (HostingEnvironment.IsDevelopment())
    {
        services.AddTransient<IEmailSender, RealEmailSender>();
        //...
    }
    else
    {
        services.AddTransient<IEmailSender, FakeEmailSender>();
        //...
    }
    services.AddScoped<IUserManager, UserManager>();
    //Many lines of similar code...
    services.AddScoped<IDeviceManager, DeviceManager>();
}
```

With attribute based registration you can solve this problem and **improve code cohesion**! As result - one line of code during configuration:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddAttributeRegistration(HostingEnvironment.EnvironmentName);
}
```

## Samples

You can use lightweight registration way:

```cs
[Register]
public class UserManager : IUserManager, ISignInManager
{
    //manager implementation
}
```
Or you can define all option for registration and multiple attributes: 

```cs
[Register(typeof(IUserManager), ServiceLifetime.Scoped, EnvironmentNames.Production, EnvironmentNames.Staging)]
[Register(typeof(ISignInManager), ServiceLifetime.Scoped, EnvironmentNames.Production)]
public class UserManager : IUserManager, ISignInManager
{
    //manager implementation
}
```
## Documentation

Please pay special attention to tag functionality. Use this table for better understanding:

Tag(s) to register\Dependecy tag(s)  | [] | ["dev"] | ["prod"] | ["dev", "prod"]
------------------------------------ | -- | ------- | -------- | ---------------
[]                                   | ✓ | ✗ | ✗ | ✗ |
["dev"]                              | ✓ | ✓ | ✗ | ✓ |
["prod"]                             | ✓ | ✗ | ✓ | ✓ |
["dev", "prod"]                      | ✓ | ✗ | ✗ | ✓ |

## Where can I get it?

First, install [NuGet](https://docs.nuget.org/docs/start-here/installing-nuget). Then, install [DIAttributeRegistrar](https://www.nuget.org/packages/DIAttributeRegistrar) from the package manager console:

`PM> Install-Package DIAttributeRegistrar`

[![NuGet](https://img.shields.io/nuget/v/DIAttributeRegistrar.svg?style=flat-square)]()

## License, etc.

**DIAttributeRegistrar** is Copyright © 2017 Ihnat Klimchuk and other contributors under the MIT license.
