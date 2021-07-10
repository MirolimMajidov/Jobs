echo "Port forwarding k8s job service..."
kubectl port-forward deployment/job-api-deployment 8002:80