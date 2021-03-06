To launch this app
 - Run `dotnet run` or `dotnet watch run` inside ./API directory

To display queries made by Entity set `Logging.LogLevel.Microsoft` to `Information` (default is `Warning`) in [appsettings.json](./API/appsettings.json) or [appsettings.Development.json](./API/appsettings.Development.json)

Used Entity Framweork version: 5.0.3
 - To create a new migation run `dotnet ef migrations add [MIGRATION_NAME] -p ./Persistence -s ./API`
 - Database commands: `dotnet ef database -h`
 - Updated to version 5.0.5 with command: `dotnet tool update -g dotnet-ef`

Steps to deploy to Heroku
 - Install Heroku CLI
 - Login to Heroku in terminal: `heroku login`
 - Add repository to Heroku: `heroku git:remote -a reactivities-led-zeff`
 - Set build pack: `heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack`
