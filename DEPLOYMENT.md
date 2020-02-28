# Deployment Guide
1. Run `new-server-setup.sh` on the new VM to install necessary packages
2. (Optional) Log into the build server (vcm-12796.vm.duke.edu) and use `ssh-copy-id` to copy the certificates necessary for password-less SSH connections from the build server to the new VM. This is necessary for automated builds to run.
3. Add a new deployment job to the build script, located in `.gitlab-ci.yml`. An example is shown below.

```
deploy_staging:
 - stage: deploy
 - script
    - docker login -u $CI_REGISTRY_USER -p $CI_REGISTRY_PASSWORD $CI_REGISTRY
    - docker-compose -f ./src/docker-compose.yml build
    - docker-compose -f ./src/docker-compose.yml push
    - docker-compose -H "ssh://vcm@vcm-12801.vm.duke.edu" -f ./src/docker-compose.yml pull
    - docker-compose -H "ssh://vcm@vcm-12801.vm.duke.edu" -f ./src/docker-compose.yml up
nvironment:
   - name: staging
   - url: http://vcm-12801.vm.duke.edu/
only:
   - dev
```

This snippet above is all that is needed to begin automated deployment to the new environment. In this example, our deployment job will be run for all new refs pushed to the `dev` branch. The script is simple. It accesses a docker image registry hosted by gitlab, builds all containers, pushes all images to the registry, and then connects to the new VM via SSH and pulls + runs the container stack.
