# Development Guide
The HypoSoft program allows users to view racks, assets, and models in a selected datacenter as well as all the datacenters available. Our software follows the Model-View-Controller design pattern, using Vue as our front-end JavaScript framework, ASP.net (C#) as our back-end framework, and a MS SQL database. The general project structure has 4 main directories within the src folder: Web.Api, Web.Api.Common, Web.Api.Core, and Web.Api.Infrastructure. The Controllers within the Web.Api directory serve as a layer of API calls that exist between the model and view; they are written in ASP.net and our VueClient uses the library Axios to handle HTTP communication by making ajax requests. For more information about the controller classes and the use of the Axios library, refer to the Front-end Development section.

This program uses Microsoft’s Entity Framework, which is an object-relational mapper that allows access to data using domain-specific objects. This eliminates the need for a lot of the data-access code. This allows Entity objects to be defined (in this case for Models, Instances, Racks, and Users).

To configure a development/build environment:
1. use docker-compose (multi-container orchestration tool) to build images required to run the application (db and web app, atm)
2. push each new image to a central docker container registry, hosted by gitlab
3. using docker-compose via password-less SSH (to the staging/production environment), pull the latest images from the registry, stop the currently running containers, and start the new containers

All of this is continuous integration (CI) and continuous delivery (CD), and runs on a dedicated build server that is provided by a virtual container (vcm).

## Front-end Development
The front-end code is all contained within the Web.Api folder, which contains the following directories: Authentication, Configuration, Controllers, Dtos, ExceptionHandling, Helpers, Mappers, Resources and VueClient. Within the Web.Api directory, we have also included files essential to the structure of our web app; appsettings.json, which stores our connection strings (connecting to the database) and file paths, our Dockerfile, which dictates the behavior of our CI/CD deployment pipeline, Program.cs, which also connects to the database and takes care of initializing logging, and Startup.cs.

The VueClient folder contains a set of components (in the components folder), which dictate all of the frontend structures used throughout the project. These components are integrated into the UI using router links, where each tab corresponds to its own address. The code dictating what these links are and their names to be referred to throughout the Vue project is in router/index.js. A lot of the main sections of the UI (with CRUD actions) follow this basic structure (example based off of models): 

1. `Dashboard.vue` - This is the file that dictates all of the tabs that will appear in the user interface. One of these tabs is models: 
2. `Model.vue` - This is the main page that shows up for this tab in the user interface. This will generally contain a table showing all of the related items and their fields, as well as entries in each row (for admins some additional columns with actions like edit and delete will be visible).
3. `ModelEdit.vue` - This is the page that appears when an admin clicks on the edit material design icon in the Actions column of the Models table. This will provide all of the fields necessary to change or add a new Model. This component is used for both editing and creating a model, the difference being that if a Model is being created the fields will appear empty, while if a Model is being edited, the fields will be prefilled with the stored values for that Model.
4. `ModelDetails.vue` - This is the page that appears when any user clicks on a row in the table to get more information. This view contains all of the fields for a Model, as well as a link to the detail view for the Instances associated with that model.

To add a new component, simply add it to the components in the VueClient folder.

## Back-end Development
From the controller there are two main layers to the backend. One is the service layer, which acts as interference between the controller layer and the database, which is directly accessed by the last layer, the repository.

The `Web.Api.Core` namespace holds services, the layer between the controller and the repository. This layer is responsible for enforcing business rules and mediating interactions between the data access layer and the API layer. All communication between layers takes place via DTOs, or data transfer objects. Pre-compiled mapping plans are applied to the domain model of one layer in order to map it to the model expected and understood by another layer, allowing for the data layer, business layer, and API layer to grow independently of one another. An entity has a direct mapping from the database object to a c# object that developers can use to extract information. This single c# object has multiple mappings to more specialized DTOs; for example, a dto for the creation of the object, a dto for the reading of an object, and a dto for the update of an object. These specialized Dtos are then passed from the controller to the client side, and vice versa. 

The data access layer is located in the `Web.Api.Infrastructure` namespace. Here, entities that represent our versioned database schema, as well as the migrations that map these objects to the database are managed. `ApplicationDbContext` builds the database using these entities, as well as sets some specific rules. For example, by specifying that the vendor and number of a model isUnique(), the database rejects any post request if there already exists an entity with the same fields. The repositories also exist here. This is the layer that directly interferes with the database by saving objects or getting them. 

## Testing Environment
The tests are all contained within the “Test” folder under the solution App. These tests are for backend testing only, and are automatically run on the git pipeline when the developer pushes to git. Developers, and other members of the team, are notified of this push via automated slack messaging, and can see if the pipeline job has failed on gitlab pipelines if any of these tests fail.

The goal of these tests is to make sure that the new code does not break the core of the backend. The tests are broken up into several distinct folders:
Mappers: Since we are using several layers of mapping from the backend, these tests are extremely important. The flow of information between the database and front end goes through several different mappings, between an entity to a dto to a more specialized dto object, which are passed between the frontend and backend. Because it is so crucial that each mapping contains the correct information, all of the mapping tests are dedicated to make sure that the information flow is not violated. 
Validator tests: these tests are for validation tests for the backend. These test for rules for certain objects. For example, that all model vendor and number combinations are different, and assets don’t overlap in a rack.
The rest of the tests are more for the overall information passing in the backend. For example, they test from the controller layer all the way to the database, to generally confirm that a certain input from the client side will result in an appropriate response.

## Git flow 
There is a sprint branch that is deployed on a server, and developers send merge requests to this branch throughout the entire sprint. The dev branch is also deployed on a server, and after every sprint the sprint branch is merged into dev. The master branch is the production server and is protected to prevent accidental pushes. 

Within a sprint, the developers follow the rules of gitflow when they create another branch. When working on a new feature, the branch should be preceded by a ‘feature/’ followed by an apt label to the branch, to notify others what specifically this branch is for. When a branch is created to fix a bug, the branch name should be preceded by a ‘hotfix/’ followed by a label which describes the nature of the bug. 


