# HypoSoft

## Deployment Guide
-----
stages:
  - build
  - deploy

build_and_test:
  - stage: build
  - script:
    ```cd src```
    ```dotnet restore App.sln```
    ```dotnet build App.sln```
    ```dotnet test App.sln```
    ```cd Web.Api/VueClient```
    ```npm run test:unit ```

deploy_staging:
 - stage: deploy
 - script
    ```docker login -u $CI_REGISTRY_USER -p $CI_REGISTRY_PASSWORD $CI_REGISTRY```
    ```docker-compose -f ./src/docker-compose.yml build```
    ```docker-compose -f ./src/docker-compose.yml push```
    ```docker-compose -H "ssh://vcm@vcm-12801.vm.duke.edu" -f ./src/docker-compose.yml pull```
    ```docker-compose -H "ssh://vcm@vcm-12801.vm.duke.edu" -f ./src/docker-compose.yml up```

environment:
   - name: staging
   - url: http://vcm-12801.vm.duke.edu/

only:
   - dev

## Development Guide
-----
The HypoSoft program allows users to view racks, instances, and models in a datacenter full of racks of servers. The software follows an MVC design pattern, with the view written in Vue (JavaScript framework) and the model in ASP.net (C#), with a MS SQL database. The controllers are a layer of API calls that exist between the model and view, and they are written in ASP.net with a dependence on Axios, a library which deal with http communication by making ajax requests.

This program uses Microsoft’s Entity Framework, which is an object-relational mapper that allows access to data using domain-specific objects. This eliminates the need for a lot of the data-access code. This allows Entity objects to be defined (in this case for Models, Instances, Racks, and Users).

To configure a development/build environment:
1.	use docker-compose (multi-container orchestration tool) to build images required to run the application (db and web app, atm)
2.	push each new image to a central docker container registry, hosted by gitlab
3.	using docker-compose via password-less SSH (to the staging/production environment), pull the latest images from the registry, stop the currently running containers, and start the new containers

All of this is continuous integration (CI) and continuous delivery (CD), and runs on a dedicated build server that is provided by a virtual container (vcm).
