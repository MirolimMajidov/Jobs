echo "Port forwarding k8s identity service..."
kubectl port-forward deployment/identity-api-deployment 8001:80