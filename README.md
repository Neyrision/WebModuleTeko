# Project Tela | WebModuleTeko

This is a small Web Project setup with fly.io, keycloak, postgres, c# asp net core and angular. Purpose of this project is to setup and deploy a small website with login functionality for a Webmodule.
In this readme I will shortly go over:

- Building
- Deployment

## Folder Structure

- root
  - fly
    - webmodule -> fly config for backend and frontend
    - keycloak  -> dockerfile and fly config for keycloak
  - local-testing -> docker-compose to test deployment locally
  - WebModuleTeko -> c# Backend and Angular Frontend (AppClient/)

## Local development

For local development it is best adviced to use VS22 for the c# backend and use the 'WebModuleTeko' launch profile.
To run it following secrets should be defined alongside appsettings.

```
{
  "ConnectionStrings:WmtContext": "Host=localhost;Database=wmt;Username=<user>;Password=<password>",
  "ApiConfiguration:ClientSecret": "<keycloak client secret>"
}
```

To test all functions locally it is best to use the docker-compose file found inside under the 'local-testing' folder and configure the keycloak instance manually or use the 'keycloak/realm-export.json' to import the web-module realm.

For the frontend, manually start it using `ng s` while inside the 'WebModuleTeko/ClientApp' folder.

## Swagger

When building the c# backend it will automatically generate a 'swagger.json' and a 'ClientApp/src/api/api-services.ts' service which can be used inside the angular app as is. The configuration for generating the Client and 'swagger.json' can be found inside the file 'nswag.json'.

## Deployment

Deployment of the app is done using the two Github actions 'Tela Docker Image' and 'Tela Fly Deploy' by triggering them manually. (They dont trigger on a PR or anything else)

- Tela Docker Image -> Builds and pushes the 'thecodewizard27/tela' docker image.
- Tela Fly Deploy -> Calls fly deploy using the fly/webmodule/fly.toml config.

That these deployment actions work the following secrets need to be added to the Github Repository.

- FLY_TOKEN -> Access token for the fly app web-module
- DOCKER_USERNAME -> username for the docker registry account
- DOCKER_PASSWORD -> password or access token for the docker registry account

There is no Github action but using the 'fly/keycloak/fly.toml' you can deploy and configure the keycloak fly app. Of course Keycloak needs to further be configured using the keycloak Webui.

For the Fly WebModule app following Env Secrets should be configured.

- ApiConfiguration__ClientSecret -> Keycloak client secret
- ConnectionStrings__WmtContext -> npgsql connectionstring
