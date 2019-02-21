# MetaSaver.Framework.Data

Part of the MetaSaver Framework suite which allows simple connection to a database, 
implemeting automatic retry logic using Polly

## !!! WARNING !!!

![WARNING](http://www.animatedimages.org/data/media/703/animated-warning-image-0011.gif)

MetaSaver.Framework is currently in pre-release mode and should not be used in production systems

## Goals

MetaSaver.Framework has several top level goals:

1. Provide a simple config based way to do .net Core startup
1. No coding required except to choose the Startup options you require
1. MetaSaver.Framework.Configuration provides the extension method to setup Appsetting
1. It also provides a strongly typed AppSettings class which can be used in Dependency Injection

Add the following example node to your AppSettings.json file and each subnode part of the MetaSaver
framework that you wish to use.

```javascrip
{
    "AppSettings": {
...
        "Connections": {
            "Database": "Your connection string"
        },
...
    },
}
```

### Connections
List of connection strings to use for the MetaSaver framework

#### Database
SQL Server connection string to be used with MetaSaver.Framework.Data.ContextManager
