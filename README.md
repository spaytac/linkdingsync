# Linkding Sync
LinkdingSync is a collection of tools that make life with [Linkding](https://github.com/sissbruecker/linkding) easier. 

One of the workers is for syncing to [Wallabag](https://wallabag.org/en).

## Getting Started
It is recommended to use the Docker images. Otherwise, a .NET 6 environment is required to customize and build the code.

## Environment Variables
For the containers to work, the environment variables must be passed. This can be done either directly via the Docker run **-e** switch, via the **environment** settings in a Docker compose definition, or via an environment variable file.

### WallabagSync
Environment variables for the wallabag worker.

| Variable               | Value     | Description                                                                                                  | Attention                                                                                                                                                     |
|------------------------|-----------|--------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Worker__Intervall      | int (>=0) | This value sets the execution schedule in minutes. 1 = every minute, 10 = every 10 minutes (default value 0) | 0 = runs only one time. The container will be stopped after the execution. This method is the preferred way to run the container with a scheduler (e.g. cron) |
| Worker__SyncTag        | text      | The linkding tag to create the bookmarks in Wallabag. (default value 'readlater')                            |                                                                                                                                                               |
| Linkding__Url          | text      | URL to the linkding instance                                                                                 |                                                                                                                                                               |
| Linkding__Key          | text      | The linkding application key                                                                                 | [Instructions](https://github.com/sissbruecker/linkding/blob/master/docs/API.md)                                                                              |
| Wallabag__Url          | text      | URL to the Wallabag instance                                                                                 |                                                                                                                                                               |
| Wallabag__Username     | text      | Wallabag User Name                                                                                           |                                                                                                                                                               |
| Wallabag__Password     | text      | Wallabag User Password                                                                                       |                                                                                                                                                               |
| Wallabag__ClientId     | text      | Wallabag Client Id                                                                                           |                                                                                                                                                               |
| Wallabag__ClientSecret | text      | Wallabag Client Secret                                                                                       |                                                                                                                                                               |

### LinkdingUpdater
Environment variables for the linkding worker.

| Variable               | Value     | Description                                                                                | Attention                                                                                                                                                     |
|------------------------|-----------|--------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Worker__Intervall      | int (>=0) | This value sets the execution schedule in minutes. 1 = every minute, 10 = every 10 minutes | 0 = runs only one time. The container will be stopped after the execution. This method is the preferred way to run the container with a scheduler (e.g. cron) |
| Worker__SyncTag        | text      | The linkding tag to create the bookmarks in Wallabag.                                      |                                                                                                                                                               |
| Linkding__Url          | text      | URL to the linkding instance                                                               |                                                                                                                                                               |
| Linkding__Key          | text      | The linkding application key                                                               | [Instructions](https://github.com/sissbruecker/linkding/blob/master/docs/API.md)                                                                              |

## Configuration
The following explains the configuration options.
### WallabagSync
The configuration is optional. In the configuration (**YAML File**) rules can be defined in regex to exclude certain domains from sync.

Exampel:
````yaml
excludedDomains:
- name: youtube
  pattern: 'https://[[a-zA-Z0-9]+\.]?(youtube)\.com(?:/.*)?'
- name: ebay
  pattern: 'https://[[a-zA-Z0-9]+\.]?(ebay)\.(com|de|fr)(?:/.*)?'
- name: amazon
  pattern: 'https://[[a-zA-Z0-9]+\.]?(amazon)\.(com|de|fr)(?:/.*)?'
````
With this configuration every matching bookmark from linkding will be excluded from the sync.

### LinkdingUpdater
The configuration is optional. In the configuration (**YAML File**) rules can be defined in regex to assign tags dynamically. Additionally tags can be defined to domains.

If operated without a configuration file, only the year of the tag is added (currently).

Example:
````yaml
urlTagMapping:
  - name: microsoft_azure
    url: https://github.com/azure
  - name: microsoft_azuread
    url: https://github.com/AzureAD
  - name: microsoft_dotnet
    url: https://github.com/dotnet-architecture
    
taggingRule:
  - name: reddit
    pattern: https://(?:www\.)?(reddit)\.com(?:/r/)?([ a-zA-Z0-9\-\+_ ]+)?(?:/.*)?
    replace: $1,$2
  - name: microsoft
    pattern: https://([ a-zA-Z0-9 ]+)?[ \. ]?(microsoft)\.com(?:/.*)?
    replace: $1,$2
  - name: microsoft_docs
    pattern: 'https://(?:docs)\.(?:microsoft)\.com[ / ]?(?: [ a-zA-Z0-9\-\+_ ]+)(?:/)?([ a-zA-Z0-9\-\+_ ]+)?(?:/)?([ a-zA-Z0-9\-\+_ ]+)?(?:/.*)?'
    replace: $1,$2
  - name: youtube
    pattern: https://[ [ a-zA-Z0-9 ]+\. ]?(youtube)\.com(?:/.*)?
    replace: $1
  - name: ebay
    pattern: https://[ [ a-zA-Z0-9 ]+\. ]?(ebay)\.(com|de|fr)(?:/.*)?
    replace: $1
  - name: amazon
    pattern: https://[ [ a-zA-Z0-9 ]+\. ]?(amazon)\.(com|de|fr)(?:/.*)?
    replace: $1
  - name: docker
    pattern: https://([ a-zA-Z0-9 ]+)?[ \. ]?(docker)\.com(?:/.*)?
    replace: $1,$2
  - name: xbox
    pattern: https://[ [ a-zA-Z0-9 ]+\. ]?(xbox)\.com(?:/.*)?
    replace: $1
  - name: github
    pattern: https://([ a-zA-Z0-9 ]+)?[ \. ]?(github)\.com[ / ]?([ a-zA-Z0-9\-\+_ ]+)(?:/)?([ a-zA-Z0-9\-\+_ ]+)?(?:/.*)?
    replace: $1,$2,$3,$4
  - name: github.io
    pattern: https://([ a-zA-Z0-9 ]+)\.(github)\.io[ / ]?([ a-zA-Z0-9\-\+_ ]+)(?:/)?([ a-zA-Z0-9\-\+_ ]+)?(?:/.*)?
    replace: $1,$2,$3
````

## Docker Run
```
docker run --rm -it --env-file .env -v <path>/config.yml:/app/data/config.yml linkdingsync/wallabag:latest
```

You can also 

## Docker Compose
You can find [examples](./examples/) in the examples folder..

- [Wallabag Example](./examples/wallabag/)
- [LinkdingUpdater Example](./examples/linkding/)


## Build Docker Image

### LinkdingUpdater
```
docker build -t linkdingsync/linkdingupdater:latest -f .\Dockerfile_Linkding .
```

### WallabagSync
```
docker build -t linkdingsync/wallabag:latest -f .\Dockerfile_Wallabag . 
```