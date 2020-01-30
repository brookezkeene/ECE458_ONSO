docker info
docker build -t $CI_PROJECT_PATH./src/Dockerfile
docker tag $CI_PROJECT_PATH $CI_REGISTRY_IMAGE:latest
docker push $CI_REGISTRY_IMAGE